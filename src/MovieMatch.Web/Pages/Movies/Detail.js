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
            $(`#idWatchLater`).css("background-color", "red");
            $(`#idWatchLater`).text("UnWatch");
            abp.notify.success(
                'Movie added watch later list.',
                'Success'
            );
        });
    }
    
    addWatchedBefore = (movieId, userId) => {
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#idWatchedBefore`).attr("data-content", "True");
            $(`#idWatchedBefore`).css("background-color", "red");
            $(`#idWatchedBefore`).text("UnWatch");
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
                            $(`#idWatchLater`).text("Watch Later");
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
                            $(`#idWatchedBefore`).text("Watched Before");
                            dataTable.ajax.reload();
                        })
                }
            });
    };

    watchersList = (movieId) => {
        var list = $("#nav-profile");
        list.empty();
        movieMatch.moviesWatchedBefore.watchedBefore.listOfUsers(movieId).done(async (res) => {
            debugger;
            if (res.length == 0) list.append(` <div style = "position:relative; left:133px; top:49px;font-style: oblique;font-size: 25px;">no watchers yet</div>`)
            else {
                for (var element of res) {
                    var count = 0;
                    await getCount(element.id).then((c) => {
                        count = c;
                    });
                    list.append(`
                <div class="card mb-3">
                  <div class="row g-0" style="min-height:50px">
                     <div class="col-md-2 d-flex flex-column justify-content-between">
                        <img class="card-img-top" style="padding-top: 8px;border-radius: 70%;padding-left: 8px;padding-bottom: 8px;width:120px;height:140px" src="https://image.tmdb.org/t/p/original//kAVRgw7GgK1CfYEJq8ME6EvRIgU.jpg">
                       </div>
                    <div class="col-md-5">
                       <div class="card-body position-relative">              
                           <p class="card-text"><span class="badge bg-light" style="font-size:20px; width:113px; border-radius:40%; font-style:oblique;color:darkslategray">${element.userName} </span></p>
                          <div style = "position:relative; left:11px; top:35px;">
                        <p>${count} movie watched</p>
                         </div>
                        </div>
                    
                   </div>
                    <div class="col-md-5">
                      <div style = "position:relative; left:200px; top:35px;">
                        ${abp.currentUser.userName == element.userName ? '' : '<button type="button" class="btn btn-outline-dark">takip et</button>'}
                         </div>
                    </div>
                  </div>
                </div>
                `);

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



    

});

