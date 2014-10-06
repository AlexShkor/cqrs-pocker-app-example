
var gameApp = angular.module('poker.game', ['rzModule']);

gameApp.controller("GameController", function ($scope, $stateParams, $http, $sce, $timeout, eventAggregatorService, signalsService) {
    this.$scope = $scope;
    this.$stateParams = $stateParams;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;
    
    signalsService.invoke("connectToTable", $stateParams.tableId);

    var timeForTurnConst = 20;

    $scope.RaiseValue = 0;
    $scope.Logs = [];
    $scope.timeForTurn = timeForTurnConst;
    $scope.timeBarWidth = "100%";
    $scope.messages = [];
    $scope.newMessage = "";

    var load = function () {
        $http.post("/game/load/", { tableId: $stateParams.tableId }).success(function (data) {
            $scope.game = data;
            $scope.RaiseValue = 0;

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

    $scope.raise = function() {
        $scope.RaiseValue = $scope.rates[$scope.rateIndex];
        var player = getPlayer($scope.game.CurrentPlayerId);
        var amount = $scope.RaiseValue - player.Bet;
        $http.post("/game/raise", { tableId: $scope.game.Id, amount: amount });
        $scope.RaiseValue = null;
    };

    $scope.join = function () {
        $http.post("/game/join", { tableId: $stateParams.tableId }).success(function (response) {
            load();
        });
    };

    $scope.sendMessage = function (e) {
        if (e.type === 'click' || (e.type === 'keyup' && e.keyCode === keyCode.Enter)) {
            if ($scope.newMessage) {
                $http.post("/chat/send", { message: $scope.newMessage }).success(function (parameters) { });
                $scope.newMessage = "";
            }
        }
    }

    $scope.isMyTurn = function () {
        if ($scope.game)
            return $scope.game.CurrentPlayerId == $scope.game.MyId;

        return false;
    }


    $scope.showExistingEmptyPlace = function () {
        if ($scope.game)
            return $scope.game.Players.length < MaxPlayersCount && $scope.game.IsGuest;

        return false;
    };

    $scope.isPlayerMe = function (player) {
        return player.UserId == $scope.game.MyId;
    }

    var msgCont = document.getElementById('msgs');
    eventAggregatorService.subscribe("chatMessage", function (e, data) {
        $scope.$apply(function () {
            $scope.messages.unshift(data);
            if (msgCont)
                msgCont.scrollTop = 0;
        });
    });

    $scope.refresh = function () {
        load();
    };
    $scope.onTimeout = function () {
        var newValue = $scope.timeForTurn - 0.1;
        if (newValue > 0) {
            $scope.timeForTurn = newValue;
            $scope.timeBarWidth = ($scope.timeForTurn / timeForTurnConst)*100;
        } else {
            //next turn
        }
        mytimeout = $timeout($scope.onTimeout, 100);
    }
    var mytimeout = $timeout($scope.onTimeout, 100);

    $scope.stop = function () {
        $timeout.cancel(mytimeout);
    }

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
        if (player) {
            player.Cash = data.NewCashValue;
            player.Bet = data.Bet;
            $scope.$apply();
            addLog(logs.bidMade, { name: player.Name, bidType: data.BidType }, data.BidType + '-note');
        }
    });

    eventAggregatorService.subscribe("biddingFinished", function (e, data) {
        $scope.game.MaxBet = $scope.game.SmallBind;
        $scope.game.Bank = data.Bank;
        resetAllBids();
        $scope.$apply();
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
        $scope.timeForTurn = timeForTurnConst;
        $scope.game.MinBet = data.MinBet;
        initUserRates();
        $scope.$apply();

        addLog(logs.playerTurned, { name: currentPlayerName });

    });


    eventAggregatorService.subscribe("gameFinished", function (e, data) {

        for (var i = 0; i < data.Winners.length; i++) {
            var winner = getPlayer(data.Winners[i].UserId);
            addLog(logs.noteWinner, { name: winner.Name, hand: data.Winners[i].Hand }, 'highlight');
        }
        $scope.game.Bank = 0;
        $scope.$apply();
        addLog(logs.gameFinished, null, 'highlight');
    });

    eventAggregatorService.subscribe("playerJoined", function (e, data) {

        $scope.game.Players.push(data.NewPlayer);
        $scope.$apply();
    });


    function resetAllBids() {
        for (var i = 0; i < $scope.game.Players.length; i++) {
            $scope.game.Players[i].Bet = 0;
        }
    }

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

        $scope.rates = [];

        if (!$scope.game.IsGuest) {

            var me = getPlayer($scope.game.MyId);

            var smallBlind = $scope.game.SmallBlind;

            var minBet = $scope.game.MinBet;
            var betRange = me.Cash - minBet + me.Bet;
            if (betRange > 0) {
                var bet = minBet;
                while (betRange) {
                    $scope.rates.push(bet);
                    bet += smallBlind;
                    betRange -= smallBlind;
                }
                $scope.rates.push(bet);
                $scope.rateIndex = 0;
                $scope.rateMaxIndex = $scope.rates.length - 1;

                $('.bubble').css('display', 'none');
            }
        }

        // TODO: logic if not enough cash
    }


    $scope.currentRateIndex = function (index) {
        $scope.rateIndex = index;
    };

    var logs = {
        gameCreated: "New game is created",
        bidMade: { msg: "/name/ /bidType/" },
        playerTurned: "/name/ turn",
        gameFinished: "Game is finished",
        noteWinner: { msg: "/name/ is winner. Hand: /hand/" },
        cardsDealed: "Cards are dealt",
        deckDealed: "Deck is dealt"
    };

    var logsCont = document.getElementById('logs');
    function addLog(log, replacements, css) {
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
        if (css)
            log.css = css.toLowerCase();

        log.msg = date.toLocaleTimeString() + " " + log.msg;
        $scope.Logs.unshift(log);

        if (logsCont)
            logsCont.scrollTop = 0;
    }

    String.prototype.splice = function (idx, rem, s) {
        return (this.slice(0, idx) + s + this.slice(idx + Math.abs(rem)));
    };

    $scope.addLog = function (log, replacements) {
        addLog(log, replacements);
    }

});

var keyCode = {
    Enter: 13
};

var MaxPlayersCount = 10;

