﻿$("#btnPortScan").click(function btnPingClick(e) {
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
            pb.value(0);
            clearInterval(interval);
        }
    }, 1000); 

    var showInHistory = $("#showInHistory").is(":checked");
    var wellKnownPorts = $("#wellKnownPorts").is(":checked");
    $.ajax({
        method: "POST",
        url: "/portscan/generateid",

        data: {
            ip: $("#ip").val(),
            showInHistory: showInHistory,
            wellKnownPorts: wellKnownPorts
        },
        success: function f(id) {
            var error = id.error;
            if (typeof error != 'undefined') {
           
                $("#loadingProgressBar").hide();
                var l = $("#btnPortScan").ladda();  

                // Start loading 
                l.ladda('stop');
                window.antGlobal.showNotification(error, "error");
            }
            else {
                window.location.href = "portscan?id=" + id;     
            }


            //window.location.href = "portscan?id=" + id;
        }
    })

});
$("#ip").keyup(function myfunction(eventData) {
    if (eventData.keyCode == 13) {
        $("#btnPortScan").click();
    }

});