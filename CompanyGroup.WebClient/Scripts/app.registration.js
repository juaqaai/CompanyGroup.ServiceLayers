var companyGroup = companyGroup || {};

companyGroup.registration = $.sammy(function () {

    //this.use(Sammy.Mustache);

    this.use(Sammy.Title);

    //szerződési feltételek betöltése (regisztrációs adatok elkérése, template-ek feltöltése)
    this.get('#/', function (context) {
        context.title('Regisztráció - szerződési feltételek');
        $.ajax({
            url: companyGroup.utils.instance().getRegistrationApiUrl('GetRegistrationData'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000
        }).then(function (response) {
            //console.log(response);
            initRegistrationData(response);
            setTabsVisibility(1);
        });                                       
    });
    //adatrögzítő adatainak betöltése (új regisztráció hozzáadása)
    this.get('#/datarecording', function (context) {
        //console.log(context);
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
                    context.title('Regisztráció - kitöltő adatai');
                }
                else {
                    console.log('addNew result failed');
                }
            },
            error: function () {
                console.log('addNew call failed');
            }
        });
    });
    //adatrögzítő adatainak validálása
    this.before({ only: { verb: 'post', path: '#/registrationdata'} }, function (e) {

        var error_msg = '';

        if ($("#txtDataRecordingName").val() == '') {
            error_msg += 'Az adatrögzítő név kitöltése kötelező! <br/>';
        }
        if ($("#txtDataRecordingPhone").val() == '') {
            error_msg += 'Az adatrögzítő telefonszám kitöltése kötelező! <br/>';
        }
        if ($("#txtDataRecordingEmail").val() == '') {
            error_msg += 'Az adatrögzítő email cím kitöltése kötelező! <br/>';
        }

        $("#spanDataRecordingError").html(error_msg);

        return (error_msg === '');
    });
    //adatrögzítő adatainak mentése, cégregisztrációs adatlap betöltése
    this.post('#/registrationdata', function (context) {
        //console.log(context);$('form').serialize()
        var data = {
            Email: $("#txtDataRecordingEmail").val(),
            Name: $("#txtDataRecordingName").val(),
            Phone: $("#txtDataRecordingPhone").val()
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
                    console.log('updateDataRecording result failed');
                }
            },
            error: function () {
                console.log('updateDataRecording call failed');
            }
        });
    });
    //cégregisztráció adatainak validálása
    this.before({ only: { verb: 'post', path: '#/webadministrator'} }, function (e) {

        var error_msg = '';

        if ($("#txtCustomerName").val() == '') {
            error_msg += 'A vevőnév kitöltése kötelező! <br/>';
        }
        if ($("#txtVatNumber").val() == '') {
            error_msg += 'Az adószám kitöltése kötelező! <br/>';
        }
        if ($("#txtMainEmail").val() == '') {
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
    //webadmin adatainak betöltése, cégregisztrációs adatlap mentése
    this.post('#/webadministrator', function (context) {
        //console.log(context);
        var data = {
            CompanyData: {
                RegistrationNumber: $("#txtRegistrationNumber").val(),
                NewsletterToMainEmail: $('#chkNewsletterToMainEmail').is(':checked'), //bool
                SignatureEntityFile: $("#signatureEntityFile_Name").html(),
                CustomerId: $("#hfCustomerId").val(),
                CustomerName: $("#txtCustomerName").val(),
                VatNumber: $("#txtVatNumber").val(),
                EUVatNumber: $("#txtEUVatNumber").val(),
                MainEmail: $("#txtMainEmail").val(),
                CountryRegionId: $("#selectCountry").val()
            },
            invoiceAddress: {
                CountryRegionId: $("#selectInvoiceCountry").val(),
                City: $("#txtInvoiceCity").val(),
                Street: $("#txtInvoiceStreet").val(),
                ZipCode: $("#txtInvoiceZipCode").val(),
                Phone: $("#txtInvoicePhone").val()
            },
            MailAddress: {
                CountryRegionId: $("#selectMailCountry").val(),
                City: $("#txtMailAddressCity").val(),
                Street: $("#txtMailAddressStreet").val(),
                ZipCode: $("#txtMailAddressZipCode").val()
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
                        context.title('Regisztráció - web adminisztrátor');
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    console.log('updateRegistrationData result failed');
                }
            },
            error: function () {
                console.log('updateRegistrationData call failed');
            }
        });
    });
    //webadmin adatainak validálása
    this.before({ only: { verb: 'post', path: '#/contactperson'} }, function (e) {

        var error_msg = '';

        if ($("#txtWebAdminFirstName").val() == '') {
            error_msg += 'A webadminisztrátor vezetéknév kitöltése kötelező! <br/>';
        }
        if ($("#txtWebAdminLastName").val() == '') {
            error_msg += 'A webadminisztrátor keresztnév kitöltése kötelező! <br/>';
        }
        if ($("#txtWebAdminEmail").val() == '') {
            error_msg += 'Az webadminisztrátor email cím kitöltése kötelező! <br/>';
        }
        if ($("#txtWebAdminPassword").val() == '') {
            error_msg += 'A webadminisztrátor jelszó kitöltése kötelező! <br/>';
        }
        if ($("#txtWebAdminUserName").val() == '') {
            error_msg += 'A webadminisztrátor belépési név kitöltése kötelező! <br/>';
        }
        if ($("#txtWebAdminPassword").val() != $("#txtWebAdminPassword2").val()) {
            error_msg += 'A jelszó és a jelszó megerősítése mező nem egyezik! <br/>';
        }
        $("#spanWebAdminDataError").html(error_msg);

        return (error_msg === '');
    });
    //kapcsolattartó adatainak betöltése, webadmin adatainak mentése 
    this.post('#/contactperson', function (context) {
        //console.log(context);
        var data = {
            AllowOrder: $("#chkWebAdminAllowOrder").val(),
            AllowReceiptOfGoods: $("#chkWebAdminAllowReceiptOfGoods").val(),
            ContactPersonId: $("#hiddenWebAdminContactPersonId").val(),
            Email: $("#txtWebAdminEmail").val(),
            EmailArriveOfGoods: $("#chkWebAdminEmailArriveOfGoods").val(),
            EmailOfDelivery: $("#chkWebAdminEmailOfDelivery").val(),
            EmailOfOrderConfirm: $("#chkWebAdminEmailOfOrderConfirm").val(),
            FirstName: $("#txtWebAdminFirstName").val(),
            InvoiceInfo: $("#chkWebAdminInvoiceInfo").val(),
            LastName: $("#txtWebAdminLastName").val(),
            LeftCompany: false,
            Newsletter: $("#chkWebAdminNewsletter").val(),
            Password: $("#txtWebAdminPassword").val(),
            PriceListDownload: $("#chkWebAdminPriceListDownload").val(),
            RecId: $("#hiddenWebAdminRecId").val(),
            RefRecId: $("#hiddenWebAdminRefRecId").val(),
            SmsArriveOfGoods: $("#chkWebAdminSmsArriveOfGoods").val(),
            SmsOfDelivery: $("#chkWebAdminSmsOfDelivery").val(),
            SmsOrderConfirm: $("#chkWebAdminSmsOrderConfirm").val(),
            Telephone: $("#txtWebAdminPhone").val(),
            UserName: $("#txtWebAdminUserName").val()
        };
        //data.RegistrationNumber = $("#txtRegistrationNumber").val();
        var dataString = $.toJSON(data);
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
                        context.title('Regisztráció - kapcsolattartó');
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    console.log('updateWebAdministrator result failed');
                }
            },
            error: function () {
                console.log('updateWebAdministrator call failed');
            }
        });
    });
    //előnézet betöltése, (kapcsolattartó adatainak mentése nem szükséges)
    this.post('#/preview', function (context) {
        //console.log(context);
        setTabsVisibility(6);
        context.title('Regisztráció - lezárás, szerződés nyomtatás');

    });
    //regisztrációs adatok nyomtatása
    this.post('#/print', function (context) {
        //console.log(context);

        setTabsVisibility(7);
        context.title('Regisztráció eredménye');
    });

    this.get('#/addbankaccount', function (context) {
        //console.log(context);
        var data = {
            Part1: $('#txtBankAccountPart1').val(),
            Part2: $('#txtBankAccountPart2').val(),
            Part3: $('#txtBankAccountPart3').val(),
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

                    $('#txtBankAccountPart1').val('');
                    $('#txtBankAccountPart2').val('');
                    $('#txtBankAccountPart3').val('');
                    context.title('Regisztráció - kapcsolattartó hozzáadás');
                }
                else {
                    console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('removeDeliveryAddress call failed');
            }
        });
    });

    this.get('#/selectforupdatebankaccount/:id', function (context) {
        //console.log(context);
        var data = { SelectedId: id };
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
                    context.title('Regisztráció - kapcsolattartó hozzáadás');
                }
                else {
                    console.log('selectForUpdateBankAccount result failed');
                }
            },
            error: function () {
                console.log('selectForUpdateBankAccount call failed');
            }
        });
    });

    this.get('#/updatebankaccount/:id/:recId', function (context) {
        //console.log(context);
        var part1 = '#txtBankAccountPart1_' + id;
        var part2 = '#txtBankAccountPart2_' + id;
        var part3 = '#txtBankAccountPart3_' + id;
        var data = {
            Part1: $(part1).val(),
            Part2: $(part2).val(),
            Part3: $(part3).val(),
            RecId: recId,
            Id: id
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
                    context.title('Regisztráció - kapcsolattartó hozzáadás');
                }
                else {
                    console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('removeDeliveryAddress call failed');
            }
        });

    });
    this.get('#/removebankaccount/:id', function (context) {
        //console.log(context);
        var data = {
            Id: id
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
                    context.title('Regisztráció - kapcsolattartó hozzáadás');
                }
                else {
                    console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('removeDeliveryAddress call failed');
            }
        });

    });

    this.get('#/adddeliveryaddress', function (context) {
        //console.log(context);
        var data = {
            RecId: 0,
            City: $('#txtDeliveryAddressCity').val(),
            Street: $('#txtDeliveryAddressStreet').val(),
            ZipCode: $('#txtDeliveryAddressZipCode').val(),
            CountryRegionId: $('#selectDeliveryAddressCountry').val(),
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
                    $('#txtDeliveryAddressCity').val('');
                    $('#txtDeliveryAddressStreet').val('');
                    $('#txtDeliveryAddressZipCode').val('');
                    context.title('Regisztráció - szállítási cím hozzáadás');
                }
                else {
                    console.log('addDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('addDeliveryAddress call failed');
            }
        });
    });

    this.get('#/selectforupdatedeliveryaddress/:id', function (context) {
        //console.log(context);
        var data = {
            SelectedId: id
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
                    console.log('selectForUpdateDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('selectForUpdateDeliveryAddress call failed');
            }
        });

    });

    this.get('#/updatedeliveryaddress', function (context) {
        //console.log(context);
        var city = '#txtDeliveryAddressCity_' + id;
        var street = '#txtDeliveryAddressStreet_' + id;
        var zipCode = '#txtDeliveryAddressZipCode_' + id;
        var country = '#selectDeliveryAddressCountry_' + id;
        var data = {
            RecId: recId,
            City: $(city).val(),
            Street: $(street).val(),
            ZipCode: $(zipCode).val(),
            CountryRegionId: $(country).val(),
            Id: id
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
                    console.log('addDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('addDeliveryAddress call failed');
            }
        });
    });

    this.get('#/removedeliveryaddress/:id', function (context) {
        //console.log(context);
        var data = {
            Id: id
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
                    console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('removeDeliveryAddress call failed');
            }
        });

    });

    this.get('#/addcontactperson', function (context) {
        //console.log(context);
        var data = {
            Id: '',
            AllowOrder: $("#chkContactPersonAllowOrder").val(),
            AllowReceiptOfGoods: $("#chkContactPersonAllowReceiptOfGoods").val(),
            ContactPersonId: '',
            Email: $("#txtContactPersonEmail").val(),
            EmailArriveOfGoods: $("#chkContactPersonEmailArriveOfGoods").val(),
            EmailOfDelivery: $("#chkContactPersonEmailOfDelivery").val(),
            EmailOfOrderConfirm: $("#chkContactPersonEmailOfOrderConfirm").val(),
            FirstName: $("#txtContactPersonFirstName").val(),
            InvoiceInfo: $("#chkContactPersonInvoiceInfo").val(),
            LastName: $("#txtContactPersonLastName").val(),
            LeftCompany: false,
            Newsletter: $("#chkContactPersonNewsletter").val(),
            Password: $("#txtContactPersonPassword").val(),
            PriceListDownload: $("#chkContactPersonPriceListDownload").val(),
            RecId: 0,
            RefRecId: 0,
            SmsArriveOfGoods: $("#chkContactPersonSmsArriveOfGoods").val(),
            SmsOfDelivery: $("#chkContactPersonSmsOfDelivery").val(),
            SmsOrderConfirm: $("#chkContactPersonSmsOrderConfirm").val(),
            Telephone: $("#txtContactPersonPhone").val(),
            UserName: $("#txtContactPersonUserName").val()
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
                    console.log('addContactPerson result failed');
                }
            },
            error: function () {
                console.log('addContactPerson call failed');
            }
        });
    });

    this.get('#/selectforupdatecontactperson/:id', function (context) {
        //console.log(context);
        var data = {
            selectedId: id
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
                    console.log('selectForUpdateContactPerson result failed');
                }
            },
            error: function () {
                console.log('selectForUpdateContactPerson call failed');
            }
        });

    });
    this.get('#/updatecontactperson/:id', function (context) {
        var data = {
            Id: id,
            AllowOrder: $("#chkContactPersonAllowOrder").val(),
            AllowReceiptOfGoods: $("#chkContactPersonAllowReceiptOfGoods").val(),
            ContactPersonId: '',
            Email: $("#txtContactPersonEmail").val(),
            EmailArriveOfGoods: $("#chkContactPersonEmailArriveOfGoods").val(),
            EmailOfDelivery: $("#chkContactPersonEmailOfDelivery").val(),
            EmailOfOrderConfirm: $("#chkContactPersonEmailOfOrderConfirm").val(),
            FirstName: $("#txtContactPersonFirstName").val(),
            InvoiceInfo: $("#chkContactPersonInvoiceInfo").val(),
            LastName: $("#txtContactPersonLastName").val(),
            LeftCompany: false,
            Newsletter: $("#chkContactPersonNewsletter").val(),
            Password: $("#txtContactPersonPassword").val(),
            PriceListDownload: $("#chkContactPersonPriceListDownload").val(),
            RecId: 0,
            RefRecId: 0,
            SmsArriveOfGoods: $("#chkContactPersonSmsArriveOfGoods").val(),
            SmsOfDelivery: $("#chkContactPersonSmsOfDelivery").val(),
            SmsOrderConfirm: $("#chkContactPersonSmsOrderConfirm").val(),
            Telephone: $("#txtContactPersonPhone").val(),
            UserName: $("#txtContactPersonUserName").val()
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
                    console.log('removeDeliveryAddress result failed');
                }
            },
            error: function () {
                console.log('removeDeliveryAddress call failed');
            }
        });
    });

    this.get('#/removecontactperson/:id', function (context) {
        //console.log(context);
        var data = {
            Id: id
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
                    console.log('removeContactPerson result failed');
                }
            },
            error: function () {
                console.log('removeContactPerson call failed');
            }
        });
    });
    //regisztrációs adatok mentése
    this.post('#/saveregistration', function (context) {
        //console.log(context);
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
                        setTabsVisibility(7);
                        context.title('Regisztráció - lezárás, adatok mentése');
                    } else {
                        alert(result.Message);
                    }
                }
                else {
                    console.log('post result failed');
                }
            },
            error: function () {
                console.log('post call failed');
            }
        });
    });
    var initRegistrationData = function (data) {
        $('#txtDataRecordingName').val(data.DataRecording.Name);
        $('#txtDataRecordingPhone').val(data.DataRecording.Phone);
        $('#txtDataRecordingEmail').val(data.DataRecording.Email);

        $('#txtCustomerName').val(data.CompanyData.CustomerName);
        $('#hfCustomerId').val(data.CompanyData.CustomerId);
        $('#txtCompanyRegisterNumber').val(data.CompanyData.RegistrationNumber);
        $('#txtVatNumber').val(data.CompanyData.VatNumber);
        $('#txtEUVatNumber').val(data.CompanyData.EUVatNumber);
        $('#txtMainEmail').val(data.CompanyData.MainEmail);
        $('#chkNewsletterToMainEmail').val(data.CompanyData.NewsletterToMainEmail);

        $('#bankAccountContainer').html(Mustache.render($('#bankAccountTemplate').html(), data.BankAccounts));

        //$('#txtBankAccountPart1').val();
        //$('#txtBankAccountPart2').val();
        //$('#txtBankAccountPart3').val();

        //$('#signatureEntityFile_Name').val();

        $('#countryContainer').html(Mustache.render($('#countryTemplate').html(), data.Countries));

        $('#invoiceCountryContainer').html(Mustache.render($('#invoiceCountryTemplate').html(), data.Countries));

        $('#txtInvoiceZipCode').val(data.InvoiceAddress.ZipCode);
        $('#txtInvoiceCity').val(data.InvoiceAddress.City);
        $('#txtInvoiceStreet').val(data.InvoiceAddress.Street);
        $('#txtInvoicePhone').val(data.InvoiceAddress.Phone);

        $('#mailCountryContainer').html(Mustache.render($('#mailCountryTemplate').html(), data.Countries));

        $('#txtMailAddressZipCode').val(data.MailAddress.ZipCode);
        $('#txtMailAddressCity').val(data.MailAddress.City);
        $('#txtMailAddressStreet').val(data.MailAddress.Street);

        $('#deliveryAddressContainer').html(Mustache.render($('#deliveryAddressTemplate').html(), data.DeliveryAddresses));

        $('#deliveryAddressCountryContainer').html(Mustache.render($('#deliveryAddressCountryTemplate').html(), data.Countries));
        //        $('#selectDeliveryAddressCountry').val();
        //        $('#txtDeliveryAddressZipCode').val();
        //        $('#txtDeliveryAddressCity').val();
        //        $('#txtDeliveryAddressStreet').val();

        $('#txtWebAdminUserName').val(data.WebAdministrator.UserName);
        $('#txtWebAdminPassword').val(data.WebAdministrator.Password);
        $('#txtWebAdminPassword2').val(data.WebAdministrator.Password);
        $('#txtWebAdminFirstName').val(data.WebAdministrator.FirstName);
        $('#hiddenWebAdminContactPersonId').val(data.WebAdministrator.ContactPersonId);
        $('#hiddenWebAdminRecId').val(data.WebAdministrator.RecId);
        $('#hiddenWebAdminRefRecId').val(data.WebAdministrator.RefRecId);
        $('#txtWebAdminLastName').val(data.WebAdministrator.LastName);
        $('#txtWebAdminEmail').val(data.WebAdministrator.Email);
        $('#txtWebAdminPhone').val(data.WebAdministrator.Telephone);
        $('#chkWebAdminAllowOrder').val(data.WebAdministrator.AllowOrder);
        $('#chkWebAdminAllowReceiptOfGoods').val(data.WebAdministrator.AllowReceiptOfGoods);
        $('#chkWebAdminEmailArriveOfGoods').val(data.WebAdministrator.EmailArriveOfGoods);
        $('#chkWebAdminEmailOfDelivery').val(data.WebAdministrator.EmailOfDelivery);
        $('#chkWebAdminEmailOfOrderConfirm').val(data.WebAdministrator.EmailOfOrderConfirm);
        $('#chkWebAdminInvoiceInfo').val(data.WebAdministrator.InvoiceInfo);
        $('#chkWebAdminNewsletter').val(data.WebAdministrator.Newsletter);
        $('#chkWebAdminPriceListDownload').val(data.WebAdministrator.PriceListDownload);
        $('#chkWebAdminSmsArriveOfGoods').val(data.WebAdministrator.SmsArriveOfGoods);
        $('#chkWebAdminSmsOfDelivery').val(data.WebAdministrator.SmsOfDelivery);
        $('#chkWebAdminSmsOrderConfirm').val(data.WebAdministrator.SmsOrderConfirm);

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
		$('#tabs-1').show(); 
		$('#tabs-1-button').hide();
 		} else { 
		$("#tabs-1").hide(500);
		}

		if (i == 2) { 
			$('#tabs-2').show(500); 
			$('#tabs-1-button').show(500);
		} else { 
			$("#tabs-2").hide(500);
			$('#tabs-2-button').hide();
		}
		if (i == 3) {
            $('#tabs-3').show(500);
			$('#tabs-2-button').show(500);
        } else {
            $("#tabs-3").hide(500);
			$('#tabs-3-button').hide();
        }
        if (i == 4) {
            $("#tabs-4").show(500);
			$('#tabs-3-button').show(500);
        } else {
            $("#tabs-4").hide(500);
			$('#tabs-4-button').hide();
        }
        if (i == 5) {
            $("#tabs-5").show(500);
			$('#tabs-4-button').show(500);
        } else {
            $("#tabs-5").hide(500);
			$('#tabs-5-button').hide();
        }
        if (i == 6) {
            $("#tabs-6").show(500);
			$('#tabs-5-button').show(500);
        } else {
            $("#tabs-6").hide(500);
        }
        if (i == 7) {
            $("#tabs-7").show(500);
        } else {
            $("#tabs-7").hide(500);
        }
    }

});