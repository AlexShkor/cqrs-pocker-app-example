
var gameApp = angular.module('poker.game', ['rzModule']);

gameApp.controller("GameController", function ($scope, $stateParams, $http, $sce, $timeout, eventAggregatorService, signalsService) {
    this.$scope = $scope;
    this.$stateParams = $stateParams;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;

    signalsService.invoke("connectToTable", $stateParams.tableId);

    $scope.RaiseValue = 0;
    $scope.Logs = [];

    $scope.messages = [];
    $scope.newMessage = "";

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

    $scope.check = function () {
        $http.post("/game/check", { tableId: $scope.game.Id });
    };

    $scope.fold = function () {
        $http.post("/game/fold", { tableId: $scope.game.Id });
    };

    $scope.call = function () {
        $http.post("/game/call", { tableId: $scope.game.Id });
    };

    $scope.raise = function () {
        if ($scope.RaiseValue > 0 && $scope.RaiseValue > $scope.game.MaxBid) {

            var player = getPlayer($scope.game.CurrentPlayerId);
            var amount = $scope.RaiseValue - player.Bid;

            $http.post("/game/raise", { tableId: $scope.game.Id, amount: amount });
            $scope.RaiseValue = null;
        }
    };

    $scope.sendMessage = function () {
        if ($scope.newMessage) {
            $http.post("/chat/send", { message: $scope.newMessage }).success(function (parameters) { });
            $scope.newMessage = "";
        }
    }

    $scope.isMe = function () {
        if ($scope.game)
            return $scope.game.CurrentPlayerId == $scope.game.MyId;

        return false;
    }

    eventAggregatorService.subscribe("chatMessage", function (e, data) {
        $scope.$apply(function () {
            $scope.messages.push(data);
        });
    });

    $scope.refresh = function () {
        load();
    };

    var gameCreationDelay = 1000; // ms

    eventAggregatorService.subscribe("gameCreated", function (e, data) {
        addLog(logs.gameCreated);
        $timeout(function () {
            load();
        }, gameCreationDelay);
    });

    eventAggregatorService.subscribe("cardsDealed", function (e, data) {

        //for (var i = 0; i < data.Cards.length; i++) {
        //    var model = data.Cards[i];
        //    var player = getPlayer(model.UserId);
        //    if (player != null) {
        //        player.Cards.push(model.Card);
        //        model.Card.Symbol = $sce.trustAsHtml(model.Card.Symbol);
        //    }
        //}

        addLog(logs.cardsDealed);
    });

    eventAggregatorService.subscribe("deckDealed", function (e, data) {

        for (var i = 0; i < data.Deck.length; i++) {
            var card = data.Deck[i];
            $scope.game.Deck.push(card);
            card.Symbol = $sce.trustAsHtml(card.Symbol);
        }

        addLog(logs.deckDealed);
    });

    eventAggregatorService.subscribe("bidMade", function (e, data) {
        var player = getPlayer(data.UserId);
        player.Cash = data.NewCashValue;
        player.Bid = data.Bid;
        $scope.game.MaxBid = data.MaxBid;

        initUserRates();
        $scope.$apply();

        addLog(logs.bidMade, { name: player.Name });
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

        addLog(logs.playerTurned, { name: currentPlayerName });

    });


    eventAggregatorService.subscribe("gameFinished", function (e, data) {

        for (var i = 0; i < data.Winners.length; i++) {
            var winner = getPlayer(data.Winners[i].UserId);
            addLog(logs.noteWinner, { name: winner.Name });
        }
        addLog(logs.gameFinished);
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

            if (reminder != 0) {
                var lastRate = rates[rates.length - 1];
                rates.push(lastRate + reminder);
            }

            $scope.rateIndex = 0;
            $scope.rateMaxIndex = rates.length - 1;

            $scope.minRateValue = rates[0];
            $scope.maxRateValue = rates[rates.length - 1];

            $('.bubble').css('display', 'none');

            $scope.currentUserRate = function (index) {
                $scope.RaiseValue = rates[index];
                $scope.rateIndex = index;
                return rates[index].toString();
            };
        }

        // TODO: logic if not enougth cash
    }

    function initBlinds() {
        for (var i = 0; i < $scope.game.Players.length; i++) {
            var player = $scope.game.Players[i];
            player.IsSmallBlind = false;
            player.IsBigBlind = false;
            player.BlindText = '';

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

    var logs = {
        gameCreated: "New game is created",
        bidMade: "/name/ made a bid",
        playerTurned: "/name/ turn",
        gameFinished: { msg: "Game is finished", ishighlighted: true },
        noteWinner: { msg: "/name/ is winner.", ishighlighted: true },
        cardsDealed: "Cards are dealt",
        deckDealed: "Deck is dealt"
    };

    var logsCont = document.getElementById('logs');
    function addLog(log, replacements) {
        var date = new Date();
        if (typeof (log) != "object") {
            log = { msg: log, ishighlighted: false }
        } else {
            var temp = {};
            angular.copy(log, temp);
            log = temp;
        }
        if (replacements) {
            for (var key in replacements) {
                var alias = "/" + key + "/";
                log.msg = log.msg.splice(log.msg.indexOf(alias), alias.length, replacements[key]);
            }
        }
        log.msg = date.toLocaleTimeString() + " " + log.msg;
        $scope.Logs.push(log);

        if (logsCont)
        logsCont.scrollTop = logsCont.scrollHeight;
    }

    String.prototype.splice = function (idx, rem, s) {
        return (this.slice(0, idx) + s + this.slice(idx + Math.abs(rem)));
    };

    $scope.addLog = function (log, replacements) {
        addLog(log, replacements);
    }

});

