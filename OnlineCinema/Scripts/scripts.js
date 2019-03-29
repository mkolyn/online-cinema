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
    
    $('.remove-places').click(function () {
        $('.cinema-hall-cell.active').addClass('removed');
    });

    $('.save-places').click(function () {
        var places = [];

        for (var i = 0; i < $('.cinema-hall-row').length; i++) {
            for (var j = 0; j < $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').length; j++) {
                var cell = $('.cinema-hall-row').eq(i).find('.cinema-hall-cell').eq(j);
                if (!cell.hasClass('removed')) {
                    places.push({ row: i + 1, cell: j + 1, rows: cell.data('rows'), cells: cell.data('cells') });
                }
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

    $('.cinema-hall-places').on('click', '.cinema-hall-cell', function () {
        $(this).toggleClass('active');

        if ($('.cinema-hall-cell.active').length > 0) {
            if ($('.cinema-hall-cell.active').length > 1) {
                $('.join-places').removeClass('hidden');
            } else {
                $('.join-places').addClass('hidden');
            }
            $('.remove-places').removeClass('hidden');
        } else {
            $('.join-places').addClass('hidden');
            $('.remove-places').addClass('hidden');
        }
    });
});