var chatterName = 'Visitor';

// Initialize the SignalR client
// Correct spelling
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

var dialogEl = document.getElementById('chatDialog');

connection.on('ReceiveMessage', renderMessage);

connection.onclose(function () {
    onDisconnected();
    console.log("Reconnecting in 5 seconds...");
    setTimeout(startConnection, 5000);
})

function startConnection() {
    connection.start()
        .then(onConnected)
        .catch(function(err) {
            console.error(err);
        });
}

function onDisconnected() {
    dialogEl.classList.add('disconnected');
}

function onConnected() {
    dialogEl.classList.remove('disconnected');

    var messageTextBoxEl = document.getElementById('messageTextbox');
    messageTextBoxEl.focus();

    connection.invoke('SetName', chatterName);
}

function showChatDialog() {
    dialogEl.style.display = "block";
}

function sendMessage(text) {
    if (text && text.length) {
        connection.invoke('SendMessage', chatterName, text);
    }
}

function ready() {
    setTimeout(showChatDialog, 750);

    var chatFromEl = document.getElementById('chatForm');
    chatFromEl.addEventListener('submit', function (e) {
        e.preventDefault();

        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);
    });

    var welcomePannelEl = document.getElementById('chatWelcomePanel');
    welcomePannelEl.addEventListener('submit', function (e) {
        e.preventDefault();

        var name = e.target[0].value;
        if (name && name.length) {
            welcomePannelEl.style.display = 'none';
            chatterName = name;
            startConnection();
        }
    })
}

function renderMessage(name, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('H:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.textContent = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}


document.addEventListener('DOMContentLoaded', ready);