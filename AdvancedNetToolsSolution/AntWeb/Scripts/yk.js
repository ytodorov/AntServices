$(window).resize(function () {
    $(".k-chart").data("kendoChart").refresh();
   
});

$(window).ready(function myfunction() {
    //заради менюто което се премахва на малък екран   
    setTimeout(function resizeChart() {
        var $kendoCharts = $(".k-chart").data("kendoChart");
        if ($kendoCharts)
        {
            $kendoCharts.refresh();
        }
    }
   , 1);


    var clipboard = new Clipboard('.fa-pencil-square');

    clipboard.on('success', function (e) {
        debugger;
        var text = e.text;
        console.log(e);
    });

    clipboard.on('error', function (e) {
        console.log(e);
    });
})

