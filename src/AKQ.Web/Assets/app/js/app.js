define(function() {
        window.AKQ = { };

        if (!window.app) {
            window.app = Sammy(function() {
                var app = this;

                app.init = function() {
                    app.run();
                };

                var setGameModule = function(route, modulePath, moduleLoadedCallback) {
                    app.get(route, function () {
                        $("#globalLoader").show();
                        var gameId = this.params['gameId'];
                        var context = this;
                        require([modulePath], function(module) {
            
                            if (moduleLoadedCallback) {
                               moduleLoadedCallback(context, module, gameId);
                            } else {
                                if (module.init) {
                                    module.init(gameId);
                                }
                            }
                            $("#globalLoader").hide();
                        });
                    });
                };

                app.setGameModule = setGameModule;

                var setModule = function(route, modulePath, moduleLoadedCallback) {
                    app.get(route, function () {
                        $("#globalLoader").show();
                        var context = this;
                        require([modulePath], function (module) {
                   
                            if (moduleLoadedCallback) {
                                moduleLoadedCallback(context, module);
                            } else {
                                if (module.init) {
                                    module.init();
                                }
                            }
                            $("#globalLoader").hide();
                        });
                    });
                };

                var setHtmlModule = function(templateName) {
                    setModule("#/" + templateName, "text!/templates/" + templateName, function(context, html) {
                        $("main").empty();
                        $("#main").html(html);
                    });
                };

                app.setModule = setModule;

                setGameModule('/game/view/:gameId', "app/js/views/play");
                setGameModule('#/game/view/:gameId', "app/js/views/play");
                setGameModule('#/game/replay/:gameId', "app/js/views/replay");
                setGameModule('#/replay/tournament/:id/game/:gameId', "app/js/views/tournamentReplay", function(context,replay, gameId) {
                    var id = context.params['id'];
                    replay.init(id, gameId);
                });
                setGameModule('#/tournament', "app/js/views/tournament");
                setGameModule('/room', "app/js/views/room");
                setModule('/', "app/js/views/home");
                setModule('#_=_', "app/js/views/home");
                setModule('#/history', "app/js/views/history");
                setModule('#/history/:userId', "app/js/views/history", function (context, results) {
                    $("#main").empty();
                    var userId = context.params['userId'];
                    results.init(userId);
                });
                setModule('#/history/deal/:dealId', "app/js/views/history", function (context, results) {
                    $("#main").empty();
                    var dealId = context.params['dealId'];
                    results.init(null,dealId);
                });
                setModule('#/results/game-sets/:setId', "app/js/views/results", function(context, results) {
                    $("#main").empty();
                    var id = context.params['setId'];
                    results.init(id);
                });
                setModule('#/results/game-sets/:userId/:setId', "app/js/views/results", function (context, results) {
                    $("#main").empty();
                    var id = context.params['setId'];
                    var userId = context.params['userId'];
                    results.init(id, userId);
                });
                setModule('#/results/repetition', "app/js/views/repetition", function (context, results) {
                    $("#main").empty();
                    results.init();
                });
                setModule('#/profile', "app/js/views/profile");

                setHtmlModule("change-password");

                var goToGame = function (postData) {
                    $("#globalLoader").show();
                    amplify.request("creategame", postData, function(data) {
                        window.location.hash = "/game/view/" + data;
                    });
                };

                this.bind('creategame', function(e,data) {
                    goToGame(data);
                });

                this.get('#/game/create/:gameType', function() {
                    app.trigger("creategame",{ gameType: this.params['gameType'] });
                });

                this.get('#/game/create/deal/:dealId', function() {
                    app.trigger("creategame", { dealId: this.params['dealId'] });
                });

                this.get('#/game/tournament/:tournamentId/next', function() {
                    app.trigger("creategame", { tournamentId: this.params['tournamentId'] });
                });

                this.get('#/game/next/:gameType/:dealId', function() {
                    $("#globalLoader").show();
                    amplify.request("nextgame", { gameType: this.params['gameType'], dealId: this.params['dealId'] }, function (data) {
                        window.location.hash = "/game/view/" + data;
                    });
                });
            });
        }

        return window.app;
    });
