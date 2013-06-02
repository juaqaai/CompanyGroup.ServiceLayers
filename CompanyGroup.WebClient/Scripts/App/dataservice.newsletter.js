//define('dataservice.newsletter',
//    ['amplify'],
//amplify

var companyGroup = companyGroup || {};
companyGroup.newsletterDataService = (function () {

    var init = function () {
        amplify.request.define('NewsletterList', 'ajax', {
            url: companyGroup.utils.instance().getNewsletterApiUrl('NewsletterList'),
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json; charset=utf-8'
        });
    },

    getNewsletterList = function (callbacks, data) {

        return amplify.request({
            resourceId: 'NewsletterList',
            data: data,
            success: callbacks.success,
            error: callbacks.error
        });
    };

    init();

    return {
        getNewsletterList: getNewsletterList
    };

})();