$(document).ready(function () {
    var cities = $('.cities');

    $('.choose-city').mouseenter(function () {
        cities.addClass('active');
    }).mouseleave(function () {
        cities.removeClass('active');
    });

    $('body').click(function (e) {
        if ($(e.target).closest('.select').length === 0) {
            $('.select-options').addClass('hidden');
        }
    });

    $('.select .action-button').click(function () {
        var selectOptions = $(this).closest('.select').find('.select-options');
        var isHidden = selectOptions.hasClass('hidden');
        $('.select-options').addClass('hidden');
        isHidden ? selectOptions.removeClass('hidden') : selectOptions.addClass('hidden');
    });

    $('.select-option').click(function () {
        var select = $(this).closest('.select');
        select.find('.select-value').val($(this).data('value'));
        select.find('.select-title').html($(this).html());
        select.find('.select-option').removeClass('selected');
        $(this).addClass('selected');
        select.find('.select-options').addClass('hidden');
    });

    $('.glyphicon-menu-hamburger').click(function () {
        $('.mobile-menu').removeClass('hidden');
        $('body').addClass('mobile-menu-opened');
    });

    $('.mobile-menu-close').click(function () {
        $('.mobile-menu').addClass('hidden');
        $('body').removeClass('mobile-menu-opened');
    });
});