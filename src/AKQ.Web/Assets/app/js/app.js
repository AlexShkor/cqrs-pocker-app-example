/// <reference path="../../../Scripts/typings/angularjs/angular.d.ts" />
var app = angular.module("myApp", []);

app.config(function ($routeProvider) {
    $routeProvider.when('/page1', { templateUrl: 'pages/page1.html' }).when('/page2', { templateUrl: 'pages/page2.html' }).when('/404', { templateUrl: 'pages/404' }).when('#', { templateUrl: 'pages/home' }).when('', { templateUrl: 'home/view', controller: 'HomeController' }).otherwise({ redirectTo: '/404' });
});
