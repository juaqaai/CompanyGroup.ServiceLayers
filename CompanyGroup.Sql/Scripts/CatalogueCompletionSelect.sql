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

USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueCompletionSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueCompletionSelect] (@Prefix nvarchar(64) = '', 
															 @DataAreaId nvarchar(4) = 'hrp',
												   @Manufacturers nvarchar (250) = '',
												   @Category1 nvarchar (250) = '',
												   @Category2 nvarchar (250) = '',
												   @Category3 nvarchar (250) = '',	       
															 @Discount bit = 0,      
															 @SecondHand bit = 0,     
															 @New bit = 0,         
															 @Stock bit = 0,
															 @PriceFilter nvarchar(16) = '',
															 @PriceFilterRelation INT = 0  
)
AS
SET NOCOUNT ON

	SET ROWCOUNT 30;

	IF ISNULL(@Prefix, '') = '' 
		SET @Prefix = '""';

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

	SELECT Catalogue.Id, ProductId, DataAreaId,	Name, EnglishName, PictureId
	FROM InternetUser.Catalogue as Catalogue
	WHERE 
		(Catalogue.ManufacturerId IN (SELECT SUBSTRING(@Manufacturers, Mi, COALESCE(NULLIF(Mj, 0), LEN(@Manufacturers) + 1) - Mi) FROM Manufacturers_CTE) OR (@Manufacturers = '')) AND
		(Catalogue.Category1Id IN (SELECT SUBSTRING(@Category1, C1i, COALESCE(NULLIF(C1j, 0), LEN(@Category1) + 1) - C1i) FROM Category1_CTE) OR (@Category1 = '')) AND
	    (Catalogue.Category2Id IN (SELECT SUBSTRING(@Category2, C2i, COALESCE(NULLIF(C2j, 0), LEN(@Category2) + 1) - C2i) FROM Category2_CTE) OR (@Category2 = '')) AND
		(Catalogue.Category3Id IN (SELECT SUBSTRING(@Category3, C3i, COALESCE(NULLIF(C3j, 0), LEN(@Category3) + 1) - C3i) FROM Category3_CTE) OR (@Category3 = '')) AND
 		Discount = CASE WHEN @Discount = 1 THEN 1 ELSE Discount END AND  
		SecondHand = CASE WHEN @SecondHand = 1 THEN 1 ELSE SecondHand END AND 
		New = CASE WHEN @New = 1 THEN 1 ELSE New END AND  
		Valid = 1 AND
		1 = CASE WHEN ItemState = 1 AND Stock <= 0 THEN 0 ELSE 1 END AND 
		1 = CASE WHEN @Stock = 1 AND Stock <= 0 THEN 0 ELSE 1 END AND
		1 = CASE WHEN @PriceFilterRelation = 1 AND Price5 < CONVERT(INT, @PriceFilter) THEN 0 ELSE 1 END AND
		1 = CASE WHEN @PriceFilterRelation = 2 AND Price5 > CONVERT(INT, @PriceFilter) THEN 0 ELSE 1 END AND
		DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE DataAreaId END AND 
		-- SearchContent LIKE CASE WHEN @Prefix <> '' THEN '%' + @Prefix + '%' ELSE SearchContent END
		1 = CASE WHEN @Prefix = '""' OR CONTAINS(SearchContent, @Prefix) THEN 1 ELSE 0 END 
		--1 = CASE WHEN @FindText = '""' OR CONTAINS(SearchContent, @FindText) THEN 1 ELSE 0 END
	ORDER BY Name;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueCompletionSelect TO InternetUser
-- EXEC InternetUser.CatalogueCompletionSelect @Prefix = 'DELL', @Stock = 1;
/*
EXEC InternetUser.CatalogueCompletionSelect  @Prefix = '"ASUS*" AND "sz�m�t�g�p*"', @StructureXml = '
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