USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- használt cikkek lista
DROP PROCEDURE [InternetUser].[cms_SecondHandList];
GO
CREATE PROCEDURE [InternetUser].[cms_SecondHandList]( @DataAreaId nvarchar(3) = 'hrp') 
AS
SET NOCOUNT ON
	;
	-- XX konfiguráción HASZNALT, vagy 2100 raktárban lévõ termékek
	WITH XXConfig_CTE(ConfigId, InventLocationId, InventDimId, ProductId, StatusDescription, DataAreaId)
	AS (
		SELECT cfg.configId, idim.InventLocationId, idim.InventDimId, cfg.ItemId, cfg.Name, cfg.dataAreaId 
		FROM Axdb_20130131.dbo.ConfigTable as cfg 
		INNER JOIN Axdb_20130131.dbo.InventDim as idim ON cfg.configId = idim.configId and 
														cfg.dataAreaId = idim.dataAreaId AND 
														cfg.ConfigId like 'xx%' AND 
														idim.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END 
		WHERE cfg.dataareaid = @DataAreaId
	), 
	-- készletek, árral, konfigurációnként és cikkenként, elérhetõ mennyiségre aggregálva
	Stock_CTE(ConfigId, InventLocationId, ProductId, Quantity, Price, StatusDescription, DataAreaId) AS
	(
		SELECT c.ConfigId, c.InventLocationId, c.ProductId, CONVERT( INT, SUM(ins.AvailPhysical) ), InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId ), c.StatusDescription, c.DataAreaId
		FROM XXConfig_CTE as c 
		INNER JOIN Axdb_20130131.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId = CASE WHEN @DataAreaId = 'hrp' THEN 'HASZNALT' ELSE '2100' END )
		INNER JOIN Axdb_20130131.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
		WHERE ins.Closed = 0 
		GROUP BY c.ConfigId, c.InventLocationId, c.ProductId, c.StatusDescription, c.DataAreaId
	)

	SELECT * FROM Stock_CTE WHERE Quantity > 0 AND Price > 0;
	-- ConfigId, InventLocationId, ProductId, Stock as Quantity, Price, StatusDescription, DataAreaId
	--SELECT * FROM Stock_CTE AS config
	--LEFT OUTER JOIN XXConfig_CTE AS stock ON config.ProductId = stock.ProductId AND config.ConfigId = stock.ConfigId AND config.DataAreaId = stock.DataAreaId;
RETURN

-- EXEC [InternetUser].[cms_SecondHandList]