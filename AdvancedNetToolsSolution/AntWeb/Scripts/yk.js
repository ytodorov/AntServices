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

    //var isMobile = false; //initiate as false
    //// device detection
    //if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
    //    || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4)));

    //isMobile = true;

    ////Това скрива лявото меню като се зареди началната страница
    //var isMenuVisible = $.cookie('isMenuVisible');

    var isMenuVisible = $.cookie('isMenuVisible');
    debugger;
    if (isMenuVisible != null)
    {
        var $body = $("body");
        if (isMenuVisible !== "false")
        {
            $body.removeClass("hide-sidebar");
        }
        else
        {
            $body.addClass("hide-sidebar");
        }
    }

    $(".header-link").click(function myfunction() {
        debugger;
        // скрито ли е менюто
        var isMenuVisible = $("body.hide-sidebar").length == 0;
        $.cookie('isMenuVisible', isMenuVisible);
    })

    //if (!isMobile) {

    //    $(".header-link").click();
        
    //}
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
    $(".pdfExportPage").click(function () {
        // Тук скриваме и след това показваме някои части, които пречат на хубаво генериран експорт
        $("#divFooter").hide();
        $(".addthis_sharing_toolbox").hide();
        // Convert the DOM element to a drawing using kendo.drawing.drawDOM
        kendo.drawing.drawDOM($("#divBody"))
        .then(function (group) {
            // Render the result as a PDF file
            return kendo.drawing.exportPDF(group, {
                paperSize: "auto",
                margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
            });
        })
        .done(function (data) {
            // Save the PDF file
            debugger;

            kendo.saveAs({
                dataURI: data,
                fileName: "ToolsForNet" + new Date().getDate() + ".pdf",
                proxyURL: window.location.origin + "/export/pdf"
            });
            $("#divFooter").show();
            $(".addthis_sharing_toolbox").show();
        });

    });

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
        alert(message);
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
            var visible = vis(); // gives current state
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

