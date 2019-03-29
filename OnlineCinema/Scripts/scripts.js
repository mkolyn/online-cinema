$(document).ready(function () {
    $('.generate-places').click(function () {
        $('.cinema-hall-places').html('');

        for (var i = 0; i < $('.cinema-hall-rows').val(); i++) {
            var cinemaHallRow = $('<div class="cinema-hall-row"></div>');
            for (var j = 0; j < $('.cinema-hall-cells').val(); j++) {
                cinemaHallRow.append('<div class="cinema-hall-cell"></div>');
            }
            $('.cinema-hall-places').append(cinemaHallRow);
        }

        $('.cinema-hall-cell').click(function () {
            $(this).toggleClass('active');
            if ($('.cinema-hall-cell.active').length > 0) {
                $('.cineme-hall-actions').removeClass('hidden');
            } else {
                $('.cineme-hall-actions').addClass('hidden');
            }
        });
    });
    
    $('.remove-places').click(function () {
        $('.cinema-hall-cell.active').addClass('removed');
    });

    $('.save-places').click(function () {
        var places = [];

        for (var i = 0; i < $('.cinema-hall-row').length; i++) {
            for (var j = 0; j < $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').not('.removed').length; j++) {
                places.push([i + 1, j + 1]);
            }
        }

        $.ajax({
            url: '/CinemaHallPlaces/Save',
            method: "POST",
            data: { id: $('.cinema-hall-id').val(), places: places },
            success: function () {
            }
        });
    });

    $('.cinema-hall-cell').click(function () {
        $(this).toggleClass('active');
        if ($('.cinema-hall-cell.active').length > 0) {
            $('.cineme-hall-actions').removeClass('hidden');
        } else {
            $('.cineme-hall-actions').addClass('hidden');
        }
    });
});