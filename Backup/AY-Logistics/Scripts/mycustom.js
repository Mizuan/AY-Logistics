
$.download = function (url, data, method) {
    //url and data options required
    if (url && data) {
        var form = $('<form />', { action: url, method: (method || 'get') });
        $.each(data, function (key, value) {
            var input = $('<input />', {
                type: 'hidden',
                name: key,
                value: value
            }).appendTo(form);
        });
        return form.appendTo('body').submit().remove();
    }
    throw new Error('$.download(url, data) - url or data invalid');
};

$(function () {
    $(".select-highlight tbody tr").live('click', function (event) {
        $.each($(".select-highlight tbody tr"), function () {
            $(this).removeAttr('style');
            $(this).find('td').removeClass('selected-row');
        });
        $(this).find('td').addClass('selected-row');
        $(this).css({ 'background': '#fffeee' });
    });


    $(".select-highlight tbody tr").live('mouseover', function () {
        if ($(this).hasClass('selected-row')) {
            $(this).css({ 'background': '#fffeee' });
        }
    });

});