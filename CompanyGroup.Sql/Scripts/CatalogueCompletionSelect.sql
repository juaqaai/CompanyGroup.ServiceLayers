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

*/

USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueCompletionSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueCompletionSelect] (@Prefix nvarchar(16) = '', 
															 @DataAreaId nvarchar(4) = 'hrp',
															 @StructureXml nvarchar (4000) = '',	       
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

	DECLARE @Xml Xml = CONVERT(Xml, @StructureXml);

	SET ROWCOUNT 30;

	WITH Manufacturers_CTE(Id)
	AS(
		SELECT CONVERT(nvarchar(4), Manufacturer.Id.query('./text()'))
		FROM @Xml.nodes('/Structure/Manufacturer/Id') as Manufacturer(Id)
	),
	Category1_CTE(Id)
	AS(
		SELECT CONVERT(nvarchar(4), Category1.Id.query('./text()'))
		FROM @Xml.nodes('/Structure/Category1/Id') as Category1(Id)
	),
	Category2_CTE(Id)
	AS(
		SELECT CONVERT(nvarchar(4), Category2.Id.query('./text()'))
		FROM @Xml.nodes('/Structure/Category2/Id') as Category2(Id)
	),
	Category3_CTE(Id)
	AS(
		SELECT CONVERT(nvarchar(4), Category3.Id.query('./text()'))
		FROM @Xml.nodes('/Structure/Category3/Id') as Category3(Id)
	)

	SELECT Catalogue.Id, ProductId, DataAreaId,	Name, EnglishName, PictureId
	FROM InternetUser.Catalogue as Catalogue
	WHERE Name Like '%' + @Prefix + '%' AND 
		1 = CASE WHEN (EXISTS (SELECT * FROM Manufacturers_CTE WHERE Catalogue.ManufacturerId IN (Manufacturers_CTE.Id))) OR ((SELECT COUNT(*) FROM Manufacturers_CTE) = 0) THEN 1 ELSE 0 END AND
		1 = CASE WHEN (EXISTS (SELECT * FROM Category1_CTE WHERE Catalogue.Category1Id IN (Category1_CTE.Id))) OR ((SELECT COUNT(*) FROM Category1_CTE) = 0) THEN 1 ELSE 0 END AND
		1 = CASE WHEN (EXISTS (SELECT * FROM Category2_CTE WHERE Catalogue.Category2Id IN (Category2_CTE.Id))) OR ((SELECT COUNT(*) FROM Category2_CTE) = 0) THEN 1 ELSE 0 END AND
		1 = CASE WHEN (EXISTS (SELECT * FROM Category3_CTE WHERE Catalogue.Category3Id IN (Category3_CTE.Id))) OR ((SELECT COUNT(*) FROM Category3_CTE) = 0) THEN 1 ELSE 0 END AND 
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
	ORDER BY Name;
			
RETURN

-- EXEC InternetUser.CatalogueCompletionSelect @Prefix = 'DELL', @Stock = 1;
/*
EXEC InternetUser.CatalogueCompletionSelect  @Prefix = 'Zy', @StructureXml = '
<Structure>
<Manufacturer>
    <Id>A142</Id>
    <Id>A169</Id>
</Manufacturer>
<Category1>
	<Id>B004</Id>
</Category1>
<Category2>
</Category2>
<Category3>
</Category3>
</Structure>'

*/