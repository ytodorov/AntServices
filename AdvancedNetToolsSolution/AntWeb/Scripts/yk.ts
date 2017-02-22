declare var FB: any;
declare var twttr: any;
declare var gapi: any;


//window.cookieconsent_options = { "message": "This website uses cookies to ensure you get the best experience on our website", "dismiss": "Got it!", "learnMore": "More info", "link": null, "theme": "light-top" };
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

$(window).load(function () {
    setTimeout(function asd() {

        var theDiv = $("#outerHeader");
        //theDiv.html('<div id="divInnerSocial" style="display:none"  class="normalheader transition animate-panel animated fadeIn small-header right">                <div class="hpanel">                    <div id="divSocial" class="panel-body">                        <div class="pull-left fb-like"                             data-href="https://www.facebook.com/toolsfornet"                             data-colorscheme="light"                             data-layout="button_count"                             data-action="like"                             data-size="small"                             data-show-faces="false"                             data-share="true">                        </div>                        &nbsp;<a class="pull-right twitter-share-button"                           href="https://twitter.com/intent/tweet?url=https://toolsfornet.com"                           data-show-screen-name="false">                            Tweet                        </a>                        <div id="divPlusOne" class="pull-left g-plusone" data-size="medium" data-annotation="none" data-width="200"></div>                        <a id="aPinterest" style="vertical-align:top !important" data-pin-do="buttonBookmark" data-pin-save="true" href="https://www.pinterest.com/pin/create/button/"></a>                        <a class="tumblr-share-button" href="https://www.tumblr.com/share"></a>                        <script id="tumblr-js" async src="https://assets.tumblr.com/share-button.js"></script>                        <script src="https://platform.linkedin.com/in.js" type="text/javascript"> lang: en_US                        </script>                        <script type="IN/Share" data-url="https://toolsfornet.com" data-counter="right">                        </script>                    </div>                              </div>            </div>');
        theDiv.html('<div id="divInnerSocial" style="display:none"  class="normalheader transition animate-panel animated fadeIn small-header right"><div class="hpanel"><div id="divSocial" class="panel-body"><div class="pull-left fb-like" data-href="https://www.facebook.com/toolsfornet" data-colorscheme="light" data-layout="button_count" data-action="like" data-size="small" data-show-faces="false" data-share="true"> </div> &nbsp;<a class="pull-right twitter-share-button" href="https://twitter.com/intent/tweet?url=https://toolsfornet.com" data-show-screen-name="false"> Tweet </a></div>');

        FB.XFBML.parse(document.getElementById('divSocial'));
        twttr.widgets.load();

        //var w: any = window;
     
        //w.parsePins(document.getElementById('aPinterest'));

        //w.parsePins();

        $("#divInnerSocial").show("slow");

    }, 30);

});

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
    //$("#trustLogoLi").append('<script type="text/javascript" >TrustLogo("https://toolsfornet.com/content/img/comodo/comodo_secure_seal_76x26_transp.png", "CL1", "none");</script>');

    var intervalCounter = 0;
    var interval = setInterval(function alignGoogle() {

        var g = $("span[data-pin-log]");
        var c = g.length;
        g.each(function () {
            this.style.setProperty('vertical-align', 'top', 'important');
        });

        intervalCounter++;
        if (intervalCounter > 5) {
            clearInterval(interval);
        }
    }, 500);

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

        $.ajax(
            {
                method: 'POST',
                url: '/home/getip'
            }
        ).done(function f(ip: string) {
            $("#aUserHostAddress").attr('href', 'https://toolsfornet.com/iplocation?ip=' + ip);
            $("#pUserHostAddress").html('Hello ' + ip);
            });


    }
        , 200);

    setTimeout(function f() {

        var script = document.createElement('script')
        script.src = '//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-571b2acff4b44bbf';
        document.documentElement.firstChild.appendChild(script)
    }, 1000);

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




    //setTimeout(function pl() {
    //    sessionStorage.setItem("isWindowActive", "true");
    //    var $ip = $("#ip");
    //    if ($ip.length != 0) {
    //        var arrOfPlaceholders = [];
    //        arrOfPlaceholders[0] = "google.com";
    //        arrOfPlaceholders[1] = "www.yahoo.com";
    //        arrOfPlaceholders[2] = "http://www.facebook.com";
    //        arrOfPlaceholders[3] = "ftp://ftp.microsoft.com/";
    //        arrOfPlaceholders[4] = "https://weather.com/en-GB";
    //        arrOfPlaceholders[5] = "8.8.8.8";
    //        arrOfPlaceholders[6] = "172.217.3.238";
    //        arrOfPlaceholders[7] = "https://en.wikipedia.org/wiki/Main_Page";
    //        arrOfPlaceholders[8] = "https://www.youtube.com/";
    //        arrOfPlaceholders[9] = "134.170.188.232";
    //        arrOfPlaceholders[10] = "216.58.212.142";

    //        for (var i = 0; i < arrOfPlaceholders.length * 1000; i++) {
    //            setTimeout(animateIpPlaceholder, 5000 * i, arrOfPlaceholders[i % arrOfPlaceholders.length]);
    //        }

    //    }
    //}, 1000); 
    // TODO: kendo.drawing gives an error
    // export pdf
    $(".pdfexportpage").click(function () {
        // тук скриваме и след това показваме някои части, които пречат на хубаво генериран експорт
        $("#divfooter").hide();
        $("#divMap").remove();
        $(".addthis_sharing_toolbox").hide();
        // convert the dom element to a drawing using kendo.drawing.drawdom
        kendo.drawing.drawDOM($("#divbody"), null)
            .then(function (group) {
                // render the result as a pdf file
                return kendo.drawing.exportPDF(group, {
                    paperSize: "auto",
                    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                });
            })
            .done(function (data) {
                // save the pdf file
                kendo.saveAs({
                    datauri: data,
                    filename: "toolsfornet" + new Date().getDate() + ".pdf",
                    proxyurl: window.location.origin + "/export/pdf"
                });
                $("#divfooter").show();
                // $("#divMap").show();
                $(".addthis_sharing_toolbox").show();
            });

    });



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
        window.antGlobal.showNotification('An error occured! Try again.', "error");
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
        if (document) {
            return !document[stateKey];
        }
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
});

$(window).on("error", function (e: any) {
    if (typeof e.originalEvent.error != 'undefined' && e.originalEvent.error != null) {
        window.antGlobal.showNotification(e.originalEvent.error.message, "error");
    }
    else {
        window.antGlobal.showNotification(e.originalEvent, "error");
    }


});