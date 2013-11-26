/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />


interface IGameRouteParams extends ng.IRouteParamsService {
    tableId: string;
}

interface IGameScope extends ng.IScope {
    game: IGameModel;
    call: Function;
    check: Function;
    raise: Function;
    fold: Function;
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
        $scope.check = function (player) {
            $http.post("/game/check", { tableId: $scope.game.Id });
        };
        $scope.raise = function (player) {
            if (player.RaiseValue > 0) {
                $http.post("/game/raise", { tableId: $scope.game.Id, amount: player.RaiseValue });
                player.RaiseValue = null;
            }
        };
        $scope.fold = function (player) {
            $http.post("/game/fold", { tableId: $scope.game.Id });
        };
    }
}