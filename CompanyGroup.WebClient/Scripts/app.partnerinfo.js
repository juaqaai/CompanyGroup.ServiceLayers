var companyGroup = companyGroup || {};

companyGroup.partnerinfo = $.sammy(function () {

    //this.use(Sammy.Mustache, 'html');

    this.use(Sammy.Title);

    this.use(companyGroupHelpers);

    this.setTitle('Partnerinfo -');

    this.get('#/closed', function (context) {
        //console.log(context);
        $.fancybox.close()
    });
    //események
    this.bind('run', function (e, data) {
        var context = this;
        //szűrés azonnal elvihetőre
        $("input[name='radio_canbetaken']").live('change', function () {
            var checked = $(this).val() == '1' ? true : false;
            context.trigger('filterByCanBeTaken', { Checked: checked });
        });
        $("input[name='radio_paymenttype']").live('change', function () {
            context.redirect('#/invoiceinfo/' + $(this).val());
            //context.trigger('invoiceinfo', { PaymentType: parseInt($(this).val(), 0) });
        });
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
        //számlalista kiválasztott oldalindex változás
        $("#select_pageindex_top").live('change', function () {
            context.trigger('selectedPageIndexChanged', { PageIndex: parseInt($("#select_pageindex_top").val(), 0) });
        });
        //számlalista látható elemek száma változás
        $("#select_visibleitemlist_top").live('change', function () {
            context.trigger('visibleItemListChanged', { Orientation: 'top', Index: parseInt($("#select_visibleitemlist_top").val(), 0) });
        });
    });
    this.bind('filterByCanBeTaken', function (e, data) {
        salesOrderRequest.CanBeTaken = data.Checked;
        loadOrderInfo();
        this.title('Szűrés azonnal elvihető - beszerzés alatt lévő rendelésekre');
    });

    //kezdőállapot
    this.get('#/', function (context) {
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').hide();
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
    this.get('#/authenticated', function (context) {
        //console.log('authenticated');
    });
    //bejelentkezés panel megmutatása
    this.get('#/showSignInPanel', function (context) {
        this.showSignInPanel();
    });
    //belépési adatok ellenörzése
    this.before({ only: { verb: 'post', path: '#/signin'} }, function (e) {
        return this.beforeSignIn();
    });
    //bejelentkezés (Visitor + Products objektummal tér vissza)
    this.post('#/signin', function (context) {
        return this.signIn(context.params['txt_username'], context.params['txt_password'], companyGroup.utils.instance().getVisitorApiUrl('SignIn'), function (result) {
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
            $("#visitorInfoTemplate").tmpl(result).appendTo("#cus_header1");
            $("#usermenuContainer").empty();
            $("#usermenuTemplate").tmpl(result).appendTo("#usermenuContainer");
            /*var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
            $('#cus_header1').html(visitorInfoHtml);
            var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result);
            $('#usermenuContainer').html(usermenuHtml);
            context.redirect('#/webshop')*/
        });
    });
    //keresés
    this.post('#/searchByTextFilter', function (context) {
        this.searchByText(context.params['txt_globalsearch']);
    });
    this.get('#/showForgetPasswordPanel', function (context) {
        this.showForgetPasswordPanel();
    });
    this.post('#/sendforgetpassword', function (context) {
        this.sendForgetPassword(context.params['txt_forgetpassword_username']);
    });
    //jelszócsere view
    this.get('#/changepassword', function (context) {
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').hide();
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
    //adatok ellenörzése
    this.before({ only: { verb: 'post', path: '#/changepassword'} }, function (context) {
        var error_msg = '';
        if (context.params['txt_oldpassword'] == '') {
            //console.log('txt_oldpassword cannot be empty');
            error_msg += 'A régi jelszó kitöltése kötelező! <br/>';
        }
        if (context.params['txt_username'] == '') {
            //console.log('txt_username cannot be empty');
            error_msg += 'A vevőnév kitöltése kötelező! <br/>';
        }
        if (context.params['txt_newpassword'] == '') {
            //console.log('txt_newpassword cannot be empty');
            error_msg += 'Az új jelszó kitöltése kötelező! <br/>';
        }
		if (context.params['txt_newpassword'] == context.params['txt_oldpassword']) {
            //console.log('txt_newpassword cannot be empty');
            error_msg += 'A jelszó már használatban van! <br/>';
        }
        if (context.params['txt_confirmpassword'] != context.params['txt_newpassword']) {
            //console.log('txt_confirmpassword cannot be empty');
            error_msg += 'Nem egyeznek a jelszavak! <br/>';
        }
		$("#changepassword_errors").html(error_msg);
		 return (error_msg === '');

    });
    //jelszócsere művelet  
    this.post('#/changepassword', function (context) {
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').hide();
        var data = {
            OldPassword: context.params['txt_oldpassword'],
            NewPassword: context.params['txt_newpassword'],
            UserName: context.params['txt_username']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getContactPersonApiUrl('ChangePwd'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //console.log(result);
                    if (result.OperationSucceeded) {
                        context.title('"Password change succeeded!" - ');
					$('#txt_username').val('');
						$('#txt_newpassword').val('') ;
						$('#txt_newpassword').val('') ;
						$('#txt_confirmpassword').val('') ;	
						$('#txt_oldpassword').val('');
						
					$.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>'+result.Message +'</H2></div>', {
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
                    beforeClose: function () {}
					
					
                });
                    }
                    else {
                        context.title('"Password change failed!" - ');
						
						$('#txt_oldpassword').val('');
						$('#txt_username').val('');
						$('#txt_newpassword').val('') ;
						$('#txt_newpassword').val('') ;
						$('#txt_confirmpassword').val('') ;

                    $.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>A jelszómódosítás sikeresen megtörtént!</H2></div>', {
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
                    beforeClose: function () {}
						
					
					
                });
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
    //elfelejtett jelszó kérés
    this.post('#/forgetpassword', function (context) {
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').hide();
        var data = {
            UserName: context.params['txt_username']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getContactPersonApiUrl('ForgetPwd'),
            data: JSON.stringify(data),
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
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').hide();
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
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').hide();
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
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').show();
        var paymenttype = parseInt(context.params['paymenttype']);
        invoiceInfoRequest.Debit = (paymenttype === 1);
        invoiceInfoRequest.Overdue = (paymenttype === 2);
        loadInvoiceInfoByFilter();
        context.title('Számla információ - ');
        this.invoiceSumValues(function (result) {
            $('#invoiceSumValuesContainer').empty();
            $("#invoiceSumValuesTemplate").tmpl(result).appendTo("#invoiceSumValuesContainer");
        });
    });
    //számla info részletek 
    this.get('#/invoice_details/:id', function (context) {
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').show();
        invoiceInfoRequest.Items.push(parseInt(context.params['id']));
        loadInvoiceInfoByFilter();
        context.title('Számla információ - részletek');
        context.redirect("#/details_open");
    });
    //számlainformációk szűrése paraméterek alapján
    this.post('#/invoiceinfoByFilter', function (context) {
        $('#orderInfoFilter').hide();
        $('#invoiceInfoFilter').show();
        var paymenttype = parseInt(context.params['paymenttype']);
        invoiceInfoRequest.Debit = (paymenttype === 1);
        invoiceInfoRequest.Overdue = (paymenttype === 2);
        invoiceInfoRequest.DateIntervall = parseInt(context.params['select_dateintervall']);
        invoiceInfoRequest.InvoiceId = (context.params['txt_invoiceid'] === 'Kattintson ide') ? '' : context.params['txt_invoiceid'];
        invoiceInfoRequest.ItemName = (context.params['txt_itemname'] === 'Kattintson ide') ? '' : context.params['txt_itemname'];
        invoiceInfoRequest.ItemId = (context.params['txt_itemid'] === 'Kattintson ide') ? '' : context.params['txt_itemid'];
        invoiceInfoRequest.SalesId = (context.params['txt_salesorderid'] === 'Kattintson ide') ? '' : context.params['txt_salesorderid'];
        invoiceInfoRequest.SerialNumber = (context.params['txt_serialnumber'] === 'Kattintson ide') ? '' : context.params['txt_serialnumber'];
        invoiceInfoRequest.CurrentPageIndex = 1;
        loadInvoiceInfoByFilter();
    });
    //kiválasztott sorszámú lapra ugrás
    this.bind('selectedPageIndexChanged', function (e, data) {
        //console.log('selectedPageIndexChanged: ' + data.PageIndex);
        invoiceInfoRequest.CurrentPageIndex = parseInt(data.PageIndex, 0);
        loadInvoiceInfoByFilter();
        this.title('kiválasztott sorszámú lapra ugrás');
    });
    //ugrás az első oldalra
    this.get('#/firstInvoiceInfoPage', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context);
        var currentPageIndex = invoiceInfoRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            invoiceInfoRequest.CurrentPageIndex = 1;
            loadInvoiceInfoByFilter();
            context.title('Első oldal');
        }
    });
    //ugrás az utolsó oldalra
    this.get('#/lastInvoiceInfoPage', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context);
        var currentPageIndex = invoiceInfoRequest.CurrentPageIndex;
        var lastPageIndex = parseInt($("#spanTopLastPageIndex").text(), 0);
        if (currentPageIndex < (lastPageIndex)) {
            invoiceInfoRequest.CurrentPageIndex = lastPageIndex;
            loadInvoiceInfoByFilter();
            context.title('Utolsó oldal');
        }
    });
    //ugrás a következő oldalra
    this.get('#/nextInvoiceInfoPage/:index', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context.params['index']);
        var currentPageIndex = invoiceInfoRequest.CurrentPageIndex;
        var lastPageIndex = parseInt($("#spanTopLastPageIndex").text(), 0);
        if (currentPageIndex < (lastPageIndex)) {
            currentPageIndex = currentPageIndex + 1;
            invoiceInfoRequest.CurrentPageIndex = currentPageIndex;
            loadInvoiceInfoByFilter();
            context.title('Következő oldal');
        }
    });
    //ugrás az előző oldalra
    this.get('#/previousInvoiceInfoPage/:index', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context.params['index']);
        var currentPageIndex = invoiceInfoRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            currentPageIndex = currentPageIndex - 1;
            invoiceInfoRequest.CurrentPageIndex = currentPageIndex;
            loadInvoiceInfoByFilter();
            context.title('Előző oldal');
        }
    });
    //megjelenített elemek száma változás
    this.bind('visibleItemListChanged', function (e, data) {
        //console.log(context);
        invoiceInfoRequest.CurrentPageIndex = 1;
        if (data.Orientation === 'top') {
            invoiceInfoRequest.ItemsOnPage = parseInt(data.Index, 0);
        }
        else {
            invoiceInfoRequest.ItemsOnPage = parseInt(data.Index, 0);
        }
        loadInvoiceInfoByFilter();
        this.title('megjelenített elemek száma változás');
    });


    //megrendelés info 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt)
    this.get('#/salesorderinfo', function (context) {
        //console.log(context);
        $('#orderInfoFilter').show();
        $('#invoiceInfoFilter').hide();
        $("#radio_nonpaid").prop('checked', true);
        $("#radio_overdue").prop('checked', false);
        $("#radio_overdue").prop('checked', false);
        loadOrderInfo();
        context.title('Megrendelés információ - ');
    });
    this.get('#/kereses_megrendeles', function (context) {
        $("form#megrendeles").submit();
    });
    this.get('#/kereses_szamla', function (context) {
        $("form#szamla").submit();
    });
    this.get('#/kereses_torlese_megrendeles', function (context) {
        salesOrderRequest.CustomerOrderNo = (context.params['txt_customerorderno'] === 'Kattintson ide') ? '' : context.params['txt_customerorderno'];
        salesOrderRequest.ItemName = (context.params['txt_itemname'] === 'Kattintson ide') ? '' : context.params['txt_itemname'];
        salesOrderRequest.ItemId = (context.params['txt_itemid'] === 'Kattintson ide') ? '' : context.params['txt_itemid'];
        salesOrderRequest.SalesOrderId = (context.params['txt_salesorderid'] === 'Kattintson ide') ? '' : context.params['txt_salesorderid'];
        loadOrderInfo();
        context.title('Megrendelés törlése - ');
        context.redirect('#/resetform')
        $('#txt_customerorderno').val("Kattintson ide");
        $('#txt_itemname').val("Kattintson ide");
        $('#txt_itemid').val("Kattintson ide");
        $('#txt_salesorderid').val("Kattintson ide");
    });
    this.get('#/kereses_torlese_szamla', function (context) {
        var paymenttype = parseInt(context.params['paymenttype']);
        invoiceInfoRequest.DateIntervall = parseInt(context.params['select_dateintervall']);
        invoiceInfoRequest.InvoiceId = (context.params['txt_invoiceid'] === 'Kattintson ide') ? '' : context.params['txt_invoiceid'];
        invoiceInfoRequest.ItemName = (context.params['txt_itemname'] === 'Kattintson ide') ? '' : context.params['txt_itemname'];
        invoiceInfoRequest.ItemId = (context.params['txt_itemid'] === 'Kattintson ide') ? '' : context.params['txt_itemid'];
        invoiceInfoRequest.SalesId = (context.params['txt_salesorderid'] === 'Kattintson ide') ? '' : context.params['txt_salesorderid'];
        invoiceInfoRequest.SerialNumber = (context.params['txt_serialnumber'] === 'Kattintson ide') ? '' : context.params['txt_serialnumber'];
        invoiceInfoRequest.CurrentPageIndex = 1;
        loadInvoiceInfoByFilter();
        context.title('Számlák törlése - ');
        context.redirect('#/resetform')
        $('#txt_invoiceid_invoice').val("Kattintson ide");
        $('#txt_itemname_invoice').val("Kattintson ide");
        $('#txt_itemid_invoice').val("Kattintson ide");
        $('#txt_salesorderid_invoice').val("Kattintson ide");
        $('#txt_serialnumber_invoice').val("Kattintson ide");
        $("#radio_nonpaid").prop('checked', true);
        $("#radio_overdue").prop('checked', false);
        $("#radio_overdue").prop('checked', false)
        //$("#radio_nonpaid").prop('checked', true);
        //$("#radio_overdue").prop('checked', false);
        //$("#radio_overdue").prop('checked', false)
    });
    this.post('#/orderInfoByFilter', function (context) {
        salesOrderRequest.CustomerOrderNo = (context.params['txt_customerorderno'] === 'Kattintson ide') ? '' : context.params['txt_customerorderno'];
        salesOrderRequest.ItemName = (context.params['txt_itemname'] === 'Kattintson ide') ? '' : context.params['txt_itemname'];
        salesOrderRequest.ItemId = (context.params['txt_itemid'] === 'Kattintson ide') ? '' : context.params['txt_itemid'];
        salesOrderRequest.SalesOrderId = (context.params['txt_salesorderid'] === 'Kattintson ide') ? '' : context.params['txt_salesorderid'];
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
                $('#span_openOrderAmount').html(response.OpenOrderAmount);
                $('#span_ordercount').html(response.Items.length);
                $('#main_container').empty();
                $("#salesorderTemplate").tmpl(response).appendTo("#main_container");
            },
            error: function () {
                //console.log('LoadOrderInfo call failed');
            }
        });
    };

    var loadInvoiceInfoByFilter = function () {
        $.ajax({
            //console.log(context);
            url: companyGroup.utils.instance().getInvoiceApiUrl('GetInvoiceInfo'),
            data: JSON.stringify(invoiceInfoRequest),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (response) {
                //console.log(response);
                $('#main_container').empty();
                $('#span_invoicecount').html(response.ListCount);
                $('#span_AllOverdueDebts').html(response.AllOverdueDebts);
                $('#span_TotalNettoCredit').html(response.TotalNettoCredit);
                $("#invoiceInfoTemplate").tmpl(response).appendTo("#main_container");

                $('#invoicePagerContainer').empty();
                $("#invoicePagerTemplate").tmpl(response).appendTo("#invoicePagerContainer");
                //                var html = Mustache.to_html($('#invoiceTemplate').html(), response);
                //                $('#main_container').html(html);
            },
            error: function () {
                //console.log('GetVisitorInfo call failed');
            }
        });
    };

    this.get('#/addShoppingCart/:ItemId', function (context) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('AddCart'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    //$('.cartnumber').spin();
                    $("input#hidden_cartId").val(result.ActiveCart.Id);
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('addCart call failed');
            }
        });
    });


    //nagyobb méretű termékkép
    this.get('#/showPicture/:productId//', function (context) {
        //this.get('#/showPicture/:ItemId/:dataAreaId/:productName', function (context) {
        var arr_pics = new Array();
        var data = {
            ProductId: context.params['productId'],
            DataAreaId: context.params['dataAreaId']
        };
        var productName = context.params['productName'];
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getPictureApiUrl('GetListByProduct'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 15000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result.Items.length > 0) {
                    $.each(result.Items, function (i, pic) {
                        var item = new Object();
                        item.href = companyGroup.utils.instance().getBigPictureUrl(pic.Id);
                        item.title = data.ProductId;
                        arr_pics.push(item);
                        context.redirect('#/gallery')
                        $.fancybox(
                            arr_pics,
                            {
                                'padding': 0,
                                'transitionIn': 'elastic',
                                'transitionOut': 'elastic',
                                'type': 'image',
                                'closeBtn': 'true',
                                'changeFade': 0,
                                'speedIn': 300,
                                'speedOut': 300,
                                'width': '150%',
                                'height': '150%',
                                'autoScale': true,
                                'titlePosition': 'inside',
                                'titleFormat': function (title, currentArray, currentIndex, currentOpts) {
                                    return '<a href="' + companyGroup.utils.instance().getProductDetailsUrl(data.ProductId) + '"><span id="fancybox-title-over"> ' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? '&nbsp; ' + title + '&nbsp;&nbsp;' + productName + '&nbsp;' : '') + '</span></a>';
                                }
                            });
                    });
                }
            },
            error: function () {
                alert('Service call failed: GetListByProduct');
            }
        });
    });

    //kérés paramétereit összefogó objektum
    var salesOrderRequest = {
        CanBeTaken: true,
        SalesStatus: 1,
        SalesOrderId: '',
        ItemName: '',
        ItemId: '',
        CustomerOrderNo: ''
    };
    var invoiceInfoRequest = {
        Debit: true,
        Overdue: false,
        SalesId: '',
        ItemName: '',
        ItemId: '',
        InvoiceId: '',
        SerialNumber: '',
        DateIntervall: 0,
        Sequence: 0,
        CurrentPageIndex: 1,
        ItemsOnPage: 30,
        Items: []
    };

});