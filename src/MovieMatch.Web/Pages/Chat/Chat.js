

$(function () {
    
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalr-hubs/chat").build();

    connection.on("ReceiveMessage", function (targetUserName,text) {
        let row = document.createElement('div');
        row.className = "row"

        let col = document.createElement('div');

        let container = document.createElement('div');
        container.className = "container bg-light";

        let sender = document.createElement('p');
        sender.className = "sender text-left";
        sender.innerHTML = targetUserName;
        let mytext = document.createElement('p');
        mytext.className = "text-left"
        mytext.innerHTML = text;

        let when = document.createElement('span');
        when.className = "time-left";
        var currentdate = new Date();
        when.innerHTML =
            (currentdate.getMonth() + 1) + "/"
            + currentdate.getDate() + "/"
            + currentdate.getFullYear() + " "
            + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

        row.appendChild(col);
        col.appendChild(container);
        container.appendChild(sender);
        container.appendChild(mytext);
        container.appendChild(when);
        $("#MessageList_" + targetUserName).append(row);
    });

    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });
    $('#submitButton').on("click",function (e) {
        
        e.preventDefault();
        var when = new Date();
        var userName = abp.currentUser.userName
        var targetUserName = $(".nav-link.active").attr("data-content"); 
        console.log(`${targetUserName}`);
        var text = $("#messageText").val(); 
        var userId = abp.currentUser.id;
        movieMatch.messages.message.create({ when: when, userName: userName, targetUserName: targetUserName, text: text, userId: userId }).done((res) => {
            connection.invoke("SendMessage", targetUserName, text)
                .then(function (data) {

                        let row = document.createElement('div');
                        row.className="row"

                        let col = document.createElement('div');
                        col.className="col-md-6 offset-md-6"

                        let container = document.createElement('div');
                        container.className = "container darker bg-primary";

                        let sender = document.createElement('p');
                        sender.className = "sender text-right text-white";
                        sender.innerHTML = abp.currentUser.userName;
                        let mytext = document.createElement('p');
                        mytext.className ="text-right text-white"
                        mytext.innerHTML = $("#messageText").val();

                        let when = document.createElement('span');
                        when.className = "time-right text-light";
                        var currentdate = new Date();
                        when.innerHTML =
                            (currentdate.getMonth() + 1) + "/"
                            + currentdate.getDate() + "/"
                            + currentdate.getFullYear() + " "
                            + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

                    row.appendChild(col);
                    col.appendChild(container);
                    container.appendChild(sender);
                    container.appendChild(mytext);
                    container.appendChild(when);
                    $(`#MessageList_${targetUserName}`).append(row);
                    $("#messageText").val("");
                })
                .catch(function (err) {
                    return console.error(err.toString());
                });

        })
        
    });

    $('.nav-link').on("click", function (e) {
        $('#messageForm').css("display", "unset");
    })

});

