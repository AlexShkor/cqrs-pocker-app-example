/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />
var GameController = (function () {
    function GameController($scope, $routeParams, $http, $hubs) {
        this.$scope = $scope;
        this.$routeParams = $routeParams;
        this.$http = $http;
        this.$hubs = $hubs;
        $http.post("/game/load/", { tableId: $routeParams.tableId }).success(function (data) {
            $scope.game = data;
        });
        $scope.call = function (player) {
            $http.post("/game/call", { tableId: $scope.game.Id });
        };
    }
    return GameController;
})();
//# sourceMappingURL=game.js.map
