$(document).ready(function () {
    addCinemaHallPlacesEvents();

    $('.book-places').click(function () {
        var data = {
            cinemaHallPlaces: getCinemaHallPlaceIds()
        };

        ajax('/Book/Create/' + $('.cinema-hall-movie-id').val(), data, function (data) {
            if (data.success) {
                window.location.href = '/Book/Confirm/' + data.id;
            } else {
                alert(data.message);
            }
        });
    });

    setInterval(function () {
        var data = {
            cinemaHallPlaces: getCinemaHallPlaceIds()
        };

        ajax('/Book/UpdatePlaces/' + $('.cinema-hall-movie-id').val(), data, function (data) {
            //$('.cinema-hall-book-places').html(data);
            //addCinemaHallPlacesEvents();
        }, true);
    }, 10000);
});

function addCinemaHallPlacesEvents() {
    $('.cinema-hall-places').on('click', '.cinema-hall-cell', function (e) {
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

        var totalPrice = 0;
        var places = '';
        $('.cinema-hall-cell.active').each(function () {
            if ($(this).hasClass('joined')) {
                if ($(this).attr('data-rows') > 0 && $(this).attr('data-cells') > 0) {
                    if (typeof $(this).attr('data-price') !== 'undefined') {
                        totalPrice += parseInt($(this).attr('data-price'));
                    } else {
                        totalPrice += parseInt($('.cinema-hall-movie-price').val());
                    }
                }
            } else {
                totalPrice += parseInt($('.cinema-hall-movie-price').val());
            }

            var placeText = 'Ряд: ' + $(this).attr('data-row') + ', Місце: ' + $(this).attr('data-cell');
            places += '<div class="book-places-info-choise-item">' + placeText + '</div>';
            
        });

        $('.book-places-info-choise').html(places);

        $('.total-price').html(totalPrice);
        totalPrice > 0 ? $('.book-places-info-total').removeClass('hidden') : $('.book-places-info-total').addClass('hidden');
    });

    $('.cinema-hall-cell.joined.booked').each(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('booked');
    });

    $('.cinema-hall-cell.joined').not('.booked').mouseenter(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('grouped');
    }).mouseleave(function () {
        $('.cinema-hall-cell.joined').removeClass('grouped');
    });
}

function getCinemaHallPlaceIds() {
    return $.map($('.cinema-hall-cell.active'), function (item) {
        if (!$(item).hasClass('joined') || $(item).attr('data-rows') > 0 && $(item).attr('data-cells') > 0) {
            return $(item).attr('data-id');
        }
    });
}