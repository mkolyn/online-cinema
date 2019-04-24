$(document).ready(function () {
    $('.cinema-hall-cell.joined').mouseenter(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('active');
    }).mouseleave(function () {
        $('.cinema-hall-cell.joined').removeClass('active');
    });

    $('.cinema-hall-places').on('click', '.cinema-hall-cell', function () {
        $(this).toggleClass('active');

        if ($('.cinema-hall-cell.active').length > 0) {
            $('.book-places').removeClass('hidden');
        } else {
            $('.book-places').addClass('hidden');
        }
    });
});