$(function () {

    $("#searchMovieForm").on('submit', (e) => {
        e.preventDefault();

        const movieName = $("#movie-name").val();
        const movieList = $("#movie-list");
        if (isNullOrEmpty(movieName)) return;
        
        movieMatch.search.search.getMovies({ name: movieName }).done((res) => {

            movieList.empty();

            res.forEach((val, i) => {
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
        });

    });

    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }
});