$.validator.unobtrusive.adapters.addSingleVal("exclude", "chars");
$.validator.addMethod("exclude", function (value, element, exclude) {
    if (value) {
        for (var i = 0; i < exclude.length; i++) {
            if (jQuery.inArray(exclude[i], value) != -1) {
                return false;
            }
        }
    }
    return true;
})

$.validator.unobtrusive.adapters.addSingleVal("include", "chars");
$.validator.addMethod("include", function (value, element, include) {
    if (value) {
        let validCharList = include.split(',');
        console.log(validCharList);
        if (!validCharList.includes(value)) {
            return false
        }
    }
    return true;
})


////$.validator.addMethod("uppercase",
////    function (value, element, params) {
////        return value === value.toUpperCase();
////    });

////$.validator.unobtrusive.adapters.add("uppercase",
////    params,
////    function (options) {
////        options.rules[index] = options.params;
////        options.messages[index] = options.message;
////    });


//(function ($) {
//    $.validator.addMethod('requiredif', function (value, element, params) {
//        /*var inAppPurchase = $('#InAppPurchase').is(':checked');

//        if (inAppPurchase) {
//            return true;
//        }

//        return false;*/

//        var isChecked = $(param).is(':checked');

//        if (isChecked) {
//            return false;
//        }

//        return true;
//    }, '');

//    $.validator.unobtrusive.adapters.add('requiredif', ['param'], function (options) {
//        options.rules["requiredif"] = '#' + options.params.param;
//        options.messages['requiredif'] = options.message;
//    });
//})(jQuery);
