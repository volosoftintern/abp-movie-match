abp.modals.FollowingInfo = function () {
    function initModal(modalManager, args) {
        console.log(args)
        console.log(modalManager.getArgs().username)
        var userName = modalManager.getArgs().username;
        var l = abp.localization.getResource('MovieMatch');
        var dataTable = $('#FollowingTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, 'asc']],
                searching: true,
                scrollX: true,

                ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowing, { username: userName }),

                columnDefs:
                    [
                        //{
                        //    //data: "id",
                        //    //render: function (data) {
                        //    //    movieMatch.userConnections.userConnection.getFollowersCount(data).done(res => {
                        //    //        $('#followers').innerText = res;
                        //    //    });

                        //    //}
                        //},
                        {
                            data: "path",
                            render: function (data) {
                                if (data != null) {
                                    return `<img class="profile rounded-circle" src="/images/host/my-file-container/${data}"/>`
                                }
                                else {
                                    return '<img class="profile rounded-circle" src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"/>'
                                }
                            }
                        },
                        {
                            title: l('Username'),
                            data: "name",
                            render: function (name) {

                                return `<a  href=${name} style="text-transform:capitalize" /* onclick="changeinfo(this)"*/ > ${name} </a>`

                            }



                        }

                        ,

                        {

                            
                            data: "isFollow",
                            render: function (data, type, row) {
                                isActive = row.isFollow; var id = row.id;


                                if (isActive === true) {
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

followUser = (button) => {
    var btn = $(button);
    var id = btn.attr("id");


    if ((btn).attr("isActive") == 'false') {
        movieMatch.userConnections.userConnection.follow(btn.attr("id"), btn.attr("isActive")).done(() => {
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
        movieMatch.userConnections.userConnection.unFollow(id, isActive).done(() => {
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