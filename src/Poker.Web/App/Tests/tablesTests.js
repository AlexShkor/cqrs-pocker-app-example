/// <reference path="~/Scripts/jquery-2.0.3.js"/>
/// <reference path="~/Scripts/angular.js"/>
/// <reference path="~/Scripts/angular-mocks.js"/>
/// <reference path="~/App/eventAggregatorService.js"/>
/// <reference path="~/App/app.js"/>
/// <reference path="~/App/tables.js"/>

describe('Tables Controller Tests', function () {
    var ctrl, scope, eventAggregator, http, location;

    var tables = [
        {
            Id: 1,
            BuyIn: 1000,
            SmallBind: 10,
            Name: "Table1"
        },
        {
            Id: 2,
            BuyIn: 1200,
            SmallBind: 15,
            Name: "Table2"
        }
    ];

    beforeEach(function () {
        module('event-agregator');
        module('poker.tables');
    });
    beforeEach(inject(['$controller', '$rootScope', 'eventAggregatorService', '$location', '$httpBackend', function ($controller, $rootScope, eventAggregatorService, $location, $httpBackend) {
        scope = $rootScope.$new();
        eventAggregator = eventAggregatorService;
        location = $location;
        http = $httpBackend;

        ctrl = $controller('TablesController', { $scope: scope, $location: location });
    }]));

    it('should get tables from server', function() {
        http.expect('POST', '/tables/load/').respond(tables);

        http.flush();

        expect(scope.items.length).toBe(2);
        expect(scope.items).toEqual(tables);
    });

    it('should create controller', function() {
        expect(ctrl).not.toBe(null);
    });

    it('should have view method', function () {
        expect(angular.isFunction(scope.view)).toBe(true);
    });

    it('should have join method', function() {
        expect(angular.isFunction(scope.join)).toBe(true);
    });

    it('should send join request', function () {
        var table = {};
        scope.join(table);
        http.expect('POST', '/game/join/');
    });

    it('should set table name on join request', function () {
        var table = {};
        scope.join(table);
        expect(table.Name).toBe("Joining...");
    });

    it('should go to view table if already joined', function () {
        var table = { Id: "123" };
        http.when('POST', '/game/join/').respond({ Joined: true });
        scope.join(table);
        expect(location.path()).toBe('/game/view/123');
    });
});