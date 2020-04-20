$(document).ready(function () {
    /*$('.cinema select').change(function () {
        search();
    });

    $('.search').click(function () {
        search();
    });*/

    /*$('.calendar-date').click(function () {
        location.href = "/date/" + $(this).data('year') + "/" + $(this).data('month') + "/" + $(this).data('day');
    });*/

    $('.genre-select .select-option').click(loadMoviesHtml);
    addCalendarSliderEvents(loadMoviesHtml);
    addMovieSliderEvents($('.coming-soon-movies-container'));

    $('.movie-trailer').click(function () {
        var movieTrailerPopup = $('.movie-trailer-popup').removeClass('hidden');
        movieTrailerPopup.removeClass('hidden');
        movieTrailerPopup.find('iframe').attr('src', $(this).closest('.movie-item').data('youtube-url'));
    });

    $('.movie-trailer-popup-close').click(function () {
        $('.movie-trailer-popup').addClass('hidden');
    });
});

function loadMoviesHtml() {
    var calendarDate = $('.calendar-date.active');
    var params = {
        year: calendarDate.data('year'),
        month: calendarDate.data('month'),
        day: calendarDate.data('day'),
        cinemaId: 0,
        genreId: $('.genre').val(),
        searchString: $('.header-search input').val().trim()
    };
    ajax('/Home/GetMoviesHtml', params, function (data) {
        $('.movies-container').html(data);
        addMovieSliderEvents($('.movies-container'));
    });
}

function addMovieSliderEvents(container) {
    var moviesSlider = container.find('.movies').lightSlider({
        pager: false,
        //autoWidth: true,
        item: 5,
        responsive: [
            {
                breakpoint: 1280,
                settings: {
                    item: 4
                }
            },
            {
                breakpoint: 900,
                settings: {
                    item: 3
                }
            },
            {
                breakpoint: 700,
                settings: {
                    item: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    item: 1,
                    //autoWidth: false
                }
            }
        ]
    });

    container.find('.movies-slider-next').click(function () {
        moviesSlider.goToNextSlide();
    });

    container.find('.movies-slider-prev').click(function () {
        moviesSlider.goToPrevSlide();
    });
}