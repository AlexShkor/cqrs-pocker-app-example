define(["app/js/events", "text!/templates/game-sets", "router"], function (events, html, router) {

    var ResultsModel = function (data, index) {
        var self = this;
        this.model = mapping.fromJS(data);
        this.selectedSet = ko.observable(null);
        this.select = function(item) {
            self.selectedSet(item);
        };

        this.playAgain = function(deal) {
            router.createGame({
                dealId: deal.dealId()
            });
        };

        this.viewGame = function(game) {
            window.location.hash = "/game/replay/" + game.gameId();
        };

        $.each(self.model.items(), function(i, item) {
            if (item.index() == index) {
                self.selectedSet(item);
            }
        });
    };
    var init = function (selectedSetId, userId) {
        setTimeout(function() {
            $("#globalLoader").show();
        }, 1);
        amplify.request("gamesets", {index: selectedSetId, userId: userId}, function(response) {
            $("#main").html(html);
            ko.applyBindings(new ResultsModel(response, selectedSetId));
            $("#globalLoader").hide();
        });
    };
    return {
        init: init
    };
});