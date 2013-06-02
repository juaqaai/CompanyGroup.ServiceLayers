//https://github.com/brandonaaron/sammystodos
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

    this.use(companyGroupHelpers);

    this.setTitle('HRP/BSC Webáruház -');

    this.get('#/finance_close', function (context) {
        //console.log(context);
		var r=confirm("Szeretné a törölni a kosarat?");
		
		
		
			if (r==true)
 				 {
  				 var data = {
            CartId: parseInt($("input#hidden_cartId").val())
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('RemoveCart'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
					$("#cus_ajanlat_finance").hide('slow');
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
					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
					context.redirect("#/removed")
                    //$('.cartnumber').spin();
                    $("input#hidden_cartId").val(result.ActiveCart.Id);
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('removeCart call failed');
            }
        });
  				}
				else
 				 {
				 $("#cus_ajanlat_finance").hide('slow');
  				context.redirect("#/notremoved")
  				} 	
				
    });
    this.get('#/order_close', function (context) {
        //console.log(context);
        $("#cus_rendeles_feladas").hide('slow');
    });
    this.get('#/', function (context) {
        //console.log(context);
 							$('#hidden_cartopen').val('0');
							
		$("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
        context.title('kezdőoldal');
        loadStructure(true, true, true, true);
        //részletes termékadatlap log lista
        loadCatalogueDetailsLogList();
    });

    //események
    this.bind('run', function (e, data) {
        var context = this;

        //bal oldali választólista változása
        $(".chzn-select").chosen(); // jQuery version
        $(".chzn-select").unbind('change').bind('change', function () {
            if ($(this).attr('id') === 'manufacturerList') {
                var manufacturerIdList = $('#manufacturerList').val();
				var category1IdList = $('#category1List').val();     
				 var category2IdList = $('#category2List').val();
				 var category3IdList = $('#category3List').val();
				  var manufacturerIdList2 = (manufacturerIdList === null || manufacturerIdList === '') ? [] :  manufacturerIdList;
				var category1IdList2 = (category1IdList === null || category1IdList === '') ? [] : '/' + category1IdList;     
				 var category2IdList2 = (category2IdList === null || category2IdList === '') ? [] : '/' + category2IdList;
				 var category3IdList2 = (category3IdList === null || category3IdList === '') ? [] : '/' + category3IdList;
                catalogueRequest.ManufacturerIdList = (manufacturerIdList === null || manufacturerIdList === '') ? [] : manufacturerIdList;
                catalogueRequest.CurrentPageIndex = 1;
                loadStructure(false, true, true, true);
                loadCatalogue();
                showProductList(true);
                context.redirect('#/' +  manufacturerIdList2 +    category1IdList2  +   category2IdList2  +   category3IdList2 );
                $("html, body").animate({ scrollTop: 0 }, 100);
            } else if ($(this).attr('id') === 'category1List') {
                 var manufacturerIdList = $('#manufacturerList').val();
				var category1IdList = $('#category1List').val();
				 var category2IdList = $('#category2List').val();
				 var category3IdList = $('#category3List').val();
				 var manufacturerIdList2 = (manufacturerIdList === null || manufacturerIdList === '') ? [] :  manufacturerIdList;
				var category1IdList2 = (category1IdList === null || category1IdList === '') ? [] : '/' + category1IdList;     
				 var category2IdList2 = (category2IdList === null || category2IdList === '') ? [] :'/' + category2IdList;
				 var category3IdList2 = (category3IdList === null || category3IdList === '') ? [] :'/' + category3IdList;
                catalogueRequest.Category1IdList = (category1IdList === null || category1IdList === '') ? [] : category1IdList;
                catalogueRequest.CurrentPageIndex = 1;
                loadStructure(true, false, true, true);
                loadCatalogue();
                showProductList(true);
                context.redirect('#/' +  manufacturerIdList2 +    category1IdList2  +   category2IdList2  +   category3IdList2 );
                $("html, body").animate({ scrollTop: 0 }, 100);
            } else if ($(this).attr('id') === 'category2List') {
                 var manufacturerIdList = $('#manufacturerList').val();
				var category1IdList = $('#category1List').val();
				 var category2IdList = $('#category2List').val();
				 var category3IdList = $('#category3List').val();
				 var manufacturerIdList2 = (manufacturerIdList === null || manufacturerIdList === '') ? [] :  manufacturerIdList;
				var category1IdList2 = (category1IdList === null || category1IdList === '') ? [] : '/' + category1IdList;     
				 var category2IdList2 = (category2IdList === null || category2IdList === '') ? [] : '/' + category2IdList;
				 var category3IdList2 = (category3IdList === null || category3IdList === '') ? [] : '/' + category3IdList;
                catalogueRequest.Category2IdList = (category2IdList === null || category2IdList === '') ? [] : category2IdList;
                catalogueRequest.CurrentPageIndex = 1;
                loadStructure(true, true, false, true);
                loadCatalogue();
                showProductList(true);
                context.redirect('#/' +  manufacturerIdList2 +    category1IdList2  +   category2IdList2  +   category3IdList2 );
                $("html, body").animate({ scrollTop: 0 }, 100);
            } else if ($(this).attr('id') === 'category3List') {
                var manufacturerIdList = $('#manufacturerList').val();
				var category1IdList = $('#category1List').val();
				var category2IdList = $('#category2List').val();
				var category3IdList = $('#category3List').val();
				var manufacturerIdList2 = (manufacturerIdList === null || manufacturerIdList === '') ? [] :  manufacturerIdList;
				var category1IdList2 = (category1IdList === null || category1IdList === '') ? [] : '/' + category1IdList;     
				 var category2IdList2 = (category2IdList === null || category2IdList === '') ? [] :'/' + category2IdList;
				 var category3IdList2 = (category3IdList === null || category3IdList === '') ? [] :'/' + category3IdList;
                catalogueRequest.Category3IdList = (category3IdList === null || category3IdList === '') ? [] : category3IdList;
                catalogueRequest.CurrentPageIndex = 1;
                loadStructure(true, true, true, false);
                loadCatalogue();
                showProductList(true);
                context.redirect('#/' +  manufacturerIdList2 +    category1IdList2  +   category2IdList2  +   category3IdList2 );
                $("html, body").animate({ scrollTop: 0 }, 100);
            }
        });



        //alsó (ajánlott) termék lapozó 
        $('#cus_recommended_prod_content').easyPaginate({
            step: 4,
            delay: 300,
            numeric: false,
            nextprev: true,
            auto: false,
            pause: 5000,
            clickstop: true,
            controls: 'pagination',
            current: 'current'
        });
        $('ul#items').easyPaginate({
            step: 3,
            nextprev: true,
            auto: false,
            numeric: false
        });


        //$(".expand_all").toggle(function () {
        /*$(".expand_all").toggle(function () {
        $(this).addClass("expanded");
        }, function () {
        $(this).removeClass("expanded");
        });*/

        //terméklista kiválasztott oldalindex változás
        $("#select_pageindex_top").live('change', function () {
            context.trigger('selectedPageIndexChanged', { PageIndex: parseInt($("#select_pageindex_top").val(), 0) });
        });
        //terméklista kiválasztott oldalindex változás
        $("#select_pageindex_bottom").live('change', function () {
            context.trigger('selectedPageIndexChanged', { PageIndex: parseInt($("#select_pageindex_bottom").val(), 0) });

        });
        //terméklista látható elemek száma változás
        $("#select_visibleitemlist_top").live('change', function () {
            context.trigger('visibleItemListChanged', { Orientation: 'top', Index: parseInt($("#select_visibleitemlist_top").val(), 0) });

        });
        //terméklista látható elemek száma változás
        $("#select_visibleitemlist_bottom").live('change', function () {
            context.trigger('visibleItemListChanged', { Orientation: 'bottom', Index: parseInt($("#select_visibleitemlist_bottom").val(), 0) });

        });
        //keresés névre, vagy cikkszámra
        $("#txt_globalsearch").live('focus', function () {
            if ('keresés a termékek között' === $(this).val()) {
                $(this).val('');
            }
        }).live('blur', function () {
            var $this = $(this),
                text = $.trim($this.val());
            if (!text) {
                $this.val('keresés a termékek között');
            }
        });
        //rendezés stílusbeállítás
        //$('.select2').skinner({'type':'left'});
        //kosárlista kiválasztott elem beállítása
        $("#saved_shoppingcart_list").live('change', function () {
            context.trigger('activateShoppingCart', { CartId: $(this).val() });
        });
        //kosárban lévő cikk mennyiség változása
        $(".cartnumber").live('change', function () {
            context.trigger('updateShoppingCartLine', { Quantity: $(this).val(), LineId: $(this).attr("title") });
        });
        //szűrés csak készleten-re
        $("#chk_onstock").live('change', function () {
            context.trigger('filterByStock', { Checked: $(this).is(':checked') });
        });
        //szűrés akciós-ra
        $("#chk_onaction").live('change', function () {
            context.trigger('filterByAction', { Checked: $(this).is(':checked') });
        });
        //szűrés leértékelt-re
        $("#chk_bargain").live('change', function () {
            context.trigger('filterByBargain', { Checked: $(this).is(':checked') });
        });
        //szűrés új-ra
        $("#chk_new").live('change', function () {
            context.trigger('filterByNew', { Checked: $(this).is(':checked') });
        });
        $("#chk_filterByHrp").live('change', function () {
            context.trigger('filterByHrp', { HrpChecked: $(this).is(':checked'), BscChecked: $('#chk_filterByBsc').is(':checked') });
        });
        $("#chk_filterByBsc").live('change', function () {
            context.trigger('filterByBsc', { HrpChecked: $('#chk_filterByHrp').is(':checked'), BscChecked: $(this).is(':checked') });
        });
        //teljes termékadatbázis keresés
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
                            var arr_suggestions = [];
                            $.each(result.Items, function (i, val) {
							
                                var inner_html = '<div class="list_item_container"><table border="0" cellpadding="5" cellspacing="0"><tr><td><div class="image">';
								if ( val.PictureId > 0 ) { 
								inner_html += '<img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(val.PictureId) + '" alt="" />';
								}
								else { 
								inner_html += '<img src="../Content/Media/images/nopic.jpg"  width="50" height="50" alt="" /> ';
								}  
								inner_html += '</a></div></td><td><div class="label"><strong></strong></div><div class="description"><a href="#/details/' + val.ProductId + '/' + val.DataAreaId + '">' + val.ProductName + '</a></div></td></tr></table></div>';  
                                arr_suggestions.push({ label: inner_html, value: val.ProductName });
                            });
                            response(arr_suggestions);
                        }
                        else {
                            //console.log(result);
                        }
                    },
                    error: function () {
                        //console.log('CompletionListServiceUrl failed');
                    }
                });
            },
            //            select: function (event, ui) {
            //                console.log(ui.item);
            //                //window.location.href = ui.item.ProductId;
            //            },
            minLength: 2,
            html: 'html'
        });
        //        .data("autocomplete")._renderItem = function (ul, item) {
        //            console.log(item);
        //            var inner_html = '<div class="list_item_container"><table border="0" cellpadding="5" cellspacing="0"><tr><td><div class="image"><a><img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(item.PictureId) + ' alt="" /></a></div></td><td><div class="label"><strong></strong></div><div class="description"><a href="#/details/' + item.ProductId + '/' + item.DataAreaId + '">' + item.ProductName + '</a></div></td></tr></table></div>';
        //            return $("<li></li>")
        //            .data("item.autocomplete", item)
        //            .append(inner_html)
        //            .appendTo(ul);
        //        };
        //fizetési mód beállítás
        $("input[name='radio_payment']").live('change', function () {
            //console.log($(this).val());
            context.trigger('changePayment', { Payment: parseInt($(this).val(), 0) });
        });
        //szállítási mód beállítás
        $("input[name='radio_delivery']").live('change', function () {
            //console.log($(this).val());
            context.trigger('changeDelivery', { Delivery: parseInt($(this).val(), 0) });
        });
        //rendelés form alapértelmezettre állítása
        $("input[name='resetOrderForm']").live('click', function () {
            //console.log('resetOrderForm');
            context.trigger('resetOrderForm');
        });
        $('.tabs a').live('click', function () {
            context.trigger('changeDetailsTab', $(this));
        });
        //switch_tabs($('.defaulttab'));
        $("select[name='catalogueSequence']").live('change', function () {
            context.trigger('catalogueSequence', { Sequence: parseInt($(this).val(), 0) });
        });
        //Lenyíló menük delay beállítása
        $("#navmenu").mouseover(function () {
            $("ul#navmenu ul li").slideDown("slow");
            $(this).toggleClass("active");
            return false;
        });
        $("#navmenu_order").mouseover(function () {
            $("ul#navmenu_order ul li").slideDown("slow");
            $(this).toggleClass("active");
            return false;
        });
        $("#navmenu_o").mouseover(function () {
            $("ul#navmenu_o li").slideDown("slow");
            $(this).toggleClass("active");
            return false;
        });
        $("#usermenu").mouseover(function () {
            $("ul#usermenu ul li").slideDown("slow");
            $(this).toggleClass("active");
            return false;
        });
        //oldal tetejére ugrás link
        //$('#top-link').topLink({
        //  min: 1,
        //  fadeSpeed: 500
        //  });
        //szállítási dátum, idő
        $("#naptar").datepicker({
            beforeShowDay: $.datepicker.noWeekends,
            minDate: +1,
            maxDate: "+1W +1D",
            defaultDate: +1,
            dateFormat: 'yy-mm-dd',
            constrainInput: true,
            //appendText: '(év-hónap-nap)',
            dayNames: ['Vasárnap', 'Hétfő', 'Kedd', 'Szerda', 'Csütörtök', 'Péntek', 'Szombat'],
            dayNamesMin: ['V', 'H', 'K', 'Sz', 'Cs', 'P', 'Sz'],
            firstDay: 1,
            monthNames: ['Jan', 'Feb', 'Már', 'Ápr', 'Máj', 'Jún', 'Júl', 'Aug', 'Szep', 'Okt', 'Nov', 'Dec'],
            prevText: '<strong><|</strong>',
            nextText: '<strong>|></strong>'
        });
        //jelleg szűrő fix pozíció beállítás
        var msie6 = $.browser == 'msie' && $.browser.version < 7;
        if (!msie6) {
            var top = $('#cus_banner_table').offset().top - parseFloat($('#cus_banner_table').css('margin-top').replace(/auto/, 0));
            $(window).scroll(function (event) {
                var y = $(this).scrollTop();
                if (y >= top) {
                    $('#cus_banner_table').addClass('fixed');
                } else {
                    $('#cus_banner_table').removeClass('fixed');
                }
            });
        }
    });
    //szűrés készleten lévő termékekre
    this.bind('filterByStock', function (e, data) {
        catalogueRequest.StockFilter = data.Checked;
		catalogueRequest.CurrentPageIndex = 1;
        loadStructure(true, true, true, true);
        loadCatalogue();
        this.title('szűrés készleten lévő termékekre');
        showProductList(true);
        $("html, body").animate({ scrollTop: 0 }, 100)
    });
    //szűrés akciós termékekre
    this.bind('filterByAction', function (e, data) {
        catalogueRequest.DiscountFilter = data.Checked;
		catalogueRequest.CurrentPageIndex = 1;
        loadStructure(true, true, true, true);
        loadCatalogue();
        this.title('szűrés akciós termékekre');
        showProductList(true);
        $("html, body").animate({ scrollTop: 0 }, 100)
    });
    //szűrés használt termékekre
    this.bind('filterByBargain', function (e, data) {
        catalogueRequest.SecondhandFilter = data.Checked;
		catalogueRequest.CurrentPageIndex = 1;
        loadStructure(true, true, true, true);
        loadCatalogue();
        this.title('szűrés használt termékekre');
        showProductList(true);
        $("html, body").animate({ scrollTop: 0 }, 100)
    });
    //szűrés új termékekre
    this.bind('filterByNew', function (e, data) {
        catalogueRequest.NewFilter = data.Checked;
		catalogueRequest.CurrentPageIndex = 1;
        loadStructure(true, true, true, true);
        loadCatalogue();
        this.title('szűrés új termékekre');
        showProductList(true);
        $("html, body").animate({ scrollTop: 0 }, 100)
    });
    this.bind('filterByHrp', function (e, data) {
        if (data.HrpChecked || data.BscChecked) {
            catalogueRequest.HrpFilter = data.HrpChecked;
			catalogueRequest.CurrentPageIndex = 1;
            loadStructure(true, true, true, true);
            loadCatalogue();
            this.title('szűrés hardware termékekre');
            showProductList(true);
            $("html, body").animate({ scrollTop: 0 }, 100)
        } else {
            $.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Figyelem!</H2><p>HRP, vagy BSC beállítás kötelező!</p></div>', {
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
                beforeClose: function () { $('#chk_filterByHrp').attr('checked', true); }
            });
        }
    });
    this.bind('filterByBsc', function (e, data) {
        if (data.HrpChecked || data.BscChecked) {
            catalogueRequest.BscFilter = data.BscChecked;
			catalogueRequest.CurrentPageIndex = 1;
            loadStructure(true, true, true, true);
            loadCatalogue();
            this.title('szűrés szoftver termékekre');
            showProductList(true);
            $("html, body").animate({ scrollTop: 0 }, 100)
        } else {
            $.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Figyelem!</H2><p>HRP, vagy BSC beállítás kötelező!</p></div>', {
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
                beforeClose: function () { $('#chk_filterByBsc').attr('checked', true); }
            });
        }
    });
    //fizetési mód beállítás
    this.bind('changePayment', function (e, data) {
        //készpénzes fizetés
        if (data.Payment === 1) {
            //            $('#raktari_off').hide();
            //            $('#kiszallitas_off').show();
            //            $('#cim_off').show();
            //            $('#idopont_off').show();
            //            $('#szallcimadat_off').show();
            //            $('#atutalas_off').show();
            //            $('#eloreutalas_off').show();
            //            $('#utanvet_off').show();
        }
        //átutalásos fizetés
        else if (data.Payment === 2) {
            //            $('#raktari_off').hide();
            //            $('#kiszallitas_off').hide();
            //            $('#keszpenz_off').show();
            //            $('#eloreutalas_off').show();
            //            $('#utanvet_off').show();
        }
        //előre utalásos fizetés
        else if (data.Payment === 3) {
            //            $('#raktari_off').hide();
            //            $('#kiszallitas_off').hide();
            //            $('#keszpenz_off').show();
            //            $('#atutalas_off').show();
            //            $('#utanvet_off').show();
        }
        //utánvét
        else if (data.Payment === 4) {
            //            $('#raktari_off').show();
            //            $('#kiszallitas_off').hide();
            //            $('#keszpenz_off').show();
            //            $('#eloreutalas_off').show();
            //            $('#atutalas_off').show();
        }
    });
    //szállítási mód beállítás
    this.bind('changeDelivery', function (e, data) {
        //raktári átvétel
        if (data.Delivery === 1) {
            //            $("#feladas").removeClass("feladasopciok_block");
            //            $("#feladas").addClass("feladasopciok_ok");
            //            $('.feladasopciok_ok').removeAttr("disabled");
            //            $('#szallcimadat_off').show();
            //            $('#kiszallitas_off').show();
            //            $('#custom_number').removeAttr("disabled");
            //            $('#user_comment').removeAttr("disabled");
        }
        //kiszállítást kér
        else {
            //            $('#szallcimadat_off').hide();
            //            $('#szallcimadat').show();
            //            $('#raktari_off').show();
        }
    });
    //váltás tabfülre
    this.bind('changeDetailsTab', function (e, data) {
        switch_tabs(data);
    });
    //rendelés form alapértelmezettre állítása
    this.bind('resetOrderForm', function (e, data) {
        //        $('#raktari_off').show();
        //        $('#kiszallitas_off').show();
        //        $('#cim_off').show();
        //        $('#idopont_off').show();
        //        $('#szallcimadat_off').show();
        //        $('#keszpenz_off').hide();
        //        $('#atutalas_off').hide();
        //        $('#eloreutalas_off').hide();
        //        $('#utanvet_off').hide();
        //        $('.feladasopciok_ok').attr("disabled", "disabled");
        //        $('#custom_number').attr("disabled", "disabled");
        //        $('#user_comment').attr("disabled", "disabled");
        //        $("#feladas").addClass("feladasopciok_block");
    });
    //oldal tetejére ugrás    
    //this.bind('scrollTo', function (e, data) {
    //e.preventDefault();
    //console.log(e);
    // e.scrollTop(300);
    //});
    //terméklista sorrend
    this.bind('catalogueSequence', function (e, data) {
        catalogueRequest.Sequence = data.Sequence;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        if (data.Sequence === 6) {
            this.title('rendezés ár szerint növekvőleg');
        }
        else if (data.Sequence === 7) {
            this.title('rendezés ár szerint csökkenőleg');
        }
        else if (data.Sequence === 2) {
            this.title('rendezés cikkszám szerint növekvőleg');
        }
        else if (data.Sequence === 3) {
            this.title('rendezés cikkszám szerint csökkenőleg');
        }
        else if (data.Sequence === 4) {
            this.title('rendezés név szerint növekvőleg');
        }
        else if (data.Sequence === 5) {
            this.title('rendezés név szerint csökkenőleg');
        }
        else if (data.Sequence === 8) {
            this.title('rendezés készlet szerint növekvőleg');
        }
        else if (data.Sequence === 9) {
            this.title('rendezés készlet szerint csökkenőleg');
        }
        else if (data.Sequence === 12) {
            this.title('rendezés garancia szerint növekvőleg');
        }
        else if (data.Sequence === 13) {
            this.title('rendezés garancia szerint csökkenőleg');
        }
        else if (data.Sequence === 14) {
            this.title('rendezés leértékelt szerint');
        }
        else if (data.Sequence === 15) {
            this.title('rendezés újdonság szerint');
        }
        showProductList(true);
    });
    this.get('#/expandall', function (context) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopApiUrl('SaveCatalogueOpenStatus'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result === true) {
                    $(".toggle_container").show('slow');

                } else {
                    $(".toggle_container").hide('slow');


                }
                context.redirect('#/catalogueopenedstatus/' + result);
            },
            error: function () {
                alert('saveCatalogueOpenStatus call failed');
            }
        });
    });

    this.get('#/catalogueopenedstatus/:status', function (context) {
        var status_msg = context['status'] ? 'bővített nézet' : 'alapértelmezett nézet';
        context.title('terméklista - ' + status_msg);
    });

    //vissza a terméklistára
    this.get('#/backtolist', function (context) {
        $('#hidden_product_id').val('');
        $('#hidden_product_dataareaid').val('');
        showProductList(true);
        loadCatalogueDetailsLogList();
        context.title('terméklista');
    });
    /*
    /// 0: átlagos életkor csökkenő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
    /// 1: átlagos életkor növekvő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg, 
    /// 2: azonosito növekvő, (cikkszám)
    /// 3: azonosito csökkenő, (cikkszám)
    /// 4  nev növekvő,
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
        context.title('rendezés ár szerint növekvőleg');
        showProductList(true);
    });
    //rendezés árra csökkenően
    this.get('#/sequenceByPriceDown', function (context) {
        catalogueRequest.Sequence = 7;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés ár szerint csökkenőleg');
        showProductList(true);
    });
    //rendezés cikkszámra növekvően
    this.get('#/sequenceByPartNumberUp', function (context) {
        //console.log(self);
        catalogueRequest.Sequence = 2;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés cikkszám szerint növekvőleg');
        showProductList(true);
    });
    //rendezés cikkszámra csökkenőleg
    this.get('#/sequenceByPartNumberDown', function (context) {
        //console.log(self);
        catalogueRequest.Sequence = 3;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés cikkszám szerint csökkenőleg');
        showProductList(true);
    });
    //rendezés név szerint növekvőleg
    this.get('#/sequenceByNameUp', function (context) {
        //console.log(this);
        catalogueRequest.Sequence = 4;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés név szerint növekvőleg');
        showProductList(true);
    });
    //rendezés név szerint csökkenőleg
    this.get('#/sequenceByNameDown', function (context) {
        //console.log(this);
        catalogueRequest.Sequence = 5;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés név szerint csökkenőleg');
        showProductList(true);
    });
    //rendezés készletre növekvően
    this.get('#/sequenceByStockUp', function (context) {
        catalogueRequest.Sequence = 8;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés készlet szerint növekvőleg');
        showProductList(true);
    });
    //rendezés készletre csökkenőleg
    this.get('#/sequenceByStockDown', function (context) {
        catalogueRequest.Sequence = 9;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés készlet szerint csökkenőleg');
        showProductList(true);
    });
    //rendezés garanciaidőre növekvően
    this.get('#/sequenceByGarantyUp', function (context) {
        catalogueRequest.Sequence = 12;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés garancia szerint növekvőleg');
        showProductList(true);
    });
    //rendezés garanciaidőre csökkenőleg
    this.get('#/sequenceByGarantyDown', function (context) {
        catalogueRequest.Sequence = 13;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        context.title('rendezés garancia szerint csökkenőleg');
        showProductList(true);
    });
    //szűrés a hrp termékekre
    //    this.get('#/filterByHrp', function (context) {
    //        catalogueRequest.clear();
    //        catalogueRequest.HrpFilter = true;
    //        catalogueRequest.BscFilter = false;
    //        loadCatalogue();
    //        context.title('hardver termékek');
    //        showProductList(true);
    //    });
    //kilépés
    this.get('#/signOut', function (context) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopApiUrl('SignOut'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (!result.Visitor.IsValidLogin) {
                    //$('.number').spinner();
                    $("#chk_onstock").prop('checked', false);
                    $("#chk_onaction").prop('checked', false);
                    $("#chk_bargain").prop('checked', false);
                    $("#chk_new").prop('checked', false);

                    $('option').prop('selected', false);
                    $('.chzn-select').trigger('liszt:updated');

                    $("#txt_globalsearch").val('');
                    $("#txt_filterbyprice").val('');

                    $("#cus_header1").empty();
                    var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result.Visitor);
                    $('#cus_header1').html(visitorInfoHtml);


                    $("#usermenuContainer").empty();
                    var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result.Visitor);
                    $('#usermenuContainer').html(usermenuHtml);

                    $("#becomePartnerContainer").empty();
                    var becomePartnerHtml = Mustache.to_html($('#becomePartnerTemplate').html(), result.Visitor);
                    $('#becomePartnerContainer').html(becomePartnerHtml);

                    $("#newsletterContainer").empty();
                    var newsletterHtml = Mustache.to_html($('#newsletterTemplate').html(), result.Visitor);
                    $('#newsletterContainer').html(newsletterHtml);

                    $("#div_pager_top").empty();
                    $("#pagerTemplateTop").tmpl(result).appendTo("#div_pager_top");


                    $("#div_pager_bottom").empty();
                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");

                    $("#div_catalogue").empty();
                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
                    //$("#catalogueSequenceContainer").empty();
                    //$("#catalogueSequenceTemplate").tmpl(result.Visitor).appendTo("#catalogueSequenceContainer");
                    //$("#catalogueDownloadContainer").empty();
                    //$("#catalogueDownloadTemplate").tmpl(result.Visitor).appendTo("#catalogueDownloadContainer");
                    //$("#cus_filter_price").empty();
                    //$("#priceFilterTemplate").tmpl(result.Visitor).appendTo("#cus_filter_price");
                    $("#negyes_szuro").hide();
                    $("#cus_filter_price").hide();
                    $("#cus_checkout_menu").hide();

                    $("#hidden_cartId").val('');
                    $('#hidden_cartopen').val('0');
                    //CompanyGroupCms.ShoppingCartInfo.Instance().SetCartId('');
                    //CompanyGroupCms.ShoppingCartSummary.Instance().Init(result.Visitor.IsValidLogin, 0);
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#deliveryAddressTemplate").tmpl(result.DeliveryAddresses).appendTo("#deliveryAddressContainer");
                    $("#basket_panel").hide();

                    var productId = $('#hidden_product_id').val();
                    var dataAreaId = $('#hidden_product_dataareaid').val();
                    $(".toggle_container").show();

                    if (productId && dataAreaId) {
                        context.redirect('#/refreshdetails/' + productId + '/' + dataAreaId);
                    }

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
        //        var error_msg = '';

        //        if ($("#txt_username").val() === '') {
        //            error_msg += 'A bejelentkezési név kitöltése kötelező! <br/>';
        //        }
        //        if ($("#txt_password").val() === '') {
        //            error_msg += 'A jelszó kitöltése kötelező!';
        //        }
        //        $("#login_errors").html(error_msg);

        //        return (error_msg === '');
        return this.beforeSignIn();
    });
    //bejelentkezés (Visitor + Products objektummal tér vissza)
    this.post('#/signin', function (context) {
        var data = {
            UserName: context.params['txt_username'],
            Password: context.params['txt_password']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopApiUrl('SignIn'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result.Visitor.IsValidLogin) {
                    $.fancybox.close();
                    //$('.number').spinner();

                    //console.log(result);
                    $("#cus_header1").empty();
                    var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result.Visitor);
                    $('#cus_header1').html(visitorInfoHtml);

                    $("#usermenuContainer").empty();
                    var usermenuHtml = Mustache.to_html($('#usermenuTemplate').html(), result.Visitor);
                    $('#usermenuContainer').html(usermenuHtml);

                    $("#becomePartnerContainer").empty();
                    var becomePartnerHtml = Mustache.to_html($('#becomePartnerTemplate').html(), result.Visitor);
                    $('#becomePartnerContainer').html(becomePartnerHtml);

                    $("#newsletterContainer").empty();
                    var newsletterHtml = Mustache.to_html($('#newsletterTemplate').html(), result.Visitor);
                    $('#newsletterContainer').html(newsletterHtml);

                    $("#div_pager_top").empty();
                    $("#pagerTemplateTop").tmpl(result).appendTo("#div_pager_top");

                    $("#div_pager_bottom").empty();
                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
                    $("#div_catalogue").empty();
                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
                    //$("#catalogueSequenceContainer").empty();
                    //$("#catalogueSequenceTemplate").tmpl(result.Visitor).appendTo("#catalogueSequenceContainer");
                    //$("#catalogueDownloadContainer").empty();
                    //$("#catalogueDownloadTemplate").tmpl(result.Visitor).appendTo("#catalogueDownloadContainer");

                    //$("#cus_filter_price").empty();
                    //$("#priceFilterTemplate").tmpl(result.Visitor).appendTo("#cus_filter_price");
                    $("#negyes_szuro").show();
                    $("#cus_filter_price").show();
                    $("#cus_checkout_menu").show();

					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
                    $("#hidden_cartId").val(result.ActiveCart.Id);
                    //CompanyGroupCms.ShoppingCartInfo.Instance().SetCartId(result.ActiveCart.Id);
                    //CompanyGroupCms.ShoppingCartSummary.Instance().Init(result.Visitor.IsValidLogin, result.ActiveCart.SumTotal);
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button"); // CompanyGroupCms.ShoppingCartSummary.Instance() 
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $('#hidden_cartopen').val('0');
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');

                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#deliveryAddressTemplate").tmpl(result.DeliveryAddresses).appendTo("#deliveryAddressContainer");

                    var productId = $('#hidden_product_id').val();
                    var dataAreaId = $('#hidden_product_dataareaid').val();
                    if (productId && dataAreaId) {
                        context.redirect('#/refreshdetails/' + productId + '/' + dataAreaId);
                    }
                    else {
                        context.redirect('#/authenticated');
                    }
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
        //console.log('authenticated');

    });
    //bejelentkezés panel megmutatása
    this.get('#/showSignInPanel', function (context) {
        //        $.fancybox({
        //            href: '#div_login',
        //            autoDimensions: true,
        //            autoScale: false,
        //            transitionIn: 'fade',
        //            transitionOut: 'fade',
        //            beforeClose: function () { console.log('signin panel closed'); }
        //        });
        this.showSignInPanel();
    });
    this.get('#/showForgetPasswordPanel', function (context) {
        this.showForgetPasswordPanel();
    });
    this.post('#/sendforgetpassword', function (context) {
        this.sendForgetPassword(context.params['txt_forgetpassword_username']);
    });
    //nyelv megváltoztatása
    this.get('#/changeLanguage/:language', function (context) {
        this.changeLanguage(context.params['language']);
        //        var data = {
        //            Language: (context.params['language'] === '' || context.params['language'] === 'HU') ? 'EN' : 'HU'
        //        };
        //        $.ajax({
        //            type: "POST",
        //            url: companyGroup.utils.instance().getCustomerApiUrl('ChangeLanguage'),
        //            data: JSON.stringify(data),
        //            contentType: "application/json; charset=utf-8",
        //            timeout: 10000,
        //            dataType: "json",
        //            processData: true,
        //            success: function (result) {
        //                console.log('ChangeLanguage');
        //                $("#inverse_language_id").html(result.Language);
        //            },
        //            error: function () {
        //                console.log('ChangeLanguage call failed');
        //            }
        //        });
    });
    //pénznem meváltoztatása
    this.get('#/changeCurrency/:currency', function (context) {
        var data = {
            Currency: context.params['currency']
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getVisitorApiUrl('ChangeCurrency'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                $("#cus_header1").empty();
                var visitorInfoHtml = Mustache.to_html($('#visitorInfoTemplate').html(), result);
                $('#cus_header1').html(visitorInfoHtml);
                loadCatalogue();
                loadShoppingCart();
                var productId = $('#hidden_product_id').val();
                var dataAreaId = $('#hidden_product_dataareaid').val();
                if (productId && dataAreaId) {
                    context.redirect('#/refreshdetails/' + productId + '/' + dataAreaId);
                }
            },
            error: function () {
                //console.log('ChangeCurrency call failed');
            }
        });
    });

    this.post('#/searchByTextFilter', function (context) {
        //console.log(context.params['txt_globalsearch']);
        catalogueRequest.TextFilter = context.params['txt_globalsearch'];
        catalogueRequest.CurrentPageIndex = 1;
        loadStructure(true, true, true, true);
        loadCatalogue();
        showProductList(true);
    });

    //kosár mentése
    this.post('#/saveShoppingCart', function (context) {
        var data = {
            CartId: parseInt($("input#hidden_cartId").val()),
            Name: $('#txt_cartname').val()
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('SaveCart'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    $.fancybox.close();
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    context.redirect("#/saved")
                    //$("#deliveryAddressTemplate").tmpl(result.DeliveryAddresses).appendTo("#site_select");
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
                    //$('.cartnumber').spin();
                }
                else {
                    //console.log('Nincs eleme a listának.');
                }
            },
            error: function () {
                //console.log('saveCart call failed');
            }
        });
    });

    this.post('#/addSite', function (context) {

    });

    this.post('#/importShoppingCart', function (context) {

    });
    //szűrés ár szerint
    this.post('#/filterbyprice', function (context) {
        catalogueRequest.PriceFilter = parseInt($('#txt_filterbyprice').val(), 0);
        catalogueRequest.PriceFilterRelation = parseInt($('#select_filterbyprice').val(), 0);
        loadStructure(true, true, true, true);
        loadCatalogue();
        showProductList(true);
        $("html, body").animate({ scrollTop: 0 }, 100)
    });
    //kosár állapotát menti
    this.get('#/saveShoppingCartOpenStatus/:status', function (context) {

        var isOpen = ($('#hidden_cartopen').val() === '1');
        isOpen = !isOpen;
        if (isOpen) {
            $('#hidden_cartopen').val('1');
            $("#shoppingCartSummaryCaption").text('bezárása');
            $("#basket_panel").show("fast");
            context.redirect("#/cartopened")

            //$("#checkout_btn").hide();
        } else {
            $('#hidden_cartopen').val('0');
            $("#shoppingCartSummaryCaption").text('módosítása / Megrendelés');
            $("#basket_panel").hide("fast");
            context.redirect("#/cartclosed")

            //$("#checkout_btn").show();
        }
        var data = {
            IsOpen: context.params['status'] == 1 ? true : false
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
                context.title('kosár');

                $("#cus_rendeles_feladas").hide();
                // $("#basket_panel").slideToggle("fast");
                $("#active_basket").toggleClass("active");
                $("#cus_ajanlat_finance").hide();
                //$("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                //alert(isOpen)
                //context.redirect('#/shoppingcartopenedstatus');
            },
            error: function () {
                alert('saveShoppingCartOpenStatus call failed');
            }
        });
    });
    //rendelés feladás
    this.get('#/showCreateOrder', function (context) {
        context.title('rendelés feladás');
        $("#cus_rendeles_feladas").slideToggle();
        $("#cus_ajanlat_vegfelhasznalo").hide();
        $("#cus_ajanlat_finance").hide();
        $("#atvetel").prop('checked', false);
        $("#transfer").prop('checked', false);
        $("#cash").prop('checked', false);
        $("#kiszallitas").prop('checked', false);
        $("#payafter").prop('checked', false);
        $('#szal_adat').hide();
        $('#payment_option').hide();
        $('#order_rest').hide();
        $('.hr1').hide();
        $("#span_hrporderid").html("");
        $("#span_hrpsecondhandorderid").html("");
        $("#span_bscorderid").html("");
        $("#span_bscsecondhandorderid").html("");
		if (result.ActiveCart.Items.length > 0) {
                        $("#checkout_btn").show();
						$("#finance_btn").show();
                    }
                    else {
                        $("#checkout_btn").hide();
						$("#finance_btn").hide();
                    }

        //ha valamiből nincs készleten, akkor nem választható dátum


    });
    //megrendeles állapotát menti
    /* this.get('#/showCreateOrder/:status', function (context) {
    var isOpen = ($('#hidden_cartopen').val() === '1');
    isOpen = !isOpen;
    if (isOpen) {
    $('#hidden_cartopen').val('1');
   			
    } else {
    $('#hidden_cartopen').val('');
   				
    }  
    var data = {
    IsOpen: context.params['status'] == 1 ? true : false
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
    $("#basket_panel").show();
    $("#cus_rendeles_feladas").show();
    $("#cus_ajanlat_vegfelhasznalo").hide();
    $("#cus_ajanlat_finance").hide();
    context.title('rendelés feladás');
    $("#basket_panel").slideToggle("fast");
    $("#active_basket").toggleClass("active");
    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '') ? 'módosítása' : 'bezárása');
    context.redirect('#/shoppingcartopenedstatus');
    },
    error: function () {
    alert('saveShoppingCartOpenStatus call failed');
    }
    });
    });*/
    //kosár hozzáadás
    this.get('#/addShoppingCart', function (context) {
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
                    context.redirect("#/added")
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
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
    //aktív kosár elemeinek eltávolítása
    this.get('#/removeShoppingCart', function (context) {
        var r = confirm("Valóban törölni szeretné a kosarat?");
        if (r == true) {
            var data = {
                CartId: parseInt($("input#hidden_cartId").val())
            };
            $.ajax({
                type: "POST",
                url: companyGroup.utils.instance().getShoppingCartApiUrl('RemoveCart'),
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
                        $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
					
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
					context.redirect("#/removed")
                    //$('.cartnumber').spin();
                    $("input#hidden_cartId").val(result.ActiveCart.Id);
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
                }
                else {
                    alert('Nincs eleme a listának.');
                }
            },
            error: function () {
                alert('removeCart call failed');
            }
        });
  				}
				else
 				 {

            context.redirect("#/notremoved")
        }

    });
    //kosár mentése panel megmutatás
    this.get('#/showSaveCartPanel', function (context) {
        context.redirect("#/savecart");
        $.fancybox({
            href: '#save_basket_win',
            closeBtn: true,
            autoDimensions: true,
            autoScale: false,
            transitionIn: 'fade',
            transitionOut: 'fade'
        });
    });
    //kosár sor eltávolítás
    this.get('#/removeLineFromShoppingCart/:lineId', function (context) {
        var data = {
            CartId: parseInt($("input#hidden_cartId").val()),
            LineId: context.params['lineId']
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
                    //$('.cartnumber').spin();
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
         							if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
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
    //
    this.before({ only: { verb: 'post', path: '#/addLineToShoppingCart'} }, function (context) {
        var stock = parseInt(context.params['hidden_stock']);
        var endofsales = context.params['hidden_endofsales'];
        var quantity = parseInt(context.params['txt_quantity']);
        $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');



        var notforsale = ((quantity > stock) && (endofsales === 'true'));
        if (notforsale === true) {
            $.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>A termékből legfeljebb ' + stock + ' rendelhető!</H2><p>Adjon meg a ' + stock + '-nál kisebb, vagy ezzel megegyező értéket!<p></div>', {
                'autoDimensions': true,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic',
                'closeBtn': 'true',
                'changeFade': 0,
                'speedIn': 300,
                'speedOut': 300,
                'width': '150%',
                'height': '150%',
                'autoScale': true
            });
        }
        //console.log(endofsales);
        //console.log('stock ' + stock + ' quantity ' + quantity + ' notforsale ' + notforsale);
        return (!notforsale);
    });
    //kosár sor hozzáadás 
    this.post('#/addLineToShoppingCart', function (context) {
        //console.log(context.params['productId'] + ' ' + context.params['dataAreaId']);
        var isSecondHand = (context.params['hidden_secondhand'] == 1) ? true : false;
        var data = {
            CartId: parseInt($('input#hidden_cartId').val()),   //alert($('input#foo').val());
            ProductId: context.params['hidden_productid'],
            Quantity: context.params['txt_quantity'],
            DataAreaId: context.params['hidden_dataareaid'],
            SecondHand: isSecondHand
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
                    //$('.cartnumber').spin();
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
          							if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    $.floatingMessage('<span style="font-family: verdana; font-size: 13px; color:#fff;"> A kiválasztott termék:<br /><strong>' + data.ProductId + '</strong><br />bekerült a kosárba.</span>', {
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
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
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
    //kosár sor módosítás
    this.bind('updateShoppingCartLine', function (e, data) {
        var data = {
            CartId: parseInt($("input#hidden_cartId").val()),
            LineId: data.LineId,
            Quantity: data.Quantity
        };
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
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
											if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
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
    //rendelés feladás
    this.post('#/createOrder', function (context) {
        var deliveryDate = $("#naptar").val();
        if (deliveryDate == 'Kattintson ide') {
            var currentDate = new Date()
            deliveryDate = currentDate.getFullYear() + '-' + (currentDate.getMonth() + 1) + '-' + currentDate.getDate();
        }

        var data = {
            CartId: parseInt($("input#hidden_cartId").val()),
            CustomerOrderNote: $("#user_comment").val(),
            CustomerOrderId: $("#custom_number").val(),
            DeliveryRequest: $("input[name=radio_delivery]:checked").val() === '2',  //szállítást kért-e
            DeliveryDate: deliveryDate,                                            //szállítás időpontja
            PaymentTerm: $("input[name=radio_payment]:checked").val(),               //1: átut, 2: KP, 3: előreut, 4: utánvét
            DeliveryTerm: $("input[name=radio_delivery]:checked").val(),             //1: raktár, 2: kiszállítás
            DeliveryAddressRecId: $("#site_select").val()                                //szállítási cím azonosító
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('CreateOrder'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 0,
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
                    IsValidated                 
                    */
                    if (result.Created) {
                        context.title('rendelés feladás');

                        $("#cus_rendeles_feladas").hide();
                        $("#active_basket").toggleClass("active");
                        $("#atvetel").prop('checked', false);
                        $("#transfer").prop('checked', false);
                        $("#cash").prop('checked', false);
                        $("#kiszallitas").prop('checked', false);
                        $("#payafter").prop('checked', false);
                        $('#szal_adat').hide();
                        $('#payment_option').hide();
                        $('#order_rest').hide();
                        $('.hr1').hide();
                        $("#basket_panel").hide("slow");
                        $("#span_hrporderid").html('');
                        $("#span_hrpsecondhandorderid").html('');
                        $("#span_bscorderid").html('');
                        $("#span_bscsecondhandorderid").html('');
						if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }




                        $("#hrporderid_txt").show();
                        $("#hrpsecondhandorderid_txt").show();
                        $("#bscorderid_txt").show();
                        $("#bscsecondhandorderid_txt").show();


                        //$("#span_hrporderid_text").html(($('#span_hrporderid').html() === '') ? 'Az Ön HRP bizonylat száma: ' : 'there is nothing');
                        // $("#span_hrpsecondhandorderid_text").html(($('#span_hrpsecondhandorderid').html() === '') ? 'Az Ön HRP (leértékelt) bizonylat száma: ' : 'there is nothing');
                        // $("#span_bscorderid_text").html(($('#span_bscorderid').html() === '') ? 'Az BSC bizonylat száma:  ' : 'there is nothing');
                        //  $("#span_bscsecondhandorderid_text").html(($('#span_bscsecondhandorderid').html() === '') ? 'Az Ön BSC (leértékelt) bizonylat száma: ' : 'there is nothing');

                        //alert(result.HrpOrderId +" "+ result.HrpSecondHandOrderId +" "+ result.BscOrderId +" "+ result.BscSecondHandOrderId)



                        if (result.HrpOrderId === '') {

                            $("#hrporderid_txt").hide();
                        }

                        if (result.HrpSecondHandOrderId === '') {

                            $("#hrpsecondhandorderid_txt").hide();
                        }

                        if (result.BscOrderId === '') {

                            $("#bscorderid_txt").hide();
                        }


                        if (result.BscSecondHandOrderId === '') {


                            $("#bscsecondhandorderid_txt").hide();
                        }

                        $("#span_hrporderid").html(result.HrpOrderId);
                        $("#span_hrpsecondhandorderid").html(result.HrpSecondHandOrderId);
                        $("#span_bscorderid").html(result.BscOrderId);
                        $("#span_bscsecondhandorderid").html(result.BscSecondHandOrderId);

						if (result.WaitForAutoPost == false){

                        $.fancybox('<p>A rendelés feladása sikeresen megtörtént</p>',
                        {
                            href: '#order_confirm',
                            'autoDimensions': true,
                            'transitionIn': 'elastic',
                            'transitionOut': 'elastic',
                            'changeFade': 0,
                            'speedIn': 300,
                            'speedOut': 300,
                            'width': '150%',
                            'height': '150%',
                            'autoScale': true
                        });
                    }
					if (result.WaitForAutoPost == true) {
								$.fancybox('<p>A rendelés feladása sikeresen megtörtént</p>',
                        {
                            href: '#order_confirm_wait',
                            'autoDimensions': true,
                            'transitionIn': 'elastic',
                            'transitionOut': 'elastic',
                            'changeFade': 0,
                            'speedIn': 300,
                            'speedOut': 300,
                            'width': '150%',
                            'height': '150%',
                            'autoScale': true
                        });
					}
						
						
					}
                    else {
                        $("#span_createorder_message").html(result.Message);
                    }
                    //console.log(result);
                    $("#basket_open_button").empty();
                    $("#shoppingCartSummaryTemplate").tmpl(result).appendTo("#basket_open_button");
                    $("#cus_basket_menu").empty();
                    $("#shoppingCartHeaderTemplate").tmpl(result).appendTo("#cus_basket_menu");
                    $("#cus_basket").empty();
                    $("#shoppingCartLineTemplate").tmpl(result).appendTo("#cus_basket");
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");
                    $('#hidden_cartopen').val('0');
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
                    ///$('.cartnumber').spin();
                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    //$("#form_createorder").hide();
                    $("input#hidden_cartId").val(result.ActiveCart.Id);
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
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
    this.post('#/createFinanceOffer', function (context) {
        var data = {
            //            PersonName: $("#txt_offername").val(),
            //            Address: $("#txt_offeraddress").val(),
            //            Phone: $("#txt_offerphone").val(),
            //            StatNumber: $("#txt_offerstatnumber").val(),
            NumOfMonth: context.params['radio_selectNumOfMonth'] //$("input[name=radio_selectNumOfMonth]").val()
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopBaseUrl('CreateFinanceOffer'),
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
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');

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
					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
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
            CartId: parseInt(data.CartId)
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
                    $("#shoppingCartSummaryCaption").text(($('#hidden_cartopen').val() === '0') ? 'módosítása / Megrendelés' : 'bezárása');
							if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
                    }
                    $("#leasingOptionsContainer").empty();
                    $("#leasingOptionsTemplate").tmpl(result.LeasingOptions).appendTo("#leasingOptionsContainer");

                    $("input#hidden_cartId").val(result.ActiveCart.Id);

                    if (result.LeasingOptions.Items.length == 0) {
                        $("#form_financeoffer").hide();
                    }
                    else {
                        $("#form_financeoffer").show();
                    }
                    allowDeliveryDateSettings(result.ActiveCart.AllInStock);
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

    //kiválasztott sorszámú lapra ugrás
    this.bind('selectedPageIndexChanged', function (e, data) {
        //console.log('selectedPageIndexChanged: ' + data.PageIndex);
        catalogueRequest.CurrentPageIndex = parseInt(data.PageIndex, 0);
        loadCatalogue();
        this.title('kiválasztott sorszámú lapra ugrás');
    });
    //ugrás az első oldalra
    this.get('#/firstpage', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            catalogueRequest.CurrentPageIndex = 1;
            loadCatalogue();
            context.title('Első oldal');
        }
    });
    //ugrás az utolsó oldalra
    this.get('#/lastPage', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        var lastPageIndex = parseInt($("#spanTopLastPageIndex").text(), 0);
        if (currentPageIndex < (lastPageIndex)) {
            catalogueRequest.CurrentPageIndex = lastPageIndex;
            loadCatalogue();
            context.title('Utolsó oldal');
        }
    });
    //ugrás a következő oldalra
    this.get('#/nextPage/:index', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context.params['index']);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        var lastPageIndex = parseInt($("#spanTopLastPageIndex").text(), 0);
        if (currentPageIndex < (lastPageIndex)) {
            currentPageIndex = currentPageIndex + 1;
            catalogueRequest.CurrentPageIndex = currentPageIndex;
            loadCatalogue();
            context.title('Következő oldal');
        }
    });
    //ugrás az előző oldalra
    this.get('#/previousPage/:index', function (context) {
        $("html, body").animate({ scrollTop: 0 }, 100);
        //console.log(context.params['index']);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            currentPageIndex = currentPageIndex - 1;
            catalogueRequest.CurrentPageIndex = currentPageIndex;
            loadCatalogue();
            context.title('Előző oldal');
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
        this.title('megjelenített elemek száma változás');
    });
    //szűrés a hrp hardvare termékekre
    this.get('#/filterByCategoryHrp/:category', function (context) {
        catalogueRequest.clear();
        catalogueRequest.Category1IdList.push(context.params['category']);
        loadStructure(true, true, true, true);
        loadCatalogue();
        $('#category1List').val(context.params['category']);
        $("#category1List").trigger("liszt:updated");
        context.title('hardver termékek');
        showProductList(true);
        ;
    });
    //szűrés a bsc termékekre
    //    this.get('#/filterByBsc', function (context) {
    //        catalogueRequest.clear();
    //        catalogueRequest.HrpFilter = false;
    //        catalogueRequest.BscFilter = true;
    //        loadCatalogue();
    //        loadStructure(true, true, true, true);
    //        context.title('szoftver termékek');
    //        showProductList(true);
    //    });
    //szűrés a bsc szoftvertermékeire
    this.get('#/filterByCategoryBsc/:category', function (context) {
        catalogueRequest.clear();
        catalogueRequest.Category1IdList.push(context.params['category']);
        loadStructure(true, true, true, true);
        loadCatalogue();
        $('#category1List').val(context.params['category']);
        $("#category1List").trigger("liszt:updated");
        context.title('szoftver termékek');
        showProductList(true);
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
        $('#chk_filterByBsc').attr('checked', true);
        $('#chk_filterByHrp').attr('checked', true);
        catalogueRequest.clear();
        loadStructure(true, true, true, true);
        loadCatalogue();
        showProductList(true);
        $("#txt_globalsearch").val('');
        $("#txt_filterbyprice").val('');
        context.redirect('#/clearedFilters');
        $("#chk_onstock").prop('checked', false);
        $("#chk_onaction").prop('checked', false);
        $("#chk_bargain").prop('checked', false);
        $("#chk_new").prop('checked', false);
    });
    this.get('#/clearedFilters', function (context) {
        context.title('szűrőfeltételek törlése');
    });
    //használtcikk lista
    this.get('#/showSecondHandList/:productId/:dataAreaId', function (context) {
        alert('Ide jön a ' + productId + '-hoz kapcsolt használtcikk lista,');
        context.title('leértékelt lista');
        showProductList(true);

    });
    //keresés a termékek között
    this.get('#/searchByTextFilter/:textfilter', function (context) {
        catalogueRequest.TextFilter = context.params['textfilter'];
        catalogueRequest.CurrentPageIndex = 1;
        loadStructure(true, true, true, true);
        loadCatalogue();
        context.title('keresés');
        showProductList(true);

    });

    this.get('#/closed', function (context) {
        //console.log(context);
        $.fancybox.close()
    });
    //termék adatlap
    this.get('#/details/:productId/:dataAreaId', function (context) {

        var productId = context.params['productId'];
        var dataAreaId = context.params['dataAreaId'];
        $("html, body").animate({ scrollTop: 0 }, 100);
        $('#hidden_product_id').val(productId);
        $('#hidden_product_dataareaid').val(dataAreaId);
        loadDetails(productId, dataAreaId);
        context.title(productId + ' adatlap');


    });
    //termék adatlap frissítése
    this.get('#/refreshdetails/:productId/:dataAreaId', function (context) {



        var productId = context.params['productId'];
        var dataAreaId = context.params['dataAreaId'];
        loadDetails(productId, dataAreaId);
        context.title(productId + ' adatlap');



    });
    //recent items
    this.get('#/getcataloguedetailsloglist', function (context) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopApiUrl('GetCatalogueDetailsLogList'),
            data: {},
            contentType: "application/json; charset=utf-8",
            timeout: 15000,
            dataType: "json",
            processData: true,
            success: function (result) {
                //console.log(result);
            },
            error: function () {
                //console.log('Service call failed: GetCatalogueDetailsLogList');
            }
        });
    });
    //finanszírozási ajánlat
    this.get('#/showCreateFinanceOffer', function (context) {
        $("#cus_ajanlat_finance").slideToggle();
		$("#submit_finance").hide();
        $("#cus_ajanlat_vegfelhasznalo").hide();
        $("#cus_rendeles_feladas").hide();
        context.title('finanszírozási ajánlat');
    });
    //árlista letöltés
    this.get('#/downloadPriceList', function (context) {
        //console.log('downloadPriceList');
        window.location = companyGroup.utils.instance().getDownloadPriceListUrl() + '?' + $.param(catalogueRequest);
        //        $.ajax({
        //            type: "GET",
        //            url: companyGroup.utils.instance().getDownloadPriceListUrl(),
        //            data: catalogueRequest,
        //            contentType: "application/json; charset=utf-8",
        //            timeout: 10000,
        //            dataType: "json",
        //            processData: true,
        //            success: function (result) {
        //                if (result) {
        //                    //console.log(result);
        ////                    $("#div_pager_top").empty();
        ////                    $("#pagerTemplateTop").tmpl(result.Products).appendTo("#div_pager_top");
        ////                    $("#div_pager_bottom").empty();
        ////                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
        ////                    $("#div_catalogue").empty();
        ////                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
        //                    //$('.number').spin();
        //                }
        //                else {
        //                    console.log('downloadPriceList result failed');
        //                }
        //            },
        //            error: function () {
        //                console.log('downloadPriceList call failed');
        //            }
        //        });
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
    //struktúra betöltés
    var loadStructure = function (loadManufacturer, loadCategory1, loadCategory2, loadCategory3) {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getStructureApiUrl('GetStructure'),
            data: JSON.stringify(catalogueRequest),
            contentType: "application/json; charset=utf-8",
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result) {
                    //var loadedManufacturer = false; var loadedCategory1 = false; var loadedCategory2 = false; var loadedCategory3 = false;
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
                        //loadedManufacturer = true;
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
                        //loadedCategory1 = true;
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
                        //loadedCategory2 = true;
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
                        //loadedCategory3 = true;
                    }
                }
                else {
                    //console.log('LoadStructure call failed!');
                }
            },
            error: function () {
                //console.log('LoadStructure call failed!');
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
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (result) {
                if (result.Products.ListCount == 0) {
                    $('list_item_container').show();
                    $.fancybox('<div align="center" style="width:250px; padding:10px;"><H2>Nincs megjelenitendő elem!</H2><p>Próbálja keresési jellegek használatával szűrni a találatokat bizonyos feltételek alapján.<p></div>', {
                        'autoDimensions': true,

                        'transitionIn': 'elastic',
                        'transitionOut': 'elastic',
                        'closeBtn': 'true',
                        'changeFade': 0,
                        'speedIn': 300,
                        'speedOut': 300,
                        'width': '150%',
                        'height': '150%',
                        'autoScale': true
                    });
                } else {
                    $('list_item_container').hide();
                }
                if (result) {
                    //console.log(result);
                    $("#div_pager_top").empty();
                    $("#pagerTemplateTop").tmpl(result).appendTo("#div_pager_top");
                    $("#div_pager_bottom").empty();
                    $("#pagerTemplateBottom").tmpl(result.Products).appendTo("#div_pager_bottom");
                    $("#div_catalogue").empty();
                    $("#productTemplate").tmpl(result).appendTo("#div_catalogue");
                    //$('.number').spin();
                    $('.select2').val(result.Sequence);
                    //$('.select2').skinner({ 'type': 'left' });//
                }
                else {
                    //console.log('loadCatalogueList result failed');
                }
            },
            error: function () {
                //console.log('loadCatalogueList call failed');
            }
        });
    };
    //részletes termékadatlap log lista
    var loadCatalogueDetailsLogList = function () {
                $.ajax({
                    type: "POST",
                    url: companyGroup.utils.instance().getWebshopApiUrl('GetCatalogueDetailsLogList'),
                    data: {},
                    contentType: "application/json; charset=utf-8",
                    timeout: 15000,
                    dataType: "json",
                    processData: true,
                    success: function (result) {
                        $("#container_2").empty();
                        $("#catalogueDetailsLogTemplate").tmpl(result).appendTo("#container_2");
                    },
                    error: function () {
                        
                    }
                });

//        dataservice.webshop.loadCatalogueDetailsLogList(
//        {
//            success: function (response) {
//                $("#container_2").empty();
//                $("#catalogueDetailsLogTemplate").tmpl(result).appendTo("#container_2");
//            },
//            error: function (response) {

//                return;
//            }
//        });
    };

    //termékadatlap
    var loadDetails = function (productId, dataAreaId) {
        var data = {
            ProductId: productId,
            DataAreaId: dataAreaId
        };
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getWebshopApiUrl('GetDetails'),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            timeout: 15000,
            dataType: "json",
            processData: true,
            success: function (result) {
                $("#cus_productdetails_table").empty();
                $("#productDetailsTemplate").tmpl(result).appendTo("#cus_productdetails_table");
                switch_tabs($('.defaulttab'));
                showProductList(false);
                // console.log($('#top').offset().top);
                // window.location.hash = '#top';

                $("html, body").animate({ scrollTop: 0 }, 0);


                //$(document.body).scrollTop($('#top').offset().top);
            },
            error: function () {
                //console.log('Service call failed: GetDetails');
            }
        });
    };
    var loadShoppingCart = function () {
        $.ajax({
            type: "POST",
            url: companyGroup.utils.instance().getShoppingCartApiUrl('GetActiveCart'),
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
					if (result.ActiveCart.Items.length > 0) {
                        $("#cus_checkout_menu").show();						
                    }
                    else {
                        $("#cus_checkout_menu").hide();
						$("#cus_ajanlat_finance").hide();	
						$("#cus_rendeles_feladas").hide();						
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
    };
    //engedélyezett-e a kiszállítási dátum beállítás
    var allowDeliveryDateSettings = function (value) {
        //console.log('allowDeliveryDateSettings: ' + value);
        if (value) {
            $("#div_deliverydate_notallowed").hide();
            $("#naptar").show();
        }
        else {
            $("#div_deliverydate_notallowed").show();
            $("#naptar").hide();
        }
    };
    //terméklista megmutatása, elrejtése
    var showProductList = function (value) {
        if (value) {
            $("#cus_list_order_panel").show();
            $("#cus_product_table").show();
            $("#cus_list_order_panel2").show();
            $("#cus_productdetails_table").hide();
        }
        else {
            $("#cus_list_order_panel").hide();
            $("#cus_product_table").hide();
            $("#cus_list_order_panel2").hide();
            $("#cus_productdetails_table").show();
        }
    };
    var switch_tabs = function (obj) {
        $('.tab-content').hide();
        $('.tabs a').removeClass("selected");
        var id = obj.attr("rel");
        $('#' + id).show();
        obj.addClass("selected");
    };
    //kérés paramétereit összefogó objektum
    var catalogueRequest = {
        ManufacturerIdList: [],
        Category1IdList: [],
        Category2IdList: [],
        Category3IdList: [],
        DiscountFilter: false,
        SecondhandFilter: false,
        NewFilter: false,
        StockFilter: false,
        TextFilter: '',
        HrpFilter: true,
        BscFilter: true,
        IsInNewsletterFilter: true,
        PriceFilter: '0',
        PriceFilterRelation: '0',
        Sequence: 0,
        CurrentPageIndex: 1,
        ItemsOnPage: 30,
        clear: function () {
            this.ManufacturerIdList = [];
            this.Category1IdList = [];
            this.Category2IdList = [];
            this.Category3IdList = [];
            this.DiscountFilter = false;
            this.SecondhandFilter = false;
            this.NewFilter = false;
            this.StockFilter = false;
            this.TextFilter = '';
            this.HrpFilter = true;
            this.BscFilter = true;
            this.IsInNewsletterFilter = false;
            this.PriceFilter = '0';
            this.PriceFilterRelation = '0';
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
                            //console.log(result);
                        }
                    },
                    error: function () {
                        //console.log('CompletionListServiceUrl failed');
                    }
                });
            },
            minLength: 2
        }).data("autocomplete")._renderItem = function (ul, item) {
            //console.log(item);
            var inner_html = '<div class="list_item_container"><div class="image"><img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(item.PictureId) + ' alt=\"\" /></div><div class="label">' + item.ProductId + '</div><div class="description">' + item.ProductName + '</div></div>';
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
                            //console.log(result);
                        }
                    },
                    error: function () {
                        //console.log('CompletionListServiceUrl failed');
                    }
                });
            },
            minLength: 2
        })
        .data("autocomplete")._renderItem = function (ul, item) {
            //console.log(item);
            var inner_html = '<div class="list_item_container"><div class="image"><a style="color:#FFF; text-decoration:none; font-size:12px;" href="#/details/' + item.ProductId + '" title="' + item.ProductName + '"><img src="' + companyGroup.utils.instance().getThumbnailPictureUrl(item.ProductId) + '" alt="" /></div><div class="label">' + item.ProductId + '</div><div class="description">' + item.ProductName + '</div></div>';
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
        //console.log('downloadPriceList');
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


