/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
var app = angular.module("myApp", []);

app.config(function ($routeProvider) {
    $routeProvider.when('/page1', { templateUrl: 'pages/page1.html' }).when('/page2', { templateUrl: 'pages/page2.html' }).when('/404', { templateUrl: 'pages/404' }).when('', { templateUrl: 'home/view', controller: 'HomeController' }).when('/tables', { templateUrl: 'tables', controller: 'TablesController' });
    //  .otherwise(<ng.IRoute>{ redirectTo: '/404' });
});
var hubsInstance = new Hubs();
app.factory("$hubs", function () {
    return hubsInstance;
});
