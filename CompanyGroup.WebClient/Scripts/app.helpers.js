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
                    beforeClose: function () { 
                    //console.log('signin panel closed'); 
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
            }
        });
    };

})(jQuery);