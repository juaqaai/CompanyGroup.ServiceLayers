
USE [TechnicalMetadata]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek készlet frissítés
DROP PROCEDURE [dbo].[CatalogueStockUpdate];
GO
CREATE PROCEDURE [dbo].[CatalogueStockUpdate] 
AS
SET NOCOUNT ON
	--;
	--WITH Duplicates_CTE(ItemId, AvailPhysical, DataAreaId, InventLocationId, [Version], [Status])
	--AS (
	--	SELECT stage1.ItemId, stage1.AvailPhysical, stage1.DataAreaId, stage1.InventLocationId, stage1.[Version], [Status] -- stage2.Count
	--	FROM Web.InternetUser.Stage_InventSum AS stage1
	--	INNER JOIN (
	--		SELECT ItemId, COUNT(*) AS Count
	--		FROM Web.InternetUser.Stage_InventSum
	--		GROUP BY ItemId
	--		HAVING COUNT(*) > 1
	--	) stage2 ON stage1.ItemId = stage2.ItemId
	--	--ORDER BY stage1.[Version]
	--)

	UPDATE Web.InternetUser.Stage_InventSum  SET [Status] = 0
	FROM Web.InternetUser.Stage_InventSum 
	LEFT OUTER JOIN (
	   SELECT MAX([Version]) as [Version], ItemId, DataAreaId, InventLocationId  
	   FROM Web.InternetUser.Stage_InventSum  
	   GROUP BY ItemId, DataAreaId, InventLocationId
	) as KeepRows ON
		 Web.InternetUser.Stage_InventSum.[Version] = KeepRows.[Version]
	WHERE KeepRows.[Version] IS NULL


	--SELECT * FROM Web.InternetUser.Stage_InventSum order by ItemId, Version;

	UPDATE Web.InternetUser.Catalogue SET Stock = stage.AvailPhysical 
	FROM Web.InternetUser.Catalogue as C 
	INNER JOIN Web.InternetUser.Stage_InventSum as stage ON C.DataAreaId = stage.DataAreaId and 
															C.ProductId = stage.ItemId and 
															stage.InventLocationId = CASE WHEN C.DataAreaId = 'hrp' THEN 'KULSO' ELSE '7000' END
	WHERE stage.[Status] = 1

RETURN
GO

-- select * from Web.InternetUser.Stage_InventSum

-- exec [dbo].[CatalogueStockUpdate]; 

/*
select stage1.ItemId, stage2.Count, stage1.Version
from Web.InternetUser.Stage_InventSum as stage1
inner join (
    SELECT ItemId, COUNT(*) AS Count
    FROM Web.InternetUser.Stage_InventSum
    GROUP BY ItemId
    HAVING COUNT(*) > 1
) stage2 on stage1.ItemId = stage2.ItemId


*/