$(function () {
    $('.partial-content').each(function () {
        let panel = $(this);
        let url = panel.data('partial-url');

        if (url) {
            let load = function () {
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
            };

            load();
            
            if (panel.data('partial-ajax-refresh')) {
                AjaxManager.onSuccess.push(load);
            }
        }
        else {
            toastr.error("An error occurred determining content url; please contact support to resolve this issue.");
        }
    });
});