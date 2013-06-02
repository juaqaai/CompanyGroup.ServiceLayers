/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban SalesOrderExtract lekérdezés
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
@SalesSource INT = -1,  -- -1: mindegyik, 0: bolt, 1:internet, 2: xml, 3 : edi 
@SalesStatus INT = 1,	-- 1: Nyitott rendelés (backorder), 2: Szállítva (delivered), 3: Számlázva (Invoiced), 4: Érvénytelenítve (Canceled)
@SalesType INT = 3,		-- 0 : naplo (Journal), 1 : Árajánlat (Quotation), 2 : Elõfizetés (Subscription)
						-- 3 : Értékesítési rendelés (Sales), 4 : Visszáru (ReturnItem), 5 : Keretrendelés (TradeBlanketOrder), 
						-- 6 : Cikkszükséglet (ItemReq), 7 : Nem definiált információ (Undefined)
@OnOrder BIT = 1, 
@Reserved BIT = 1,
@ReservedOrdered BIT = 1
*/
DROP PROCEDURE InternetUser.SalesOrderExtract2
GO
CREATE PROCEDURE InternetUser.SalesOrderExtract2

AS
SET NOCOUNT ON

	SELECT H.SALESID as SalesId, 
		   H.CreatedDate as CreatedDate, 
		   -- H.INVENTLOCATIONID as InventLocationID,
		   Pt.[Description] as Payment, 
		   H.SalesHeaderType as SalesHeaderType, 
		   H.SalesSource as SalesSource,	-- rendeles forrasa
		   H.SalesStatus as HeaderSalesStatus,	--rendelés állapota a szállításra és a számlázásra vonatkozóan
		   H.ShippingDateRequested as ShippingDateRequested, 
		   C.Txt as Currency,
		   CONVERT( INT, D.LineNum )as LineNum, --CONVERT( INT, D.LineNum )
		   D.SalesStatus as SalesStatus, -- rendelés tétel állapota a szállításra és a számlázásra vonatkozóan
		   D.ItemID as ProductId, 
		   D.Name as ProductName, 
		   SUM( CONVERT( INT, ABS( it.Qty ) ) ) as Quantity, 
		   CONVERT( INT, D.SalesPrice ) as SalesPrice, 
		   CONVERT( INT, D.LineAmount ) as LineAmount, 
		   CONVERT( INT, D.SalesDeliverNow ) as SalesDeliverNow, 
		   CONVERT( INT, D.RemainSalesPhysical ) as RemainSalesPhysical, 
		   it.StatusIssue as StatusIssue, 
		   --MAX( it.RecID ) as RecID,
		   InventDim.InventLocationID as ItemInventLocationID  
	FROM Axdb.dbo.SALESTABLE AS H 
		 INNER JOIN Axdb.dbo.SALESLINE AS D ON H.SALESID = D.SALESID AND H.DataAreaID = D.DataAreaID
		 INNER JOIN Axdb.dbo.InventTrans as it ON D.DataAreaID = it.DataAreaID and D.InventTransId = it.InventTransId -- it.TRANSREFID = D.SALESID AND D.ItemID = it.ItemID AND 
		 INNER JOIN Axdb.dbo.InventDim AS InventDim ON InventDim.InventDimID = D.InventDimID AND InventDim.DataAreaID = H.DataAreaID
		 INNER JOIN Axdb.dbo.PAYMTERM AS Pt ON H.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
		 INNER JOIN Axdb.dbo.Currency as C ON C.CURRENCYCODE = H.CURRENCYCODE AND C.DATAAREAID = H.DATAAREAID
	WHERE H.DataAreaId IN ('bsc', 'hrp') AND 
		  --H.SalesStatus = 1 AND
		  --D.SalesStatus = 1 AND
		  H.SalesType = 3  
		  AND InventDim.InventLocationId IN ( 'BELSO', 'KULSO', '1000', '7000', 'HASZNALT', '2100' ) 
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
GRANT EXECUTE ON [InternetUser].SalesOrderExtract2 TO InternetUser;

-- exec [InternetUser].[SalesOrderExtract2] 'V001446', 'hrp';

--select SalesHeaderType from Axdb.dbo.updSalesType where dataAreaID = 'hrp' group by SalesHeaderType;
-- select * from Axdb.dbo.PaymTerm
-- select * from Axdb.dbo.Currency
-- select top 10 * from Axdb.dbo.SALESTABLE
/*
Operation	Version	CustomerId	DataAreaId	SalesId		CreatedDate				ShippingDateRequested	CurrencyCode	Payment				SalesHeaderType	SalesHeaderStatus	CustomerOrderNo	DlvTerm	LineNum	SalesStatus	ProductId	ProductName															SalesPrice	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	ItemDate				FileName
U	683862			V000807		hrp			VR648523	2013-04-02 00:00:00.000	2013-04-02 00:00:00.000	HUF				Átutalás kövhó+120	Standard		1					190079888		SILVERF	16		1			CE847A		hp laserjet Pro M1132 mfp lézernyomtató / másoló / színes szkenner	21017		7			147119		7				7					4			KULSO				2013-04-03 00:00:00.000	CE847A.jpg
*/
GO
DROP PROCEDURE InternetUser.SalesOrderExtract
GO
CREATE PROCEDURE InternetUser.SalesOrderExtract
AS
SET NOCOUNT ON;

	-- képek duplikációk nélküli kikeresése
	;
	WITH Picture_CTE (ItemId, [FileName]) AS
	(
		SELECT ItemId, [FileName] 
		FROM Axdb.dbo.UPDKEPEK
		GROUP BY ItemId, [FileName], ELSODLEGESKEP
		HAVING ELSODLEGESKEP = 1 AND COUNT(*) = 1 AND ItemId <> ''
	),
	SalesLineCTE (SalesId, LineNum, SalesStatus, ProductId, ProductName, SalesPrice, Quantity, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, FileName)
	AS  ( select sl.SalesId, 
   			     CONVERT( INT, sl.LineNum )as LineNum,
				 sl.SalesStatus, 
				 sl.ItemId,
				 sl.Name,
				 CONVERT( INT, sl.SalesPrice ) as SalesPrice,
				 CONVERT( INT, ABS( it.Qty ) ) as Quantity,
				 CONVERT( INT, sl.LineAmount ) as LineAmount, 
				 CONVERT( INT, sl.SalesDeliverNow ) as SalesDeliverNow, 
				 CONVERT( INT, sl.RemainSalesPhysical ) as RemainSalesPhysical, 
				 it.StatusIssue as StatusIssue, 
				 id.InventLocationId, 
				 sl.CreatedDate, 
				 ISNULL(picture.[FileName], '') as [FileName]
				 from Axdb.dbo.SALESLINE as sl
				 INNER JOIN Axdb.dbo.InventTrans as it ON sl.DataAreaID = it.DataAreaID and sl.InventTransId = it.InventTransId
				 INNER JOIN Axdb.dbo.InventDim AS id ON id.InventDimId = sl.InventDimId AND id.DataAreaId = sl.DataAreaId
				 LEFT OUTER JOIN Picture_CTE as Picture ON Picture.ItemId = sl.ItemId
				 where sl.DataAreaId IN ('bsc', 'hrp') and 
						--sl.SalesStatus = 1 and	-- 1: Nyitott rendelés (backorder), 2: Szállítva (delivered), 3: Számlázva (Invoiced), 4: Érvénytelenítve (Canceled)
						id.InventLocationId IN ( 'BELSO', 'KULSO', '1000', '7000', 'HASZNALT', '2100' ) and 
						it.StatusIssue IN (2, 3, 4, 5, 6)	-- 0 none, 1 sold (eladva), 2 deducted (levonva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)
	), 
	SalesHeaderCTE (CustomerId, DataAreaId, SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment, SalesHeaderType, SalesHeaderStatus, CusomerOrderNo, DlvTerm) AS (
		select st.CustAccount, st.DataAreaId, st.SalesId, st.CreatedDate, st.ShippingDateRequested, st.CurrencyCode, Pt.[Description], st.SalesHeaderType, st.SalesStatus, st.VEVORENDELESSZAMA, st.DLVTERM
		from Axdb.dbo.SalesTable as st 
		inner join Axdb.dbo.PAYMTERM AS pt ON st.Payment = pt.PaymTermId AND Pt.DATAAREAID = 'mst'
		where st.SalesType = 3 and 
			  st.DataAreaId IN ('bsc', 'hrp')
	)

	select SalesHeaderCTE.*, SalesLineCTE.LineNum, SalesLineCTE.SalesStatus, SalesLineCTE.ProductId, SalesLineCTE.ProductName, SalesLineCTE.SalesPrice, SUM(SalesLineCTE.Quantity) as Quantity, SalesLineCTE.LineAmount, 
							 SalesLineCTE.SalesDeliverNow, SalesLineCTE.RemainSalesPhysical, MAX(SalesLineCTE.StatusIssue) as StatusIssue, SalesLineCTE.InventLocationId, SalesLineCTE.ItemDate, SalesLineCTE.[FileName]
	from SalesLineCTE 
	     inner join SalesHeaderCTE on SalesLineCTE.SalesId = SalesHeaderCTE.SalesId
		 -- where SalesHeaderCTE.CustomerId = 'V005024'

	group by CustomerId, DataAreaId, SalesHeaderCTE.SalesId, SalesLineCTE.SalesId, CreatedDate, ShippingDateRequested, CurrencyCode,	Payment,	SalesHeaderType,	SalesHeaderStatus,	CusomerOrderNo,	DlvTerm,	
			 LineNum,	SalesStatus,	ProductId,	ProductName,	SalesPrice,	LineAmount,	SalesDeliverNow,	RemainSalesPhysical,	InventLocationId,	ItemDate,	[FileName]

	order by SalesHeaderCTE.CreatedDate, SalesLineCTE.SalesId, SalesLineCTE.LineNum;

RETURN 
GO
GRANT EXECUTE ON [InternetUser].SalesOrderExtract TO InternetUser;

GO
GRANT EXECUTE ON [InternetUser].SalesOrderExtract TO [HRP_HEADOFFICE\AXPROXY]
GO
-- exec InternetUser.SalesOrderExtract 