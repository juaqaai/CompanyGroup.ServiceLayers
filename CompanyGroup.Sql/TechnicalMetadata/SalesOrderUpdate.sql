
USE [TechnicalMetadata]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- megrendelések frissítés
DROP PROCEDURE [dbo].[SalesOrderUpdate];
GO
CREATE PROCEDURE [dbo].[SalesOrderUpdate]  
AS
SET NOCOUNT ON
	
	--MERGE Web.InternetUser.SalesOrder as t
	--USING Web.InternetUser.Stage_SalesOrder s
	--ON t.SalesId = s.SalesId AND t.LineNum = s.LineNum AND t.DataAreaId = s.DataAreaId
	--WHEN MATCHED THEN
	--	UPDATE
	--	SET t.ShippingDateRequested = s.ShippingDateRequested, 
	--		t.CurrencyCode = s.CurrencyCode, 
	--		t.Payment = s.Payment, 
	--		t.SalesHeaderStatus = s.SalesHeaderStatus,
	--		t.CustomerOrderNo = s.CustomerOrderNo,
	--		t.DlvTerm = s.DlvTerm,
	--		t.SalesStatus = s.SalesStatus, 
	--		t.ItemId = s.ItemId, 
	--		t.ItemName = s.ItemName, 
	--		t.Quantity = s.Quantity, 
	--		t.SalesPrice = s.SalesPrice, 
	--		t.LineAmount = s.LineAmount, 
	--		t.SalesDeliverNow = s.SalesDeliverNow,
	--		t.RemainSalesPhysical = s.RemainSalesPhysical,
	--		t.StatusIssue = s.StatusIssue,
	--		t.InventLocationId = s.InventLocationId,
	--		t.ItemDate = s.ItemDate
	--WHEN NOT MATCHED BY TARGET THEN
	--	INSERT (CustomerId, DataAreaId,	SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment,	SalesHeaderType, SalesHeaderStatus,	CustomerOrderNo, DlvTerm, LineNum, SalesStatus, ItemId, ItemName, Quantity, SalesPrice, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName], ExtractDate, PackageLogKey)
	--	VALUES (CustomerId, DataAreaId, SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment, SalesHeaderType, SalesHeaderStatus, CustomerOrderNo, DlvTerm, LineNum, SalesStatus, ItemId, ItemName, Quantity, SalesPrice, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName], ExtractDate, PackageLogKey);

	UPDATE Web.InternetUser.SalesOrder
		SET ShippingDateRequested = s.ShippingDateRequested, 
			CurrencyCode = s.CurrencyCode, 
			Payment = s.Payment, 
			SalesHeaderStatus = s.SalesHeaderStatus,
			CustomerOrderNo = s.CustomerOrderNo,
			DlvTerm = s.DlvTerm,
			SalesStatus = s.SalesStatus, 
			ItemId = s.ItemId, 
			ItemName = s.ItemName, 
			Quantity = s.Quantity, 
			SalesPrice = s.SalesPrice, 
			LineAmount = s.LineAmount, 
			SalesDeliverNow = s.SalesDeliverNow,
			RemainSalesPhysical = s.RemainSalesPhysical,
			StatusIssue = s.StatusIssue,
			InventLocationId = s.InventLocationId,
			ItemDate = s.ItemDate
		FROM  Web.InternetUser.Stage_SalesOrder as s
			INNER JOIN Web.InternetUser.SalesOrder as t ON 
			t.SalesId = s.SalesId AND t.LineNum = s.LineNum AND t.DataAreaId = s.DataAreaId
		WHERE Operation = 'U';

	INSERT INTO Web.InternetUser.SalesOrder 
	(CustomerId, DataAreaId, SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment, SalesHeaderType, SalesHeaderStatus, CustomerOrderNo, 
	 DlvTerm, LineNum, SalesStatus, ItemId, ItemName, Quantity, SalesPrice, LineAmount, SalesDeliverNow, RemainSalesPhysical, 
	 StatusIssue, InventLocationId, ItemDate, [FileName], ExtractDate, PackageLogKey)
	SELECT 
	s.CustomerId, s.DataAreaId, s.SalesId, s.CreatedDate, s.ShippingDateRequested, s.CurrencyCode, s.Payment, s.SalesHeaderType, s.SalesHeaderStatus, s.CustomerOrderNo, 
	s.DlvTerm, s.LineNum, s.SalesStatus, s.ItemId, s.ItemName, s.Quantity, s.SalesPrice, s.LineAmount, s.SalesDeliverNow, s.RemainSalesPhysical, 
	s.StatusIssue, s.InventLocationId, s.ItemDate, s.[FileName], s.ExtractDate, s.PackageLogKey
	FROM Web.InternetUser.Stage_SalesOrder as s
	FULL OUTER JOIN Web.InternetUser.SalesOrder as t ON 
			t.SalesId = s.SalesId AND t.LineNum = s.LineNum AND t.DataAreaId = s.DataAreaId
	WHERE Operation = 'I' AND t.SalesId IS NULL AND t.LineNum IS NULL AND t.DataAreaId IS NULL;
RETURN
GO
GRANT EXECUTE ON dbo.[SalesOrderUpdate] TO [HRP_HEADOFFICE\AXPROXY]
GO
-- exec [dbo].[SalesOrderUpdate]
-- select * from Web.InternetUser.SalesOrder	1822
-- select * from Web.InternetUser.Stage_SalesOrder  1064
-- select * from Web.InternetUser.SalesOrder where SalesId = 'VR657927'
-- select SalesId, LineNum, DataAreaId from Web.InternetUser.Stage_SalesOrder group by SalesId, LineNum, DataAreaId 	select * from Web.InternetUser.Stage_SalesOrder
-- select top 10 * from Web.InternetUser.SalesOrder where SalesStatus = 1 order by SalesId 


