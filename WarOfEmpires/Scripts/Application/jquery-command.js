$(function () {
    $('body').on('click', 'button[type="submit"]', function () {
        let commandField = $(this).closest('form').find('input#Command');

        if (commandField.length > 0) {
            commandField.val($(this).val());
        }
    })
});