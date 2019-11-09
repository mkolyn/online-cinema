$(document).ready(function () {
    $('.liqpay-pay').click(function () {
        var email = $('.email');

        if (email.hasClass('error')) {
            return false;
        }

        if (!isValidEmail(email.val())) {
            email.addClass('error');
            return false;
        }

        // validate order, then save user email (to send qr code later), then pay for tickets
        var orderItemIds = $.map($('.order-item'), function (item) {
            return $(item).data('id');
        });

        ajax('/Book/CheckOrder/' + $('.order-id').val(), { orderItemIds: orderItemIds }, function (data) {
            if (data.success) {
                ajax('/Book/UpdateOrder/' + $('.order-id').val(), { email: email.val() }, function () {
                    ajax('/Home/AllowShowThankyouPage', {}, function () {
                        $('.liqpay-form').submit();
                    });
                });
            } else {
                alert(data.message);
            }
        });

        return false;
    });

    var confirmTimeLeft = $('.time-left');
    var confirmToTime = Math.round(new Date(YEAR, MONTH - 1, DAY, HOUR, MINUTE, SECOND) / 1000);
    var currentTime = Math.round(new Date() / 1000);
    var timeLeft = confirmToTime - currentTime;

    if (timeLeft > 0) {
        setInterval(function () {
            updateTimeLeft();
        }, 1000);
    }

    function updateTimeLeft() {
        var currentTime = Math.round(new Date() / 1000);
        var timeLeft = confirmToTime - currentTime;

        if (timeLeft >= 0) {
            var minutesLeft = timeLeft / 60;
            minutesLeft = minutesLeft > 1 ? parseInt(minutesLeft) : 0;
            var secondsLeft = timeLeft - minutesLeft * 60;

            minutesLeft = minutesLeft < 10 ? "0" + minutesLeft : minutesLeft;
            secondsLeft = secondsLeft < 10 ? "0" + secondsLeft : secondsLeft;

            confirmTimeLeft.html(minutesLeft + ":" + secondsLeft);
        } else {
            $('.liqpay-form').addClass('hidden');
        }
    }
});