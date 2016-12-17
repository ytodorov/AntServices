$(document).ready(function readyPing() {
    $("#btnBarcode").click(function btnPingClick(e) {
        e.preventDefault();



        var l = $("#btnBarcode").ladda();

        // Start loading
        l.ladda('start');


        var pb = $("#loadingProgressBar").data("kendoProgressBar");
        pb.value(0);
        $("#loadingProgressBar").show();


        var interval = setInterval(function () {
            if (pb.value() < 100) {
                pb.value(pb.value() + 1);
            } else {
                clearInterval(interval);
            }
        }, 100);


        var showInHistory = $("#showInHistory").is(":checked");

        $.ajax({
            method: "POST",
            url: "/barcode/generateid",

            data: {
                ip: $("#ip").val(),
                showInHistory: showInHistory,
            },
            success: function f(id) {
                var error = id.error;
                if (typeof error != 'undefined') {
                    $("#loadingProgressBar").hide();
                    var l = $("#btnBarcode").ladda();

                    // Start loading
                    l.ladda('stop');
                    window.antGlobal.showNotification(error, "error");
                }
                else {
                    window.location.href = "barcode?id=" + id;
                }

            }
        })

    });
    $("#ip").keyup(function myfunction(eventData) {
        if (eventData.keyCode == 13) {
            $("#btnBarcode").click();
        }

    });
});