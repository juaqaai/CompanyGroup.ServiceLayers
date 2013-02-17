USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[StructureSelect];
GO
CREATE PROCEDURE [InternetUser].[StructureSelect] (@DataAreaId nvarchar(4) = 'hrp',
												   @Manufacturers nvarchar (250) = '',
												   @Category1 nvarchar (250) = '',
												   @Category2 nvarchar (250) = '',
												   @Category3 nvarchar (250) = '',	 
												   @Discount bit = 0,      
												   @SecondHand bit = 0,     
												   @New bit = 0,         
												   @Stock bit = 0,     
												   @FindText nvarchar(64) = '', 
												   @PriceFilter nvarchar(16) = '',
												   @PriceFilterRelation INT = 0
)
AS
SET NOCOUNT ON

	DECLARE @Separator varchar(1) = ',';

	WITH Manufacturers_CTE (Mi, Mj)
	AS (
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Manufacturers)
		UNION ALL
		SELECT CONVERT(INT, Mj) + 1, CHARINDEX(@Separator, @Manufacturers, CONVERT(INT, Mj) + 1)
		FROM Manufacturers_CTE
		WHERE Mj > 0
		--SELECT CONVERT(nvarchar(4), Manufacturer.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Manufacturer/Id') as Manufacturer(Id)
	),
	Category1_CTE(C1i, C1j)
	AS(
		--SELECT CONVERT(nvarchar(4), Category1.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Category1/Id') as Category1(Id)
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Category1)
		UNION ALL
		SELECT CONVERT(INT, C1j) + 1, CHARINDEX(@Separator, @Category1, CONVERT(INT, C1j) + 1)
		FROM Category1_CTE
		WHERE C1j > 0
	),
	Category2_CTE(C2i, C2j)
	AS(
		--SELECT CONVERT(nvarchar(4), Category2.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Category2/Id') as Category2(Id)
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Category2)
		UNION ALL
		SELECT CONVERT(INT, C2j) + 1, CHARINDEX(@Separator, @Category2, CONVERT(INT, C2j) + 1)
		FROM Category2_CTE
		WHERE C2j > 0
	),
	Category3_CTE(C3i, C3j)
	AS(
		--SELECT CONVERT(nvarchar(4), Category3.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Category3/Id') as Category3(Id)
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Category3)
		UNION ALL
		SELECT CONVERT(INT, C3j) + 1, CHARINDEX(@Separator, @Category3, CONVERT(INT, C3j) + 1)
		FROM Category3_CTE
		WHERE C3j > 0
	)

	SELECT ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, 
			Category2Id, Category2Name, Category2EnglishName, 
			Category3Id, Category3Name, Category3EnglishName
	FROM InternetUser.Catalogue as Catalogue
	WHERE (Catalogue.ManufacturerId IN (SELECT SUBSTRING(@Manufacturers, Mi, COALESCE(NULLIF(Mj, 0), LEN(@Manufacturers) + 1) - Mi) FROM Manufacturers_CTE) OR (@Manufacturers = '')) AND
		  (Catalogue.Category1Id IN (SELECT SUBSTRING(@Category1, C1i, COALESCE(NULLIF(C1j, 0), LEN(@Category1) + 1) - C1i) FROM Category1_CTE) OR (@Category1 = '')) AND
	      (Catalogue.Category2Id IN (SELECT SUBSTRING(@Category2, C2i, COALESCE(NULLIF(C2j, 0), LEN(@Category2) + 1) - C2i) FROM Category2_CTE) OR (@Category2 = '')) AND
		  (Catalogue.Category3Id IN (SELECT SUBSTRING(@Category3, C3i, COALESCE(NULLIF(C3j, 0), LEN(@Category3) + 1) - C3i) FROM Category3_CTE) OR (@Category3 = '')) AND
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
	ORDER BY ManufacturerName, Category1Name, Category2Name, Category3Name;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.StructureSelect TO InternetUser
GO
/* EXEC InternetUser.StructureSelect @DataAreaId = '',
									 @Manufacturers = '',
									 @Category1 = 'B008',													      
									 @Category2 = '',
									 @Category3 = '',
									 @Discount = 0,      
									 @SecondHand = 0,     
									 @New = 0,         
									 @Stock = 0,     
									 @FindText = '', 
									 @PriceFilter = '',
									 @PriceFilterRelation = 0;

SELECT ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, 
			Category2Id, Category2Name, Category2EnglishName, 
			Category3Id, Category3Name, Category3EnglishName 
FROM InternetUser.Catalogue 
WHERE Category1Id = 'B008' --AND Category2Id = 'C230'	
GROUP BY ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, 
			Category2Id, Category2Name, Category2EnglishName, 
			Category3Id, Category3Name, Category3EnglishName

*/
