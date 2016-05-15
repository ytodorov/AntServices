$("#btnPortScan").click(function btnPingClick(e) {
    e.preventDefault();

    var l = $("#btnPortScan").ladda();

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
        url: "/portscan/generateid",

        data: {
            ip: $("#ip").val(),
            showInHistory: showInHistory,
        },
        success: function f(id) {
            window.location.href = "portscan?id=" + id;
        }
    })

});
$("#ip").keyup(function myfunction(eventData) {
    if (eventData.keyCode == 13) {
        $("#btnPortScan").click();
    }

});