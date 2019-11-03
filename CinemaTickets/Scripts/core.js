function ajax(url, data, func) {
    $('body').append('<div class="ajax-loading"></div>');

    $.ajax({
        url: url,
        method: "POST",
        data: data,
        success: function (data) {
            $('.ajax-loading').remove();
            if (typeof func == 'function') {
                func(data);
            }
        }
    });
}

function isValidEmail(email) {
    return /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(email);
}

$(document).ready(function () {
    $('.btn').click(function (e) {
        var isValid = true;
        var requiredInputs = $(this).closest('form').find('.required');
        requiredInputs.removeClass('error');

        for (var i = 0; i < requiredInputs.length; i++) {
            if (requiredInputs.eq(i).val().trim() == '') {
                requiredInputs.eq(i).addClass('error');
                isValid = false;
            }
        }

        if (!isValid) {
            return false;
        }
    });
});