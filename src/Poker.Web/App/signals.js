'use strict';

angular.module("hubs.service", ['event-agregator'])
    .service('signalsService', ['eventAggregatorService', function (eventAggregatorService) {

        var proxy = null;

        var initialize = function () {

            $.connection.hub.url = "/signalr";
      
            var connection = $.hubConnection();
            connection.logging = true;
            this.proxy = connection.createHubProxy('usersHub');
            this.proxy.on('goToTable', function (data) {
                eventAggregatorService.publish('goToTable', data);
            });
            

            $.connection.hub.start().done(function() {
                
            });
        };

        return {
            initialize: initialize
        };
    }]);
