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