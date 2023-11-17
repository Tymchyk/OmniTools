window.addEventListener('DOMContentLoaded', function() {
    let mainSection = document.querySelector('.main-section');
    mainSection.style.opacity = 1;
    // Додаємо подію для обробки оновлення сторінки
});

window.addEventListener('scroll', function(){
    var currentScrollPosition = window.scrollY || document.documentElement.scrollTop;
    console.log('Поточна висота прокрутки:', currentScrollPosition);
    let parallax = document.querySelectorAll('.parallax');
    let scrolled = window.scrollY;
    var windowHeight = window.innerHeight;
    let gear = document.querySelector('.gear');
    var translateY = (scrolled  / 2) % windowHeight;

    // Позитивний або від'ємний зсув по горизонталі для руху вправо-вліво
    var translateX = (scrolled % windowHeight) - windowHeight / 2;
    console.log(translateX)
    console.log(translateY)

    // Зміна напрямку при досягненні краю
    if (scrolled  % (windowHeight * 2) >= windowHeight) {
        translateX = -translateX;
    }
    var rotationFactor = 0.1;
    var bounceRotate = Math.sin(scrolled * rotationFactor) * 15;
    // Застосування трансформації до картинки
    gear.style.transform = 'translate(' + translateX + 'px, ' + translateY + 'px) rotate(' + bounceRotate + 'deg)';

        // Додаємо клас для включення додаткової анімації при скролі
    gear.classList.add('additional-animation');
    if (currentScrollPosition < 50){
        gear.classList.remove('additional-animation');
    }

    parallax.forEach(function(element) {
        let speed = element.getAttribute('data-speed');
        element.style.transform = 'translateY(' + (scrolled * speed) + 'px)';
    });
    let sections = document.querySelectorAll('.section');
    sections.forEach(function(section) {
        let sectionTop = section.getBoundingClientRect().top;
        console.log(sectionTop)
        let value = section.getAttribute('value');
        if (sectionTop < window.innerHeight + 100) {
            section.classList.add('show');
        }
        if (value === "left"){
            section.querySelector(".animate").classList.add('animate_left');
        }
        else if (value === "right"){
            section.querySelector(".animate").classList.add('animate_right');
        }

    });
});