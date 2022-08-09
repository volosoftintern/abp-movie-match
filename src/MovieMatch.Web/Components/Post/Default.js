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

        if (isNullOrEmpty(rate)) return //popup
        if (isNullOrEmpty(comment)) return //popup
        if (isNullOrEmpty(movieId)) return

        let post = {
            userId: abp.currentUser.id,
            movieId: movieId,
            rate: rate,
            comment:comment
        }

        clearForms();

        postservice.post({ userId: abp.currentUser.id, movieId: movieId, rate: rate, comment: comment, }).done((res) => {
            
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


    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }

});
