$(document).ready(function () {
    function search() {
        var url = '/date/' + $('.cinema-year').val() + '/' + $('.cinema-month').val() + '/' + $('.cinema-day').val();
        var cinemaId = $('#CinemaID').val();
        var genreId = $('#GenreID').val();
        var movieName = $('.cinema-movie-name').val();

        cinemaId = genreId !== '' && cinemaId === '' ? 0 : cinemaId;
        if (cinemaId !== '') {
            url += '/' + cinemaId;
        }

        if (genreId !== '') {
            if (cinemaId === '') {
                url += '/0';
            }
            url += '/' + genreId;
        }

        if (movieName !== "") {
            url += '?searchString=' + movieName;
        }

        window.location.href = url;
    }

    $('.cinema select').change(function () {
        search();
    });

    $('.search').click(function () {
        search();
    });

    $('.calendar-date').click(function () {
        location.href = "/date/" + $(this).data('year') + "/" + $(this).data('month') + "/" + $(this).data('day');
    });

    addCalendarSliderEvents(function () {
        var calendarDate = $('.calendar-date.active');
        var params = {
            year: calendarDate.data('year'),
            month: calendarDate.data('month'),
            day: calendarDate.data('day'),
            cinemaId: 0,
            genreId: 0,
            searchString: ""
        };
        ajax('/Home/GetMoviesHtml', params, function (data) {
            $('.movies-container').html(data);
            addMovieSliderEvents();
        });
    });
});

function addMovieSliderEvents() {
    var moviesSlider = $('.movies').lightSlider({
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

    $('.movies-slider-next').click(function () {
        moviesSlider.goToNextSlide();
    });

    $('.movies-slider-prev').click(function () {
        moviesSlider.goToPrevSlide();
    });
}