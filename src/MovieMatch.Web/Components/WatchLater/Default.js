﻿$(function () {
    var l = abp.localization.getResource('MovieMatch');
    debugger;
    var dataTable = $('#MoviesWatchLaterTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: false,
            paging: true,
            order: [[1, "asc"], [2, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.movies.movie.getWatchLaterList),
            columnDefs:
                [
                    {
                        title: "Title",
                        data: "title",
                    },
                    {
                        title: "Overview",
                        data: "overview",
                    },
                    {
                        title: "Delete movie",
                        data:"id",
                        render: function (data) {
                            return `<button class='btn far fa-trash-alt' onclick='removeFromWatchLaterList("${data}")'></button>`
                        }
                    }
                ]
        })
    );

    removeFromWatchLaterList = (id) => {

        abp.message.confirm('Are you sure?')
            .then((confirmed)=> {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchLater(id)
                    .then(()=> {
                        abp.notify.info("Successfully deleted!");
                        dataTable.ajax.reload();
                     })
                }
            });
        };

});
