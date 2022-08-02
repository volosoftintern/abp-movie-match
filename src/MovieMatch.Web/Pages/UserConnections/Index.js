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
                        title: l('Username'),
                        data: "userName"
                    },

                    {


                        data: "isActive",
                        render: function (data, type, row) {
                            isActive = row.isActive; var id = row.id;


                            if (isActive === false) {
                                return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-success">Follow User</button>`


                            }
                            else {
                                return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-danger">UnFollow User</button>`

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
                (btn).attr("class", "btn btn-danger");
               
                (btn).text("UnFollow User");
                 (btn).attr("isActive",'true');
               
                 abp.notify.success('Followed user');
                
             });
            }
            else {
                movieMatch.userConnections.userConnection.unFollow(id,isActive).done(() => {
                    (btn).attr("class", "btn btn-success");
                             
                    (btn).attr("isActive", 'false');
                    (btn).text("Follow User");
                 
                    abp.notify.success(`UnFollowed user`);
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
                order: [[0, 'desc']],
                searching: true,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowers),

                columnDefs:
                    [
                        {
                            title: l('Username'),
                            data: "name"
                        },

                     {


                            data: "isActive",
                            render: function (data, type, row) {
                                isActive = row.isActive; var id = row.id;


                                if (isActive === false) {
                                    return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-success">Follow User</button>`


                                }
                                else {
                                    return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-danger">UnFollow User</button>`

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





abp.modals.FollowingInfo = function () {

    function initModal(modalManager, args) {


        console.log("asd");
        var l = abp.localization.getResource('MovieMatch');
        var dataTable = $('#FollowingTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[0, 'desc']],
                searching: true,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowing),

                columnDefs:
                    [
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
                                    return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-success">Follow User</button>`


                                }
                                else {
                                    return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-danger">UnFollow User</button>`

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









    
 

