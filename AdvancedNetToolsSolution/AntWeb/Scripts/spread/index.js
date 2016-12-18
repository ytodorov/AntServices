function onSelectSpreadProcessing(ev) {
    var file = ev.files[0];
    if (!/.xlsx|.csv|.txt/.test(file.extension)) {
        alert("Only documents with *.xlsx, *.csv or *.txt extensions are accepted!");
        ev.preventDefault();
    }
}
//# sourceMappingURL=index.js.map