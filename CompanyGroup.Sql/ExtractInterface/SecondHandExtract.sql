/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban SecondHand tábla szerinti lekérdezés
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
DROP PROCEDURE [InternetUser].[SecondHandExtract];
GO
CREATE PROCEDURE [InternetUser].[SecondHandExtract] 
AS
SET NOCOUNT ON
	-- XX konfiguráción HASZNALT, vagy 2100 raktárban lévõ termékek
	;
	--WITH XXConfig_CTE(ConfigId, InventLocationId, InventDimId, ProductId, StatusDescription, DataAreaId)
	--AS (
	--	SELECT cfg.configId, idim.InventLocationId, idim.InventDimId, cfg.ItemId, cfg.Name, cfg.dataAreaId 
	--	FROM Axdb.dbo.ConfigTable as cfg 
	--	INNER JOIN Axdb.dbo.InventDim as idim ON cfg.configId = idim.configId and 
	--													cfg.dataAreaId = idim.dataAreaId AND 
	--													cfg.ConfigId like 'xx%' AND 
	--													idim.InventLocationId IN ('HASZNALT', '2100')
	--	WHERE cfg.dataareaid IN ('bsc', 'hrp')
	--), 
	-- készletek, árral, konfigurációnként és cikkenként, elérhetõ mennyiségre aggregálva
	WITH SecondHandStock_CTE(ConfigId, InventLocationId, ProductId, Quantity, Price, StatusDescription, DataAreaId) AS
	(
		--SELECT c.ConfigId, c.InventLocationId, c.ProductId, CONVERT( INT, SUM(ins.AvailPhysical) ), InternetUser.GetSecondHandPrice( c.DataAreaId, c.ProductId, c.ConfigId ), c.StatusDescription, c.DataAreaId
		--FROM XXConfig_CTE as c 
		--INNER JOIN Axdb.dbo.InventDim AS ind ON ( ind.configId = c.ConfigId and ind.DataAreaId = c.DataAreaId AND ind.InventLocationId IN ('HASZNALT', '2100') )
		--INNER JOIN Axdb.dbo.InventSum AS ins ON ( ins.inventDimId = ind.InventDimId AND ins.DataAreaId = ind.DataAreaId AND ins.ItemId = c.ProductId )
		--WHERE ins.Closed = 0 
		--GROUP BY c.ConfigId, c.InventLocationId, c.ProductId, c.StatusDescription, c.DataAreaId

		SELECT cfg.ConfigId, idim.InventLocationId, cfg.ItemId, CONVERT(INT, ins.AvailPhysical), InternetUser.GetSecondHandPrice( cfg.DataAreaId, cfg.ItemId, cfg.ConfigId ), cfg.Name, cfg.dataAreaId
		FROM Axdb.dbo.ConfigTable as cfg 
		INNER JOIN Axdb.dbo.InventDim as idim ON cfg.configId = idim.configId and 
														cfg.dataAreaId = idim.dataAreaId 
		INNER JOIN Axdb.dbo.InventSum AS ins ON ( ins.inventDimId = idim.InventDimId AND ins.DataAreaId = idim.DataAreaId AND ins.ItemId = cfg.ItemId )
		WHERE cfg.dataareaid IN ('bsc', 'hrp') and ins.Closed = 0 AND ins.AvailPhysical > 0 AND   
			  cfg.ConfigId like 'xx%' AND idim.InventLocationId IN ('HASZNALT', '2100')
	)

	SELECT DataAreaId, ProductId, ConfigId, InventLocationId, Quantity, Price, StatusDescription, GetDate() as CreatedDate, Convert(bit, 1) as Valid
	FROM SecondHandStock_CTE 
	WHERE Quantity > 0 AND Price > 0
	ORDER BY ProductId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[SecondHandExtract] TO InternetUser
GO
GRANT EXECUTE ON [InternetUser].[SecondHandExtract] TO [HRP_HEADOFFICE\AXPROXY]
GO

-- EXEC [InternetUser].[SecondHandExtract];
GO
DROP FUNCTION [InternetUser].[GetSecondHandPrice]
GO
CREATE FUNCTION [InternetUser].[GetSecondHandPrice]( @DataAreaId nvarchar(3) = 'hrp', -- vallalatkod
													 @ProductId nvarchar(20),		  -- termekazonosito
													 @ConfigId nvarchar(20) )		  -- konfig.
RETURNS REAL	
AS
BEGIN

	DECLARE @Price real;

	SET @Price = ISNULL( ( SELECT TOP 1 pdt.Amount 
						   FROM AxDb.dbo.PriceDiscTable AS pdt 
						   INNER JOIN AxDb.dbo.InventDim AS idim on idim.inventDimId = pdt.InventDimId and pdt.DataAreaID = idim.DataAreaID
						   WHERE pdt.ItemRelation = @ProductId AND pdt.dataAreaId = @DataAreaId and idim.ConfigId = @ConfigId
						   ORDER BY pdt.Amount 
						), 0 );

	RETURN @Price;
END

GO
GRANT EXECUTE ON [InternetUser].[GetSecondHandPrice] TO InternetUser
GO
GRANT EXECUTE ON [InternetUser].[GetSecondHandPrice] TO [HRP_HEADOFFICE\AXPROXY]