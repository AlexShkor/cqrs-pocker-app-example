define(["app/js/akq/auction", "app/js/akq/replayModel", "app/js/sidebar", "text!/templates/replay"], function (Auction, ReplayModel, sidebar , html) {

    var init = function (tournamentId, gameId) {
        $("#main").empty();
        $("#main").html(html);
        var game = new ReplayModel(gameId, tournamentId);
        ko.applyBindings(game);
        game.load();
        $(".gameLoading").hide();
        $(".gameTable").show();
        sidebar.init();
    };

    return {
        init: init
    };
});