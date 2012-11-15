var companyGroup = companyGroup || {};

companyGroup.registration = $.sammy('#main_content', function () {

    //this.use(Sammy.Mustache);

    this.use(Sammy.Title);

    this.get('#/', function (context) {
        //console.log(context);
        this.title('Regisztráció - ');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
            .then(function (response) {
                //context.partial(viewPath('termsandconditions'), response);
                $("#tabs-1").show();
                $("#tabs-2").hide();
                $("#tabs-3").hide();
            });
    });
    //adatrögzítő
    this.get('#/datarecording', function (context) {
        //console.log(context);
        this.title('Regisztráció - kitöltő adatai');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            //context.partial(viewPath('datarecording'), response);
            $("#tabs-1").hide();
            $("#tabs-2").show();
            $("#tabs-3").hide();
        });
    });

    this.get('#/registrationdata', function (context) {
        //console.log(context);
        this.title('Regisztráció - törzsadatok');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            //context.partial(viewPath('registrationdata'), response);
        });
    });

    this.get('#/webadministrator', function (context) {
        //console.log(context);
        this.title('Regisztráció - web adminisztrátor');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            //context.partial(viewPath('webadministrator'), response);
        });
    });

    this.get('#/contactperson', function (context) {
        //console.log(context);
        this.title('Regisztráció - kapcsolattartó');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            //context.partial(viewPath('contactperson'), response);
        });
    });

//    function viewPath(name) {
//        return '/CompanyGroup.WebClient/Content/HtmlTemplates/Registration/' + name + '.html';
//    }
});