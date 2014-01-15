var app = angular.module("myApp", ["ui.router", "hubs.service", "event-agregator"]);
app.value('$', $);
app.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
    
    $locationProvider.html5Mode(true);
    $urlRouterProvider.otherwise('/');


    $stateProvider
        .state('/page1', { templateUrl: 'pages/page1.html' })
        .state('/page2', { templateUrl: 'pages/page2.html' })
        .state('/404', { templateUrl: 'pages/404' })
        .state('/tables', { templateUrl: 'tables', controller: 'TablesController' })
        .state('/game/view/:tableId', { templateUrl: 'game', controller: 'GameController' });
});

app.controller('AppCtrl', ['$scope', '$rootScope',  "signalsService", "eventAggregatorService", function ($scope, $rootScope,  signalsService, eventAggregatorService) {
    $scope.init = function (user) {
        signalsService.initialize();
    };
}]);
