/// <reference path="../../../Scripts/typings/signalr/signalr.d.ts" />
// Interface
interface UsersHub extends HubConnection {
    goToTable: Function;
    server: any;
    client: any;
    connect();
}

interface SignalR{
    usersHub: UsersHub;
}

// Local variables
var myhub = $.connection.usersHub;

myhub.client.goToTable = (e) => {
    alert(e.TableId);
};

$.connection.hub.start().done(() => { myhub.server.connect(); });