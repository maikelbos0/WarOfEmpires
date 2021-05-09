function Grid(id, dataUrl) {
    this.id = id;
    this.dataUrl = dataUrl;

    this.detailUrl = null;        
    this.sortColumn = null;
    this.sortDescending = false;
    this.searchFormId = null;

    let columns = [];
    let base = this;

    let populate = function (metaData) {
        let grid = $('#' + base.id);
        let searchForm = $('#' + base.searchFormId);

        $.ajax({
            method: "POST",
            url: base.dataUrl,
            data: {
                metaData,
                search: searchForm.serializeArray().reduce(function (s, item) {
                    s[item.name] = item.value;
                    return s;
                }, {})
            },
            success: function (d) {
                if (d && d.data && d.metaData) {
                    grid.datagridview(function () {
                        this.populate(d.metaData, d.data);
                    });
                }
                else if (typeof d === "string" && d.indexOf("Log in") > -1) {
                    window.location.reload();
                }
                else {
                    toastr.error("An error occurred loading data; please contact support to resolve this issue.");
                }
            },
            error: function () {
                toastr.error("An error occurred loading data; please contact support to resolve this issue.");
            }
        });
    }

    let renderer = function (cell, value, dataRow) {
        if (base.detailUrl) {
            cell.append($('<a>', { title: value, href: base.detailUrl + '?id=' + dataRow.Id }).text(value));
        }
        else {
            cell.text(value).attr('title', value);
        }

        if (dataRow.IsRead === false) {
            cell.addClass("font-weight-bold");
        }
    }

    this.addColumn = function (width, data, header, sortData, responsiveDisplayBehaviour) {
        let ResponsiveDisplayBehaviourClasses = {
            AlwaysVisible: null,
            HiddenFromSmall: "d-none d-md-block",
            HiddenFromMedium: "d-none d-lg-block",
            HiddenFromLarge: "d-none d-xl-block"
        };

        columns.push({
            width: width,
            data: data,
            header: header,
            sortData: sortData,
            renderer: renderer,
            class: ResponsiveDisplayBehaviourClasses[responsiveDisplayBehaviour]
        });
    }

    this.initialize = function () {
        let grid = $('#' + this.id);
        let searchForm = $('#' + this.searchFormId);

        grid.datagridview({
            columns
        });

        searchForm.on('submit', function (event) {
            event.preventDefault();
            grid.datagridview(function () {
                populate(this.getMetaData());
            });
        });

        grid.on('datagridview.sorted datagridview.paged', function (event, metaData) {
            populate(metaData);
        });

        populate(new DataGridViewMetaData(this.sortColumn, this.sortDescending, 0, 25, 0));
    }
}