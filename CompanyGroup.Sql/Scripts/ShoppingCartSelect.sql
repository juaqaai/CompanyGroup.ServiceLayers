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
		Line.ProductId, 
		Line.CartId,
		Catalogue.Name as ProductName,
		Catalogue.EnglishName as ProductNameEnglish,
		Catalogue.PartNumber,
		Catalogue.StandardConfigId as ConfigId,
		Line.Price as CustomerPrice,
		Catalogue.ItemState,
		Line.DataAreaId,
		Line.Quantity,
		Line.Status, 
		Line.CreatedDate, 
		Catalogue.InnerStock as [Inner], 
		Catalogue.OuterStock as [Outer]
	FROM InternetUser.ShoppingCart as Cart
	INNER JOIN InternetUser.ShoppingCartLine as Line ON Cart.Id = Line.CartId
	INNER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE Cart.Status IN (1, 2) AND 
		  Line.Status IN (1, 2) AND 
		  Cart.Id = @CartId;
RETURN
-- EXEC [InternetUser].[GetShoppingCart] 1;
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCart] TO InternetUser
GO

DROP PROCEDURE [InternetUser].[GetShoppingCartCollection];
GO
CREATE PROCEDURE [InternetUser].[GetShoppingCartCollection] (@VisitorId NVARCHAR(20) = '') 
AS
SET NOCOUNT ON
	SELECT Cart.Id, 
		Cart.VisitorId,
		'' as CompanyId,
		'' as PersonId,
		Cart.Name, 
		Cart.PaymentTerms,
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
		Line.Id as LineId,
		Line.CartId, 
		Line.ProductId,
		Catalogue.Name as ProductName,
		Catalogue.EnglishName as ProductNameEnglish,
		Catalogue.PartNumber,
		Catalogue.StandardConfigId as ConfigId,
		Line.Price as CustomerPrice,
		Catalogue.ItemState,
		Line.DataAreaId,
		Line.Quantity,
		Line.Status, 
		Line.CreatedDate, 
		Catalogue.InnerStock as [Inner], 
		Catalogue.OuterStock as [Outer]
	FROM InternetUser.ShoppingCart as Cart
	INNER JOIN InternetUser.ShoppingCartLine as Line ON Cart.Id = Line.CartId
	INNER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE Cart.Status IN (1, 2) AND 
		  Line.Status IN (1, 2) AND 
		  Cart.VisitorId = @VisitorId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCartCollection] TO InternetUser
GO
-- EXEC [InternetUser].[GetShoppingCartCollection] 'teszt';

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
		Line.Quantity,
		Line.Status, 
		Line.CreatedDate, 
		Catalogue.InnerStock as [Inner], 
		Catalogue.OuterStock as [Outer]
	FROM InternetUser.ShoppingCartLine as Line
	INNER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE Line.Status IN (1, 2) AND 
		  Line.Id = @LineId;
RETURN
GO
GRANT EXECUTE ON [InternetUser].[GetShoppingCartLine] TO InternetUser
GO

-- EXEC [InternetUser].[GetShoppingCartLine] 1