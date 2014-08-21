var app = angular.module("poker",
    ["ui.router",
     "hubs.service",
     "event-agregator",
     "poker.home",
     "poker.header",
     "poker.tables",
     "poker.game"]);
app.value('$', $);
app.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {

    $locationProvider.html5Mode(true);
    $urlRouterProvider.otherwise('/');

    $stateProvider
        .state('new_table', { url: '/tables/create', templateUrl: '/tables/create', controller: 'CreateTableController' })
        .state('home', { url: '/', templateUrl: '/app/home.html', controller: 'HomeController' })
        .state('tables', { url: '/tables', templateUrl: '/tables/view', controller: 'TablesController' })
        .state("game", { url: '/game/view/:tableId', templateUrl: '/game/template', controller: 'GameController' })
        .state("myprofile", { url: '/profile', templateUrl: '/profile/view', controller: 'ProfileController' })
        .state("chooseavatar", { url: '/profile/avatar', templateUrl: '/profile/choose-avatar', controller: 'AvatarController' });
});

app.controller('AppCtrl', ['$scope', '$rootScope', "signalsService", "eventAggregatorService", function ($scope, $rootScope, signalsService, eventAggregatorService) {
    $scope.init = function (user) {
        signalsService.initialize();
    };
}]); 