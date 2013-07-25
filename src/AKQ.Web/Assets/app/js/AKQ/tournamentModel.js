define(["router"], function (router) {
    var tournamentViewModel = function () {
        var self = this;
        self.tournamentId = null;
        self.isAuthenticated = ko.observable($("#isAuth").val() == "True");
        self.visible = ko.observable(false);
        self.loading = ko.observable(false);
        self.secondsToStart = ko.observable(999);
        self.registred = ko.observable(false);
        self.registerModalVisible = ko.observable(false);
        self.intervalId = null;
        
        self.players = ko.observable(0);

        self.load = function (orCreate) {
            self.loading(true);
            clearInterval(self.intervalId);
            amplify.request('loadtournament', { orCreate: orCreate || false}, function (data) {
                if (data.tournamentId) {
                    self.loading(false);
                    self.tournamentId = data.tournamentId;
                    self.secondsToStart(data.secondsToStart);
                    self.registred(data.registred);
                    self.players(data.players);
                    self.intervalId = setInterval(onTimer, 1000);
                } else {
                    self.visible(false);
                }
                if (orCreate) {
                    self.register();
                }
            });
        };

        self.timeToStart = ko.computed(function () {
            var seconsds = self.secondsToStart();
            if (seconsds <= 0) {
                return "0:00";
            }
            var minutes = ~~(seconsds / 60);
            seconsds = seconsds % 60;
            return minutes + ":" + (seconsds < 10 ? "0" : "") + seconsds;
        }, self);

        self.register = function () {
            if (!self.registred()) {
                amplify.request('register', { id: self.tournamentId }, function () {
                    self.registred(true);
                    self.players(self.players() + 1);
                });
            }
        };

        self.open = function () {
            if (!self.isAuthenticated()) {
                self.registerModalVisible(true);
                return;
            }
            self.visible(true);
            self.load(true);
        };
        self.close = function() {
            self.visible(false);
        };

        self.withdraw = function () {
            if (self.registred()) {
                amplify.request('withdraw', { id: self.tournamentId }, function () {
                    self.registred(false);
                    self.players(self.players() - 1);
                });
            }
        };

        self.practise = function () {
            if (!self.isAuthenticated()) {
                self.registerModalVisible(true);
                return;
            }
            router.createGame({
                gameType: "attack"
            });
        };

        self.repetition = function () {
            if (!self.isAuthenticated()) {
                self.registerModalVisible(true);
                return;
            }
            router.createGame({
                gameType: "repetition"
            });
        };

        self.playAnyway = function() {
            router.createGame({
                gameType: "attack"
            });
        };

        self.closeRegister = function() {
            self.registerModalVisible(false);
        };

        var onTimer = function() {
            var seconds = self.secondsToStart();
            if (seconds <= 0) {
                clearInterval(self.intervalId);
                if (self.registred()) {
                    router.createGame({
                        tournamentId: self.tournamentId
                    });
                } else {
                    self.load();
                }
            } else {
                self.secondsToStart(seconds - 1);
            }

        };
    };
    return tournamentViewModel;
});