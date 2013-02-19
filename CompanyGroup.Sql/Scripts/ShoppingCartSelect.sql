USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár elem 
DROP PROCEDURE [InternetUser].[GetShoppingCart];
GO
CREATE PROCEDURE [InternetUser].[GetShoppingCart] (@CartId INT = 0) 
AS
SET NOCOUNT ON
	SELECT Cart.Id, 
		Cart.VisitorId,
		'' as CompanyId,
		'' as PersonId,
		Cart.Name, 
		Cart.PaymentTerms,
		Cart.DeliveryTerms,
		Cart.DeliveryDateRequested as DateRequested,
		Cart.DeliveryZipCode as ZipCode,
		Cart.DeliveryCity as City,
		Cart.DeliveryCountry as Country,
		Cart.DeliveryStreet as Street, 
		Cart.DeliveryAddrRecId as AddrRecId,
		Cart.InvoiceAttached,
		Cart.Active, 
		Cart.Currency,
		Cart.[Status],
		Cart.CreatedDate,
		Line.Id as LineId,
		ISNULL(Line.CartId, 0) as CartId, 
		Line.ProductId as ProductId,
		ISNULL(Catalogue.Name, '') as ProductName,
		ISNULL(Catalogue.EnglishName, '') as ProductNameEnglish,
		ISNULL(Catalogue.PartNumber, '') as PartNumber,
		ISNULL(Line.Price, 0) as CustomerPrice,
		ISNULL(Catalogue.ItemState, 0) as ItemState,
		ISNULL(Line.DataAreaId, '') as DataAreaId,
		ISNULL(Line.ConfigId, '') as ConfigId,
		ISNULL(Line.InventLocationId, '') as InventLocationId,
		ISNULL(Line.Quantity, 0) as Quantity,
		ISNULL(Line.Status, 0) as Status, 
		ISNULL(Line.CreatedDate, CONVERT(DateTime, 0)) as CreatedDate, 
		ISNULL(Catalogue.InnerStock, 0) as [Inner], 
		ISNULL(Catalogue.OuterStock, 0) as [Outer]
	FROM InternetUser.ShoppingCart as Cart
	LEFT OUTER JOIN InternetUser.ShoppingCartLine as Line ON Cart.Id = Line.CartId
	LEFT OUTER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE (Cart.Status IN (1, 2)) AND 
		  (Line.Status IN (1, 2) OR Line.Status IS NULL) AND 
		  (Cart.Id = @CartId);
RETURN
-- EXEC [InternetUser].[GetShoppingCart] 106;
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCart] TO InternetUser
GO

DROP PROCEDURE [InternetUser].[GetShoppingCartCollection];
GO
CREATE PROCEDURE [InternetUser].[GetShoppingCartCollection] (@VisitorId NVARCHAR(64) = '') 
AS
SET NOCOUNT ON
	SELECT Cart.Id, 
		Cart.VisitorId,
		'' as CompanyId,
		'' as PersonId,
		Cart.Name, 
		Cart.PaymentTerms,
		Cart.DeliveryTerms,
		Cart.DeliveryDateRequested as DateRequested,
		Cart.DeliveryZipCode as ZipCode,
		Cart.DeliveryCity as City,
		Cart.DeliveryCountry as Country,
		Cart.DeliveryStreet as Street, 
		Cart.DeliveryAddrRecId as AddrRecId,
		Cart.InvoiceAttached,
		Cart.Active, 
		Cart.Currency,
		Cart.[Status],
		ISNULL(Line.Id, 0) as LineId,
		ISNULL(Line.CartId, 0) as CartId, 
		ISNULL(Line.ProductId, '') as ProductId,
		ISNULL(Catalogue.Name, '') as ProductName,
		ISNULL(Catalogue.EnglishName, '') as ProductNameEnglish,
		ISNULL(Catalogue.PartNumber, '') as PartNumber,
		ISNULL(Catalogue.StandardConfigId, '') as ConfigId,
		ISNULL(Line.Price, 0) as CustomerPrice,
		ISNULL(Catalogue.ItemState, 0) as ItemState,
		ISNULL(Line.DataAreaId, '') as DataAreaId,
		ISNULL(Line.ConfigId, '') as ConfigId,
		ISNULL(Line.InventLocationId, '') as InventLocationId,
		ISNULL(Line.Quantity, 0) as Quantity,
		ISNULL(Line.Status, 0) as Status, 
		ISNULL(Line.CreatedDate, CONVERT(DateTime, 0)) as CreatedDate, 
		ISNULL(Catalogue.InnerStock, 0) as [Inner], 
		ISNULL(Catalogue.OuterStock, 0) as [Outer]
	FROM InternetUser.ShoppingCart as Cart
	LEFT OUTER JOIN InternetUser.ShoppingCartLine as Line ON Cart.Id = Line.CartId
	LEFT OUTER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE Cart.Status IN (1, 2) AND 
		  ((Line.Status IN (1, 2)) OR (Line.Status IS NULL)) AND 
		  Cart.VisitorId = @VisitorId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCartCollection] TO InternetUser
GO
-- EXEC [InternetUser].[GetShoppingCartCollection] 'C1FF170BBF084ABEBCD9EFF9AA1EB5F0';

DROP PROCEDURE [InternetUser].[GetShoppingCartHeaders];
GO
CREATE PROCEDURE [InternetUser].[GetShoppingCartHeaders] (@VisitorId NVARCHAR(64) = '') 
AS
SET NOCOUNT ON
	SELECT Id, Name, Active, [Status]
	FROM InternetUser.ShoppingCart
	WHERE [Status] IN (1, 2) AND 
		  VisitorId = @VisitorId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCartHeaders] TO InternetUser
GO
-- EXEC [InternetUser].[GetShoppingCartHeaders] 'C1FF170BBF084ABEBCD9EFF9AA1EB5F0';

DROP PROCEDURE [InternetUser].[GetShoppingCartLine];
GO
CREATE PROCEDURE [InternetUser].[GetShoppingCartLine] (@LineId INT = 0) 
AS
SET NOCOUNT ON
	SELECT Line.Id as LineId,
		Line.CartId,
		Line.ProductId,
		Catalogue.Name as ProductName,
		Catalogue.EnglishName as ProductNameEnglish,
		Catalogue.PartNumber,
		Catalogue.StandardConfigId as ConfigId,
		Line.Price as CustomerPrice,
		Catalogue.ItemState,
		Line.DataAreaId,
		Line.ConfigId,
		Line.InventLocationId,
		Line.Quantity,
		Line.Status, 
		Line.CreatedDate, 
		Catalogue.InnerStock as [Inner], 
		Catalogue.OuterStock as [Outer]
	FROM InternetUser.ShoppingCartLine as Line
	LEFT OUTER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE Line.Status IN (1, 2) AND 
		  Line.Id = @LineId;
RETURN
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCartLine] TO InternetUser
GO

-- EXEC [InternetUser].[GetShoppingCartLine] 1