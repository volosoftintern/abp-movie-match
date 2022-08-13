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
                                    return `<img class="profile" src="/images/host/my-file-container/${data}"/>`
                                }
                                else {
                                    return '<img class="profile" src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"/>'
                                }
                            }
                        },

                        {
                            title: l('Username'),
                            data: "name",
                            render: function (name) {
                                return `<a  href=${name} /* onclick="changeinfo(this)"*/ > ${name} </a>`

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