(function ($) {

    //var companyGroup = companyGroup || {};

    companyGroupHelpers = function () {

        this.helpers({
            showSignInPanel: function () {
                $.fancybox({
                    href: '#div_login',
                    autoDimensions: true,
                    autoScale: false,
                    transitionIn: 'fade',
                    transitionOut: 'fade',
                    afterShow: function () {
                        //console.log('signin panel loaded');
                        $("#txt_password").val('');
                        $('#txt_username').val('');
                        $('#txt_username').focus();
                    }
                });
            },
            showForgetPasswordPanel: function () {
                $.fancybox({
                    href: '#div_forgetpassword',
                    autoDimensions: true,
                    autoScale: false,
                    transitionIn: 'fade',
                    transitionOut: 'fade',
                    afterShow: function () {
                        //console.log('forgetPassword panel loaded');
                        $("#txt_forgetpassword_username").val('');
                        $('#txt_forgetpassword_username').focus();
                    }
                });
            },
            sendForgetPassword: function (userName) {
                var data = {
                    UserName: userName
                };
                $.ajax({
                    type: "POST",
                    url: companyGroup.utils.instance().getContactPersonApiUrl('ForgetPwd'),
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        if (result) {
                            //console.log(result);
                            $('#forgetPasswordResult').show();
                            $('#forgetPasswordResult').html(result.Message);
                        }
                        else {
                            $('#forgetPasswordResult').show();
                            $('#forgetPasswordResult').html('ForgetPassword result failed');
                        }
                    },
                    error: function () {
                        $('#forgetPasswordResult').show();
                        $('#forgetPasswordResult').html('ForgetPassword call failed');
                    }
                });
            },
            beforeSignIn: function () {
                var error_msg = '';
                if ($("#txt_username").val() === '') {
                    error_msg += 'A bejelentkezési név kitöltése kötelező! <br/>';
                }
                if ($("#txt_password").val() === '') {
                    error_msg += 'A jelszó kitöltése kötelező!';
                }
                $("#login_errors").html(error_msg);

                return (error_msg === '');
            },
            signIn: function (userName, password, url, callback) {
                var data = {
                    UserName: userName,
                    Password: password
                };
                $.ajax({
                    type: "POST",
                    url: url,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        if (result.IsValidLogin) {
                            if (callback)
                                callback(result);
                        }
                        else {
                            $("#login_errors").html(result.ErrorMessage);
                            $("#login_errors").show();
                        }
                    },
                    error: function () {
                        alert('SignIn call failed!');
                    }
                });
            },
            signOut: function (url, callback) {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {},
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        if (!result.IsValidLogin) {
                            if (callback)
                                callback(result);
                        }
                        else {
                            //console.log('SignOut result failed');
                        }
                    },
                    error: function () {
                        //console.log('SignOut call failed');
                    }
                });
            },
            changeLanguage: function (language) {
                var data = {
                    Language: (language === '') ? 'EN' : language
                };
                $.ajax({
                    type: "POST",
                    url: companyGroup.utils.instance().getVisitorApiUrl('ChangeLanguage'),
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        //$("#inverse_language_id").html(result.Language);
                        var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
                        $('#cus_header1').html(visitorInfoHtml);
                    },
                    error: function () {
                        //console.log('ChangeLanguage call failed');
                    }
                });
            },
            changeCurrency: function (currency) {
                var data = {
                    Currency: (currency === '') ? 'HUF' : currency
                };
                $.ajax({
                    type: "POST",
                    url: companyGroup.utils.instance().getVisitorApiUrl('ChangeCurrency'),
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
                        $('#cus_header1').html(visitorInfoHtml);
                        //                        if (result.Currency === 'HUF') {
                        //                            $("#currency_huf").css('background-color', '#900');
                        //                            $("#currency_eur").css('background-color', '#666');
                        //                            $("#currency_usd").css('background-color', '#666');
                        //                        }
                        //                        else if (result.Currency === 'EUR') {
                        //                            $("#currency_huf").css('background-color', '#666');
                        //                            $("#currency_eur").css('background-color', '#900');
                        //                            $("#currency_usd").css('background-color', '#666');
                        //                        }
                        //                        else {
                        //                            $("#currency_huf").css('background-color', '#666');
                        //                            $("#currency_eur").css('background-color', '#666');
                        //                            $("#currency_usd").css('background-color', '#900');
                        //                        }
                    },
                    error: function () {
                        //console.log('ChangeCurrency call failed');
                    }
                });
            },
            searchByText: function (searchText) {
                window.location.href = companyGroup.utils.instance().getWebshopBaseUrl('Index/?q=' + searchText);
            },
            globalSearchOnFocus: function (self) {
                if ('keresés a termékek között' === self.val()) {
                    self.val('');
                }
            },
            globalSearchLostFocus: function (self) {
                var text = $.trim(self.val());
                if (!text) {
                    self.val('keresés a termékek között');
                }
            },
            globalSearchAutoComplete: function (request, response) {
                $.ajax({
                    type: "GET",
                    url: companyGroup.utils.instance().getCompletionListAllProductUrl(),
                    data: { Prefix: request.term },
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        if (result) {
                            var arr_suggestions = [];
                            $.each(result.Items, function (i, val) {
                                var inner_html = '<div class="list_item_container"><table border="0" cellpadding="5" cellspacing="0"><tr><td><div class="image"><a><img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(val.PictureId) + ' alt="" /></a></div></td><td><div class="label"><strong></strong></div><div class="description"><a href="#/details/' + val.ProductId + '/' + val.DataAreaId + '">' + val.ProductName + '</a></div></td></tr></table></div>';
                                arr_suggestions.push({ label: inner_html, value: val.ProductName });
                            });
                            response(arr_suggestions);
                        }
                        else {
                            //console.log(result);
                        }
                    },
                    error: function () {
                        //console.log('CompletionListServiceUrl failed');
                    }
                });
            },
            invoiceSumValues: function (callback) {
                $.ajax({
                    //console.log(context);
                    url: companyGroup.utils.instance().getInvoiceApiUrl('InvoiceSumValues'),
                    data: {},
                    type: "GET",
                    contentType: "application/json;charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    success: function (result) {
                        if (callback)
                            callback(result);
                    },
                    error: function () {
                        //console.log('GetVisitorInfo call failed');
                    }
                });
            }
        });
    };

})(jQuery);