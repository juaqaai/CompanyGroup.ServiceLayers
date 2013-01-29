USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueBannerSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueBannerSelect] (@DataAreaId nvarchar(4) = 'hrp',
												   @Manufacturers nvarchar (250) = '',
												   @Category1 nvarchar (250) = '',
												   @Category2 nvarchar (250) = '',
												   @Category3 nvarchar (250) = ''
)
AS
SET NOCOUNT ON

	SET ROWCOUNT 50;

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
	SELECT Catalogue.Id, Catalogue.ProductId, PartNumber, DataAreaId, Name,	EnglishName, 
		   ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
		   Category1Id, Category1Name, Category1EnglishName, 
		   Category2Id, Category2Name, Category2EnglishName, 
		   Category3Id, Category3Name, Category3EnglishName,
		   InnerStock, OuterStock, 
		   Price1, Price2, Price3, Price4, Price5, 
		   PictureId, Picture.[FileName], Picture.[Primary], Picture.RecId
	FROM InternetUser.Catalogue as Catalogue
	INNER JOIN InternetUser.Picture as Picture ON Picture.Id = Catalogue.PictureId
	WHERE (Catalogue.ManufacturerId IN (SELECT SUBSTRING(@Manufacturers, Mi, COALESCE(NULLIF(Mj, 0), LEN(@Manufacturers) + 1) - Mi) FROM Manufacturers_CTE) OR (@Manufacturers = '')) AND
		  (Catalogue.Category1Id IN (SELECT SUBSTRING(@Category1, C1i, COALESCE(NULLIF(C1j, 0), LEN(@Category1) + 1) - C1i) FROM Category1_CTE) OR (@Category1 = '')) AND
	      (Catalogue.Category2Id IN (SELECT SUBSTRING(@Category2, C2i, COALESCE(NULLIF(C2j, 0), LEN(@Category2) + 1) - C2i) FROM Category2_CTE) OR (@Category2 = '')) AND
		  (Catalogue.Category3Id IN (SELECT SUBSTRING(@Category3, C3i, COALESCE(NULLIF(C3j, 0), LEN(@Category3) + 1) - C3i) FROM Category3_CTE) OR (@Category3 = '')) AND
		Catalogue.Valid = 1 AND
		1 = CASE WHEN ItemState = 1 AND InnerStock + OuterStock <= 0 THEN 0 ELSE 1 END AND 
		DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE DataAreaId END AND 
		PictureId > 0 AND 
		( InnerStock + OuterStock ) > 0
	ORDER BY Discount DESC, AverageInventory DESC;
			
RETURN
GO
GO
GRANT EXECUTE ON InternetUser.CatalogueBannerSelect TO InternetUser

-- EXEC InternetUser.CatalogueBannerSelect @DataAreaId = 'hrp', @StructureXml = '';
/*
EXEC InternetUser.CatalogueBannerSelect  @StructureXml = '
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