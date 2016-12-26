$("#btnTraceRoute").click(function btnTraceRoute(e) {
    e.preventDefault();

    var l = $("#btnTraceRoute").ladda();

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
    }, 1000);


    var showInHistory = $("#showInHistory").is(":checked");
    var ip = $("#ip").val();
    $.ajax({
        method: "POST",
        url: "/traceroute/generateid",

        data: {
            ip: ip,
            showInHistory: showInHistory,
        },
        success: function f(id) {
            var error = id.error;
            if (typeof error != 'undefined') {
                $("#loadingProgressBar").hide();
                var l = $("#btnTraceRoute").ladda();

                // Start loading
                l.ladda('stop');
                window.antGlobal.showNotification(error, "error");
            }
            else {
                window.location.href = "traceroute?url=" + ip + "&" + "id=" + id;
            }

        }
    })

});
$("#ip").keyup(function myfunction(eventData) {
    if (eventData.keyCode == 13) {
        $("#btnTraceRoute").click();
    }

});