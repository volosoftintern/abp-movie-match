$(function () {

    var swiper = new Swiper(".movie-carousel", {
        slidesPerView: 5,
        spaceBetween:10,
        centeredSlides: true,
        allowTouchMove:true,
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
