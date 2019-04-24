$(document).ready(function () {
    var cities = $('.cities');

    $('.choose-city').mouseenter(function () {
        cities.addClass('active');
    }).mouseleave(function () {
        cities.removeClass('active');
    });
});