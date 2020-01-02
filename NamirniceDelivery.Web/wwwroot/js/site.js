$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');

            var forms = placeholderElement.find('form');
            $.validator.unobtrusive.parse(forms[forms.length - 1]);
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.data('post');
        var dataToSend = form.serialize();
        $.post(actionUrl, dataToSend).done(function (data) {
            if (data == "Ok") {
                placeholderElement.find('.modal').modal('hide');
                return;
            }
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var forms = placeholderElement.find('form');
            $.validator.unobtrusive.parse(forms[forms.length - 1]);
        });
    });
});

function URL_add_parameter(url, param, value) {
    var hash = {};
    var parser = document.createElement('a');

    parser.href = url;

    var parameters = parser.search.split(/\?|&/);

    for (var i = 0; i < parameters.length; i++) {
        if (!parameters[i])
            continue;

        var ary = parameters[i].split('=');
        hash[ary[0]] = ary[1];
    }

    hash[param] = value;

    var list = [];
    Object.keys(hash).forEach(function (key) {
        list.push(key + '=' + hash[key]);
    });

    parser.search = '?' + list.join('&');
    return parser.href;
}