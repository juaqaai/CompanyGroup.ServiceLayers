var companyGroup = companyGroup || {};

companyGroup.partnerinfo = $.sammy(function () {

    //this.use(Sammy.Mustache, 'html');

    this.use(Sammy.Title);
    //jelszócsere művelet  
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
    //jelszócsere view
    this.get('#/changepassword', function (context) {
        context.title('Jelszó módosítás - ');
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getVisitorApiUrl('GetVisitorInfo'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_content').empty();
                var html = Mustache.to_html($('#changePasswordTemplate').html(), response);
                $('#main_content').html(html);
            },
            error: function () {
                console.log('GetVisitorInfo call failed');
            }
        });
    });
    //elfelejtett jelszó kérés
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
    //elfelejtett jelszó view
    this.get('#/forgetpassword', function (context) {
        context.title('ELFELEJTETT JELSZÓ - ');
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getVisitorApiUrl('GetVisitorInfo'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_content').empty();
                var html = Mustache.to_html($('#forgetPasswordTemplate').html(), response);
                $('#main_content').html(html);
            },
            error: function () {
                console.log('GetVisitorInfo call failed');
            }
        });
    });
    //jelszómódosítás csere visszavonása  
    this.get('#/undochangepassword:token', function (context) {
        context.title('Jelszó módosítás visszavonás - ');
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getContactPersonApiUrl('UndoChagePassword'),
            data: {Token:context.params['token']},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_content').empty();
                var html = Mustache.to_html($('#undoChangePasswordTemplate').html(), response);
                $('#main_content').html(html);
            },
            error: function () {
                console.log('UndoChagePassword call failed');
            }
        });
    });
    //számla info
    this.get('#/invoiceinfo/:paymenttype', function (context) {
        //context.params['paymenttype']
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getInvoiceApiUrl('GetList'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_content').empty();
                var html = Mustache.to_html($('#invoiceTemplate').html(), response);
                $('#main_content').html(html);
            },
            error: function () {
                console.log('GetVisitorInfo call failed');
            }
        });
        context.title('Számla információ - ');
    });
    //megrendelés info
    this.get('#/salesorderinfo', function (context) {
        //console.log(context);
        $.ajax({
            url: companyGroup.utils.instance().getSalesOrderApiUrl('GetOrderInfo'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_content').empty();
                var html = Mustache.to_html($('#salesorderTemplate').html(), response);
                $('#main_content').html(html);
            },
            error: function () {
                console.log('GetVisitorInfo call failed');
            }
        });
        context.title('Megrendelés információ - ');
    });
    //kezdőállapot
    this.get('#/', function (context) {
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getVisitorApiUrl('GetVisitorInfo'),
            data: {},
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_content').empty();
                var html = Mustache.to_html($('#dashboardTemplate').html(), response);
                $('#main_content').html(html);
            },
            error: function () {
                console.log('GetVisitorInfo call failed');
            }
        });
        this.title('Partnerinformáció - dashboard');
    });
});