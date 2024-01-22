$(document).ready(function () {
    toastr.options = {
        "closeButton": true,
    }
    AOS.init();
    // inicia los AOS en los modals
    $(window).on("shown.bs.modal", function () { AOS.init({ disable: true }); });
    $(window).on("hidden.bs.modal", function () { AOS.init({ disable: false }); });
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


    
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });
    

});

// PopOvers & Tooltips
function enablePopover() {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
}