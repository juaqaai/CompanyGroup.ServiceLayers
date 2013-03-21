var companyGroup = companyGroup || {};

companyGroup.registration = $.sammy(function () {

    //this.use(Sammy.Mustache);

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    //szerződési feltételek betöltése (regisztrációs adatok elkérése, template-ek feltöltése)
    this.get('#/', function (context) {
        context.title(' HRP/BSC Regisztráció - szerződési feltételek');
        $.ajax({
            url: companyGroup.utils.instance().getRegistrationApiUrl('GetRegistrationData'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000
        }).then(function (response) {
            ////console.log(response);
            initRegistrationData(response);
            setTabsVisibility(1);
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
                for (var i = 0; i < formData.length; i++) {
                    if (!formData[i].value) {
                        $('#span_signature_entity_file').html('File megadása kötelező...');
                        return false;
                    }
                }
            },
            success: function (data) {
                $('#span_signature_entity_file').html(data.Name);
            }
        });
    });
    //adatok ellenörzése
	  this.get('#/allowdatacheck', function (context) {
        $("#tabs-5").show(500);
            $("#tabs-4").show(500);
            $("#tabs-3").show(500);
            $("#tabs-2").show(500);
    });
    this.bind('checkData', function (e, data) {
        var context = this;
        if (data.Checked) {
            context.redirect('#/checked');
            context.title('HRP/BSC Regisztráció - adatok ellenörzése megtörtént');
        } else {
            context.redirect('#/notchecked');
            context.title('HRP/BSC Regisztráció - adatok ellenörzése nem történt meg');
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
        } else {
            $('#contactperson').hide("slow");
        }
    });
    this.bind('showDataCheck', function (e, data) {
        var context = this;
        context.redirect('#/showdatacheck');
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
    //bejelentkezés
    this.post('#/signin', function (context) {
        this.signIn(context.params['txt_username'], context.params['txt_password'], companyGroup.utils.instance().getVisitorApiUrl('SignIn'), function (result) {
            $.fancybox.close();

            $("#cus_header1").empty();
            var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
            $('#cus_header1').html(visitorInfoHtml);

            $("#usermenuContainer").empty();
            var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result);
            $('#usermenuContainer').html(usermenuHtml);

            context.redirect('#/authenticated');
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

        });
    });
    //keresés
    this.post('#/searchByTextFilter', function (context) {
        this.searchByText(context.params['txt_globalsearch']);
    });

    this.get('#/checked', function (context) {
        $("#tabs-1").toggle(500);
        $("#tabs-2").toggle(500);
        $("#tabs-3").toggle(500);
        $("#tabs-4").toggle(500);
        $("#tabs-5").toggle(500);
        $("#finish_reg").toggle(500);
        $("#check_span").toggle(500);
        $("#tabs-7").show(500);
    });
    this.get('#/notchecked', function (context) {
        $("#tabs-7").hide(500);
    });
    this.get('#/showdatacheck', function (context) {
        setTabsVisibility(6);
        context.title('HRP/BSC Regisztráció - adatok ellenőrzése');
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
                    setTabsVisibility(2);
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
                    if (response.Successed) {
                        setTabsVisibility(3);
                        context.title('Regisztráció - törzsadatok');
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
        $("form#form_companydata").submit();
    });
    //cégregisztráció adatainak validálása
    this.before({ only: { verb: 'post', path: '#/companydata'} }, function (e) {
        var error_msg = '';

        if ($("#txt_customername").val() == '') {
            error_msg += 'A vevőnév kitöltése kötelező! <br/>';
        }
        if ($("#txt_vatnumber").val() == '') {
            error_msg += 'Az adószám kitöltése kötelező! <br/>';
        }
        if ($("#txt_mainemail").val() == '') {
            error_msg += 'Az elsődleges email cím kitöltése kötelező! <br/>';
        }
        if ($("#selectInvoiceCountry").val() == '') {
            error_msg += 'A számlázási cím ország kiválasztása kötelező! <br/>';
        }
        if ($("#txtInvoiceCity").val() == '') {
            error_msg += 'A számlázási cím város kitöltése kötelező! <br/>';
        }
        if ($("#txtInvoiceStreet").val() == '') {
            error_msg += 'A számlázási cím utca kitöltése kötelező! <br/>';
        }
        if ($("#txtInvoiceZipCode").val() == '') {
            error_msg += 'A számlázási cím irányítószám kitöltése kötelező! <br/>';
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
                
                Hidden: context.params['hidden'],
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
                        setTabsVisibility(4);
                        context.title('Regisztráció - cégadatok');
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
            UserName: context.params['txt_webadminusername']
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
                    if (result.Successed) {
                        setTabsVisibility(5);
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
    //előnézet betöltése, (kapcsolattartó adatainak mentése nem szükséges)
    this.post('#/preview', function (context) {
        ////console.log(context);
        setTabsVisibility(6);
        context.title('Regisztráció - lezárás, szerződés nyomtatás');

    });
    //regisztrációs adatok nyomtatása
    this.post('#/print', function (context) {
        ////console.log(context);

        setTabsVisibility(7);
        context.title('Regisztráció eredménye');
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

    this.post('#/addcontactperson', function (context) {
        ////console.log(context);
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
            UserName: context.params['txt_contactpersonnusername']
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
            UserName: context.params['txt_contactpersonnusername']
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
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {   //Message, Successed
                    if (result.Successed) {
                        $('#saveregistration_result').show();
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

        $('#contactPersonContainer').html(Mustache.render($('#contactPersonTemplate').html(), data.ContactPersons));

        //  $('#txtContactPersonUserName').val();
        //  $('#txtContactPersonPassword').val();
        //  $('#txtContactPersonPassword2').val();
        //  $('#txtContactPersonFirstName').val();
        //  $('#txtContactPersonLastName').val();
        //  $('#txtContactPersonEmail').val();
        //  $('#txtContactPersonPhone').val();
        //  $('#chkContactPersonAllowOrder').val();
        //  $('#chkContactPersonAllowReceiptOfGoods').val();
        //  $('#chkContactPersonEmailArriveOfGoods').val();
        //  $('#chkContactPersonEmailOfDelivery').val();
        //  $('#chkContactPersonEmailOfOrderConfirm').val();
        //  $('#chkContactPersonInvoiceInfo').val();
        //  $('#chkContactPersonNewsletter').val();
        //  $('#chkContactPersonPriceListDownload').val();
        //  $('#chkContactPersonSmsArriveOfGoods').val();
        //  $('#chkContactPersonSmsOfDelivery').val();
        //  $('#chkContactPersonSmsOrderConfirm').val();
    }



    var setTabsVisibility = function (i) {
        if (i == 1) {
            $('#tabs-1').hide();
            $('#tabs-1-button').hide();
            $('#tab_1_ok').hide(500);
        } else {
            $("#tabs-1").hide(500);
            $('#tab_1_ok').show(500);
        }
        if (i == 2) {
            $('#tabs-2').show(500);
            $('#tabs-1-button').show(500);
            $('#tab_1_ok').show(500);
        } else {
            $("#tabs-2").hide(500);
            $('#tabs-2-button').hide(500);
        }
        if (i == 3) {
            $('#tabs-3').show(500);
            $('#tabs-2-button').show(500);
            $('#tab_2_ok').show(500);
        } else {
            $("#tabs-3").hide(500);
            $('#tabs-3-button').hide(500);
            $('#tab_2_ok').hide(500);
        }
        if (i == 4) {
            $("#tabs-4").show(500);
            $('#tabs-3-button').show(500);
            $('#tab_3_ok').show(500);
        } else {
            $("#tabs-4").hide(500);
            $('#tabs-4-button').hide(500);
            $('#tab_3_ok').hide(500);
        }
        if (i == 5) {
            $("#tabs-5").show(500);
            $('#tabs-4-button').show(500);
            $('#tab_4_ok').show(500);

        } else {
            $("#tabs-5").hide(500);
            $('#tabs-5-button').hide(500);
            $('#tab_4_ok').hide(500);
        }
        if (i == 6) {
            $("#tabs-6").show(500);
            $('#tabs-5-button').show(500);
            $('#tab_5_ok').show(500);

        } else {
            $("#tabs-6").hide(500);
            $('#tab_5_ok').hide(500);
            ;
        }
        if (i == 7) {
            $("#tabs-7").show(500);

        } else {
            $("#tabs-7").hide(500);
        }
    }


    $("#mail_address").hide();
    $("#del_address").hide();
    $("#contactperson").hide();
    $("#finish_reg").hide()
    $("#tabs-7").hide();




    $("#contract_btn").click(function () {
        $('#tabs-1').show("slow");
        $("#contract").hide('slow');

    });


    $("#tabs-1-button").click(function () {
        $('#tabs-1').toggle("slow");
    });
    $("#tabs-2-button").click(function () {
        $('#tabs-2').toggle("slow");
    });

    $("#tabs-3-button").click(function () {
        $('#tabs-3').toggle("slow");
    });
    $("#tabs-4-button").click(function () {
        $('#tabs-4').toggle("slow");
    });
    $("#tabs-5-button").click(function () {
        $('#tabs-5').toggle("slow");
    });
    $("#tabs-6-button").click(function () {
        $('#tabs-6').toggle("slow");
    });

    $(function () {
        $("#chk_print").click(function () {
            if ($(this).is(':checked'))
                $('a#nyomtatas').trigger('click');
            $("#tabs-7").hide(500);
        });
    });

    $("a#nyomtatas").fancybox({
        'hideOnContentClick': false
    });



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



});