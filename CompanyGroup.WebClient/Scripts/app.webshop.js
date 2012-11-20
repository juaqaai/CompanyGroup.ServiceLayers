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
        var lastPageIndex = $("#spanTopLastPageIndex").text();
        if (currentPageIndex < (lastPageIndex)) {
            catalogueRequest.CurrentPageIndex = lastPageIndex;
            loadCatalogue();
            context.title('Webshop - Utolsó oldal');
        }
    });
    //ugrás a következő oldalra
    this.get('#/nextPage', function (context) {
        //console.log(context);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        var lastPageIndex = $("#spanTopLastPageIndex").text();
        if (currentPageIndex < (lastPageIndex)) {
            currentPageIndex = currentPageIndex + 1;
            catalogueRequest.CurrentPageIndex = currentPageIndex;
            loadCatalogue();
            context.title('Webshop - Következő oldal');
        }
    });
    //ugrás az előző oldalra
    this.get('#/previousPage', function (context) {
        //console.log(context);
        var currentPageIndex = catalogueRequest.CurrentPageIndex;
        if (currentPageIndex > 1) {
            currentPageIndex = currentPageIndex - 1;
            catalogueRequest.CurrentPageIndex = currentPageIndex;
            loadCatalogue();
            context.title('Webshop - Előző oldal');
        }
    });
    //megjelenített elemek száma változás
    this.get('#/visibleItemListChanged', function (context) {
        //console.log(context);
        catalogueRequest.CurrentPageIndex = 1;
        if (value === 'top') {
            catalogueRequest.ItemsOnPage = parseInt($("#select_visibleitemlist_top").val(), 0);
        }
        else {
            catalogueRequest.ItemsOnPage = parseInt($("#select_visibleitemlist_bottom").val(), 0);
        }
        loadCatalogue();
        context.title('Webshop - megjelenített elemek száma változás');
    });
    //kiválasztott sorszámú lapra ugrás
    this.get('#/selectedPageIndexChanged', function (context) {
        //console.log(context);
        if (value === 'top') {
            catalogueRequest.CurrentPageIndex = parseInt($("#select_pageindex_top").val(), 0);
        }
        else {
            catalogueRequest.CurrentPageIndex = parseInt($("#select_pageindex_bottom").val(), 0);
        }
        loadCatalogue();
        context.title('Webshop - kiválasztott sorszámú lapra ugrás');
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
        catalogueRequest.StockFilter = value;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés készleten lévő termékekre');
    });
    //szűrés akciós termékekre
    this.get('#/filterByAction/:value', function (context) {
        catalogueRequest.ActionFilter = value;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés akciós termékekre');
    });
    //szűrés használt termékekre
    this.get('#/filterByBargain/:value', function (context) {
        catalogueRequest.BargainFilter = value;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - szűrés használt termékekre');
    });
    //szűrés új termékekre
    this.get('#/filterByNew/:value', function (context) {
        catalogueRequest.NewFilter = value;
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
    this.get('#/filterByCategoryHrp/:value', function (context) {
        catalogueRequest.clear();
        catalogueRequest.Category1IdList.push(value);
        loadCatalogue();
        loadStructure(true, true, true, true);
        $('#category1List').val(value);
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
    this.get('#/filterByCategoryBsc/:value', function (context) {
        catalogueRequest.clear();
        catalogueRequest.Category1IdList.push(value);
        loadCatalogue();
        loadStructure(true, true, true, true);
        $('#category1List').val(value);
        $("#category1List").trigger("liszt:updated");
        context.title('Webshop - szoftver termékek');
    });
    //szűrőfeltételek törlése
    this.get('#/clearFilters', function (context) {
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
        catalogueRequest.TextFilter = textfilter;
        catalogueRequest.CurrentPageIndex = 1;
        loadCatalogue();
        loadStructure(true, true, true, true);
        context.title('Webshop - keresés');
    });
    //nagyobb méretű termékkép
    this.get('#/showPicture/:productId/:dataAreaId/:productName', function (context) {
        var arr_pics = new Array();
        var data = {
            ProductId: productId,
            DataAreaId: dataAreaId
        };
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
                        item.href = companyGroup.utils.instance().getBigPictureUrl(productId, pic.RecId, dataAreaId);
                        item.title = productId;
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
                                    return '<a href="' + companyGroup.utils.instance().getProductDetailsUrl(productId) + '"><span id="fancybox-title-over"> ' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? '&nbsp; ' + title + '&nbsp;&nbsp;' + productName + '&nbsp;' : '') + '</span></a>';
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
            type: "GET",
            url: companyGroup.utils.instance().getWebshopApiUrl('GetCatalogue'),
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
        clear: function() {
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


