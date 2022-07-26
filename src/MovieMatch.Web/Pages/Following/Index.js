$(function () {
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#FollowingTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[0, 'asc']],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getFollowing),

            columnDefs:
                [
                    {
                        title: l('Username'),
                        data: "name"
                    }

                ]
        })
    );
})