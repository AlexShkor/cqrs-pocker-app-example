define(["app/js/events", "text!/templates/profile"], function (events, html) {
    var init = function () {
        $("#main").html(html);
    };
    return {
        init: init
    };
});