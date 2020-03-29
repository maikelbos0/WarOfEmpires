let GridManager = {
    initialize: function (selector, url, sortColumn, sortDescending) {
        var grid = $(selector);
        var populate = function (metaData) {
            $.ajax({
                method: "POST",
                url: url,
                data: metaData,
                success: function (d) {
                    grid.datagridview(function () {
                        this.populate(d.metaData, d.data);
                    });
                },
                error: function () {
                    toastr.error("An error occurred loading data; please contact support to resolve this issue.");
                }
            });
        }

        grid.on('datagridview.sorted datagridview.paged', function (event, metaData) {
            populate(metaData);
        });

        populate(new DataGridViewMetaData(sortColumn, sortDescending, 0, 25, 0));
    }
}