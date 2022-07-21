$(function () {
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#MoviesWatchLaterTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.moviesWatchLater.watchLater.getList),

            columnDefs:
                [
                    {
                        title: l('MovieId'),
                        data: "movieId"
                    },
                    {
                        title: l('UserId'),
                        data: "userId",
                    },
                    {
                        title: "Delete movie",
                        data:"id",
                        render: function (data) {
                            return `<button class='btn far fa-trash-alt' onclick='removeFromList("${data}")'></button>`
                        }
                    }
                ]
        })
    );

    removeFromList = (id) => {

        abp.message.confirm('Are you sure to delete the "admin" role?')
            .then((confirmed)=> {
                if (confirmed) {
                    movieMatch.moviesWatchLater.watchLater
                    .delete(id)
                    .then(()=> {
                        abp.notify.info("Successfully deleted!");
                        dataTable.ajax.reload();
                     })
                }
            });
        };

});
