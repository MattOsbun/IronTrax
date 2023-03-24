
$(function () {
    var spinner = $('<div></div>')
        .addClass('fa-3x')
        .append($('<i></i>').addClass('fas fa-spinner fa-spin'));
    var placeholderElement = $('#modal-placeholder');

    var success = function () {
        placeholderElement.empty();
    }

    function tryParseJson(str) {
        try {
            return { isJson: true, data: JSON.parse(str)}
        } catch (e) {
            return { isJson: false, data: "" };
        }
    }

    function isPostSuccessful(data) {
        var isPostSuccessful = false;
        var parsedDataResult = tryParseJson(data);
        if (parsedDataResult["isJson"]) {
            isPostSuccessful = parsedDataResult["data"]["success"];
        }

        return isPostSuccessful;
    }

    $('[data-toggle="ajax-modal"]').click(function (event) {
        event.preventDefault();
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        $(this).parents('.modal').find('.modal-footer').find('.btn').replaceWith(spinner);
        $.post(actionUrl, dataToSend).done(function (data) {
            if (isPostSuccessful(data)) {
                placeholderElement.find('.modal').modal('hide');
                success();
                if (form.data('redirect')) {
                    window.location.replace("/Redirect/RedirectToLocal?localUrl=" + form.data('redirect'));
                }
            }
            else {
                var newBody = $('.modal-body', data);
                placeholderElement.find('.modal-body').replaceWith(newBody);
            }
        });
    });
});