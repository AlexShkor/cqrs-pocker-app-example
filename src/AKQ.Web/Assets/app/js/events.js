define(function () {

    var listen = function (gameId, callbacks) {
        $.connection.hub.stop();
        var bridge = $.connection.bridgeHub;
        
        bridge.client.cardPlayed = function(data) {
            callbacks.cardPlayed(data);
        };
        bridge.client.trickEnded = function(data) {
            callbacks.trickEnded(data);
        };

        bridge.client.playerTurnStarted = function(data) {
            callbacks.playerTurnStarted(data);
        };
        bridge.client.playerJoined = function(data) {
            callbacks.playerJoined(data);
        };
        bridge.client.gameStarted = function (data) {
            callbacks.gameStarted(data);
        };
        bridge.client.gameFinished = function (data) {
            callbacks.gameFinished(data);
        };
        $.connection.hub.start().done(function() {
            bridge.server.join(gameId);
        });
        
    };

    var room = function (callbacks) {
        $.connection.hub.stop();
        var roomHub = $.connection.roomHub;

        roomHub.client.playerJoined = function (data) {
            callbacks.playerJoined(data);
        };
        
        roomHub.client.playerLeft = function (data) {
            callbacks.playerLeft(data);
        };
        
        roomHub.client.playerAccepted = function (data) {
            callbacks.playerAccepted(data);
        };
        $.connection.hub.start();
    };
    
    return {
        listen: listen,
        room: room
    };
});