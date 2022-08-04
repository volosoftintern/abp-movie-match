$(function () {

    $('#searchBtn').on('click', (e) => {
        const movieName = $('#movieName').val();
        if (isNullOrEmpty(movieName)) return;

        movieMatch.search.search.getMovies({ name: movieName, currentPage: 1 }).done((response) => {
            $('#movieList')
                .find('option')
                .remove()

            response.results.forEach((val, i) => {
                $('#movieList').append(`<option value="${val.id}">${val.title}</option>`);
            });

        })
    });

    $('#shareBtn').on('click', (e) => {

        const rate = $("input[name=rating]:checked").val();
        const comment = $("#movieComment").val();
        const movieId = $("#movieList").val();
        const movieTitle = $("#movieList option:selected").text('selected')

        if (isNullOrEmpty(rate)) return //popup
        if (isNullOrEmpty(comment)) return //popup
        if (isNullOrEmpty(movieId)) return

        let post = {
            userId: `${abp.currentUser.id}`,
            movieId: movieId,
            rate: rate,
            comment: `${comment}`,
            movieTitle: `${movieTitle}`
        }

        movieMatch.posts.post.create(post).done((res) => {

            $('#post-list').prepend(`
                <div class="card" style="width: 18rem;">
                    <div class="card-body">
                        <h5 class="card-title">${res.username}</h5>
                        <h6 class="card-subtitle mb-2 text-muted">${res.movieTitle}</h6>
                        <p class="card-text">${res.comment}</p>
                        <p class="card-text"><i class="fas fa-star"></i> ${res.rate}</p>
                    </div>
                </div>
            `)
            clearForms();
        });

    });

    clearForms = () => {
        $('#movieName').val('');
        $('#movieComment').val('');
        $('#movieList')
            .find('option')
            .remove();

        $('input[type=radio]:checked').prop('checked', false);

    }

    

    getPosts = () => {
        movieMatch.posts.post.getFeed({ userId: `${abp.currentUser.id}` }).done((res) => {
            //render results
            renderResults(res.items);
        });
    }

    renderResults = (posts) => {
        $('#post-list').empty();

        posts.forEach((val, i) => {
            $('#post-list').append(`
                <div class="card" style="width: 18rem;">
                    <div class="card-body">
                        <h5 class="card-title">${val.username}</h5>
                        <h6 class="card-subtitle mb-2 text-muted">${val.movieTitle}</h6>
                        <p class="card-text">${val.comment}</p>
                        <p class="card-text"><i class="fas fa-star"></i> ${val.rate}</p>
                    </div>
                </div>
            `)
        })
    }


    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }


    getPosts();
    
});
