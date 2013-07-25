define(function() {
    Function.prototype.curry = function() {
        if (arguments.length < 1) {
            return this; //nothing to curry with - return function
        }
        var self = this;
        var args = Array.prototype.slice.call(arguments);
        //    var args = toArray(arguments);
        return function() {
            return self.apply(this, args.concat(Array.prototype.slice.call(arguments)));
        };
    };

    function postData(url, data, callback) {
        $.post(url, data, function(result) {
            return callback(result);
        });
    }

    ko.bindingHandlers.forecolor = {
        init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).css({ 'color': value });
        },
        update: function(element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).css({ 'color': value });
        }
    };
});