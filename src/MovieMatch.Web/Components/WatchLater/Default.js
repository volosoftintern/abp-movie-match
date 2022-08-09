$(function () {
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#MoviesWatchLaterTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: false,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.movies.movie.getWatchLaterList),
            columnDefs:
                [
                    {
                        title: "Title",
                        data: { title: "Title", posterPath: "posterPath" },
                        render: function (data) {
                            return `<img class="card-img-top" style="position: relative; width: 149px; image-rendering: auto;height: 146px;border-radius: 50%" src="https://image.tmdb.org/t/p/original//${data.posterPath}">
                            <div style="position: relative; width:200px; top: 12px; left: 7px;">${data.title}</div>
                            `
                        },

                    },
                    {
                        title: "Overview",
                        data: { overview: "overview", id: "id" },
                        render: function (data) {
                            return `<div class="text" style="position: relative;left: -54px;top: 28px;">${data.overview}</div>
                        <button class='btn far fa-trash-alt' onclick='removeFromWatchLaterList("${data.id}")' style="position: relative;left: 609px; top:-44px"></button>`
                        }
                    },

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
                        movieMatch.moviesWatchLater.watchLater.getCount(abp.currentUser.id).done((count) => {
                            var str = l('ProfileTab:MoviesIWillWatch', count);
                            $("#ProfileManagementWrapper a[role='tab'].active")[0].innerText = str;
                            $(".tab-content > div.active>h2")[0].innerText = str;
                        });
                     })
                }
            });
        };

});
