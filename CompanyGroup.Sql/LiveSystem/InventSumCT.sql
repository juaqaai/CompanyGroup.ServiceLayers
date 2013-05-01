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

	IF (@LastVersion = 0)
	BEGIN
		SET @LastVersion = ISNULL((SELECT MAX(LastVersion) FROM ExtractInterface.dbo.SyncMetadata WHERE TableName = 'InventSum' AND [Status] = 1), 0);
	END

	SELECT MAX(ct.SYS_CHANGE_VERSION) as [Version], 
		   stock.ItemId,
		   InternetUser.CalculateWebStock(stock.ItemId, ind.InventLocationId, stock.DataAreaId) as AvailPhysical,   -- stock.AvailPhysical as AvailPhysical,
		   --SUM(Stock.AvailPhysical), 
		   stock.DataAreaId, 
		   ISNULL(ind.InventLocationId, '') as InventLocationId, 
		   ind.ConfigId, 
		   invent.ItemState
--		   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'ItemId', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as ItemIdChanged, 
--		   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'AvailPhysical', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AvailPhysicalChanged, 
--		   ct.SYS_CHANGE_OPERATION as Operation
	FROM [Axdb_20130131].[dbo].[INVENTSUM] as Stock
	INNER JOIN changetable(changes Axdb_20130131.dbo.INVENTSUM, @LastVersion) as ct
	on Stock.ItemId = ct.ItemId and Stock.InventDimId = ct.InventDimId and Stock.DataAreaId = ct.DataAreaId
	INNER JOIN Axdb_20130131.dbo.InventDim AS ind on ind.inventDimId = stock.inventDimId and 
													      ind.dataAreaId = stock.DataAreaId and 
													      ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' )
	INNER JOIN Axdb_20130131.dbo.InventTable as invent on invent.ItemId = Stock.ItemId and Invent.DataAreaId = Stock.DataAreaId
	WHERE stock.DataAreaId IN ('hrp', 'bsc') and 
		  Stock.Closed = 0
	GROUP BY stock.ItemId, ind.InventLocationId, ind.ConfigId, stock.DataAreaId, invent.ItemState	--, ct.SYS_CHANGE_OPERATION
	--HAVING SUM(stock.AvailPhysical) > 0 
	ORDER BY MAX(SYS_CHANGE_VERSION)

	RETURN
			--SELECT invent.StandardConfigId, invent.ItemId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ), 
			--	   ind.InventLocationId, invent.DataAreaId
			--FROM Axdb_20130131.dbo.InventTable as invent
			--INNER JOIN Axdb_20130131.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
			--												 ind.dataAreaId = invent.DataAreaId and 
			--												 ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' ) -- '1000', 'BELSO', 
			--INNER JOIN Axdb_20130131.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
			--												ins.inventDimId = ind.inventDimId and 
			--												ins.ItemId = invent.ItemId and 
			--												ins.Closed = 0
			--INNER JOIN changetable(changes Axdb_20130131.dbo.INVENTSUM, 0) as ct on ct.ItemId = invent.ItemId and ct.DataAreaId = invent.DataAreaId
			--WHERE invent.WEBARUHAZ = 1 AND 
			--	  invent.ITEMSTATE in ( 0, 1 ) AND 
			--	  invent.DataAreaID IN ('bsc', 'hrp')
			--GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId



GO
GRANT EXECUTE ON InternetUser.InventSumCT TO [HRP_HEADOFFICE\AXPROXY]
GO

 select * from changetable(changes Axdb_20130131.dbo.INVENTSUM, 0) as ct

/*
  exec InternetUser.InventSumCT 0

 exec InternetUser.InventSumCT_Detailed 0

Version	ItemId	AvailPhysical	DataAreaId	InventLocationId	ConfigId	ItemState
198	K33926EU	6	hrp	KULSO	ALAP	0
200	KL1137XBAFS-HUN	14	bsc	7000	SPEC	1
206	TBC055EU	145	hrp	KULSO	ALAP	0
207	PDFTRANSF3PRO	3	bsc	7000	ALAP	0
208	AMP8H61MLXR2	0	hrp	HASZNALT	XXOEMJAV	0
220	TSB023EU	4	hrp	KULSO	ALAP	0
222	AMP5K	0	hrp	HASZNALT	XXCSEREDB	0
223	TL-PA211-8130	0	hrp	HASZNALT	XXCSEREDB	0
224	WP4545DWTF	0	hrp	HASZNALT	XXHASZKOMP	0
225	65158266	0	bsc	7000	ALAP	0
229	AODRW1814BB	0	hrp	KULSO	ALAP	0
230	LFBKP772-1	0	hrp	KULSO	ALAP	0
40910	43324408	7	hrp	KULSO	ALAP	0
48055	43865708	1	hrp	KULSO	ALAP	0
52019	43979102	7	hrp	KULSO	ALAP	0
52026	1000SXSFP	97	hrp	KULSO	ALAP	0
52026	22J-00002	0	hrp	KULSO	ALAP	0

  exec InternetUser.InventSumCT 0
  exec InternetUser.InventSumCT2 0
   select * from changetable(changes Axdb_20130131.dbo.INVENTSUM, 0) as ct
 */

DROP FUNCTION InternetUser.CalculateWebStock
GO
CREATE FUNCTION InternetUser.CalculateWebStock(	@ItemId nvarchar(20), @InventLocationId nvarchar(20), @DataAreaId nvarchar(3) )
RETURNS INT
AS
BEGIN
	DECLARE @Stock decimal(20,2)

	SELECT @Stock = SUM(ins.AvailPhysical) 
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
		  invent.DataAreaID = @DataAreaId AND 
		  ind.InventLocationId = @InventLocationId AND 
		  invent.ItemId = @ItemId
	GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId

	SET @Stock = ISNULL(@Stock, 0);

	RETURN CONVERT(INT, @Stock);
END
GO
GRANT EXECUTE ON InternetUser.CalculateWebStock TO InternetUser
GO

-- select InternetUser.CalculateWebStock('TBEU3163-01P', 'KULSO', 'hrp')			-- WNTMC	NBG416N

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

	-- SELECT * FROM InternetUser.Catalogue where ItemState = 1 AND Stock = 0
