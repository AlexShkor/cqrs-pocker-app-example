/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
var GameController = (function () {
    function GameController($scope, $http, $hubs) {
        this.$scope = $scope;
        this.$http = $http;
        this.$hubs = $hubs;
    }
    return GameController;
})();
