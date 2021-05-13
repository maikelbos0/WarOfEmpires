let NotificationManager = {
    showNotifications: false,
    url: null,

    refresh: function () {
        if (!$("#empire-resources").length) {
            return;
        }
        
        $.ajax({
            url: NotificationManager.url,
            method: "POST",
            success: function (result) {
                $('#navbar-message-dropdown').toggleClass("notify", result.hasNewInvites || result.hasNewMessages);
                $('#invite-link').toggleClass("notify", result.hasNewInvites);
                $('#message-link').toggleClass("notify", result.hasNewMessages);

                $('#navbar-attack-dropdown, #attack-link').toggleClass("notify", result.hasNewAttacks);

                $('#navbar-market-dropdown, #market-link').toggleClass("notify", result.hasNewMarketSales);

                $('#navbar-empire-dropdown').toggleClass("notify", result.hasHousingShortage || result.hasUpkeepShortage || result.hasSoldierShortage);
                $('#empire-buildings-link').toggleClass("notify", result.hasHousingShortage);
                $('#worker-link').toggleClass("notify", result.hasUpkeepShortage);
                $('#troops-link').toggleClass("notify", result.hasSoldierShortage);
            },
            error: function () {
                toastr.error("An error occurred loading notitications; please refresh the page for accurate values.");
            }
        });
    }
}

$(function () {
    NotificationManager.refresh();
    AjaxManager.onSuccess.push(NotificationManager.refresh);
});