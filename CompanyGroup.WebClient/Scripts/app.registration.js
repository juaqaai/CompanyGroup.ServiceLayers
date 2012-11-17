//var companyGroup = companyGroup || {};

var companyGroupRegistration = $.sammy(function () {

    //this.use(Sammy.Mustache);

    this.use(Sammy.Title);

    //szerződési feltételek
    this.get('#/', function (context) {
        context.title('Regisztráció - szerződési feltételek');

        $.ajax({
            url: "/CompanyGroup.WebClient/api/RegistrationApi/GetRegistrationData",
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000
        }).then(function (response) {
            console.log(response);
            initRegistrationData(response);
            $("#tabs-1").show();
            $("#tabs-2").hide();
            $("#tabs-3").hide();
            $("#tabs-4").hide();
            $("#tabs-5").hide();
        });
    });
    //adatrögzítő adatai (regisztrációs adatok elkérése, template-ek feltöltése)
    this.get('#/datarecording', function (context) {
        //console.log(context);
        this.title('Regisztráció - kitöltő adatai');
//        this.load('/CompanyGroup.WebClient/api/RegistrationApi/GetRegistrationData')
//        .then(function (response) {
        $.ajax({
            type: "POST",
            url: '/CompanyGroup.WebClient/api/RegistrationApi/AddNew',
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (response) {
                if (response) {
//                    $("#bankAccountContainer").empty();
//                    $("#bankAccountTemplate").tmpl(response.BankAccounts).appendTo("#bankAccountContainer");
//                    $("#contactPersonContainer").empty();
//                    $("#contactPersonTemplate").tmpl(response.ContactPersons).appendTo("#contactPersonContainer");
//                    $("#deliveryAddressContainer").empty();
//                    $("#deliveryAddressTemplate").tmpl(response.DeliveryAddresses).appendTo("#deliveryAddressContainer");

                    $("#tabs-1").hide();
                    $("#tabs-2").show();
                    $("#tabs-3").hide();
                    $("#tabs-4").hide();
                    $("#tabs-5").hide();
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
    //cégregisztrációs adatok
    this.post('#/registrationdata', function (context) {
        //console.log(context);
        this.title('Regisztráció - törzsadatok');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {

        });
    });
    //webadmin adatai
    this.post('#/webadministrator', function (context) {
        //console.log(context);
        this.title('Regisztráció - web adminisztrátor');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            //context.partial(viewPath('webadministrator'), response);
        });
    });
    //kapcsolattartó adatai
    this.post('#/contactperson', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {

        });
    });
    //előnézet
    this.post('#/preview', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {

        });
    });

    this.get('#/addcontactperson', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó hozzáadás');

    });

    this.get('#/addbankaccount', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó hozzáadás');

    });

    this.get('#/selectforupdatebankaccount/:id', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó hozzáadás');

    });

    this.get('#/updatebankaccount/:id/:recId', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó hozzáadás');

    });
    this.get('#/removebankaccount/:id', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó hozzáadás');

    });

    this.get('#/adddeliveryaddress', function (context) {
        //console.log(context);
        this.title('Regisztráció - szállítási cím hozzáadás');

    });

    this.get('#/updatedeliveryaddress', function (context) {
        //console.log(context);
        this.title('Regisztráció - szállítási cím módosítás');

    });

    this.get('#/selectforupdatedeliveryaddress/:id', function (context) {
        //console.log(context);
        this.title('Regisztráció - szállítási cím kiválasztás');

    });

    this.get('#/removedeliveryaddress/:id', function (context) {
        //console.log(context);
        this.title('Regisztráció - szállítási cím törlés');

    });

    this.get('#/selectforupdatecontactperson/:id', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó választás');

    });

    this.get('#/removecontactperson/:id', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó törlés');

    });

    this.post('#/save', function (context) {
        //console.log(context);
        this.title('Regisztráció - szállítási cím hozzáadás');

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

        $('#selectCountry').html(Mustache.render($('#countryTemplate').html(), data.Countries));

        $('#selectInvoiceCountry').html(Mustache.render($('#countryTemplate').html(), data.Countries));

        $('#txtInvoiceZipCode').val(data.InvoiceAddress.ZipCode);
        $('#txtInvoiceCity').val(data.InvoiceAddress.City);
        $('#txtInvoiceStreet').val(data.InvoiceAddress.Street);
        $('#txtInvoicePhone').val(data.InvoiceAddress.Phone);

        $('#selectMailCountry').html(Mustache.render($('#countryTemplate').html(), data.Countries));

        $('#txtMailAddressZipCode').val(data.MailAddress.ZipCode);
        $('#txtMailAddressCity').val(data.MailAddress.City);
        $('#txtMailAddressStreet').val(data.MailAddress.Street);

        $('#deliveryAddressContainer').html(Mustache.render($('#deliveryAddressTemplate').html(), data.DeliveryAddresses));

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

//        $('#txtContactPersonUserName').val();
//        $('#txtContactPersonPassword').val();
//        $('#txtContactPersonPassword2').val();
//        $('#txtContactPersonFirstName').val();
//        $('#txtContactPersonLastName').val();
//        $('#txtContactPersonEmail').val();
//        $('#txtContactPersonPhone').val();
//        $('#chkContactPersonAllowOrder').val();
//        $('#chkContactPersonAllowReceiptOfGoods').val();
//        $('#chkContactPersonEmailArriveOfGoods').val();
//        $('#chkContactPersonEmailOfDelivery').val();
//        $('#chkContactPersonEmailOfOrderConfirm').val();
//        $('#chkContactPersonInvoiceInfo').val();
//        $('#chkContactPersonNewsletter').val();
//        $('#chkContactPersonPriceListDownload').val();
//        $('#chkContactPersonSmsArriveOfGoods').val();
//        $('#chkContactPersonSmsOfDelivery').val();
//        $('#chkContactPersonSmsOrderConfirm').val();
    }

});