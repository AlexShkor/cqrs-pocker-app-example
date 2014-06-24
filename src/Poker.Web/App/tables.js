
angular.module('poker.tables', []).controller("TablesController", function ($scope, $http, eventAggregatorService, $location) {
    this.$scope = $scope;
    this.$http = $http;
    this.eventAggregatorService = eventAggregatorService;
    this.$location = $location;
    
    function load() {
        $http.post("/tables/load/", null).success(function (data) {
            $scope.items = data;
        });
    }

    $scope.join = function (table) {
        table.Name = "Joining...";
        $http.post("/game/join", { tableId: table.Id }).success(function (response) {
            if (response.Joined) {
                $scope.view(table);
            }
        });
    };
    $scope.view = function (table) {
        $location.path("/game/view/" + table.Id);
    };

    $scope.viewIsAvailable = function (table) {
        if (table)
            return table.Players.length < MinPlayersCount;
        return false;
    };

    eventAggregatorService.subscribe("goToTable", function (e, data) {
        $location.path("/game/view/" + data.TableId);
    });

    eventAggregatorService.subscribe("updateTable", function (e, data) {

        for (var i = 0; i < $scope.items.length; i++) {
            var table = $scope.items[i];

            if (table.Id == data.Table.Id)
                $scope.items[i] = data.Table;
        }

        $scope.$apply();
    });

    load();
})

.controller("CreateTableController", function ($scope, $http, eventAggregatorService, $location) {
    this.$scope = $scope;
    this.$http = $http;
    this.$location = $location;
    this.eventAggregatorService = eventAggregatorService;

    $scope.newTable = { buyIn: 100, smallBlind: 5 };

    $scope.create = function (table) {
        $http.post("/tables/create", table);
    };

    eventAggregatorService.subscribe("goToTablesView", function (e) {
        $location.path("/tables");
    });
});

var MinPlayersCount = 2;