USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- használt cikkek lista
DROP PROCEDURE [InternetUser].[SecondHandCatalogueInsert];
GO
CREATE PROCEDURE [InternetUser].[SecondHandCatalogueInsert] 
AS
SET NOCOUNT ON
	;
	-- XX konfiguráción HASZNALT, vagy 2100 raktárban lévõ termékek
	WITH XXConfig_CTE(ConfigId, InventLocationId, InventDimId, ProductId, StatusDescription, DataAreaId)
	AS (
		SELECT cfg.configId, idim.InventLocationId, idim.InventDimId, cfg.ItemId, cfg.Name, cfg.dataAreaId 
		FROM axdb_20120614.dbo.ConfigTable as cfg 
		INNER JOIN axdb_20120614.dbo.InventDim as idim ON cfg.configId = idim.configId and 
														cfg.dataAreaId = idim.dataAreaId AND 
														cfg.ConfigId like 'xx%' AND 
														idim.InventLocationId IN ('HASZNALT', '2100')
		WHERE cfg.dataareaid IN ('bsc', 'hrp')
	), 
	-- készletek, árral, konfigurációnként és cikkenként, elérhetõ mennyiségre aggregálva
	SecondHandStock_CTE(ConfigId, InventLocationId, ProductId, Quantity, Price, StatusDescription, DataAreaId) AS
	(
		SELECT c.ConfigId, c.InventLocationId, c.ProductId, CONVERT( INT, SUM(ins.AvailPhysical) ), InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId ), c.StatusDescription, c.DataAreaId
		FROM XXConfig_CTE as c 
		INNER JOIN axdb_20120614.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId IN ('HASZNALT', '2100') )
		INNER JOIN axdb_20120614.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
		WHERE ins.Closed = 0 
		GROUP BY c.ConfigId, c.InventLocationId, c.ProductId, c.StatusDescription, c.DataAreaId
	),
	Description_CTE(ProductId, Txt, LanguageId)
	AS (
		SELECT ItemId, 
			   Txt, 
			   CASE LanguageId WHEN 'HU' THEN 'hun' 
			   WHEN 'en-gb' THEN 'eng'
			   ELSE '' END	 	 
		FROM axdb_20120614.dbo.INVENTTXT
		WHERE DataAreaId IN ('hrp', 'bsc') AND ItemId <> '' AND Txt <> ''
	), 
	EnglishProductName_CTE(ProductId, ProductName)
	AS (
	SELECT inventlng.ITEMID,
		   inventlng.MEGJELENITESINEV 
	FROM axdb_20120614.dbo.UPDINVENTLNG as inventlng WITH (READUNCOMMITTED) 
	INNER JOIN axdb_20120614.dbo.InventTable as invent WITH (READUNCOMMITTED) on inventlng.ITEMID = invent.ITEMID and inventlng.LANGUAGEID = 'en-gb'
	WHERE Invent.DataAreaID IN ('hrp', 'bsc') AND 
		Invent.WEBARUHAZ = 1 AND 
		Invent.ITEMSTATE IN ( 0, 1 ) AND 
		Invent.AMOUNT1 > 0 AND
		Invent.AMOUNT2 > 0 AND
		Invent.AMOUNT3 > 0 AND
		Invent.AMOUNT4 > 0 AND
		Invent.AMOUNT5 > 0 ), 
	Manufacturer_CTE(ManufacturerId, ManufacturerName, ManufacturerEnglishName)
	AS (
		SELECT m.GYARTOID,
			   m.GyartoNev,
			   CASE WHEN em.MegJelenitesiNev IS NULL THEN m.GyartoNev ELSE em.MegJelenitesiNev END as ManufacturerNameEnglish
		FROM axdb_20120614.dbo.updGyartok as m WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updGyartokLng as em WITH (READUNCOMMITTED) on m.GYARTOID = em.GYARTOID and em.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND m.GYARTOID <> '' AND m.GyartoNev <> '' ),
	Category1_CTE(CategoryId, CategoryName, CategoryNameEnglish)
	AS (
		SELECT c.jelleg1Id, 
			   c.jellegNev,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END
		FROM axdb_20120614.dbo.updJelleg1 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updJelleg1Lng as ec WITH (READUNCOMMITTED) on c.jelleg1id = ec.jelleg1id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND c.jelleg1id <> '' AND c.jellegNev <> '' ), 
	Category2_CTE(CategoryId, CategoryName, CategoryNameEnglish)
	AS (
		SELECT c.jelleg2Id, 
			   c.jellegNev,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END
		FROM axdb_20120614.dbo.updJelleg2 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updJelleg2Lng as ec WITH (READUNCOMMITTED) on c.jelleg2Id = ec.jelleg2Id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND c.jelleg2Id <> '' AND c.jellegNev <> '' ), 
	Category3_CTE(CategoryId, CategoryName, CategoryNameEnglish)
	AS (
		SELECT c.jelleg3Id, 
			   c.jellegNev,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END
		FROM axdb_20120614.dbo.updJelleg3 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updJelleg3Lng as ec WITH (READUNCOMMITTED) on c.jelleg3Id = ec.jelleg3Id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND c.jelleg3Id <> '' AND c.jellegNev <> '' )

	INSERT INTO InternetUser.Catalogue
	SELECT DISTINCT Invent.ItemId, 
					Invent.AXSTRUKTKOD, 
					Invent.DataAreaId, 
					Invent.StandardConfigId, 
					Invent.ItemName,
					ISNULL(EnglishProductName.ProductName, ''), 
					Invent.GYARTOICIKKSZAM, 
					Invent.GYARTOID, 
					ISNULL(Manufacturer.ManufacturerName, ''),
					ISNULL(Manufacturer.ManufacturerEnglishName, ''),
					JELLEG1ID, 
					ISNULL(Category1.CategoryName, ''), 
					ISNULL(Category1.CategoryNameEnglish, ''), 
					JELLEG2ID, 
					ISNULL(Category2.CategoryName, ''),  
					ISNULL(Category2.CategoryNameEnglish, ''), 
					JELLEG3ID, 
					ISNULL(Category3.CategoryName, ''),  
					ISNULL(Category3.CategoryNameEnglish, ''), 
					0, 
					0,
					Invent.AtlagosKeszletkor_Szamitott, 
					CONVERT( INT, Invent.AMOUNT1 ),
					CONVERT( INT, Invent.AMOUNT2 ),
					CONVERT( INT, Invent.AMOUNT3 ),
					CONVERT( INT, Invent.AMOUNT4 ),
					CONVERT( INT, Invent.AMOUNT5 ),
					ISNULL( gt.MEGJEGYZES, '' ), 
					ISNULL( gm.MEGJEGYZES, '' ),
					CONVERT( bit, Invent.AKCIOS ), 
					CASE WHEN DATEADD( day, 30, Invent.CREATEDDATE ) >=  GETDATE() THEN CONVERT( bit, 1 ) ELSE CONVERT( bit, 0 ) END as New, 
					Invent.ItemState,	-- 0 : aktiv, 1 : kifuto, 2 : passziv	
					ISNULL(HunDescription.Txt, '' ), 
					ISNULL(EnglishDescription.Txt, '' ),				
				    0,
					CONVERT(datetime, 0),
			        GetDate(),		-- Invent.CREATEDTIME
				    GetDate(), -- Invent.ModifiedTime
					CONVERT(BIT, 0), 
					0, 
					CONVERT(BIT, 1),
					CONVERT(BIT, 1)
	FROM axdb_20120614.dbo.InventTable as Invent WITH (READUNCOMMITTED) 
	INNER JOIN SecondHandStock_CTE as SecondHandStock ON SecondHandStock.ProductId = Invent.ItemId AND 
														 SecondHandStock.DataAreaId = Invent.DataAreaId 
	LEFT OUTER JOIN EnglishProductName_CTE as EnglishProductName ON EnglishProductName.ProductId = Invent.ItemId
	LEFT OUTER JOIN Manufacturer_CTE as Manufacturer ON Manufacturer.ManufacturerId = Invent.GYARTOID
	LEFT OUTER JOIN Category1_CTE as Category1 ON Category1.CategoryId = Invent.JELLEG1ID
	LEFT OUTER JOIN Category2_CTE as Category2 ON Category2.CategoryId = Invent.JELLEG2ID
	LEFT OUTER JOIN Category3_CTE as Category3 ON Category3.CategoryId = Invent.JELLEG3ID

    LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASIDEJE as gt ON gt.UPDJOTALLASIDEJEID = Invent.UPDJOTALLASIDEJEID AND 
																gt.DATAAREAID = Invent.DATAAREAID
	LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASMODJA as gm ON gm.UPDJOTALLASMODJAID = Invent.UPDJOTALLASMODJAID AND 
																gm.DATAAREAID = Invent.DATAAREAID	
	LEFT OUTER JOIN Description_CTE as HunDescription ON HunDescription.ProductId = Invent.ItemId AND 
														 HunDescription.LanguageId = 'hun'
	LEFT OUTER JOIN Description_CTE as EnglishDescription ON EnglishDescription.ProductId = Invent.ItemId AND 
															 EnglishDescription.LanguageId = 'eng'
	LEFT OUTER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = SecondHandStock.ProductId AND 
														   Catalogue.DataAreaId = SecondHandStock.DataAreaId														 	

	WHERE SecondHandStock.Quantity > 0 AND SecondHandStock.Price > 0 AND Catalogue.ProductId IS NULL;

RETURN

-- EXEC [InternetUser].[SecondHandCatalogueInsert];
-- select * from InternetUser.Catalogue where Available = 0