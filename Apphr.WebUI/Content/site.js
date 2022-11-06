$(document).ready(function () {
    AOS.init();
    // Inicia los Select with Search    
    $('.select2find').select2({
        theme: "bootstrap-5",
        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
        placeholder: $(this).data('placeholder'),
        language: "es",
    });

    // Scroll Top
    $('a.page-scroll').bind('click', function (event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, 'slow');
        //$("html, body").animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        //var $anchor = $(this);
        //console.log($anchor)
        //$('html, body').stop().animate({
        //    scrollTop: $($anchor.attr('href')).offset().top
        //}, 1500, 'easeInOutExpo');
       
    });
    $(window).scroll(function () {
        if ($(window).scrollTop() > 100) {
            $('#back-to-top').fadeIn();
        } else {
            $('#back-to-top').fadeOut();
        }
    });
});