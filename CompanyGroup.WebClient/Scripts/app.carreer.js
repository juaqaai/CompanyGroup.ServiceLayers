var companyGroup = companyGroup || {};

companyGroup.carreer = $.sammy(function () {

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('Karrier - ');

    this.get('#/', function (context) {
        context.title('kezdőoldal');
    });
    //események
    this.bind('run', function (e, data) {
        var context = this;
        //keresés névre, vagy cikkszámra
        $("#txt_globalsearch").live('focus', function () {
            context.globalSearchOnFocus($(this));
        }).live('blur', function () {
            context.globalSearchLostFocus($(this));
        });
        //keresés autocomplete
        $("#txt_globalsearch").autocomplete({
            source: function (request, response) {
                context.globalSearchAutoComplete(request, response);
            },
            minLength: 2,
            html: 'html'
        });
    });
    this.get('#/authenticated', function (context) {
        //console.log('authenticated');
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
        this.signIn(context.params['txt_username'], context.params['txt_password'], companyGroup.utils.instance().getVisitorApiUrl('SignIn'), function (result) {
            $.fancybox.close();

            $("#cus_header1").empty();
            var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result.Visitor);
            $('#cus_header1').html(visitorInfoHtml);

            $("#usermenuContainer").empty();
            var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result.Visitor);
            $('#usermenuContainer').html(usermenuHtml);

            context.redirect('#/authenticated');
        });

    });
    //kilépés
    this.get('#/signOut', function (context) {
        this.signOut(companyGroup.utils.instance().getVisitorApiUrl('SignOut'), function (result) {
            $("#cus_header1").empty();
            $("#usermenuContainer").empty();
            var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result.Visitor);
            $('#cus_header1').html(visitorInfoHtml);
            var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result.Visitor);
            $('#usermenuContainer').html(usermenuHtml);

        });
    });
    //keresés
    this.post('#/searchByTextFilter', function (context) {
        this.searchByText(context.params['txt_globalsearch']);
    });
    this.post('#/apply', function (context) {
        var data = {
            FirstName: context.params['txt_firstname'],
            LastName: context.params['txt_lastname'],
            PlaceOfBirth: context.params['txt_placeofbirth'],
            DayfBirth: context.params['txt_dayofbirth'],
            PermanentAddress: context.params['txt_permanentaddress'],
            TemporaryAddress: context.params['txt_temporaryaddress'],
            Phone: context.params['txt_phone'],
            Email: context.params['txt_email'],
            Message: context.params['txt_message']
        };
        //context.params['txt_upload'];
        //context.params['chk_accept'];
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getVisitorApiUrl('ApplyForTheJob'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {

                }
                else {
                    $("#login_errors").html(result.ErrorMessage);
                    $("#login_errors").show();
                }
            },
            error: function () {
                alert('ApplyForTheJob call failed!');
            }
        });
    });
});