USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár elem 
DROP PROCEDURE [InternetUser].[GetFinanceOffer];
GO
CREATE PROCEDURE [InternetUser].[GetFinanceOffer] (@OfferId INT = 0) 
AS
SET NOCOUNT ON

	SELECT Offer.Id, 
		Offer.VisitorId,
		'' as CompanyId,
		'' as PersonId,
		Offer.LeasingPersonName, 
		Offer.LeasingAddress,
		Offer.LeasingPhone,
		Offer.LeasingStatNumber,
		Offer.NumOfMonth, 
		Offer.Currency,
		Offer.[Status],
		Offer.CreatedDate,
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
	FROM InternetUser.FinanceOffer as Offer
	INNER JOIN InternetUser.ShoppingCartLine as Line ON Offer.Id = Line.CartId
	INNER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = Line.ProductId AND Line.DataAreaId = Catalogue.DataAreaId
	WHERE Offer.Status IN (1, 2) AND 
		  Line.Status IN (1, 2) AND 
		  Offer.Id = @OfferId;
RETURN
GO
GRANT EXECUTE ON InternetUser.GetFinanceOffer TO InternetUser
-- EXEC [InternetUser].[GetFinanceOffer] 1;