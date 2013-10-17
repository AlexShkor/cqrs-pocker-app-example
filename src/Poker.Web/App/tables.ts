/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />

interface ITablesScope extends ng.IScope {
    items: ITable[];
    join: Function;
}

interface ITable{
    Name: string;
    Id: string;
}

class TablesController {
    constructor(private $scope: ITablesScope, private $http: ng.IHttpService, private $hubs: Hubs,private $location: ng.ILocationService) {

        $http.post("/tables/load/", null).success((data) => {
            $scope.items = data;
        });
        $scope.join = (table: ITable) => {
            table.Name = "Joining...";
            $http.post("/game/join", { tableId: table.Id });
        };
        $scope.view = (table: ITable) => {
            $location.path("/game/view/" + table.Id);
        };
        $hubs.Users.goToTable((e) => {
            $location.path("/game/view/" + e.TableId);
        });
    }
}