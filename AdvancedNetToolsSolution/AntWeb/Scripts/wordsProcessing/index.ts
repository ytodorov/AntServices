function onSelect(ev) {
    var file = ev.files[0];
    //clearHighlights();

    if (!/.docx|.rtf|.html|.txt/.test(file.extension)) {
        alert("Only documents with *.docx, *.rtf, *.html or *.txt extensions are accepted!");
        ev.preventDefault();
    }
  
}
