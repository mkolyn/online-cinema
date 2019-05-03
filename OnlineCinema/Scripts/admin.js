var SCHEDULE_HOUR_HEIGHT = 24;
var ADMIN_BASE_URL = '/administrator/';

$(document).ready(function () {
    var scheduleMovieChooseMinute = $('.schedule-movie-choose-minute');

    $('.generate-places').click(function () {
        $('.cinema-hall-places').html('');

        for (var i = 0; i < $('.cinema-hall-rows').val(); i++) {
            var cinemaHallRow = $('<div class="cinema-hall-row"></div>');
            for (var j = 0; j < $('.cinema-hall-cells').val(); j++) {
                cinemaHallRow.append('<div class="cinema-hall-cell"></div>');
            }
            $('.cinema-hall-places').append(cinemaHallRow);
        }
    });

    $('.join-places').click(function () {
        var firstActiveCell = $('.cinema-hall-cell.active:first');
        var firstRow = firstActiveCell.closest('.cinema-hall-row').index();
        var lastRow = $('.cinema-hall-cell.active:last').closest('.cinema-hall-row').index();
        var firstCell = firstActiveCell.index();
        var lastCell = $('.cinema-hall-cell.active:last').index();

        firstActiveCell.attr('data-rows', lastRow - firstRow + 1);
        firstActiveCell.attr('data-cells', lastCell - firstCell + 1);

        for (var i = firstRow; i <= lastRow; i++) {
            for (var j = firstCell; j <= lastCell; j++) {
                $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').eq(j).removeClass('removed').addClass('joined');
            }
        }
    });

    $('.separate-places').click(function () {
        $('.cinema-hall-cell[data-group="' + $('.cinema-hall-cell.active').attr('data-group') + '"]').removeClass('joined active').removeAttr('data-rows data-cells');
    });
    
    $('.remove-places').click(function () {
        $('.cinema-hall-cell.active').addClass('removed');
    });

    $('.save-places').click(function () {
        var places = [];

        for (var i = 0; i < $('.cinema-hall-row').length; i++) {
            for (var j = 0; j < $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').length; j++) {
                var cell = $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').eq(j);
                if (!cell.hasClass('removed')) {
                    places.push({ row: i + 1, cell: j + 1, rows: cell.attr('data-rows'), cells: cell.attr('data-cells') });
                }
            }
        }

        ajax(ADMIN_BASE_URL + 'CinemaHallPlaces/Save', { id: $('.cinema-hall-id').val(), places: places });
    });

    $('.cinema-hall-cell.joined').mouseenter(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('grouped');
    }).mouseleave(function () {
        $('.cinema-hall-cell.joined').removeClass('grouped');
    });

    $('.cinema-hall-places').on('click', '.cinema-hall-cell', function () {
        $(this).toggleClass('active');

        var isActive = $(this).hasClass('active');
        if ($(this).hasClass('joined')) {
            if (isActive) {
                $('.set-places-group').removeClass('hidden');
            }
            $('.cinema-hall-cell.joined').removeClass('active');
            $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').each(function () {
                isActive ? $(this).addClass('active') : $(this).removeClass('active');
            });
        } else {
            $('.set-places-group').addClass('hidden');
        }

        if ($('.cinema-hall-cell.active').length > 0) {
            if ($('.cinema-hall-cell.active').length > 1) {
                $('.join-places').removeClass('hidden');
                $('.separate-places').addClass('hidden');
            } else {
                $('.join-places').addClass('hidden');
                var group = $(this).attr('data-group');
                if (typeof group != 'undefined' && group != '') {
                    $('.separate-places').removeClass('hidden');
                } else {
                    $('.separate-places').addClass('hidden');
                }
            }
            $('.remove-places').removeClass('hidden');
        } else {
            $('.join-places').addClass('hidden');
            $('.remove-places').addClass('hidden');
            $('.separate-places').addClass('hidden');
        }
    });

    $('.cinema-hall-cell').each(function () {
        var rows = parseInt($(this).attr('data-rows'));
        var cells = parseInt($(this).attr('data-cells'));
        if (rows > 1 || cells > 1) {
            var firstRow = $(this).closest('.cinema-hall-row').index();
            var lastRow = firstRow + rows - 1;
            var firstCell = $(this).index();
            var lastCell = firstCell + cells - 1;

            for (var i = firstRow; i <= lastRow; i++) {
                for (var j = firstCell; j <= lastCell; j++) {
                    var cell = $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').eq(j);
                    cell.attr('data-group', 'group_' + firstRow + '_' + firstCell);
                    if (i == firstRow) {
                        cell.addClass('border-top');
                    }
                    if (i == lastRow) {
                        cell.addClass('border-bottom');
                    }
                    if (j == firstCell) {
                        cell.addClass('border-left');
                    }
                    if (j == lastCell) {
                        cell.addClass('border-right');
                    }
                }
            }
        }
    });

    $('.cinema-hall-movie').autocomplete({
        source: '/Movies/Find',
        select: function (event, ui) {
            ajax(ADMIN_BASE_URL + 'CinemaHallSchedule/GetMovieItemHtml', { id: ui.item.id }, function (data) {
                $('.schedule-movies').append(data);

                addDraggableEvents($('.schedule-movie:last'));
                addScheduleMovieChooseMinuteEvents('.schedule-movie:last');
            });
        },
    });

    scheduleMovieChooseMinute.find('select').change(function () {
        var scheduleMovie = $('.schedule-movie').eq(parseInt(scheduleMovieChooseMinute.attr('data-schedule-movie')));
        var minute = parseInt($(this).val());
        var startHour = parseInt(parseInt(scheduleMovie.attr('data-start-minute')) / 60);
        var startMinute = startHour * 60 + minute;

        scheduleMovie.attr('data-start-minute', startMinute);
        setScheduleMoviePosition(scheduleMovie, startMinute);

        scheduleMovieChooseMinute.addClass('hidden');
    });

    $('.save-schedule').click(function () {
        var movies = [];
        $('.schedule-movie').each(function () {
            movies.push({
                cinemaHallMovieID: $(this).attr('data-cinema-hall-movie-id'),
                movieID: $(this).attr('data-movie-id'),
                startMinute: $(this).attr('data-start-minute'),
                isRemoved: $(this).attr('data-is-removed') == 1,
            });
        });

        var data = {
            id: $('.cinema-hall-id').val(),
            year: $('.year').val(),
            month: $('.month').val(),
            day: $('.day').val(),
            movies: movies,
        };
        ajax(ADMIN_BASE_URL + 'CinemaHallSchedule/Save', data);
    });

    $('.schedule-movie').each(function () {
        setScheduleMoviePosition($(this), parseInt($(this).attr('data-start-minute')));
    });

    addDraggableEvents($('.schedule-movie'));
    addScheduleMovieChooseMinuteEvents('.schedule-movie');

    $('.schedule-movie-remove').click(function () {
        $(this).closest('.schedule-movie').attr('data-is-removed', 1).addClass('hidden');
    });

    $('.prev-day').click(function () {
        changeDate(1, 0);
    });

    $('.next-day').click(function () {
        changeDate(1, 1);
    });

    $('.prev-week').click(function () {
        changeDate(7, 0);
    });

    $('.next-week').click(function () {
        changeDate(7, 1);
    });

    $('.set-places-group').click(function () {
        $('.popup').remove();

        var cinemaHallPlaceId = $('.cinema-hall-cell[data-group="' + $('.cinema-hall-cell.active').attr('data-group') + '"]').eq(0).attr('data-id');
        var data = {
            cinemaHallPlaceId: cinemaHallPlaceId,
        };

        ajax(ADMIN_BASE_URL + 'CinemaHallSchedule/LoadSetPlacesGroupPopupHtml', data, function (data) {
            $('body').append(data.html);

            $('.save-places-group').click(function () {
                var data = {
                    cinemaHallPlaceId: cinemaHallPlaceId,
                    cinemaPlaceGroupId: $('.cinema-place-group-id').find('select').val(),
                };

                ajax(ADMIN_BASE_URL + 'CinemaHallSchedule/SetPlacesGroup', data, function (data) {
                    $('.popup').remove();
                });
            });

            $('.popup-close').click(function () {
                $('.popup').remove();
            });
        });
    });
});

function setScheduleMoviePosition(scheduleMovie, startMinute) {
    var scheduleHour = $('.schedule-table-hour[data-hour="' + parseInt(startMinute / 60) + '"]');
    var duration = parseInt(scheduleMovie.attr('data-duration'));
    var minute = startMinute - parseInt(startMinute / 60) * 60;
    var endMinute = startMinute + duration;
    scheduleMovie.css('top', scheduleHour.offset().top - $('.schedule-movies').offset().top + minute / 5 * 2);
    scheduleMovie.css('left', scheduleHour.offset().left + scheduleHour.find('.schedule-table-hour-title').width() - $('.schedule-movies').offset().left);
    scheduleMovie.attr('data-end-minute', endMinute);
}

function addDraggableEvents(elems) {
    elems.draggable({
        stop: function (event, ui) {
            var currentHourIndex = 0;
            var currentOffsetDifference = 0;

            for (var i = 0; i < $('.schedule-table-hour').length; i++) {
                var offsetDifference = ui.offset.top - $('.schedule-table-hour').eq(i).offset().top;
                if (offsetDifference > 0 && offsetDifference < currentOffsetDifference || currentOffsetDifference == 0) {
                    currentHourIndex = i;
                    currentOffsetDifference = offsetDifference;
                }
            }

            var scheduleHour = $('.schedule-table-hour').eq(currentHourIndex);
            var scheduleMovie = $(event.target);
            var duration = parseInt(scheduleMovie.attr('data-duration'));
            var startMinute = parseInt(scheduleHour.attr('data-hour')) * 60;
            var endMinute = startMinute + duration;

            $('.schedule-movie:visible').each(function () {
                if ($(this).index() != scheduleMovie.index()) {
                    var movieStartMinute = parseInt($(this).attr('data-start-minute'));
                    var movieEndMinute = parseInt($(this).attr('data-end-minute'));
                    if (movieStartMinute < endMinute && movieEndMinute > startMinute) {
                        startMinute = movieEndMinute + 5;
                        endMinute = startMinute + duration;
                        scheduleHour = $('.schedule-table-hour[data-hour="' + parseInt(startMinute / 60) + '"]');
                    }
                }
            });

            var minute = startMinute - parseInt(startMinute / 60) * 60;
            scheduleMovie.css('top', scheduleHour.offset().top - $('.schedule-movies').offset().top + minute / 5 * 2);
            scheduleMovie.attr('data-start-minute', startMinute);
            scheduleMovie.attr('data-end-minute', endMinute);
        }
    });
}

function changeDate(days, direction) {
    var data = {
        cinemaHallId: $('.cinema-hall-id').val(),
        year: $('.year').val(),
        month: $('.month').val(),
        day: $('.day').val(),
        days: days,
        direction: direction,
    };

    ajax(ADMIN_BASE_URL + 'CinemaHallSchedule/ChangeDate', data, function (data) {
        $('.year').val(data.year);
        $('.month').val(data.month);
        $('.day').val(data.day);

        $('.schedule-title').html(data.date);
        $('.prev-day-title').html(data.prevDay);
        $('.next-day-title').html(data.nextDay);
        $('.prev-week-title').html(data.prevWeek);
        $('.next-week-title').html(data.nextWeek);

        $('.schedule-movies').html(data.html);

        $('.schedule-movie').each(function () {
            setScheduleMoviePosition($(this), parseInt($(this).attr('data-start-minute')));
            addScheduleMovieChooseMinuteEvents(this);
        });
        addDraggableEvents($('.schedule-movie'));
    });
}

function addScheduleMovieChooseMinuteEvents(selector) {
    var scheduleMovieChooseMinute = $('.schedule-movie-choose-minute');

    $(selector).find('.schedule-movie-edit').click(function () {
        var scheduleMovie = $(this).closest('.schedule-movie');
        var startMinute = parseInt(scheduleMovie.attr('data-start-minute'));
        var minute = startMinute - parseInt(startMinute / 60) * 60;

        scheduleMovieChooseMinute.removeClass('hidden');
        scheduleMovieChooseMinute.css('top', scheduleMovie.offset().top + 'px');
        scheduleMovieChooseMinute.css('left', scheduleMovie.offset().left + scheduleMovie.width() + 25 + 'px');
        scheduleMovieChooseMinute.attr('data-schedule-movie', scheduleMovie.index());
        scheduleMovieChooseMinute.find('select').find('option').prop('selected', false);
        scheduleMovieChooseMinute.find('select').find('option[value="' + minute + '"]').prop('selected', true);
    });
}