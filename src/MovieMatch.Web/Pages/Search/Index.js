$(function () {
    $paginationPopular = $('#pagination-popular');
    $paginationSearch= $('#pagination-search');

    fetchPopularMovies = (event, page) => {

        event.preventDefault();

        movieMatch.search.search.getPopularMovies({ currentPage: page }).done((response) => {
            $("#movie-list").empty();
            renderResults(response.results, $("#movie-list"));
            //$paginationPopular.twbsPagination('changeTotalPages', { totalPages: response.totalPages, currentPage: 1 });
        })
    }


    getPopularMovies = (pagination, movieList) => {

        $paginationSearch.hide();
        $paginationPopular.show();

        movieMatch.search.search.getPopularMovies({ currentPage: 1 }).done((response) => {
            
            $paginationPopular.twbsPagination({
                currentPage: 1,
                totalPages: response.totalPages,
                onPageClick: fetchPopularMovies
            });

            $("#movie-list").empty();

            renderResults(response.results, $("#movie-list"));

            //pagination.twbsPagination('changeTotalPages', response.totalPages, 1);

        });
    }

    searchMovies = (ev, page) => {
        event.preventDefault();

        movieMatch.search.search.getMovies({ name: $("#movie-name").val(), currentPage: page }).done((res) => {

            $("#movie-list").empty();
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


            $paginationSearch.twbsPagination({
                currentPage: 1,
                totalPages: response.totalPages,
                onPageClick: searchMovies
            });

            $paginationSearch.twbsPagination({

                startPage: 1,

                totalPages: response.totalPages,

                onPageClick: searchMovies
            })

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
                                        <button class="btn btn-sm btn-primary" onclick="addWatchLater(${val.id},'${abp.currentUser.id}')">Watch Later</a>
                                        <button class="btn btn-sm btn-primary" onclick="addWatchedBefore(${val.id},'${abp.currentUser.id}')">Watched Before</a>
                                    </div>

                                </div>
                            </div>

                    `)
        })
    }

    addWatchLater = (movieId, userId) => {
        movieMatch.moviesWatchLater.watchLater.create({ userId: userId, movieId: movieId }).done((res) => {
            abp.notify.success(
                'Movie added watch later list.',
                'Success'
            );
        });
    }

    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {
            abp.notify.success(
                'Movie added watch later list.',
                'Success'
            );
        });
    }

    getPopularMovies($paginationPopular, $('#movie-list'));

});