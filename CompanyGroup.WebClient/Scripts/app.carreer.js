var companyGroup = companyGroup || {};

companyGroup.carreer = $.sammy(function () {

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('Karrier - ');

    this.get('#/', function (context) {
        context.title('kezdőoldal');
		$("#div_applyform").hide();
		$("#actual").show();
		$("#Kereskedő").show();
		$("#Termékmenedzser").show();
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

        //var options = {
        //target: '#span_uploadresult'   // target element(s) to be updated with server response 
        //beforeSubmit: showRequest,  // pre-submit callback 
        //success: showResponse  // post-submit callback 
        //url:       url         // override for form's 'action' attribute 
        //type:      type        // 'get' or 'post', override for form's 'method' attribute 
        //dataType:  null        // 'xml', 'script', or 'json' (expected server response type) 
        //clearForm: true        // clear all form fields after successful submit 
        //resetForm: true        // reset the form after successful submit 
        // $.ajax options can be used here too, for example: 
        //timeout:   3000 
        //};

        $('#form_upload').ajaxForm({
            beforeSubmit: function (formData, jqForm, options) {
                $('#span_uploadresult').html('Küldés folyamatban...');
                for (var i = 0; i < formData.length; i++) {
                    if (!formData[i].value) {
                        $('#span_uploadresult').html('File megadása kötelező...');
                        return false;
                    }
                }
            },
            success: function (data) {
                $('#span_uploadresult').html(data.FileName);
            }
        });

        var months = new Array("01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12");
        var days = new Array("01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31");
        var currentTime = new Date();
        var currentDate = currentTime.getFullYear() + "." + months[currentTime.getMonth()] + "." + days[currentTime.getDate() - 1] + ".";
        $('#label_currentdate').html(currentDate);
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
    this.get('#/applyform/:jobtitle', function (context) {
        var div = '#' + context.params['jobtitle'];
        $(div).toggle("fast");
		$("#div_applyform").hide();
		
    });
    this.get('#/showapplyform/:jobtitle', function (context) {
        $("#select_position").val(context.params['jobtitle']);
        $("#div_applyform").show();
		 var div = '#' + context.params['jobtitle'];
 		$("#actual").hide("fast");
		$("html, body").animate({ scrollTop: 0 }, 100);
	
    });
	    this.get('#/vissza', function (context) {
		$("#div_applyform").hide();
 		$("#actual").show();
		$("html, body").animate({ scrollTop: 0 }, 100);
		
    });
    //jelentkezési lap elküldése
    this.post('#/applyforjob', function (context) {
        var data = {
            FirstName: context.params['txt_firstname'],
            LastName: context.params['txt_lastname'],
            PlaceOfBirth: context.params['txt_placeofbirth'],
            DayfBirth: context.params['txt_dayofbirth'],
            PermanentAddress: context.params['txt_permanentaddress'],
            TemporaryAddress: context.params['txt_temporaryaddress'],
            Phone: context.params['txt_phone'],
            Position: context.params['select_position'],
            Email: context.params['txt_email'],
            Message: context.params['txt_message'],
            UploadFileName: $("#span_uploadresult").html(),
            CheckReference: context.params['chk_accept']
        };

        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getCompanyUrl('ApplyForJob'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $("#span_applyresult").html('A jelentkezési adatok rögzítése sikeresen megtörtént!');
                }
                else {
                    $("#span_applyresult").html('A jelentkezési adatok rögzítése nem sikerült!');
                }
            },
            error: function () {
                alert('ApplyForTheJob call failed!');
            }
        });
    });
    //jelentkezési lap elküldés előtti esemény
    this.before({ only: { verb: 'post', path: '#/applyforjob'} }, function (e) {
        var error_msg = '';
        if ($("#txt_firstname").val() == '') {
            error_msg += 'A vezetéknév kitöltése kötelező! <br/>';
        }
        if ($("#txt_lastname").val() == '') {
            error_msg += 'A keresztnév kitöltése kötelező! <br/>';
        }
        if ($("#txt_placeofbirth").val() == '') {
            error_msg += 'A születési hely kitöltése kötelező! <br/>';
        }
        if ($("#txt_dayofbirth").val() == '') {
            error_msg += 'A születési idő kitöltése kötelező! <br/>';
        }
        if ($("#txt_permanentadress").val() == '') {
            error_msg += 'Az állandó cím kitöltése kötelező! <br/>';
        }
        if ($("#txt_temporaryaddress").val() == '') {
            error_msg += 'Az ideiglenes cím kitöltése kötelező! <br/>';
        }
        if ($("#txt_phone").val() == '') {
            error_msg += 'A telefonszám kitöltése kötelező! <br/>';
        }
        if ($("#txt_email").val() == '') {
            error_msg += 'Az email cím kitöltése kötelező! <br/>';
        }
        $("#span_applyresult").html(error_msg);
        return (error_msg === '');
    });
});