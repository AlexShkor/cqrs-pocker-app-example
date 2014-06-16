/// <reference path="~/Scripts/jquery-2.0.3.js"/>
/// <reference path="~/Scripts/angular.js"/>
/// <reference path="~/Scripts/angular-mocks.js"/>
/// <reference path="~/App/eventAggregatorService.js"/>
/// <reference path="~/App/signals.js"/>
/// <reference path="~/App/app.js"/>
/// <reference path="/Assets/vendor/angular-spiner/js/rzslider.min.js"/>
/// <reference path="~/App/game.js"/>

describe('Game Controller Test', function () {
    var ctrl, scope, eventAggregator, http, location, stateParams, signalsService;

    var game = {
        BuyIn: 1000,
        CurrentPlayerId: "me1",
        Deck: [],
        Id: "game123",
        MaxBid: 10,
        MyId: "me1",
        Name: "Game 123",
        SmallBlind: 5,

        Players: [

        {
            Bid: 10,
            Cards: [],
            Cash: 990,
            CurrentTurn: true,
            IsMe: true,
            Name: "Player 1",
            Position: 1,
            RaiseValue: 0,
            UserId: "me1",
        },

        {
            Bid: 5,
            Cards: [],
            Cash: 995,
            CurrentTurn: false,
            IsMe: false,
            Name: "Player 2",
            Position: 2,
            RaiseValue: 0,
            UserId: "me2"
        },

        {
            Bid: 0,
            Cards: [],
            Cash: 1000,
            CurrentTurn: false,
            IsMe: false,
            Name: "Player 3",
            Position: 3,
            RaiseValue: 0,
            UserId: "me3"
        }]
    }


    beforeEach(function () {
        module('event-agregator');
        module('hubs.service');
        module('rzModule');
        module('poker.game');
    });

    // signalsService
    beforeEach(inject(['$controller', '$rootScope', '$location', '$httpBackend', 'eventAggregatorService', function ($controller, $rootScope, $location, $httpBackend, eventAggregatorService) {
        scope = $rootScope.$new();
        eventAggregator = eventAggregatorService;

        location = $location;
        http = $httpBackend;
        stateParams = { tableId: 'game123' };
        signalsService = { invoke: function () { } }

        http.expect('POST', '/game/load/').respond(game);
        ctrl = $controller('GameController', { $scope: scope, $location: location, $stateParams: stateParams, signalsService: signalsService, eventAggregatorService: eventAggregator });
    }]));


    afterEach(function () {
        http.verifyNoOutstandingExpectation();
    });

    it('should load game when user comes', function () {

        http.flush();
        expect(scope.game.Id).toEqual(game.Id);
        expect(scope.game.Name).toEqual(game.Name);
    });

    it('should assign Big Blind when new game is created', function () {

        http.flush();
        expect(scope.game.Players[0].IsBigBlind).toBe(true);
        expect(scope.game.Players[0].BlindText).toBe('Big blind');
    });

    it('should assign Small Blind when new game is created', function () {

        http.flush();
        expect(scope.game.Players[1].IsSmallBlind).toBe(true);
        expect(scope.game.Players[1].BlindText).toBe('Small blind');
    });

    it('should not assign Blinds to other players when new game is created', function () {

        http.flush();
        expect(scope.game.Players[2].IsSmallBlind).toBe(false);
        expect(scope.game.Players[2].IsBigBlind).toBe(false);
        expect(scope.game.Players[2].BlindText).toBe('');
    });

    it('should init My actual and max rates', function () {

        http.flush();
        expect(scope.minRateValue).toEqual(10);
        expect(scope.maxRateValue).toEqual(990);
    });

    it('should init My actual and max rates with reminder', function () {

        game.Players[0].Cash = 802;
        http.flush();
        expect(scope.minRateValue).toEqual(10);
        expect(scope.maxRateValue).toEqual(802);
    });

    it('should send request when player checks', function () {

        http.flush();
        http.expect('POST', '/game/check').respond(200);
        scope.check();
    });

    it('should send request when player folds', function () {

        http.flush();
        http.expect('POST', '/game/fold').respond(200);
        scope.fold();
    });

    it('should send request when player calls', function () {

        http.flush();
        http.expect('POST', '/game/call').respond(200);
        scope.call();
    });

    it('should send request when player raises and his bid is legal', function () {

        http.flush();
        scope.game.MaxBid = 40;
        scope.RaiseValue = 50;
        http.expect('POST', '/game/raise').respond(200);
        scope.raise();
    });

    it('should not send request when player raises and his bid is illegal', function () {

        http.flush();
        scope.raise();

        // verifyNoOutstandingExpectation will be called
    });


    it('should send message from player and then clear message input', function () {

        scope.newMessage = 'My message';
        http.expect('POST', '/chat/send').respond(200);
        scope.sendMessage();
        expect(scope.newMessage).toEqual('');
    });


    it('should not send message from player when message is empty', function () {

        scope.newMessage = '';
        scope.sendMessage();

        // verifyNoOutstandingExpectation will be called
    });


    it('should add highlighted log and replace alias', function () {

        var msg = 'Player /name/ on position /position/ is winner';
        scope.addLog({ msg: msg, ishighlighted: true }, { name: 'Nik', position: '5' });
        var log = scope.Logs[scope.Logs.length - 1];
        expect(log.msg).toContain('Player Nik on position 5 is winner');
        expect(log.ishighlighted).toBe(true);
    });

});