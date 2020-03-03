﻿function ajax(url, data, func, disableAnimation) {
    disableAnimation = typeof disableAnimation !== 'undefined' ? disableAnimation : false;
    if (!disableAnimation) {
        $('body').append('<div class="ajax-loading"></div>');
    }

    $.ajax({
        url: url,
        method: "POST",
        data: data,
        success: function (data) {
            if (!disableAnimation) {
                $('.ajax-loading').remove();
            }
            
            if (typeof func === 'function') {
                func(data);
            }
        }
    });
}

function isValidEmail(email) {
    return /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(email);
}

$(document).ready(function () {
    $('.btn, .button').click(function (e) {
        var isValid = true;
        $(this).closest('form').find('.input').removeClass('error');
        var requiredInputs = $(this).closest('form').find('.required');
        requiredInputs.removeClass('error');

        for (var i = 0; i < requiredInputs.length; i++) {
            if (requiredInputs.eq(i).val().trim() === '') {
                requiredInputs.eq(i).addClass('error');
                if (requiredInputs.eq(i).closest('.input').length > 0) {
                    requiredInputs.eq(i).closest('.input').addClass('error');
                }
                isValid = false;
            }
        }

        if (!isValid) {
            return false;
        }
    });
});

function addCalendarSliderEvents(afterSlideFunc) {
    var calendarSlider = $('.calendar-slider').lightSlider({
        pager: false,
        slideMargin: 0,
        item: 7,
        responsive: [
            {
                breakpoint: 800,
                settings: {
                    item: 4
                }
            },
            {
                breakpoint: 480,
                settings: {
                    item: 2
                }
            }
        ],
        onSliderLoad: function (el) {
            calendarSlider.goToSlide($('.calendar-date.slider-active').index());
        },
        onAfterSlide: function (el) {
            if (typeof afterSlideFunc === 'function') {
                afterSlideFunc();
            }
        }
    });

    $('.calendar-slider-next').click(function () {
        calendarSlider.goToNextSlide();
    });

    $('.calendar-slider-prev').click(function () {
        calendarSlider.goToPrevSlide();
    });
}