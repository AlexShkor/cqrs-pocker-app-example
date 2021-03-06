﻿/// <reference path="~/Scripts/jquery-2.0.3.js"/>
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
            Players: [],
            Name: "Table1"
        },
        {
            Id: 2,
            BuyIn: 1200,
            SmallBind: 15,
            Players: [{ UserId: "me1" }, { UserId: "me2" }],
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
        http.expect('POST', '/tables/load/').respond(tables);

    }]));

    afterEach(function () {
        http.verifyNoOutstandingExpectation();
    });

    it('should get tables from server', function () {
        http.flush();
        expect(scope.items.length).toBe(2);
        expect(scope.items).toEqual(tables);
    });

    it('should create controller', function () {
        expect(ctrl).not.toBe(null);
    });

    it('should have view method', function () {
        expect(angular.isFunction(scope.view)).toBe(true);
    });

    it('should have join method', function () {
        expect(angular.isFunction(scope.join)).toBe(true);
    });

    it('should send join request', function () {
        var table = {};
        http.expect('POST', '/game/join').respond({ Joined: true });
        scope.join(table);
    });

    it('should set table name on join request', function () {
        var table = {};
        http.expect('POST', '/game/join').respond({ Joined: true });
        scope.join(table);

        expect(table.Name).toBe("Joining...");
    });

    it('should go to view table if already joined', function () {
        var table = { Id: "123" };
        http.expect('POST', '/game/join').respond({ Joined: true });
        scope.join(table);
        http.flush();
        expect(location.path()).toBe('/game/view/123');
    });

    it('should disable view button if table has no game yet', function () {
        http.flush();
        var disabled = scope.viewIsAvailable(scope.items[0]);
        expect(disabled).toBe(true);
    });

    it('should enable view button if table has game', function () {
        http.flush();
        var disabled = scope.viewIsAvailable(scope.items[1]);
        expect(disabled).toBe(false);
    });

});