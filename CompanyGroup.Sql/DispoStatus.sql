
with SalesLineCTE (SalesId, CreatedDate, LineNum, ShippingDateRequested, ItemId, Name, SalesPrice, CurrencyCode, Quantity, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId)
	 AS  ( select sl.SalesId, 
				  sl.CreatedDate, 
				  CONVERT( INT, sl.LineNum )as LineNum,
				  sl.ShippingDateRequested as ShippingDateRequested,
				  sl.ItemId,
				  sl.Name,
				  CONVERT( INT, sl.SalesPrice ) as SalesPrice,
				  sl.CurrencyCode, 
				  CONVERT( INT, ABS( it.Qty ) ) as Quantity,
				  CONVERT( INT, sl.LineAmount ) as LineAmount, 
				  CONVERT( INT, sl.SalesDeliverNow ) as SalesDeliverNow, 
				  CONVERT( INT, sl.RemainSalesPhysical ) as RemainSalesPhysical, 
				  it.StatusIssue as StatusIssue, 
				  id.InventLocationId 
				  from AxDb.dbo.SALESLINE as sl
				  INNER JOIN AxDb.dbo.InventTrans as it ON sl.DataAreaID = it.DataAreaID and sl.InventTransId = it.InventTransId
				  INNER JOIN AxDb.dbo.InventDim AS id ON id.InventDimId = sl.InventDimId AND id.DataAreaId = sl.DataAreaId
				  where sl.CustAccount = 'V000787' and 
						sl.SalesStatus = 1 and	-- 1: Nyitott rendel�s (backorder), 2: Sz�ll�tva (delivered), 3: Sz�ml�zva (Invoiced), 4: �rv�nytelen�tve (Canceled)
						id.InventLocationId IN ( 'BELSO', 'KULSO', '1000', '7000', 'HASZNALT', '2100' ) and 
						it.StatusIssue IN (4, 5, 6)	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kiv�ve), 4 ReservPhysical (foglalt t�nyleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendel�s alatt), 7 Quotation issue (�raj�nlat kiad�sa)
				  ), 
	SalesHeaderCTE (SalesId, Payment) AS (select st.SalesId, Pt.[Description] from AxDb.dbo.SalesTable as st 
		 inner join AxDb.dbo.PAYMTERM AS pt ON st.Payment = pt.PaymTermId AND Pt.DATAAREAID = 'mst'
		 where st.CustAccount = 'V000787'
		 )

	select cte1.*, cte2.* 
	from SalesLineCTE as cte1 
	     inner join SalesHeaderCTE as cte2 on cte1.SalesId = cte2.SalesId
	order by cte1.CreatedDate desc, cte1.SalesId asc, cte1.LineNum asc;

    Select InventTrans where InventTrans.InventTransId == this.InventTransId && inventtrans.StatusIssue != statusissue::Sold;	
	-- kiadas allapota: 0 none, 1 sold, 2 deducted (eladva), 3 picked (kiv�ve), 4 ReservPhysical (foglalt t�nyleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendel�s alatt), 7 Quotation issue (�raj�nlat kiad�sa)

    IF (!InventTrans) return DispoStatus::Diszponalt;

    Select InventTrans where InventTrans.InventTransId == this.InventTransId && inventtrans.StatusReceipt != statusReceipt::Purchased; 	-- bev�telez�s �llapota: 0 none, 1 purchased (beszerezve), 2 Received (fogadott), 3 Registered (regisztr�lva), 4 Arrived (be�rkezett), 5 Ordered (rendelt), 6 QuotationReceipt (�raj�nlat - bev�telez�s)
    
	IF (!InventTrans)
        return DispoStatus::Diszponalt;
    //endBB

    //BB 20120903 Nyiteng
    if (this.ItemId == "NYITENG")
        return DispoStatus::Diszponalt;	-- 1 diszpon�lt, 2 diszpon�lhat�, 3 be�rkez�s, 4 �tt�rol�s, 5 besz�ll�t�s, 6 nem diszpon�lhat�,
    //endBB

    IF (this.SalesQty<=0)
        return DispoStatus::None;

    IF (this.LineAmount <=0)
        return DispoStatus::None;

    Select Sum(qty) from  InventTrans where
        InventTrans.InventTransId == this.InventTransId;
    IF (!InventTrans.Qty)
        return DispoStatus::None;

    -- k�szletrendel�s t�bl�ban az �sszes mennyis�g abszol�t �rt�kben egyenl� az inventTrans mennyis�ggel, 
    --  akkor diszpon�lt
    Select sum(qty) From WmsOrder Index hint InventTransIdx Where
        wmsorder.inventTransId == this.InventTransId;
    IF (InventTrans.Qty == -1* wmsorder.qty)
        return DispoStatus::Diszponalt;

    -- ha szolg�ltat�s t�pus� a term�k, akkor diszpon�lhat�
    Select Itemtype from InventTable where
        InventTable.ItemId == this.ItemId;
    IF (Inventtable.ItemType == ItemType::Service)	-- ItemType: 0 item (cikk), 1 BOM anyagjegyz�k, 2 Service (szolg�ltat�s)
        Return  DispoStatus::Diszponalhato;

   -- 
   Select sum(qty) from InventTrans where
     InventTrans.InventTransId == this.InventTransId &&
     InventTrans.StatusIssue > StatusIssue::ReservPhysical;(foglalt t�nyleges)-n�l nagyobb a foglalt rendelt, vagy 
a rendel�s alatti st�tusz �s nincs bel�le, akkor diszpon�lhat�
   IF (!InventTrans.qty)
       return DispoStatus::Diszponalhato;

   Select sum(qty) from InventTrans where
     InventTrans.InventTransId == this.InventTransId &&
     InventTrans.StatusIssue > StatusIssue::ReservOrdered;	-- (foglalt rendelt)-n�l nagyobb a rendel�s alatt
   IF (!InventTrans.qty)					-- ha nincs bel�le
       return DispoStatus::Attarolas;

Return DispoStatus::Beszallitas;

select st.*, Pt.[Description] from AxDb.dbo.SalesTable as st 
		 inner join AxDb.dbo.PAYMTERM AS pt ON st.Payment = pt.PaymTermId AND Pt.DATAAREAID = 'mst'
		 where st.CustAccount = 'V000787'