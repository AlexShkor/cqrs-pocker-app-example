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
    MyCards: Card[];
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
    constructor(private $scope: IGameScope, private $routeParams: IGameRouteParams ,private $http: ng.IHttpService, private $hubs: Hubs) {
        $http.post("/game/load/", $routeParams.tableId).success((data: IGameModel) => {
            alert(data.Name);
        });
    }
}