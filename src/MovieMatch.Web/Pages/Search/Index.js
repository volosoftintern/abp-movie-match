$(function () {

    $paginationPopular = $('#pagination-popular');
    $paginationSearch = $('#pagination-search');
    

    fetchPopularMovies = (event, page) => {

        event.preventDefault();

        movieMatch.search.search.getMovies({ currentPage: page }).done((response) => {
            $("#movie-list").empty();
            renderResults(response.results, $("#movie-list"));
            //$paginationPopular.twbsPagination('changeTotalPages', { totalPages: response.totalPages, currentPage: 1 });
        })
    }


    getPopularMovies = (pagination, movieList) => {
        $('.loader').fadeIn();
        $paginationSearch.hide();
        $paginationPopular.show();

        movieMatch.search.search.getMovies({currentPage: 1 }).done((response) => {

            $paginationPopular.twbsPagination({
                currentPage: 1,
                totalPages: response.totalPages,
                onPageClick: fetchPopularMovies
            });

            $("#movie-list").empty();

            renderResults(response.results, $("#movie-list"));
            $('.loader').fadeOut();
            $('.movie-list-title').fadeIn();
            //pagination.twbsPagination('changeTotalPages', response.totalPages, 1);

        });
    }

    searchMovies = (ev, page) => {
        event.preventDefault();
        $('.movie-list-title').fadeOut();
        $('.loader').fadeIn();


        console.log(`SearchMovies: ${$("#movie-name").val()}`);

        movieMatch.search.search.getMovies({ name: $("#movie-name").val(), currentPage: page }).done((res) => {

            $("#movie-list").empty();
            $('.movie-list-title').text(`Total ${res.totalResults} results for "${$("#movie-name").val()}"`);

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
                $('.movie-list-title').text(`Total ${response.totalResults} results for "${$("#movie-name").val()}"`);
            } else {
                $('.movie-list-title').text(`No result for "${$("#movie-name").val()}"`);
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
            //$('#pagination-search').twbsPagination('show', 1);
            $('#pagination-search').twbsPagination('changeTotalPages', response.totalPages,1);

            //$paginationSearch.twbsPagination({

            //    startPage: 1,

            //    totalPages: response.totalPages,

            //    onPageClick: searchMovies
            //})



        });
    })

    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }
  
    renderResults = (results, movieList) => {

        results.forEach((val, i) => {
            movieList.append(`
                            <div class="card my-3 movie-card-sm" >
                                <img style="display:${isNullOrEmpty(val.posterPath)?'none':'block'}"  class="card-img-top" src="https://image.tmdb.org/t/p/original/${val.posterPath}" alt="${val.title}">
                                <div class="card-body">
                                    <h5 class="card-title"><a href="Movies/${val.id}">${val.title}</a></h5>
                                    <p class="card-text movie-limited-overview">${val.overview}</p>
                                    <div class="d-flex justify-content-between">
                                        
                                        <button class="btn btn-sm btn-watch-later ${val.isActiveWatchLater ? 'btn-danger' : 'btn-primary'}" id='${val.id}'>${val.isActiveWatchLater ? 'UnWatch' : 'Watch Later'} </a>
                                        <button class="btn btn-sm btn-watched-before ${val.isActiveWatchedBefore ? 'btn-danger' : 'btn-secondary'}" id='${val.id}'>${val.isActiveWatchedBefore ? 'UnWatch' : 'Watched Before'} </a>
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
            $(`#${movieId}.btn-watch-later`).css("background-color", "red");
            $(`#${movieId}.btn-watch-later`).text("UnWatch");
            abp.notify.success(
                    'Movie added watch later list.',
                    'Success'
            );   
        });
    }
    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {            debugger;
            $(`#${movieId}.btn-watched-before`).data('isactivewatchedbefore', true);
            $(`#${movieId}.btn-watched-before`).css("background-color", "red");
            $(`#${movieId}.btn-watched-before`).text("UnWatch");
            abp.notify.success(
                    'Movie added watched before list.',
                    'Success'
            );   
        });
    }

    removeFromWatchLaterList = (id) => {
        abp.message.confirm('Are you sure?')
            .then((confirmed) => {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchLater(id)
                        .then(() => {
                            $(`#${id}.btn-watch-later`).data('isactivewatchlater', false);
                            abp.notify.info("Successfully deleted!");
                            $(`#${id}.btn-watch-later`).css("background-color", "blue");
                            $(`#${id}.btn-watch-later`).text("Watch Later");
                            dataTable.ajax.reload();
                        })
                }
            });
    };
    removeFromWatchedBeforeList = (id) => {
        abp.message.confirm('Are you sure?')
            .then((confirmed) => {
                if (confirmed) {
                    movieMatch.movies.movie
                        .deleteMoviesWatchedBefore(id)
                        .then(() => {
                            $(`#${id}.btn-watched-before`).data('isactivewatchedbefore', false);
                            abp.notify.info("Successfully deleted!");
                            $(`#${id}.btn-watched-before`).css("background-color", "grey");
                            $(`#${id}.btn-watched-before`).text("Watched Before");
                            dataTable.ajax.reload();
                        })
                }
            });
    };

    getPopularMovies($paginationPopular, $('#movie-list'));

});