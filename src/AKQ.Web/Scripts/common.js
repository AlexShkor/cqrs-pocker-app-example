
ko.bindingHandlers.forecolor = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).css({ 'color': value });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).css({ 'color': value });
    }
};

window.mapping = ko.mapping;