$(function () {

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
        movieMatch.moviesWatchedBefore.watchedBefore.create({ userId: userId, movieId: movieId }).done((res) => {
            $(`#${movieId}.btn-watched-before`).data('isactivewatchedbefore', true);
            $(`#${movieId}.btn-watched-before`).css("background-color", "red");
            $(`#${movieId}.btn-watched-before`).text("UnWatch");
            abp.notify.success(
                'Movie added watched before list.',
                'Success'
            );
        });
    }

    watchersList = (movieId) => {
        var list = $("#nav-profile");
        list.empty();
        movieMatch.moviesWatchedBefore.watchedBefore.listOfUsers(movieId).done(async(res) => {
            
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
                           <p class="card-text"><span class="badge bg-light" style="font-size:20px; width:113px; border-radius:40%; font-style:oblique;color:dodgerblue">${element.userName} </span></p>
                          <div style = "position:relative; left:11px; top:35px;">
                        <p>${count} movie watched</p>
                         </div>
                        </div>
                    
                   </div>
                    <div class="col-md-5">
                      <div style = "position:relative; left:200px; top:35px;">
                        <button type="button" class="btn btn-outline-dark">takip et</button>
                         </div>
                    </div>
                  </div>
                </div>
                `);

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



    //abp.widgets.CmsRating = function ($widget) {
//    debugger;
//    var widgetManager = $widget.data("abp-widget-manager");
//    function registerUndoLink() {
//        $widget.find(".rating-undo-link").each(function () {
//            $(this).on('click', '', function (e) {
//                e.preventDefault();

//                abp.message.confirm("RatingUndoMessage"), function (ok) {
//                    if (ok) {
//                        volo.cmsKit.public.ratings.ratingPublic.delete(
//                            $ratingArea.attr("data-entity-type"),
//                            $ratingArea.attr("data-entity-id")
//                        ).then(function () {
//                            widgetManager.refresh($widget);
//                        })
//                    }
//                }
//            })
//        }
//        )
//    }
//}
//function init() {
//    registerUndoLink();
//}

//return {
//    init: init,
//}