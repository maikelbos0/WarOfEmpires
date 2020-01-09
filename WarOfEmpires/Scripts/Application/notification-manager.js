let NotificationManager = {
    url: null,

    refresh: function () {
        let messageElements = $('#navbar-message-dropdown, #message-link');
        let attackElements = $('#navbar-attack-dropdown, #attack-link');
        let empireElements = $('#navbar-empire-dropdown');
        let housingElements = $('#empire-buildings-link');
        let upkeepElements = $('#worker-link');
        let troopElements = $('#troops-link');

        if (messageElements.length > 0) {
            $.ajax({
                url: NotificationManager.url,
                method: "POST",
                success: function (result) {
                    messageElements.toggleClass("notify", result.HasNewMessages);
                    attackElements.toggleClass("notify", result.HasNewAttacks);
                    empireElements.toggleClass("notify", result.HasHousingShortage || result.HasUpkeepShortage || result.HasSoldierShortage);
                    housingElements.toggleClass("notify", result.HasHousingShortage);
                    upkeepElements.toggleClass("notify", result.HasUpkeepShortage);
                    troopElements.toggleClass("notify", result.HasSoldierShortage);
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