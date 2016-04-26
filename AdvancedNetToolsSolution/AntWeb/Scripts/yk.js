$(window).resize(function () {
    //заради менюто което се премахва на малък екран   
    setTimeout(function () {
        resizeCharts();
        resizeGrids();
    }, 1);
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
        });
    }
}
$(document).ready(function docReady() {
    //Това скрива лявото меню като се зареди началната страница
    $(".header-link").click();
});
$(window).ready(function myfunction() {
    //заради менюто което се премахва на малък екран. Важно е да е около 200, защото лявото меню се скрива след определено време и то трябва да мине за да се преначертае графиката отново
    setTimeout(function () {
        resizeCharts();
        resizeGrids();
    }, 200);
    $("div.hide-menu").click(function () {
        setTimeout(function () {
            resizeCharts();
            resizeGrids();
        }, 300); // важен е този timeout 300
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
    // TODO: kendo.drawing gives an error
    // export pdf
    $(".pdfexportpage").click(function () {
        // тук скриваме и след това показваме някои части, които пречат на хубаво генериран експорт
        $("#divfooter").hide();
        $(".addthis_sharing_toolbox").hide();
        // convert the dom element to a drawing using kendo.drawing.drawdom
        kendo.drawing.drawdom($("#divbody"))
            .then(function (group) {
                // render the result as a pdf file
                return kendo.drawing.exportpdf(group, {
                    papersize: "auto",
                    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                });
            })
            .done(function (data) {
                // save the pdf file
                debugger;
                kendo.saveas({
                    datauri: data,
                    filename: "toolsfornet" + new date().getdate() + ".pdf",
                    proxyurl: window.location.origin + "/export/pdf"
                });
                $("#divfooter").show();
                $(".addthis_sharing_toolbox").show();
            });
    });
    $("#pngexportpage").click(function () {
        // convert the dom element to a drawing using kendo.drawing.drawdom
        kendo.drawing.drawdom($("#divbody"))
        .then(function (group) {
            // render the result as a png image
            return kendo.drawing.exportimage(group);
        })
        .done(function (data) {
            // save the image file
            kendo.saveas({
                datauri: data,
                filename: "toolsfornet" + new date().getdate() + ".png",
                proxyurl: "@url.action('pdf', 'export')"
            });
        });
    });
    $("#svgexportpage").click(function () {
        // convert the dom element to a drawing using kendo.drawing.drawdom
        kendo.drawing.drawdom($("#divbody"))
        .then(function (group) {
            // render the result as a svg document
            return kendo.drawing.exportsvg(group);
        })
        .done(function (data) {
            // save the svg document
            kendo.saveas({
                datauri: data,
                filename: "toolsfornet" + new date().getdate() + ".svg",
                proxyurl: "@url.action('pdf', 'export')"
            });
        });
    });
});
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
                    $ip.attr('placeholder', $ip.attr('placeholder').slice(0, -1)); // remove the '|'
                    clearTimeout(timeOut);
                }
            }
            else {
                $ip.attr('placeholder', txt);
            }
        }, humanize);
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
        if (c)
            document.addEventListener(eventKey, c);
        return !document[stateKey];
    };
})();
//# sourceMappingURL=yk.js.map