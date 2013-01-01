USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueBannerSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueBannerSelect] (@DataAreaId nvarchar(4) = 'hrp',
														 @StructureXml nvarchar (4000) = '' 
)
AS
SET NOCOUNT ON

	DECLARE @Xml Xml = CONVERT(Xml, @StructureXml);

	SET ROWCOUNT 50;

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

	SELECT Id, ProductId, PartNumber, DataAreaId, Name,	EnglishName, InnerStock, OuterStock, Price1, Price2, Price3, Price4, Price5, PictureId
	FROM InternetUser.Catalogue as Catalogue
	WHERE 1 = CASE WHEN (EXISTS (SELECT * FROM Manufacturers_CTE WHERE Catalogue.ManufacturerId IN (Manufacturers_CTE.Id))) OR ((SELECT COUNT(*) FROM Manufacturers_CTE) = 0) THEN 1 ELSE 0 END AND
		1 = CASE WHEN (EXISTS (SELECT * FROM Category1_CTE WHERE Catalogue.Category1Id IN (Category1_CTE.Id))) OR ((SELECT COUNT(*) FROM Category1_CTE) = 0) THEN 1 ELSE 0 END AND
		1 = CASE WHEN (EXISTS (SELECT * FROM Category2_CTE WHERE Catalogue.Category2Id IN (Category2_CTE.Id))) OR ((SELECT COUNT(*) FROM Category2_CTE) = 0) THEN 1 ELSE 0 END AND
		1 = CASE WHEN (EXISTS (SELECT * FROM Category3_CTE WHERE Catalogue.Category3Id IN (Category3_CTE.Id))) OR ((SELECT COUNT(*) FROM Category3_CTE) = 0) THEN 1 ELSE 0 END AND 
		Valid = 1 AND
		1 = CASE WHEN ItemState = 1 AND InnerStock + OuterStock <= 0 THEN 0 ELSE 1 END AND 
		DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE DataAreaId END AND 
		PictureId > 0 AND 
		( InnerStock + OuterStock ) > 0
	ORDER BY Discount DESC, AverageInventory DESC;
			
RETURN

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