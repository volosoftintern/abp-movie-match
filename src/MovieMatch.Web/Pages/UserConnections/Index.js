$(function () {
    
    var followersInfoModal, followingInfoModal;

    


    followingInfoModal = new abp.ModalManager({
        viewUrl: '/Following/FollowingInfoModal',
        modalClass: 'FollowingInfo',
        scriptUrl: '/Pages/Following/Index.js'
    });
    followersInfoModal = new abp.ModalManager({
        viewUrl: '/Followers/FollowersInfoModal',
        modalClass: 'FollowersInfo',
        scriptUrl: '/Pages/Followers/Index.js'

    });
    watchedMoviesInfoModal = new abp.ModalManager({
        viewUrl: '/WatchedMovies/WatchedMoviesInfoModal',
        modalClass: 'WatchedMoviesInfo',
        scriptUrl: '/Pages/WatchedMovies/Index.js'

    });


    $('#showFollowing').click(function () {
        var that = $(this);
        var username = that.data('name');


        followingInfoModal.open({ username: username });
    });
    $('#showWatchedMovies').click(function () {
       


        watchedMoviesInfoModal.open();
    });


    $('#showFollowers').on('click', function () {

        var username = $(this).data('name');
        followersInfoModal.open({ username: username });
    });


})

$(document).ready(function () {

    var currentusername = $('#UploadFileDto_File').data('username');
    var username = $('#UploadFileDto_File').data('name');
    $('#img').click(function (e) {

        e.preventDefault();
        if (currentusername == username) {
            $('#UploadFileDto_File').click();
        }
        else {
            abp.notify.error('You only change your photo');
        }
    });
});



$("#UploadFileDto_File").on('change', function (e) {
    e.preventDefault();

    var formData = new FormData();
    var fileInput = document.getElementById('UploadFileDto_File');
    formData.append("Content", fileInput.files[0]);
    formData.append("Name", $('#UploadFileDto_Name').val());
    var httpMethod = 'POST'

    $.ajax({

        url: '/file/upload',
        data: formData,
        type: httpMethod,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#img').attr('src', '/images/host/my-file-container/' + response);
            $('#upload').click();

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









$(document).ready(function () {

    $('#followbtn').on('click', function () {
        var btn = $(this);
        if (btn.text().trim() == 'Follow') {

            movieMatch.userConnections.userConnection.follow(btn.data("id")).done(() => {
                btn.text('UnFollow');
                abp.notify.success('Followed User');
                var count = parseInt($('#followers').text());
                count++;
                $('#followers').text(count);
            });


        }
        if (btn.text().trim() == 'UnFollow') {
            movieMatch.userConnections.userConnection.unFollow(btn.data("id")).done(() => {
                btn.text('Follow');
                abp.notify.success('UnFollowed User');
                var count = parseInt($('#followers').text());
                count--;
                $('#followers').text(count);

            });
        }
    });


});









