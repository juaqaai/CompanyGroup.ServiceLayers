DROP PROCEDURE InternetUser.StockList
GO
CREATE PROCEDURE InternetUser.StockList( @DataAreaId nvarchar(3)) 
AS
SET NOCOUNT ON

--
--	IF ( @LocationID <> '' )
--	BEGIN
--		SET @rAmount = ISNULL( ( SELECT SUM( AvailPhysical ) 
--								 FROM AxDb20101004.dbo.InventSum AS ins
--								 INNER JOIN AxDb20101004.dbo.InventDim AS ind ON ( ind.inventDimId = ins.inventDimId AND ind.dataAreaId = ins.dataAreaId )     
--								 INNER JOIN AxDb20101004.dbo.InventTable AS it on ( it.ItemId = ins.ItemId AND it.dataAreaId = ins.dataAreaId AND ind.configId = it.StandardConfigId )
--								 WHERE ins.Closed = 0 AND ins.dataAreaId = @DataAreaID AND ins.ItemId = @ProductId AND 
--									   ind.INVENTLOCATIONID = @LocationId ), 0 ); -- ind.INVENTLOCATIONID
--	END
--	ELSE
--	BEGIN
--		SET @rAmount = ISNULL( ( SELECT SUM( AvailPhysical ) 
--								 FROM AxDb20101004.dbo.InventSum AS ins
--								 INNER JOIN AxDb20101004.dbo.InventDim AS ind ON ( ind.inventDimId = ins.inventDimId AND ind.dataAreaId = ins.dataAreaId )     
--								 INNER JOIN AxDb20101004.dbo.InventTable it on ( it.ItemId = ins.ItemId AND it.dataAreaId = ins.dataAreaId AND ind.configId = it.StandardConfigId )
--								 WHERE ins.Closed = 0 AND ins.dataAreaId = @DataAreaId AND ins.ItemId = @ProductId AND 
--									   ( ind.INVENTLOCATIONID = CASE WHEN ins.dataAreaId = 'hrp' THEN 'BELSO' ELSE '1000' END 
--										 OR ind.INVENTLOCATIONID = CASE WHEN ins.dataAreaId = 'hrp' THEN 'KULSO' ELSE '7000' END ) ), 0 ); -- ind.INVENTLOCATIONID
--	END
--	RETURN CONVERT( INT, @rAmount );

	IF ( @DataAreaId = 'hrp' )
	BEGIN
		select invent.StandardConfigId, invent.ItemId as ProductId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ) as Quantity, 
			   ind.InventLocationId, invent.DataAreaId
		from AxDb.dbo.InventTable as invent
		inner join AxDb.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
														ind.dataAreaId = invent.DataAreaId and 
														ind.InventLocationId in ( 'BELSO', 'KULSO', 'HASZNALT' ) 
		inner join AxDb.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
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
			from AxDb.dbo.InventTable as invent
			inner join AxDb.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
															ind.dataAreaId = invent.DataAreaId and 
															ind.InventLocationId in ( '1000', '7000', '2100' ) 
			inner join AxDb.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
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
			from AxDb.dbo.InventTable as invent
			inner join AxDb.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
															ind.dataAreaId = invent.DataAreaId and 
															ind.InventLocationId = 'ser'
			inner join AxDb.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
													 ins.inventDimId = ind.inventDimId and 
													 ins.ItemId = invent.ItemId and 
													 ins.Closed = 0
			where invent.WEBARUHAZ = 1 AND invent.ITEMSTATE IN ( 0, 1 ) and invent.DataAreaId = @DataAreaId
			group by invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId
			order by invent.ItemId, invent.StandardConfigId, invent.DataAreaId;
		END

RETURN
GO
GRANT EXECUTE ON InternetUser.StockList TO InternetUser
GO

-- exec InternetUser.StockList 'ser';