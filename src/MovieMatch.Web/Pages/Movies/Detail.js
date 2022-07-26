$(function () {

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
