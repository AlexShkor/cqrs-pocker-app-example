/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />


interface IGameRouteParams extends ng.IRouteParamsService {
    tableId: string;
}

interface IGameScope extends ng.IScope {
}

interface IGameModel {
    Id: string;
    Name: string;
    BuyIn: string;
    SmallBlind: string;
    MaxPlayers: string;
}


class GameController {
    constructor(private $scope: IGameScope, private $routeParams: IGameRouteParams ,private $http: ng.IHttpService, private $hubs: Hubs) {
        $http.post("/game/load/", $routeParams.tableId).success((data: IGameModel) => {
            alert(data.Name);
        });
    }
}