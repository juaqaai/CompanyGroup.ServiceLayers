USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueSelect] (@DataAreaId nvarchar(4) = 'hrp',
												   @ManufacturerId nvarchar (4) = '',	
 												   @Category1Id nvarchar (4) = '',       
 												   @Category2Id nvarchar (4) = '',       
												   @Category3Id nvarchar (4) = '',       
												   @Discount bit = 0,      
												   @SecondHand bit = 0,     
												   @New bit = 0,         
												   @Stock bit = 0,     
												   @Sequence int = 2,	
												   @FindText nvarchar(64) = '', 
												   @CurrentPageIndex INT = 1, 
												   @ItemsOnPage INT = 30  
)
AS
SET NOCOUNT ON

		SELECT Id, ProductId, AxStructCode,	DataAreaId,	StandardConfigId, Name,	EnglishName, PartNumber, ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
				Category1Id, Category1Name, Category1EnglishName, Category2Id, Category2Name, Category2EnglishName, Category3Id, Category3Name, Category3EnglishName, 
				InnerStock, OuterStock, AverageInventory,	Price1,	Price2,	Price3,	Price4,	Price5,	Garanty, GarantyMode, 
				Discount, New, ItemState, Description, EnglishDescription, ProductManagerId, ShippingDate, CreatedDate, Updated, Valid
		FROM InternetUser.Catalogue
		WHERE ManufacturerId = CASE WHEN @ManufacturerId <> '' THEN @ManufacturerId ELSE ManufacturerId END AND 
			Category1Id = CASE WHEN @Category1Id <> '' THEN @Category1Id ELSE Category1Id END AND 
			Category2Id = CASE WHEN @Category2Id <> '' THEN @Category2Id ELSE Category2Id END AND 
			Category3Id = CASE WHEN @Category3Id <> '' THEN @Category3Id ELSE Category3Id END AND
			Discount = CASE WHEN @Discount = 1 THEN 1 ELSE Discount END AND  
		-- c.bBargain = CASE WHEN @bBargainCounter = 1 THEN 1 ELSE c.bBargain END AND  
			New = CASE WHEN @New = 1 THEN 1 ELSE New END AND  
			Valid = 1 AND
			1 = CASE WHEN ItemState = 1 AND InnerStock + OuterStock <= 0 THEN 0 ELSE 1 END AND 
			1 = CASE WHEN @Stock = 1 AND InnerStock + OuterStock <= 0 THEN 0 ELSE 1 END AND
			DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE DataAreaId END AND 
		( Name LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE Name END OR 
		ProductId LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE ProductId END OR 
		PartNumber LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE PartNumber END OR
		Description LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE Description END OR
		ManufacturerName LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE ManufacturerName END OR
		Category1Name LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE Category1Name END OR
		Category2Name LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE Category2Name END OR 
		Category3Name LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE Category3Name END
		)
		ORDER BY 
		CASE WHEN @Sequence =  0 THEN --�tlagos �letkor cs�kken�, akci�s cs�kken�, gy�rt� n�vekv�, term�kazonos�t� szerint n�vekv�leg,
			AverageInventory + Discount END DESC,
		CASE WHEN @Sequence =  1 THEN -- �tlagos �letkor n�vekv�, akci�s cs�kken�, gy�rt� n�vekv�, term�kazonos�t� szerint n�vekv�leg,
			AverageInventory + Discount END ASC, 
		CASE WHEN @Sequence =  2 THEN -- azonosito novekvo
			ProductId END ASC,
		CASE WHEN @Sequence = 3 THEN -- azonosito csokkeno 
			ProductId END DESC, 
		CASE WHEN @Sequence = 4 THEN  -- nev szerint novekvo
			Name END DESC,
		CASE WHEN @Sequence = 5 THEN  -- nev szerint csokkeno
			Name END ASC, 
		CASE WHEN @Sequence = 6 THEN  
			Price5 END ASC,
		CASE WHEN @Sequence = 7 THEN  
			Price5 END DESC, 
		CASE WHEN @Sequence = 8 THEN   
			InnerStock END ASC, 
		CASE WHEN @Sequence = 9 THEN    
			InnerStock END DESC, 
		CASE WHEN @Sequence = 10 THEN 
			OuterStock END ASC, 
		CASE WHEN @Sequence = 11 THEN 
			OuterStock END DESC, 
		CASE WHEN @Sequence = 12 THEN  
			Garanty END ASC, 
		CASE WHEN @Sequence = 13 THEN  
			Garanty END DESC
		OFFSET (@CurrentPageIndex-1)*@ItemsOnPage ROWS
		FETCH NEXT @ItemsOnPage ROWS ONLY;
			
RETURN

-- EXEC InternetUser.CatalogueSelect @Stock = 1, @Sequence = 7;