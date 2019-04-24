$(document).ready(function () {
    $('.cinema-hall-cell.joined').mouseenter(function () {
        $('.cinema-hall-cell.joined[data-group="' + $(this).attr('data-group') + '"]').addClass('active');
    }).mouseleave(function () {
        $('.cinema-hall-cell.joined').removeClass('active');
    });
});