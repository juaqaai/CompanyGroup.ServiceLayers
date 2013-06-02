/*
Lekérdezi a megrendelés info változásokat a legutolsó szinkronizáció óta
*/

USE [Axdb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.SalesLineCT
GO
CREATE PROCEDURE InternetUser.SalesLineCT( @LastVersion BIGINT = 0 )
AS
SET NOCOUNT ON
	-- DECLARE @LastVersion BIGINT = 0;
	IF (@LastVersion = 0)
	BEGIN
		SET @LastVersion = ISNULL((SELECT MAX(LastVersion) FROM ExtractInterface.dbo.SyncMetadata WHERE TableName = 'SalesOrder' AND [Status] = 1), 0);
	END

	;
	WITH ChangeTableCTE([Version], Operation, InventTransId, DataAreaId)
	AS (
		SELECT ct.SYS_CHANGE_VERSION as [Version], 
		ct.SYS_CHANGE_OPERATION as Operation,
		ct.InventTransId, 
		ct.[DataAreaId]
		FROM Axdb.dbo.SALESLINE as L 
		RIGHT OUTER JOIN changetable(changes Axdb.dbo.SALESLINE, @LastVersion) as ct
		on L.InventTransId = ct.InventTransId and L.DataAreaId = ct.DataAreaId
		--WHERE  L.CustAccount = 'V005024' and ct.SYS_CHANGE_OPERATION <> 'D'
	),
	--select * from ChangeTableCTE;
	-- képek duplikációk nélküli kikeresése	
	--Picture_CTE (ItemId, [FileName]) AS
	--(
	--	SELECT ItemId, [FileName] 
	--	FROM Axdb.dbo.UPDKEPEK
	--	GROUP BY ItemId, [FileName], ELSODLEGESKEP
	--	HAVING ELSODLEGESKEP = 1 AND COUNT(*) = 1 AND ItemId <> ''
	--),
	SalesLineCTE ([Version], Operation, InventTransId, DataAreaId, SalesId, LineNum, SalesStatus, ProductId, ProductName, SalesPrice, Quantity, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName])
	AS  ( select CTE.[Version], 
				 CTE.Operation,
				 sl.InventTransId,
				 sl.DataAreaId, 
				 sl.SalesId, 
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
				 INNER JOIN ChangeTableCTE as CTE on CTE.InventTransId = sl.InventTransId and CTE.DataAreaId = sl.DataAreaId
				 INNER JOIN Axdb.dbo.InventTrans as it ON sl.DataAreaID = it.DataAreaID and sl.InventTransId = it.InventTransId
				 INNER JOIN Axdb.dbo.InventDim AS id ON id.InventDimId = sl.InventDimId AND id.DataAreaId = sl.DataAreaId
				 LEFT OUTER JOIN Axdb.dbo.UPDKEPEK as Picture ON Picture.ItemId = sl.ItemId and Picture.ELSODLEGESKEP = 1
				 where sl.DataAreaId IN ('bsc', 'hrp') and 
						--sl.SalesStatus = 1 and	-- 1: Nyitott rendelés (backorder), 2: Szállítva (delivered), 3: Számlázva (Invoiced), 4: Érvénytelenítve (Canceled)
						id.InventLocationId IN ( 'BELSO', 'KULSO', '1000', '7000', 'HASZNALT', '2100' ) and 
						it.StatusIssue IN (1, 2, 3, 4, 5, 6)	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)
	) 
	-- select * from SalesLineCTE;


	-- SalesHeaderCTE (CustomerId, DataAreaId, SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment, SalesHeaderType, SalesHeaderStatus, CustomerOrderNo, DlvTerm) AS (
		select SalesLineCTE.Operation, SalesLineCTE.[Version], 
			   st.CustAccount as CustomerId, st.DataAreaId, st.SalesId, st.CreatedDate, st.ShippingDateRequested, st.CurrencyCode, Pt.[Description] as Payment, st.SalesHeaderType, st.SalesStatus as SalesHeaderStatus, st.VEVORENDELESSZAMA as CustomerOrderNo, st.DLVTERM as DlvTerm,
			   SalesLineCTE.LineNum, SalesLineCTE.SalesStatus, SalesLineCTE.ProductId, SalesLineCTE.ProductName, SalesLineCTE.SalesPrice, SalesLineCTE.Quantity, SalesLineCTE.LineAmount, 
			   SalesLineCTE.SalesDeliverNow, SalesLineCTE.RemainSalesPhysical, SalesLineCTE.StatusIssue, SalesLineCTE.InventLocationId, SalesLineCTE.ItemDate, SalesLineCTE.[FileName]
		from Axdb.dbo.SalesTable as st 
		inner join SalesLineCTE on SalesLineCTE.SalesId = st.SalesId
		inner join Axdb.dbo.PAYMTERM AS pt ON st.Payment = pt.PaymTermId AND Pt.DATAAREAID = 'mst'
		where st.SalesType = 3 and 
			  st.DataAreaId IN ('bsc', 'hrp')
	--)


	--select ChangeTableCTE.Operation, ChangeTableCTE.[Version], 
	--	   SalesHeaderCTE.*, SalesLineCTE.LineNum, SalesLineCTE.SalesStatus, SalesLineCTE.ProductId, SalesLineCTE.ProductName, SalesLineCTE.SalesPrice, SalesLineCTE.Quantity, SalesLineCTE.LineAmount, 
	--	   SalesLineCTE.SalesDeliverNow, SalesLineCTE.RemainSalesPhysical, SalesLineCTE.StatusIssue, SalesLineCTE.InventLocationId, SalesLineCTE.ItemDate, SalesLineCTE.[FileName]
	--from ChangeTableCTE 
	--	 inner join SalesLineCTE on ChangeTableCTE.InventTransId = SalesLineCTE.InventTransId and ChangeTableCTE.DataAreaId = SalesLineCTE.DataAreaId
	--     inner join SalesHeaderCTE on SalesLineCTE.SalesId = SalesHeaderCTE.SalesId
	--order by SalesHeaderCTE.CreatedDate, SalesLineCTE.SalesId, SalesLineCTE.LineNum;

RETURN

GO
GRANT EXECUTE ON InternetUser.SalesLineCT TO [HRP_HEADOFFICE\AXPROXY]
GO
GRANT EXECUTE ON InternetUser.SalesLineCT TO InternetUser
GO
-- exec InternetUser.SalesLineCT 1310

/*
select top 1 * from Axdb.dbo.SalesLine
select * from changetable(changes Axdb.dbo.SalesLine, 0) as ct

Operation	Version	CustomerId	DataAreaId	SalesId	CreatedDate	ShippingDateRequested	CurrencyCode	Payment	SalesHeaderType	SalesHeaderStatus	CustomerOrderNo	DlvTerm	LineNum	SalesStatus	ProductId	ProductName	SalesPrice	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	ItemDate	FileName
I	52401	V001446	hrp	VR636575	2013-04-28 00:00:00.000	2013-04-28 00:00:00.000	HUF	Átutalás 21 napra	Standard	1		RAKTÁRBÓL	1	1	X55ASX044D	ASUS X55A-SX044D   15.6" HD Pentium Dual Celeron B820, 2GB,320GB ,webcam, DVD DL	64290	1	64290	0	1	6	KULSO	2013-04-28 00:00:00.000	X55ASX044D.jpg

Operation	Version	CustAccount	DataAreaId	SalesId	CreatedDate	ShippingDateRequested	CurrencyCode	Description	SalesHeaderType	SalesStatus	VEVORENDELESSZAMA	DLVTERM	LineNum	SalesStatus	ProductId	ProductName	SalesPrice	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	ItemDate	FileName
I	52401	V001446	hrp	VR636575	2013-04-28 00:00:00.000	2013-04-28 00:00:00.000	HUF	Átutalás 21 napra	Standard	1		RAKTÁRBÓL	1	1	X55ASX044D	ASUS X55A-SX044D   15.6" HD Pentium Dual Celeron B820, 2GB,320GB ,webcam, DVD DL	64290	1	64290	0	1	6	KULSO	2013-04-28 00:00:00.000	X55ASX044D.jpg
*/