﻿var companyGroup = companyGroup || {};

companyGroup.company = $.sammy(function () {

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('Céginformáció - ');

    this.get('#/', function (context) {
        context.title('kezdőoldal');
    });

    this.get('#/authenticated', function (context) {
        console.log('authenticated');
    });
    //bejelentkezés panel megmutatása
    this.get('#/showSignInPanel', function (context) {
        this.showSignInPanel();
    });
    //nyelv megváltoztatása
    this.get('#/changeLanguage/:language', function (context) {
        this.changeLanguage(context.params['language']);
    });
    //pénznem megváltoztatása
    this.get('#/changeCurrency/:currency', function (context) {
        this.changeCurrency(context.params['currency']);
    });
    //belépési adatok ellenörzése
    this.before({ only: { verb: 'post', path: '#/signin'} }, function (e) {
        return this.beforeSignIn();
    });
    //bejelentkezés
    this.post('#/signin', function (context) {
        this.signIn(context.params['txt_username'], context.params['txt_password'], companyGroup.utils.instance().getCustomerApiUrl('SignIn'), function (result) {
            $.fancybox.close();

            $("#cus_header1").empty();
            var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result.Visitor);
            $('#cus_header1').html(visitorInfoHtml);

            $("#usermenuContainer").empty();
            var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result.Visitor);
            $('#usermenuContainer').html(usermenuHtml);

            $("#quickmenuContainer").empty();
            var quickmenuHtml = Mustache.to_html($('#quickmenuTemplate').html(), result.Visitor);
            $('#quickmenuContainer').html(quickmenuHtml);

            context.redirect('#/authenticated');
        });

    });
    //kilépés
    this.get('#/signOut', function (context) {
        this.signOut(companyGroup.utils.instance().getCustomerApiUrl('SignOut'), function (result) {
            $("#cus_header1").empty();
            $("#visitorInfoTemplate").tmpl(result.Visitor).appendTo("#cus_header1");
            $("#quickmenuContainer").empty();
            $("#quickmenuTemplate").tmpl(result.Visitor).appendTo("#quickmenuContainer");
            $("#usermenuContainer").empty();
            $("#usermenuTemplate").tmpl(result.Visitor).appendTo("#usermenuContainer");
        });
    });

});