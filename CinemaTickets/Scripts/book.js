$(document).ready(function () {
    $('.liqpay-pay').click(function () {
        if ($('.email').hasClass('error')) {
            return false;
        }

        ajax('/Book/SaveEmail/' + $('.order-id').val(), { email: $('.email').val() }, function () {
            ajax('/Home/AllowShowThankyouPage', {}, function () {
                $('.liqpay-form').submit();
            });
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