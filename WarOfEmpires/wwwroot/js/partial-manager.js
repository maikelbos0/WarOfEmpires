$(function () {
    function loadPanelContent(panel) {
        let url = panel.data('partial-url');

        if (url) {
            $.ajax({
                url: url,
                method: "GET",
                cache: false,
                success: function (result) {
                    panel.html(result);
                },
                error: function () {
                    toastr.error("An error occurred loading partial content; please refresh the page for accurate values.");
                }
            });
        }
        else {
            toastr.error("An error occurred determining content url; please contact support to resolve this issue.");
        }
    }

    $('.partial-content').each(function () {
        let panel = $(this);

        loadPanelContent(panel);
    });

    AjaxManager.onSuccess.push(function () {
        $('.partial-content[data-partial-ajax-refresh]').each(function () {
            let panel = $(this);

            loadPanelContent(panel);
        });
    });
});