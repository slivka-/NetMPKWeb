$(document).ready(function () {
    $(".singleLetter").click(function (event) {
        if (event.target.id != "ALL") {
            $(".stopListItem").css("display", "none");
            $("h4").css("display", "none");
            $("div[id^='STOP" + event.target.id + "']").css("display", "flex");
            $("h4[id^=" + event.target.id + "]").css("display", "flex");
        } else {
            $(".stopListItem").css("display", "flex");
            $("h4").css("display", "flex");
        }
    });
});