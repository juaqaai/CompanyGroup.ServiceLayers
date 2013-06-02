
USE [TechnicalMetadata]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- megrendelések frissítés
DROP PROCEDURE [dbo].[SalesOrderInsert];
GO
CREATE PROCEDURE [dbo].[SalesOrderInsert]  
AS
SET NOCOUNT ON
	
	INSERT INTO Web.InternetUser.SalesOrder 

	SELECT 
	s.CustomerId, s.DataAreaId, s.SalesId, s.CreatedDate, s.ShippingDateRequested, s.CurrencyCode, s.Payment, s.SalesHeaderType, s.SalesHeaderStatus, s.CustomerOrderNo, 
	s.DlvTerm, s.LineNum, s.SalesStatus, s.ItemId, s.ItemName, s.Quantity, s.SalesPrice, s.LineAmount, s.SalesDeliverNow, s.RemainSalesPhysical, 
	s.StatusIssue, s.InventLocationId, s.ItemDate, s.[FileName], s.ExtractDate, s.PackageLogKey
	FROM Web.InternetUser.Stage_SalesOrder as s
	WHERE Operation = 'I';
RETURN
GO
GRANT EXECUTE ON dbo.[SalesOrderInsert] TO [HRP_HEADOFFICE\AXPROXY]
GO
-- exec [dbo].[SalesOrderInsert]
-- select * from Web.InternetUser.SalesOrder	1783
-- select * from Web.InternetUser.Stage_SalesOrder  1064
-- select * from Web.InternetUser.SalesOrder where SalesId = 'VR657927'
-- select SalesId, LineNum, DataAreaId from Web.InternetUser.Stage_SalesOrder group by SalesId, LineNum, DataAreaId 	select * from Web.InternetUser.Stage_SalesOrder
-- select top 10 * from Web.InternetUser.SalesOrder where SalesStatus = 1 order by SalesId 


