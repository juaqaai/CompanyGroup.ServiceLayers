var companyGroup = companyGroup || {};

companyGroup.registration = $.sammy(function () {

    //this.use(Sammy.Mustache);

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.get('#/closed', function (context) {
        //console.log(context);
        $.fancybox.close()
    });

    //szerződési feltételek betöltése (regisztrációs adatok elkérése, template-ek feltöltése)
    this.get('#/', function (context) {

        $.ajax({
            url: companyGroup.utils.instance().getRegistrationApiUrl('GetRegistrationData'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $("#ugyintezo_container").empty();
                var ugyintezoHtml = Mustache.to_html($('#ugyintezo_template').html(), response.Visitor);
                $('#ugyintezo_container').html(ugyintezoHtml);

                $("#compdata_container").empty();
                var compdataHtml = Mustache.to_html($('#compdata_template').html(), response.Visitor);
                $('#compdata_container').html(compdataHtml);

                $("#webadmin_container").empty();
                var webadminHtml = Mustache.to_html($('#webadmin_template').html(), response.Visitor);
                $('#webadmin_container').html(webadminHtml);

                $("#kapcsolattarto_container").empty();
                var kapcsolattartoHtml = Mustache.to_html($('#kapcsolattarto_template').html(), response.Visitor);
                $('#kapcsolattarto_container').html(kapcsolattartoHtml);
				$(".select_position2").chosen();
				

				
                $("#datachk_container").empty();
                var datachkHtml = Mustache.to_html($('#datachk_template').html(), response.Visitor);
                $('#datachk_container').html(datachkHtml);

                if (response.Visitor.IsValidLogin) {
                    context.title(' HRP/BSC Cégadatmódosítás');
                }
                else {
                    context.title(' HRP/BSC Regisztráció - szerződési feltételek');
                }

                //                if (response.Visitor.IsValidLogin) {
                //                    context.title(' HRP/BSC Regisztráció - szerződési feltételek');
                //                    $('#tabs-2-button').hide();
                //                    $('#tabs-3-button').hide();
                //                    $('#tabs-4-button').hide();
                //                    $('#tabs-5-button').hide();
                //                    $('#tabs-6-button').hide();
                //                    $('#tabs-7-button').hide();
                //                    $('#tab_2_ok').hide();
                //                    $('#tab_3_ok').hide();
                //                    $('#tab_4_ok').hide();
                //                    $('#tab_5_ok').hide();
                //                    $('#tab_6_ok').hide();
                //                    $('#tab_7_ok').hide();
                //                    $('#modify_mgs').hide();
                //                    $("#mail_address").hide();
                //                    $("#del_address").hide();
                //                    $("#contactperson").hide();
                //                    $("#finish_reg").hide()
                //                    $("#tabs-7").hide();
                //                }
                //                else {
                //                    context.title(' HRP/BSC Cégadatmódosítás');
                //                    $("#tabs_7_title").hide();
                //                    $('#tabs-1-button').show();
                //                    $('#tabs-2-button_modosit').hide();
                //                    $('#tabs-3-button_modosit').hide();
                //                    $('#tabs-4-button_modosit').hide();
                //                    $('#tabs-5-button_modosit').hide();
                //                    $('#tabs-6-button_modosit').hide();
                //                    $('#tabs-7-button_modosit').hide();
                //                    $('#tab_2_ok').hide();
                //                    $('#tab_3_ok').hide();
                //                    $('#tab_4_ok').hide();
                //                    $('#tab_5_ok').hide();
                //                    $('#tab_6_ok').hide();
                //                    $('#tab_7_ok').hide();
                //                    $('#modify_mgs').hide();
                //                    $("#mail_address").show();
                //                    $("#del_address").show();
                //                    $("#contactperson").show();
                //                    $("#finish_reg").hide()
                //                    $("#tabs-7").hide();
                //                    $("#add_new_delivery").hide();
                //                    $("#add_new_contact").hide();
                //                    $("#chk_del_address").hide();
                //                    $("#chk_mail_address").hide();
                //                    $("#chk_contactperson_div").hide();
                //                    $("#address_tip").hide();
                //                    $("#address_tip2").hide();
                //                }

                initRegistrationData(response);
                setTabsVisibility(1, response.Visitor.IsValidLogin);
            },
            error: function () {
                //console.log('GetVisitorInfo call failed');
            }
        });
    });
    //események
    this.bind('run', function (e, data) {
        var context = this;
        $("#select_webadminposition").chosen({ no_results_text: "Nincs találat" });
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
        //szerződési feltételek elfogadása
        //        $("#chk_accept").change(function () {
        //            if ($('#chk_accept').is(':checked')) {
        //                document.location.hash = '/datarecording';
        //                $("#contract").hide('slow');
        //            }
        //            else {
        //                document.location.hash = '/';
        //                $('#tabs-2').hide(500);
        //                $("#contract").show('slow');
        //            }
        //        });

        //adatok ellenörzésének megerősítése

        $("#chk_checkdata").live('change', function () {
            context.trigger('checkData', { Checked: $(this).is(':checked') });
        });
        $("#chk_accept").live('change', function () {
            context.trigger('acceptContract', { Accepted: $(this).is(':checked') });
        });
        $("#chk_mail_address").live('change', function () {
            context.trigger('setMailAddress', { Checked: $(this).is(':checked') });
        });
        $("#chk_del_address").live('change', function () {
            context.trigger('setDeliveryAddress', { Checked: $(this).is(':checked') });
        });
        $("#chk_contactperson").live('change', function () {
            context.trigger('setContactPerson', { Checked: $(this).is(':checked') });
        });
        $("#btn_showdatacheck").live('click', function () {
            context.trigger('showDataCheck');
        });
        $("#btn_showwebadmin").live('click', function () {
            context.trigger('showwebadmin');
        });
        $('#form_upload').ajaxForm({
            beforeSubmit: function (formData, jqForm, options) {
                $('#span_signature_entity_file').html('Küldés folyamatban...');
                /*for (var i = 0; i < formData.length; i++) {
                    if (!formData[i].value) {
                        $('#span_signature_entity_file').html('File megadása kötelező...');
                        return false;
                    }
                }*/
				return true;

            },
            success: function (data) {
                $('#span_signature_entity_file').html(data);
				$.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Sikeres feltöltés</H2></div>', {
                'autoDimensions': true,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic',
                'closeBtn': 'true',
                'changeFade': 0,
                'speedIn': 300,
                'speedOut': 300,
                'width': '150%',
                'height': '150%',
                'autoScale': true
            });
            }
        });
        $("#tabs-1-button").live('click', function () {
            $('#tabs-1').toggle("slow");
        });

        $("#tabs-2-button").live('click', function () {
            $('#tabs-2').toggle("slow");
            $("#hr_1").toggle("slow");
        });

        $("#tabs-3-button").live('click', function () {
            $('#tabs-3').toggle("slow");
            $("#hr_2").toggle("slow");
        });
        $("#tabs-4-button").live('click', function () {
            $('#tabs-4').toggle("slow");
            $("#hr_3").toggle("slow");
        });
        $("#tabs-5-button").live('click', function () {
            $('#tabs-5').toggle("slow");
            $("#hr_4").toggle("slow");
        });
        $("#tabs-6-button").live('click', function () {
            $('#tabs-6').toggle("slow");
            $("#hr_5").toggle("slow");
        });

        $("#tabs-1-button_modosit").live('click', function () {
            $('#tabs-1').toggle("slow");
        });
        $("#tabs-2-button_modosit").live('click', function () {
            $('#tabs-2').toggle("slow");
            $("#hr_1").toggle("slow");
        });

        $("#tabs-3-button_modosit").live('click', function () {
            $('#tabs-3').toggle("slow");
            $("#hr_2").toggle("slow");
        });
        $("#tabs-4-button_modosit").live('click', function () {
            $('#tabs-4').toggle("slow");
            $("#hr_3").toggle("slow");
        });
        $("#tabs-5-button_modosit").live('click', function () {
            $('#tabs-5').toggle("slow");
            $("#hr_4").toggle("slow");
        });
        $("#tabs-6-button_modosit").live('click', function () {
            $('#tabs-6').toggle("slow");
            $("#hr_5").toggle("slow");
        });
    });
    //adatok ellenörzése
    this.get('#/allowdatacheck', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        $("#tabs-5").show(500);
        $("#tabs-4").show(500);
        $("#tabs-3").show(500);
        $("#tabs-2").show(500);
        $("#hr_1").hide(500);
        $("#hr_2").hide(500);
        $("#hr_3").hide(500);
        $("#hr_4").hide(500);
        $("#hr_5").hide(500);

    });
    this.bind('checkData', function (e, data) {
        var context = this;
        if (data.Checked) {
            if ($('#chk_contactpersonallowreceiptofgoods').is(':checked') || $('#chk_webadminallowreceiptofgoods').is(':checked')) {
                context.redirect('#/checked');
                context.title('HRP/BSC Regisztráció - adatok ellenörzése megtörtént');
            }
            else {
                $.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Figyelem!</H2><p>Legalább egy főnél be kell jelölve lenni, hogy árut átvehet!</p></div>', {
                    'autoDimensions': true,
                    'transitionIn': 'elastic',
                    'transitionOut': 'elastic',
                    'closeBtn': 'true',
                    'changeFade': 0,
                    'speedIn': 300,
                    'speedOut': 300,
                    'width': '150%',
                    'height': '150%',
                    'autoScale': true,
                    beforeClose: function () { $('#chk_filterByHrp').attr('checked', true); }
                });
                $("#chk_checkdata").prop('checked', false);
                context.redirect('#/notchecked');
                context.title('HRP/BSC Regisztráció - adatok ellenörzése nem történt meg');
                $('#tab_6_ok').hide(500);
            }

        } else {
            context.redirect('#/notchecked');
            context.title('HRP/BSC Regisztráció - adatok ellenörzése nem történt meg');
            $('#tab_6_ok').hide(500);
        }
    });
    //szerződési feltételek elfogadása (ha elfogadás történt, akkor az adatfeltöltésre irányít a rendszer)
    this.bind('acceptContract', function (e, data) {
        var context = this;
        if (data.Accepted) {
            $("#contract").hide('slow');
            context.redirect('#/datarecording');
        } else {
            $('#tabs-2').hide(500);
            $("#contract").show('slow');
            context.redirect('#/');
        }
    });
    this.bind('setMailAddress', function (e, data) {
        var context = this;
        if (data.Checked) {
            $('#mail_address').show("slow");
        } else {
            $('#mail_address').hide("slow");
        }
    });
    this.bind('setDeliveryAddress', function (e, data) {
        var context = this;
        if (data.Checked) {
            $('#del_address').show("slow");
        } else {
            $('#del_address').hide("slow");
        }
    });
    this.bind('setContactPerson', function (e, data) {
        var context = this;
        if (data.Checked) {
            $('#contactperson').show("slow");
      $(".select_position2").chosen();
			
        } else {
            $('#contactperson').hide("slow");
        }
    });
    this.bind('showDataCheck', function (e, data) {
		
        var context = this;
		var submit_contact ='';
			
		 if ($("#txt_contactpersonfirstname").val() != '' || $("#txt_contactpersonlastname").val() != '' || $("#txt_contactpersonemail").val() != ''  ) {
            
			submit_contact = false;
			$.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Figyelem!</H2><p>A kapcsolattartó kitőltése nem lett befejezve.</p></div>', {
                    'autoDimensions': true,
                    'transitionIn': 'elastic',
                    'transitionOut': 'elastic',
                    'closeBtn': 'true',
                    'changeFade': 0,
                    'speedIn': 300,
                    'speedOut': 300,
                    'width': '150%',
                    'height': '150%',
                    'autoScale': true,
                });
        }
		else {
			
			submit_contact = true;
		
		}
		
		if ($("#txt_contactpersonfirstname").val() != '' && $("#txt_contactpersonlastname").val() != '' && $("#txt_contactpersonemail").val() != '') {
             submit_contact = true;
			 $.fancybox.close();
			 
        } 
		
		if (submit_contact) {context.redirect('#/showdatacheck');
        }
        
    });
    this.bind('showwebadmin', function (e, data) {
        var context = this;
        context.redirect('#/companydata');
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
    //bejelentkezés ()
    this.post('#/signin', function (context) {
        this.signIn(context.params['txt_username'], context.params['txt_password'], companyGroup.utils.instance().getVisitorApiUrl('SignIn'), function (result) {
            if (result.IsValidLogin) {
                $.fancybox.close();
                $("#cus_header1").empty();
                var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
                $('#cus_header1').html(visitorInfoHtml);
                $("#usermenuContainer").empty();
                var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result);
                $('#usermenuContainer').html(usermenuHtml);
                context.redirect('#/');
            }
            else {
                $("#login_errors").html(result.ErrorMessage);
                $("#login_errors").show();
            }
        });

    });
    //kilépés
    this.get('#/signOut', function (context) {
        this.signOut(companyGroup.utils.instance().getVisitorApiUrl('SignOut'), function (result) {
            $("#cus_header1").empty();
            $("#usermenuContainer").empty();
            var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
            $('#cus_header1').html(visitorInfoHtml);
            var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result);
            $('#usermenuContainer').html(usermenuHtml);
            context.redirect('#/');
        });
    });
    //keresés
    this.post('#/searchByTextFilter', function (context) {
        this.searchByText(context.params['txt_globalsearch']);
    });
    //ellenörzés    
    this.get('#/checked', function (context) {

        this.getVisitorInfo(function (result) {
            if (result.IsValidLogin == 0) {
                $("#tabs-1").hide(500);
                $("#tabs-2").hide(500);
                $("#tabs-3").hide(500);
                $("#tabs-4").hide(500);
                $("#tabs-5").hide(500);
                $("#tabs-6").hide(500);
                $("#finish_reg").hide(500);
                $("#check_span").hide(500);
                $("#tabs_7_title").show(500);
                $("#tabs-7").show(500);
                $('#tab_6_ok').show(500);
                $('#tabs-6-button').show(500);
                $("#hr_1").show(500);
                $("#hr_2").show(500);
                $("#hr_3").show(500);
                $("#hr_4").show(500);
                $("#hr_5").show(500);
                context.redirect('#/saveregistration_print')
            }
            else {
                //itt van az adatok vizsgálata hogy egyezik a kis módosításhoz..
                var contract_modify1 = 0, contract_modify2 = 0, contract_modify2_2 = 0, contract_modify2_3 = 0, contract_modify3 = 0, contract_modify4 = 0, contract_modify5 = 0, contract_modify6 = 0
                if ($("#customername").text() === $("#newcustomername").text() || $("#newcustomername").text() == "") { contract_modify1 = 1 } else { contract_modify1 = 0 }
                if ($("#vatnumber").text() === $("#newvatnumber").text() || $("#newvatnumber").text() == "") { contract_modify2 = 1 } else { contract_modify2 = 0 }
                if ($("#vatnumber2").text() === $("#newvatnumber2").text() || $("#newvatnumber2").text() == "") { contract_modify2_2 = 1 } else { contract_modify2_2 = 0 }
                if ($("#vatnumber3").text() === $("#newvatnumber3").text() || $("#newvatnumber3").text() == "") { contract_modify2_3 = 1 } else { contract_modify2_3 = 0 }
                if ($("#invoicezipcode").text() === $("#newinvoicezipcode").text() || $("#newinvoicezipcode").text() == "") { contract_modify3 = 1 } else { contract_modify3 = 0 }
                if ($("#invoicecity").text() === $("#newinvoicecity").text() || $("#newinvoicecity").text() == "") { contract_modify4 = 1 } else { contract_modify4 = 0 }
                if ($("#invoicestreet").text() === $("#newinvoicestreet").text() || $("#newinvoicestreet").text() == "") { contract_modify5 = 1 } else { contract_modify5 = 0 }
                if ($("#invoicecountry").text() === $("#newinvoicecountry").text() || $("#newinvoicecountry").text() == "") { contract_modify6 = 1 } else { contract_modify6 = 0 }
                if (contract_modify1 === 1 && contract_modify2 === 1 && contract_modify2_2 === 1 && contract_modify2_3 === 1 && contract_modify3 === 1 && contract_modify4 === 1 && contract_modify5 === 1 && contract_modify6 === 1) {
                    $("#tabs-1").hide(500);
                    $("#tabs-2").hide(500);
                    $("#tabs-3").hide(500);
                    $("#tabs-4").hide(500);
                    $("#tabs-5").hide(500);
                    $("#tabs-6").hide(500);
                    $("#finish_reg").hide(500);
                    $("#check_span").hide(500);
                    $("#tabs-7").hide(500);
                    $('#tab_6_ok').show(500);
                    $('#tabs-6-button').show(500);
                    $("#hr_1").show(500);
                    $("#hr_2").show(500);
                    $("#hr_3").show(500);
                    $("#hr_4").show(500);
                    $("#hr_5").show(500);
                    context.redirect('#/saveregistration')
                }
                else {
                    $("#tabs-1").hide(500);
                    $("#tabs-2").hide(500);
                    $("#tabs-3").hide(500);
                    $("#tabs-4").hide(500);
                    $("#tabs-5").hide(500);
                    $("#tabs-6").hide(500);
                    $("#finish_reg").hide(500);
                    $("#check_span").hide(500);
                    $("#tabs_7_title").show(500);
                    $("#tabs-7").show(500);
                    $('#tab_6_ok').show(500);
                    $('#tabs-6-button').show(500);
                    $("#hr_1").show(500);
                    $("#hr_2").show(500);
                    $("#hr_3").show(500);
                    $("#hr_4").show(500);
                    $("#hr_5").show(500);
                    context.redirect('#/saveregistration_print')
                }
            }
        });
    });
    this.get('#/notchecked', function (context) {
        $("#tabs-7").hide(500);
    });
    //kapcsolattartók panel után tovább-ot választottak
    this.get('#/showdatacheck', function (context) {

        //kapcsolattartó mentés

        var data = {
            Id: '',
            AllowOrder: $("#chk_contactpersonalloworder").is(':checked'),
            AllowReceiptOfGoods: $("#chk_contactpersonallowreceiptofgoods").is(':checked'),
            ContactPersonId: '',
            Email: $("#txt_contactpersonemail").val(),
            EmailArriveOfGoods: $("#chk_contactpersonemailarriveofgoods").is(':checked'),
            EmailOfDelivery: $("#chk_contactpersonemailofdelivery").is(':checked'),
            EmailOfOrderConfirm: $("#chk_contactpersonemailoforderconfirm").is(':checked'),
            FirstName: $("#txt_contactpersonfirstname").val(),
            InvoiceInfo: $("#chk_contactpersoninvoiceinfo").is(':checked'),
            LastName: $("#txt_contactpersonlastname").val(),
            LeftCompany: false,
            Newsletter: $("#chk_contactpersonnewsletter2").is(':checked'),
            Password: $("#txt_contactpersonpassword").val(),
            PriceListDownload: $("#chk_contactpersonpricelistdownload").is(':checked'),
            RecId: 0,
            RefRecId: 0,
            SmsArriveOfGoods: false,
            SmsOfDelivery: false,
            SmsOrderConfirm: false,
            Telephone: $("#txt_contactpersonphone").val(),
            UserName: $("#txt_contactpersonnusername").val(),
            Positions: $(".select_position2").val()
        };
        //data.RegistrationNumber = $("#txtRegistrationNumber").val();
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('SaveContactPerson'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {//ContactPersonId,FirstName,LastName,AllowOrder ,AllowReceiptOfGoods ,SmsArriveOfGoods,SmsOrderConfirm,SmsOfDelivery ,EmailArriveOfGoods ,EmailOfOrderConfirm ,EmailOfDelivery ,WebAdmin ,PriceListDownload 
                    ///InvoiceInfo,UserName,Password ,Newsletter,Telephone,Email ,LeftCompany,Id
                    $("#contactPersonContainer").empty();
                    var html = Mustache.to_html($('#contactPersonTemplate').html(), result);
                    $('#contactPersonContainer').html(html);
                    context.title('HRP/BSC Regisztráció - adatok ellenőrzése');
                    $("#add_new_contact").show();
                    $("#add_new_contact_btn").hide();
                    setTabsVisibility(6);
                }
                else {
                    //console.log('addContactPerson result failed');
                }
            },
            error: function () {
                //console.log('addContactPerson call failed');
            }
        });

    });
    //adatrögzítő adatainak betöltése (új regisztráció hozzáadása)
    this.get('#/datarecording', function (context) {
        ////console.log(context);
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('AddNew'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (response) {
                if (response) {
                    if (response.Visitor.IsValidLogin) {
                        initRegistrationData(response);
                    }
                    setTabsVisibility(2, response.Visitor.IsValidLogin);
                    context.title('HRP/BSC Regisztráció - kitöltő adatai');
                }
                else {
                    //console.log('addNew result failed');
                }
            },
            error: function () {
                //console.log('addNew call failed');
            }
        });
    });
    //adatrögzítő adatainak validálása
    this.before({ only: { verb: 'post', path: '#/datarecorder'} }, function (e) {

        var error_msg = '';

        if ($('#txt_datarecordingname').val() === '') {
            error_msg += 'Az adatrögzítő név kitöltése kötelező! <br/>';
        }
        if ($('#txt_datarecordingphone').val() === '') {
            error_msg += 'Az adatrögzítő telefonszám kitöltése kötelező! <br/>';
        }
        if ($('#txt_datarecordingemail').val() === '') {
            error_msg += 'Az adatrögzítő email cím kitöltése kötelező! <br/>';
        }

        $("#span_datarecordingerror").html(error_msg);

        return (error_msg === '');
    });
    //adatrögzítő adatainak mentése, cégregisztrációs adatlap betöltése
    this.post('#/datarecorder', function (context) {
        ////console.log(context);$('form').serialize()
        var data = {
            Email: context.params['txt_datarecordingemail'],
            Name: context.params['txt_datarecordingname'],
            Phone: context.params['txt_datarecordingphone']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('UpdateDataRecording'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (response) {
                if (response) {
                    if (response.Succeeded) {
                        setTabsVisibility(3, response.Visitor.IsValidLogin);

                        // A korábban megadott adatokat	elmentjük
                        context.title('Regisztráció - törzsadatok');
                        var customername = $("#txt_customername").val();
                        var vatnumber = $("#txt_vatnumber").val();
                        var vatnumber2 = $("#txt_vatnumber2").val();
                        var vatnumber3 = $("#txt_vatnumber3").val();
                        var invoicecountry = $("#select_invoicecountry").val();
                        var invoicezipcode = $("#txt_invoicezipcode").val();
                        var invoicecity = $("#txt_invoicecity").val();
                        var invoicestreet = $("#txt_invoicestreet").val();

                        $("#customername").text(customername);
                        $("#vatnumber").text(vatnumber);
                        $("#vatnumber2").text(vatnumber2);
                        $("#vatnumber3").text(vatnumber3);
                        $("#invoicecountry").text(invoicecountry);
                        $("#invoicecity").text(invoicecity);
                        $("#invoicestreet").text(invoicestreet);
                        $("#invoicezipcode").text(invoicezipcode);

                    } else {
                        alert(response.Message);
                    }
                }
                else {
                    //console.log('updateDataRecording result failed');
                }
            },
            error: function () {
                //console.log('updateDataRecording call failed');
            }
        });
    });
    this.get('#/companydata', function (context) {
		
		$("form#addbankaccount").submit(); 
		
		
        
        var submit_delivery ='';
		 if ($("#txt_deliveryaddresszipcode").val() != '' || $("#txt_deliveryaddresscity").val() != '' || $("#txt_deliveryaddressstreet").val() != '') {
            
			submit_delivery = false;
			$.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Figyelem!</H2><p>A szállítási cím kitőltése nem lett befejezve.</p></div>', {
                    'autoDimensions': true,
                    'transitionIn': 'elastic',
                    'transitionOut': 'elastic',
                    'closeBtn': 'true',
                    'changeFade': 0,
                    'speedIn': 300,
                    'speedOut': 300,
                    'width': '150%',
                    'height': '150%',
                    'autoScale': true,
                });
        }
		else {
			
			submit_delivery = true;
		
		}
		
		if ($("#txt_deliveryaddresszipcode").val() != '' && $("#txt_deliveryaddresscity").val() != '' && $("#txt_deliveryaddressstreet").val() != '') {
             submit_delivery = true;
			 $.fancybox.close();
			 
        } 
		
		if (submit_delivery) {
		$("form#form_companydata").submit();
		}
	   
	   
        // validálás után az uj adatokat olvasom be
        var newcustomername = $("#txt_customername").val();
        var newvatnumber = $("#txt_vatnumber").val();
        var newvatnumber2 = $("#txt_vatnumber2").val();
        var newvatnumber3 = $("#txt_vatnumber3").val();
        var newinvoicecountry = $("#select_invoicecountry").val();
        var newinvoicezipcode = $("#txt_invoicezipcode").val();
        var newinvoicecity = $("#txt_invoicecity").val();
        var newinvoicestreet = $("#txt_invoicestreet").val();

        $("#newcustomername").text(newcustomername);
        $("#newvatnumber").text(newvatnumber);
        $("#newvatnumber2").text(newvatnumber2);
        $("#newvatnumber3").text(newvatnumber3);
        $("#newinvoicecountry").text(newinvoicecountry);
        $("#newinvoicecity").text(newinvoicecity);
        $("#newinvoicestreet").text(newinvoicestreet);
        $("#newinvoicezipcode").text(newinvoicezipcode);
    });
    //cégregisztráció adatainak validálása
    this.before({ only: { verb: 'post', path: '#/companydata'} }, function (e) {
        var error_msg = '';

        if ($("#txt_customername").val() == '') {
            error_msg += 'A vevőnév kitöltése kötelező! <br/>';
        }
        //if ($("#txt_vatnumber").val() == '') {
        //		error_msg += 'Az adószám kitöltése kötelező! <br/>';
        //}
        if ($("#txt_mainemail").val() == '') {
            error_msg += 'Az elsődleges email cím kitöltése kötelező! <br/>';
        }
        if ($("#select_invoicecountry").val() == '') {
            error_msg += 'A számlázási cím ország kiválasztása kötelező! <br/>';
        }
        if ($("#txt_invoicecity").val() == '') {
            error_msg += 'A számlázási cím város kitöltése kötelező! <br/>';
        }
        if ($("#txt_invoicestreet").val() == '') {
            error_msg += 'A számlázási cím utca kitöltése kötelező! <br/>';
        }
        if ($("#txt_invoicezipcode").val() == '') {
            error_msg += 'A számlázási cím irányítószám kitöltése kötelező! <br/>';
        }
        if ($("#txt_invoicephone").val() == '') {
            error_msg += 'A telefonszám kitöltése kötelező! <br/>';
        }
        if ($("#selectMailCountry").val() == '') {
            error_msg += 'A levelezési cím ország kiválasztása kötelező! <br/>';
        }
        if ($("#txtMailAddressCity").val() == '') {
            error_msg += 'A levelezési cím város kitöltése kötelező! <br/>';
        }
        if ($("#txtMailAddressStreet").val() == '') {
            error_msg += 'A levelezési cím utca kitöltése kötelező! <br/>';
        }
        if ($("#txtMailAddressZipCode").val() == '') {
            error_msg += 'A levelezési cím irányítószám kitöltése kötelező! <br/>';
        }
		$("#spanCompanyDataError").html(error_msg);

        return (error_msg === '');

    });
    //cégregisztrációs adatlap mentése, webadmin adatainak betöltése
    this.post('#/companydata', function (context) {
        ////console.log(context);
        var vatNumber = $("#txt_vatnumber").val() + '-' + $("#txt_vatnumber2").val() + '-' + $("#txt_vatnumber3").val();
        vatNumber = (vatNumber === '') ? $("#txt_vatnumber4").val() : vatNumber;
        var data = {
            CompanyData: {
                RegistrationNumber: $("#txt_registrationnumber").val(),
                NewsletterToMainEmail: $('#chk_newslettertomainemail').is(':checked'), //bool
                SignatureEntityFile: $("#span_signature_entity_file").html(),
                CustomerId: $("#hfCustomerId").val(),
                CustomerName: $("#txt_customername").val(),
                VatNumber: vatNumber,
                EUVatNumber: $("#txt_euvatnumber").val(),
                MainEmail: $("#txt_mainemail").val(),
                CountryRegionId: $("#select_country").val()
            },
            InvoiceAddress: {
                CountryRegionId: $("#select_invoicecountry").val(),
                City: context.params['txt_invoicecity'],
                Street: context.params['txt_invoicestreet'],
                ZipCode: context.params['txt_invoicezipcode'],
                Phone: context.params['txt_invoicephone']
            },
            MailAddress: {
                CountryRegionId: $("#select_mailcountry").val(),
                City: context.params['txt_mailaddresscity'],
                Street: context.params['txt_mailaddressstreet'],
                ZipCode: context.params['txt_mailaddresszipcode']
            },
            Hidden: {

                Hidden: context.params['hidden']
            },
            ModifyMailAddress: $('#chk_mail_address').is(':checked'),
            ModifyDeliveryAddress: $('#chk_del_address').is(':checked'),
            DeliveryAddress: {
                RecId: 0,
                City: $("#txt_deliveryaddresscity").val(),
                Street: $("#txt_deliveryaddressstreet").val(),
                ZipCode: $("#txt_deliveryaddresszipcode").val(),
                CountryRegionId: $("#select_deliveryaddresscountry").val(),
                Id: ''
            },
            BankAccount: {
                Part1: $("#txt_bankaccountpart1").val(),
                Part2: $("#txt_bankaccountpart2").val(),
                Part3: $("#txt_bankaccountpart3").val(),
                RecId: 0,
                Id: ''
            }
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('UpdateRegistrationData'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    if (result.Successed) {
                        setTabsVisibility(4, result.Visitor.IsValidLogin);
                        context.title('Regisztráció - cégadatok');
                        if (result.Visitor.IsValidLogin) {
                            $("#add_new_contact_btn").show();
                        } else {
                            $("#add_new_contact_btn").hide();
                        }
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    //console.log('updateRegistrationData result failed');
                }
            },
            error: function () {
                //console.log('updateRegistrationData call failed');
            }
        });
    });

    this.before({ only: { verb: 'post', path: '#/addbankaccount'} }, function (e) {
        var error_msg = '';
        if ($("#txt_bankaccountpart1").val() === '' || $("#txt_bankaccountpart2").val() === '' || $("#txt_bankaccountpart3").val() === '') {
            error_msg += 'A bankszámlaszám kitöltése kötelező! <br/>';
        }
        $("#span_addbankaccount_error").html(error_msg);
        return (error_msg === '');
    });
    this.post('#/addbankaccount', function (context) {
        ////console.log(context);
        var data = {
            Part1: context.params['txt_bankaccountpart1'],
            Part2: context.params['txt_bankaccountpart2'],
            Part3: context.params['txt_bankaccountpart3'],
            RecId: 0,
            Id: ''
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('AddBankAccount'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items Part1 Part2 Part3 RecId Id
                    $("#bankAccountContainer").empty();
                    //$("#bankAccountTemplate").tmpl(result).appendTo("#bankAccountContainer");
                    var html = Mustache.to_html($('#bankAccountTemplate').html(), result);
                    $('#bankAccountContainer').html(html);

                    $('#txt_bankaccountpart1').val('');
                    $('#txt_bankaccountpart2').val('');
                    $('#txt_bankaccountpart3').val('');
                    context.title('Regisztráció - bankszámlaszám hozzáadás');
                }
                else {
                    //console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('removeDeliveryAddress call failed');
            }
        });
    });

    this.get('#/selectforupdatebankaccount/:selectedId', function (context) {
        var data = { SelectedId: context.params['selectedId'] };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('SelectForUpdateBankAccount'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items Part1 Part2 Part3 RecId Id
                    $("#bankAccountContainer").empty();
                    var html = Mustache.to_html($('#bankAccountTemplate').html(), result);
                    $('#bankAccountContainer').html(html);
                    context.title('Regisztráció - bankszámlaszám módosítás');
                }
                else {
                    //console.log('selectForUpdateBankAccount result failed');
                }
            },
            error: function () {
                //console.log('selectForUpdateBankAccount call failed');
            }
        });
    });

    this.post('#/updatebankaccount/:id/:recId', function (context) {
        ////console.log(context);
        //        var part1 = '#txtBankAccountPart1_' + id;
        //        var part2 = '#txtBankAccountPart2_' + id;
        //        var part3 = '#txtBankAccountPart3_' + id;
        var data = {
            Part1: context.params['txt_bankaccountpart1'],
            Part2: context.params['txt_bankaccountpart2'],
            Part3: context.params['txt_bankaccountpart3'],
            RecId: context.params['recId'],
            Id: context.params['id']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('UpdateBankAccount'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items Part1 Part2 Part3 RecId Id
                    $("#bankAccountContainer").empty();
                    var html = Mustache.to_html($('#bankAccountTemplate').html(), result);
                    $('#bankAccountContainer').html(html);
                    context.title('Regisztráció - bankszámlaszám módosítás');
                }
                else {
                    //console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('removeDeliveryAddress call failed');
            }
        });

    });
    this.get('#/removebankaccount/:id', function (context) {
        ////console.log(context);
        var data = {
            Id: context.params['id']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('RemoveBankAccount'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items Part1 Part2 Part3 RecId Id   
                    $("#bankAccountContainer").empty();
                    var html = Mustache.to_html($('#bankAccountTemplate').html(), result);
                    $('#bankAccountContainer').html(html);
                    context.title('Regisztráció - bankszámlaszám hozzáadás');
                }
                else {
                    //console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('removeDeliveryAddress call failed');
            }
        });
    });
    //webadmin adatainak validálása
    this.before({ only: { verb: 'post', path: '#/webadministrator'} }, function (e) {

        var error_msg = '';

        if ($("#txt_webadminfirstname").val() == '') {
            error_msg += 'A webadminisztrátor vezetéknév kitöltése kötelező! <br/>';
        }
        if ($("#txt_webadminlastname").val() == '') {
            error_msg += 'A webadminisztrátor keresztnév kitöltése kötelező! <br/>';
        }
        if ($("#txt_webadminemail").val() == '') {
            error_msg += 'Az webadminisztrátor email cím kitöltése kötelező! <br/>';
        }
        if ($("#txt_webadminpassword").val() == '') {
            error_msg += 'A webadminisztrátor jelszó kitöltése kötelező! <br/>';
        }
        if ($("#txt_webadminusername").val() == '') {
            error_msg += 'A webadminisztrátor belépési név kitöltése kötelező! <br/>';
        }
        if ($("#txt_webadminpassword").val() != $("#txt_webadminpassword2").val()) {
            error_msg += 'A jelszó és a jelszó megerősítése mező nem egyezik! <br/>';
        }
        $("#spanWebAdminDataError").html(error_msg);

        return (error_msg === '');
    });
    //kapcsolattartó adatainak betöltése, webadmin adatainak mentése 
    this.post('#/webadministrator', function (context) {
        ////console.log(context);
        var webadminPositions = context.params['select_webadminposition'];
        var data = {
            AllowOrder: $("#chk_webadminalloworder").is(':checked'),
            AllowReceiptOfGoods: $("#chk_webadminallowreceiptofgoods").is(':checked'),
            ContactPersonId: $("#hiddenWebAdminContactPersonId").val(),
            Email: context.params['txt_webadminemail'],
            EmailArriveOfGoods: $("#chk_webadminemailarriveofgoods").is(':checked'),
            EmailOfDelivery: $("#chk_webadminemailofdelivery").is(':checked'),
            EmailOfOrderConfirm: $("#chk_webadminemailoforderconfirm").is(':checked'),
            FirstName: context.params['txt_webadminfirstname'],
            InvoiceInfo: $("#chk_webadmininvoiceinfo").is(':checked'),
            LastName: context.params['txt_webadminlastname'],
            LeftCompany: false,
            Newsletter: $("#chk_webadminnewsletter2").is(':checked'),
            Password: context.params['txt_webadminpassword'],
            PriceListDownload: $("#chk_webadminpricelistdownload").is(':checked'),
            RecId: $("#hiddenWebAdminRecId").val(),
            RefRecId: $("#hiddenWebAdminRefRecId").val(),
            SmsArriveOfGoods: false,
            SmsOfDelivery: false,
            SmsOrderConfirm: false,
            Telephone: context.params['txt_webadminphone'],
            UserName: context.params['txt_webadminusername'],
            Positions: (webadminPositions === null || webadminPositions === '') ? [] : webadminPositions
        };
        //data.RegistrationNumber = $("#txtRegistrationNumber").val();
        var dataString = JSON.stringify(data);
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('UpdateWebAdministrator'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    if (result.Succeeded) {
                        setTabsVisibility(5, result.Visitor.IsValidLogin);
                        context.title('HRP/BSC Regisztráció - kapcsolattartó');
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    //console.log('updateWebAdministrator result failed');
                }
            },
            error: function () {
                //console.log('updateWebAdministrator call failed');
            }
        });
    });
    //előnézet betöltése, (kapcsolattartó adatainak mentése)
    this.post('#/preview', function (context) {

        ////console.log(context);
        setTabsVisibility(6);
        context.title('Regisztráció - lezárás, szerződés nyomtatás');

    });
    //regisztrációs adatok nyomtatása
    this.post('#/print', function (context) {
        ////console.log(context);
        if ($('#chk_contactpersonallowreceiptofgoods').is(':checked') || $('#chk_webadminallowreceiptofgoods').is(':checked')) {
            setTabsVisibility(7);
            context.title('Regisztráció eredménye');
        }
        else {
            alert("LEGALÁBB 1 főnél be kell jeölve lenni az árut átvehet!");
        }

    });
    
	
	
	this.before({ only: { verb: 'post', path: '#/adddeliveryaddress'} }, function (e) {  
		var error_msg = '';
		if ($("#txt_deliveryaddresszipcode").val() == '' || $("#txt_deliveryaddresscity").val() == '' ||  $("#txt_deliveryaddressstreet").val() == '' ) {
            error_msg += 'A szállítási cím kitőltése nem lett befejezve! <br/>';
        }
		$("#spanCompanyDataError").html(error_msg);
		return (error_msg === '');
	});	
	this.post('#/adddeliveryaddress', function (context) {
        ////console.log(context);
        var data = {
            RecId: 0,
            City: context.params['txt_deliveryaddresscity'],
            Street: context.params['txt_deliveryaddressstreet'],
            ZipCode: context.params['txt_deliveryaddresszipcode'],
            CountryRegionId: context.params['select_deliveryaddresscountry'],
            Id: ''
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('AddDeliveryAddress'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items RecId, City, Street, ZipCode, CountryRegionId, Id
                    $("#deliveryAddressContainer").empty();
                    //$("#deliveryAddressTemplate").tmpl(result).appendTo("#deliveryAddressContainer");
                    var deliveryAddressHtml = Mustache.to_html($('#deliveryAddressTemplate').html(), result);
                    $('#deliveryAddressContainer').html(deliveryAddressHtml);
                    $('#txt_deliveryaddresscity').val('');
                    $('#txt_deliveryaddressstreet').val('');
                    $('#txt_deliveryaddresszipcode').val('');
                    context.title('Regisztráció - szállítási cím hozzáadás');
                    $("#add_new_delivery").show();
                    $("#add_new_delivery_btn").hide();
                }
                else {
                    //console.log('addDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('addDeliveryAddress call failed');
            }
        });
    });

    this.get('#/selectforupdatedeliveryaddress/:selectedId', function (context) {
        ////console.log(context);
        var data = {
            SelectedId: context.params['selectedId']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('SelectForUpdateDeliveryAddress'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items RecId, City, Street, ZipCode, CountryRegionId, Id
                    $("#deliveryAddressContainer").empty();
                    //$("#deliveryAddressTemplate").tmpl(result).appendTo("#deliveryAddressContainer");
                    var deliveryAddressHtml = Mustache.to_html($('#deliveryAddressTemplate').html(), result);
                    $('#deliveryAddressContainer').html(deliveryAddressHtml);
                    context.title('Regisztráció - szállítási cím kiválasztás');
                }
                else {
                    //console.log('selectForUpdateDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('selectForUpdateDeliveryAddress call failed');
            }
        });
    });

    this.post('#/updatedeliveryaddress/:id/:recId', function (context) {
        ////console.log(context);
        var data = {
            RecId: context.params['recId'],
            City: context.params['txt_deliveryaddresscity'],
            Street: context.params['txt_deliveryaddressstreet'],
            ZipCode: context.params['txt_deliveryaddresszipcode'],
            CountryRegionId: context.params['select_deliveryaddresscountry'],
            Id: context.params['id']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('UpdateDeliveryAddress'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items RecId, City, Street, ZipCode, CountryRegionId, Id
                    $("#deliveryAddressContainer").empty();
                    //$("#deliveryAddressTemplate").tmpl(result).appendTo("#deliveryAddressContainer");
                    var deliveryAddressHtml = Mustache.to_html($('#deliveryAddressTemplate').html(), result);
                    $('#deliveryAddressContainer').html(deliveryAddressHtml);
                    context.title('Regisztráció - szállítási cím módosítás');
                }
                else {
                    //console.log('addDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('addDeliveryAddress call failed');
            }
        });
    });

    this.get('#/removedeliveryaddress/:id', function (context) {
        ////console.log(context);
        var data = {
            Id: context.params['id']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('RemoveDeliveryAddress'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items  RecId, City, Street, ZipCode, CountryRegionId, Id
                    $("#deliveryAddressContainer").empty();
                    //$("#deliveryAddressTemplate").tmpl(result).appendTo("#deliveryAddressContainer");
                    var deliveryAddressHtml = Mustache.to_html($('#deliveryAddressTemplate').html(), result);
                    $('#deliveryAddressContainer').html(deliveryAddressHtml);
                    context.title('Regisztráció - szállítási cím törlés');
                }
                else {
                    //console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('removeDeliveryAddress call failed');
            }
        });

    });

    this.before({ only: { verb: 'post', path: '#/addcontactperson'} }, function (e) {

        var error_msg = '';

        if ($("#txt_contactpersonfirstname").val() == '' || $("#txt_contactpersonlastname").val() == '' || $("#txt_contactpersonemail").val() == '') {
            error_msg += 'A kapcsolattartó kitőltése nem lett befejezve! <br/>';
        }
        if ($("#txt_contactpersonpassword").val() != $("#txt_contactpersonpassword2").val()) {
            error_msg += 'A jelszó és a jelszó megerősítése mező nem egyezik! <br/>';
        }
        $("#spanContactPersonDataError").html(error_msg);

        return (error_msg === '');
    });
    this.post('#/addcontactperson', function (context) {
        var contactPositions = context.params['select_position2'];
		var data = {
            Id: '',
            AllowOrder: $("#chk_contactpersonalloworder").is(':checked'),
            AllowReceiptOfGoods: $("#chk_contactpersonallowreceiptofgoods").is(':checked'),
            ContactPersonId: '',
            Email: context.params['txt_contactpersonemail'],
            EmailArriveOfGoods: $("#chk_contactpersonemailarriveofgoods").is(':checked'),
            EmailOfDelivery: $("#chk_contactpersonemailofdelivery").is(':checked'),
            EmailOfOrderConfirm: $("#chk_contactpersonemailoforderconfirm").is(':checked'),
            FirstName: context.params['txt_contactpersonfirstname'],
            InvoiceInfo: $("#chk_contactpersoninvoiceinfo").is(':checked'),
            LastName: context.params['txt_contactpersonlastname'],
            LeftCompany: false,
            Newsletter: $("#chk_contactpersonnewsletter2").is(':checked'),
            Password: context.params['txt_contactpersonpassword'],
            PriceListDownload: $("#chk_contactpersonpricelistdownload").is(':checked'),
            RecId: 0,
            RefRecId: 0,
            SmsArriveOfGoods: false,
            SmsOfDelivery: false,
            SmsOrderConfirm: false,
            Telephone: context.params['txt_contactpersonphone'],
            UserName: context.params['txt_contactpersonnusername'],
            Positions: (contactPositions === null || contactPositions === '') ? [] : contactPositions
        };
        //data.RegistrationNumber = $("#txtRegistrationNumber").val();
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('AddContactPerson'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {//ContactPersonId,FirstName,LastName,AllowOrder ,AllowReceiptOfGoods ,SmsArriveOfGoods,SmsOrderConfirm,SmsOfDelivery ,EmailArriveOfGoods ,EmailOfOrderConfirm ,EmailOfDelivery ,WebAdmin ,PriceListDownload 
                    ///InvoiceInfo,UserName,Password ,Newsletter,Telephone,Email ,LeftCompany,Id
                    $("#contactPersonContainer").empty();
                    var html = Mustache.to_html($('#contactPersonTemplate').html(), result);
                    $('#contactPersonContainer').html(html);
                    context.title('Regisztráció - kapcsolattartó hozzáadás');
					$("#txt_contactpersonphone").val('');
					$("#txt_contactpersonnusername").val('');
					$("#txt_contactpersonfirstname").val('');
					$("#txt_contactpersonlastname").val('');
 					$("#txt_contactpersonemail").val('');
 					$("#txt_contactpersonpassword").val('');
   					$("#txt_contactpersonpassword2").val('');
					$("#chk_contactpersonalloworder").prop('checked', false);
					$("#chk_contactpersonallowreceiptofgoods").prop('checked', false);
					$("#chk_contactpersonemailarriveofgoods").prop('checked', false);
					$("#chk_contactpersonemailofdelivery").prop('checked', false);
					$("#chk_contactpersonemailoforderconfirm").prop('checked', false);
					$("#chk_contactpersoninvoiceinfo").prop('checked', false);
					$("#chk_contactpersonnewsletter2").prop('checked', false);
					$("#chk_contactpersonpricelistdownload").prop('checked', false);
					$('.select_position2 option').prop('selected', false);
    				$('.select_position2').trigger('liszt:updated');
					$("#add_new_contact").show();
                    $("#add_new_contact_btn").hide();
                }
                else {
                    //console.log('addContactPerson result failed');
                }
            },
            error: function () {
                //console.log('addContactPerson call failed');
            }
        });

			
		
    });

    this.get('#/selectforupdatecontactperson/:id', function (context) {
        ////console.log(context);
        var data = {
            selectedId: context.params['id']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('SelectForUpdateContactPerson'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //result.Items Part1 Part2 Part3 RecId Id
                    $("#contactPersonContainer").empty();
                 

                    var html = Mustache.to_html($('#contactPersonTemplate').html(), result);
                    $('#contactPersonContainer').html(html);
					$(".select_position2").chosen();
    				
                    context.title('Regisztráció - kapcsolattartó kiválasztás');
                }
                else {
                    //console.log('selectForUpdateContactPerson result failed');
                }
            },
            error: function () {
                //console.log('selectForUpdateContactPerson call failed');
            }
        });

    });
    this.post('#/updatecontactperson/:id/:recId', function (context) {
        var data = {
            Id: context.params['id'],
            AllowOrder: $("#chk_contactpersonalloworder").is(':checked'),
            AllowReceiptOfGoods: $("#chk_contactpersonallowreceiptofgoods").is(':checked'),
            ContactPersonId: '',
            Email: context.params['txt_contactpersonemail'],
            EmailArriveOfGoods: $("#chk_contactpersonemailarriveofgoods").is(':checked'),
            EmailOfDelivery: $("#chk_contactpersonemailofdelivery").is(':checked'),
            EmailOfOrderConfirm: $("#chk_contactpersonemailoforderconfirm").is(':checked'),
            FirstName: context.params['txt_contactpersonfirstname'],
            InvoiceInfo: $("#chk_contactpersoninvoiceinfo").is(':checked'),
            LastName: context.params['txt_contactpersonlastname'],
            LeftCompany: false,
            Newsletter: $("#chk_contactpersonnewsletter2").is(':checked'),
            Password: context.params['txt_contactpersonpassword'],
            PriceListDownload: $("#chk_contactpersonpricelistdownload").is(':checked'),
            RecId: context.params['recId'],
            RefRecId: 0,
            SmsArriveOfGoods: false,
            SmsOfDelivery: false,
            SmsOrderConfirm: false,
            Telephone: context.params['txt_contactpersonphone'],
            UserName: context.params['txt_contactpersonnusername'],
            Positions: context.params['select_position2']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('UpdateContactPerson'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {//ContactPersonId,FirstName,LastName,AllowOrder ,AllowReceiptOfGoods ,SmsArriveOfGoods,SmsOrderConfirm,SmsOfDelivery ,EmailArriveOfGoods ,EmailOfOrderConfirm ,EmailOfDelivery ,WebAdmin ,PriceListDownload 
                    ///InvoiceInfo,UserName,Password ,Newsletter,Telephone,Email ,LeftCompany,Id
                    $("#contactPersonContainer").empty();
					$(".select_position2").chosen();
					
    				
                 

                    var html = Mustache.to_html($('#contactPersonTemplate').html(), result);
                    $('#contactPersonContainer').html(html);
                    context.title('Regisztráció - kapcsolattartó módosítás');
                }
                else {
                    //console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                //console.log('removeDeliveryAddress call failed');
            }
        });
    });

    this.get('#/removecontactperson/:id', function (context) {
        ////console.log(context);
        var data = {
            Id: context.params['id']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('RemoveContactPerson'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {//ContactPersonId,FirstName,LastName,AllowOrder ,AllowReceiptOfGoods ,SmsArriveOfGoods,SmsOrderConfirm,SmsOfDelivery ,EmailArriveOfGoods ,EmailOfOrderConfirm ,EmailOfDelivery ,WebAdmin ,PriceListDownload 
                    ///InvoiceInfo,UserName,Password ,Newsletter,Telephone,Email ,LeftCompany,Id
                    $("#contactPersonContainer").empty();
 					$(".select_position2").chosen();
				
    				
                  

                    var html = Mustache.to_html($('#contactPersonTemplate').html(), result);
                    $('#contactPersonContainer').html(html);
                    context.title('Regisztráció - kapcsolattartó törlés');
                }
                else {
                    //console.log('removeContactPerson result failed');
                }
            },
            error: function () {
                //console.log('removeContactPerson call failed');
            }
        });
    });
    //regisztrációs adatok mentése
    this.get('#/saveregistration', function (context) {
        ////console.log(context);
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('Post'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {   //Message, Successed
                    if (result.Succeeded) {
					$('#registration_num').html(result.CustomerRegistrationId);
					
                        $('a#nyomtatas').trigger('click');
                        context.title('Regisztráció - lezárás, adatok mentése sikeresen megtörtént');
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    //console.log('post result failed');
                }
            },
            error: function () {
                //console.log('post call failed');
            }
        });
    });

    //regisztrációs adatok mentése
    this.get('#/saveregistration_print', function (context) {
        ////console.log(context);
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getRegistrationApiUrl('Post'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {   //Message, Successed
                    if (result.Succeeded) {
					$('#registration_num').html(result.CustomerRegistrationId);
					$('.vsz_number').val(result.CustomerRegistrationId);
                        context.title('Regisztráció - lezárás, adatok mentése sikeresen megtörtént');
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    //console.log('post result failed');
                }
            },
            error: function () {
                //console.log('post call failed');
            }
        });
    });


    var initRegistrationData = function (data) {

        if (data.Visitor.IsValidLogin) {
            $("#tabs_7_title").hide();
            $('#tabs-1-button').show();
            $('#tabs-2-button_modosit').hide();
            $('#tabs-3-button_modosit').hide();
            $('#tabs-4-button_modosit').hide();
            $('#tabs-5-button_modosit').hide();
            $('#tabs-6-button_modosit').hide();
            $('#tabs-7-button_modosit').hide();
            $('#tab_2_ok').hide();
            $('#tab_3_ok').hide();
            $('#tab_4_ok').hide();
            $('#tab_5_ok').hide();
            $('#tab_6_ok').hide();
            $('#tab_7_ok').hide();
            $('#modify_mgs').hide();
            $("#mail_address").show();
            $("#del_address").show();
            $("#contactperson").show();
            $("#finish_reg").hide()
            $("#tabs-7").hide();
            $("#add_new_delivery").hide();
            $("#add_new_contact").hide();
            $("#chk_del_address").hide();
            $("#chk_mail_address").hide();
            $("#chk_contactperson_div").hide();
            $("#address_tip").hide();
            $("#address_tip2").hide();
        }
        else {
            $('#tabs-2-button').hide();
            $('#tabs-3-button').hide();
            $('#tabs-4-button').hide();
            $('#tabs-5-button').hide();
            $('#tabs-6-button').hide();
            $('#tabs-7-button').hide();
            $('#tab_2_ok').hide();
            $('#tab_3_ok').hide();
            $('#tab_4_ok').hide();
            $('#tab_5_ok').hide();
            $('#tab_6_ok').hide();
            $('#tab_7_ok').hide();
            $('#modify_mgs').hide();
            $("#mail_address").hide();
            $("#del_address").hide();
            $("#contactperson").hide();
            $("#finish_reg").hide()
            $("#tabs-7").hide();
        }

        /**/

        $('#txt_datarecordingname').val(data.DataRecording.Name);
        $('#txt_datarecordingphone').val(data.DataRecording.Phone);
        $('#txt_datarecordingemail').val(data.DataRecording.Email);

        $('#txt_customername').val(data.CompanyData.CustomerName);
        $('#hfCustomerId').val(data.CompanyData.CustomerId);
        $('#txt_registrationnumber').val(data.CompanyData.RegistrationNumber);

        var vatNumbers = companyGroup.utils.instance().sliceVatNumber(data.CompanyData.VatNumber);
        $('#txt_vatnumber').val(vatNumbers.Number1);
        $('#txt_vatnumber2').val(vatNumbers.Number2);
        $('#txt_vatnumber3').val(vatNumbers.Number3);

        $('#txt_euvatnumber').val(data.CompanyData.EUVatNumber);
        $('#txt_mainemail').val(data.CompanyData.MainEmail);
        $('#chk_newslettertomainemail').prop('checked', data.CompanyData.NewsletterToMainEmail);

        $('#bankAccountContainer').html(Mustache.render($('#bankAccountTemplate').html(), data.BankAccounts));

        //$('#txtBankAccountPart1').val();
        //$('#txtBankAccountPart2').val();
        //$('#txtBankAccountPart3').val();

        $('#span_signature_entity_file').html(data.CompanyData.SignatureEntityFile);
        //        var countryHtml = Mustache.to_html($('#countryTemplate').html(), data.Countries);
        //        $('#countryContainer').html(countryHtml);

        //        var invoiceCountryHtml = Mustache.to_html($('#invoiceCountryTemplate').html(), data.Countries);
        //        $('#invoiceCountryContainer').html(invoiceCountryHtml);

        $('#txt_invoicezipcode').val(data.InvoiceAddress.ZipCode);
        $('#txt_invoicecity').val(data.InvoiceAddress.City);
        $('#txt_invoicestreet').val(data.InvoiceAddress.Street);
        $('#txt_invoicephone').val(data.InvoiceAddress.Phone);

        //        $('#mailCountryContainer').html(Mustache.render($('#mailCountryTemplate').html(), data.Countries));

        $('#txt_mailaddresszipcode').val(data.MailAddress.ZipCode);
        $('#txt_mailaddresscity').val(data.MailAddress.City);
        $('#txt_mailaddressstreet').val(data.MailAddress.Street);

        $('#deliveryAddressContainer').html(Mustache.render($('#deliveryAddressTemplate').html(), data.DeliveryAddresses));

        //        $('#deliveryAddressCountryContainer').html(Mustache.render($('#deliveryAddressCountryTemplate').html(), data.Countries));
        //        $('#selectDeliveryAddressCountry').val();
        //        $('#txtDeliveryAddressZipCode').val();
        //        $('#txtDeliveryAddressCity').val();
        //        $('#txtDeliveryAddressStreet').val();

        $('#txt_webadminusername').val(data.WebAdministrator.UserName);
        $('#txt_webadminpassword').val(data.WebAdministrator.Password);
        $('#txt_webadminpassword2').val(data.WebAdministrator.Password);
        $('#txt_webadminfirstname').val(data.WebAdministrator.FirstName);
        $('#hiddenWebAdminContactPersonId').val(data.WebAdministrator.ContactPersonId);
        $('#hiddenWebAdminRecId').val(data.WebAdministrator.RecId);
        $('#hiddenWebAdminRefRecId').val(data.WebAdministrator.RefRecId);
        $('#txt_webadminlastname').val(data.WebAdministrator.LastName);
        $('#txt_webadminemail').val(data.WebAdministrator.Email);
        $('#txt_webadminphone').val(data.WebAdministrator.Telephone);
        $('#chk_webadminalloworder').prop('checked', data.WebAdministrator.AllowOrder);
        $('#chk_webadminallowreceiptofgoods').prop('checked', data.WebAdministrator.AllowReceiptOfGoods);
        $('#chk_webadminemailarriveofgoods').prop('checked', data.WebAdministrator.EmailArriveOfGoods);
        $('#chk_webadminemailofdelivery').prop('checked', data.WebAdministrator.EmailOfDelivery);
        $('#chk_webadminemailoforderconfirm').prop('checked', data.WebAdministrator.EmailOfOrderConfirm);
        $('#chk_webadmininvoiceinfo').prop('checked', data.WebAdministrator.InvoiceInfo);
        $('#chk_webadminnewsletter2').prop('checked', data.WebAdministrator.Newsletter);
        $('#chk_webadminpricelistdownload').prop('checked', data.WebAdministrator.PriceListDownload);
        //$('#chkWebAdminSmsArriveOfGoods').prop('checked', data.WebAdministrator.SmsArriveOfGoods);
        //$('#chkWebAdminSmsOfDelivery').prop('checked', data.WebAdministrator.SmsOfDelivery);
        //$('#chkWebAdminSmsOrderConfirm').prop('checked', data.WebAdministrator.SmsOrderConfirm);
        $('#select_webadminposition').val(data.WebAdministrator.Positions);
        $("#select_webadminposition").trigger("liszt:updated");

        $('#contactPersonContainer').html(Mustache.render($('#contactPersonTemplate').html(), data.ContactPersons));
    }

    var setTabsVisibility = function (i, isValidLogin) {
        if (i == 1) {   //regisztrációs adatok lekérdezése a szerverről
            $('#tabs-1').hide();
            $('#tabs-1-button').hide();
            $('#tab_1_ok').hide(500);
        } else {
            $("#tabs-1").hide(500);
            $('#tab_1_ok').show(500);
        }
        if (i == 2) {   //új regisztráció hozzáadása, adatrögzítő adatainak betöltése
            $('#tabs-2').show(500);
            $('#tab_1_ok').show(500);
            $("#hr_1").hide(500);
			
            if (isValidLogin) {
                $("#add_new_delivery_btn").show();	
					
            } else {
                $("#add_new_delivery_btn").hide();
				$("#chk_newslettertomainemail").prop('checked', true);	
            }
        } else {
            $("#tabs-2").hide(500);
            $("#hr_1").show(500);
        }
        if (i == 3) {   //adatrögzítő adatainak mentése
            if (isValidLogin) {
                $('#tabs-3').show(500);
                $('#tabs-2-button').show(500);
                $('#tab_2_ok').show(500);
                $("#hr_2").hide(500);
				
            }
            else {
                $('#tabs-3').show(500);
                $('#tabs-2-button_modosit').show(500);
                $('#tabs-3-button_modosit').show(500);
                $('#tabs-4-button_modosit').show(500);
                $('#tabs-5-button_modosit').show(500);
                $('#tab_2_ok').show(500);
                $('#reg_point_title').css('color', '#084385');
                $('#reg_point_title_2').css('color', '#084385');
                $('#reg_point_title_3').css('color', '#084385');
                $('#error_mgs').hide(500);
                $('#error_mgs_2').hide(500);
                $('#error_mgs_3').hide(500);
                $('#modify_mgs').show(500);
                $("#hr_2").hide(500);
            }
        } else {

            $("#tabs-3").hide(500);
            $("#hr_2").show(500);
        }
        //cégregisztrációs adatok mentése
        if (i == 4) {
            if (isValidLogin) {
                $("#tabs-4").show(500);
                $('#tabs-3-button').show(500);
                $('#tab_3_ok').show(500);
                $("#hr_3").hide(500);
				$('#tabs-6-button_modosit').show(500);
            }
            else {
                $("#tabs-4").show(500);
                $('#tabs-3-button').show(500);
                $('#tab_3_ok').show(500);
                $("#hr_3").hide(500);
            }
        } else {
            $("#tabs-4").hide(500);
            $("#hr_3").show(500);
        }
        //webadmin mentése
        if (i == 5) {
            if (isValidLogin) {
                $("#tabs-5").show(500);
                $('#tabs-4-button').show(500);
                $('#tab_4_ok').show(500);
                $("#hr_4").hide(500);
				//$("#add_new_contact_btn").hide();
				$('#tabs-6-button_modosit').show(500);

            } else {
                $("#tabs-5").show(500);
                $('#tabs-4-button').show(500);
                $('#tab_4_ok').show(500);
                $("#hr_4").hide(500);
                
				$("#add_new_contact_btn").hide();
            }
        } else {
            $("#tabs-5").hide(500);
            $("#hr_4").show(500);
        }
        //kapcsolattartókról továbblépés
        if (i == 6) {
            $("#tabs-6").show(500);
            $('#tabs-5-button').show(500);
            $('#tab_5_ok').show(500);
            $("#hr_5").hide(500);
        } else {
            $("#tabs-6").hide(500);
            $("#hr_5").show(500);
        }
        if (i == 7) {
            $("#tabs-7").show(500);
        } else {
            $("#tabs-7").hide(500);
        }
    }

    $("#contract_btn").click(function () {
        $('#tabs-1').show("slow");
        $("#contract").hide('slow');
    });

    $("#add_new_delivery_btn").click(function () {
        $("#add_new_delivery_btn").hide();
        $("#add_new_delivery").show();
    });
    $("#add_new_delivery_cancel_btn").click(function () {
        $("#add_new_delivery_btn").show();
        $("#add_new_delivery").hide();
    });

    $("#add_new_contact_btn").click(function () {
        $("#add_new_contact_btn").hide();
        $("#add_new_contact").show();
    });

    $("#add_new_contact_cancel_btn").click(function () {
        $("#add_new_contact_btn").show('slow');
        $("#add_new_contact").hide('slow');
    });

    $(function () {
        $("#chk_print").click(function () {
            if ($(this).is(':checked'))
                $('a#nyomtatas').trigger('click');
            $("#tabs-7").hide(500);
        });
    });

    $("a#nyomtatas").fancybox();


    /*$("#iframe").fancybox({
    'width'				: '960px',
    'height'			: '80%',
    'fitToView'  : false,
    'autoSize'   : false,
    'closeBtn'   : true,
    'transitionIn'		: 'none',
    'transitionOut'		: 'none',
    'type'				: 'iframe'
		
    });*/



    function displayVals() {
        var singleValues = $("#select_country").val();
        if (singleValues != "HU") {
            $("#foreign_vat").show(500);
            $("#hun_vat").hide(500);
        } else {
            $("#foreign_vat").hide(500);
            $("#hun_vat").show(500);
        }
    }
    $("select").change(displayVals);
    displayVals();

    document.onkeydown = function () {
        switch (event.keyCode) {
            case 116: //F5 button
                event.returnValue = false;
                event.keyCode = 0;
                return false;
            case 82: //R button
                if (event.ctrlKey) {
                    event.returnValue = false;
                    event.keyCode = 0;
                    return false;
                }
        }
    }



});