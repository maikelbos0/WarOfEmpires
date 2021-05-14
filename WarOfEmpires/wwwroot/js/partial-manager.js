$(function () {
    $('.partial-content').each(function () {
        var panel = $(this);
        var url = panel.data('partial-url');

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
    });
});