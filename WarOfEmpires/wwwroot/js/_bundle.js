toastr.options = {
    positionClass: "toast-bottom-right",
	timeOut: 3000
};
$.fn.datagridview.defaults.getFooterPlugins = function () {
    return [
        $.fn.datagridview.footerPlugins.prevNext,
        $.fn.datagridview.footerPlugins.displayFull,
        $.fn.datagridview.footerPlugins.displayBasic
    ];
}
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
            cell.append($('<a>', { title: value, href: base.detailUrl + '?id=' + dataRow.id }).text(value));
        }
        else {
            cell.text(value).attr('title', value);
        }
        
        if (dataRow.isRead === false) {
            cell.addClass("fw-bold");
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
let AjaxManager = {
    onSuccess: []
}

$(function () {
    function displaySuccessMessage(form, jqXHR) {
        let successMessage = form.data("success-message");
        let command = form.find("#Command").val();

        if (command && form.data("success-message-" + command)) {
            successMessage = form.data("success-message-" + command);
        }

        if (jqXHR.getResponseHeader("X-IsValid") === "true" && successMessage) {
            toastr.success(successMessage);
        }
    }

    function displayWarningMessages(jqXHR) {
        if (jqXHR.getResponseHeader("X-Warnings")) {
            $.each(decodeURIComponent(jqXHR.getResponseHeader("X-Warnings")).split("|"), function () {
                toastr.warning(this);
            });
        }
    }

    function displayResult(panel, result) {
        panel.html(result);

        // Since the page won't be refreshed we try to find the title in the new content
        let title = panel.find('h2').text();

        if (title) {
            document.title = title + ' - War of Empires';
        }
    }

    function callAjaxCallbacks() {
        $.each(AjaxManager.onSuccess, function (i, func) {
            func();
        });
    }

    $('body').on('submit', 'form:not(.html-only):not(.search-form)', function () {        
        let form = $(this);
        let panel = form.closest('#main-content, .partial-content');
        let submitButtons = form.find('button[type="submit"]');

        if (panel.length === 0) {
            toastr.error("An error occurred locating content panel; please contact support to resolve this issue.");
        }
        else if (form.valid()) {
            submitButtons.prop('disabled', true);

            $.ajax({
                url: this.action,
                method: this.method,
                data: form.serialize(),
                success: function (result, _, jqXHR) {
                    displaySuccessMessage(form, jqXHR);
                    displayWarningMessages(jqXHR);
                    displayResult(panel, result);
                    callAjaxCallbacks();
                },
                error: function (jqXHR) {
                    if (jqXHR.status == 401) {
                        // Don't reload in case the current request is a POST
                        window.location.assign(window.location.href);
                    }
                    else {
                        toastr.error("An error occurred processing data; please try again.");
                        submitButtons.prop('disabled', false);
                    }
                }
            });
        }

        return false;
    });
});
$(function () {
    $('body').on('click', 'button[type="submit"]', function () {
        let commandField = $(this).closest('form').find('input#Command');

        if (commandField.length > 0) {
            commandField.val($(this).val());
        }
    })
});
$(function () {
    $('body').on('submit', 'form.html-only', function () {
        let submitButtons = $(this).find('button[type="submit"]');

        if ($(this).valid()) {
            submitButtons.prop('disabled', true);
        }

        return true;
    });
});
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
let PasswordStrength = {
    get: function (password) {
        if (password === null || password === undefined || password.toString().trim() === '') {
            return 0;
        }

        password = password.toString().trim();

        let score = 0;

        if (PasswordStrength.hasLowerCaseLetters(password)) { score++; }
        if (PasswordStrength.hasUpperCaseLetters(password)) { score++; }
        if (PasswordStrength.hasDigits(password)) { score++; }
        if (PasswordStrength.hasSpecialCharacters(password)) { score++; }

        if (password.length < 6) {
            return Math.min(score, 1);
        }
        else if (password.length < 8) {
            return Math.min(score, 2);
        }
        else if (password.length < 10) {
            return Math.min(score, 3);
        }
        else {
            return score;
        }
    },

    hasLowerCaseLetters: function (password) {
        for (var i = 0; i < password.length; i++) {
            let c = password.charAt(i);

            if (c === c.toLowerCase() && c !== c.toUpperCase()) {
                return true;
            }
        }

        return false;
    },

    hasUpperCaseLetters: function (password) {
        for (var i = 0; i < password.length; i++) {
            let c = password.charAt(i);

            if (c !== c.toLowerCase() && c === c.toUpperCase()) {
                return true;
            }
        }

        return false;
    },

    hasDigits: function (password) {
        return /\d/.test(password);
    },

    hasSpecialCharacters: function (password) {
        for (var i = 0; i < password.length; i++) {
            let c = password.charAt(i);

            if (c === c.toLowerCase() && c === c.toUpperCase() && !/\d/.test(c)) {
                return true;
            }
        }

        return false;
    }
}

$(function () {
    $('input.password-strength').each(function () {
        let base = $(this);
        let meter = $('<div>', { class: 'small site-password-meter' }).hide();

        base.after(meter);

        base.on('keyup', function () {
            switch (PasswordStrength.get(base.val())) {
                case 0:
                    meter.hide();
                    break;
                case 1:
                case 2:
                    meter.removeClass('site-password-meter-2 site-password-meter-3');
                    meter.text('Weak');
                    meter.addClass('site-password-meter-1');
                    meter.show();
                    break;
                case 3:
                    meter.removeClass('site-password-meter-1 site-password-meter-3');
                    meter.text('Moderate');
                    meter.addClass('site-password-meter-2');
                    meter.show();
                    break;
                case 4:
                    meter.removeClass('site-password-meter-1 site-password-meter-2');
                    meter.text('Strong');
                    meter.addClass('site-password-meter-3');
                    meter.show();
                    break;
            }

        });
    });
});
$(function () {
    function loadPanelContent(panel) {
        let url = panel.data('partial-url');

        if (url) {
            $.ajax({
                url: url,
                method: "GET",
                cache: false,
                success: function (result) {
                    panel.html(result);
                },
                error: function () {
                    toastr.error("An error occurred loading partial content; please refresh the page for accurate values.");
                }
            });
        }
        else {
            toastr.error("An error occurred determining content url; please contact support to resolve this issue.");
        }
    }

    $('.partial-content').each(function () {
        let panel = $(this);

        loadPanelContent(panel);
    });

    AjaxManager.onSuccess.push(function () {
        $('.partial-content[data-partial-ajax-refresh]').each(function () {
            let panel = $(this);

            loadPanelContent(panel);
        });
    });
});