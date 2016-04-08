$(window).resize(function () {
    $(".k-chart").data("kendoChart").refresh();
   
});

$(window).ready(function myfunction() {
    //заради менюто което се премахва на малък екран   
    setTimeout(function resizeChart() {
        debugger;
        $(".k-chart").data("kendoChart").refresh();
    }
   , 1);
})
