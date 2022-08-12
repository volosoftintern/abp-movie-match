$(function () {
    var DOWNLOAD_ENDPOINT = "/download";

    var downloadForm = $("form#DownloadFile");

    downloadForm.submit(function (event) {
        event.preventDefault();

        var fileName = $("#fileName").val().trim();

        var downloadWindow = window.open(
            DOWNLOAD_ENDPOINT + "/" + fileName,
            "_blank"
        );
        downloadWindow.focus();
    });

    //$("#file").change(function () {
    //    var fileName = $(this)[0].files[0].name;

    //    $("#UploadFileDto_Name").val(fileName);
    //});


    $("#upload").on('click',function (e) {
        e.preventDefault();
            
        //if (!$(this).valid()) {
        //    return false;
        //}

        var formData = new FormData();
        var fileInput = document.getElementById('UploadFileDto_File');
        formData.append("Content", fileInput.files[0]);
        formData.append("Name", $('#UploadFileDto_Name').val());
        //formData = FillNewEventFormData(formData);
        console.log(fileInput);
        var httpMetod = 'POST'

        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total;
                        percentComplete = parseInt(percentComplete * 100);
                        if (percentComplete !== 100) {
                            $('#upload').prop("disabled", true);
                        }
                    }
                }, false);

                return xhr;
            },
            url: '/file/upload',
            data: formData,
            type: httpMetod,
            contentType: false,
            processData: false,
            success: function (response) {
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

    })

});