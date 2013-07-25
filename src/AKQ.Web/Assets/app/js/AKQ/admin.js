var AKQ = AKQ || {};

define(function () {
    var handAdmin = function () {
        var self = this;

        self.upload = function () {
            amplify.request('upload', {}, function (response) {
                alert(response);
            });
        };
    };
    return new handAdmin();
});