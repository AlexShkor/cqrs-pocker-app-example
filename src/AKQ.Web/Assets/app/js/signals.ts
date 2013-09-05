/// <reference path="../../../Scripts/typings/signalr/signalr.d.ts" />
// Interface
interface UsersHub extends HubConnection {
    goToTable: Function;
    server: any;
    connect();
}

interface SignalR{
    usersHub: UsersHub;
}

// Local variables
var myhub = $.connection.usersHub;

myhub.goToTable = (e) => {
    alert(e.tableId);
};

$.connection.hub.start().done(() => { myhub.server.connect(); });