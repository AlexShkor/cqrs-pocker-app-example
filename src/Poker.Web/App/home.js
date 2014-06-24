
angular.module('poker.home', [])
    .controller("HomeController", function($scope) {
        var self = this;
        this.$scope = $scope;
        $scope.title = "Hello!";
        $scope.clickCounter = 0;
        $scope.onDisplay = function() {
            $scope.clickCounter++;
            self.$scope.title = "2 next";
        };
    })
    .controller("ProfileController", function($scope, $http) {
        $scope.model = {};
        $http.post("/profile/load", {}).success(function (data) {
            $scope.model = data;
        });
    })
    .controller("AvatarController", function($scope, $http) {
        $scope.avatars = [];
        $scope.currentAvatar = "";
        $http.post("/profile/avatars", {}).success(function (data) {
            for (var i = 0; i < data.length; i++) {
                $scope.avatars.push(data[i]);
            }
        });
        $scope.choose = function(avatar) {
            $scope.currentAvatar = avatar.Url;
        }
    });