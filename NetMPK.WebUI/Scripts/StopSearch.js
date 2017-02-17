$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    //starting Stop ============================================================================================
    //search
    $("#startName").keyup(function () {
        filterValuesStart();
    });

    //clear
    $("#startClearIcon").click(function () {
        $("#startName").val("");
        filterValuesStart();
    });


    //over and out
    $('.stopSelectStart').mouseover(function () {
            $(this).css("background-color", "#CED3D7")
    });

    $('.stopSelectStart').mouseout(function () {
            $(this).css("background-color", "#FFFFFF")
    });

    //onclick
    $('.stopSelectStart').click(function () {
        $('.stopSelectStart').css("background-color", "white");
        $(this).css("background-color", "rgb(67,139,202)");
        $("#startName").val($(this).text());
        filterValuesStart();
    });

    function filterValuesStart()
    {
        $('.stopSelectStart').css("display", "none");
        var startVal = $("#startName").val().toUpperCase();
        if (startVal.length != 0)
        {
            if ($("#searchSelectRadioFormStart :radio:checked").attr("id") == "startPointStop")
            {
                $("div[id^='" + startVal + "'][class='stopSelectStart']").css("display", "block");
            }
            else if ($("#searchSelectRadioFormStart :radio:checked").attr("id") == "startPointStreet") {
                $("div[name^='" + startVal + "'][class='stopSelectStart']").css("display", "block");
            }
        }
        else
        {
            $('.stopSelectStart').css("display", "block");
        }
    }
//destinationStop================================================================================================================
    //search
    $("#stopName").keyup(function () {
        filterValuesStop();
    });

    //clear
    $("#stopClearIcon").click(function () {
        $("#stopName").val("");
        filterValuesStop();
    });


    //over and out
    $('.stopSelectStop').mouseover(function () {
        $(this).css("background-color", "#CED3D7")
    });

    $('.stopSelectStop').mouseout(function () {
        $(this).css("background-color", "#FFFFFF")
    });

    //onclick
    $('.stopSelectStop').click(function () {
        $('.stopSelectStop').css("background-color", "white");
        $(this).css("background-color", "rgb(67,139,202)");
        $("#stopName").val($(this).text());
        filterValuesStop();
    });

    function filterValuesStop() {
        $('.stopSelectStop').css("display", "none");
        var startVal = $("#stopName").val().toUpperCase();
        if (startVal.length != 0)
        {
            if ($("#searchSelectRadioFormEnd :radio:checked").attr("id") == "stopPointStop") {
                $("div[id^='" + startVal + "'][class='stopSelectStop']").css("display", "block");
            }
            else if ($("#searchSelectRadioFormEnd :radio:checked").attr("id") == "stopPointStreet") {
                $("div[name^='" + startVal + "'][class='stopSelectStop']").css("display", "block");
            }
        }
        else
        {
            $('.stopSelectStop').css("display", "block");
        }
    }

});