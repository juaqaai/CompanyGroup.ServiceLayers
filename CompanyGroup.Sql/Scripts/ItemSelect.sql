USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- árlista
DROP PROCEDURE [InternetUser].[ItemSelect2];
GO
CREATE PROCEDURE [InternetUser].[ItemSelect2] (@VisitorKey int = 0, 
											   @DataAreaId nvarchar(4) = 'hrp',
											   @ProductId nvarchar (20) = '')
AS
SET NOCOUNT ON

	DECLARE @CustomerPriceGroup AS CustomerPriceGroupTableType;

	INSERT INTO @CustomerPriceGroup (PriceGroupId, StructureCode, Sequence, DataAreaId)
	SELECT PriceGroupId, 
		   CONCAT( CASE WHEN ManufacturerId = '' THEN '____' ELSE ManufacturerId END,  
		   CASE WHEN Category1Id = '' THEN '____' ELSE Category1Id END, 
		   CASE WHEN Category2Id = '' THEN '____' ELSE Category2Id END, 
		   CASE WHEN Category3Id = '' THEN '____' ELSE Category3Id END ) , 
		   [Order], 
		   DataAreaId  
	FROM InternetUser.CustomerPriceGroup
	WHERE VisitorId = @VisitorKey;

	SELECT TOP 1 Catalogue.Id, Catalogue.ProductId, AxStructCode, Catalogue.DataAreaId,	StandardConfigId, Name,	EnglishName, PartNumber, 
			ManufacturerId, ManufacturerName, ManufacturerEnglishName, 
			Category1Id, Category1Name, Category1EnglishName, Category2Id, Category2Name, Category2EnglishName, Category3Id, Category3Name, Category3EnglishName, 
			Stock, AverageInventory, 
			Price1,	Price2,	Price3,	Price4,	Price5,	InternetUser.CalculateCustomerPrice(AxStructCode, ISNULL(P.Price, 0), Price1, Price2, Price3, Price4, Price5, Catalogue.DataAreaId, @CustomerPriceGroup) as CustomerPrice,
			Garanty, GarantyMode, 
			Discount, New, ItemState, [Description], EnglishDescription, ProductManagerId, ShippingDate, IsPurchaseOrdered, CreatedDate, Updated, Available, PictureId, SecondHand, Valid
	FROM InternetUser.Catalogue as Catalogue
	LEFT OUTER JOIN InternetUser.CustomerSpecialPrice as P ON Catalogue.ProductId = P.ProductId AND Catalogue.DataAreaId = P.DataAreaId AND P.VisitorId = @VisitorKey
	WHERE Catalogue.DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE Catalogue.DataAreaId END AND 
		Catalogue.ProductId = @ProductId;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.ItemSelect2 TO InternetUser
-- EXEC InternetUser.ItemSelect2 @VisitorKey = 44826, @ProductId = '0728658032067', @DataAreaId = 'bsc';
-- SELECT * FROM InternetUser.Catalogue WHERE DataAreaId = 'bsc' AND 
GO

DROP PROCEDURE [InternetUser].[ItemSelectByShoppingCartLineId2];
GO
CREATE PROCEDURE [InternetUser].[ItemSelectByShoppingCartLineId2] (@VisitorKey int = 0, @LineId INT = 0)
AS
SET NOCOUNT ON

	DECLARE @CustomerPriceGroup AS CustomerPriceGroupTableType;

	INSERT INTO @CustomerPriceGroup (PriceGroupId, StructureCode, Sequence, DataAreaId)
	SELECT PriceGroupId, 
		   CONCAT( CASE WHEN ManufacturerId = '' THEN '____' ELSE ManufacturerId END,  
		   CASE WHEN Category1Id = '' THEN '____' ELSE Category1Id END, 
		   CASE WHEN Category2Id = '' THEN '____' ELSE Category2Id END, 
		   CASE WHEN Category3Id = '' THEN '____' ELSE Category3Id END ) , 
		   [Order], 
		   DataAreaId  
	FROM InternetUser.CustomerPriceGroup
	WHERE VisitorId = @VisitorKey;

	SELECT TOP 1 Catalogue.Id, Catalogue.ProductId, AxStructCode, Catalogue.DataAreaId,	StandardConfigId, Name,	EnglishName, PartNumber, 
			ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, Category2Id, Category2Name, Category2EnglishName, Category3Id, Category3Name, Category3EnglishName, 
			Stock, AverageInventory, 
			Price1,	Price2,	Price3,	Price4,	Price5,	InternetUser.CalculateCustomerPrice(AxStructCode, ISNULL(P.Price, 0), Price1, Price2, Price3, Price4, Price5, Catalogue.DataAreaId, @CustomerPriceGroup) as CustomerPrice,
			Garanty, GarantyMode, 
			Discount, New, ItemState, Description, EnglishDescription, ProductManagerId, ShippingDate, IsPurchaseOrdered, Catalogue.CreatedDate, Updated, Available, PictureId, SecondHand, Valid
	FROM InternetUser.Catalogue as Catalogue
	INNER JOIN InternetUser.ShoppingCartLine as CartLine ON Catalogue.ProductId = CartLine.ProductId AND Catalogue.DataAreaId = CartLine.DataAreaId
	LEFT OUTER JOIN InternetUser.CustomerSpecialPrice as P ON Catalogue.ProductId = P.ProductId AND Catalogue.DataAreaId = P.DataAreaId AND P.VisitorId = @VisitorKey
	WHERE CartLine.Id = @LineId;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.ItemSelectByShoppingCartLineId2 TO InternetUser

/*
select top 1000 * from InternetUser.ShoppingCartLine order by Id desc

EXEC [InternetUser].[ItemSelectByShoppingCartLineId2] 8348;
*/