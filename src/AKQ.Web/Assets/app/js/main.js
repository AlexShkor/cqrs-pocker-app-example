
require.config({
    baseUrl: "/assets/",
    urlArgs: "&bust=v" + document.getElementById("requireStartPoint").getAttribute("data-version"),
    paths: {
        "text": "vendor/require/text",
        "application": "app/js/app",
        "router": "app/js/router",
    },
    shim: {

    }
});

require([
    "application"
], function (app) {
    app.init();
});