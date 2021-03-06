USE [Axdb]
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
		   ct.ItemId,
		   InternetUser.CalculateWebStock(ct.ItemId, ind.InventLocationId, ct.DataAreaId) as AvailPhysical,   --SUM(stock.AvailPhysical) as AvailPhysical2,
		   --SUM(Stock.AvailPhysical), 
		   ct.DataAreaId, 
		   ISNULL(ind.InventLocationId, '') as InventLocationId, 
		   ind.ConfigId, 
		   invent.ItemState as ItemState
--		   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'ItemId', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as ItemIdChanged, 
--		   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'AvailPhysical', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AvailPhysicalChanged, 
--		   ct.SYS_CHANGE_OPERATION as Operation
	FROM changetable(changes Axdb.dbo.INVENTSUM, @LastVersion) as ct
	INNER JOIN [Axdb].[dbo].[INVENTSUM] as Stock on Stock.ItemId = ct.ItemId and Stock.InventDimId = ct.InventDimId and Stock.DataAreaId = ct.DataAreaId
	INNER JOIN Axdb.dbo.InventDim AS ind on ind.inventDimId = ct.InventDimId and 
											 ind.dataAreaId = ct.DataAreaId and 
											 ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' )
	INNER JOIN Axdb.dbo.InventTable as invent on invent.ItemId = ct.ItemId and Invent.DataAreaId = ct.DataAreaId 
											 and invent.WEBARUHAZ = 1 and invent.ITEMSTATE in ( 0, 1 )
	WHERE ct.DataAreaId IN ('hrp', 'bsc')  
		  and Stock.Closed = 0
	GROUP BY ct.ItemId, ind.InventLocationId, ind.ConfigId, ct.DataAreaId, invent.ItemState	--, ct.SYS_CHANGE_OPERATION

	--invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId

	--HAVING SUM(stock.AvailPhysical) > 0 
	ORDER BY ct.ItemId, MAX(SYS_CHANGE_VERSION)

	RETURN
			--SELECT invent.StandardConfigId, invent.ItemId, ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ), 
			--	   ind.InventLocationId, invent.DataAreaId
			--FROM Axdb.dbo.InventTable as invent
			--INNER JOIN Axdb.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
			--												 ind.dataAreaId = invent.DataAreaId and 
			--												 ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' ) -- '1000', 'BELSO', 
			--INNER JOIN Axdb.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
			--												ins.inventDimId = ind.inventDimId and 
			--												ins.ItemId = invent.ItemId and 
			--												ins.Closed = 0
			--INNER JOIN changetable(changes Axdb.dbo.INVENTSUM, 0) as ct on ct.ItemId = invent.ItemId and ct.DataAreaId = invent.DataAreaId
			--WHERE invent.WEBARUHAZ = 1 AND 
			--	  invent.ITEMSTATE in ( 0, 1 ) AND 
			--	  invent.DataAreaID IN ('bsc', 'hrp')
			--GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId



GO
GRANT EXECUTE ON InternetUser.InventSumCT TO [HRP_HEADOFFICE\AXPROXY]
GO
GRANT EXECUTE ON InternetUser.InventSumCT TO InternetUser
GO

-- select * from changetable(changes Axdb.dbo.INVENTSUM, 0) as ct

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
   select * from changetable(changes Axdb.dbo.INVENTSUM, 0) as ct
 */

DROP FUNCTION InternetUser.CalculateWebStock
GO
CREATE FUNCTION InternetUser.CalculateWebStock(	@ItemId nvarchar(20), @InventLocationId nvarchar(20), @DataAreaId nvarchar(3) )
RETURNS INT
AS
BEGIN
	DECLARE @Stock decimal(20,2)

	SELECT @Stock = SUM(ins.AvailPhysical) 
	FROM Axdb.dbo.InventTable as invent
	INNER JOIN Axdb.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
														ind.dataAreaId = invent.DataAreaId and 
														ind.InventLocationId = @InventLocationId -- in ( '7000', '2100', 'KULSO', 'HASZNALT' ) -- '1000', 'BELSO', 
	INNER JOIN Axdb.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
													ins.inventDimId = ind.inventDimId and 
													ins.ItemId = invent.ItemId and 
													ins.Closed = 0
	WHERE invent.WEBARUHAZ = 1 AND 
		  invent.ITEMSTATE in ( 0, 1 ) AND 
		  invent.DataAreaID = @DataAreaId AND 
--		  ind.InventLocationId = @InventLocationId AND 
		  invent.ItemId = @ItemId
	GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId

	SET @Stock = ISNULL(@Stock, 0);

	RETURN CONVERT(INT, @Stock);
END
GO
GRANT EXECUTE ON InternetUser.CalculateWebStock TO InternetUser
GO

-- select InternetUser.CalculateWebStock('AP4GAH326B', 'KULSO', 'hrp')			-- WNTMC	NBG416N

	/*SELECT invent.ItemId
	FROM Axdb.dbo.InventTable as invent
	INNER JOIN Axdb.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and 
														ind.dataAreaId = invent.DataAreaId and 
														ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' ) -- '1000', 'BELSO', 
	INNER JOIN Axdb.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and 
													ins.inventDimId = ind.inventDimId and 
													ins.ItemId = invent.ItemId and 
													ins.Closed = 0
	WHERE invent.WEBARUHAZ = 1 AND 
			invent.ITEMSTATE in ( 0, 1 ) AND 
			ins.AvailPhysical > 0
	GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId*/

	-- SELECT * FROM InternetUser.Catalogue where ItemState = 1 AND Stock = 0
