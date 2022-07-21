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
                        <li class="list-group-item">
                            <div class="card" style="width:300px;">
                                <img style="width:300px" class="card-img-top" src="https://image.tmdb.org/t/p/original/${val.posterPath}" alt="${val.title}">
                                <div class="card-body">
                                    <h5 class="card-title"><a href="Movies/${val.id}">${val.title}</a></h5>
                                    <p class="card-text">${val.overview}</p>
                                    <div class="d-flex justify-content-between">
                                        <button data-id="${val.id}" class="btn btn-primary">Watch Later</a>
                                        <button data-id="${val.id}" class="btn btn-primary">Watched Before</a>
                                    </div>

                                </div>
                            </div>
                        </li>
                    `)
        })
    }

});