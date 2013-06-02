/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban Catalogue tábla szerinti lekérdezés
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[StockExtract];
GO
CREATE PROCEDURE [InternetUser].[StockExtract] (@DataAreaId nvarchar(3), @InventLocationId nvarchar(20), @ProductId nvarchar(20))
AS
SET NOCOUNT ON
	;			
			SELECT ISNULL( CONVERT( INT, SUM(ins.AvailPhysical) ), 0 ) as Stock
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
				  invent.DataAreaID = @DataAreaId AND 
				  ind.InventLocationId = @InventLocationId AND 
				  invent.ItemId = @ProductId
			GROUP BY invent.StandardConfigId, invent.ItemId, ind.InventLocationId, invent.DataAreaId

RETURN
GO
GRANT EXECUTE ON [InternetUser].[StockExtract] TO InternetUser

-- exec [InternetUser].[StockExtract] 'hrp', 'KULSO', 'WN820BAG'