// Local variables
var myhub = $.connection.usersHub;

myhub.client.goToTable = function (e) {
    alert(e.TableId);
};

$.connection.hub.start().done(function () {
    myhub.server.connect();
});
