var companyGroup = companyGroup || {};

companyGroup.partnerinfo = $.sammy('#main_content', function () {

    this.use(Sammy.Mustache, 'html');
    //this.use('Mustache', 'html');

    this.use(Sammy.Title);

    this.post('#/changepassword', function (context) {

        var data = {
            OldPassword: context.params['txt_oldpassword'],
            NewPassword: context.params['txt_newpassword'],
            UserName: context.params['txt_username']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getContactPersonApiUrl('ChangePwd'),
            data: data,
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    console.log(result);
                    if (result.ChangePassword.OperationSucceeded) {
                        context.title('"Password change succeeded!" - ');
                        context.partial(viewPath('changepassword_succeeded'), result.ChangePassword);

                        //$("#changePasswordContainer").empty();
                        //$("#changePasswordTemplate").tmpl(result.Visitor).appendTo("#changePasswordContainer");
                    }
                    else {
                        context.title('"Password change failed!" - ');
                        context.partial(viewPath('changepassword_failed'), result.ChangePassword);
                    }
                }
                else {
                    console.log('changePassword result failed');
                }
            },
            error: function () {
                console.log('changePassword call failed');
            }
        });

    });

    this.get('#/changepassword', function (context) {

        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
            .then(function (response) {
                context.partial(viewPath('changepassword'), response);

                context.title('Jelszó módosítás - ');
            });
    });

    this.post('#/forgetpassword', function (context) {

        var data = {
            UserName: context.params['txt_username']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getContactPersonApiUrl('ForgetPwd'),
            data: data,
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    console.log(result);
                    if (result.ForgetPassword.OperationSucceeded) {
                        context.title('"Password change succeeded!" - ');
                        context.partial(viewPath('forgetpassword_succeeded'), result.ForgetPassword);
                    }
                    else {
                        context.title('"Password change failed!" - ');
                        context.partial(viewPath('forgetpassword_failed'), result.ForgetPassword);
                    }
                }
                else {
                    console.log('ForgetPassword result failed');
                }
            },
            error: function () {
                console.log('ForgetPassword call failed');
            }
        });
    });
    this.get('#/forgetpassword', function (context) {

        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
        .then(function (response) {
            context.partial(viewPath('forgetpassword'), response);

            context.title('ELFELEJTETT JELSZÓ - ');
        });
    });
    this.get('#/undochangepassword:token', function (context) {

        this.load('/CompanyGroup.WebClient/api/ContactPersonApi/UndoChagePassword' + context.params['token'])
        .then(function (response) {
            context.partial(viewPath('undochangepassword'), response);

            context.title('Jelszó módosítás visszavonás - ');
        });
    });
//    this.get('#/order/:id', function (context) {

//        this.load('/app/product/' + context.params['id'])
//            .then(function (response) {

//                var title = 'Place Your Order : ' + response.Name;

//                context.title(title + ' - Sammy Fourth Coffee');

//                var order = {
//                    productId: response.Id,
//                    productName: response.Name,
//                    productPicture: response.Picture,
//                    productPrice: response.Price,
//                    quantity: 1,
//                    amount: function () { return this.productPrice * this.quantity; }
//                };

//                context.partial(viewPath('order'), { title: title, order: order });
//            });
//    });

    this.get('#/invoiceinfo/:paymenttype', function (context) {

        this.load('/CompanyGroup.WebClient/api/InvoiceApi/GetInvoiceInfo/' + context.params['paymenttype'])
            .then(function (response) {
                context.partial(viewPath('invoiceinfo'), response);

                context.title('Számla információ - ');
            });
    });

    this.get('#/salesorderinfo', function (context) {

        this.load('/CompanyGroup.WebClient/api/SalesOrderApi/GetOrderInfo')
            .then(function (response) {
                context.partial(viewPath('salesorderinfo'), response);

                context.title('Megrendelés információ - ');
            });
    });

    this.get('#/', function (context) {
        //console.log(context);
        this.title('Partnerinformáció - ');
        this.load('/CompanyGroup.WebClient/api/VisitorApi/GetVisitorInfo')
            .then(function (response) {
                context.partial(viewPath('dashboard'), response);
            });
    });

    function viewPath(name) {
        return '/CompanyGroup.WebClient/Content/HtmlTemplates/PartnerInfo/' + name + '.html';
    }
});