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

    $("#UploadFileDto_File").change(function () {
        var fileName = $(this)[0].files[0].name;

        $("#UploadFileDto_Name").val(fileName);
    });
});