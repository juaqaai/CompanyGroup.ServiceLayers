define('dataservice.webshop',
    ['amplify'],
    function (amplify) {
        var init = function () {
            amplify.request.define('GetCatalogueDetailsLogList', 'ajax', {
                url: companyGroup.utils.instance().getWebshopApiUrl('GetCatalogueDetailsLogList'),
                dataType: 'json',
                type: 'POST',
                contentType: 'application/json; charset=utf-8'
            });
        },

        getCatalogueDetailsLogList = function (callbacks, data) {

            return amplify.request({
                resourceId: 'GetCatalogueDetailsLogList',
                data: data,
                success: callbacks.success,
                error: callbacks.error
            });
        };

        init();

        return {
            getCatalogueDetailsLogList: getCatalogueDetailsLogList
        };
    });