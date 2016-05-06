$(window).resize(function () {
    //заради менюто което се премахва на малък екран   
    setTimeout(function () {
        resizeCharts();
        resizeGrids();
    }
        , 1);

});


function resizeCharts() {
    var $kendoCharts = $(".k-chart").data("kendoChart");
    if ($kendoCharts) {
        $kendoCharts.refresh();
    }
}

function resizeGrids() {
    var grids = $(".k-grid table");
    if (grids) {
        grids.each(function (i) {
            var $tbody = $(this).find("tbody").first();
            if (!$tbody) {
                return;
            }
            if ($tbody.width() > $(this).parent().parent().width()) {
                if (!$(this).attr("style")) {
                    $(this).attr('style', 'overflow-x: auto !important; display: block !important;');
                }
            }
            else {
                if ($(this).attr("style")) {
                    $(this).removeAttr("style");
                }
            }
        }
        );
    }
}

$(document).ready(function docReady() {

    var isMenuVisible = $.cookie('isMenuVisible');
    if (isMenuVisible != null) {
        var $body = $("body");
        if (isMenuVisible !== "false") {
            $body.removeClass("hide-sidebar");
        }
        else {
            $body.addClass("hide-sidebar");
        }
    }

    $(".header-link").click(function myfunction() {
        // скрито ли е менюто
        var isMenuVisible = $("body.hide-sidebar").length == 0;
        $.cookie('isMenuVisible', isMenuVisible);
    })


});

/* Back to top */
(function () {
    $("#back-top").hide();

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#back-top').fadeIn();
        } else {
            $('#back-top').fadeOut();
        }
    });

    $('#back-top a').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 600);
        return false;
    });
})();



$(window).ready(function myfunction() {
    //заради менюто което се премахва на малък екран. Важно е да е около 200, защото лявото меню се скрива след определено време и то трябва да мине за да се преначертае графиката отново
    setTimeout(function () {
        resizeCharts();
        resizeGrids();
    }
        , 200);

    $("div.hide-menu").click(function () {
        setTimeout(function () {
            resizeCharts();
            resizeGrids();
        }
            , 300); // важен е този timeout 300
    });

    //var clipboard = new Clipboard('.fa-pencil-square');

    //clipboard.on('success', function (e) {

    //    // Toastr options
    //    toastr.options = {
    //        "debug": false,
    //        "newestOnTop": false,
    //        "positionClass": "toast-top-center",
    //        "closeButton": true,
    //        "toastClass": "animated fadeInDown",
    //    };


    //    toastr.info('Copied to clipboard.');
    //    e.preventDefault();
    //    $("#iconSendEmail").click();

    //});

    //clipboard.on('error', function (e) {
    //});




    setTimeout(function pl() {
        sessionStorage.setItem("isWindowActive", "true");
        var $ip = $("#ip");
        if ($ip.length != 0) {
            var arrOfPlaceholders = [];
            arrOfPlaceholders[0] = "google.com";
            arrOfPlaceholders[1] = "www.yahoo.com";
            arrOfPlaceholders[2] = "http://www.facebook.com";
            arrOfPlaceholders[3] = "ftp://ftp.microsoft.com/";
            arrOfPlaceholders[4] = "https://weather.com/en-GB";
            arrOfPlaceholders[5] = "8.8.8.8";
            arrOfPlaceholders[6] = "172.217.3.238";
            arrOfPlaceholders[7] = "https://en.wikipedia.org/wiki/Main_Page";
            arrOfPlaceholders[8] = "https://www.youtube.com/";
            arrOfPlaceholders[9] = "134.170.188.232";
            arrOfPlaceholders[10] = "216.58.212.142";

            for (var i = 0; i < arrOfPlaceholders.length * 1000; i++) {
                setTimeout(animateIpPlaceholder, 5000 * i, arrOfPlaceholders[i % arrOfPlaceholders.length]);
            }

        }
    }, 1000); 
    // TODO: kendo.drawing gives an error
    // export pdf
    $(".pdfexportpage").click(function () {
        debugger;
        // тук скриваме и след това показваме някои части, които пречат на хубаво генериран експорт
        $("#divfooter").hide();
        $(".addthis_sharing_toolbox").hide();
        // convert the dom element to a drawing using kendo.drawing.drawdom
        kendo.drawing.drawDOM($("#divbody"), null)
            .then(function (group) {
                debugger;
                // render the result as a pdf file
                return kendo.drawing.exportPDF(group, {
                    paperSize: "auto",
                    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                });
            }) 
            .done(function (data) {
                // save the pdf file
                debugger;
                kendo.saveAs({
                    datauri: data,
                    filename: "toolsfornet" + new Date().getDate() + ".pdf",
                    proxyurl: window.location.origin + "/export/pdf"
                });
                $("#divfooter").show();
                $(".addthis_sharing_toolbox").show();
            });

    });

    //$("#pngexportpage").click(function () {
    //    // convert the dom element to a drawing using kendo.drawing.drawdom
    //    var draw = kendo.drawing;

    //    draw.drawDOM($("#divbody"))
    //    .then(function (group) {
    //        // render the result as a png image
    //        return draw.exportImage(group);
    //    })
    //    .done(function (data) {
    //        // save the image file
    //        kendo.saveAs({
    //            datauri: data,
    //            filename: "toolsfornet" + new Date().getDate() + ".png",
    //            proxyurl: "@url.action('pdf', 'export')"
    //        });
    //    });
    //});

    //$("#svgexportpage").click(function () {

    //    // convert the dom element to a drawing using kendo.drawing.drawdom
    //    var draw = kendo.drawing;
    //    draw.drawDOM($("#divbody"))
    //    .then(function (group) {
    //        // render the result as a svg document
    //        return draw.exportSVG(group);
    //    })
    //    .done(function (data) {
    //        // save the svg document
    //        kendo.saveAs({
    //            datauri: data,
    //            filename: "toolsfornet" + new Date().getDate() + ".svg",
    //            proxyurl: "@url.action('pdf', 'export')"
    //        });
    //    });
    //});


})

$.ajaxSetup({
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        // Toastr options
        toastr.options = {
            "debug": false,
            "newestOnTop": false,
            "positionClass": "toast-top-center",
            "closeButton": true,
            "toastClass": "animated fadeInDown",
        };
        var laddaButtons = $("[data-loading]");
        laddaButtons.ladda('stop');

        toastr.error('An error occured! Try again.');
    },
    success: function myfunction(event, xhr, settings) {
       // alert(message);
    }
});

function animateIpPlaceholder(txt) {

    var timeOut;
    var txtLen = txt.length;
    var char = 0;
    var $ip = $("#ip");
    $ip.attr('placeholder', '|');
    (function typeIt() {
        var humanize = Math.round(Math.random() * (200 - 30)) + 30;
        timeOut = setTimeout(function () {
            //visible = vis(); // gives current state
            //if (visible) {
                char++;
                var type = txt.substring(0, char);
                $ip.attr('placeholder', type + '|');
            
                var visible: boolean = sessionStorage.getItem("isWindowActive") === "true";
                debugger;
                if (visible) {
                    typeIt();
                }
                else {
                    $ip.attr('placeholder', txt);
                }

                if (char == txtLen) {
                    $ip.attr('placeholder', $ip.attr('placeholder').slice(0, -1)) // remove the '|'
                    clearTimeout(timeOut);
                }
            //}
            //else {
            //    $ip.attr('placeholder', txt);
            //}

        },
            humanize);
    })();
}

var vis = (function () {
    var stateKey, eventKey, keys = {
        hidden: "visibilitychange",
        webkitHidden: "webkitvisibilitychange",
        mozHidden: "mozvisibilitychange",
        msHidden: "msvisibilitychange"
    };
    for (stateKey in keys) {
        if (stateKey in document) {
            eventKey = keys[stateKey];
            break;
        }
    }
    return function (c) {
        if (c) document.addEventListener(eventKey, c);
        return !document[stateKey];
    }
})();

$(window).on("blur focus", function (e) {
    var prevType = $(this).data("prevType");

    if (prevType != e.type) {   //  reduce double fire issues
        switch (e.type) {
            case "blur":
                sessionStorage.setItem("isWindowActive", "false");
                break;
            case "focus":
                sessionStorage.setItem("isWindowActive", "true");
                break;
        }
    }

    $(this).data("prevType", e.type);
})