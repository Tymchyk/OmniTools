window.addEventListener('DOMContentLoaded', function() {
    var swiper = new Swiper('.swiper-container', {
        slidesPerView: 1,
        spaceBetween: 20,
        loop: false,
        navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
        },
        pagination: {
        el: '.swiper-pagination',
        clickable: true,
        renderBullet: function (index, className) {
            return '<span class="' + className + '">' + (index + 1) + '</span>';
        },
        },
        initialSlide: 1,  
        effect: 'coverflow',
        coverflowEffect: {
        rotate: 50,
        stretch: 0,
        depth: 100,
        modifier: 1,
        slideShadows: true,
        },
    });

    swiper.on('slideChange', function () {
        updateText();
    });

    function updateText() {
        var activeSlideText = swiper.slides[swiper.activeIndex].innerText;
        document.getElementById('currentSlideText').innerText = activeSlideText.split(/\s+/)[0];
        console.log(activeSlideText);
    }
});