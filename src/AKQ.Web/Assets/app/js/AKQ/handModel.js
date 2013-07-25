define(function() {
    var HandModel = function () {
        var self = this;
        this.cards = ko.observableArray();
        this.hasControl = ko.observable(false);
        this.isVisible = ko.observable(false);
        this.playerName = ko.observable("");

        self.eachCard = function (each) {
            _.each(self.cards(), function (suit) {
                _.each(suit(), function (c) {
                    each(c);
                });
            });
        };

        self.disableCards = function () {
            if (!self.hasControl()) return;
            self.eachCard(function (c) {
                c.isSelectable(false);
            });
        };

        self.enableCards = function (led, trump) {
            if (!self.hasControl()) return;

            var hasLedCard = self.hasSuitCard(led);
            if (!hasLedCard) {
                self.eachCard(function (c) {
                    c.isSelectable(true);
                });
            } else {
                self.eachCard(function (c) {
                    c.isSelectable(
                            led === 'NT' || c.suit() === led || (!hasLedCard && c.suit() === trump)
                        );
                });
            }
        };

        self.hasSuitCard = function (suitValue) {
            var result = false;
            self.eachCard(function (c) {
                if (c.suit() == suitValue) {
                    result = true;
                }
            });
            return result;
        };
    };
    return HandModel;
});

