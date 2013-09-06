/// <reference path="signals.ts" />
/// <reference path="../Scripts/typings/angularjs/angular.d.ts" />


interface IGameScope extends ng.IScope {
}

class GameController {
    constructor(private $scope: IGameScope, private $http: ng.IHttpService, private $hubs: Hubs) {

    }
}