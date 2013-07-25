define(["text!/templates/home", "app/js/akq/tournamentModel"],
    function (html, tournamentViewModel) {
        var init = function() {
            $("main").empty();
            $("#main").html(html);
            var model = new tournamentViewModel();
            ko.applyBindings(model);
        };
        return {
            init: init
        };
    }   
);