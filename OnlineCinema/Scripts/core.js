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