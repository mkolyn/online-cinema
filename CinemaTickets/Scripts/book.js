$(document).ready(function () {
    $('.liqpay-pay').click(function () {
        $.ajax({
            url: '/Home/AllowShowThankyouPage',
            method: "POST",
            success: function () {
                $('.liqpay-form').submit();
            }
        });

        return false;
    });
});