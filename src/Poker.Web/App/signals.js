'use strict';

angular.module("hubs.service", ['event-agregator'])
    .service('signalsService', ['eventAggregatorService', function (eventAggregatorService) {

        var proxy = null;

        var initialize = function () {

            $.connection.hub.url = "/signalr";

            var connection = $.hubConnection();
            connection.logging = true;
            proxy = connection.createHubProxy('usersHub');

            connection.start().done(function () {
                console.log("SignalR Started");
            })
            .fail(function () {
                console.log("SignalR faild");
            });
            proxy.on('goToTable', function (data) {
                eventAggregatorService.publish('goToTable', data);
            });
            proxy.on('playerTurnChanged', function (data) {
                eventAggregatorService.publish('playerTurnChanged', data);
            });
            proxy.on('bidMade', function (data) {
                eventAggregatorService.publish('bidMade', data);
            });
            proxy.on('cardsDealed', function (data) {
                eventAggregatorService.publish('cardsDealed', data);
            });
            proxy.on('deckDealed', function (data) {
                eventAggregatorService.publish('deckDealed', data);
            });
            proxy.on('gameFinished', function (data) {
                eventAggregatorService.publish('gameFinished', data);
            });
            proxy.on('gameCreated', function (data) {
                eventAggregatorService.publish('gameCreated', data);
            });
            proxy.on('chatMessage', function (data) {
                eventAggregatorService.publish('chatMessage', data);
            });

        };

        var invoke = function (method, data) {
            proxy.invoke(method, data);
        };

        return {
            initialize: initialize,
            invoke: invoke
        };
    }]);
