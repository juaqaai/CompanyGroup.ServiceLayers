USE [ExtractInterface]
GO

/* bejelentkezéskor hívódik meg, ezért ha változás történik a beállításokon, akkor újra ki és be kell lépni */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.CustomerPriceGroupSelect
GO
CREATE PROCEDURE InternetUser.CustomerPriceGroupSelect( @CustomerId NVARCHAR(10) = '' )
AS
SET NOCOUNT ON

	SELECT -- StructPriceGroup.VevoSzam as CustomerId,
			Row_Number() Over(order by StructPriceGroup.GyartoId, StructPriceGroup.Jelleg1Id, StructPriceGroup.Jelleg2Id, StructPriceGroup.Jelleg3Id, PriceDiscGroup.Sorrend desc) as LineId, 
		   0 as VisitorKey, 
		   StructPriceGroup.PriceGroupID as PriceGroupId, 
		   StructPriceGroup.GyartoId as ManufacturerId, 
		   StructPriceGroup.Jelleg1Id as Category1Id, 
		   StructPriceGroup.Jelleg2Id as Category2Id, 
		   StructPriceGroup.Jelleg3Id as Category3Id, --TOP 1 PriceGroupID
		   PriceDiscGroup.Sorrend as [Order], 
		   StructPriceGroup.DataAreaId
	FROM Axdb_20130131.dbo.updStrukturaArcsoport as StructPriceGroup
	INNER JOIN Axdb_20130131.dbo.PriceDiscGroup as PriceDiscGroup ON StructPriceGroup.PriceGroupID = PriceDiscGroup.GroupID AND 
															PriceDiscGroup.DataAreaID = StructPriceGroup.DataAreaID
	WHERE StructPriceGroup.DataAreaID IN ( 'hrp', 'bsc' )
		  AND StructPriceGroup.PriceGroupID	IN ( '1', '2', '3', '4', '5', '6', 'H', 'B' ) 
		  AND StructPriceGroup.VevoSzam = CASE WHEN @CustomerId <> '' THEN @CustomerId ELSE StructPriceGroup.VevoSzam END
	ORDER BY StructPriceGroup.GyartoId, StructPriceGroup.Jelleg1Id, StructPriceGroup.Jelleg2Id, StructPriceGroup.Jelleg3Id, PriceDiscGroup.Sorrend ASC

RETURN

GO
GRANT EXECUTE ON InternetUser.CustomerPriceGroupSelect TO InternetUser
-- EXEC InternetUser.CustomerPriceGroupSelect 'V001446';	--V002020

-- select * from InternetUser.Catalogue where ManufacturerId = 'A011' and Category1Id = 'B011'

