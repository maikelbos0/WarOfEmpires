let AjaxManager = {
    onSuccess: []
}

$(function () {
    function displayMessages(result, form) {
        if (result.warnings.length > 0) {
            $.each(result.warnings, function () {
                toastr.warning(this);
            });
        }
        else {
            let successMessage = form.data("success-message");
            let command = form.find("#Command").val();

            if (command && form.data("success-message-" + command)) {
                successMessage = form.data("success-message-" + command);
            }

            if (successMessage) {
                toastr.success(successMessage);
            }
        }
    }

    function displayResult(panel, result) {
        panel.html(result);

        // Since the page won't be refreshed we try to find the title in the new content
        let title = panel.find('h2').text();

        if (title) {
            document.title = title + ' - War of Empires';
        }
    }

    function handleErrors(jqXHR) {
        if (jqXHR.status == 401) {
            window.location.assign(window.location.href);
        }
        else {
            toastr.error("An error occurred processing data; please try again.");
            submitButtons.prop('disabled', false);
        }
    }

    function callAjaxCallbacks() {
        $.each(AjaxManager.onSuccess, function (i, func) {
            func();
        });
    }

    $('body').on('submit', 'form:not(.html-only):not(.search-form)', function () {
        let form = $(this);
        let panel = form.closest('#main-content, .partial-content');
        let submitButtons = form.find('button[type="submit"]');

        if (panel.length === 0) {
            toastr.error("An error occurred locating content panel; please contact support to resolve this issue.");
        }
        else if (form.valid()) {
            submitButtons.prop('disabled', true);

            $.ajax({
                url: this.action,
                method: "POST",
                data: form.serialize(),
                success: function (result) {
                    if (result.success) {
                        displayMessages(result, form);

                        $.ajax({
                            url: result.redirectUrl,
                            method: "GET",
                            cache: false,
                            success: function (result) {
                                displayResult(panel, result);

                                callAjaxCallbacks();
                            },
                            error: handleErrors
                        });
                    }
                    else {
                        displayResult(panel, result);

                        callAjaxCallbacks();
                    }
                },
                error: handleErrors
            });
        }

        return false;
    });
});