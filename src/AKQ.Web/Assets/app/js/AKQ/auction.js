define(function () {
    var Auction = function() {
        var self = this;
        self.visible = ko.observable(false);
        self.bestpossibleResult = ko.observable(0);
        self.contract = ko.observable();
        self.declarer = ko.observable();
        self.title = ko.observable();
        self.desclimer = ko.observable("You can wait for another player or start game imidiatly with robots.");
        self.players = ko.observableArray();

    
        self.done = function() {

        };

        self.process = function(contract, joinedPlayers) {
            self.bestpossibleResult(Number(contract.value) + 6);
            self.contract(mapping.fromJS(contract));
            self.declarer(contract.declarer);
            self.title(contract.value + contract.symbol);
            _.each(joinedPlayers, function (pl) {
                self.players.push(pl);
            });
        };

        self.show = function (done) {
         
            self.done = done;
            self.visible(true);
        };

        self.close = function() {
            self.visible(false);
            self.done();
        };
    };
    return Auction;
});