
angular.module('poker.home', []).controller("HomeController", function ($scope) {
    var self = this;
    this.$scope = $scope;
    $scope.title = "Hello!";
    $scope.clickCounter = 0;
    $scope.onDisplay = function () {
        $scope.clickCounter++;
        self.$scope.title = "2 next";
    };
});