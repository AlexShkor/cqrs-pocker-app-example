
var gameApp = angular.module('poker.game', ['rzModule']);

gameApp.controller("GameController", function ($scope, $stateParams, $http, $sce, eventAggregatorService, signalsService) {
    this.$scope = $scope;
    this.$stateParams = $stateParams;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;

    signalsService.invoke("connectToTable", $stateParams.tableId);

    $scope.RaiseValue = 0;


    var load = function () {
        $http.post("/game/load/", { tableId: $stateParams.tableId }).success(function (data) {
            $scope.game = data;
            $scope.RaiseValue = data.MaxBid;

            initUserRates();
            trustSuitsAsHtml($scope.game);
        });
    };

    load();

    $scope.call = function () {
        $http.post("/game/call", { tableId: $scope.game.Id });
    };
    $scope.check = function () {
        $http.post("/game/check", { tableId: $scope.game.Id });
    };
    $scope.raise = function () {
        if ($scope.RaiseValue > 0) {

            var player = getPlayer($scope.game.CurrentPlayerId);
            var amount = $scope.RaiseValue - player.Bid;

            $http.post("/game/raise", { tableId: $scope.game.Id, amount: amount });
            $scope.RaiseValue = null;
        }
    };

    $scope.fold = function () {
        $http.post("/game/fold", { tableId: $scope.game.Id });
    };

    $scope.refresh = function () {
        load();
    };

    eventAggregatorService.subscribe("playerTurnChanged", function (e, data) {

        for (var i = 0; i < $scope.game.Players.length; i++) {
            var player = $scope.game.Players[i];

            if (player.UserId == data.CurrentPlayerId)
                player.CurrentTurn = true;
            else
                player.CurrentTurn = false;
        }

        $scope.game.CurrentPlayerId = data.CurrentPlayerId;
        $scope.$apply();

        console.log("Next turn");
    });

    eventAggregatorService.subscribe("bidMade", function (e, data) {

        for (var i = 0; i < $scope.game.Players.length; i++) {
            var player = $scope.game.Players[i];

            if (player.UserId == data.UserId) {
                player.Cash = data.NewCashValue;
                player.Bid = data.Bid;
            }
        }

        $scope.game.MaxBid = data.MaxBid;

        initUserRates();
        $scope.$apply();

    });


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

    function getPlayer(id) {
        for (var i = 0; i < $scope.game.Players.length; i++) {
            var player = $scope.game.Players[i];
            for (var j = 0; j < player.Cards.length; j++) {
                if (player.UserId == id)
                    return player;
            }
        }
    }

    function initUserRates() {

        var me = getPlayer($scope.game.MyId);
        var rates = [];

        var availableCash = me.Cash - $scope.game.MaxBid + $scope.game.SmallBlind * 2;

        if (availableCash > 0) {

            var reminder = availableCash % $scope.game.SmallBlind;
            var loopCashValue = availableCash - reminder;
            var rate = 0;

            while ($scope.game.SmallBlind < loopCashValue) {

                if (rate == 0) {
                    rate = $scope.game.MaxBid;
                    rates.push(rate);
                } else {
                    rate += $scope.game.SmallBlind;
                    rates.push(rate);
                }

                loopCashValue -= $scope.game.SmallBlind;
            }

            if (reminder != 0)
                rates.push(reminder);

            $scope.rate = 0; // index
            $scope.rateMax = rates.length - 1;

            $scope.minRateDisplay = rates[0];
            $scope.maxRateDisplay = rates[rates.length - 1];

            $('.bubble').css('display', 'none');

            $scope.currentUserRate = function (rate) {
                $scope.RaiseValue = rates[rate];
                return rates[rate].toString();
            };
        }

        // TODO: logig if not enougth cash
    }

});

