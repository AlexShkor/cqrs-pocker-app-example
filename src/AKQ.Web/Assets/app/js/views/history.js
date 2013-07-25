define(["app/js/events", "text!/templates/history", "router"], function (events, html, router) {

    var HistoryModel = function (data) {
        var self = this;
        this.model = mapping.fromJS(data);

        this.playAgain = function (deal) {
            router.createGame({
                dealId: deal.dealId()
            });
        };

        this.viewDealHistory = function(deal) {
            location.hash = "/history/deal/" + deal.dealId();
        };

        this.viewGame = function (game) {
            window.location.hash = "/game/replay/" + game.gameId();
        };
        
    };
    var init = function (userId,dealId) {
        setTimeout(function () {
            $("#globalLoader").show();
        }, 1);
        amplify.request("history", {userId: userId, dealId: dealId}, function (response) {
            $("#main").html(html);
            ko.applyBindings(new HistoryModel(response));
            $("#globalLoader").hide();
        });
    };
    return {
        init: init
    };
});