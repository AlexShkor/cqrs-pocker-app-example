/// <reference path="~/Scripts/jquery-2.0.3.js"/>
/// <reference path="~/Scripts/angular.js"/>
/// <reference path="~/Scripts/angular-mocks.js"/>
/// <reference path="~/App/eventAggregatorService.js"/>
/// <reference path="~/App/app.js"/>
/// <reference path="~/App/game.js"/>

describe('Game Controller Test', function () {
    var ctrl, scope, eventAggregator, http, location;


    beforeEach(function () {
    });
    beforeEach(inject(['$controller', '$rootScope', 'eventAggregatorService', '$location', '$httpBackend', function ($controller, $rootScope, eventAggregatorService, $location, $httpBackend) {
        scope = $rootScope.$new();
        eventAggregator = eventAggregatorService;
        location = $location;
        http = $httpBackend;

        ctrl = $controller('GameController', { $scope: scope, $location: location });
    }]));

    it('should', function () {
    });
});