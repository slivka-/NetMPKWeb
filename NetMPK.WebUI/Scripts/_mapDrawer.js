$(document).ready(function () {

    $(".mapPoint").popover("hide");
    $(".mapPoint").popover({ trigger: "toggle", container: "body", html: "true"});
    /*
    $(".mapPoint").on("click", function (e) {
        $(".mapPoint").not(this).popover("hide");
    })*/ 
});