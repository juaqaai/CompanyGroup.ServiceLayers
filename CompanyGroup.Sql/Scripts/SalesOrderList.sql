USE ExtractInterface
GO
/****** Object:  StoredProcedure [InternetUser].[web_GetSales]    Script Date: 2012.09.18. 20:55:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.GetSalesOrders
GO
CREATE PROCEDURE InternetUser.GetSalesOrders( @CustomerID NVARCHAR(10),
											  @DataAreaID NVARCHAR(3) = 'hrp',
											  @SalesSource INT = -1, -- -1: mindegyik, 0: bolt, 1:internet, 2: xml, 3 : edi 
											  @SalesStatus INT = 1, -- 1: Nyitott rendel�s (backorder), 2: Sz�ll�tva (delivered), 3: Sz�ml�zva (Invoiced), 4: �rv�nytelen�tve (Canceled)
											  @SalesType INT = 3, -- 0 : naplo (Journal), 1 : �raj�nlat (Quotation), 2 : El�fizet�s (Subscription)
																	 -- 3 : �rt�kes�t�si rendel�s (Sales), 4 : Vissz�ru (ReturnItem), 5 : Keretrendel�s (TradeBlanketOrder), 
																     -- 6 : Cikksz�ks�glet (ItemReq), 7 : Nem defini�lt inform�ci� (Undefined)
											  @OnOrder BIT = 1, 
											  @Reserved BIT = 1,
											  @ReservedOrdered BIT = 1 )
AS
SET NOCOUNT ON

	SELECT H.SALESID as SalesID, 
		   H.CreatedDate as CreatedDate, 
		   -- H.INVENTLOCATIONID as InventLocationID,
		   Pt.[Description] as Payment, 
		   H.SalesHeaderType as SalesHeaderType, 
		   H.SalesSource as SalesSource,	-- rendeles forrasa
		   H.SalesStatus as HeaderSalesStatus,	--rendel�s �llapota a sz�ll�t�sra �s a sz�ml�z�sra vonatkoz�an
		   H.ShippingDateRequested as ShippingDateRequested, 
		   C.Txt as Currency,
		   CONVERT( INT, D.LineNum )as LineNum, --CONVERT( INT, D.LineNum )
		   D.SalesStatus as SalesStatus, -- rendel�s t�tel �llapota a sz�ll�t�sra �s a sz�ml�z�sra vonatkoz�an
		   D.ItemID as ProductID, 
		   D.Name as ProductName, 
		   SUM( CONVERT( INT, ABS( it.Qty ) ) ) as Quantity, 
		   CONVERT( INT, D.SalesPrice ) as SalesPrice, 
		   CONVERT( INT, D.LineAmount ) as LineAmount, 
		   CONVERT( INT, D.SalesDeliverNow ) as SalesDeliverNow, 
		   CONVERT( INT, D.RemainSalesPhysical ) as RemainSalesPhysical, 
		   it.StatusIssue as StatusIssue, 
		   MAX( it.RecID ) as RecID,
		   InventDim.InventLocationID as ItemInventLocationID  
	FROM axdb_20120614.dbo.SALESTABLE AS H 
		 INNER JOIN axdb_20120614.dbo.SALESLINE AS D ON H.SALESID = D.SALESID AND H.DataAreaID = D.DataAreaID
		 INNER JOIN axdb_20120614.dbo.InventTrans as it ON D.DataAreaID = it.DataAreaID and D.InventTransId = it.InventTransId -- it.TRANSREFID = D.SALESID AND D.ItemID = it.ItemID AND 
		 INNER JOIN axdb_20120614.dbo.InventDim AS InventDim ON InventDim.InventDimID = D.InventDimID AND InventDim.DataAreaID = H.DataAreaID
		 INNER JOIN axdb_20120614.dbo.PAYMTERM AS Pt ON H.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
		 INNER JOIN axdb_20120614.dbo.Currency as C ON C.CURRENCYCODE = H.CURRENCYCODE AND C.DATAAREAID = H.DATAAREAID
	WHERE H.CustAccount = @CustomerID AND 
		  H.DataAreaID = @DataAreaID
		  AND
		  H.SalesSource = CASE WHEN ( @SalesSource = -1 ) THEN H.SalesSource ELSE @SalesSource END AND 
		  H.SalesStatus = CASE WHEN ( @SalesStatus = 0 ) THEN H.SalesStatus ELSE @SalesStatus END AND
		  D.SalesStatus = CASE WHEN ( @SalesStatus = 0 ) THEN D.SalesStatus ELSE @SalesStatus END AND
		  H.SalesType = @SalesType  
--		  AND ( ( InventDim.ConfigID = 'ALAP' ) OR ( InventDim.ConfigId like 'xx%' ) OR ( InventDim.ConfigID = 'Spec �r' )  ) 
		  AND InventDim.InventLocationID IN ( 'BELSO', 'KULSO', '1000', '7000', 'HASZNALT', '2100' ) 
		  AND
		  ( it.StatusIssue <> CASE WHEN ( @OnOrder = 0 ) THEN 6 ELSE 99 END AND
		    it.StatusIssue <> CASE WHEN ( @Reserved = 0 ) THEN 4 ELSE 99 END AND
		    it.StatusIssue <> CASE WHEN ( @ReservedOrdered = 0 ) THEN 5 ELSE 99 END )
	GROUP BY H.SALESID, H.CreatedDate, H.INVENTLOCATIONID, 
		   Pt.[Description], C.Txt,
		   H.SalesHeaderType, H.SalesSource, H.SalesStatus, H.ShippingDateRequested, 
		   CONVERT( INT, D.LineNum ), D.SalesStatus, D.ItemID, D.Name, 
		   CONVERT( INT, D.SalesPrice ), CONVERT( INT, D.LineAmount ), 
		   CONVERT( INT, D.SalesDeliverNow ), CONVERT( INT, D.RemainSalesPhysical ), 
		   it.StatusIssue, InventDim.InventLocationID
	ORDER BY SalesID, CONVERT( INT, D.LineNum ), MAX(it.RecID) 

RETURN
GO
GRANT EXECUTE ON [InternetUser].[GetSalesOrders] TO InternetUser;

-- exec [InternetUser].[GetSalesOrders] 'V000787', 'hrp';

--select SalesHeaderType from Axdb.dbo.updSalesType where dataAreaID = 'hrp' group by SalesHeaderType;
-- select * from Axdb.dbo.PaymTerm
-- select * from Axdb.dbo.Currency

GO
DROP FUNCTION InternetUser.ConvertInventLocationId
GO
CREATE FUNCTION InternetUser.ConvertInventLocationId( @InventLocationId NVARCHAR(10) )
RETURNS NVARCHAR(10)	
AS
BEGIN
	DECLARE @Result NVARCHAR(10);

	IF (@InventLocationId = 'BELSO' OR @InventLocationId = '1000') 
		SET @Result = 'V�s�';
	ELSE
		IF ( @InventLocationId = 'HASZNALT' OR @InventLocationId = '2100' )
			SET @Result = @InventLocationId;
		ELSE
			SET @Result = 'Huszti';

	RETURN @Result;
END

-- SELECT InternetUser.ConvertInventLocationId('7000');
GO
DROP PROCEDURE InternetUser.SalesOrderList
GO
CREATE PROCEDURE InternetUser.SalesOrderList( @CustomerId NVARCHAR(10),
											  @DataAreaId NVARCHAR(3) = 'hrp' )
AS
SET NOCOUNT ON;
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
				  from axdb_20120614.dbo.SALESLINE as sl
				  INNER JOIN axdb_20120614.dbo.InventTrans as it ON sl.DataAreaID = it.DataAreaID and sl.InventTransId = it.InventTransId
				  INNER JOIN axdb_20120614.dbo.InventDim AS id ON id.InventDimId = sl.InventDimId AND id.DataAreaId = sl.DataAreaId
				  where sl.CustAccount = @CustomerId and 
						sl.SalesStatus = 1 and	-- 1: Nyitott rendel�s (backorder), 2: Sz�ll�tva (delivered), 3: Sz�ml�zva (Invoiced), 4: �rv�nytelen�tve (Canceled)
						id.InventLocationId IN ( 'BELSO', 'KULSO', '1000', '7000', 'HASZNALT', '2100' ) and 
						it.StatusIssue IN (4, 5, 6)	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kiv�ve), 4 ReservPhysical (foglalt t�nyleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendel�s alatt), 7 Quotation issue (�raj�nlat kiad�sa)
				  ), 
	SalesHeaderCTE (SalesId, Payment) AS (select st.SalesId, Pt.[Description] from AxDb.dbo.SalesTable as st 
		 inner join AxDb.dbo.PAYMTERM AS pt ON st.Payment = pt.PaymTermId AND Pt.DATAAREAID = 'mst'
		 where st.CustAccount = @CustomerId
		 )

	select cte1.*, cte2.Payment 
	from SalesLineCTE as cte1 
	     inner join SalesHeaderCTE as cte2 on cte1.SalesId = cte2.SalesId
	order by cte1.CreatedDate desc, cte1.SalesId asc, cte1.LineNum asc;

RETURN 
GO
GRANT EXECUTE ON [InternetUser].[SalesOrderList] TO InternetUser;

-- exec InternetUser.SalesOrderList 'V001446'; --'V001686'	--'V000787'