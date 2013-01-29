USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- korábbiak lista
-- alaphelyzet: notebook, akciós, öregkészlet
-- gyártó választás, jelleg választás: más gyártó ugyanolyan jellegeit
-- jelleg választás (gyártó választás nem): notebook, akciós, öregkészlet
DROP PROCEDURE [InternetUser].[CatalogueRecentSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueRecentSelect] (@DataAreaId nvarchar(4) = 'hrp',
														 @StructureXml nvarchar (4000) = '' 
)
AS
SET NOCOUNT ON

	DECLARE @ManufacturerCount INT;

	DECLARE @Category1Count INT;

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
	)

	SELECT @ManufacturerCount = COUNT(*) FROM Manufacturers_CTE;
	 
	SELECT @Category1Count = COUNT(*) FROM Category1_CTE;

	--(@ManufacturerCount > 0) AND (@Category1Count > 0)

	SELECT Catalogue.Id, Catalogue.ProductId, PartNumber, DataAreaId, Name,	EnglishName, 
		   ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
		   Category1Id, Category1Name, Category1EnglishName, 
		   Category2Id, Category2Name, Category2EnglishName, 
		   Category3Id, Category3Name, Category3EnglishName,
		   InnerStock, OuterStock, 
		   Price1, Price2, Price3, Price4, Price5, 
		   PictureId 
	FROM InternetUser.Catalogue as Catalogue 
	WHERE Catalogue.ManufacturerId NOT IN (Manufacturers_CTE.Id) AND 
		  Catalogue.Category1Id IN (SELECT Category1_CTE.Id FROM Category1_CTE) AND
		  Catalogue.Valid = 1 AND
		  1 = CASE WHEN ItemState = 1 AND InnerStock + OuterStock <= 0 THEN 0 ELSE 1 END AND 
		  DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE DataAreaId END AND 
		  PictureId > 0 AND 
		  ( InnerStock + OuterStock ) > 0
	ORDER BY Discount DESC, AverageInventory DESC;

RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueRecentSelect TO InternetUser