

$(function () {
    const elements = [];
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalr-hubs/chat").build();

    connection.on("ReceiveMessage", function (targetUserName, text) {
        debugger;
        $(".nav-link.d-flex").each(function (index, element) {
            var name=element.attributes[2].nodeValue
            elements[index]=name

        });
        if (!elements.includes(targetUserName)) {
            $(".chat-list").append(`<div class="nav-link d-flex align-items-center" id="v-pills-${targetUserName}-tab" data-content="${targetUserName}" data-bs-toggle="pill" data-bs-target="#v-pills-${targetUserName}" type="button" role="tab" aria-controls="v-pills-settings" aria-selected="false">
                                                                        <img class="img-fluid" src="https://mehedihtml.com/chatbox/assets/img/user.png" alt="user img">
                                                                    <div class="ms-3">
                                                                        <h3>${targetUserName}</h3>
                                                                    </div>
                                                             </div>`)
            
        }

        $(".msg-body").append(`<div class="tab-content" id="v-pills-tabContent">
                                    <div class= "tab-pane fade" id="v-pills-${targetUserName}" role="tabpanel" aria-labelledby="v-pills-${targetUserName}-tab" tabindex="0">
                                                        <div class="divider">
                                                            <h6>Today</h6>
                                                        </div>
                                     <ul class="position-relative" id="MessageList_${targetUserName}" style=""><ul>
                                    </div>
                                </div>
                `)
        $(`#v-pills-${targetUserName}-tab`).on("click", function (e) {
            $('#messageForm').css("display", "unset");
            $("div.msg-header").empty()
            $("div.msg-header").prepend(`<div class="msg-head">
                  <div class="row">
                    <div class="col-8">
                      <div class="d-flex align-items-center">
                        <span class="chat-icon"><img class="img-fluid" src="https://mehedihtml.com/chatbox/assets/img/arroleftt.svg" alt="image title"></span>
                        <div class="flex-shrink-0">
                          <img class="img-fluid" src="https://mehedihtml.com/chatbox/assets/img/user.png" alt="user img">
                        </div>
                        <div class="flex-grow-1 ms-3">
                          <h3>${targetUserName}</h3>
                        </div>
                      </div>
                    </div>
                    <div class="col-4">
                      <ul class="moreoption">
                        <li class="navbar nav-item dropdown">
                          <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                          <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="#">Action</a></li>
                            <li><a class="dropdown-item" href="#">Another action</a></li>
                            <li>
                              <hr class="dropdown-divider">
                            </li>
                            <li><a class="dropdown-item" href="#">Something else here</a></li>
                          </ul>
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>`);
        })
       
        let container = document.createElement('li');
        container.className = "sender";

        let mytext = document.createElement('p');
        mytext.className = "text-left"
        mytext.innerHTML = text;

        let when = document.createElement('span');
        when.className = "time";
        var currentdate = new Date();
        when.innerHTML =
            (currentdate.getMonth() + 1) + "/"
            + currentdate.getDate() + "/"
            + currentdate.getFullYear() + " "
            + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })


        container.appendChild(mytext);
        container.appendChild(when);
        $("#MessageList_" + targetUserName).append(container);
        


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

                        let container = document.createElement('li');
                        container.className = "reply";

                        let mytext = document.createElement('p');
                        mytext.innerHTML = $("#messageText").val();

                        let when = document.createElement('span');
                        when.className = "time";
                        var currentdate = new Date();
                        when.innerHTML =
                            (currentdate.getMonth() + 1) + "/"
                            + currentdate.getDate() + "/"
                            + currentdate.getFullYear() + " "
                            + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

                    container.appendChild(mytext);
                    container.appendChild(when);
                    $(`#MessageList_${targetUserName}`).append(container);
                    $("#messageText").val("");
                })
                .catch(function (err) {
                    return console.error(err.toString());
                });

        })
        
    });

    $('.nav-link.d-flex').on("click", function (e) {
        $('#messageForm').css("display", "unset");
        var name = $('.nav-link.active').attr("data-content")
        $("div.msg-header").empty()
        $("div.msg-header").prepend(`<div class="msg-head">
                  <div class="row">
                    <div class="col-8">
                      <div class="d-flex align-items-center">
                        <span class="chat-icon"><img class="img-fluid" src="https://mehedihtml.com/chatbox/assets/img/arroleftt.svg" alt="image title"></span>
                        <div class="flex-shrink-0">
                          <img class="img-fluid" src="https://mehedihtml.com/chatbox/assets/img/user.png" alt="user img">
                        </div>
                        <div class="flex-grow-1 ms-3">
                          <h3>${name}</h3>
                        </div>
                      </div>
                    </div>
                    <div class="col-4">
                      <ul class="moreoption">
                        <li class="navbar nav-item dropdown">
                          <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                          <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="#">Action</a></li>
                            <li><a class="dropdown-item" href="#">Another action</a></li>
                            <li>
                              <hr class="dropdown-divider">
                            </li>
                            <li><a class="dropdown-item" href="#">Something else here</a></li>
                          </ul>
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>`);
    })

    $(".chat-list a").click(function () {
        $(".chatbox").addClass('showbox');
        return false;
    });

    $(".chat-icon").click(function () {
        $(".chatbox").removeClass('showbox');
    });

});


