/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />


var app = angular.module("myApp", []);

app.config(($routeProvider) => {
    $routeProvider
        .when('/page1', <ng.IRoute>{ templateUrl: 'pages/page1.html' })
        .when('/page2', <ng.IRoute>{ templateUrl: 'pages/page2.html' })
        .when('/404', <ng.IRoute>{ templateUrl: 'pages/404' })
        .when('', <ng.IRoute>{ templateUrl: 'home/view' , controller: 'HomeController'})
        .when('/tables', <ng.IRoute>{ templateUrl: 'tables' , controller: 'TablesController'})
        .when('/game/view/:tableId', <ng.IRoute>{ templateUrl: 'game', controller: 'GameController'})
      //  .otherwise(<ng.IRoute>{ redirectTo: '/404' });
});
var hubsInstance =  new Hubs();
app.factory("$hubs", () => {
    return hubsInstance;
});