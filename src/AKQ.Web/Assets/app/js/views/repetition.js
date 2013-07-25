define(["text!/templates/results/repetition", "router"], function (html, router) {

    var RepetitionResultsModel = function (data) {
        var self = this;
        this.items = ko.observableArray(data.items);

        this.viewGame = function(game) {
            window.location.hash = "/game/replay/" + game.gameId;
        };
    };
    var init = function (userId) {
        setTimeout(function() {
            $("#globalLoader").show();
        }, 1);
        amplify.request("repetition-results", {userId: userId}, function(response) {
            $("#main").html(html);
            ko.applyBindings(new RepetitionResultsModel(response));
            $("#globalLoader").hide();
        });
    };
    return {
        init: init
    };
});