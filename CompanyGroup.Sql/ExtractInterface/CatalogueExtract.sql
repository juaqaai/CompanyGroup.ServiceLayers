/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatb�zisban Catalogue t�bla szerinti lek�rdez�s
	running script : InternetUser, Acetylsalicilum91 nev�ben
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
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueExtract];
GO
CREATE PROCEDURE [InternetUser].[CatalogueExtract] 
AS
SET NOCOUNT ON
	;
	WITH EnglishProductName_CTE(ProductId, ProductName)
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
		WHERE DataAreaId = 'hun' AND c.jelleg3Id <> '' AND c.jellegNev <> '' ), 
	Stock_CTE ( StandardConfigId, ProductId, Quantity, InventLocationId, DataAreaId )
	AS (
			SELECT invent.StandardConfigId, invent.ItemId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ), 
				   ind.InventLocationId, invent.DataAreaId
			FROM axdb_20120614.dbo.InventTable as invent
			INNER JOIN axdb_20120614.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
															ind.dataAreaId = invent.DataAreaId and 
															ind.InventLocationId in ( '1000', '7000', '2100', 'BELSO', 'KULSO', 'HASZNALT' ) 
			INNER JOIN axdb_20120614.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
															ins.inventDimId = ind.inventDimId and 
															ins.ItemId = invent.ItemId and 
															ins.Closed = 0
			WHERE invent.WEBARUHAZ = 1 AND 
				  invent.ITEMSTATE in ( 0, 1 ) AND 
				  invent.DataAreaID IN ('bsc', 'hrp')
			GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId ),
	PurchaseOrderLine_CTE (ProductId, PurchQty, DeliveryDate, ConfirmedDlv, QtyOrdered, RemainInventPhysical, RemainPurchPhysical, DataAreaId)
	AS (
		SELECT  
		--Purch.BrEngedelyezes,			-- Enged�lyez�s st�tusz, 2: Engedelyezve, 3: Nemkellengedelyezni
		Purch.ItemId, 
		CONVERT(INT, Purch.PurchQty) as PurchQty,					-- mennyis�g
		Purch.DeliveryDate,				-- k�rt sz�ll�t�si id�pont
		Purch.ConfirmedDlv,				-- visszaigazolva
		--Purch.PurchStatus,				-- sor �llpota (1:nyitott rendel�s, 2:fogadott, 3:szamlazva, 4:ervenytelenitve)
		CONVERT(INT, Purch.QtyOrdered) as QtyOrdered,				-- mennyis�g
		CONVERT(INT, Purch.RemainInventPhysical) as RemainInventPhysical,		-- fennmarad� sz�ll�t�sa
		CONVERT(INT, Purch.RemainPurchPhysical) as RemainPurchPhysical,		-- fennmarad� sz�ll�t�sa
		--( SELECT SUM(Qty) FROM Axdb.dbo.InventTrans WHERE InventTransId = Purch.InventTransId AND StatusReceipt = 5 ) as Ordered,	-- rendelt
		Purch.DataAreaId
		FROM axdb_20120614.dbo.PurchLine as Purch WITH (READUNCOMMITTED) 
		INNER JOIN axdb_20120614.dbo.InventTable as Invent WITH (READUNCOMMITTED) ON Invent.ItemId = Purch.ItemId AND Invent.DataAreaId = Purch.DataAreaId
		WHERE Purch.ConfirmedDlv > '1900-01-01' AND 
			Purch.BrEngedelyezes = 2 AND			-- enged�lyez�s st�tusz, 2: Engedelyezve, 3: Nemkellengedelyezni
			Purch.PURCHASETYPE = 3 AND	
			Purch.PurchStatus IN (1, 2) AND	
			CONVERT(INT, Purch.RemainPurchPhysical) > 0 AND
			Invent.DataAreaId IN ('bsc', 'hrp') AND 
			Invent.WEBARUHAZ = 1 AND 
			Invent.ITEMSTATE IN ( 0, 1 ) AND 
			Invent.AMOUNT1 > 0 AND
			Invent.AMOUNT2 > 0 AND
			Invent.AMOUNT3 > 0 AND
			Invent.AMOUNT4 > 0 AND
			Invent.AMOUNT5 > 0 ), 
	Description_CTE(ProductId, Txt, LanguageId)
	AS (
		SELECT ItemId, 
			   Txt, 
			   CASE LanguageId WHEN 'HU' THEN 'hun' 
			   WHEN 'en-gb' THEN 'eng'
			   ELSE '' END	 	 
		FROM axdb_20120614.dbo.INVENTTXT
		WHERE DataAreaId IN ('hrp', 'bsc') AND ItemId <> '' AND Txt <> ''
	)

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
					ISNULL(StockInner.Quantity, 0) as InnerStock, 
					ISNULL(StockOuter.Quantity, 0) as OuterStock,
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
					ISNULL(PurchaseOrderLine.DeliveryDate, CONVERT(datetime, 0)) as ShippingDate,
			        GetDate() as CreatedDate,		-- Invent.CREATEDTIME
				    GetDate() as Updated, -- Invent.ModifiedTime
					CONVERT(BIT, 1) as Available, 
					0 as PictureId, 
					CONVERT(BIT, 0) as SecondHand,
					CONVERT(BIT, 1)	as Valid		
	FROM axdb_20120614.dbo.InventTable as Invent WITH (READUNCOMMITTED) 
	LEFT OUTER JOIN EnglishProductName_CTE as EnglishProductName ON EnglishProductName.ProductId = Invent.ItemId
	LEFT OUTER JOIN Manufacturer_CTE as Manufacturer ON Manufacturer.ManufacturerId = Invent.GYARTOID
	LEFT OUTER JOIN Category1_CTE as Category1 ON Category1.CategoryId = Invent.JELLEG1ID
	LEFT OUTER JOIN Category2_CTE as Category2 ON Category2.CategoryId = Invent.JELLEG2ID
	LEFT OUTER JOIN Category3_CTE as Category3 ON Category3.CategoryId = Invent.JELLEG3ID
	LEFT OUTER JOIN Stock_CTE as StockInner ON StockInner.ProductId = Invent.ItemId AND 
										  StockInner.DataAreaId = Invent.DataAreaId AND 
										  StockInner.InventLocationId = CASE WHEN Invent.DataAreaId = 'hrp' THEN 'BELSO' ELSE '1000' END
	LEFT OUTER JOIN Stock_CTE as StockOuter ON StockOuter.ProductId = Invent.ItemId AND 
										  StockOuter.DataAreaId = Invent.DataAreaId AND 
										  StockOuter.InventLocationId = CASE WHEN Invent.DataAreaId = 'hrp' THEN 'KULSO' ELSE '7000' END
    LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASIDEJE as gt ON gt.UPDJOTALLASIDEJEID = Invent.UPDJOTALLASIDEJEID AND 
																gt.DATAAREAID = Invent.DATAAREAID
	LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASMODJA as gm ON gm.UPDJOTALLASMODJAID = Invent.UPDJOTALLASMODJAID AND 
																gm.DATAAREAID = Invent.DATAAREAID	
	LEFT OUTER JOIN Description_CTE as HunDescription ON HunDescription.ProductId = Invent.ItemId AND 
														 HunDescription.LanguageId = 'hun'
	LEFT OUTER JOIN Description_CTE as EnglishDescription ON EnglishDescription.ProductId = Invent.ItemId AND 
															 EnglishDescription.LanguageId = 'eng'	
	LEFT OUTER JOIN PurchaseOrderLine_CTE as PurchaseOrderLine ON PurchaseOrderLine.ProductId = Invent.ItemId							 

	WHERE Invent.DataAreaId IN ('bsc', 'hrp') AND 
		  Invent.WEBARUHAZ = 1 AND 
		  Invent.ITEMSTATE IN ( 0, 1 ) AND 
		  --1 = CASE WHEN Invent.ItemState = 1 AND ( @InnerStock + @OuterStock ) > 0 THEN 1 ELSE 0 END AND
		  Invent.AMOUNT1 > 0 AND
		  Invent.AMOUNT2 > 0 AND
		  Invent.AMOUNT3 > 0 AND
		  Invent.AMOUNT4 > 0 AND
		  Invent.AMOUNT5 > 0 
	ORDER BY CONVERT( bit, AKCIOS ) DESC, Invent.AtlagosKeszletkor_Szamitott DESC, JELLEG1ID, JELLEG2ID, JELLEG3ID, Invent.ItemId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[CatalogueExtract] TO InternetUser

-- EXEC [InternetUser].[CatalogueExtract];
-- select * from InternetUser.Catalogue;

-- 1. CatalogueInsert
-- 2. SecondHandCatalogueInsert
-- 3. SecondHandInsert
-- 4. PictureInsert