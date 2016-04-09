$("#btnPing").click(function btnPingClick(e) {
    e.preventDefault();

    var l = $("#btnPing").ladda();

    // Start loading
    l.ladda('start');


    $.ajax({
        method: "POST",
        url: "/ping/GenerateId",
        data: {
            ip: $("#ip").val(),
            packetsCount: $("#tbPacketsCount").val(),
            packetsSize: $("#tbPacketsSize").val(),
            delayBetweenPings: $("#tbDelayBetweenPings").val(),
            ttl: $("#tbTTL").val(),
            df: $('#cbDontFragment').is(':checked')
        },
        success: function f(id) {
            window.location.href = "ping?id=" + id;

        }
    })

});
$("#ip").keyup(function myfunction(eventData) {
    if (eventData.keyCode == 13) {
        $("#btnPing").click();
    }

});