﻿//https://github.com/brandonaaron/sammystodos
/*
        this.notFound = function(verb, path) {
            this.runRoute('get', '#/404');
        };
        this.get('#/404', function() {
            this.partial('templates/404.template', {}, function(html) {
                $('#page').html(html);
            });
        });

*/

var companyGroup = companyGroup || {};

companyGroup.webshop = $.sammy(function () {

    this.use(Sammy.Title);

    this.get('#/', function (context) {
        //console.log(context);

        jQuery('.chosen').chosen();

        jQuery('.chosen').unbind('change').bind('change', function () {
            if ($(this).attr('id') === 'manufacturerList') {
                var manufacturerIdList = $('#manufacturerList').val();
                catalogueRequest.ManufacturerIdList = (manufacturerIdList === null || manufacturerIdList === '') ? [] : manufacturerIdList;
                loadStructure(false, true, true, true);
                loadCatalogue();
            } else if ($(this).attr('id') === 'category1List') {
                var category1IdList = $('#category1List').val();
                catalogueRequest.Category1IdList = (category1IdList === null || category1IdList === '') ? [] : category1IdList;
                loadStructure(true, false, true, true);
                loadCatalogue();
            } else if ($(this).attr('id') === 'category2List') {
                var category2IdList = $('#category2List').val();
                catalogueRequest.Category2IdList = (category2IdList === null || category2IdList === '') ? [] : category2IdList;
                loadStructure(true, true, false, true);
                loadCatalogue();
            } else if ($(this).attr('id') === 'category3List') {
                var category3IdList = $('#category3List').val();
                catalogueRequest.Category3IdList = (category3IdList === null || category3IdList === '') ? [] : category3IdList;
                loadStructure(true, true, true, false);
                loadCatalogue();
            }
        });

        //alsó (ajánlott) termék lapozó 
        $('#cus_recommended_prod_content').easyPaginate({
            step: 4,
            delay: 300,
            numeric: true,
            nextprev: false,
            auto: false,
            pause: 5000,
            clickstop: true,
            controls: 'pagination',
            current: 'current'
        });

        context.title('Webshop - Kezdőoldal');

    });

    //események
    this.bind('run', function (e, data) {
        var context = this;

        $("#select_pageindex_top").live('change', function () {
            context.trigger('selectedPageIndexChanged', { PageIndex: parseInt($("#select_pageindex_top").val(), 0) });
        });
        $("#select_pageindex_bottom").live('change', function () {
            context.trigger('selectedPageIndexChanged', { PageIndex: parseInt($("#select_pageindex_bottom").val(), 0) });
        });
        $("#select_visibleitemlist_top").live('change', function () {
            context.trigger('visibleItemListChanged', { Orientation: 'top', Index: parseInt($("#select_visibleitemlist_top").val(), 0) });
        });
        $("#select_visibleitemlist_bottom").live('change', function () {
            context.trigger('visibleItemListChanged', { Orientation: 'bottom', Index: parseInt($("#select_visibleitemlist_bottom").val(), 0) });
        });
        $("#txt_filterbynameorpartnumber").live('focus', function () {
            if ('név vagy cikkszám szűrése' === $(this).val()) {
                $(this).val('');
            }
        })
        .live('blur', function () {
            var $this = $(this),
                text = $.trim($this.val());
            if (!text) {
                $this.val('név vagy cikkszám szűrése');
            }
            //                        else {
            //                        if ($this.is('h1')) {
            //                            // it is the title
            //                            localStorage.setItem('title', text);
            //                        } else {
            //                            // save it
            //                            app.trigger('save', {
            //                                type: $this.attr('data-type'),
            //                                id: $this.attr('data-id'),
            //                                name: text
            //                            });
            //                        }
            //                    }
        })
        .live('keypress', function (event) {
            // save on enter
            if (event.which === 13) {
                //this.blur();
                return false;
            }
        });
        //kosárlista kiválasztott elem beállítása
        $("#saved_shoppingcart_list").live('change', function () {
            context.trigger('activateShoppingCart', { CartId: $(this).val() });
        });

        $("#txt_quantity").live('change', function () {
            context.trigger('updateShoppingCartLine', { Quantity: $(this).val(), ProductId: $(this).attr("title") });
        });
    });
    //kilépés
    this.get('#/signOut', function (context) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getCustomerApiUrl('SignOut'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (!result.Visitor.IsValidLogin) {
                    $("#cus_header1").empty();
                    $("#visitorInfoTemplate").tmpl(result.Visitor).appendTo("#cus_header1");
                    $("#quickmenuContainer").empty();
                    $("#quickmenuTemplate").tmpl(result.Visitor).appendTo("#quickmenuContainer");
                    $("#usermenuContainer").empty();
                    $("#usermenuTemplate").tmpl(result.Visitor).appendTo("#usermenuContainer");

                    $("#div_pager_top").empty();
                    $("#pagerTemplateTop").tmpl(result.Products).appendTo("#div_pager_top");
                    $("#div_pager_bottom").empty();
                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
                    $("#div_catalogue").empty();
                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
                    $("#catalogueSequenceContainer").empty();
                    $("#catalogueSequenceTemplate").tmpl(result.Visitor).appendTo("#catalogueSequenceContainer");
                    $("#catalogueDownloadContainer").empty();
                    $("#catalogueDownloadTemplate").tmpl(result.Visitor).appendTo("#catalogueDownloadContainer");
                    //$("#cus_filter_price").empty();
                    //$("#priceFilterTemplate").tmpl(result.Visitor).appendTo("#cus_filter_price");
                    $("#cus_filter_price").hide();

                    $("#hidden_cartId").val('');
                    //CompanyGroupCms.ShoppingCartInfo.Instance().SetCartId('');
                    //CompanyGroupCms.ShoppingCartSummary.Instance().Init(result.Visitor.IsValidLogin, 0);
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#deliveryAddressTemplate").tmpl(result.DeliveryAddresses).appendTo("#deliveryAddressContainer");
                }
                else {
                    alert('SignOut result failed');
                }
            },
            error: function () {
                alert('SignOut call failed');
            }
        });
    });
    //belépési adatok ellenörzése
    this.before({ only: { verb: 'post', path: '#/signin'} }, function (e) {
        var error_msg = '';

        if ($("#txt_username").val() == '') {
            error_msg += 'A bejelentkezési név kitöltése kötelező! <br/>';
        }
        if ($("#txt_password").val() == '') {
            error_msg += 'A jelszó kitöltése kötelező!';
        }
        $("#login_errors").html(error_msg);

        return (error_msg === '');
    });
    //bejelentkezés
    this.post('#/signin', function (context) {
        var data = {
            UserName: context.params['txt_username'],
            Password: context.params['txt_password']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getCustomerApiUrl('SignIn'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result.Visitor.IsValidLogin) {
                    $.fancybox.close();

                    $("#cus_header1").empty();
                    var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result.Visitor);
                    $('#cus_header1').html(visitorInfoHtml);

                    $("#usermenuContainer").empty();
                    var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result.Visitor);
                    $('#usermenuContainer').html(usermenuHtml);

                    $("#quickmenuContainer").empty();
                    var quickmenuHtml = Mustache.to_html($('#quickmenuTemplate').html(), result.Visitor);
                    $('#quickmenuContainer').html(quickmenuHtml);

                    $("#div_pager_top").empty();
                    $("#pagerTemplateTop").tmpl(result.Products).appendTo("#div_pager_top");
                    $("#div_pager_bottom").empty();
                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
                    $("#div_catalogue").empty();
                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
                    $("#catalogueSequenceContainer").empty();
                    $("#catalogueSequenceTemplate").tmpl(result.Visitor).appendTo("#catalogueSequenceContainer");
                    $("#catalogueDownloadContainer").empty();
                    $("#catalogueDownloadTemplate").tmpl(result.Visitor).appendTo("#catalogueDownloadContainer");

                    //$("#cus_filter_price").empty();
                    //$("#priceFilterTemplate").tmpl(result.Visitor).appendTo("#cus_filter_price");
                    $("#cus_filter_price").show();

                    $("#hidden_cartId").val(result.ActiveCart.Id);
                    //CompanyGroupCms.ShoppingCartInfo.Instance().SetCartId(result.ActiveCart.Id);
                    //CompanyGroupCms.ShoppingCartSummary.Instance().Init(result.Visitor.IsValidLogin, result.ActiveCart.SumTotal);
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button"); // CompanyGroupCms.ShoppingCartSummary.Instance() 
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#deliveryAddressTemplate").tmpl(result.DeliveryAddresses).appendTo("#deliveryAddressContainer");

                    context.redirect('#/authenticated');
                }
                else {
                    $("#login_errors").html(result.Visitor.ErrorMessage);
                    $("#login_errors").show();
                }
            },
            error: function () {
                alert('SignIn call failed!');
            }
        });

    });
    this.get('#/authenticated', function (context) {
        console.log('authenticated');
    });
    //bejelentkezés panel megmutatása
    this.get('#/showSignInPanel', function (context) {
        $.fancybox({
            href: '#div_login',
            autoDimensions: true,
            autoScale: false,
            transitionIn: 'fade',
            transitionOut: 'fade',
            beforeClose: function () { console.log('signin panel closed'); }
        });
    });
    //nyelv megváltoztatása
    this.get('#/changeLanguage/:language', function (context) {
        var data = {
            Language: (context.params['language'] === '' || context.params['language'] === 'HU') ? 'EN' : 'HU'
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getCustomerApiUrl('ChangeLanguage'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                console.log('ChangeLanguage');
                $("#inverse_language_id").html(result.Language);
            },
            error: function () {
                console.log('ChangeLanguage call failed');
            }
        });
    });
    //pénznem meváltoztatása
    this.get('#/changeCurrency/:currency', function (context) {
        var data = {
            Currency: context.params['currency']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getCustomerApiUrl('ChangeCurrency'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                console.log('ChangeCurrency');
                if (result.Currency === 'HUF') {
                    $("#currency_huf").css('background-color', '#900');
                    $("#currency_eur").css('background-color', '#666');
                    $("#currency_usd").css('background-color', '#666');
                    loadCatalogue();
                }
                else if (result.Currency === 'EUR') {
                    $("#currency_huf").css('background-color', '#666');
                    $("#currency_eur").css('background-color', '#900');
                    $("#currency_usd").css('background-color', '#666');
                    loadCatalogue();
                }
                else {
                    $("#currency_huf").css('background-color', '#666');
                    $("#currency_eur").css('background-color', '#666');
                    $("#currency_usd").css('background-color', '#900');
                    loadCatalogue();
                }
            },
            error: function () {
                console.log('ChangeCurrency call failed');
            }
        });
    });

    this.post('#/searchByTextFilter', function (context) {
        console.log('searchByTextFilter');
        catalogueRequest.TextFilter = context.params['txt_globalsearch'];
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        loadStructure(true, true, true, true);
    });

    this.get('#/forgetPassword', function (context) {
        var data = {
            UserName: $('#txt_username').val(),
            RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getCustomerApiUrl('ForgetPwd'),
            data: data,
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    console.log(result);
                    if (result.ForgetPassword.Succeeded) {
                        $('#forgetPasswordSucceededResult').show();
                        $('#forgetpassword_succeededmessage').html(result.ForgetPassword.Message);
                        $('#forgetPasswordFailedResult').hide();

                        $("#forgetPasswordContainer").empty();
                        $("#forgetPasswordTemplate").tmpl(result.Visitor).appendTo("#forgetPasswordContainer");
                    }
                    else {
                        $('#forgetPasswordFailedResult').show();
                        $('#forgetpassword_failedmessage').html(result.ForgetPassword.Message);
                        $('#forgetPasswordSucceededResult').hide();
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

    this.post('#/saveShoppingCart', function (context) {

    });

    this.post('#/addSite', function (context) {

    });

    this.post('#/importShoppingCart', function (context) {

    });
    //kosár nyitott állapotát menti
    this.get('#saveShoppingCartOpenStatus', function (context) {
        var isOpen = ($('#hidden_cartopen').val() === '1');
        isOpen = !isOpen;
        if (isOpen) {
            $('#hidden_cartopen').val('1');
        } else {
            $('#hidden_cartopen').val('');
        }
        var data = {
            IsOpen: isOpen
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('SaveShoppingCartOpenStatus'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                $("#basket_panel").slideToggle("fast");
                $("#active_basket").toggleClass("active");
                $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '') ? '[ + Megnyitás ]' : '[ - Bezárás ]');
            },
            error: function () {
                alert('saveShoppingCartOpenStatus call failed');
            }
        });
    });
    //kosár hozzáadás
    this.get('#addshoppingcart', function (context) {

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

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    //$('.cartnumber').spin();
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
    //aktív kosár eltávolítása
    this.get('#removeshoppingcart', function (context) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('RemoveCart'),
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

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");

                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }

                    $('.cartnumber').spin();
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('removeCart call failed');
            }
        });
    });
    //kosár mentése panel megmutatás
    this.get('#showsavecartpanel', function (context) {
        $.fancybox({
            href: '#save_basket_win',
            autoDimensions: true,
            autoScale: false,
            transitionIn: 'fade',
            transitionOut: 'fade'
        });
    });

    this.get('#removeLineFromShoppingCart/:productId', function (context) {
        var data = {
            ProductId: context.params['productId']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('RemoveLine'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");

                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $('.cartnumber').spin();

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.ActiveCart.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.ActiveCart.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('removeCart call failed');
            }
        });
    });
    //kosár sor hozzáadás 
    this.post('#addLineToShoppingCart', function (context) {
        var data = {
            //data.CartId = $("#hidden_cartId").val();
            ProductId: context.params['hidden_productId'],
            Quantity: context.params['txt_quantity']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('AddLine'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");

                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $('.cartnumber').spin();

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.ActiveCart.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.ActiveCart.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    $.floatingMessage('<span style="font-family: verdana; font-size: 13px; color:#fff;"> A kiválasztott termék:<br /><strong>' + productId + '</strong><br />bekerült a kosárba.</span>', {
                        time: 5000,
                        align: 'right',
                        verticalAlign: 'bottom',
                        show: 'blind',
                        hide: 'puff',
                        stuffEaseTime: 100,
                        stuffEasing: 'easeInExpo',
                        moveEaseTime: 200,
                        moveEasing: 'easeOutBounce'
                    });
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('addLine call failed');
            }
        });
    });

    //rendelés feladás
    this.post('#createOrder', function (context) {
        var data = {
            CustomerOrderNote: $("#user_comment").val(),
            CustomerOrderId: $("#custom_number").val(),
            DeliveryRequest: $("input[name=radio_szallitasimod]:checked").val() === '2',  //szállítást kért-e
            DeliveryDate: $("#naptar").val(),                                            //szállítás időpontja
            PaymentTerm: $("input[name=radio_fizetesimod]:checked").val(),               //1: átut, 2: KP, 3: előreut, 4: utánvét
            DeliveryTerm: $("input[name=radio_szallitasimod]:checked").val(),             //1: raktár, 2: kiszállítás
            DeliveryAddressRecId: $("#site_select").val()                                //szállítási cím azonosító
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('CreateOrder'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    /*
                    Visitor = visitor,
                    ActiveCart = response.ActiveCart,
                    OpenedItems = response.OpenedItems,
                    StoredItems = response.StoredItems,
                    ShoppingCartOpenStatus = shoppingCartOpenStatus,
                    CatalogueOpenStatus = catalogueOpenStatus,
                    LeasingOptions = response.LeasingOptions,
                    Created = response.Created,
                    WaitForAutoPost = response.WaitForAutoPost,
                    Message = response.Message                    
                    */
                    $.fancybox('<p>A rendelés feladása sikeresen megtörtént</p>',
                    {
                        'autoDimensions': true,
                        'padding': 0,
                        'transitionIn': 'elastic',
                        'transitionOut': 'elastic',
                        'changeFade': 0,
                        'speedIn': 300,
                        'speedOut': 300,
                        'width': '150%',
                        'height': '150%',
                        'autoScale': true
                    });
                    console.log(result);
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    ///$('.cartnumber').spin();
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    //$("#form_createorder").hide();
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('createOrder call failed');
            }
        });
    });
    //finanszírozási ajánlat
    this.post('#createFinanceOffer', function (context) {
        var data = {
            PersonName: $("#txt_offername").val(),
            Address: $("#txt_offeraddress").val(),
            Phone: $("#txt_offerphone").val(),
            StatNumber: $("#txt_offerstatnumber").val(),
            NumOfMonth: $("input[name=radio_selectNumOfMonth]").val()
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('CreateFinanceOffer'),
            data: JSON.stringify(data),
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
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    //$('.cartnumber').spin();
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('createFinanceOffer call failed');
            }
        });
    });
    //kosár aktiválás
    this.bind('activateShoppingCart', function (e, data) {
        var data = {
            CartId: data.CartId
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('ActivateCart'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button"); // CompanyGroupCms.ShoppingCartSummary.Instance() 
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");

                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }

                    //$('.cartnumber').spin();
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('removeCart call failed');
            }
        });
    });

    this.bind('updateShoppingCartLine', function (e, data) {
        var data = {
            //data.CartId = $("#hidden_cartId").val();
            ProductId: data.ProductId,
            Quantity: data.Quantity
        };
        var dataString = $.toJSON(data);
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('UpdateLineQuantity'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");

                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    //$('.cartnumber').spin();

                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.ActiveCart.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.ActiveCart.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('updateLine call failed');
            }
        });
    });

    //kiválasztott sorszámú lapra ugrás
    this.bind('selectedPageIndexChanged', function (e, data) {
        //console.log('selectedPageIndexChanged: ' + data.PageIndex);
        catalogueRequest.CurrentPageIndex = parseInt(data.PageIndex, 0);
        loadCatalogue();
        this.title('Webshop - kiválasztott sorszámú lapra ugrás');
    });
    //ugrás az első oldalra
    this.get('#/firstpage', function (context) {
        //console.log(context);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            catalogueRequest.CurrentPageIndex = 1;
            loadCatalogue();
            context.title('Webshop - Első oldal');
        }
    });
    //ugrás az utolsó oldalra
    this.get('#/lastPage', function (context) {
        //console.log(context);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        var lastPageIndex = parseInt($("#spanTopLastPageIndex").text(), 0);
        if (currentPageIndex < (lastPageIndex)) {
            catalogueRequest.CurrentPageIndex = lastPageIndex;
            loadCatalogue();
            context.title('Webshop - Utolsó oldal');
        }
    });
    //ugrás a következő oldalra
    this.get('#/nextPage/:index', function (context) {
        //console.log(context.params['index']);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        var lastPageIndex = parseInt($("#spanTopLastPageIndex").text(), 0);
        if (currentPageIndex < (lastPageIndex)) {
            currentPageIndex = currentPageIndex + 1;
            catalogueRequest.CurrentPageIndex = currentPageIndex;
            loadCatalogue();
            context.title('Webshop - Következő oldal');
        }
    });
    //ugrás az előző oldalra
    this.get('#/previousPage/:index', function (context) {
        //console.log(context.params['index']);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            currentPageIndex = currentPageIndex - 1;
            catalogueRequest.CurrentPageIndex = currentPageIndex;
            loadCatalogue();
            context.title('Webshop - Előző oldal');
        }
    });
    //megjelenített elemek száma változás
    this.bind('visibleItemListChanged', function (e, data) {
        //console.log(context);
        catalogueRequest.CurrentPageIndex = 1;
        if (data.Orientation === 'top') {
            catalogueRequest.ItemsOnPage = parseInt(data.Index, 0);
        }
        else {
            catalogueRequest.ItemsOnPage = parseInt(data.Index, 0);
        }
        loadCatalogue();
        this.title('Webshop - megjelenített elemek száma változás');
    });
    /*
    /// 0: átlagos életkor csökkenő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
    /// 1: átlagos életkor növekvő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg, 
    /// 2: azonosito növekvő, (cikkszám)
    /// 3: azonosito csökkenő, (cikkszám)
    /// 4 nev növekvő,
    /// 5: nev csökkenő,
    /// 6: ar növekvő,
    /// 7: ar csökkenő, 
    /// 8: belső készlet növekvően, 
    /// 9: belső készlet csökkenően
    /// 10: külső készlet növekvően
    /// 11: külső készlet csökkenően
    /// 12: garancia növekvően
    /// 13: garancia csökkenő    
    */
    //rendezés árra növekvően
    this.get('#/sequenceByPriceUp', function (context) {
        catalogueRequest.Sequence = 6;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés ár szerint növekvőleg');
    });
    //rendezés árra csökkenően
    this.get('#/sequenceByPriceDown', function (context) {
        catalogueRequest.Sequence = 7;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés ár szerint csökkenőleg');
    });
    //rendezés cikkszámra növekvően
    this.get('#/sequenceByPartNumberUp', function (context) {
        //console.log(self);
        catalogueRequest.Sequence = 2;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés cikkszám szerint növekvőleg');
    });
    //rendezés cikkszámra csökkenőleg
    this.get('#/sequenceByPartNumberDown', function (context) {
        //console.log(self);
        catalogueRequest.Sequence = 3;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés cikkszám szerint csökkenőleg');
    });
    //rendezés név szerint növekvőleg
    this.get('#/sequenceByNameUp', function (context) {
        //console.log(this);
        catalogueRequest.Sequence = 4;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés név szerint növekvőleg');
    });
    //rendezés név szerint csökkenőleg
    this.get('#/sequenceByNameDown', function (context) {
        //console.log(this);
        catalogueRequest.Sequence = 5;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés név szerint csökkenőleg');
    });
    //rendezés készletre növekvően
    this.get('#/sequenceByStockUp', function (context) {
        catalogueRequest.Sequence = 8;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés készlet szerint növekvőleg');
    });
    //rendezés készletre csökkenőleg
    this.get('#/sequenceByStockDown', function (context) {
        catalogueRequest.Sequence = 9;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés készlet szerint csökkenőleg');
    });
    //rendezés garanciaidőre növekvően
    this.get('#/sequenceByGarantyUp', function (context) {
        catalogueRequest.Sequence = 12;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés garancia szerint növekvőleg');
    });
    //rendezés garanciaidőre csökkenőleg
    this.get('#/sequenceByGarantyDown', function (context) {
        catalogueRequest.Sequence = 13;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('Webshop - rendezés garancia szerint csökkenőleg');
    });
    //szűrés készleten lévő termékekre
    this.get('#/filterByStock/:value', function (context) {
        catalogueRequest.StockFilter = context.params['value'];
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés készleten lévő termékekre');
    });
    //szűrés akciós termékekre
    this.get('#/filterByAction/:value', function (context) {
        catalogueRequest.ActionFilter = context.params['value'];
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés akciós termékekre');
    });
    //szűrés használt termékekre
    this.get('#/filterByBargain/:value', function (context) {
        catalogueRequest.BargainFilter = context.params['value'];
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés használt termékekre');
    });
    //szűrés új termékekre
    this.get('#/filterByNew/:value', function (context) {
        catalogueRequest.NewFilter = context.params['value'];
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés új termékekre');
    });
    //szűrés a hrp termékekre
    this.get('#/filterByHrp', function (context) {
        catalogueRequest.clear();
        catalogueRequest.HrpFilter = true;
        catalogueRequest.BscFilter = false;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - hardver termékek');
    });
    //szűrés a hrp hardvare termékekre
    this.get('#/filterByCategoryHrp/:category', function (context) {
        catalogueRequest.clear();
        catalogueRequest.Category1IdList.push(context.params['category']);
        loadCatalogue();
        loadStructure(true, true, true, true);
        $('#category1List').val(context.params['category']);
        $("#category1List").trigger("liszt:updated");
        context.title('Webshop - hardver termékek');
    });
    //szűrés a bsc termékekre
    this.get('#/filterByBsc', function (context) {
        catalogueRequest.clear();
        catalogueRequest.HrpFilter = false;
        catalogueRequest.BscFilter = true;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szoftver termékek');
    });
    //szűrés a bsc szoftvertermékeire
    this.get('#/filterByCategoryBsc/:category', function (context) {
        catalogueRequest.clear();
        catalogueRequest.Category1IdList.push(context.params['category']);
        loadCatalogue();
        loadStructure(true, true, true, true);
        $('#category1List').val(context.params['category']);
        $("#category1List").trigger("liszt:updated");
        context.title('Webshop - szoftver termékek');
    });
    //szűrőfeltételek törlése
    this.get('#/clearFilters', function (context) {
        $("#manufacturerList").empty();
        $("#manufacturerList").trigger("liszt:updated");
        $("#category1List").empty();
        $("#category1List").trigger("liszt:updated");
        $("#category2List").empty();
        $("#category2List").trigger("liszt:updated");
        $("#category3List").empty();
        $("#category3List").trigger("liszt:updated");
        catalogueRequest.clear();
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrőfeltételek törlése');
    });
    //használtcikk lista
    this.get('#/showSecondHandList/:productId/:dataAreaId', function (context) {
        alert('Ide jön a ' + productId + '-hoz kapcsolt használtcikk lista,');
        context.title('Webshop - leértékelt lista');
    });
    //keresés a termékek között
    this.get('#/searchByTextFilter/:textfilter', function (context) {
        catalogueRequest.TextFilter = context.params['textfilter'];
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - keresés');
    });
    this.get('#/downloadPriceList', function (context) {
        console.log('downloadPriceList');
        window.location = companyGroup.utils.instance().getDownloadPriceListUrl() + '?' + $.param(catalogueRequest);
    });
    //nagyobb méretű termékkép
    this.get('#/showPicture/:productId/:dataAreaId/:productName', function (context) {
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
                        item.href = companyGroup.utils.instance().getBigPictureUrl(data.ProductId, pic.RecId, data.DataAreaId);
                        item.title = data.ProductId;
                        arr_pics.push(item);
                        $.fancybox(
                            arr_pics,
                            {
                                'padding': 0,
                                'transitionIn': 'elastic',
                                'transitionOut': 'elastic',
                                'type': 'image',
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
    //struktúra betöltés
    var loadStructure = function (loadManufacturer, loadCategory1, loadCategory2, loadCategory3) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getStructureApiUrl('GetStructure'),
            data: JSON.stringify(catalogueRequest),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    if (loadManufacturer) {
                        var manufacturer = $('#manufacturerList').val()
                        $("#manufacturerList").empty();
                        $.each(result.Manufacturers, function (key, value) {
                            var option = $('<option>').text(value.Name).val(value.Id);
                            $("#manufacturerList").append(option);
                        });
                        if (manufacturer != '') {
                            $("#manufacturerList").val(manufacturer);
                        }
                        $("#manufacturerList").trigger("liszt:updated");
                    }
                    if (loadCategory1) {
                        var selectList = $("#category1List");
                        var categories = selectList.val();
                        selectList.empty();
                        $.each(result.FirstLevelCategories, function (key, value) {
                            var option = $('<option>').text(value.Name).val(value.Id);
                            selectList.append(option);
                        });
                        if (categories != '') {
                            selectList.val(categories);
                        }
                        $("#category1List").trigger("liszt:updated");
                    }
                    if (loadCategory2) {
                        var selectList = $("#category2List");
                        var categories = selectList.val();
                        selectList.empty();
                        $.each(result.SecondLevelCategories, function (key, value) {
                            var option = $('<option>').text(value.Name).val(value.Id);
                            selectList.append(option);
                        });
                        if (categories != '') {
                            selectList.val(categories);
                        }
                        $("#category2List").trigger("liszt:updated");
                    }
                    if (loadCategory3) {
                        var selectList = $("#category3List");
                        var categories = selectList.val();
                        selectList.empty();
                        $.each(result.ThirdLevelCategories, function (key, value) {
                            var option = $('<option>').text(value.Name).val(value.Id);
                            selectList.append(option);
                        });
                        if (categories != '') {
                            selectList.val(categories);
                        }
                        $("#category3List").trigger("liszt:updated");
                    }
                }
                else {
                    console.log('LoadStructure call failed!');
                }
            },
            error: function () {
                console.log('LoadStructure call failed!');
            }
        });
    };
    //terméklista betöltés
    var loadCatalogue = function () {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopApiUrl('GetProducts'),
            data: JSON.stringify(catalogueRequest),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //console.log(result);
                    $("#div_pager_top").empty();
                    $("#pagerTemplateTop").tmpl(result.Products).appendTo("#div_pager_top");
                    $("#div_pager_bottom").empty();
                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
                    $("#div_catalogue").empty();
                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
                    //$('.number').spin();
                }
                else {
                    console.log('loadCatalogueList result failed');
                }
            },
            error: function () {
                console.log('loadCatalogueList call failed');
            }
        });
    };
    //kérés paramétereit összefogó objektum
    var catalogueRequest = {
        ManufacturerIdList: [],
        Category1IdList: [],
        Category2IdList: [],
        Category3IdList: [],
        ActionFilter: false,
        BargainFilter: false,
        NewFilter: false,
        StockFilter: false,
        TextFilter: '',
        HrpFilter: true,
        BscFilter: true,
        PriceFilter: '0',
        PriceFilterRelation: '0',
        NameOrPartNumberFilter: '',
        Sequence: 0,
        CurrentPageIndex: 1,
        ItemsOnPage: 30,
        clear: function () {
            this.ManufacturerIdList = [];
            this.Category1IdList = [];
            this.Category2IdList = [];
            this.Category3IdList = [];
            this.ActionFilter = false;
            this.BargainFilter = false;
            this.NewFilter = false;
            this.StockFilter = false;
            this.TextFilter = '';
            this.HrpFilter = true;
            this.BscFilter = true;
            this.PriceFilter = '0';
            this.PriceFilterRelation = '0';
            this.NameOrPartNumberFilter = '';
            this.Sequence = 0;
            this.CurrentPageIndex = 1;
            this.ItemsOnPage = 30;
        }
    };
});

companyGroup.autocomplete = (function () {
    var initAutoCompletionBaseProduct = function () {
        $("#txt_filterbynameorpartnumber").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "GET",
                    url: companyGroup.utils.instance().getCompletionListBaseProductUrl(),
                    data: { Prefix: request.term },
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        if (result) {
                            var resultObject = result.Items;
                            var suggestions = [];
                            $.each(resultObject, function (i, val) {
                                suggestions.push(val);
                            });
                            response(suggestions);
                        }
                        else {
                            console.log(result);
                        }
                    },
                    error: function () {
                        console.log('CompletionListServiceUrl failed');
                    }
                });
            },
            minLength: 2
        }).data("autocomplete")._renderItem = function (ul, item) {
            console.log(item);
            var inner_html = '<div class="list_item_container"><div class="image"><img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(item.ProductId, item.RecId, item.DataAreaId) + ' alt=\"\" /></div><div class="label">' + item.ProductId + '</div><div class="description">' + item.ProductName + '</div></div>';
            return $("<li></li>")
            .data("item.autocomplete", item)
            .append(inner_html)
            .appendTo(ul);
        };
    };
    var initAutoCompletionAllProduct = function () {
        $("#txt_globalsearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "GET",
                    url: companyGroup.utils.instance().getCompletionListAllProductUrl(),
                    data: { Prefix: request.term },
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        if (result) {
                            var resultObject = result.Items;
                            var suggestions = [];
                            $.each(resultObject, function (i, val) {
                                suggestions.push(val);
                            });
                            response(suggestions);
                        }
                        else {
                            console.log(result);
                        }
                    },
                    error: function () {
                        console.log('CompletionListServiceUrl failed');
                    }
                });
            },
            minLength: 2
            //            //define select handler
            //            select: function (e, ui) {
            //                //create formatted friend
            //                var friend = ui.item.value,
            //							span = $("<span>").text(friend),
            //							a = $("<a>").addClass("remove").attr({
            //							    href: "javascript:",
            //							    title: "Remove " + friend
            //							}).text("x").appendTo(span);
            //                //add friend to friend div
            //							span.insertBefore("#txtSearch");
            //            },
            //            //define select handler
            //            change: function () {
            //                //prevent 'to' field being updated and correct position
            //                $("#txtSearch").val("").css("top", 2);
            //            }
            /*
            .data("autocomplete")._renderItem = function (ul, item) {
            var inner_html = '<a><div class="list_item_container"><div class="image"><img src="' + item.image + '"></div><div class="label">' + item.label + '</div><div class="description">' + item.description + '</div></div></a>';
            return $("<li></li>")
            .data("item.autocomplete", item)
            .append(inner_html)
            .appendTo(ul);
            };            
            */
        })
        .data("autocomplete")._renderItem = function (ul, item) {
            console.log(item);
            var inner_html = '<div class="list_item_container"><div class="image"><img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(item.ProductId, item.RecId, item.DataAreaId) + ' alt=\"\" /></div><div class="label">' + item.ProductId + '</div><div class="description">' + item.ProductName + '</div></div>';
            return $("<li></li>")
            .data("item.autocomplete", item)
            .append(inner_html)
            .appendTo(ul);
        };

    };

//    var loadCatalogue = function () {
//        var dataString = $.toJSON(catalogueRequest);
//        $.ajax({
//            type: "POST",
//            url: CompanyGroupCms.Constants.Instance().getProductListServiceUrl(),
//            data: dataString,
//            contentType: "application/json; charset=utf-8",
//            timeout: 10000,
//            dataType: "json",
//            processData: true,
//            success: function (result) {
//                if (result) {
//                    //console.log(result);
//                    $("#div_pager_top").empty();
//                    $("#pagerTemplateTop").tmpl(result.Products).appendTo("#div_pager_top");
//                    $("#div_pager_bottom").empty();
//                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
//                    $("#div_catalogue").empty();
//                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
//                    $('.number').spin();
//                }
//                else {
//                    alert('loadCatalogueList result failed');
//                }
//            },
//            error: function () {
//                alert('loadCatalogueList call failed');
//            }
//        });
//    };
    var downloadPriceList = function () {
        console.log('downloadPriceList');
        window.location = CompanyGroupCms.Constants.Instance().getDownloadPriceListServiceUrl() + '?' + $.param(catalogueRequest);
    };
//    var loadStructure = function (loadManufacturer, loadCategory1, loadCategory2, loadCategory3) {
//        var dataString = $.toJSON(catalogueRequest);
//        $.ajax({
//            type: "POST",
//            url: CompanyGroupCms.Constants.Instance().getStructureServiceUrl(),
//            data: dataString,
//            contentType: "application/json; charset=utf-8",
//            timeout: 10000,
//            dataType: "json",
//            processData: true,
//            success: function (result) {
//                if (result) {
//                    if (loadManufacturer) {
//                        var manufacturer = $('#manufacturerList').val()
//                        $("#manufacturerList").empty();
//                        $.each(result.Manufacturers, function (key, value) {
//                            var option = $('<option>').text(value.Name).val(value.Id);
//                            $("#manufacturerList").append(option);
//                        });
//                        if (manufacturer != '') {
//                            $("#manufacturerList").val(manufacturer);
//                        }
//                        $("#manufacturerList").trigger("liszt:updated");
//                    }
//                    if (loadCategory1) {
//                        var selectList = $("#category1List");
//                        var categories = selectList.val();
//                        selectList.empty();
//                        $.each(result.FirstLevelCategories, function (key, value) {
//                            var option = $('<option>').text(value.Name).val(value.Id);
//                            selectList.append(option);
//                        });
//                        if (categories != '') {
//                            selectList.val(categories);
//                        }
//                        $("#category1List").trigger("liszt:updated");
//                    }
//                    if (loadCategory2) {
//                        var selectList = $("#category2List");
//                        var categories = selectList.val();
//                        selectList.empty();
//                        $.each(result.SecondLevelCategories, function (key, value) {
//                            var option = $('<option>').text(value.Name).val(value.Id);
//                            selectList.append(option);
//                        });
//                        if (categories != '') {
//                            selectList.val(categories);
//                        }
//                        $("#category2List").trigger("liszt:updated");
//                    }
//                    if (loadCategory3) {
//                        var selectList = $("#category3List");
//                        var categories = selectList.val();
//                        selectList.empty();
//                        $.each(result.ThirdLevelCategories, function (key, value) {
//                            var option = $('<option>').text(value.Name).val(value.Id);
//                            selectList.append(option);
//                        });
//                        if (categories != '') {
//                            selectList.val(categories);
//                        }
//                        $("#category3List").trigger("liszt:updated");
//                    }
//                }
//                else {
//                    alert('LoadStructure call failed!');
//                }
//            },
//            error: function () {
//                alert('LoadStructure call failed!');
//            }
//        });
//    };
    return {
        DownloadPriceList: downloadPriceList,
        InitAutoCompletionAllProduct: initAutoCompletionAllProduct,
        InitAutoCompletionBaseProduct: initAutoCompletionBaseProduct
    };
})();


