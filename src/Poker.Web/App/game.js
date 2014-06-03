
angular.module('poker.game', []).controller("GameController", function ($scope, $stateParams, $http, $sce, eventAggregatorService) {
    this.$scope = $scope;
    this.$stateParams = $stateParams;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;

    $scope.RaiseValue = 0;

    $http.post("/game/load/", { tableId: $stateParams.tableId }).success(function (data) {
        $scope.game = data;
        trustSuitsAsHtml($scope.game);
    });

    $scope.call = function () {
        $http.post("/game/call", { tableId: $scope.game.Id });
    };
    $scope.check = function () {
        $http.post("/game/check", { tableId: $scope.game.Id });
    };
    $scope.raise = function () {
        if ($scope.RaiseValue > 0) {
            $http.post("/game/raise", { tableId: $scope.game.Id, amount: $scope.RaiseValue });
            $scope.RaiseValue = null;
        }
    };
    $scope.fold = function () {
        $http.post("/game/fold", { tableId: $scope.game.Id });
    };


    function trustSuitsAsHtml(game) {

        for (var i = 0; i < game.Players.length; i++) {
            var player = game.Players[i];
            for (var j = 0; j < player.Cards.length; j++) {
                var card = player.Cards[j];
                card.Symbol = $sce.trustAsHtml(card.Symbol);
            }
        }

        for (var k = 0; k < game.Deck.length; k++) {
            card = game.Deck[k];
            card.Symbol = $sce.trustAsHtml(card.Symbol);
        }
    }

    
    //var hub = $.connection.gameHub;
    //hub.client.playerTurnChanged = function(e) {
    //    alert(JSON.stringify(e));
    //};

    //$.connection.hub.stop();
    //$.connection.hub.start().done(function () {

    //    hub.server.connect({ id: $routeParams.tableId });
    //});

});
