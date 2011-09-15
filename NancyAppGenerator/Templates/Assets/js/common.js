$(function () {
    $("*[data-confirm]").click(function (event) {
        if (!confirm($(this).attr("data-confirm"))) {
            event.preventDefault();
            event.stopImmediatePropagation();
        }
    });
    $("a[data-method]").click(function (event) {
        event.preventDefault();
        var url = $(this).attr("href");
        $("<form method='POST' style='display:none' action='" + url + "'><input type='hidden' name='_method'  value='" + $(this).attr("data-method") + "'></form>")
        .appendTo("body")
        .submit();

    });
});