let ResourceManager = {
    url: null,

    refresh: function () {
        $.ajax({
            url: ResourceManager.url,
            cache: false,
            success: function (result) {
                $('#empire-resources').html(result);
            },
            error: function () {
                toastr.error("An error occurred loading resource values; please refresh the page for accurate values.");
            }
        });
    }
}