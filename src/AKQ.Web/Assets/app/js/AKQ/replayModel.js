define(["app/js/events", "app/js/akq/handModel" , "router"], function (events, Hand, router) {

    AKQ.Replay = function (gameId, tournamentId) {
        var self = this;
        self.gameId = gameId;
        self.myHandId = gameId;
        self.gameToReplayId = gameId;
        self.tournamentId = tournamentId;
        self.dealId = null;

        self.hands = {
            'N': new Hand(),
            'S': new Hand(),
            'W': new Hand(),
            'E': new Hand()
        };
        self.controlsVisible = ko.observable(false);
        self.tagsSelectVisible = ko.observable(false);
        self.model = ko.observable(null);
        self.contract = ko.observable(null);
        self.nsTricks = ko.observable(0);
        self.ewTricks = ko.observable(0);
        self.trickIndex = ko.observable(-1);
        self.dealNumber = ko.observable(1);
        self.tricks = ko.observableArray();
        self.activeTab = ko.observable("#play");
        self.isBestPlay = ko.observable(false);
        self.otherTables = ko.observableArray();
        self.myHands = ko.observableArray();

        self.currentTrick = ko.computed(function() {
            if (self.trickIndex() != -1) {
                var trick = self.model().tricks()[self.trickIndex()];
                var buildmodel = function(side) {
                    return ko.utils.arrayFirst(trick.players(), function (item) { return item.position() == side; });
                };
                return {
                    w: buildmodel("W"),
                    n: buildmodel("N"),
                    e: buildmodel("E"),
                    s: buildmodel("S"),
                };
            }
            return null;
        }, self);

        self.toggleTagsSelect = function() {
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

        self.tagsCaption = ko.computed(function() {
            if (self.model()) {
                var arr = self.getSelectedTags();
                if (arr.length > 0) {
                    return ko.utils.arrayMap(arr, function(item) {
                        return item.title();
                    }).join(',<br/>');
                }
            }
            return "Tags";
        }, self);

        self.getSelectedTags = function() {
            return ko.utils.arrayFilter(self.model().tactics.tags(), function(item) {
                return item.selected();
            });
        };

        self.load = function () {
            $("#globalLoader").show();
            amplify.request('loadreplay', { id: self.gameToReplayId }, function (response) {
                self.controlsVisible(true);
                self.response = response;
                self.process(response);
                self.isBestPlay(false);
                $("#globalLoader").hide();
            });
        };

        self.bestPlay = function () {

            $("#globalLoader").show();
            amplify.request('best-play', { dealId: self.dealId }, function (response) {
                self.gameToReplayId = null;
                self.controlsVisible(true);
                self.response = response;
                self.process(response);
                self.isBestPlay(true);
                $("#globalLoader").hide();
            });
        };

        self.process = function (response) {
            self.dealId = response.dealId;
            self.trickIndex(-1);
            self.nsTricks(0);
            self.ewTricks(0);
            self.ewTricks(0);
            var mapHand = function (data, model) {
                model.cards.removeAll();
                for (var i = 0; i < data.cards.length; i++) {
                    model.cards.push(mapping.fromJS(data.cards[i]));
                }
                model.hasControl(data.hasControl);
                model.isVisible(data.isVisible);
                model.playerName(data.playerName);
            };
            mapHand(response.hands.w, self.hands['W']);
            mapHand(response.hands.n, self.hands['N']);
            mapHand(response.hands.e, self.hands['E']);
            mapHand(response.hands.s, self.hands['S']);
            self.model(mapping.fromJS(response));
        };

        self.loadOtherTables = function () {
            amplify.request('loadothers', { gameId: self.myHandId, tournamentId: self.tournamentId }, function (response) {
                self.otherTables.removeAll();
                _.each(response, function(item) {
                    self.otherTables.push(item);
                });
               
            });
        };

        self.next = function () {
            var index = self.trickIndex();
            if (index >= 12) {
                return;
            }
            index++;
            var trick = self.model().tricks()[index];
            for (var i = 0; i < trick.players().length; i++) {
                var player = trick.players()[i];
                if (player.isWon()) {
                    self.addWonTrick(player.position());
                }
                _.each(self.hands[player.position()].cards(), function(suit) {
                    var card = ko.utils.arrayFirst(suit(), function(item) { return item.suit() + item.value() == player.card(); });
                    if (card) {
                        player.trickCard = card;
                        suit.remove(card);
                    }
                });
            }
            self.trickIndex(index);
        };

        self.prev = function () {
            var index = self.trickIndex();
            if (index < 0) {
                return;
            }
            self.reset();
            for (var i = 0; i < index; i++) {
                self.next();
            }
        };

        self.addWonTrick = function(position) {
            switch (position) {
                case "N":
                case "S":
                    self.nsTricks(self.nsTricks() + 1);
                    break;
                case "E":
                case "W":
                    self.ewTricks(self.ewTricks() + 1);
                    break;
            }
        };

        self.reset = function() {
            self.process(self.response);
        };

        self.loadMyHands = function () {

            amplify.request('loadhands', { gameId: self.gameId, tournamentId: self.tournamentId }, function (response) {
                self.myHands.removeAll();
                _.each(response, function (item) {
                    self.myHands.push(item);
                });
            });
        };

        self.selectHand = function (item) {
            if (item.id != self.gameToReplayId) {
                var myHand = self.myHandId;
                self.myHandId = item.id;
                self.dealNumber(self.myHands.indexOf(item) + 1);
                self.selectOther(item);
                if (myHand != item.id) {
                    self.otherTables.removeAll();
                    self.loadOtherTables();
                }
            }
        };

        self.selectOther = function (item) {
            if (item.id != self.gameToReplayId) {
                self.gameToReplayId = item.id;
                self.controlsVisible(false);
                self.load();
            }
        };
        
        self.playAgain = function () {
            router.createGame({
                dealId: self.dealId
            });
        };
        self.loadMyHands();
        self.loadOtherTables();
    };
    return AKQ.Replay;
});
