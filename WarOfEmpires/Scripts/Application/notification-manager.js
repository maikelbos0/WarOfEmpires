let NotificationManager = {
    url: null,

    refresh: function () {
        let messageElements = $('#navbar-message-dropdown, #message-link');
        let attackElements = $('#navbar-attack-dropdown, #attack-link');
        let housingElements = $('#navbar-empire-dropdown, #empire-buildings-link');

        if (messageElements.length > 0) {
            $.ajax({
                url: NotificationManager.url,
                method: "POST",
                success: function (result) {
                    messageElements.toggleClass("notify", result.HasNewMessages);
                    attackElements.toggleClass("notify", result.HasNewAttacks);
                    housingElements.toggleClass("notify", result.HasHousingShortage);
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