define(["app/js/events", "app/js/akq/handModel", "router"], function (events, Hand, router) {

    AKQ.Game = function (gameId, theStoreKey, auction) {
        var self = this;
        var storeKey = theStoreKey;
        self.isFinished = ko.observable(false);
        self.hasWon = ko.observable(false);
        self.isHost = ko.observable(false);
        self.isAuth = ko.observable(false);
        self.isTournament = ko.observable(false);
        self.tactics = ko.observable(null);
        self.bestPossibleResult = ko.observable();
        self.nextDealText = ko.observable();
        self.nextDealLink = ko.observable();
        self.continueLink = ko.observable();
        self.secondsToPlay = ko.observable();
        self.tableNumber = ko.observable();
        self.tournamentStarted = ko.observable();
        self.gameId = gameId;
        self.dealId = null;
        self.auction = auction;

        self.timeLeft = ko.computed(function() {
            var seconsds = self.secondsToPlay();
            if (seconsds <= 0) {
                return "0:00";
            }
            var minutes = ~~(seconsds / 60);
            seconsds = seconsds % 60;
            return minutes + ":" + (seconsds < 10 ? "0" : "" ) + seconsds;
        }, self);

        self.hands = {
            'N': new Hand(),
            'S': new Hand(),
            'W': new Hand(),
            'E': new Hand()
        };

        self.currentTrick = ko.observable(new AKQ.Trick());
        self.previousTrick = ko.observable(new AKQ.Trick());

        self.nsTrickCount = ko.observable(0);

        self.ewTrickCount = ko.observable(0);
        
        self.resultMessage = ko.computed(function () {
            if (self.auction) {
                var result = self.nsTrickCount() - self.auction.bestpossibleResult();
                if (result == 0) {
                    return "Well done!";
                }
                if (result > 0) {
                    return "Excellent play!";
                }
                if (result < 0) {
                    return "Almost made it!";
                }
            }
            return "";
        }, self);
        
        self.tagsSelectVisible = ko.observable(false);
        
        self.toggleTagsSelect = function () {
            self.tagsSelectVisible(!self.tagsSelectVisible());
        };

        self.toggleTag = function (item) {
            if (item.selected() || self.getSelectedTags().length < 3) {
                item.selected(!item.selected());
                var data = {
                    tags: ko.toJS(self.getSelectedTags()),
                    dealId: self.dealId
                };
                router.send('tags', data);
            }
        };

        self.tagsCaption = ko.computed(function () {
            if (self.tactics()) {
                var arr = self.getSelectedTags();
                if (arr.length > 0) {
                    return ko.utils.arrayMap(arr, function (item) {
                        return item.title();
                    }).join(',<br/>');
                }
            }
            return "Tags";
        }, self);

        self.getSelectedTags = function () {
            return ko.utils.arrayFilter(self.tactics().tags(), function (item) {
                return item.selected();
            });
        };


        self.currentPlayer = ko.observable(null);
        self.trumpSuit = ko.observable(null);
        self.estimates = ko.observableArray();

        self.newDeal = function () {
            window.location = self.nextDealLink();
        };

        self.replay = function() {
            router.createGame({
                dealId: self.dealId
            });
        };
        
        self.loadgame = function () {
            amplify.request('loadgame', { id: self.gameId }, function (response) {
                self.update(response);
            });
        };

        self.update = function (response) {
            amplify.store(storeKey, self.gameId);
            self.gameId = response.gameId;
            self.dealId = response.dealId;
            var mapHand = function(data, model) {
                for (var i = 0; i < data.cards.length; i++) {
                    model.cards.push(mapping.fromJS(data.cards[i]));
                }
                model.hasControl(data.hasControl);
                model.isVisible(data.isVisible);
                model.playerName(data.playerName);
            };

            for (var i = 0; i < response.estimates.suits.length; i++) {
                self.estimates.push(new AKQ.SuitEstimate(response.estimates.suits[i]));
            }

            mapHand(response.westHand, self.hands['W']);
            mapHand(response.northHand, self.hands['N']);
            mapHand(response.eastHand, self.hands['E']);
            mapHand(response.southHand, self.hands['S']);

            self.currentTrick().process(response.currentTrick);
            if (response.previousTrick) {
                self.previousTrick().process(response.previousTrick);
            }
            self.currentPlayer(response.currentPlayer);

            self.nsTrickCount(response.nsTrickCount);
            self.ewTrickCount(response.ewTrickCount);
            self.tactics(mapping.fromJS(response.tactics));

            self.isHost(response.isHost);
            self.isAuth(response.isAuth);
            self.isTournament(response.isTournament);
            self.tournamentStarted(response.tournamentStarted);
            self.startTournamentTimer();
            self.tableNumber(response.tableNumber);
            self.secondsToPlay(response.secondsToPlay);
            self.nextDealText(response.nextDealText);
            self.nextDealLink(response.nextDealLink);
            self.continueLink(response.continueLink);

            self.trumpSuit(response.trumpSuit);
            self.auction.process(response.contract, response.joinedPlayers);
            if (!response.isStarted) {
                self.auction.show(function() {
                    self.start();
                });
            }
        };

        self.startTournamentTimer = function () {
            if (self.isTournament() && self.tournamentStarted()) {
                setInterval(function () {
                    self.secondsToPlay(self.secondsToPlay() - 1);
                }, 1000);
            }
        };

        self.start = function () {
            amplify.request('startgame', { id: self.gameId });
        };

        self.playCard = function (card) {
            if (!card.isSelectable()) return;
            self.hands[self.currentPlayer()].disableCards();
            amplify.request('playercard', {
                gameId: self.gameId,
                suit: card.suit(),
                rank: card.value(),
                pos: self.currentPlayer()
            });
        };

        events.listen(gameId, {
            cardPlayed: function (e) {
                var hand = self.hands[e.player];
                if (!hand.isVisible()) {
                    self.currentTrick().setCard(e.player, mapping.fromJS(e.card));
                } else {
                    var cardToRemove;
                    _.each(hand.cards(), function (suit) {
                        _.each(suit(), function (card) {
                            if (card.value() === e.card.value && card.suit() === e.card.suit) {
                                cardToRemove = card;
                            }
                        });
                        if (cardToRemove != null) {
                            suit.remove(cardToRemove);
                            self.currentTrick().setCard(e.player, cardToRemove);
                            cardToRemove = null;
                        }
                    });
                }

            },
            playerTurnStarted: function (e) {
                if (self.currentTrick().completed()) {
                    self.previousTrick(self.currentTrick());
                    self.currentTrick(new AKQ.Trick());
                }
                self.currentPlayer(e.player);
                for (var pos in self.hands) {
                    var hand = self.hands[pos];
                    if (pos === e.player) {
                        hand.enableCards(e.ledSuit, self.trumpSuit());
                    } else {
                        hand.disableCards();
                    }
                }
                self.currentTrick().setTurn(e.player);
            },
            trickEnded: function (e) {
                self.currentTrick().setWinner(e.winner);
                switch (e.winner) {
                    case 'N':
                    case 'S':
                        self.nsTrickCount(self.nsTrickCount() + 1);
                        break;
                    case 'E':
                    case 'W':
                        self.ewTrickCount(self.ewTrickCount() + 1);
                        break;
                }
            },
            gameFinished: function (e) {
                self.hasWon(e.result >= 0);
                setTimeout(function () {
                    self.isFinished(true);
                    self.previousTrick(self.currentTrick());
                }, 1000);
                for (var handKey in e.originalHands) {
                    self.hands[handKey.toUpperCase()].cards.removeAll();
                    for (var i = 0; i < e.originalHands[handKey].cards.length; i++) {
                        var card = e.originalHands[handKey].cards[i];
                        self.hands[handKey.toUpperCase()].cards.push(mapping.fromJS(card));
                    }
                }
                _.each(self.estimates(), function(item) {
                    item.check(e.suitsCount[item.suit().toLowerCase()]);
                });
            },
            playerJoined: function (e) {
                self.auction.players.push(e);
                self.auction.desclimer(e.name + " joined.");
                self.hands[e.position].playerName(e.name);
            },
            gameStarted: function (e) {
                if (!self.tournamentStarted()) {
                    self.tournamentStarted(true);
                    self.startTournamentTimer();
                }
                self.auction.visible(false);
            }
        });
    };

    AKQ.Trick = function () {
        this.ledSuit = ko.observable(null);
        this.w = new AKQ.TrickItem();
        this.n = new AKQ.TrickItem();
        this.e = new AKQ.TrickItem();
        this.s = new AKQ.TrickItem();

        this.completed = ko.observable(false);

        this.process = function (data) {
            this.ledSuit(data.ledSuit);
            this.completed(data.completed);
            this.w.process(data.w);
            this.n.process(data.n);
            this.e.process(data.e);
            this.s.process(data.s);
        };

        this.setTurn = function (pos) {
            this[pos.toLowerCase()].isCurrent(true);
        };

        this.setCard = function (pos, card) {
            this[pos.toLowerCase()].trickCard(card);
            this[pos.toLowerCase()].isEmpty(false);
        };

        this.setWinner = function (pos) {
            this[pos.toLowerCase()].isWinner(true);
            this.completed(true);
        };
    };

    AKQ.TrickItem = function () {
        this.trickCard = ko.observable(null);
        this.isCurrent = ko.observable(false);
        this.isEmpty = ko.observable(true);
        this.isWinner = ko.observable(false);

        this.process = function (data) {
            this.trickCard(data.trickCard);
            this.isCurrent(data.isCurrent);
            this.isEmpty(data.isEmpty);
            this.isWinner(data.isWinner);
        };
    };

    AKQ.SuitEstimate = function (data) {
        var self = this;
        this.players = ko.observableArray();

        this.suit = ko.observable(data.suit);
        this.symbol = ko.observable(data.symbol);
        this.color = ko.observable(data.color);

        this.rowColor = ko.observable(null);

        this.estimatedValue = ko.computed(function () {
            var result = 13;
            var visible = false;
            var editable = null;
            _.each(self.players(), function (player) {
                if (!player.estimated()) {
                    result -= player.count();
                }
                if (player.editable()) {
                    editable = player;
                    if (player.count()) {
                        visible = true;
                    }
                }
            });
            return visible ? result : "";
        }, this);
        
        for (var key in data.players) {
            self.players.push(mapping.fromJS(data.players[key]));
        }

        this.check = function (suitResultData) {
           
            if (self.estimatedValue() !== "") {
                var resultColor = "tomato";
                _.each(self.players(), function(item) {
                    if (item.editable() && item.count() == suitResultData[item.position().toLowerCase()]) {
                        resultColor = "green";
                    }
                });
                self.rowColor(resultColor);
            }
        };
    };

    return AKQ.Game;
});
