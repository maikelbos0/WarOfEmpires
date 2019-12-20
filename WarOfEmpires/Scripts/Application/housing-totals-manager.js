let HousingTotalsManager = {
    url: null,

    refresh: function () {
        let totalsPanel = $('#empire-housing-totals');

        if (totalsPanel.length > 0) {
            $.ajax({
                url: HousingTotalsManager.url,
                cache: false,
                success: function (result) {
                    totalsPanel.html(result);
                },
                error: function () {
                    toastr.error("An error occurred loading housing totals; please refresh the page for accurate values.");
                }
            });
        }
    }
}

$(function () {
    HousingTotalsManager.refresh();
    AjaxManager.onSuccess.push(HousingTotalsManager.refresh);
});