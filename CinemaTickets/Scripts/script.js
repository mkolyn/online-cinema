$(document).ready(function () {
    var cities = $('.cities');

    $('.choose-city').mouseenter(function () {
        cities.addClass('active');
    }).mouseleave(function () {
        cities.removeClass('active');
    });

    var calendarSlider = $('.calendar-slider').lightSlider({
        pager: false,
        //autoWidth: true,
        slideMargin: 0,
        responsive: [
            {
                breakpoint: 800,
                settings: {
                    item: 4
                }
            }
        ],
    });

    $('.calendar-slider-next').click(function () {
        calendarSlider.goToNextSlide();
    });

    $('.calendar-slider-prev').click(function () {
        calendarSlider.goToPrevSlide();
    });

    $('body').click(function (e) {
        if ($(e.target).closest('.select').length === 0) {
            $('.select-options').addClass('hidden');
        }
    });

    $('.select .action-button').click(function () {
        var selectOptions = $(this).closest('.select').find('.select-options');
        var isHidden = selectOptions.hasClass('hidden');
        $('.select-options').addClass('hidden');
        isHidden ? selectOptions.removeClass('hidden') : selectOptions.addClass('hidden');
    });
});