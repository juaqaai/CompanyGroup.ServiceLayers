
select sl.* from AxDb.dbo.SALESLINE as sl
INNER JOIN AxDb.dbo.InventTrans as it ON sl.DataAreaID = it.DataAreaID and sl.InventTransId = it.InventTransId
where sl.CustAccount = 'V000787' and 
	  sl.SalesStatus = 1	-- nyitottak

	  ;


    Select InventTrans where InventTrans.InventTransId == this.InventTransId && inventtrans.StatusIssue != statusissue::Sold;	
	-- kiadas allapota: 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)

    IF (!InventTrans) return DispoStatus::Diszponalt;

    Select InventTrans where InventTrans.InventTransId == this.InventTransId && inventtrans.StatusReceipt != statusReceipt::Purchased; 	-- bevételezés állapota: 0 none, 1 purchased (beszerezve), 2 Received (fogadott), 3 Registered (regisztrálva), 4 Arrived (beérkezett), 5 Ordered (rendelt), 6 QuotationReceipt (árajánlat - bevételezés)
    
	IF (!InventTrans)
        return DispoStatus::Diszponalt;
    //endBB

    //BB 20120903 Nyiteng
    if (this.ItemId == "NYITENG")
        return DispoStatus::Diszponalt;	-- 1 diszponált, 2 diszponálható, 3 beérkezés, 4 áttárolás, 5 beszállítás, 6 nem diszponálható,
    //endBB

    IF (this.SalesQty<=0)
        return DispoStatus::None;

    IF (this.LineAmount <=0)
        return DispoStatus::None;

    Select Sum(qty) from  InventTrans where
        InventTrans.InventTransId == this.InventTransId;
    IF (!InventTrans.Qty)
        return DispoStatus::None;

    -- készletrendelés táblában az összes mennyiség abszolút értékben egyenlõ az inventTrans mennyiséggel, 
    --  akkor diszponált
    Select sum(qty) From WmsOrder Index hint InventTransIdx Where
        wmsorder.inventTransId == this.InventTransId;
    IF (InventTrans.Qty == -1* wmsorder.qty)
        return DispoStatus::Diszponalt;

    -- ha szolgáltatás típusú a termék, akkor diszponálható
    Select Itemtype from InventTable where
        InventTable.ItemId == this.ItemId;
    IF (Inventtable.ItemType == ItemType::Service)	-- ItemType: 0 item (cikk), 1 BOM anyagjegyzék, 2 Service (szolgáltatás)
        Return  DispoStatus::Diszponalhato;

   -- 
   Select sum(qty) from InventTrans where
     InventTrans.InventTransId == this.InventTransId &&
     InventTrans.StatusIssue > StatusIssue::ReservPhysical;(foglalt tényleges)-nél nagyobb a foglalt rendelt, vagy 
a rendelés alatti státusz és nincs belõle, akkor diszponálható
   IF (!InventTrans.qty)
       return DispoStatus::Diszponalhato;

   Select sum(qty) from InventTrans where
     InventTrans.InventTransId == this.InventTransId &&
     InventTrans.StatusIssue > StatusIssue::ReservOrdered;	-- (foglalt rendelt)-nél nagyobb a rendelés alatt
   IF (!InventTrans.qty)					-- ha nincs belõle
       return DispoStatus::Attarolas;

Return DispoStatus::Beszallitas;