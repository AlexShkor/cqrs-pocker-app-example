/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />


interface IGameRouteParams extends ng.IRouteParamsService {
    tableId: string;
}

interface IGameScope extends ng.IScope {
    game: IGameModel;
    call: Function;
}

interface IGameModel {
    Id: string;
    Name: string;
    BuyIn: string;
    SmallBlind: string;
    Deck: Card[];
    Players: Player[];
}

interface Card{
    Suit: string;
    Value: string;
    Symbol: string;
    Color: string;
}

interface Player {
    Position: number;
    Cash: number;
    Bid: number;
    UserId: string;
    Name: string;
}


class GameController {
    constructor(private $scope: IGameScope, private $routeParams:any ,private $http: ng.IHttpService, private $hubs: Hubs) {
        $http.post("/game/load/", { tableId: $routeParams.tableId }).success((data: IGameModel) => {
            $scope.game = data;
        });
        $scope.call = function (player) {
            $http.post("/game/call", { tableId: $scope.game.Id });
        };
    }
}