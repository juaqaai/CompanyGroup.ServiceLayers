USE Web
GO
/****** Object:  StoredProcedure [InternetUser].[web_GetSales]    Script Date: 2012.09.18. 20:55:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.SalesOrderSelect
GO
CREATE PROCEDURE InternetUser.SalesOrderSelect(@CustomerId NVARCHAR(10), 
											   @CanBeTaken BIT = 0, 	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása))
											   @SalesStatus INT = 1, 
											   @CustomerOrderNo NVARCHAR(20) = '', @ItemName NVARCHAR(1000) = '', @ItemId NVARCHAR(20) = '', @SalesOrderId NVARCHAR(20) = '')
	AS
	SET NOCOUNT ON;

	SELECT S.Id, S.DataAreaId, SalesId, S.CreatedDate, ShippingDateRequested, CurrencyCode, Payment, SalesHeaderType, SalesHeaderStatus, CustomerOrderNo, CASE WHEN S.DlvTerm = 'KISZALL' THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as WithDelivery, 
	  	   LineNum, SalesStatus, ItemId as ProductId, ItemName as ProductName, Quantity, SalesPrice, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName], 
		   CASE WHEN ISNULL(C.Stock, 0) > 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as InStock, 
		   CASE WHEN ISNULL(C.Available, 0) > 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as AvailableInWebShop
	FROM InternetUser.SalesOrder as S
		 LEFT OUTER JOIN InternetUser.Catalogue as C ON S.ItemId = C.ProductId AND s.DataAreaId = C.DataAreaId
	WHERE CustomerId = @CustomerId AND 
		  SalesStatus = @SalesStatus AND
		  1 = CASE WHEN (@CanBeTaken = 1 AND StatusIssue = 4) OR (@CanBeTaken = 0) THEN 1 ELSE 0 END AND 
		  StatusIssue IN (4, 5, 6) AND	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)
		  CustomerOrderNo = CASE WHEN @CustomerOrderNo <> CustomerOrderNo THEN @CustomerOrderNo ELSE '' END AND 
		  ItemName LIKE CASE WHEN @ItemName <> '' THEN '%' + @ItemName + '%' ELSE ItemName END AND 
		  ItemId LIKE CASE WHEN @ItemId <> '' THEN '%' + @ItemId + '%' ELSE ItemId END AND 
		  SalesId LIKE CASE WHEN @SalesOrderId <> '' THEN '%' + @SalesOrderId + '%' ELSE SalesId END

	ORDER BY SalesId, LineNum

RETURN 
GO
GRANT EXECUTE ON InternetUser.SalesOrderSelect TO InternetUser;

-- exec InternetUser.SalesOrderSelect 'V001446';
-- select CASE WHEN DlvTerm = 'KISZALL' THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as WithDelivery, * from InternetUser.SalesOrder where DlvTerm <> ''


GO
DROP PROCEDURE InternetUser.SalesOrderOpenOrderAmount
GO
CREATE PROCEDURE InternetUser.SalesOrderOpenOrderAmount(@CustomerId NVARCHAR(10))
	AS
	SET NOCOUNT ON;

	SELECT SUM(LineAmount) as Amount
	FROM InternetUser.SalesOrder
	WHERE CustomerId = @CustomerId AND 
		  SalesStatus = 1 AND
		  StatusIssue IN (4, 5, 6)	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)

RETURN 
GO
GRANT EXECUTE ON InternetUser.SalesOrderOpenOrderAmount TO InternetUser;

-- exec InternetUser.SalesOrderOpenOrderAmount 'V001446';

DROP PROCEDURE [InternetUser].[SalesOrderPictureSelect];
GO
CREATE PROCEDURE [InternetUser].[SalesOrderPictureSelect]( @Id INT = 0 )										
AS
SET NOCOUNT ON
	SELECT TOP 1 Id, [FileName], CONVERT(BIT, 1) as [Primary], 0 as RecId
	FROM InternetUser.SalesOrder
	WHERE Id = @Id AND [FileName] <> '';
RETURN
GO
GRANT EXECUTE ON InternetUser.SalesOrderPictureSelect TO InternetUser
GO

-- exec [InternetUser].[SalesOrderPictureSelect] 10