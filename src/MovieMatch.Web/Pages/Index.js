$(function () {

    var skipCount = 0;
    var maxResultCount = 10;
    var resultCount = 0;

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

    

    getPosts = async () => {
        await $('.loader').fadeIn().promise();

        //await $('.post-list').fadeOut().promise();
        
        movieMatch.posts.post.getFeed({ userId: `${abp.currentUser.id}`,maxCount:maxResultCount,skipCount,skipCount }).done(async (res) => {
            await $('.loader').fadeOut().promise();

            //await $('.post-list').fadeIn().promise();

            renderResults(res.items);
            

            if (res.items.length < maxResultCount) {

                if (res.items.length == 0) {
                    await $('.no-post-data').fadeIn().promise();
                } else {
                    $('.btn-load-more').prop('disabled', true);
                    $('.btn-load-more').text('No More Data');

                }
            } 

            if (res.items.length>0)
                await $('.btn-load-more').fadeIn().promise();
            
            skipCount += res.items.length;
           
        });
    }

    renderResults = (posts) => {

        posts.forEach((val, i) => {
            
            $('#post-list').append(`
                <div class="card" >
                    <div class="card-body">
                       
                        <h5 class="card-title">  <img class="profile rounded-circle prep" src="/images/host/my-file-container/${val.user.extraProperties.Photo}"/>  ${val.user.name}<span class="text-muted px-1">@<a href="${val.user.userName}">${val.user.userName}</a></span><span class="px-1">&#x26AC;</span><span class="text-muted px-1">${getTimeDiffer(`${val.creationTime}`)}</span> </h5>
                        <h6 class="card-subtitle text-muted mb-1">${val.movie.title}</h6>
                        <a href="/Movies/${val.movie.id}">
                            <div class="row mb-1">
                                <div class="col-2">
                                    <img class="card-img-top" src="https://image.tmdb.org/t/p/original/${val.movie.posterPath}" alt="${val.movie.title}">
                                </div>
                                <div class="col-7">
                                    <p class="card-text movie-comment">${val.comment}</p>
                                </div>
                                <div class="col-2">
                                    <p class="card-text"><i class="fas fa-star"></i> ${val.rate}</p>
                                </div>
                            </div>
                        </a>

                    </div>
                </div>
            `)


            $(`#${val.movie.id}.btn-watch-later`).data('isactivewatchlater', val.movie.isActiveWatchLater)
            $(`#${val.movie.id}.btn-watch-later`).on('click', () => {
                const isActiveWatchLater = $(`#${val.movie.id}.btn-watch-later`).data('isactivewatchlater');
                if (isActiveWatchLater == false) {
                    addWatchLater(val.movie.id, abp.currentUser.id)
                }
                else {
                    removeFromWatchLaterList(val.movie.id)
                }

            });

            $(`#${val.movie.id}.btn-watched-before`).data('isactivewatchedbefore', val.movie.isActiveWatchedBefore)
            $(`#${val.movie.id}.btn-watched-before`).on('click', () => {
                const isActiveWatchedBefore = $(`#${val.movie.id}.btn-watched-before`).data('isactivewatchedbefore');
                if (isActiveWatchedBefore == false) {
                    addWatchedBefore(val.movie.id, abp.currentUser.id)
                }
                else {
                    removeFromWatchedBeforeList(val.movie.id)
                }
            })

        })
    }

    addWatchLater = (movieId, userId) => {
        movieMatch.moviesWatchLater.watchLater.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#${movieId}.btn-watch-later`).data('isactivewatchlater', true);
            $(`#${movieId}.btn-watch-later`).toggleClass("btn-primary btn-secondary");
            $(`#${movieId}.btn-watch-later`).text("Remove Watch Later");
            abp.notify.success(
                'Movie added watch later list.',
                'Success'
            );
        });
    }
    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#${movieId}.btn-watched-before`).data('isactivewatchedbefore', true);
            $(`#${movieId}.btn-watched-before`).toggleClass("btn-primary btn-secondary");
            $(`#${movieId}.btn-watched-before`).text("Remove Watched Before");
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
                            $(`#${id}.btn-watch-later`).toggleClass("btn-secondary btn-primary");
                            $(`#${id}.btn-watch-later`).text("Add Watch Later");
                            
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
                            $(`#${id}.btn-watched-before`).toggleClass("btn-secondary btn-primary");
                            $(`#${id}.btn-watched-before`).text("Add Watched Before");
                        })
                }
            });
    };


    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }

    loadMore =async (skipCount, maxResultCount) => {

        await $('.btn-load-more').fadeOut().promise();

        movieMatch.posts.post.getFeed({ userId: `${abp.currentUser.id}`, maxCount: maxResultCount, skipCount, skipCount }).done(async (res) => {

            await $('.loader').fadeOut().promise();

            renderResults(res.items);
           

            if (res.items.length < maxResultCount) {
                $('.btn-load-more').prop('disabled', true);
                $('.btn-load-more').html('No More Data');
                
            }

            skipCount += res.items.length;

            await $('.btn-load-more').fadeIn().promise();

        });
    }


    $('.btn-load-more').on('click', () => {
        loadMore(skipCount, maxResultCount);
    });

    getTimeDiffer=(creationTime)=>{

        const date = new Date(creationTime);
        const currentTime = new Date();
        const diff = currentTime.getTime() - date.getTime();
        const minute = 60 * 1000
        const hour = 60*minute;
        const day = 24*hour;
        const month = 30*day;

        if (diff < hour) {
            return `${Math.floor(diff/minute)}m`;
        } else if (diff < day) {
            return `${Math.floor(diff/hour)}h`;
        } else if (diff < month) {
            return `${Math.floor(diff/day)}d`;
        } 

        return date.toLocaleDateString();
       
    }

    getPosts();
    
});
