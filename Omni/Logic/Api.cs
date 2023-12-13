using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Omnitool.Data;


namespace Api
{
    public class OpenApi
        {

            private readonly IConfiguration _configuration;

            public OpenApi(IConfiguration configuration)
                {
                    _configuration = configuration;
                }
            public async Task<string> GetValueFromRequest(string query)
            {
                var apiKey = _configuration["OpenApiKey"];
                var openai = new OpenAIAPI(apiKey);
                CompletionRequest request = new CompletionRequest();
                request.Prompt = query;
                request.Model = OpenAI_API.Models.Model.CurieText;
                var completions = openai.Completions.CreateCompletionAsync(request);
                StringBuilder value = new StringBuilder();
                foreach (var task in completions.Result.Completions)
                {
                    value.Append(task);
                }
                string result = value.ToString();
                return result;
            
            }

        }

    public class CurrencyApi
    {


            static readonly HttpClient client = new HttpClient();
            private readonly IConfiguration _configuration;

            public CurrencyApi(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<Dictionary<string, object>> Currency(){
                var apiKey = _configuration["CurrencyApiKey"];
                HttpResponseMessage response = await client.GetAsync($"https://openexchangerates.org/api/latest.json?app_id={apiKey}");
                if(response.IsSuccessStatusCode){
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object> resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                    return resultDictionary;
                }
                else{
                    throw new HttpRequestException("An error occurred while making the request: " + response.StatusCode);
                }
            }

            public Dictionary<string, float> ValueToDict(Task<Dictionary<string, object>> task){
                Dictionary<string, float> dict = JsonSerializer.Deserialize<Dictionary<string, float>>(JsonSerializer.Serialize(task.Result["rates"]));
                Dictionary<string, float> values = new Dictionary<string, float>();
                string[] currencyName = {"UAH","EUR","GBP"};
                foreach(string currencyItem in currencyName)
                {
                    if (currencyItem == "UAH")
                    {
                        values.Add("USD", (float)Math.Round(dict["UAH"]));
                    }
                    else{
                        float price = dict["UAH"] / dict[currencyItem];
                        values.Add(currencyItem,(float)Math.Round(price,2));
                    } 
                }
                return values;
        }
    }
    public class CryptoApi
    {
        public  void CryptoValue()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string pythonFilePath = Path.Combine(currentDirectory, "Logic/CryptoandFinance.py");

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "/usr/bin/python3";
            start.Arguments = pythonFilePath;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;

            using (Process process = Process.Start(start))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                }
            }
        }
    }
    
}