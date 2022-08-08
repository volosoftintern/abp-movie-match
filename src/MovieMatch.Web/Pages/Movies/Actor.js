$(function () {

    var swiper = new Swiper(".movie-carousel", {
        slidesPerView: 5,
        spaceBetween: 10,
        centeredSlides: true,
        allowTouchMove: true,
        allowClick:true,
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


    //swiper.on('click', (element, event) => {
    //    const id = ($(event.target).data('id'))

        
    //})
});
