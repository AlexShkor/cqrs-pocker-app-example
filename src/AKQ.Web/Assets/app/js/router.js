define(function () {
        var parseValues = function (data, callback, prefix, postfix) {
            postfix = postfix || "";
            prefix = prefix || "";
            for (var key in data) {
                if (data[key] == null) {
                    continue;
                }
                 if ( Object.prototype.toString.call( data[key] ) === '[object Array]') {
                    parseValues(data[key], callback, prefix + key + postfix + "[", "]");
                 } else if (typeof data[key] == "object") {
                     parseValues(data[key], callback, prefix + key + postfix + ".");
                 } else {
                    callback(prefix + key, data[key]);
                }
            }
        };

        var jsonToFormData = function (json) {
            var data = {};
            parseValues(json, function (key, value) {
                data[key] = value;
            });
            return data;
        };

        return {
            createGame: function (data) {
                app.trigger("creategame", data);
            },
            send: function (url, data, callback) {
                amplify.request(url, jsonToFormData(data), callback);
            }
        };
            
    });
