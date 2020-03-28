$(document).ready(function () {
    addTimeTableEvents();
    /*addCalendarSliderEvents();*/
});

function addTimeTableEvents() {
    $('.time-table-period-prev').click(function () {
        loadSelectMovieTimeHtml($('.movie-period-start-day').val(), $('.movie-period-start-month').val(), $('.movie-period-start-year').val(), -1);
    });

    $('.time-table-period-next').click(function () {
        loadSelectMovieTimeHtml($('.movie-period-end-day').val(), $('.movie-period-end-month').val(), $('.movie-period-end-year').val(), 1);
    });
}

function loadSelectMovieTimeHtml(day, month, year, direction) {
    var days = $(window).width() > 480 ? SELECT_MOVIE_TIME_DAYS : SELECT_MOVIE_MOBILE_TIME_DAYS;
    var params = { day: day, month: month, year: year, days: days, direction: direction };
    ajax('/Movie/GetSelectMovieTimeHtml/' + $('.movie-id').val(), params, function (data) {
        $('.movie-schedule').html(data);
        addTimeTableEvents();
    });
}