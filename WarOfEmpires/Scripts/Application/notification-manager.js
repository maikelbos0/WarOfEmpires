let NotificationManager = {
    url: null,
    messageLink: $('#message-link'),

    refresh: function () {
        let messageLink = $('#message-link');

        if (messageLink.length > 0) {
            $.ajax({
                url: NotificationManager.url,
                method: "POST",
                success: function (result) {
                    messageLink.toggleClass("notify", result.HasNewMessages);
                },
                error: function () {
                    toastr.error("An error occurred loading notitications; please refresh the page for accurate values.");
                }
            });
        }
    }
}

$(function () {
    NotificationManager.refresh();
    AjaxManager.onSuccess.push(NotificationManager.refresh);
});