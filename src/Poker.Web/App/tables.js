
angular.module('poker.tables',[]).controller("TablesController", function($scope, $http, eventAggregatorService, $location) {
    this.$scope = $scope;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;
    this.$location = $location;
    $http.post("/tables/load/", null).success(function(data) {
        $scope.items = data;
    });
    $scope.join = function(table) {
        table.Name = "Joining...";
        $http.post("/game/join", { tableId: table.Id });
    };
    $scope.view = function(table) {
        $location.path("/game/view/" + table.Id);
    };
    eventAggregatorService.subscribe("goToTable", function(e) {
        $location.path("/game/view/" + e.TableId);
    });
});