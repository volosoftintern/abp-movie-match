$(function () {

    var followersInfoModal, followingInfoModal;
   
    var isActive;
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#UserConnectionsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getList),

            columnDefs:
                [
                    {
                        data: "extraProperties.Photo",
                        render: function (data) {
                            if (data != null) {
                                return `<img class="profile" src="/images/host/my-file-container/${data}"/>`
                            }
                            else {
                                return '<img class="profile" src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"/>'
                            }
                        }
                    },
                    {
                        title: l('Username'),
                        data: "userName"
                    },

                    {


                        data: "isActive",
                        render: function (data, type, row) {
                            isActive = row.isActive; var id = row.id;


                            if (isActive === false) {
                                return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-outline-info">Follow User</button>`

                            }
                            else {
                                return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-outline-info">UnFollow User</button>`

                            }

                       

                        }

                    }


                ]

        })
    );
   
   
    followUser = (button) => {
        var btn = $(button);
        var id = btn.attr("id");


        if ((btn).attr("isActive") == 'false') {
             movieMatch.userConnections.userConnection.follow(btn.attr("id"),btn.attr("isActive")).done(() => {
                 (btn).attr("class", "btn btn-outline-info");
               
                (btn).text("UnFollow User");
                 (btn).attr("isActive", 'true');
                 $('#UserConnectionsTable').DataTable().ajax.reload();
                 abp.notify.success('Followed user');

                 var following = document.getElementById('following');
                 var number = parseInt(following.innerText);
                 value = number + 1;
                 following.innerText = value;
                
             });
            }
            else {
                movieMatch.userConnections.userConnection.unFollow(id,isActive).done(() => {
                    (btn).attr("class", "btn btn-outline-info");
                    (btn).attr("isActive", 'false');
                    $('#UserConnectionsTable').DataTable().ajax.reload();
                    (btn).text("Follow User");
                    
                    abp.notify.success(`UnFollowed user`);
                    var following = document.getElementById('following');
                    var number = parseInt(following.innerText);
                    value = number - 1;
                    following.innerText = value;
                    
                    


                    





                });
        }
    }

    
         followingInfoModal = new abp.ModalManager({
             viewUrl: '/Following/FollowingInfoModal',
             modalClass:'FollowingInfo'
            // scriptUrl: '/Pages/Following/Index.js' //Lazy load sadece 1 kere çalışır

        });
         followersInfoModal = new abp.ModalManager({
             viewUrl: '/Followers/FollowersInfoModal',
             modalClass:'FollowersInfo'
            
            
        });
   
   
    $('#showFollowing').click(function () {

        followingInfoModal.open();
    });

   
    $('#showFollowers').on('click', function () {

        followersInfoModal.open();
    });
  

    })     
    
abp.modals.FollowersInfo = function () {

    function initModal(modalManager, args) {
        

        console.log("asd");
        var l = abp.localization.getResource('MovieMatch');
        var dataTable = $('#FollowersTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, 'asc']],
                searching: true,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowers),

                columnDefs:
                    [
                        {
                            data: "path",
                            render: function (data) {
                                if (data != null) {
                                    return `<img class="profile" src="/images/host/my-file-container/${data}"/>`
                                }
                                else {
                                    return '<img class="profile" src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"/>'
                                }
                            }
                        },

                        {
                            title: l('Username'),
                            data: "name"
                        },

                  


                    ]
            })
        );

        console.log('initialized the modal...');
    };

    return {
        initModal: initModal
    };
};





abp.modals.FollowingInfo = function () {

    function initModal(modalManager, args) {


        console.log("asd");
        var l = abp.localization.getResource('MovieMatch');
        var dataTable = $('#FollowingTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, 'asc']],
                searching: true,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowing),

                columnDefs:
                    [
                        {
                            data: "path",
                            render: function (data) {
                                if (data != null) {
                                    return `<img class="profile" src="/images/host/my-file-container/${data}"/>`
                                }
                                else {
                                    return '<img class="profile" src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"/>'
                                }
                            }
                        },
                        {
                            title: l('Username'),
                            data: "name"
                        }

                        ,

                        {


                            data: "isActive",
                            render: function (data, type, row) {
                                isActive = row.isActive; var id = row.id;


                                if (isActive === false) {
                                    return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-outline-info">Follow User</button>`


                                }
                                else {
                                    return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-outline-info">UnFollow User</button>`

                                }



                            }

                        }


                    ]
            })
        );

        console.log('initialized the modal...');
    };

    return {
        initModal: initModal
    };



};


(function () {
    $('#btn').click(function (event) {
        event.preventDefault;
    })
   
    

})

    $('#img').click(function (e) {
        e.preventDefault();
        //
        $('#UploadFileDto_File').click();
    });

    $("#UploadFileDto_File").on('change', function (e) {
        e.preventDefault();

        var formData = new FormData();
        var fileInput = document.getElementById('UploadFileDto_File');
        formData.append("Content", fileInput.files[0]);
        formData.append("Name", $('#UploadFileDto_Name').val());
        //formData = FillNewEventFormData(formData);
        console.log(fileInput);
        var httpMetod = 'POST'

        $.ajax({
            //xhr: function () {
            //    var xhr = new window.XMLHttpRequest();
            //    xhr.upload.addEventListener("progress", function (evt) {
            //        if (evt.lengthComputable) {
            //            var percentComplete = evt.loaded / evt.total;
            //            percentComplete = parseInt(percentComplete * 100);
            //            if (percentComplete !== 100) {
            //                $('#upload').prop("disabled", true);
            //            }
            //        }
            //    }, false);

            //    return xhr;
            //},
            url: '/file/upload',
            data: formData,
            type: httpMetod,
            contentType: false,
            processData: false,
            success: function (response) {
                $('#img').attr('src', '/images/host/my-file-container/'+ response);
                $('#upload').click();
                //if (eventIdInput.val().length === 36) {
                //    abp.notify.success('Updated event');
                //} else {
                //    abp.notify.success('Created event as a draft', '', toastr.options.timeOut = 2500);
                //    //eventIdInput.val(response.id);
                //    //eventUrlCodeInput.val(response.urlCode);
                //}
                //    SwitchToTrackCreation()
            },
            error: function (errorRaw) {
                abp.notify.error(errorRaw.responseJSON.error.message, 'Error',
                    toastr.options = {
                        timeOut: 2500,
                        progressBar: true,
                        positionClass: "toast-bottom-right"
                    }
                );
            }
        });

    

    });
   
    

    


    







    
 

