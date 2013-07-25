define(["app/js/akq/auction", "app/js/akq/game", "app/js/sidebar", "text!/templates/game"], function (Auction, Game, sidebar, html) {


    var init = function (gameId) {
        if (gameId.length === 0) {
            gameId = amplify.store("AKQAttackCurrent");
        }
        $("#main").empty();
        $("#main").html(html);
        var game = new Game(gameId, "AKQAttackCurrent", new Auction());
        ko.applyBindings(game);
        game.loadgame();
        $(".gameLoading").hide();
        $(".gameTable").show();
        sidebar.init();
    };

    return {
        init: init
    };
});