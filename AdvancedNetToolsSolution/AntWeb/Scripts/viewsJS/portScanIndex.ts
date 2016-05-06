$("#btnPortScan").click(function btnPingClick(e) {
    e.preventDefault();

    var l = $("#btnPortScan").ladda();

    l.ladda('start');
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