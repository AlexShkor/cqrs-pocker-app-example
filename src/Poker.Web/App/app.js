var app = angular.module("poker",
    ["ui.router",
     "hubs.service",
     "event-agregator",
     "poker.home",
     "poker.tables",
     "poker.game"]);
app.value('$', $);
app.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
    
    $locationProvider.html5Mode(true);
    $urlRouterProvider.otherwise('/');
    
    $stateProvider
        .state('home', { url: '/', templateUrl: '/app/home.html', controller: 'HomeController' })
        .state('tables', {url: '/tables', templateUrl: '/tables', controller: 'TablesController' })
        .state("game", { url: '/game/view/:tableId', templateUrl: '/game', controller: 'GameController' });
});

app.controller('AppCtrl', ['$scope', '$rootScope',  "signalsService", "eventAggregatorService", function ($scope, $rootScope,  signalsService, eventAggregatorService) {
    $scope.init = function (user) {
        signalsService.initialize();
    };
}]);