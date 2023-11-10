using Microsoft.AspNetCore.Mvc;
using Omnitool.Models;
using System.Diagnostics;
using System.Net.Http;
using Api;
using static Model;
using Omnitool.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Omnitool.Controllers
{
    public class FormModel
    {
        public string Query { get; set; }
    }

    public class TranslateFormModel
    {
        public string Message{get;set;}
        public string FromLanguage {get; set;}
        public string ToLanguage {get; set;}
    }

    public class AddTaskFormModel 
    {
        public string taskMessage{get;set;}
        public string taskDate{get;set;}
    }


    public class PreviewController : Controller
    {
        private readonly ILogger<PreviewController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public PreviewController(ILogger<PreviewController> logger,ApplicationDbContext context, UserManager<IdentityUser> userManager,IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
        
            var user = await _userManager.GetUserAsync(User);
            DateTime currentDate = DateTime.Now.Date;
            var items = _context.Currencies.Where(c => c.CreatedAt == currentDate).ToList();
            if (items.Count() == 0){
                CurrencyApi api = new CurrencyApi(_configuration);
                Dictionary<string, float> currency = api.ValueToDict(api.Currency());
                DateTime currentTime = DateTime.Now.Date;
                foreach(var curr in currency.Keys){
                    Currency currencyObj = new Currency();
                    currencyObj.Price = currency[curr];
                    currencyObj.Name = curr;
                    currencyObj.CreatedAt = currentTime;
                    if (ModelState.IsValid)
                    {
                    _context.Currencies.Add(currencyObj);
                    }
                }
                _context.SaveChanges();
                CryptoApi cryptoapi = new CryptoApi();
                cryptoapi.CryptoValue();
                items = _context.Currencies.Where(c => c.CreatedAt == currentDate).ToList();
            }
            var cryptoitems = _context.Cryptos.Where(c => c.CreatedAt == currentDate).ToList();
            var financeitems = _context.Finances.Where(c => c.CreatedAt == currentDate).ToList();
            var chatsitems = _context.Chats.Where(c => c.User == user).OrderBy(c => c.CreatedAt).ToList();

            string[] translate = new string[2]; 
            if(TempData["Translate"] != null)
            {
                translate[0] = TempData["Translate"] as string;
                translate[1] = TempData["MessageTranslate"] as string;
            }
        
            var model = new Model
            {
                chats = chatsitems,
                currencies = items,
                cryptos = cryptoitems,
                finances = financeitems,
                translate = translate

        
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> QueryPost(FormModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            Console.WriteLine(user);
            if (!string.IsNullOrEmpty(model.Query) && user != null)
            {
                
                DateTime currentTime = DateTime.Now;
                Chat chat = new Chat();
                chat.Writer = "User";
                chat.Message = model.Query;
                chat.CreatedAt = currentTime;
                chat.User = user;
                if (ModelState.IsValid)
                    {
                    _context.Chats.Add(chat);
                    }
                OpenApi ai = new OpenApi(_configuration);
                var tasks = ai.GetValueFromRequest(model.Query);
                string value = tasks.Result;
                Chat chatanswer = new Chat();
                chatanswer.Writer = "Chat";
                chatanswer.Message = value;
                chatanswer.CreatedAt = currentTime;
                chatanswer.User = user;
                if (ModelState.IsValid)
                    {
                    _context.Chats.Add(chatanswer);
                    }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult TranslatePost(TranslateFormModel model)
        {
            if (!string.IsNullOrEmpty(model.Message))
            {
                string translate = $"Translate {model.Message} from {model.FromLanguage} to {model.ToLanguage}";
                OpenApi ai = new OpenApi(_configuration);
                var tasks = ai.GetValueFromRequest(translate);
                string value = tasks.Result;
                TempData["Translate"] = value;
                TempData["MessageTranslate"] = model.Message;


            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AddTaskFormModel  model){
             if (!string.IsNullOrEmpty(model.taskMessage) && !string.IsNullOrEmpty(model.taskDate) )
            {
                MyTask task  = new MyTask();
                task.tasks = model.taskMessage;
                task.createdForDate = model.taskDate.Replace("/", "-");
                task.User = await _userManager.GetUserAsync(User);
                 if (ModelState.IsValid)
                    {
                    _context.MyTasks.Add(task);
                    }
                    await _context.SaveChangesAsync();


            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> GetTask(AddTaskFormModel  model){
            var user = await _userManager.GetUserAsync(User);
            var tasks = _context.MyTasks.Where(c=> c.User == user).ToList();
            Dictionary<string,string> values = new Dictionary<string, string>();
            foreach(var task in tasks){
                values.Add(task.createdForDate,task.tasks);
            }
            return new JsonResult(values);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}