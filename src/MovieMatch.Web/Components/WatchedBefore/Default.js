$(function () {
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#MoviesWatchedBeforeTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[0, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.movies.movie.getWatchedBeforeList),
            columnDefs: [
                {
                    title: "Title",
                    data: "title"
                },
                {
                    title: "Overview",
                    data: "overview",
                },
                {
                    title: "Delete movie",
                    data: "id",
                    render: function (data) {
                        return `<button class='btn far fa-trash-alt' onclick='removeFromWatchedBeforeList("${data}")'></button>`
                    }
                }
            ]
        })
    );

    removeFromWatchedBeforeList = (id) => {

        abp.message.confirm('Are you sure?')
            .then((confirmed) => {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchedBefore(id)
                        .then(() => {
                            abp.notify.info("Successfully deleted!");
                            dataTable.ajax.reload();
                        })

                }
            });

    };
});
