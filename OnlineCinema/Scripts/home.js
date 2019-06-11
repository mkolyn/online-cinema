$(document).ready(function () {
    $('.cinema select').change(function () {
        window.location.href = '/date/' + $('.cinema-year').val() + '/' + $('.cinema-month').val()
            + '/' + $('.cinema-day').val() + '/' + $(this).val();
    });
});