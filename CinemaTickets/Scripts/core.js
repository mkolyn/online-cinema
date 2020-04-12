function ajax(url, data, func, disableAnimation) {
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

    $('.glyphicon-search').click(function () {
        typeof IS_HOME_PAGE !== 'undefined' && IS_HOME_PAGE ? loadMoviesHtml() : search();
    });
});

function addCalendarSliderEvents(afterSlideFunc) {
    var calendarSlider = $('.calendar-slider').lightSlider({
        pager: false,
        slideMargin: 1,
        item: 7,
        responsive: [
            /*{
                breakpoint: 800,
                settings: {
                    item: 4
                }
            },*/
            {
                breakpoint: 1280,
                settings: {
                    item: 5
                }
            },
            {
                breakpoint: 640,
                settings: {
                    item: 4
                }
            },
            {
                breakpoint: 480,
                settings: {
                    item: 3
                }
            },
            {
                breakpoint: 360,
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

    $('.calendar-date').click(function () {
        $('.calendar-date').removeClass('slider-active').removeClass('active');
        $(this).addClass('slider-active').addClass('active');
        calendarSlider.goToSlide($(this).index());
    });
}

function search() {
    //var url = '/date/' + $('.cinema-year').val() + '/' + $('.cinema-month').val() + '/' + $('.cinema-day').val();
    var url = '/date/';
    //var cinemaId = $('#CinemaID').val();
    //var genreId = $('#GenreID').val();
    var movieName = $('.header-search input').val().trim();

    /*cinemaId = genreId !== '' && cinemaId === '' ? 0 : cinemaId;
    if (cinemaId !== '') {
        url += '/' + cinemaId;
    }*/

    /*if (genreId !== '') {
        if (cinemaId === '') {
            url += '/0';
        }
        url += '/' + genreId;
    }*/

    if (movieName !== "") {
        url += '?searchString=' + movieName;
    }

    window.location.href = url;
}