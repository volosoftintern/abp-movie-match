$(function () {
    

    


    $("#searchMovieForm").on('submit', (e) => {
        e.preventDefault();

        const movieName = $("#movie-name").val();
        const $pagination = $('#pagination');
        const movieList = $("#movie-list");

        const defaultPaginationOptions = {
            startPages: 1,
            totalPages:7,
            onPageClick: function (event, page) {
                movieMatch.search.search.getMovies({ name: $("#movie-name").val(), currentPage: page }).done((res) => {
                    renderResults(res.results, movieList);
                })
            }
        }

        
        if (isNullOrEmpty(movieName)) return;
        $pagination.twbsPagination(defaultPaginationOptions);
        movieMatch.search.search.getMovies({ name: movieName, currentPage: 1 }).done((response) => {

            $pagination.twbsPagination('changeTotalPages', response.totalPages, 1);

            renderResults(response.results, movieList);
            //
          
        });

    });

    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }

    renderResults = (results, movieList) => {
        movieList.empty();
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



});