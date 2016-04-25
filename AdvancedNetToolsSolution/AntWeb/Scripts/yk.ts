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

    //Това скрива лявото меню като се зареди началната страница
    $(".header-link").click();
})

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

    var clipboard = new Clipboard('.fa-pencil-square');

    clipboard.on('success', function (e) {

        // Toastr options
        toastr.options = {
            "debug": false,
            "newestOnTop": false,
            "positionClass": "toast-top-center",
            "closeButton": true,
            "toastClass": "animated fadeInDown",
        };


        toastr.info('Copied to clipboard.');
        e.preventDefault();
        $("#iconSendEmail").click();

    });

    clipboard.on('error', function (e) {
    });






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
    // export pdf
    //$(".pdfExportPage").click(function () {
    //    // Тук скриваме и след това показваме някои части, които пречат на хубаво генериран експорт
    //    $("#divFooter").hide();
    //    $(".addthis_sharing_toolbox").hide();
    //    // Convert the DOM element to a drawing using kendo.drawing.drawDOM
    //    kendo.drawing.drawDOM($("#divBody"))
    //        .then(function (group) {
    //            // Render the result as a PDF file
    //            return kendo.drawing.exportPDF(group, {
    //                paperSize: "auto",
    //                margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
    //            });
    //        })
    //        .done(function (data) {
    //            // Save the PDF file
    //            debugger;

    //            kendo.saveAs({
    //                dataURI: data,
    //                fileName: "ToolsForNet" + new Date().getDate() + ".pdf",
    //                proxyURL: window.location.origin + "/export/pdf"
    //            });
    //            $("#divFooter").show();
    //            $(".addthis_sharing_toolbox").show();
    //        });

    //});

    //$("#pngExportPage").click(function () {
    //    // Convert the DOM element to a drawing using kendo.drawing.drawDOM
    //    kendo.drawing.drawDOM($("#divBody"))
    //    .then(function (group) {
    //        // Render the result as a PNG image
    //        return kendo.drawing.exportImage(group);
    //    })
    //    .done(function (data) {
    //        // Save the image file
    //        kendo.saveAs({
    //            dataURI: data,
    //            fileName: "ToolsForNet" + new Date().getDate() + ".png",
    //            proxyURL: "@Url.Action('Pdf', 'Export')"
    //        });
    //    });
    //});

    //$("#svgExportPage").click(function () {
    //    // Convert the DOM element to a drawing using kendo.drawing.drawDOM
    //    kendo.drawing.drawDOM($("#divBody"))
    //    .then(function (group) {
    //        // Render the result as a SVG document
    //        return kendo.drawing.exportSVG(group);
    //    })
    //    .done(function (data) {
    //        // Save the SVG document
    //        kendo.saveAs({
    //            dataURI: data,
    //            fileName: "ToolsForNet" + new Date().getDate() + ".svg",
    //            proxyURL: "@Url.Action('Pdf', 'Export')"
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
            var visible;
            //visible = vis(); // gives current state
            if (visible) {
                char++;
                var type = txt.substring(0, char);
                $ip.attr('placeholder', type + '|');
                typeIt();

                if (char == txtLen) {
                    $ip.attr('placeholder', $ip.attr('placeholder').slice(0, -1)) // remove the '|'
                    clearTimeout(timeOut);
                }
            }
            else {
                $ip.attr('placeholder', txt);
            }

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

