/// <reference path="../../../Scripts/typings/angularjs/angular.d.ts" />

interface ITablesScope extends ng.IScope {
    items: any[];
    join: Function;
}

interface ITable{
    Name: string;
    Id: string;
}

class TablesController {
    constructor(private $scope: ITablesScope, private $http: ng.IHttpService) {

        $http.post("/tables/load/", null).success((data) => {
            $scope.items = data;
        });
        $scope.join = (table: ITable) => {
            table.Name = "Joining...";
            $http.post("/game/join", { tableId: table.Id });
        };
    }
}