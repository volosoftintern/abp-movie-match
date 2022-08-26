abp.modals.FollowingInfo = function () {
    function initModal(modalManager, args) {
        
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

                        {
                            data: "path",
                            render: function (data) {

                                return `<img class="profile rounded-circle" src="/images/host/my-file-container/${data}"/>`
                            }
                        },
                        {
                            title: l('Username'),
                            data: "name",
                            render: function (name) {

                                return `<a  href='/UserConnections/${name}' style="text-transform:capitalize" > ${name} </a>`

                            }
                        }
                        ,
                        {

                            data: "isFollow",
                            render: function (data, type, row) {
                                isActive = row.isFollow; var id = row.id;
                                if (isActive === true) {
                                    return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-outline-info">${l('FollowUser')}</button>`
                                }
                                else {
                                    return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-outline-info">${l('UnFollowUser')}</button>`
                                }
                            }

                        }
                    ]
            })
        );
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
        
            (btn).text(l('UnFollowUser'));
            
            (btn).attr("isActive", 'true');
            
            $('#UserConnectionsTable').DataTable().ajax.reload();
            abp.notify.success(l('FollowedUser'));

            $('#following').text(parseInt($('#following').text())+1);
        });
    }else {
        movieMatch.userConnections.userConnection.unFollow(id, isActive).done(() => {
            
            (btn).attr("isActive", 'false');
            
            $('#UserConnectionsTable').DataTable().ajax.reload();
            
            (btn).text(l('FollowUser'));
            
            abp.notify.success(l('UnFollowedUser'));
            
            $('#following').text(parseInt($('#following').text())-1);
        });
    }
}