"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/userHub").withAutomaticReconnect().build();

connection.on("ReceiveMessage", function (user, message) {
  var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  var encodedMsg = user + " is " + msg;
  var li = document.createElement("li");
  li.textContent = encodedMsg;
  document.getElementById("messagesList").appendChild(li);
});

connection.on("UserList", function (list) {
  var htmllist;
  console.log(list);
  document.getElementById("userList").innerHTML = "";
  var t = list.map((x) => {
    var li = document.createElement("li");
    li.textContent = x;
    document.getElementById("userList").appendChild(li);
    return li;
  });
});

connection
  .start()
  .then(function () {})
  .catch(function (err) {
    return console.error(err.toString());
  });
