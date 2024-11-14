
ko.bindingHandlers.clickAndStop = {
    init: function(element, valueAccessor, allBindingsAccessor, viewModel, context) {
        var handler = ko.utils.unwrapObservable(valueAccessor()),
            newValueAccessor = function() {
                return function(data, event) {
                    handler.call(viewModel, data, event);
                    event.cancelBubble = true;
                    if (event.stopPropagation) event.stopPropagation();
                };
            };
   
        ko.bindingHandlers.click.init(element, newValueAccessor, allBindingsAccessor, viewModel, context);    
    }
};