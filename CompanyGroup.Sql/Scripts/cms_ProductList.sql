USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

/*
select * from AxDb.dbo.updGyartok

select * from AxDb.dbo.updJElleg1
*/

GO
DROP PROCEDURE [InternetUser].[cms_ProductList];
GO
CREATE PROCEDURE [InternetUser].[cms_ProductList]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON
	DECLARE @VirtualDataAreaId VARCHAR(3);

	SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	-- XX konfigur�ci�n HASZNALT, vagy 2100 rakt�rban l�v� term�kek
	WITH XXConfig_CTE(ConfigId, ProductId, DataAreaId)
	AS (
		SELECT cfg.configId, cfg.ItemId, cfg.dataAreaId 
		FROM axdb_20120614.dbo.ConfigTable as cfg 
		INNER JOIN axdb_20120614.dbo.InventDim as idim ON cfg.configId = idim.configId and 
														cfg.dataAreaId = idim.dataAreaId AND 
														cfg.ConfigId like 'xx%' AND 
														idim.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END 
		WHERE cfg.dataareaid = @DataAreaId
	), 
	-- k�szletek, �rral, konfigur�ci�nk�nt �s cikkenk�nt, el�rhet� mennyis�gre aggreg�lva
	Stock_CTE(ProductId, Quantity, Price, DataAreaId) AS
	(
		SELECT c.ProductId, CONVERT( INT, SUM(ins.AvailPhysical) ), InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId ), c.DataAreaId
		FROM XXConfig_CTE as c 
		INNER JOIN axdb_20120614.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END )
		INNER JOIN axdb_20120614.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
		WHERE ins.Closed = 0 
		GROUP BY c.ProductId, c.ConfigId, c.DataAreaId
	)

	SELECT DISTINCT Invent.ItemId as ProductId, 
					Invent.AXSTRUKTKOD AS AxStructCode, 
					GYARTOID as ManufacturerId, 
					JELLEG1ID as Category1Id, 
					JELLEG2ID as Category2Id, 
					JELLEG3ID as Category3Id, 
					Invent.StandardConfigId as StandardConfigId,
					Invent.GYARTOICIKKSZAM as PartNumber, 
					Invent.ItemName, 
					ISNULL( gt.MEGJEGYZES, '' ) as GarantyTime, 
					ISNULL( gm.MEGJEGYZES, '' ) as GarantyMode,
					ItemState, 
					CONVERT( bit, AKCIOS ) as Discount, 
					CASE WHEN DATEADD( day, 30, Invent.CREATEDDATE ) >=  GETDATE() THEN CONVERT( bit, 1 ) ELSE CONVERT( bit, 0 ) END as New, 
					Invent.AtlagosKeszletkor_Szamitott as AverageInventory,
				    --Invent.TERMEKMENEDZSERID as ProductManagerId,
					CONVERT( INT, Invent.AMOUNT1 ) as Price1,
					CONVERT( INT, Invent.AMOUNT2 ) as Price2,
					CONVERT( INT, Invent.AMOUNT3 ) as Price3,
					CONVERT( INT, Invent.AMOUNT4 ) as Price4,
					CONVERT( INT, Invent.AMOUNT5 ) as Price5,
					'HUF' as Currency,
			        Invent.CREATEDDATE as CreatedDate, 
					Invent.CREATEDTIME as CreatedTime, 
				    Invent.ModifiedDate, 
					Invent.ModifiedTime, 
					Invent.DataAreaId
	FROM axdb_20120614.dbo.InventTable as Invent WITH (READUNCOMMITTED) 
	LEFT OUTER JOIN Stock_CTE as cte ON cte.ProductId = Invent.ItemId AND cte.DataAreaId = Invent.DataAreaId
    LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASIDEJE as gt ON gt.UPDJOTALLASIDEJEID = Invent.UPDJOTALLASIDEJEID AND gt.DATAAREAID = Invent.DATAAREAID
	LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASMODJA as gm ON gm.UPDJOTALLASMODJAID = Invent.UPDJOTALLASMODJAID AND gm.DATAAREAID = Invent.DATAAREAID	
	WHERE Invent.DataAreaID = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE Invent.DataAreaID END AND 
		  Invent.DataAreaID <> CASE WHEN @DataAreaId <> '' THEN 'Axapta' ELSE 'srv' END AND 
		  Invent.WEBARUHAZ = 1 AND 
		  Invent.ITEMSTATE IN ( 0, 1 ) AND 
		  --1 = CASE WHEN Invent.ItemState = 1 AND ( @InnerStock + @OuterStock ) > 0 THEN 1 ELSE 0 END AND
		  Invent.AMOUNT1 > 0 AND
		  Invent.AMOUNT2 > 0 AND
		  Invent.AMOUNT3 > 0 AND
		  Invent.AMOUNT4 > 0 AND
		  Invent.AMOUNT5 > 0 AND
		  cte.ProductId IS NULL
	ORDER BY Invent.AtlagosKeszletkor_Szamitott DESC, CONVERT( bit, AKCIOS ) ASC, JELLEG1ID, JELLEG2ID, JELLEG3ID, Invent.ItemId;
	RETURN;

-- EXEC [InternetUser].[cms_ProductList];

-- select * from AxDb.dbo.PriceDiscTable where Currency <> 'EUR' and Currency <> 'HUF'
