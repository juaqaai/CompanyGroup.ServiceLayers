﻿var companyGroup = companyGroup || {};

companyGroup.utils = (function () {

    var _pictureBaseUrl = '';

    var _webshopBaseApiUrl = '';
    var _structureBaseApiUrl = '';
    var _pictureBaseApiUrl = '';
    var _customerBaseApiUrl = '';
    var _salesOrderBaseApiUrl = '';
    var _invoiceBaseApiUrl = '';
    var _contactPersonBaseApiUrl = '';
    var _shoppingCartBaseApiUrl = '';
    var _registrationBaseApiUrl = '';
    var _visitorBaseApiUrl = '';
    var _downloadPriceListUrl = '';
    var _shoppingCartUrl = '';

    var _instance;

    function create() {
        return {
            setPictureBaseUrl: function (url) {
                _pictureBaseUrl = url;
            },
            getPictureUrl: function (fileName) {
                return _pictureBaseUrl + fileName;
            },
            getWebshopApiUrl: function (url) {
                return _webshopBaseApiUrl + url;
            },
            setWebshopBaseApiUrl: function (url) {
                _webshopBaseApiUrl = url;
            },
            getStructureApiUrl: function (url) {
                return _structureBaseApiUrl + url;
            },
            setStructureBaseApiUrl: function (url) {
                _structureBaseApiUrl = url;
            },
            getPictureApiUrl: function (url) {
                return _pictureBaseApiUrl + url;
            },
            setPictureBaseApiUrl: function (url) {
                _pictureBaseApiUrl = url;
            },
            getCustomerApiUrl: function (url) {
                return _customerBaseApiUrl + url;
            },
            setCustomerBaseApiUrl: function (url) {
                _customerBaseApiUrl = url;
            },
            getSalesOrderApiUrl: function (url) {
                return _salesOrderBaseApiUrl + url;
            },
            setSalesOrderBaseApiUrl: function (url) {
                _salesOrderBaseApiUrl = url;
            },
            getInvoiceApiUrl: function (url) {
                return _invoiceBaseApiUrl + url;
            },
            setInvoiceBaseApiUrl: function (url) {
                _invoiceBaseApiUrl = url;
            },
            getContactPersonApiUrl: function (url) {
                return _contactPersonBaseApiUrl + url;
            },
            setContactPersonBaseApiUrl: function (url) {
                _contactPersonBaseApiUrl = url;
            },
            getShoppingCartApiUrl: function (url) {
                return _shoppingCartBaseApiUrl + url;
            },
            setShoppingCartBaseApiUrl: function (url) {
                _shoppingCartBaseApiUrl = url;
            },
            getProductDetailsUrl: function (productId) {
                return _webshopBaseApiUrl + 'Details/?ProductId=' + encodeURIComponent(productId);
            },
            getThumbnailPictureUrl: function (productId, recId, dataAreaId) {
                return _pictureBaseApiUrl + 'GetPictureItem/?ProductId=' + encodeURIComponent(productId) + '&RecId=' + recId + '&DataAreaId=' + dataAreaId + '&MaxWidth=60&MaxHeight=60';
            },
            getPictureUrl: function (productId, recId, dataAreaId) {
                return _pictureBaseApiUrl + 'GetPictureItem/?ProductId=' + encodeURIComponent(productId) + '&RecId=' + recId + '&DataAreaId=' + dataAreaId + '&MaxWidth=180&MaxHeight=134'; //webshopBaseUrl + productId + '/' + recId + '/' + dataAreaId + '/94/69/Picture';
            },
            getBigPictureUrl: function (productId, recId, dataAreaId) {
                return _pictureBaseApiUrl + 'GetPictureItem/?ProductId=' + encodeURIComponent(productId) + '&RecId=' + recId + '&DataAreaId=' + dataAreaId + '&MaxWidth=500&MaxHeight=500';
            },
            getRegistrationApiUrl: function (url) {
                return _registrationBaseApiUrl + url;
            },
            setRegistrationBaseApiUrl: function (url) {
                _registrationBaseApiUrl = url;
            },
            getVisitorApiUrl: function (url) {
                return _visitorBaseApiUrl + url;
            },
            setVisitorBaseApiUrl: function (url) {
                _visitorBaseApiUrl = url;
            },
            validateNumber: function (value) {
                return (/^[0-9]+$/.test(value));
            },
            formatNumber: function (str) {
                if (!this.validateNumber(str)) {
                    return str;
                }
                str += '';
                x = str.split('.');
                x1 = x[0];
                x2 = x.length > 1 ? '.' + x[1] : '';
                var rgx = /(\d+)(\d{3})/;
                while (rgx.test(x1))
                    x1 = x1.replace(rgx, '$1' + ' ' + '$2');
                return x1 + x2;
            },
            getCompletionListAllProductUrl: function () {
                return _webshopBaseApiUrl + 'GetCompletionListAllProduct/';
            },
            getCompletionListBaseProductUrl: function () {
                return _webshopBaseApiUrl + 'GetCompletionListBaseProduct/';
            },
            setDownloadPriceListUrl: function (url) {
                _downloadPriceListUrl = url;
            },
            getDownloadPriceListUrl: function () {

            },
            setShoppingCartBaseApiUrl: function (url) {
                _shoppingCartBaseApiUrl = url;
            },
            getShoppingCartApiUrl: function (url) {
                return _shoppingCartBaseApiUrl + url;
            }
        }
    }
    return {
        instance: function () {
            if (!_instance) {
                _instance = create();
            }
            return _instance;
        }
    }

})();

/*
companyGroup.utils.instance().setPictureBaseUrl('/...../...../...');
companyGroup.utils.instance().getPictureUrl('kep.png');
*/
//CompanyGroupCms.Utils = (function () {
//    //számformátum ellenőrzés
//    var ;
//    //szám formázása
//    var formatNumber = function (str) {
//        if (!validateNumber(str)) {
//            return str;
//        }
//        str += '';
//        x = str.split('.');
//        x1 = x[0];
//        x2 = x.length > 1 ? '.' + x[1] : '';
//        var rgx = /(\d+)(\d{3})/;
//        while (rgx.test(x1))
//            x1 = x1.replace(rgx, '$1' + ' ' + '$2');
//        return x1 + x2;
//    };
//    return {
//        ValidateNumber: validateNumber,
//        FormatNumber: formatNumber
//    }

//})();
//CompanyGroupCms.Utils.FormatNumber('3434343434');