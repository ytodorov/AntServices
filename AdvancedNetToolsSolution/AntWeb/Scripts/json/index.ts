function jsonBeautyClick() {
    var json = $("#tbJson").val();
    $.ajax({
        method: "POST",
        url: "/json/beautify",
        data: {
            json: json,

        },
        success: function f(cssMinified) {
            var $tbResult = $("#tbResult");
            $tbResult.val(cssMinified);
            debugger;
            var textArea: HTMLTextAreaElement = <HTMLTextAreaElement>$tbResult.get(0);

            var editorConfig: CodeMirror.EditorConfiguration = {
                mode: "javascript",
                lineNumbers: true,
                lineWrapping: true,
                extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"]
            };
            (<any>editorConfig).foldGutter = true;
            var editor = CodeMirror.fromTextArea(textArea,
                editorConfig
            );

            //CodeMirror.p
            //(<any>editor).foldCode(CodeMirror.Pos(13, 0));
            
        }
    });
}