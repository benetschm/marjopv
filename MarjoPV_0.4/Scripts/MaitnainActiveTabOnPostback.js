$(function () {
    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "Tab1";
    $('#Tabs a[href="#' + tabName + '"]').tab('show');
    $("#Tabs a").click(function () {
        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
    });
});