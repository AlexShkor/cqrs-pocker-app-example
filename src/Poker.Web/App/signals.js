'use strict';

angular.module("hubs.service", ['event-agregator'])
    .service('signalsService', ['eventAggregatorService', function (eventAggregatorService) {

        var proxy = null;

        var initialize = function () {
            var connection = $.hubConnection();
            connection.logging = true;
            this.proxy = connection.createHubProxy('usersHub');
            connection.start().done(function () {
            });;

            this.proxy.on('goToTable', function (data) {
                eventAggregatorService.publish('goToTable', data);
            });
        };

        return {
            initialize: initialize
        };
    }]);
