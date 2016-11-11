$(document).ready(function () {
    $("#btnSpeedTest").click(function btnDownloadClick() {
        downloadFromOneLocation("");
        uploadFormOneLocation("");
        //var numerictextbox = $("#tbDownloadLength").data("kendoNumericTextBox");
        // use markes so performance.getEntriesByName is unique
        //$("#result").append("downloadStart" + '<br />');
        //var currentDownloadLength = Math.pow(10, 6);    // 49
        // 10 ^ 7 = 10 MB
        //var marker = new Date().getTime();
        //$.ajax({
        //    async: true,
        //    url: "/speedtest/download?downloadLength=" + currentDownloadLength + "&marker=" + marker,
        //    beforeSend: function beforeSendFunction() {
        //        sessionStorage.setItem("start", window.performance.now().toString());
        //    },
        //    success: function successFunction() {
        //        var start = sessionStorage.getItem("start");
        //        var now = parseInt(performance.now().toString());
        //        var diff = now - start;
        //        $("#result").append(diff + ' download of ' + currentDownloadLength + '<br />');
        //    }
        //})
        // use markes so performance.getEntriesByName is unique
        //var marker = new Date().getTime();
        //var uploadString = window.antGlobal.CreateRandomString(currentDownloadLength);
        //$.ajax({
        //    async: false,
        //    method: "POST",
        //    url: "/speedtest/upload?marker=" + marker,
        //    data: { uploadString: uploadString },
        //    beforeSend: function myfunction() {
        //        sessionStorage.setItem("start", performance.now().toString());
        //    },
        //    success: function f(id) {
        //        var start = sessionStorage.getItem("start");
        //        var now = parseInt(performance.now().toString());
        //        var diff = now - start;
        //        $("#result").append(diff + ' upload of ' + currentDownloadLength + '<br />');
        //    }
        //});
    });
});
function downloadFromOneLocation(url) {
    var warmUpDownloadLength = Math.pow(10, 3);
    var currentDownloadLength = Math.pow(10, 6); // 49
    $.ajax({
        url: "/home/download?downloadlength=" + warmUpDownloadLength,
        success: function successFunction() {
            $.ajax({
                url: "/home/download?downloadlength=" + currentDownloadLength,
                beforeSend: function beforeSendFunction() {
                    sessionStorage.setItem("start", window.performance.now().toString());
                },
                success: function successFunction() {
                    var start = sessionStorage.getItem("start");
                    var now = parseInt(performance.now().toString());
                    var diff = now - start;
                    $("#result").append(diff + ' download of ' + currentDownloadLength + '<br />');
                }
            });
        }
    });
}
function uploadFormOneLocation(url) {
    var warmUpDownloadLength = Math.pow(10, 3);
    var currentDownloadLength = Math.pow(10, 6); // 49
    var uploadStringWarmUp = window.antGlobal.CreateRandomString(warmUpDownloadLength);
    var uploadString = window.antGlobal.CreateRandomString(currentDownloadLength);
    $.ajax({
        method: "POST",
        url: "/home/upload",
        data: { uploadString: uploadStringWarmUp },
        success: function f() {
            $.ajax({
                method: "POST",
                url: "/home/upload",
                data: { uploadString: uploadString },
                beforeSend: function myfunction() {
                    sessionStorage.setItem("start", performance.now().toString());
                },
                success: function f() {
                    var start = sessionStorage.getItem("start");
                    var now = parseInt(performance.now().toString());
                    var diff = now - start;
                    $("#result").append(diff + ' upload of ' + warmUpDownloadLength + '<br />');
                }
            });
        }
    });
}
//# sourceMappingURL=speedTestIndex.js.map