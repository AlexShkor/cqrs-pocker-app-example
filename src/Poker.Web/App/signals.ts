/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/signalr/signalr.d.ts" />
// Interface
interface UsersHub extends HubConnection {
    server: any;
    client: IUsersCallback;
}

interface IUsersCallback{
    goToTable(callback: any);
}

class UsersCallback implements IUsersCallback{

    goToTableCallbacks: JQueryCallback;

    constructor(private $client: IUsersCallback)
    {
        this.goToTableCallbacks = $.Callbacks();
        $client.goToTable = (args) => {
            this.goToTableCallbacks.fire(args);
        };
    }

    goToTable(callback: any) {
        this.goToTableCallbacks.add(callback);
    }
}

class Hubs{

    public Users: UsersCallback;

    constructor()
    {
        var usersHub = $.connection.usersHub;
        this.Users = new UsersCallback(usersHub.client);
        $.connection.hub.start().done(() => { usersHub.server.connect(); });
    }
}

interface SignalR{
    usersHub: UsersHub;
}



