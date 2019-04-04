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
                    places.push({ row: i + 1, cell: j + 1, rows: cell.attr('data-rows'), cells: cell.attr('data-cells') });
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
});