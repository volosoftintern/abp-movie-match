$(function () {

    //$(`idWatchLater`).data('isactivewatchlater', val.isActiveWatchLater)
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
            $(`#idWatchLater`).css({ "background-color": "red", "font-size": "13px" });
            $(`#idWatchLater`).text("Remove from watch later list");
            $(`#idWatchLater`).css("text-size", "13px");
            abp.notify.success(
                'Movie added watch later list.',
                'Success'
            );
        });
    }
    
    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#idWatchedBefore`).attr("data-content", "True");
            $(`#idWatchedBefore`).css({ "background-color": "red", "font-size": "13px" });
            $(`#idWatchedBefore`).text("Remove from watched before list");
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
                            $(`#idWatchLater`).attr("data-content", "False");
                            abp.notify.info("Successfully deleted!");
                            $(`#idWatchLater`).css("background-color", "blue");
                            $(`#idWatchLater`).text("Add to watch later list");
                            
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
                            abp.notify.info("Successfully deleted!");
                            $(`#idWatchedBefore`).css("background-color", "grey");
                            $(`#idWatchedBefore`).text("Add to watched before list");
                            
                            dataTable.ajax.reload();
                        })
                }
            });
    };

    watchersList = (movieId) => {
        var list = $("#nav-profile");
        list.empty();
        movieMatch.moviesWatchedBefore.watchedBefore.listOfUsers(movieId).done(async (res) => {
            if (res.length == 0) list.append(` <div class="no watchers">no watchers yet</div>`)
            else {
                console.log(res);
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
                        <p class="count">${count} movie watched</p>
                    </div>
                     <div class="col-md-5"></div>
                    
                `);
                    }
                    else {
                        list.append(`
                    
                    
                    
                    
                  
                    <div class="card d-flex flex-row">

                     <div class="col-sm-2 d-flex flex-column justify-content-around">
                        <img src="https://image.tmdb.org/t/p/original//kAVRgw7GgK1CfYEJq8ME6EvRIgU.jpg" alt="..." class="h-75 rounded-circle ">
                     </div>
                       <div class="col-sm-3">
                        <p class="card-text"><h5 class="badge bg-dark">${element.name}</h5></p>
                        <img class="profile rounded-circle prep" id="rounded" src="/default_picture.png"/>
                        <p class="count">${count} movie watched</p>
                    </div>
                     <div class="col-md-5"></div>
                    
                `);
                    }
                    
                    if (element.name != abp.currentUser.userName) {
                        console.log(abp.currentUser.userName);
                        if (element.isFollow === false)
                        {
                                      list.append(` <div class="col-md-2 d-flex flex-column justify-content-center ">
                       
                             <button onclick=followToggle(this) id=${element.id} isFollow=${element.isFollow} type="button" class="btn btn-outline-dark follow">Follow</button>
                               </div>`)
                       
                        }
                        else {
                                          
                               list.append(` <div class="col-md-2 d-flex flex-column justify-content-center ">
                
                                   <button onclick=followToggle(this) id=${element.id} isFollow=${element.isFollow} type="button" class="btn btn-outline-dark follow">UnFollow</button>
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
        console.log(button);
        btn = $(button);
        if (btn.text() == 'Follow') {
            movieMatch.userConnections.userConnection.follow(btn.attr('id'), btn.attr('isFollow')).done(() => {

                btn.text('UnFollow');


            });
        }
        else{
            movieMatch.userConnections.userConnection.unFollow(btn.attr('id'), btn.attr('isFollow')).done(() => {

                btn.text('Follow');


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



