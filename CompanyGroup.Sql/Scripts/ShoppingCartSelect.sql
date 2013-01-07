USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár elem 
DROP PROCEDURE [InternetUser].[GetCart];
GO
CREATE PROCEDURE [InternetUser].[GetCart] (@Id INT = 0) 
AS
SET NOCOUNT ON
SELECT 

      Cart.DeliveryDateRequested as DateRequested,
      Cart.DeliveryZipCode as ZipCode,
      Cart.DeliveryCity as City,
      Cart.DeliveryCountry as Country,
      Cart.DeliveryStreet as Street, 
      Cart.DeliveryAddrRecId as AddrRecId,
      Cart.InvoiceAttached as InvoiceAttached,

    Line.CartId,
    Line.ProductId,
    Catalogue.Name,
    Catalogue.EnglishName,
    Catalogue.PartNumber,
    Catalogue.StandardConfigId,
    Line.Price,
    Catalogue.ItemState,
    Line.Quantity,
    Line.DataAreaId,
    Line.Status,
    Line.CreatedDate
FROM InternetUser.ShoppingCart as Cart
INNER JOIN InternetUser.ShoppingCartLine as Line ON Cart.Id = Line.CartId
INNER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
WHERE Cart.Status IN (1, 2) AND 
	  Line.Status IN (1, 2) AND 
	  Cart.Id = @Id;

RETURN

-- EXEC [InternetUser].[GetCart] 1;

DROP PROCEDURE [InternetUser].[GetCartCollection];
GO
CREATE PROCEDURE [InternetUser].[GetCartCollection] (@VisitorId NVARCHAR(20) = '') 
AS
SET NOCOUNT ON
SELECT 

      Cart.DeliveryDateRequested as DateRequested,
      Cart.DeliveryZipCode as ZipCode,
      Cart.DeliveryCity as City,
      Cart.DeliveryCountry as Country,
      Cart.DeliveryStreet as Street, 
      Cart.DeliveryAddrRecId as AddrRecId,
      Cart.InvoiceAttached as InvoiceAttached,

    Line.CartId,
    Line.ProductId,
    Catalogue.Name,
    Catalogue.EnglishName,
    Catalogue.PartNumber,
    Catalogue.StandardConfigId,
    Line.Price,
    Catalogue.ItemState,
    Line.Quantity,
    Line.DataAreaId,
    Line.Status,
    Line.CreatedDate
FROM InternetUser.ShoppingCart as Cart
INNER JOIN InternetUser.ShoppingCartLine as Line ON Cart.Id = Line.CartId
INNER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
WHERE Cart.Status IN (1, 2) AND 
	  Line.Status IN (1, 2) AND 
	  Cart.VisitorId = @VisitorId;

RETURN

-- EXEC [InternetUser].[GetCartCollection] '';