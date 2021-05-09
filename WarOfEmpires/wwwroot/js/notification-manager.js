let NotificationManager = {
    showNotifications: false,
    url: null,

    refresh: function () {
        if (!NotificationManager.showNotifications) {
            return;
        }

        $.ajax({
            url: NotificationManager.url,
            method: "POST",
            success: function (result) {
                $('#navbar-message-dropdown').toggleClass("notify", result.HasNewInvites || result.HasNewMessages);
                $('#invite-link').toggleClass("notify", result.HasNewInvites);
                $('#message-link').toggleClass("notify", result.HasNewMessages);

                $('#navbar-attack-dropdown, #attack-link').toggleClass("notify", result.HasNewAttacks);

                $('#navbar-market-dropdown, #market-link').toggleClass("notify", result.HasNewMarketSales);

                $('#navbar-empire-dropdown').toggleClass("notify", result.HasHousingShortage || result.HasUpkeepShortage || result.HasSoldierShortage);
                $('#empire-buildings-link').toggleClass("notify", result.HasHousingShortage);
                $('#worker-link').toggleClass("notify", result.HasUpkeepShortage);
                $('#troops-link').toggleClass("notify", result.HasSoldierShortage);
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