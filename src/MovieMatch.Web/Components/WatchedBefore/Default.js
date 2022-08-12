﻿$(function () {
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#MoviesWatchedBeforeTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: false,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.movies.movie.getWatchedBeforeList),
            columnDefs: [
                {
                    title: "Title",
                    data: { title: "Title", posterPath: "posterPath"},
                    render: function (data) {
                        return `<img class="card-img-top img" src="https://image.tmdb.org/t/p/original//${data.posterPath}">
                            <div class="position-relative">
                                    <div class="position-absolute bottom-0 start-50">
                                        ${data.title}
                                    </div>
                              </div>
                            `
                    },
                    
                },
                {
                    title: "Overview",
                    data: { overview: "overview", id: "id" },
                    render: function (data) {
                        return `<div class="text">${data.overview}</div>
                        <div class="position-relative">
                        
                         <div class="position-absolute bottom-0 end-0">
                            <button class='btn far fa-trash-alt' type="button" onclick='removeFromWatchedBeforeList("${data.id}")'></button>
                        </div>
                        
                        </div>`
                        
                    }
                },
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
                            movieMatch.moviesWatchedBefore.watchedBefore.getCount(abp.currentUser.id).done((count) => {
                                var str = l('ProfileTab:MoviesIWatchedBefore', count);
                                $("#ProfileManagementWrapper a[role='tab'].active")[0].innerText = str;
                                $(".tab-content > div.active>h2")[0].innerText = str;
                                //component refresh code below
                                //var myWidgetManager = new abp.WidgetManager('.abp-widget-wrapper[data-widget-name="WatchedBefore"]');
                                //myWidgetManager.refresh();
                            });
                     })
                }
            });
    };
});
