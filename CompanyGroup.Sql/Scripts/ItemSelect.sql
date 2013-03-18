USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- �rlista
DROP PROCEDURE [InternetUser].[ItemSelect];
GO
CREATE PROCEDURE [InternetUser].[ItemSelect] (@DataAreaId nvarchar(4) = 'hrp',
											  @ProductId nvarchar (20) = '')
AS
SET NOCOUNT ON

	SELECT TOP 1 Catalogue.Id, ProductId, AxStructCode,	DataAreaId,	StandardConfigId, Name,	EnglishName, PartNumber, ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, Category2Id, Category2Name, Category2EnglishName, Category3Id, Category3Name, Category3EnglishName, 
			Stock, AverageInventory,	Price1,	Price2,	Price3,	Price4,	Price5,	Garanty, GarantyMode, 
			Discount, New, ItemState, Description, EnglishDescription, ProductManagerId, ShippingDate, IsPurchaseOrdered, CreatedDate, Updated, Available, PictureId, SecondHand, Valid
	FROM InternetUser.Catalogue as Catalogue
	WHERE
		DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE DataAreaId END AND 
		ProductId = @ProductId;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.ItemSelect TO InternetUser
-- EXEC InternetUser.ItemSelect @ProductId = 'RLA-00005', @DataAreaId = 'bsc';
-- SELECT * FROM InternetUser.Catalogue WHERE DataAreaId = 'bsc' AND 
