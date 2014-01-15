
var GameController = (function () {
    function GameController($scope, $routeParams, $http, eventAggregatorService) {
        this.$scope = $scope;
        this.$routeParams = $routeParams;
        this.$http = $http;
        this.eventAggregatorService = eventAggregatorService;

        $scope.RaiseValue = 0;

        $http.post("/game/load/", { tableId: $routeParams.tableId }).success(function (data) {
            $scope.game = data;
        });
        $scope.call = function () {
            $http.post("/game/call", { tableId: $scope.game.Id });
        };
        $scope.check = function () {
            $http.post("/game/check", { tableId: $scope.game.Id });
        };
        $scope.raise = function () {
            if ($scope.RaiseValue > 0) {
                $http.post("/game/raise", { tableId: $scope.game.Id, amount: $scope.RaiseValue });
                $scope.RaiseValue = null;
            }
        };
        $scope.fold = function () {
            $http.post("/game/fold", { tableId: $scope.game.Id });
        };
        
        
        //var hub = $.connection.gameHub;
        //hub.client.playerTurnChanged = function(e) {
        //    alert(JSON.stringify(e));
        //};

        //$.connection.hub.stop();
        //$.connection.hub.start().done(function () {

        //    hub.server.connect({ id: $routeParams.tableId });
        //});

    }
    return GameController;
})();
