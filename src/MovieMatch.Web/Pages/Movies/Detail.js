$(function () {

    const l = abp.localization.getResource("MovieMatch");
    $(`#idWatchLater`).on('click', (e) => {
        const isActiveWatchLater = $(`#idWatchLater`).attr("data-content");
        const id = $(`#idWatchLater`).attr("data-id");
        if (isActiveWatchLater == "False") {
            addWatchLater(id, abp.currentUser.id)
        }
        else {
            removeFromWatchLaterList(id)
        }
    });

    $(`#idWatchedBefore`).on('click', (e) => {
        const isActiveWatchedBefore = $(`#idWatchedBefore`).attr("data-content");
        const id = $(`#idWatchedBefore`).attr("data-id");
        if (isActiveWatchedBefore == "False") {
            addWatchedBefore(id, abp.currentUser.id)
        }
        else {
            removeFromWatchedBeforeList(id)
        }
    });

    addWatchLater = (movieId, userId) => {
        movieMatch.moviesWatchLater.watchLater.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#idWatchLater`).attr("data-content", "True");
            $(`#idWatchLater`).toggleClass("btn-primary btn-danger");
            $(`#idWatchLater`).text(l('RemoveFromWatchLaterList'));
            abp.notify.success(
                l('MovieAddedWatchLater'),
                l('Success')
            );
        });
    }

    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#idWatchedBefore`).attr("data-content", "True");
            $(`#idWatchedBefore`).toggleClass("btn-secondary btn-danger");
            $(`#idWatchedBefore`).text(l('RemoveFromWatchedBeforeList'));
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
                            $(`#idWatchLater`).attr("data-content", "False");
                            abp.notify.info(l('SuccessfullyDeleted'));
                            $(`#idWatchLater`).toggleClass("btn-primary btn-danger");
                            $(`#idWatchLater`).text(l('AddWatchLaterList'));

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
                            $(`#idWatchedBefore`).attr("data-content", "False");
                            abp.notify.info(l('SuccessfullyDeleted'));
                            $(`#idWatchedBefore`).toggleClass("btn-secondary btn-danger");
                            $(`#idWatchedBefore`).text(l('AddWatchedBeforeList'));

                            dataTable.ajax.reload();
                        })
                }
            });
    };

    watchersList = (movieId) => {
        var list = $("#nav-profile");
        list.empty();
        movieMatch.moviesWatchedBefore.watchedBefore.listOfUsers(movieId).done(async (res) => {
            if (res.length == 0) list.append(`<div class="no-watchers">${l('NoWatchersYet')}</div>`)
            else {

                for (var element of res) {
                    var count = 0;
                    await getCount(element.id).then((c) => {
                        count = c;
                    });
                    if (element.path != null) {
                        list.append(`
                     <div class="card d-flex flex-row">

                     <div class="col-sm-2 d-flex flex-column justify-content-around">
                        <img src="https://image.tmdb.org/t/p/original//kAVRgw7GgK1CfYEJq8ME6EvRIgU.jpg" alt="..." class="h-75 rounded-circle ">
                     </div>
                       <div class="col-sm-3">
                        <p class="card-text"><h5 class="badge bg-dark">${element.name}</h5></p>
                        <img class= "profile rounded-circle prep" id = "rounded" src="/images/host/my-file-container/${element.path}" />
                        <p class="count">${l('MovieWathced', count)}</p>
                    </div>
                     <div class="col-md-5"></div>
                    
                `);
                    }


                    if (element.name != abp.currentUser.userName) {
                        if (element.isFollow === false) {
                            list.append(` 
                                <div class="col-md-2 d-flex flex-column justify-content-center ">               
                                    <button onclick=followToggle(this) id=${element.id} isFollow=${element.isFollow} type="button" class="btn btn-outline-dark follow">${l('Follow')}</button>
                               </div>`)

                        }
                        else {

                            list.append(` 
                                <div class="col-md-2 d-flex flex-column justify-content-center ">
                                    <button onclick=followToggle(this) id=${element.id} isFollow=${element.isFollow} type="button" class="btn btn-outline-dark unfollow">${l('UnFollow')}</button>
                                </div>`)
                        }
                    }
                }
            }

        });

    }



    getCount = (id) => {

        return new Promise(function (myResolve) {
            movieMatch.moviesWatchedBefore.watchedBefore.getCount(id)
                .then((res) => {
                    myResolve(res);
                })
        });
    }


    followToggle = (button) => {

        if ($(button).hasClass('follow')) {
            movieMatch.userConnections.userConnection.follow($(button).attr('id'), $(button).attr('isFollow')).done(() => {
                $(button).toggleClass('follow unfollow');
                $(button).text(l('UnFollow'));
            });
        }
        else {
            movieMatch.userConnections.userConnection.unFollow($(button).attr('id'), $(button).attr('isFollow')).done(() => {
                $(button).toggleClass('follow unfollow');
                $(button).text(l('Follow'));
            });
        }
    }

    var swiper = new Swiper(".movie-carousel", {
        slidesPerView: 5,
        spaceBetween: 10,
        centeredSlides: true,
        allowTouchMove: true,
        allowClick: true,
        loop: true,
        direction: 'horizontal',
        autoHeight: true,
        centeredSlides: true,
        pagination: {
            el: '.swiper-pagination',
            clickable: true
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
    });
});



