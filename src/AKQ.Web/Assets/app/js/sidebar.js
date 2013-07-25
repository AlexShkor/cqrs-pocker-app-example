define(["vendor/drags"], function () {
    var init =  function() {
        $("#rulesBox").drags();
        $("#rulesLink, #rulesBox button").click(function () {
            $("#rulesBox").toggle();
            $("#rulesLink").toggleClass("active");
        });
        $("#sideBarCount").on("change", "input.numeric-validation", function(e) {
            var value = $(this).val();
            var intValue = parseInt(value);
            if ( intValue > 13 || intValue < 0 || (isNaN(intValue) && value)) {
                $(this).val($(this).data("last"));
                $(this).trigger("change");
            } else {
                $(this).data("last", value);
            }
        });
    };
    return {
        init: init
    };
});