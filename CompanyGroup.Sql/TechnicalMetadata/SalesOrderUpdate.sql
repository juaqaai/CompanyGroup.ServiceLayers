
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
	
	MERGE Web.InternetUser.SalesOrder as t
	USING Web.InternetUser.Stage_SalesOrder s
	ON t.SalesId = s.SalesId AND t.LineNum = s.LineNum AND t.DataAreaId = s.DataAreaId
	WHEN MATCHED THEN
		UPDATE
		SET t.ShippingDateRequested = s.ShippingDateRequested, 
			t.CurrencyCode = s.CurrencyCode, 
			t.Payment = s.Payment, 
			t.SalesHeaderStatus = s.SalesHeaderStatus,
			t.CustomerOrderNo = s.CustomerOrderNo,
			t.DlvTerm = s.DlvTerm,
			t.SalesStatus = s.SalesStatus, 
			t.ItemId = s.ItemId, 
			t.ItemName = s.ItemName, 
			t.Quantity = s.Quantity, 
			t.SalesPrice = s.SalesPrice, 
			t.LineAmount = s.LineAmount, 
			t.SalesDeliverNow = s.SalesDeliverNow,
			t.RemainSalesPhysical = s.RemainSalesPhysical,
			t.StatusIssue = s.StatusIssue,
			t.InventLocationId = s.InventLocationId,
			t.ItemDate = s.ItemDate
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (CustomerId, DataAreaId,	SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment,	SalesHeaderType, SalesHeaderStatus,	CustomerOrderNo, DlvTerm, LineNum, SalesStatus, ItemId, ItemName, Quantity, SalesPrice, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName], ExtractDate, PackageLogKey)
		VALUES (CustomerId, DataAreaId, SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment, SalesHeaderType, SalesHeaderStatus, CustomerOrderNo, DlvTerm, LineNum, SalesStatus, ItemId, ItemName, Quantity, SalesPrice, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName], ExtractDate, PackageLogKey);

RETURN
GO
GRANT EXECUTE ON dbo.[SalesOrderUpdate] TO [HRP_HEADOFFICE\AXPROXY]
GO
-- exec [dbo].[SalesOrderUpdate]  
-- select * from Web.InternetUser.Stage_SalesOrder
-- select top 10 * from Web.InternetUser.SalesOrder where SalesStatus = 1 order by SalesId 


