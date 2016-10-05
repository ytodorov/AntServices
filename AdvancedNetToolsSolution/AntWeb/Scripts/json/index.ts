function jsonBeautyClick() {
    var json = $("#tbJson").val();
    $.ajax({
        method: "POST",
        url: "/json/beautify",
        data: {
            json: json,

        },
        success: function f(cssMinified) {
            $("#tbResult").val(cssMinified);
        }
    });
}