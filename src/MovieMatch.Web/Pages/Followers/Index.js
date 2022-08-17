abp.modals.FollowersInfo = function () {
    function initModal(modalManager, args) {

        console.log(modalManager.getArgs().username)
        var userName = modalManager.getArgs().username;
        console.log(userName);

        var l = abp.localization.getResource('MovieMatch');
        var dataTable = $('#FollowersTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, 'asc']],
                searching: true,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowers, { username: userName }),

                columnDefs:
                    [
                        {
                            data: "path",
                            render: function (data) {
                                if (data != null) {
                                    return `<img class="profile rounded-circle" src="/images/host/my-file-container/${data}"/>`
                                }
                                else {
                                    return '<img class="profile rounded-circle" src="/default_picture.png"/>'
                                }
                            }
                        },

                        {
                            title: l('Username'),
                            data: "name",
                            render: function (name) {
                                return `<a  href=UserConnections/${name} style="text-transform:capitalize" /* onclick="changeinfo(this)"*/ > ${name} </a>`

                            }
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