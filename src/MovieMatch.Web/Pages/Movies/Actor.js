$(function () {

    let skipCount = 0;
    const maxCount = 20;
    var totalCount = 0;

    var swiper = new Swiper(".movie-carousel", {
        slidesPerView: 5,
        spaceBetween: 10,
        allowTouchMove: true,
        allowClick:true,
        loop: false,
        direction: 'horizontal',
        autoHeight: true,
        centeredSlides: false,
        pagination: {
            el: '.swiper-pagination',
            clickable: true
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
    });


    swiper.on('reachEnd', () => {
        if (swiper.slides.length < maxCount || swiper.slides.length == totalCount) return;
        let pageNumber = parseInt(swiper.slides.length / maxCount);
        
        skipCount = swiper.slides.length;

        movieMatch.movies.movie.getPersonMovies({
            skipCount: skipCount,
            personId: $('.person-card').data('personid'),
            isDirector: false,
            pageNumber: pageNumber+1
        }).done(async (res) => {

            totalCount = res.totalCount;
            await renderResults(res);
            swiper.update();
        })
    });

    renderResults = async (res) => {
        return new Promise((resolve, reject) => {
            res.items.forEach((val, index) => {

                $('.swiper-wrapper').append(`
                    <div class="swiper-slide">
                        <a href="/Movies/${val.id}">
                            <img alt="${val.title}" src="https://image.tmdb.org/t/p/original/${val.posterPath}" />
                        </a>
                    </div>
                `);

            })
            resolve();
        })
    }
});
