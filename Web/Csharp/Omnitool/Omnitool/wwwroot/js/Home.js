window.addEventListener('DOMContentLoaded', function() {
    let mainSection = document.querySelector('.main-section');
    mainSection.style.opacity = 1;
});

window.addEventListener('scroll', function(){
    let parallax = document.querySelectorAll('.parallax');
    let scrolled = window.scrollY;
    parallax.forEach(function(element) {
        let speed = element.getAttribute('data-speed');
        element.style.transform = 'translateY(' + (scrolled * speed) + 'px)';
    });

    let sections = document.querySelectorAll('.section');
    sections.forEach(function(section) {
        let sectionTop = section.getBoundingClientRect().top;
        if (sectionTop < window.innerHeight - 100) {
            section.classList.add('show');
        }
    });
});