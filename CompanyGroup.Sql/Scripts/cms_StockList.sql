USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[cms_StockList];
GO
CREATE PROCEDURE [InternetUser].[cms_StockList]( @DataAreaId nvarchar(3) = 'hrp') 
AS
SET NOCOUNT ON

	IF ( @DataAreaId = 'hrp' )
	BEGIN
		select invent.StandardConfigId, invent.ItemId as ProductId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ) as Quantity, 
			   ind.InventLocationId, invent.DataAreaId
		from axdb_20120614.dbo.InventTable as invent
		inner join axdb_20120614.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
														ind.dataAreaId = invent.DataAreaId and 
														ind.InventLocationId in ( 'BELSO', 'KULSO', 'HASZNALT' ) 
		inner join axdb_20120614.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
														ins.inventDimId = ind.inventDimId and 
														ins.ItemId = invent.ItemId and 
														ins.Closed = 0
		where invent.WEBARUHAZ = 1 and 
			  invent.ITEMSTATE in ( 0, 1 ) and 
			  invent.DataAreaID = @DataAreaId
		group by invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId
		order by invent.ItemId, invent.StandardConfigId, invent.DataAreaId;
	END
	ELSE
		IF ( @DataAreaId = 'bsc' )
		BEGIN
			select invent.StandardConfigId, invent.ItemId as ProductId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ) as Quantity, 
				   ind.InventLocationId, invent.DataAreaId
			from axdb_20120614.dbo.InventTable as invent
			inner join axdb_20120614.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
															ind.dataAreaId = invent.DataAreaId and 
															ind.InventLocationId in ( '1000', '7000', '2100' ) 
			inner join axdb_20120614.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
															ins.inventDimId = ind.inventDimId and 
															ins.ItemId = invent.ItemId and 
															ins.Closed = 0
			where invent.WEBARUHAZ = 1 and 
				  invent.ITEMSTATE in ( 0, 1 ) and 
				  invent.DataAreaID = @DataAreaId
			group by invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId
			order by invent.ItemId, invent.StandardConfigId, invent.DataAreaId;
		END
		ELSE
		BEGIN
			select invent.StandardConfigId, invent.ItemId as ProductId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ) as Quantity, 
				   ind.InventLocationId, invent.DataAreaId
			from axdb_20120614.dbo.InventTable as invent
			inner join axdb_20120614.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
															ind.dataAreaId = invent.DataAreaId and 
															ind.InventLocationId = 'ser'
			inner join axdb_20120614.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
													 ins.inventDimId = ind.inventDimId and 
													 ins.ItemId = invent.ItemId and 
													 ins.Closed = 0
			where invent.WEBARUHAZ = 1 AND invent.ITEMSTATE IN ( 0, 1 ) and invent.DataAreaId = @DataAreaId
			group by invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId
			order by invent.ItemId, invent.StandardConfigId, invent.DataAreaId;
		END

RETURN

-- EXEC [InternetUser].[cms_StockList] 'bsc'
