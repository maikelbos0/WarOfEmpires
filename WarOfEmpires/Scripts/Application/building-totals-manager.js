let BuildingTotalsManager = {
    url: null,

    refresh: function () {
        let totalsPanel = $('#empire-building-totals');

        if (totalsPanel.length > 0) {
            $.ajax({
                url: BuildingTotalsManager.url,
                cache: false,
                success: function (result) {
                    totalsPanel.html(result);
                },
                error: function () {
                    toastr.error("An error occurred loading building totals; please refresh the page for accurate values.");
                }
            });
        }
    }
}

$(function () {
    BuildingTotalsManager.refresh();
    AjaxManager.onSuccess.push(BuildingTotalsManager.refresh);
});