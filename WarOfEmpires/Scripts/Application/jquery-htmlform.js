$(function () {
    $('body').on('submit', 'form.html-only', function () {
        let submitButtons = $(this).find('button[type="submit"]');

        if ($(this).valid()) {
            submitButtons.prop('disabled', true);
        }

        return true;
    });
});