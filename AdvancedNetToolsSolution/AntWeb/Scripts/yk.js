$(window).resize(function () {
    //заради менюто което се премахва на малък екран   
    setTimeout(function resizeChart() {
        var $kendoCharts = $(".k-chart").data("kendoChart");
        if ($kendoCharts) {
            $kendoCharts.refresh();
        }
    }
   , 10);
   
});

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
    });

    clipboard.on('error', function (e) {
    });
})

