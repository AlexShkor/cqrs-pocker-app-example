// Local variables
var myhub = $.connection.usersHub;

myhub.goToTable = function (e) {
    alert(e.tableId);
};

$.connection.hub.start().done(function () {
    myhub.server.connect();
});
