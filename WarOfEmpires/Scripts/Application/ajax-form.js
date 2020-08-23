let AjaxManager = {
    onSuccess: []
}

$(function () {
    function redirectUnauthenticatedRequest(jqXHR) {
        if (jqXHR.getResponseHeader("X-Unauthenticated")) {
            // Don't reload in case the current request is a POST
            window.location.assign(window.location.href);
            return true;
        }
    }

    function displaySuccessMessage(form, jqXHR) {
        let successMessage = form.data("success-message");
        let command = form.find("#Command").val();

        if (command && form.data("success-message-" + command)) {
            successMessage = form.data("success-message-" + command);
        }

        if (jqXHR.getResponseHeader("X-IsValid") === "true" && successMessage) {
            toastr.success(successMessage);
        }
    }

    function displayWarningMessages(jqXHR) {
        if (jqXHR.getResponseHeader("X-Warnings")) {
            $.each(decodeURIComponent(jqXHR.getResponseHeader("X-Warnings")).split("|"), function () {
                toastr.warning(this);
            });
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
                method: this.method,
                data: form.serialize(),
                success: function (result, status, jqXHR) {
                    if (!redirectUnauthenticatedRequest(jqXHR)) {
                        displaySuccessMessage(form, jqXHR);
                        displayWarningMessages(jqXHR);
                        displayResult(panel, result);
                        callAjaxCallbacks();
                    }
                },
                error: function () {
                    toastr.error("An error occurred processing data; please try again.");
                    submitButtons.prop('disabled', false);
                }
            });
        }

        return false;
    });
});