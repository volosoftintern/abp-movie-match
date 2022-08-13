$(function () {

    var followersInfoModal, followingInfoModal;
   
    var isActive,ids,names;
   
  
         followingInfoModal = new abp.ModalManager({
             viewUrl: '/Following/FollowingInfoModal',
             modalClass: 'FollowingInfo',
             scriptUrl: '/Pages/Following/Index.js'
           // scriptUrl: '/Pages/Following/Index.js' //Lazy load sadece 1 kere çalışır

        });
         followersInfoModal = new abp.ModalManager({
             viewUrl: '/Followers/FollowersInfoModal',
             modalClass: 'FollowersInfo',
             scriptUrl: '/Pages/Followers/Index.js'
        //     scriptUrl: '/Pages/Followers/Index.js'
            
            
        });
   
   
    $('#showFollowing').click(function () {
        var that = $(this);
        var username = that.data('name');
        

        followingInfoModal.open({ username: username });
    });

   
    $('#showFollowers').on('click', function () {
      
        var username = $(this).data('name');
        followersInfoModal.open({ username:username  });
    });
  

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
   
    

    


    







    
 

