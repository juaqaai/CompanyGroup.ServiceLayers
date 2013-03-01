var companyGroup = companyGroup || {};

companyGroup.partnerinfo = $.sammy(function () {

    //this.use(Sammy.Mustache, 'html');

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('Partnerinfo -');

    //kezdőállapot
    this.get('#/', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').hide();
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
                $('#main_container').empty();
                var html = Mustache.to_html($('#dashboardTemplate').html(), response);
                $('#main_container').html(html);
            },
            error: function () {
                //console.log('GetVisitorInfo call failed');
            }
        });
        this.title('Partnerinformáció - dashboard');
    });
    //események
    this.bind('run', function (e, data) {
        var context = this;
        //szűrés azonnal elvihetőre
        $("#chk_rightaway").live('change', function () {
            context.trigger('filterByReserved', { Checked: $(this).is(':checked') });
        });
        //szűrés áttárolás utánira
        $("#chk_afteroverstore").live('change', function () {
            context.trigger('filterByReservedOrdered', { Checked: $(this).is(':checked') });
        });
        //szűrés beszerzés utánira
        $("#chk_afterpurchasing").live('change', function () {
            context.trigger('filterByOnOrder', { Checked: $(this).is(':checked') });
        });
        $("input[name='radio_paymenttype']").live('change', function () {
            context.redirect('#/invoiceinfo/' + $(this).val());
            //context.trigger('invoiceinfo', { PaymentType: parseInt($(this).val(), 0) });
        });
    });
    this.bind('filterByReservedOrdered', function (e, data) {
        salesOrderRequest.ReservedOrdered = data.Checked;
        loadOrderInfo();
        this.title('Szűrés azonnal elvihető rendelésekre');
    });
    this.bind('filterByReserved', function (e, data) {
        salesOrderRequest.Reserved = data.Checked;
        loadOrderInfo();
        this.title('Szűrés áttárolás utáni rendelésekre');
    });
    this.bind('filterByOnOrder', function (e, data) {
        salesOrderRequest.OnOrder = data.Checked;
        loadOrderInfo();
        this.title('Szűrés beszerzés alatt lévő rendelésekre');
    });

    //jelszócsere művelet  
    this.post('#/changepassword', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').hide();
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
                    //console.log(result);
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
                    //console.log('changePassword result failed');
                }
            },
            error: function () {
                //console.log('changePassword call failed');
            }
        });
    });
    //jelszócsere view
    this.get('#/changepassword', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').hide();
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
                $('#main_container').empty();
                var html = Mustache.to_html($('#changePasswordTemplate').html(), response);
                $('#main_container').html(html);
            },
            error: function () {
                //console.log('GetVisitorInfo call failed');
            }
        });
    });
    //elfelejtett jelszó kérés
    this.post('#/forgetpassword', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').hide();
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
                    //console.log(result);
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
                    //console.log('ForgetPassword result failed');
                }
            },
            error: function () {
                //console.log('ForgetPassword call failed');
            }
        });
    });
    //elfelejtett jelszó view
    this.get('#/forgetpassword', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').hide();
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
                $('#main_container').empty();
                var html = Mustache.to_html($('#forgetPasswordTemplate').html(), response);
                $('#main_container').html(html);
            },
            error: function () {
                //console.log('GetVisitorInfo call failed');
            }
        });
    });
    //jelszómódosítás csere visszavonása  
    this.get('#/undochangepassword:token', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').hide();
        context.title('Jelszó módosítás visszavonás - ');
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getContactPersonApiUrl('UndoChagePassword'),
            data: { Token: context.params['token'] },
            type: "GET",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_container').empty();
                var html = Mustache.to_html($('#undoChangePasswordTemplate').html(), response);
                $('#main_container').html(html);
            },
            error: function () {
                //console.log('UndoChagePassword call failed');
            }
        });
    });
    //számla info
    this.get('#/invoiceinfo/:paymenttype', function (context) {
        $('#salesOrderMain').hide();
        $('#szamlaszuro').show();
        //console.log(context.params['paymenttype']);
        var paymenttype = parseInt(context.params['paymenttype']);
        var data = {
            Debit: (paymenttype === 1) ? true : false,
            Overdue: (paymenttype === 2) ? true : false
        };
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getInvoiceApiUrl('GetList'),
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_container').empty();

                $("#invoiceInfoListTemplate").tmpl(response).appendTo("#main_container");

//                var html = Mustache.to_html($('#invoiceTemplate').html(), response);
//                $('#main_container').html(html);
            },
            error: function () {
                //console.log('GetVisitorInfo call failed');
            }
        });
        context.title('Számla információ - ');
    });
    //megrendelés info 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt)
    this.get('#/salesorderinfo', function (context) {
        //console.log(context);
        $('#salesOrderMain').show();
        $('#szamlaszuro').hide();
        loadOrderInfo();
        context.title('Megrendelés információ - ');
    });
    var loadOrderInfo = function () {
        $.ajax({
            url: companyGroup.utils.instance().getSalesOrderApiUrl('GetOrderInfo'),
            data: JSON.stringify(salesOrderRequest),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            timeout: 0,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_container').empty();
                var html = Mustache.to_html($('#salesorderTemplate').html(), response);
                $('#main_container').html(html);
            },
            error: function () {
                //console.log('LoadOrderInfo call failed');
            }
        });
    };
    //kérés paramétereit összefogó objektum
    var salesOrderRequest = {
        OnOrder: true,
        Reserved: true,
        ReservedOrdered: true
    };
});