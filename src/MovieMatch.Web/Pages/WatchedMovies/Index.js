abp.modals.WatchedMoviesInfo = function () {
    function initModal(modalManager, args) {

       
        var userName = modalManager.getArgs().username;
        console.log(userName);
        var l = abp.localization.getResource('MovieMatch');
        var dataTable = $('#WatchedMoviesTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: false,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(movieMatch.movies.movie.getWatchedBeforeList, {username:userName}),
                columnDefs: [
                    {
                        title: "Title",
                        data:  "title",
                     

                    },
                    {
                        
                        data: { posterPath: "posterPath", id: "id" },
                        render: function (data) {
                            return `<img class="card-img-top img" src="https://image.tmdb.org/t/p/original//${data.posterPath}">
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




    };
    removeFromWatchedBeforeList = (id) => {
        
        abp.message.confirm('Are you sure?')
            .then((confirmed) => {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchedBefore(id)
                        .then(() => {
                            
                            $('.btn.far.fa-trash-alt').parents('tr:first').remove();
                            var totalnumberofwatchedmovies = parseInt($('#watchedMovies').text())
                            totalnumberofwatchedmovies = totalnumberofwatchedmovies - 1;
                            $('#watchedMovies').text(totalnumberofwatchedmovies);

                            abp.notify.info(l('SuccessfullyDeleted'));
                            dataTable.ajax.reload();
                            movieMatch.moviesWatchedBefore.watchedBefore.getCount(abp.currentUser.id).done((count) => {
                                var str = l('ProfileTab:MoviesIWatchedBefore', count);
                                $("#ProfileManagementWrapper a[role='tab'].active")[0].innerText = str;
                                $(".tab-content > div.active>h2")[0].innerText = str;

                            });
                        })
                }
            });
    };

    return {
        initModal: initModal
    };
};