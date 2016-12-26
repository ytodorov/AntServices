$("#btnPortScan").click(function btnPingClick(e) {
    e.preventDefault();

    var l = $("#btnPortScan").ladda();

    l.ladda('start');


    var pb = $("#loadingProgressBar").data("kendoProgressBar");
    if (typeof pb != 'undefined') {
        pb.value(0);
        $("#loadingProgressBar").show();


        var interval = setInterval(function () {
            if (pb.value() < 100) {
                pb.value(pb.value() + 1);
            } else {
                pb.value(0);
                //clearInterval(interval);
            }
        }, 1000);
    }

    var showInHistory = $("#showInHistory").is(":checked");
    var wellKnown1000 = $("#wellKnown1000").is(":checked");
    var wellKnown = $("#wellKnown").is(":checked");
    var allPorts = $("#allPorts").is(":checked");

    var ip = $("#ip").val();
    $.ajax({
        method: "POST",
        url: "/portscan/generateid",
      
        data: {
            ip: ip,
            showInHistory: showInHistory,
            wellKnown1000: wellKnown1000,
            wellKnown: wellKnown,
            allPorts: allPorts
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
                window.location.href = "/portscan?url=" + ip + "&" + "id=" + id; 
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