var TablesController = (function () {
    function TablesController($scope, $http, $hubs, $location) {
        this.$scope = $scope;
        this.$http = $http;
        this.$hubs = $hubs;
        this.$location = $location;
        $http.post("/tables/load/", null).success(function (data) {
            $scope.items = data;
        });
        $scope.join = function (table) {
            table.Name = "Joining...";
            $http.post("/game/join", { tableId: table.Id });
        };
        $scope.view = function (table) {
            $location.path("/game/view/" + table.Id);
        };
        $hubs.Users.goToTable(function (e) {
            $location.path("/game/view/" + e.TableId);
        });
    }
    return TablesController;
})();
