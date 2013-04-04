USE Axdb_20130131
GO
SET ANSI_NULLS ON
GO
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET NUMERIC_ROUNDABORT OFF
SET ARITHABORT ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[StockNotifier];
GO
CREATE PROCEDURE [InternetUser].[StockNotifier] (@DataAreaId nvarchar(3), @InventLocationId nvarchar(20), @ProductId nvarchar(20))
AS
SET NOCOUNT ON

			SELECT invent.ItemId
			FROM Axdb_20130131.dbo.InventTable as invent
			INNER JOIN Axdb_20130131.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
															 ind.dataAreaId = invent.DataAreaId and 
															 ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' ) -- '1000', 'BELSO', 
			INNER JOIN Axdb_20130131.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
															ins.inventDimId = ind.inventDimId and 
															ins.ItemId = invent.ItemId and 
															ins.Closed = 0
			WHERE invent.WEBARUHAZ = 1 AND 
				  invent.ITEMSTATE in ( 0, 1 ) AND 
				  ins.AvailPhysical > 0
				  --invent.DataAreaID = @DataAreaId AND 
				  --ind.InventLocationId = @InventLocationId AND 
				  -- invent.ItemId = @ProductId
			GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId

RETURN
GO