/// <reference path="../../../Scripts/typings/angularjs/angular.d.ts" />
var HomeController = (function () {
    function HomeController($scope) {
        var _this = this;
        this.$scope = $scope;
        $scope.title = "Hello!";
        $scope.clickCounter = 0;
        $scope.onDisplay = function () {
            $scope.clickCounter++;
            _this.$scope.title = "2 next";
        };
    }
    return HomeController;
})();
