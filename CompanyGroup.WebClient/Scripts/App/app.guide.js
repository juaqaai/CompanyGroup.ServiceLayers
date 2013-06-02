var companyGroup = companyGroup || {};

companyGroup.guide = $.sammy(function () {

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('HRP/BSC Útmutató - ');

    this.get('#/', function (context) {
        context.title('felhasználási feltételek');
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
	this.get('#/closed', function (context) {
        //console.log(context);
		$.fancybox.close()
    });

    this.get('#/termOfUse', function (context) {
        setVisibility('termOfUse');
        context.title('felhasználási feltételek');
    });

    this.get('#/dataProtection', function (context) {
        setVisibility('dataProtection');
        context.title('adatvédelem');
    });

    this.get('#/deliveryAndPaymentOptions', function (context) {
        setVisibility('deliveryAndPaymentOptions');
        context.title('szállítási és fizetési feltételek');
    });

    this.get('#/returnItemConditions', function (context) {
        setVisibility('returnItemConditions');
        context.title('visszáru feltételek');
    });

    this.get('#/garantyValidation', function (context) {
        setVisibility('garantyValidation');
        context.title('garancia érvényesítése');
    });

    this.get('#/shoppingInTheWebshop', function (context) {
        setVisibility('shoppingInTheWebshop');
        context.title('Vásárlás a webáruházban');
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
            $("#visitorInfoTemplate").tmpl(result.Visitor).appendTo("#cus_header1");
            $("#usermenuContainer").empty();
            $("#usermenuTemplate").tmpl(result.Visitor).appendTo("#usermenuContainer");
        });
    });
    
    	 var setVisibility = function (controlId) {
        if (controlId == "termOfUse") { $("#termOfUse").fadeIn('slow'); }
        else { $("#termOfUse").hide(); }

        if (controlId == "dataProtection") { $("#dataProtection").fadeIn('slow'); }
        else { $("#dataProtection").hide(); }

        if (controlId == "deliveryAndPaymentOptions") {
            $("#deliveryAndPaymentOptions").fadeIn('slow');
        } else {
            $("#deliveryAndPaymentOptions").hide();
        }
        if (controlId == "returnItemConditions") {
            $("#returnItemConditions").fadeIn('slow');
        } else {
            $("#returnItemConditions").hide();
        }
        if (controlId == "garantyValidation") {
            $("#garantyValidation").fadeIn('slow');
        } else {
            $("#garantyValidation").hide();
        }
        if (controlId == "shoppingInTheWebshop") {
            $("#shoppingInTheWebshop").fadeIn('slow');
        } else {
            $("#shoppingInTheWebshop").hide();
        }
    }
    //keresés
    this.post('#/searchByTextFilter', function (context) {
        this.searchByText(context.params['txt_globalsearch']);
    });
});