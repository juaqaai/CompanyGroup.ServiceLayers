var companyGroup = companyGroup || {};

companyGroup.registration = $.sammy('#main_content', function () {

    //this.use(Sammy.Mustache);

    this.use(Sammy.Title);

    //szerződési feltételek
    this.get('#/', function (context) {
        this.title('Regisztráció - szerződési feltételek');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
            .then(function (response) {
                //context.partial(viewPath('termsandconditions'), response);
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
        this.load('/CompanyGroup.WebClient/api/RegistrationApi/GetRegistrationData')
        .then(function (response) {
            //context.partial(viewPath('datarecording'), response);
            $("#tabs-1").hide();
            $("#tabs-2").show();
            $("#tabs-3").hide();
            $("#tabs-4").hide();
            $("#tabs-5").hide();
        });
    });
    //cégregisztrációs adatok
    this.post('#/registrationdata', function (context) {
        //console.log(context);
        this.title('Regisztráció - törzsadatok');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            //context.partial(viewPath('registrationdata'), response);
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
            //context.partial(viewPath('contactperson'), response);
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
    
//    function viewPath(name) {
//        return '/CompanyGroup.WebClient/Content/HtmlTemplates/Registration/' + name + '.html';
//    }
});