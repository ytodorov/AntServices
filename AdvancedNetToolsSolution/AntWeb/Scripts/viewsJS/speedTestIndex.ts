$(document).ready(function () {


    $("#btnSpeedTest").click(function btnDownloadClick() {
        var numerictextbox = $("#tbDownloadLength").data("kendoNumericTextBox");
        // use markes so performance.getEntriesByName is unique
        $("#result").append("downloadStart" + '<br />');
        var currentDownloadLength = Math.pow(10, 6);    // 49
        // 10 ^ 7 = 10 MB
        var marker = new Date().getTime();


        $.ajax({
            async: false,
            url: "/speedtest/download?downloadLength=" + currentDownloadLength + "&marker=" + marker,
            beforeSend: function myfunction() {
                sessionStorage.setItem("start", window.performance.now().toString());

            },
            success: function f(stringWithTicks) {
                var start = sessionStorage.getItem("start");
                var now = parseInt(performance.now().toString());
                var diff = now - start;
                $("#result").append(diff + ' download of ' + currentDownloadLength + '<br />');

                //var performanceLastRequest = performance.getEntriesByName(location.origin + this.url);
                //var timeInMilliseconds = performanceLastRequest[0].responseEnd - performanceLastRequest[0].responseStart;
                //$("#result").append(timeInMilliseconds + ' download of ' + currentDownloadLength + '<br />');
            }
        })

        // use markes so performance.getEntriesByName is unique
        var marker = new Date().getTime();


        var uploadString = window.antGlobal.CreateRandomString(currentDownloadLength);
        $.ajax({
            async: false,
            method: "POST",
            url: "/speedtest/upload?marker=" + marker,
            data: { uploadString: uploadString },
            beforeSend: function myfunction() {
                sessionStorage.setItem("start", performance.now().toString());

            },
            success: function f(id) {
                var start = sessionStorage.getItem("start");
                var now = parseInt(performance.now().toString());
                var diff = now - start;
                $("#result").append(diff + ' upload of ' + currentDownloadLength + '<br />');
                //$("#result").append(' upload of ' + currentDownloadLength + '<br />');
                //var performanceLastRequest = performance.getEntriesByName(location.origin + this.url);
                //var timeInMilliseconds = performanceLastRequest[0].responseStart - performanceLastRequest[0].requestStart;
                //$("#result").append(timeInMilliseconds + ' upload of ' + currentDownloadLength + '<br />');
            }
        });

    });


});