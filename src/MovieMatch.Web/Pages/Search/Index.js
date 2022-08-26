$(function () {

    $paginationPopular = $('#pagination-popular');
    $paginationSearch = $('#pagination-search');
    
    const l= abp.localization.getResource("MovieMatch");
    fetchPopularMovies = (event, page) => {

        event.preventDefault();

        movieMatch.search.search.getMovies({ currentPage: page }).done((response) => {
            $("#movie-list").empty();
            renderResults(response.results, $("#movie-list"));
        })
    }


    getPopularMovies =async (pagination, movieList) => {
        await $('.loader').fadeIn().promise();

        $paginationSearch.hide();
        $paginationPopular.show();

        movieMatch.search.search.getMovies({currentPage: 1 }).done(async (response) => {

            $paginationPopular.twbsPagination({
                currentPage: 1,
                totalPages: response.totalPages,
                onPageClick: fetchPopularMovies
            });

            $("#movie-list").empty();

            renderResults(response.results, $("#movie-list"));
            await $('.loader').fadeOut().promise();
            await $('.movie-list-title').fadeIn().promise();
        });
    }

    searchMovies = (ev, page) => {
        ev.preventDefault();
        $('.movie-list-title').fadeOut();
        $('.loader').fadeIn();

        movieMatch.search.search.getMovies({ name: $("#movie-name").val(), currentPage: page }).done((res) => {

            $("#movie-list").empty();
            $('.movie-list-title').text(l('MovieListTotalResult',res.totalResults,$("#movie-name").val()));

            $('.loader').fadeOut();
            $('.movie-list-title').fadeIn();

            renderResults(res.results, $("#movie-list"));
            
        })
    }


    $("#searchMovieForm").on('submit', (e) => {

        e.preventDefault();

        const movieName = $("#movie-name").val();

        if (isNullOrEmpty(movieName)) return;

        $paginationPopular.hide();
        $paginationSearch.show();

        movieMatch.search.search.getMovies({ name: movieName, currentPage: 1 }).done((response) => {



            $("#movie-list").empty();
            if (response.totalResults > 0) {
                $('.movie-list-title').text(l('MovieListTotalResult',response.totalResults,$("#movie-name").val()));
            } else {
                $('.movie-list-title').text(l('MovieListNoResult',$("#movie-name").val()));
            }
                
            $('.loader').fadeOut();
            $('.movie-list-title').fadeIn();

            renderResults(response.results, $("#movie-list"));


            $('#pagination-search').twbsPagination({
                currentPage:1,
                startPage: 1,
                totalPages: response.totalPages,
                onPageClick: searchMovies
            })
            
            $('#pagination-search').twbsPagination('changeTotalPages', response.totalPages,1);
        });
    })

    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }
  
    renderResults = (results, movieList) => {

        results.forEach((val, i) => {
            movieList.append(`
                            <div class="card my-3 movie-card-sm">
                                <img style="display:${isNullOrEmpty(val.posterPath)?'none':'block'}"  class="card-img-top" src="https://image.tmdb.org/t/p/original/${val.posterPath}" alt="${val.title}">
                                <div class="card-body">
                                    <h5 class="card-title"><a href="Movies/${val.id}">${val.title}</a></h5>
                                    <p class="card-text movie-limited-overview">${val.overview}</p>
                                    <div class="d-flex justify-content-between">
                                        
                                        <button type="button" class="btn btn-sm btn-watch-later ${val.isActiveWatchLater ? 'btn-danger' : 'btn-primary'}" id='${val.id}'>${val.isActiveWatchLater ? l('Unwatch') : l('WatchLater')} </a>
                                        <button type="button" class="btn btn-sm btn-watched-before ${val.isActiveWatchedBefore ? 'btn-danger' : 'btn-secondary'}" id='${val.id}'>${val.isActiveWatchedBefore ? l('Unwatch') : l('WatchedBefore')} </a>
                                    </div>
                                </div>
                            </div>

                    `)


            $(`#${val.id}.btn-watch-later`).data('isactivewatchlater', val.isActiveWatchLater)
            $(`#${val.id}.btn-watch-later`).on('click', () => {
                const isActiveWatchLater = $(`#${val.id}.btn-watch-later`).data('isactivewatchlater');
                if (isActiveWatchLater == false) {
                    addWatchLater(val.id, abp.currentUser.id)
                }
                else {
                    removeFromWatchLaterList(val.id)
                }
                
            })
            $(`#${val.id}.btn-watched-before`).data('isactivewatchedbefore', val.isActiveWatchedBefore)
            $(`#${val.id}.btn-watched-before`).on('click', () => {
                const isActiveWatchedBefore = $(`#${val.id}.btn-watched-before`).data('isactivewatchedbefore');
                if (isActiveWatchedBefore == false) {
                    addWatchedBefore(val.id, abp.currentUser.id)
                }
                else {
                    removeFromWatchedBeforeList(val.id)
                }
             })
        })
    }
    
    addWatchLater = (movieId, userId) => {
        movieMatch.moviesWatchLater.watchLater.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#${movieId}.btn-watch-later`).data('isactivewatchlater', true);
            $(`#${movieId}.btn-watch-later`).toggleClass("btn-primary btn-danger");
            $(`#${movieId}.btn-watch-later`).text(l('Unwatch'));
            abp.notify.success(
                    l('MovieAddedWatchLater'),
                    l('Success')
            );   
        });
    }
    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {            
            $(`#${movieId}.btn-watched-before`).data('isactivewatchedbefore', true);
            $(`#${movieId}.btn-watched-before`).toggleClass("btn-secondary btn-danger");
            $(`#${movieId}.btn-watched-before`).text(l('Unwatch'));
            abp.notify.success(
                    l('MovieAddedWatchedBefore'),
                    l('Success')
            );   
        });
    }

    removeFromWatchLaterList = (id) => {
        abp.message.confirm(l('AreYouSure'))
            .then((confirmed) => {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchLater(id)
                        .then(() => {
                            $(`#${id}.btn-watch-later`).data('isactivewatchlater', false);
                            abp.notify.info(l('SuccessfullyDeleted'));
                            $(`#${id}.btn-watch-later`).toggleClass("btn-danger btn-primary");
                            $(`#${id}.btn-watch-later`).text(l('WatchLater'));
                            
                        })
                }
            });
    };
    removeFromWatchedBeforeList = (id) => {
        abp.message.confirm(l('AreYouSure'))
            .then((confirmed) => {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchedBefore(id)
                        .then(() => {
                            $(`#${id}.btn-watched-before`).data('isactivewatchedbefore', false);
                            abp.notify.info(l('SuccessfullyDeleted'));
                            $(`#${id}.btn-watched-before`).toggleClass("btn-danger btn-secondary");
                            $(`#${id}.btn-watched-before`).text(l('WatchedBefore'));
                            
                        })
                }
            });
    };

    getPopularMovies($paginationPopular, $('#movie-list'));

});