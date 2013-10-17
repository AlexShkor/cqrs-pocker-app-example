var UsersCallback = (function () {
    function UsersCallback($client) {
        var _this = this;
        this.$client = $client;
        this.goToTableCallbacks = $.Callbacks();
        $client.goToTable = function (args) {
            _this.goToTableCallbacks.fire(args);
        };
    }
    UsersCallback.prototype.goToTable = function (callback) {
        this.goToTableCallbacks.add(callback);
    };
    return UsersCallback;
})();

var Hubs = (function () {
    function Hubs() {
        var usersHub = $.connection.usersHub;
        this.Users = new UsersCallback(usersHub.client);
        $.connection.hub.start().done(function () {
            usersHub.server.connect();
        });
    }
    return Hubs;
})();
