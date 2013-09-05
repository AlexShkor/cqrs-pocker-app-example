/// <reference path="../../../Scripts/typings/angularjs/angular.d.ts" />
var TablesController = (function () {
    function TablesController($scope, $http) {
        this.$scope = $scope;
        this.$http = $http;
        $http.post("/tables/load/", null).success(function (data) {
            $scope.items = data;
        });
        $scope.join = function (table) {
            table.Name = "Joining...";
            $http.post("/game/join", { tableId: table.Id });
        };
    }
    return TablesController;
})();
