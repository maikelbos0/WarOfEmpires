$.validator.addMethod("maxfilesize", function (value, element, params) {
    if (element.files && element.files[0]) {
        return element.files[0].size <= params.size;
    }

    return true;
});

$.validator.unobtrusive.adapters.add("maxfilesize", ["size"], function (options) {
    options.rules["maxfilesize"] = {
        size: parseInt(options.params.size)
    };
    options.messages["maxfilesize"] = options.message;
});
