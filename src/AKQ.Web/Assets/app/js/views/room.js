define(["app/js/events"], function (events) {

 
    var RoomViewModel = function() {
        this.players = ko.observableArray();
        this.save = function(parameters) {

        };
    };
    var players = {};
    
    var init = function () {
        var model = new RoomViewModel();
        events.room({
            playerJoined: function (e) {
                var player = mapping.fromJS(e);
                model.players.push(player);
                players[e.UserId] = player;
            },
            playerLeft: function (e) {
                model.players.remove(function (item) { return item.userId() === e; });
            },
            playerAccepted: function (e) {
                window.location = "/game/join/" + e;
            },
        });
        ko.applyBindings(model);
        amplify.request('loadroom', function (response) {
            for (var i = 0; i < response.players.length; i++) {
                model.players.push(mapping.fromJS(response.players[i]));
            }
        });
    };

    return {
        init: init
    };
});