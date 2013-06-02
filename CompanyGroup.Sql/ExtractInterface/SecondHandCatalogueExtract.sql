/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban SecondHand tábla szerinti lekérdezés
					 azokat a kereskedelmi forgalomban már nem kapható cikkeket tartalmazza, melyeket csak használtan lehet megvenni
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- használt cikkek lista
DROP PROCEDURE [InternetUser].[SecondHandCatalogueExtract];
GO
CREATE PROCEDURE [InternetUser].[SecondHandCatalogueExtract] 
AS
SET NOCOUNT ON
	;
	-- XX konfiguráción HASZNALT, vagy 2100 raktárban lévõ termékek
	-- WITH XXConfig_CTE(ConfigId, InventLocationId, InventDimId, ProductId, StatusDescription, DataAreaId)
	--AS (
	--	SELECT cfg.configId, idim.InventLocationId, idim.InventDimId, cfg.ItemId, cfg.Name, cfg.dataAreaId 
	--	FROM Axdb.dbo.ConfigTable as cfg 
	--	INNER JOIN Axdb.dbo.InventDim as idim ON cfg.configId = idim.configId and 
	--													  cfg.dataAreaId = idim.dataAreaId AND 
	--													  cfg.ConfigId like 'xx%' AND 
	--													  idim.InventLocationId IN ('HASZNALT', '2100')
	--	--INNER JOIN Axdb.dbo.InventSum AS ins ON ( ins.inventDimId = idim.InventDimId AND 
	--	--												   ins.DataAreaId = idim.DataAreaId AND 
	--	--												   ins.ItemId = cfg.ItemId )
	--	WHERE cfg.dataareaid IN ('bsc', 'hrp') -- AND ins.Closed = 0 
	--), 
	-- készletek, árral, konfigurációnként és cikkenként, elérhetõ mennyiségre aggregálva
/*
	insert into @StockTable
	select c.sConfigID, c.sProductID, CONVERT( INT, SUM(ins.AvailPhysical) ), c.sDataAreaID
	from InternetUser.web_CatalogueSecondHand as c 
	inner join AxDb.dbo.InventDim AS ind on ( ind.configId = c.sConfigID and ind.dataAreaId = c.sDataAreaId )
	inner join AxDb.dbo.InventSum AS ins on ( ins.inventDimId = ind.inventDimId and ins.dataAreaId = ind.DataAreaID AND ins.ItemId = c.sProductID )
	where ins.Closed = 0 
	group by c.sConfigID, c.sProductID, c.sDataAreaID;
*/
	WITH SecondHandStock_CTE(ConfigId, InventLocationId, ProductId, Quantity, Price, StatusDescription, DataAreaId) AS
	(
		--SELECT c.ConfigId, c.InventLocationId, c.ProductId, CONVERT( INT, SUM(ins.AvailPhysical) ), InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId ), c.StatusDescription, c.DataAreaId
		--FROM XXConfig_CTE as c 
		--INNER JOIN Axdb.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId IN ('HASZNALT', '2100') )
		--INNER JOIN Axdb.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
		--WHERE ins.Closed = 0 
		--GROUP BY c.ConfigId, c.InventLocationId, c.ProductId, c.StatusDescription, c.DataAreaId

		SELECT cfg.ConfigId, idim.InventLocationId, cfg.ItemId, CONVERT(INT, ins.AvailPhysical), InternetUser.GetSecondHandPrice( cfg.DataAreaId, cfg.ItemId, cfg.ConfigId ), cfg.Name, cfg.dataAreaId
		FROM Axdb.dbo.ConfigTable as cfg 
		INNER JOIN Axdb.dbo.InventDim as idim ON cfg.configId = idim.configId and 
														cfg.dataAreaId = idim.dataAreaId 
														
		INNER JOIN Axdb.dbo.InventSum AS ins ON ( ins.inventDimId = idim.InventDimId AND ins.DataAreaId = idim.DataAreaId AND ins.ItemId = cfg.ItemId )
		WHERE cfg.dataareaid IN ('bsc', 'hrp') and ins.Closed = 0 AND ins.AvailPhysical > 0 AND   
			  cfg.ConfigId like 'xx%' AND idim.InventLocationId IN ('HASZNALT', '2100')
	),
	-- magyar és angol termékleírás
	Description_CTE(ProductId, Txt, LanguageId, DataAreaId)
	AS (
		SELECT ItemId, 
			   Txt, 
			   CASE LanguageId WHEN 'HU' THEN 'hun' 
			   WHEN 'en-gb' THEN 'eng'
			   ELSE '' END, 
			   DataAreaId 	 
		FROM Axdb.dbo.INVENTTXT
		WHERE DataAreaId IN ('hrp', 'bsc') AND ItemId <> '' AND Txt <> ''
	), 
	-- angol termékleírás 
	EnglishProductName_CTE(ProductId, ProductName, DataAreaId)
	AS (
		SELECT inventlng.ITEMID,
			   inventlng.MEGJELENITESINEV, 
			   inventlng.DataAreaId
		FROM Axdb.dbo.UPDINVENTLNG as inventlng WITH (READUNCOMMITTED) 
		INNER JOIN Axdb.dbo.InventTable as invent WITH (READUNCOMMITTED) on inventlng.ITEMID = invent.ITEMID and inventlng.LANGUAGEID = 'en-gb'
		WHERE Invent.DataAreaID IN ('hrp', 'bsc') AND 
			Invent.WEBARUHAZ = 1 AND 
			Invent.ITEMSTATE IN ( 0, 1 ) AND 
			Invent.AMOUNT1 > 0 AND
			Invent.AMOUNT2 > 0 AND
			Invent.AMOUNT3 > 0 AND
			Invent.AMOUNT4 > 0 AND
			Invent.AMOUNT5 > 0
		GROUP BY inventlng.ITEMID, inventlng.MEGJELENITESINEV, inventlng.DataAreaId	), 
	-- gyártó lekérdezése
	Manufacturer_CTE(ManufacturerId, ManufacturerName, ManufacturerEnglishName, SourceCompany)
	AS ( SELECT m.GYARTOID,
			   m.GyartoNev,
			   CASE WHEN em.MegJelenitesiNev IS NULL THEN m.GyartoNev ELSE em.MegJelenitesiNev END as ManufacturerNameEnglish, 
			   m.SourceCompany
		FROM Axdb.dbo.updGyartok as m WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN Axdb.dbo.updGyartokLng as em WITH (READUNCOMMITTED) on m.GYARTOID = em.GYARTOID and em.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND m.GYARTOID <> '' AND m.GyartoNev <> ''	),
	-- termékkategória 1 lekérdezése
	Category1_CTE(CategoryId, CategoryName, CategoryNameEnglish)
	AS (
		SELECT c.jelleg1Id, 
			   c.jellegNev,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END
		FROM Axdb.dbo.updJelleg1 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN Axdb.dbo.updJelleg1Lng as ec WITH (READUNCOMMITTED) on c.jelleg1id = ec.jelleg1id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND c.jelleg1id <> '' AND c.jellegNev <> '' ), 
	-- termékkategória 2 lekérdezése
	Category2_CTE(CategoryId, CategoryName, CategoryNameEnglish)
	AS (
		SELECT c.jelleg2Id, 
			   c.jellegNev,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END
		FROM Axdb.dbo.updJelleg2 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN Axdb.dbo.updJelleg2Lng as ec WITH (READUNCOMMITTED) on c.jelleg2Id = ec.jelleg2Id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND c.jelleg2Id <> '' AND c.jellegNev <> '' ), 
	-- termékkategória 3 lekérdezése
	Category3_CTE(CategoryId, CategoryName, CategoryNameEnglish)
	AS (
		SELECT c.jelleg3Id, 
			   c.jellegNev,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END
		FROM Axdb.dbo.updJelleg3 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN Axdb.dbo.updJelleg3Lng as ec WITH (READUNCOMMITTED) on c.jelleg3Id = ec.jelleg3Id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = 'hun' AND c.jelleg3Id <> '' AND c.jellegNev <> '' )

	SELECT DISTINCT Invent.ItemId as ProductId, 
					Invent.AXSTRUKTKOD as AxStructCode, 
					Invent.DataAreaId, 
					Invent.StandardConfigId, 
					Invent.ItemName as [Name],
					ISNULL(EnglishProductName.ProductName, '') as EnglishName, 
					Invent.GYARTOICIKKSZAM as PartNumber, 
					Invent.GYARTOID as ManufacturerId, 
					ISNULL(Manufacturer.ManufacturerName, '') as ManufacturerName,
					ISNULL(Manufacturer.ManufacturerEnglishName, '') as ManufacturerEnglishName,
					JELLEG1ID as Category1Id, 
					ISNULL(Category1.CategoryName, '') as Category1Name, 
					ISNULL(Category1.CategoryNameEnglish, '') as Category1EnglishName, 
					JELLEG2ID as Category2Id, 
					ISNULL(Category2.CategoryName, '') as Category2Name,  
					ISNULL(Category2.CategoryNameEnglish, '') as Category2EnglishName, 
					JELLEG3ID as Category3Id, 
					ISNULL(Category3.CategoryName, '') as Category3Name,  
					ISNULL(Category3.CategoryNameEnglish, '') as Category3EnglishName, 
					0 as Stock,
					Invent.AtlagosKeszletkor_Szamitott as AverageInventory, 
					CONVERT( INT, Invent.AMOUNT1 ) as Price1,
					CONVERT( INT, Invent.AMOUNT2 ) as Price2,
					CONVERT( INT, Invent.AMOUNT3 ) as Price3,
					CONVERT( INT, Invent.AMOUNT4 ) as Price4,
					CONVERT( INT, Invent.AMOUNT5 ) as Price5,
					ISNULL( gt.MEGJEGYZES, '' ) as Garanty, 
					ISNULL( gm.MEGJEGYZES, '' ) as GarantyMode,
					CONVERT( bit, Invent.AKCIOS ) as Discount, 
					CASE WHEN DATEADD( day, 30, Invent.CREATEDDATE ) >=  GETDATE() THEN CONVERT( bit, 1 ) ELSE CONVERT( bit, 0 ) END as New, 
					Invent.ItemState,	-- 0 : aktiv, 1 : kifuto, 2 : passziv	
					ISNULL(HunDescription.Txt, '' ) as [Description], 
					ISNULL(EnglishDescription.Txt, '' ) as EnglishDescription,				
				    0 as ProductManagerId,
					CONVERT(datetime, 0) as ShippingDate,
					CONVERT(BIT, 1)	as IsPurchaseOrdered, 
			        GetDate() as CreatedDate,		-- Invent.CREATEDTIME
				    GetDate() as Updated, -- Invent.ModifiedTime
					CONVERT(BIT, 0) as Available, 
					0 as PictureId, 
					CONVERT(BIT, 1) as SecondHand,
					CONVERT(BIT, 1)	as Valid	
	FROM Axdb.dbo.InventTable as Invent WITH (READUNCOMMITTED) 
	INNER JOIN Manufacturer_CTE as Manufacturer ON Manufacturer.ManufacturerId = Invent.GYARTOID AND Manufacturer.SourceCompany = Invent.DataAreaId

	INNER JOIN SecondHandStock_CTE as SecondHandStock ON SecondHandStock.ProductId = Invent.ItemId AND 
														 SecondHandStock.DataAreaId = Invent.DataAreaId 
	LEFT OUTER JOIN EnglishProductName_CTE as EnglishProductName ON EnglishProductName.ProductId = Invent.ItemId AND EnglishProductName.DataAreaId = Invent.DataAreaId
	LEFT OUTER JOIN Category1_CTE as Category1 ON Category1.CategoryId = Invent.JELLEG1ID
	LEFT OUTER JOIN Category2_CTE as Category2 ON Category2.CategoryId = Invent.JELLEG2ID
	LEFT OUTER JOIN Category3_CTE as Category3 ON Category3.CategoryId = Invent.JELLEG3ID

    LEFT OUTER JOIN Axdb.dbo.UPDJOTALLASIDEJE as gt ON gt.UPDJOTALLASIDEJEID = Invent.UPDJOTALLASIDEJEID AND 
																gt.DATAAREAID = Invent.DATAAREAID
	LEFT OUTER JOIN Axdb.dbo.UPDJOTALLASMODJA as gm ON gm.UPDJOTALLASMODJAID = Invent.UPDJOTALLASMODJAID AND 
																gm.DATAAREAID = Invent.DATAAREAID	
	LEFT OUTER JOIN Description_CTE as HunDescription ON HunDescription.ProductId = Invent.ItemId AND 
														 HunDescription.LanguageId = 'hun' AND 
														 HunDescription.DataAreaId = Invent.DataAreaId
	LEFT OUTER JOIN Description_CTE as EnglishDescription ON EnglishDescription.ProductId = Invent.ItemId AND 
															 EnglishDescription.LanguageId = 'eng' AND 
															 EnglishDescription.DataAreaId = Invent.DataAreaId	
	--LEFT OUTER JOIN InternetUser.Catalogue as Catalogue ON Catalogue.ProductId = SecondHandStock.ProductId AND 
	--													   Catalogue.DataAreaId = SecondHandStock.DataAreaId														 	

	WHERE SecondHandStock.Quantity > 0 AND SecondHandStock.Price > 0 AND Invent.ItemId NOT IN( SELECT C.ItemId FROM Axdb.dbo.InventTable as C WHERE C.DataAreaId IN ('bsc', 'hrp') AND 
																																							 C.WEBARUHAZ = 1 AND 
																																							 C.ITEMSTATE IN ( 0, 1 ) AND 
																																							  --1 = CASE WHEN Invent.ItemState = 1 AND ( @InnerStock + @OuterStock ) > 0 THEN 1 ELSE 0 END AND
																																							 C.AMOUNT1 > 0 AND
																																							 C.AMOUNT2 > 0 AND
																																							 C.AMOUNT3 > 0 AND
																																							 C.AMOUNT4 > 0 AND
																																							 C.AMOUNT5 > 0 )

RETURN
GO
GRANT EXECUTE ON [InternetUser].[SecondHandCatalogueExtract] TO [InternetUser];
GO
GRANT EXECUTE ON [InternetUser].[SecondHandCatalogueExtract] TO [HRP_HEADOFFICE\AXPROXY]
GO
-- EXEC [InternetUser].[SecondHandCatalogueExtract];
-- select * from InternetUser.Catalogue where Available = 0