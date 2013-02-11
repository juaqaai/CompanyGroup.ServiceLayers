USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* webes rendelés elõtt ellenõrzi a cikket */
DROP PROCEDURE InternetUser.ProductOrderCheck
GO
CREATE PROCEDURE InternetUser.ProductOrderCheck( @ProductId nvarchar(20), @DataAreaId nvarchar(3), @OrderedQty int = 0) 
AS
SET NOCOUNT ON

	DECLARE @Ret int, @AvailableQty int, @StandardConfigId nvarchar(20), @WebShop BIT, @ItemState INT;

	SET @Ret = 0;
	SET @AvailableQty = 0;
	SET @StandardConfigId = '';
	SET @WebShop = 0;
	SET @ItemState = 0;

	--		ModuleType : készlet vagy ModuleType : vevõi rendelés
	IF (EXISTS(SELECT * FROM axdb_20120614.dbo.InventTableModule WHERE ItemId = @ProductId AND DataAreaId = @DataAreaId AND ModuleType in ( 0, 2 ) AND Blocked = 1))
	BEGIN
		SET @Ret = -5;	-- le van állítva a cikk
		SET @AvailableQty = 0;
		SELECT @Ret as ResultCode, @AvailableQty as AvailableQuantity;
		RETURN;
	END

	SELECT @StandardConfigId = invent.StandardConfigId, @WebShop = invent.WEBARUHAZ, @ItemState = invent.ITEMSTATE
	FROM axdb_20120614.dbo.InventTable as invent		
	WHERE invent.ItemId = @ProductId and invent.DataAreaID = @DataAreaId;

	IF ISNULL(@StandardConfigId, '') = ''
	BEGIN
		SET @Ret = -1;	-- nincs meg a cikk, nincs ConfigId
		SET @AvailableQty = 0;
		SELECT @Ret as ResultCode, @AvailableQty as AvailableQuantity;
		RETURN;
	END

	IF ISNULL(@WebShop, 0) = 0
	BEGIN
		SET @Ret = -2;	-- nem webes a cikk
		SET @AvailableQty = 0;
		SELECT @Ret as ResultCode, @AvailableQty as AvailableQuantity;
		
		RETURN;
	END

	IF ISNULL(@ItemState, 3) > 1 
	BEGIN
		SET @Ret = -3;	-- kifutott cikk 
		SET @AvailableQty = 0;
		SELECT @Ret as ResultCode, @AvailableQty as AvailableQuantity;
		RETURN;
	END

	DECLARE @Amount INT, @Amount2 INT;
	SET @Amount2 = 0;

	SELECT @Amount = ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 )
	FROM axdb_20120614.dbo.InventDim AS ind 
	INNER JOIN axdb_20120614.dbo.InventSum AS ins on ins.DataAreaId = ind.DataAreaId and 
													ins.inventDimId = ind.inventDimId and 
													ins.ItemId = @ProductId and 
													ins.Closed = 0
	WHERE ind.configId = @StandardConfigId and 
		  ind.dataAreaId = @DataAreaId and 
		  1 = CASE WHEN ( ind.InventLocationId in ( '1000', '7000' ) and @DataAreaId = 'bsc') or 
			 		    ( ind.InventLocationId in ( 'BELSO', 'KULSO' ) and @DataAreaId = 'hrp') THEN 1 ELSE 0 END

	--group by invent.StandardConfigId;

	IF (@DataAreaId = 'ser')
	BEGIN
		SELECT @Amount2 = ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 )
		FROM axdb_20120614.dbo.InventDim AS ind
		INNER JOIN axdb_20120614.dbo.InventSum AS ins on ins.DataAreaId = ind.DataAreaId and 
														ins.inventDimId = ind.inventDimId and 
														ins.ItemId = @ProductId and 
														ins.Closed = 0
		WHERE ind.configId = @StandardConfigId and 
			  ind.DataAreaId = @DataAreaId and 
			  ind.InventLocationId IN ('SER')
		-- group by invent.StandardConfigId, ind.InventLocationId;
	END

	IF ( (@ItemState = 1) AND (@OrderedQty > ISNULL(@Amount, 0) + @Amount2) )
	BEGIN
		SET @Ret = -4;	-- kifuto cikk és nincs elegendõ

		SET @AvailableQty = ISNULL(@Amount, 0) + @Amount2;

		SELECT @Ret as ResultCode, @AvailableQty as AvailableQuantity;

		RETURN;
	END

	SET @Ret = 1;

	SET @AvailableQty = ISNULL(@Amount, 0) + @Amount2;

	SELECT @Ret as ResultCode, @AvailableQty as AvailableQuantity;

RETURN
GO
GRANT EXECUTE ON InternetUser.ProductOrderCheck TO InternetUser
GO

/*
exec InternetUser.ProductOrderCheck 'EB1900', 'hrp', 0;

exec InternetUser.ProductOrderCheck 'LX-300+II', 'ser', 0;
*/

/*
select * from AxDb.dbo.InventTableModule where ModuleType in ( 0, 2 ) and Blocked = 1

select top 1000 * from AxDb.dbo.InventTableModule where ModuleType = 2 and Blocked = 1
*/

