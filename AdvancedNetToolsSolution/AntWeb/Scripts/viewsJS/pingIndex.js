﻿$("#btnPing").click(function btnPingClick(e) {
    e.preventDefault();

    var l = $("#btnPing").ladda();

    // Start loading
    l.ladda('start');


    var pb = $("#loadingProgressBar").data("kendoProgressBar");
    $("#loadingProgressBar").show();
    pb.value(0);

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
        url: "/ping/GenerateId",

        data: {
            ip: $("#ip").val(),
            showInHistory: showInHistory,
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