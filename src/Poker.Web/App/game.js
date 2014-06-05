
var gameApp = angular.module('poker.game', ['rzModule']);

gameApp.controller("GameController", function ($scope, $stateParams, $http, $sce, eventAggregatorService, signalsService) {
    this.$scope = $scope;
    this.$stateParams = $stateParams;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;

    signalsService.invoke("connectToTable", $stateParams.tableId);

    $scope.RaiseValue = 0;
    $scope.Logs = [];

    var load = function () {
        $http.post("/game/load/", { tableId: $stateParams.tableId }).success(function (data) {
            $scope.game = data;
            $scope.RaiseValue = data.MaxBid;

            initBlinds();
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
        if ($scope.RaiseValue > 0 && $scope.RaiseValue > $scope.game.MaxBid) {

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


    eventAggregatorService.subscribe("gameCreated", function (e, data) {
        load();
        addLog(logs.gameCreated, false);
    });

    eventAggregatorService.subscribe("cardsDealed", function (e, data) {

        for (var i = 0; i < data.Cards.length; i++) {
            var model = data.Cards[i];
            var player = getPlayer(model.UserId);
            if (player != null) {
                player.Cards.push(model.Card);
                model.Card.Symbol = $sce.trustAsHtml(model.Card.Symbol);
            }
        }

        addLog(logs.cardsDealed, false);
    });

    eventAggregatorService.subscribe("deckDealed", function (e, data) {

        for (var i = 0; i < data.Deck.length; i++) {
            var card = data.Deck[i];
            $scope.game.Deck.push(card);
            card.Symbol = $sce.trustAsHtml(card.Symbol);
        }

        addLog(logs.deckDealed, false);
    });

    eventAggregatorService.subscribe("bidMade", function (e, data) {
        var player = getPlayer(data.UserId);
        player.Cash = data.NewCashValue;
        player.Bid = data.Bid;
        $scope.game.MaxBid = data.MaxBid;

        initUserRates();
        $scope.$apply();

        addLog(logs.bidMade.splice(logs.bidMade.indexOf(alias), alias.length, player.Name), false);

    });


    eventAggregatorService.subscribe("playerTurnChanged", function (e, data) {

        var currentPlayerName = "";
        for (var i = 0; i < $scope.game.Players.length; i++) {
            var player = $scope.game.Players[i];

            if (player.UserId == data.CurrentPlayerId) {
                player.CurrentTurn = true;
                currentPlayerName = player.Name;
            } else {
                player.CurrentTurn = false;
            }
        }

        $scope.game.CurrentPlayerId = data.CurrentPlayerId;
        $scope.$apply();

        addLog(logs.playerTurned.splice(logs.playerTurned.indexOf(alias), alias.length, currentPlayerName), false);

    });


    eventAggregatorService.subscribe("gameFinished", function (e, data) {

        var winner = getPlayer(data.Winner.UserId);
        $scope.WinnerName = winner.Name;
        $scope.ShowWinner = true;
        $scope.$apply();

        addLog(logs.gameFinished.splice(logs.gameFinished.indexOf(alias), alias.length, winner.Name), true);

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
            if (player.UserId == id)
                return player;
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

    function initBlinds() {
        for (var i = 0; i < $scope.game.Players.length; i++) {
            var player = $scope.game.Players[i];
            if (player.Bid == $scope.game.SmallBlind) {
                player.IsSmallBlind = true;
                player.BlindText = 'Small blind';
            }

            else if (player.Bid == $scope.game.SmallBlind * 2) {
                player.IsBigBlind = true;
                player.BlindText = 'Big blind';
            }
        }
    }

    var alias = "/name/";
    var logs = {
        gameCreated: "New game is created",
        bidMade: "/name/ made a bid",
        playerTurned: "/name/ turn",
        gameFinished: "/name/ is winner. Game is finished",
        cardsDealed: "Cards are dealt",
        deckDealed: "Deck is dealt"
    };

    var logsCont = document.getElementById('logs');
    function addLog(log, ishighlighted) {
        var date = new Date();
        var msg = date.toLocaleTimeString() + " " + log;

        var logModel = { msg: msg, ishighlighted: ishighlighted };
        $scope.Logs.push(logModel);
        logsCont.scrollTop = logsCont.scrollHeight;
    }

    String.prototype.splice = function (idx, rem, s) {
        return (this.slice(0, idx) + s + this.slice(idx + Math.abs(rem)));
    };

});

