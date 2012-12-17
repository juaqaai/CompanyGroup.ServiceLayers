USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[cms_SecondHandList_old];
GO
CREATE PROCEDURE [InternetUser].[cms_SecondHandList_old]( @DataAreaId nvarchar(3) = 'hrp') 
AS
SET NOCOUNT ON

	-- hrp, bsc hasznalt cikkeket tartalmazo ideiglenes tablazat feltoltese
	DECLARE @SecondHandTable Table(
		ConfigId nvarchar(20),	
		InventLocationId nvarchar(10), 
		ProductId nvarchar(20),
		Stock int,
		Price int,	
		StatusDescription nvarchar(1024),	
		DataAreaId nvarchar(3));

	INSERT INTO @SecondHandTable
		SELECT DISTINCT cfg.ConfigId,
		   idim.InventLocationId, 
		   cfg.ItemId, 
		   0 as Stock, 
		   0 as Price,
		   cfg.Name,
	       cfg.DataAreaId
		FROM axdb_20120614.dbo.ConfigTable as cfg 
		INNER JOIN axdb_20120614.dbo.InventDim as idim ON cfg.configId = idim.configId and 
												 cfg.dataAreaId = idim.dataAreaId AND 
												 cfg.ConfigId like 'xx%' AND 
												 idim.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END
		WHERE cfg.dataareaid = @DataAreaId AND 
			  idim.InventLocationId = CASE WHEN cfg.dataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END
		ORDER BY cfg.ItemId;

	/*
	ideiglenes keszlet tabla feltoltese
	*/
	DECLARE @StockTable Table( ConfigId nvarchar(20), ProdId nvarchar(20), Stock int, DataAreaId nvarchar(3) );

	INSERT INTO @StockTable
	SELECT c.ConfigId, c.ProductId, CONVERT( INT, SUM(ins.AvailPhysical) ), c.DataAreaId
	FROM @SecondHandTable as c 
	INNER JOIN axdb_20120614.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END )
	INNER JOIN axdb_20120614.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
	WHERE ins.Closed = 0 
	GROUP BY c.ConfigId, c.ProductId, c.DataAreaId;

	UPDATE @SecondHandTable SET Stock = ISNULL( tmp.Stock, 0 ), Price = InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId )
	FROM @SecondHandTable AS c 
	LEFT OUTER JOIN @StockTable AS tmp ON c.ProductId = tmp.ProdId AND c.ConfigId = tmp.ConfigId AND tmp.DataAreaId = c.DataAreaId;

	SELECT ConfigId, InventLocationId, ProductId, Stock as Quantity, Price, StatusDescription, DataAreaId FROM @SecondHandTable WHERE Stock > 0 AND Price > 0;

RETURN

-- EXEC [InternetUser].[cms_SecondHandList_old];

--DROP FUNCTION InternetUser.cms_GetInventSumAvailPhysical
--GO
--CREATE FUNCTION InternetUser.cms_GetInventSumAvailPhysical(@configId nvarchar(20), @inventDimId nvarchar(20), @productId nvarchar(20), @dataAreaId nvarchar(3) ) RETURNS INT
--AS
--BEGIN
--	DECLARE @Stock INT = 0;

--	SET @Stock = ISNULL((SELECT SUM(AvailPhysical) FROM axdb_20120614.dbo.InventSum WHERE InventDimId = @inventDimId AND 
--																	 DataAreaId = @dataAreaId AND 
--																	 ItemId = @productId AND 
--																	 Closed = 0), 0);

--	RETURN @Stock;
--END

-- EXEC [InternetUser].[cms_SecondHandList];

GO
DROP PROCEDURE [InternetUser].[cms_SecondHandProductList];
GO
CREATE PROCEDURE [InternetUser].[cms_SecondHandProductList]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON
	;
	-- XX konfiguráción HASZNALT, vagy 2100 raktárban lévõ termékek
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
	-- készletek, árral, konfigurációnként és cikkenként, elérhetõ mennyiségre aggregálva
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
	INNER JOIN Stock_CTE as cte ON cte.ProductId = Invent.ItemId AND cte.DataAreaId = Invent.DataAreaId
    LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASIDEJE as gt ON gt.UPDJOTALLASIDEJEID = Invent.UPDJOTALLASIDEJEID AND gt.DATAAREAID = Invent.DATAAREAID
	LEFT OUTER JOIN axdb_20120614.dbo.UPDJOTALLASMODJA as gm ON gm.UPDJOTALLASMODJAID = Invent.UPDJOTALLASMODJAID AND gm.DATAAREAID = Invent.DATAAREAID	
	WHERE Invent.DataAreaID = @DataAreaId AND 
		  cte.Quantity > 0 AND 
		  cte.Price > 0 
	ORDER BY Invent.AtlagosKeszletkor_Szamitott DESC, CONVERT( bit, AKCIOS ) ASC, JELLEG1ID, JELLEG2ID, JELLEG3ID, Invent.ItemId;

RETURN;

-- exec [InternetUser].[cms_SecondHandProductList] 'hrp'