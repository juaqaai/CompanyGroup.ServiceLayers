USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[cms_SecondHandList];
GO
CREATE PROCEDURE [InternetUser].[cms_SecondHandList]( @DataAreaId nvarchar(3) = 'hrp') 
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
		FROM AxDb.dbo.ConfigTable as cfg 
		INNER JOIN AxDb.dbo.InventDim as idim ON cfg.configId = idim.configId and 
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
	INNER JOIN AxDb.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END )
	INNER JOIN AxDb.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
	WHERE ins.Closed = 0 
	GROUP BY c.ConfigId, c.ProductId, c.DataAreaId;

	UPDATE @SecondHandTable SET Stock = ISNULL( tmp.Stock, 0 ), Price = InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId )
	FROM @SecondHandTable AS c 
	LEFT OUTER JOIN @StockTable AS tmp ON c.ProductId = tmp.ProdId AND c.ConfigId = tmp.ConfigId AND tmp.DataAreaId = c.DataAreaId;

	SELECT ConfigId, InventLocationId, ProductId, Stock as Quantity, Price, StatusDescription, DataAreaId FROM @SecondHandTable WHERE Stock > 0 AND Price > 0;

RETURN

-- EXEC [InternetUser].[cms_SecondHandList];