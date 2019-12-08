let ResourceManager = {
    url: null,

    refresh: function () {
        let resourcePanel = $('#empire-resources');

        if (resourcePanel.length > 0) {
            $.ajax({
                url: ResourceManager.url,
                cache: false,
                success: function (result) {
                    resourcePanel.html(result);
                },
                error: function () {
                    toastr.error("An error occurred loading resource values; please refresh the page for accurate values.");
                }
            });
        }
    }
}

$(function () {
    ResourceManager.refresh();
    AjaxManager.onSuccess.push(ResourceManager.refresh);
});