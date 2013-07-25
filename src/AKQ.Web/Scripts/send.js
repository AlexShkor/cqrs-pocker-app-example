
    amplify.request.define("creategame", "ajax", {
        url: '/game/create',
        type: "POST"
    });

    amplify.request.define("nextgame", "ajax", {
        url: '/game/next-game',
        type: "POST"
    });

    amplify.request.define("loadgame", "ajax", {
        url: '/game/load',
        type: "POST"
    });

    amplify.request.define("startgame", "ajax", {
        url: '/game/start',
        type: "POST"
    });

    amplify.request.define("playercard", "ajax", {
        url: '/game/play-card',
        type: "POST"
    });

    amplify.request.define("loadreplay", "ajax", {
        url: '/game/replay',
        type: "POST"
    });

    amplify.request.define("loadothers", "ajax", {
        url: '/replay/others',
        type: "POST"
    });

    amplify.request.define("loadhands", "ajax", {
        url: '/replay/myhands',
        type: "POST"
    });

    amplify.request.define("loadroom", "ajax", {
        url: '/room/load',
        type: "POST"
    });

    amplify.request.define("loadtournament", "ajax", {
        url: '/tournament/load',
        type: "POST"
    });

    amplify.request.define("register", "ajax", {
        url: '/tournament/register',
        type: "POST"
    });

    amplify.request.define("withdraw", "ajax", {
        url: '/tournament/withdraw',
        type: "POST"
    });

    amplify.request.define("tags", "ajax", {
        url: '/replay/update-tags',
        type: "POST"
    });

    amplify.request.define("gamesets", "ajax", {
        url: '/home/game-sets',
        type: "POST"
    });

    amplify.request.define("history", "ajax", {
        url: '/home/history',
        type: "POST"
    });
    amplify.request.define("best-play", "ajax", {
        url: '/replay/best-play',
        type: "POST"
    });
    amplify.request.define("repetition-results", "ajax", {
        url: '/results/repetition',
        type: "POST"
    });