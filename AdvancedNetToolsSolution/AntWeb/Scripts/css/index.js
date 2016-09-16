function cssMinifyClick() {
    var css = $("#cssInput").val();
    $.ajax({
        method: "POST",
        url: "/css/minify",
        data: {
            css: css,
        },
        success: function f(cssMinified) {
            $("#cssOutput").val(cssMinified);
        }
    });
}
//# sourceMappingURL=index.js.map