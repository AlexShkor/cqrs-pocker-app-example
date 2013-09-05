/// <reference path="../../../Scripts/typings/angularjs/angular.d.ts" />


interface IHomeScope extends ng.IScope {
    title: string;
    onDisplay();
    clickCounter: number;
}

class HomeController {
    constructor(private $scope: IHomeScope) {

        $scope.title = "Hello!";
        $scope.clickCounter = 0;
        $scope.onDisplay = () => {
            $scope.clickCounter++;
            this.$scope.title = "2 next";
        };
    }
}