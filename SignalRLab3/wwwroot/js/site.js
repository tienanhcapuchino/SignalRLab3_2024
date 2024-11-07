"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

document.getElementById("broadcastButton").disabled = true;
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("broadcastButton").disabled = false;
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    console.error(err.toString());
});

document.getElementById("broadcastButton").addEventListener("click", function (event) {
    event.preventDefault();
    var message = document.getElementById("broadcastmessage").value;
    connection.invoke("SendBroadCast", message).catch(function (err) {
        console.error(err.toString());
    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    event.preventDefault();
    var groupName = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    // Call SendMessageToGroup with correct parameters: groupName and message
    connection.invoke("SendMessageToGroup", groupName, message).catch(function (err) {
        console.error(err.toString());
    });
});

// Adjust the ReceivedMessGroup handler to display the message correctly
connection.on("ReceivedMessGroup", function (message) {
    var li = document.createElement("li");
    document.getElementById("messageList").appendChild(li);
    li.textContent = "Group message: " + message;
});
