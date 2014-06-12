/// <reference path="~/Scripts/jquery-2.0.3.js"/>
/// <reference path="~/Scripts/angular.js"/>
/// <reference path="~/Scripts/angular-mocks.js"/>
/// <reference path="~/App/eventAggregatorService.js"/>
/// <reference path="~/App/app.js"/>
/// <reference path="~/App/game.js"/>

describe('Game Controller Test', function () {
    var ctrl, scope, eventAggregator, http, location;

    var game = {
        BuyIn: 1000,
        CurrentPlayerId: "me1",
        Deck: [],
        Id: "game123",
        MaxBid: 10,
        MyId: "me1",
        Name: "Game 123",
        SmallBlind: 5,

        Players: [{
            Bid: 10,
            Cards: [],
            Cash: 990,
            CurrentTurn: true,
            IsMe: true,
            Name: "Player 1",
            Position: 1,
            RaiseValue: 0,
            UserId: "me1"
        }]

    }


    //[{
    //    Bid: 10,
    //    Cards: [],
    //    Cash: 990,
    //    CurrentTurn: true,
    //    IsMe: true,
    //    Name: "Player 1",
    //    Position: 1,
    //    RaiseValue: 0,
    //    UserId: "me1"
    //},
    //    {
    //        Bid: 5,
    //        Cards: [],
    //        Cash: 995,
    //        CurrentTurn: false,
    //        IsMe: false,
    //        Name: "Player 2",
    //        Position: 2,
    //        RaiseValue: 0,
    //        UserId: "me2"
    //    },

    //    {
    //        Bid: 0,
    //        Cards: [],
    //        Cash: 1000,
    //        CurrentTurn: false,
    //        IsMe: false,
    //        Name: "Player 3",
    //        Position: 3,
    //        RaiseValue: 0,
    //        UserId: "me3"
    //    }]


    beforeEach(function () {
        module('event-agregator');
        module('rzModule');
        module('poker.game');
    });

    beforeEach(inject(['$controller', '$rootScope', 'eventAggregatorService', '$location', '$httpBackend', function ($controller, $rootScope, eventAggregatorService, $location, $httpBackend) {
        scope = $rootScope.$new();
        eventAggregator = eventAggregatorService;
        location = $location;
        http = $httpBackend;

        ctrl = $controller('GameController', { $scope: scope, $location: location });
        http.expect('POST', '/game/load/').respond(game);

    }]));

    
    afterEach(function () {
        http.verifyNoOutstandingExpectation();
    });

    it('should load game when user comes', function () {

        //http.flush();
        //expect(scope.game).toEqual(game);

    });
});