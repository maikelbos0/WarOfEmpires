let GridManager = {
    initialize: function (selector, url, sortColumn, sortDescending, searchFormSelector) {        
        var searchForm = $(searchFormSelector);
        var grid = $(selector);
        var populate = function (metaData) {
            $.ajax({
                method: "POST",
                url: url,
                data: {
                    metaData,
                    search: searchForm.serializeArray().reduce(function (s, item) {
                        s[item.name] = item.value;
                        return s;
                    }, {})
                },
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

        searchForm.on('submit', function (event) {
            event.preventDefault();
            grid.datagridview(function () {
                populate(this.getMetaData());
            });
        });

        grid.on('datagridview.sorted datagridview.paged', function (event, metaData) {
            populate(metaData);
        });

        populate(new DataGridViewMetaData(sortColumn, sortDescending, 0, 25, 0));
    }
}