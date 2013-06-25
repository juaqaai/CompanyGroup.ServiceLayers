var companyGroup = companyGroup || {};

companyGroup.newsletter = $.sammy(function () {

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('Hírlevelek - ');

    this.get('#/', function (context) {
        context.title('hírlevelek');
		
        companyGroup.newsletterDataService.getNewsletterList(
            { success: function (response) {
                var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), response.Visitor);
                $('#cus_header1').html(visitorInfoHtml);

                var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), response.Visitor);
                $('#usermenuContainer').html(usermenuHtml);

                $("#becomePartnerContainer").empty();
                var becomePartnerHtml = Mustache.to_html($('#becomePartnerTemplate').html(), response.Visitor);
                $('#becomePartnerContainer').html(becomePartnerHtml);

                $("#newsletterContainer").empty();
                var newsletterHtml = Mustache.to_html($('#newsletterTemplate').html(), response.Visitor);
                $('#newsletterContainer').html(newsletterHtml);

                $("#div_newsletter_container").empty();
                $("#newsletter_template").tmpl(response).appendTo("#div_newsletter_container");
            },
            error: function (response) {

            }
        });
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
    //keresés
    this.post('#/searchByTextFilter', function (context) {
        this.searchByText(context.params['txt_globalsearch']);
    });
	
	
	
	this.post('#/newsletterload', function (context) {
		$("#feeds").fadeIn('slow');
		$('#div_newsletter_container').css('width', '260px');
		$("#frame").attr("src", "https://www.hrp.hu/Articles/"+ context.params['linkId']);
		$("html, body").animate({ scrollTop: 0 }, 100);
	});
	
	this.get('#/newsletter_unload', function (context) {
		$("html, body").animate({ scrollTop: 0 }, 100);
		$("#feeds").hide();
		$(".scroll_close").hide();
		
		$("#frame").attr("src", "");
		$('#div_newsletter_container').css('width', '980px');
		
		context.redirect("#/unloaded")
	});

    $(function () {
        $('#fader img:not(:first)').hide();
        $('#fader img').css('position', 'absolute');
        $('#fader img').css('top', '0px');
        $('#fader img').css('left', '50%');
        $('#fader img').each(function () {
            var img = $(this);
            $('<img>').attr('src', $(this).attr('src')).load(function () {
                img.css('margin-left', -this.width / 2 + 'px');
            });
        });

        var pause = false;

        function fadeNext() {
            $('#fader img').first().fadeOut().appendTo($('#fader'));
            $('#fader img').first().fadeIn();
        }

        function fadePrev() {
            $('#fader img').first().fadeOut();
            $('#fader img').last().prependTo($('#fader')).fadeIn();
        }

        $('#fader, #next').click(function () {
            fadeNext();
        });

        $('#prev').click(function () {
            fadePrev();
        });

        $('#fader, .button').hover(function () {
            pause = true;
        }, function () {
            pause = false;
        });

        function doRotate() {
            if (!pause) {
                fadeNext();
            }
        }

        var rotate = setInterval(doRotate, 10000);
    });
});