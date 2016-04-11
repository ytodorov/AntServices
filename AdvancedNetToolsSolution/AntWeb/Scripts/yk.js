$(window).resize(function () {
    //заради менюто което се премахва на малък екран   
    setTimeout(function resizeChart() {
        var $kendoCharts = $(".k-chart").data("kendoChart");
        if ($kendoCharts) {
            $kendoCharts.refresh();
        }

        resizeGrids()
    }
   , 10);
   
});

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

$(window).ready(function myfunction() {
    //заради менюто което се премахва на малък екран. Важно е да е около 200, защото лявото меню се скрива след определено време и то трябва да мине за да се преначертае графиката отново
    setTimeout(function resizeChart() {
        var $kendoCharts = $(".k-chart").data("kendoChart");
        if ($kendoCharts)
        {
            $kendoCharts.refresh();
        }
    }
   , 200);


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
       
          
            toastr.info('Info - Copied to clipboard.');
            e.preventDefault();
            $("#iconSendEmail").click();
    
    });

    clipboard.on('error', function (e) {
    });
})

$.ajaxSetup({
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        debugger;
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

        toastr.error('An error occured. It will be fixed soon!');
    }
});