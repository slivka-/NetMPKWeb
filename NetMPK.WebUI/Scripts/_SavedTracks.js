$(document).ready(function () {
    $("#collapseContent").data("collapsed", true);

    $("#collapseButton").click(function () {
        if ($("#collapseContent").data("collapsed") === false) {
            $("#collapseContent").animate({ width: "0",padding:"0",margin:"-10"}, 500, "swing");
            $("#collapseContent").data("collapsed", true);
        }
        else
        {
            $("#collapseContent").animate({ width: "220", padding: "20", margin: "0" }, 500, "swing");
            $("#collapseContent").data("collapsed", false);
        }
        
    });
});