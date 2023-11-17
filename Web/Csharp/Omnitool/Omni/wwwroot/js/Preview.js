// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelector(".menu").addEventListener("click", ()=> SetMenuBig());
document.querySelector(".menu_big").addEventListener("click", ()=> SetMenuSmall());
document.querySelectorAll(".Tasks").forEach(element => {element.addEventListener("click", ()=> SetTasks("tasks"))});
document.querySelectorAll(".Home").forEach(element => {element.addEventListener("click", ()=> SetTasks("home"))});
document.querySelectorAll(".Chat").forEach(element => {element.addEventListener("click", ()=> SetTasks("chat"))});
document.querySelectorAll(".chat_button").forEach(element => {
    element.addEventListener("click", 
    ()=> ChangeChat(element.dataset.value));
});
document.querySelector(".add_task").addEventListener("click", ()=> {
    let add_task = document.querySelector(".add_task_view");
    let task_view = document.querySelector(".task_view");
    add_task.style.display = "block";
    task_view.style.display = "none"; 
});

document.querySelector(".sendChat").addEventListener("click", () => SendChat());

function move(event) {
    const block = document.querySelector('.app');
    const container = document.querySelector('.container');
    const xAxis = (container.clientWidth / 2 - event.clientX) / 40;
    const yAxis = (container.clientHeight / 2 - event.clientY) / 40;
    block.style.transform = `rotateY(${xAxis}deg) rotateX(${-yAxis}deg)`;
  }

$(function() {
    fetch("/Preview/GetTask")
    .then(response => response.json())
    .then(specialDates => {
        console.log(specialDates);
        $('#datepicker').datepicker({
            beforeShowDay: function(date) {
                var dateString = $.datepicker.formatDate('mm-dd-yy', date);
                if (specialDates[dateString]) {
                    return [true, 'highlight', specialDates[dateString]];
                }
                return [true, '', ''];
            },
            onSelect: function(dateText, inst) {
                var formattedDate = $.datepicker.formatDate('mm-dd-yy', $(this).datepicker('getDate'));
                $('#date-text').text(specialDates[formattedDate] || '');
            }
        });
    })
    .catch(error => {
        console.error('Виникла помилка:', error);
      });
});

$(function() {
    $("#datepicker_input").datepicker();
});


function SetMenuBig()
{
    let bigMenu= document.querySelector(".app_menu_big");
    let smallMenu = document.querySelector(".app_menu_small");
    let content = document.querySelector(".content");
    let contentTasks = document.querySelector(".content_tasks");
    let textarea = document.querySelector(".textarea");
    let datapicker = document.querySelector("#datepicker");
    let crypto = document.querySelector(".crypto");
    let currency = document.querySelector(".currency");
    let finance = document.querySelector(".finance");
    let chat = document.querySelector(".task_content");
    bigMenu.classList.add('active');
    bigMenu.style.display = "block";
    smallMenu.style.display = "none";
    content.style.width = "1000px"
    content.style.marginLeft ="200px";
    contentTasks.style.marginLeft = "-200px";
    textarea.style.marginLeft = "300px";
    datapicker.style.marginLeft = "400px";
    crypto.style.marginLeft = "150px";
    currency.style.marginLeft = "525px";
    finance.style.marginLeft = "150px";
    chat.style.marginLeft = "100px";
    
}
function SetMenuSmall()
{
    let value = document.querySelector(".app_menu_big");
    let valueSmall = document.querySelector(".app_menu_small");
    let contentTasks = document.querySelector(".content_tasks");
    let content = document.querySelector(".content");
    let textarea = document.querySelector(".textarea");
    let datapicker = document.querySelector("#datepicker");
    let crypto = document.querySelector(".crypto");
    let currency = document.querySelector(".currency");
    let finance = document.querySelector(".finance");
    let chat = document.querySelector(".task_content");
    value.style.display = "none";
    valueSmall.style.display = "block";
    content.style.width = "1200px"
    content.style.marginLeft ="0px";
    contentTasks.style.marginLeft = "0px";
    textarea.style.marginLeft = "250px";
    datapicker.style.marginLeft = "350px";
    crypto.style.marginLeft = "300px";
    currency.style.marginLeft = "675px";
    finance.style.marginLeft = "300px";
    chat.style.marginLeft = "250px";
}

function SetTasks(value){
    let Array = [".content_tasks", ".content_home", ".content_chat"];
    let task = ".content_" + value;
    for (let element of Array) {
        if (element === task){
            document.querySelector(element).style.display = "block";
        }
        else{
            document.querySelector(element).style.display = "none";
        }
    }

}

function ChangeChat(value){
    let task = "." + value + "_page"
    let Array = [".chat_page", ".translate_page"];
    for (let element of Array) {
        if (element === task){
            document.querySelector(element).style.display = "block";
        }
        else{
            document.querySelector(element).style.display = "none";
        }
    }
}

function SendChat(){
    var value = document.querySelector(".query").value;
    let elem = document.createElement("div");
    elem.innerHTML = `<h6 class="chat_writer">User</h6>
    <h6 class="chat_message">${value}</h6>`
    document.querySelector(".chat_area").append(elem);
    let input = document.querySelector(".query");
    var scrollContainer = document.querySelector(".chat_area")
    scrollContainer.scrollTop = scrollContainer.scrollHeight;
    input.value = "";
    var dataObject = {Query: `${value}` };

    // Використовуємо fetch для відправлення POST-запиту на сервер
    fetch('/Preview/QueryPost', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(dataObject)
    })
    .then(response => response.json())
    .then(data => {
        if (data && data.query) {
            let elem = document.createElement("div");
            elem.innerHTML = `<h6 class="chat_writer">Chat</h6>
            <h6 class="chat_message">${data.query}</h6>`
            document.querySelector(".chat_area").append(elem);
            scrollContainer.scrollTop = scrollContainer.scrollHeight;
        }
    })
    .catch(error => {
        console.error('Error:', error);
        // Обробка помилки
    });
}
