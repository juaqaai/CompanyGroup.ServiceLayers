USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[StructureSelect];
GO
CREATE PROCEDURE [InternetUser].[StructureSelect] (@DataAreaId nvarchar(4) = 'hrp',
												   @ManufacturerId nvarchar (4) = '',	
 												   @Category1Id nvarchar (4) = '',       
 												   @Category2Id nvarchar (4) = '',       
												   @Category3Id nvarchar (4) = '',       
												   @Discount bit = 0,      
												   @SecondHand bit = 0,     
												   @New bit = 0,         
												   @Stock bit = 0,     
												   @Sequence int = 2,	
												   @FindText nvarchar(64) = ''
)
AS
SET NOCOUNT ON

		SELECT ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			   Category1Id, Category1Name, Category1EnglishName, 
			   Category2Id, Category2Name, Category2EnglishName, 
			   Category3Id, Category3Name, Category3EnglishName
		FROM InternetUser.Catalogue
		WHERE ManufacturerId = CASE WHEN @ManufacturerId <> '' THEN @ManufacturerId ELSE ManufacturerId END AND 
			Category1Id = CASE WHEN @Category1Id <> '' THEN @Category1Id ELSE Category1Id END AND 
			Category2Id = CASE WHEN @Category2Id <> '' THEN @Category2Id ELSE Category2Id END AND 
			Category3Id = CASE WHEN @Category3Id <> '' THEN @Category3Id ELSE Category3Id END AND
			Discount = CASE WHEN @Discount = 1 THEN 1 ELSE Discount END AND  
			SecondHand = CASE WHEN @SecondHand = 1 THEN 1 ELSE SecondHand END AND  
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
		GROUP BY ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			   Category1Id, Category1Name, Category1EnglishName, 
			   Category2Id, Category2Name, Category2EnglishName, 
			   Category3Id, Category3Name, Category3EnglishName
		ORDER BY ManufacturerId, Category1Id, Category2Id, Category3Id;
			
RETURN

-- EXEC InternetUser.StructureSelect @Stock = 1, @Sequence = 3;