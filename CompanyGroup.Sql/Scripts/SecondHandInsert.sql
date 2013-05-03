USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- használt cikkek lista
DROP PROCEDURE [InternetUser].[SecondHandInsert];
GO
CREATE PROCEDURE [InternetUser].[SecondHandInsert] 
AS
SET NOCOUNT ON

	TRUNCATE TABLE InternetUser.SecondHand;

	-- XX konfiguráción HASZNALT, vagy 2100 raktárban lévõ termékek
	WITH XXConfig_CTE(ConfigId, InventLocationId, InventDimId, ProductId, StatusDescription, DataAreaId)
	AS (
		SELECT cfg.configId, idim.InventLocationId, idim.InventDimId, cfg.ItemId, cfg.Name, cfg.dataAreaId 
		FROM Axdb_20130131.dbo.ConfigTable as cfg 
		INNER JOIN Axdb_20130131.dbo.InventDim as idim ON cfg.configId = idim.configId and 
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
		INNER JOIN Axdb_20130131.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId IN ('HASZNALT', '2100') )
		INNER JOIN Axdb_20130131.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
		WHERE ins.Closed = 0 
		GROUP BY c.ConfigId, c.InventLocationId, c.ProductId, c.StatusDescription, c.DataAreaId
	)

	INSERT INTO InternetUser.SecondHand
	SELECT DataAreaId, ProductId, ConfigId, InventLocationId, Quantity, Price, StatusDescription, GetDate(), Convert(bit, 1)
	FROM SecondHandStock_CTE 
	WHERE Quantity > 0 AND Price > 0;

	-- Catalogue tábla SecondHand mezõ frissítése InternetUser.SecondHand tábla alapján
	UPDATE InternetUser.Catalogue SET SecondHand = CONVERT(bit, 1)
	FROM InternetUser.Catalogue as C 
	INNER JOIN InternetUser.SecondHand as S ON C.ProductId = S.ProductId AND C.DataAreaId = S.DataAreaId;

RETURN

-- EXEC [InternetUser].[SecondHandInsert];
-- select * from InternetUser.SecondHand
-- select * from InternetUser.Catalogue where SecondHand = 1;