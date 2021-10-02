$.validator.addMethod("fileextension", function (_, element, params) {
    if (element.files && element.files[0]) {
        return params.extensions.some(function (extension) {
            return element.files[0].name.toLowerCase().endsWith(extension);
        });
    }

    return true;
});

$.validator.unobtrusive.adapters.add("fileextension", ["extensions"], function (options) {
    options.rules["fileextension"] = {
        extensions: options.params.extensions.toLowerCase().split(",")
    };
    options.messages["fileextension"] = options.message;
});
