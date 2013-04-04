USE [Axdb_20130131]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.InventSumCT
GO
CREATE PROCEDURE InternetUser.InventSumCT( @LastVersion BIGINT = 0 )
AS
SET NOCOUNT ON

	SELECT ct.SYS_CHANGE_VERSION as [Version], 
		   stock.ItemId,
		   stock.AvailPhysical as AvailPhysical,
		   stock.DataAreaId, 
		   ISNULL(ind.InventLocationId, '') as InventLocationId,
--		   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'ItemId', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as ItemIdChanged, 
--		   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'AvailPhysical', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AvailPhysicalChanged, 
		   ct.SYS_CHANGE_OPERATION as Operation
	FROM [Axdb_20130131].[dbo].[INVENTSUM] as Stock
	INNER JOIN changetable(changes Axdb_20130131.dbo.INVENTSUM, @LastVersion) as ct
	on Stock.ItemId = ct.ItemId and Stock.InventDimId = ct.InventDimId and Stock.DataAreaId = ct.DataAreaId
	INNER JOIN Axdb_20130131.dbo.InventDim AS ind on ind.inventDimId = stock.inventDimId and 
													      ind.dataAreaId = stock.DataAreaId and 
													      ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' )
	WHERE stock.DataAreaId IN ('hrp', 'bsc') and 
		  Stock.Closed = 0
	--GROUP BY stock.ItemId, ind.InventLocationId, stock.DataAreaId, ct.SYS_CHANGE_OPERATION
	--HAVING SUM(stock.AvailPhysical) > 0 
	ORDER BY SYS_CHANGE_VERSION


			/*SELECT invent.ItemId
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
			GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId*/



RETURN

GO
GRANT EXECUTE ON InternetUser.InventSumCT TO InternetUser
GO

-- select * from changetable(changes Axdb_20130131.dbo.INVENTSUM, 0) as ct
-- exec InternetUser.InventSumCT 0