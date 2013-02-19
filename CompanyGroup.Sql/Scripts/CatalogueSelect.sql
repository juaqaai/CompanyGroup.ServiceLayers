/*
DECLARE @XMLDoc XML = '
<Structure>
<Manufacturer>
    <Id>A004</Id>
    <Id>A006</Id>
</Manufacturer>
<Category1>
    <Id>C004</Id>
    <Id>C006</Id>
</Category1>
<Category2>
    <Id>D004</Id>
    <Id>D006</Id>
</Category2>
<Category3>
    <Id>E004</Id>
    <Id>E006</Id>
</Category3>
</Structure>';

SELECT CONVERT(nvarchar(4), Manufacturer.Id.query('./text()')) as ManufacturerId
FROM @XMLDoc.nodes('/Structure/Manufacturer/Id') as Manufacturer(Id)

SELECT @XMLDoc.query('/Structure/Manufacturer/Id/text()');

DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT, @XMLDoc

        SELECT * FROM OPENXML (@handle, '/Structure/Manufacturer', 2) WITH (Id varchar(4));
		 
EXEC sp_xml_removedocument @handle

SELECT * FROM InternetUser.Catalogue where StandardConfigId <> 'ALAP'

*/

USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueSelect] (@DataAreaId nvarchar(4) = 'hrp',
												   @Manufacturers nvarchar (250) = '',
												   @Category1 nvarchar (250) = '',
												   @Category2 nvarchar (250) = '',
												   @Category3 nvarchar (250) = '',	       
												   @Discount bit = 0,      
												   @SecondHand bit = 0,     
												   @New bit = 0,         
												   @Stock bit = 0,     
												   @Sequence int = 0,	
												   @FindText nvarchar(64) = '', 
												   @PriceFilter nvarchar(16) = '',
												   @PriceFilterRelation INT = 0,	
												   @CurrentPageIndex INT = 1, 
												   @ItemsOnPage INT = 30  
)
AS
SET NOCOUNT ON

	-- DECLARE @Xml Xml = CONVERT(Xml, @StructureXml);
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

	SELECT Catalogue.Id, ProductId, AxStructCode,	DataAreaId,	StandardConfigId, Name,	EnglishName, PartNumber, ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, Category2Id, Category2Name, Category2EnglishName, Category3Id, Category3Name, Category3EnglishName, 
			InnerStock, OuterStock, AverageInventory,	Price1,	Price2,	Price3,	Price4,	Price5,	Garanty, GarantyMode, 
			Discount, New, ItemState, Description, EnglishDescription, ProductManagerId, ShippingDate, CreatedDate, Updated, Available, PictureId, SecondHand, Valid
	FROM InternetUser.Catalogue as Catalogue
	--LEFT OUTER JOIN Manufacturers_CTE as Manufacturers ON Catalogue.ManufacturerId = Manufacturers.Id
	--LEFT OUTER JOIN Category1_CTE as Category1 ON Catalogue.Category1Id = Category1.Id
	--LEFT OUTER JOIN Category2_CTE as Category2 ON Catalogue.Category2Id = Category2.Id
	--LEFT OUTER JOIN Category3_CTE as Category3 ON Catalogue.Category3Id = Category3.Id
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
		  1 = CASE WHEN @PriceFilterRelation = 1 AND Price5 < CONVERT(INT, @PriceFilter) THEN 0 ELSE 1 END AND
		  1 = CASE WHEN @PriceFilterRelation = 2 AND Price5 > CONVERT(INT, @PriceFilter) THEN 0 ELSE 1 END AND
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
	CASE WHEN @Sequence =  0 THEN --átlagos életkor csökkenõ, akciós csökkenõ, gyártó növekvõ, termékazonosító szerint növekvõleg,
		Sequence0 END ASC,
	CASE WHEN @Sequence =  1 THEN -- átlagos életkor növekvõ, akciós csökkenõ, gyártó növekvõ, termékazonosító szerint növekvõleg,
		Sequence0 END DESC, 
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
	OFFSET (@CurrentPageIndex-1) * @ItemsOnPage ROWS
	FETCH NEXT @ItemsOnPage ROWS ONLY;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueSelect TO InternetUser
/*
EXEC InternetUser.CatalogueSelect @DataAreaId = '',		
								  @Manufacturers = '',			-- A098,A142
								  @Category1 = '',				--  B004,B021									      
								  @Category2 = '',
								  @Category3 = '',
								  @Discount = 0,      
								  @SecondHand = 0,     
								  @New = 1,         
								  @Stock = 0,     
								  @Sequence = 0,	
								  @FindText = '', 
								  @PriceFilter = '0',
								  @PriceFilterRelation = 0,	
								  @CurrentPageIndex = 1, 
								  @ItemsOnPage = 50

EXEC InternetUser.CatalogueSelect @Sequence = 0

update InternetUser.Catalogue set New = 1 WHERE Description like '%new%' Category1Id = 'B011'


*/

GO
-- cikkek rendezés beállítása
DROP PROCEDURE [InternetUser].[UpdateCatalogueSequence];
GO
CREATE PROCEDURE [InternetUser].[UpdateCatalogueSequence]
AS

	SET NOCOUNT ON;
	UPDATE InternetUser.Catalogue SET Sequence0 = NULL;

	WITH Sequence0_CTE (Id, Sequence, AverageInventory, Discount, Category1Id)
	AS (
		SELECT Id, ROW_NUMBER() OVER (ORDER BY AverageInventory DESC), AverageInventory, Discount, Category1Id 
		FROM InternetUser.Catalogue WHERE Category1Id = 'B011'
	)
	UPDATE InternetUser.Catalogue SET Sequence0 = (SELECT Sequence FROM Sequence0_CTE WHERE InternetUser.Catalogue.Id = Sequence0_CTE.Id)
	WHERE Discount = 1 AND AverageInventory > 0 AND InnerStock + OuterStock > 0;

	DECLARE @RowNumber INT = (SELECT MAX(Sequence0) FROM InternetUser.Catalogue);  

	WITH Remain_CTE (Id, Sequence, AverageInventory, Discount, Category1Id)
	AS (
		SELECT Id, ROW_NUMBER() OVER (ORDER BY AverageInventory DESC, Discount DESC), AverageInventory, Discount, Category1Id 
		FROM InternetUser.Catalogue WHERE Sequence0 IS NULL
	)

	UPDATE InternetUser.Catalogue SET Sequence0 = (SELECT Sequence + @RowNumber FROM Remain_CTE WHERE InternetUser.Catalogue.Id = Remain_CTE.Id)
	WHERE Sequence0 IS NULL;

RETURN
GO
GRANT EXECUTE ON InternetUser.UpdateCatalogueSequence TO InternetUser
GO
-- EXEC  [InternetUser].[UpdateCatalogueSequence]
-- SELECT * FROM InternetUser.Catalogue where Sequence0 is null ORDER BY Sequence0
	
/*

	DECLARE @str nvarchar(max) = 'hello, hello2, hello3', @sep nvarchar(1) = ',';
	WITH CTE (i, j ) AS 
	(
		SELECT CAST(0 AS INT) as i, CHARINDEX( @sep, @str) as j
		UNION ALL
		SELECT CONVERT(INT, j + 1), CHARINDEX(@sep, @str, j + 1)
		FROM CTE
		WHERE j > 0
	)
	SELECT SUBSTRING(@str, i, COALESCE(NULLIF(j, 0), LEN(@str) + 1) - i) as value
	FROM CTE */