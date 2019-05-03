$(document).ready(function () {
    $('.cinema-hall-places').on('click', '.cinema-hall-cell', function () {
        if ($(this).hasClass('booked')) {
            return false;
        }

        $(this).toggleClass('active');
        var isActive = $(this).hasClass('active');
        if ($(this).hasClass('joined')) {
            $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').each(function () {
                isActive ? $(this).addClass('active') : $(this).removeClass('active');
            });
        }

        if ($('.cinema-hall-cell.active').length > 0) {
            $('.book-places').removeClass('hidden');
        } else {
            $('.book-places').addClass('hidden');
        }
    });

    $('.book-places').click(function () {
        $.ajax({
            url: '/Book/Create/' + $('.cinema-hall-movie-id').val(),
            method: "POST",
            data: {
                cinemaHallPlaces: $.map($('.cinema-hall-cell.active'), function (item) {
                    if (!$(item).hasClass('joined') || $(item).attr('data-rows') > 0 && $(item).attr('data-cells') > 0) {
                        return $(item).attr('data-id');
                    }
                }),
            },
            success: function (data) {
                window.location.href = '/Book/Confirm/' + data.id;
            }
        });
    });

    $('.cinema-hall-cell.joined.booked').each(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('booked');
    });

    $('.cinema-hall-cell.joined').not('.booked').mouseenter(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('grouped');
    }).mouseleave(function () {
        $('.cinema-hall-cell.joined').removeClass('grouped');
    });
});