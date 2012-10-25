USE [WebDb_Test]
GO

/****** Object:  StoredProcedure [InternetUser].[CustomerPriceGroups]    Script Date: 2012.10.25. 6:48:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.cms_CustomerPriceGroups
GO
CREATE PROCEDURE InternetUser.cms_CustomerPriceGroups( @CustomerId NVARCHAR(10) = '' , @DataAreaId VARCHAR(3) = '' )
AS
SET NOCOUNT ON

	--DECLARE @VirtualDataAreaId VARCHAR(3);

	--SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	SELECT -- StructPriceGroup.VevoSzam as CustomerId, 
		   StructPriceGroup.PriceGroupID as PriceGroupId, 
		   StructPriceGroup.GyartoId as ManufacturerId, StructPriceGroup.Jelleg1Id as Category1Id, 
		   StructPriceGroup.Jelleg2Id as Category2Id, StructPriceGroup.Jelleg3Id as Category3Id, --TOP 1 PriceGroupID
		   PriceDiscGroup.Sorrend as [Order]
	FROM axdb_20120614.dbo.updStrukturaArcsoport as StructPriceGroup
	INNER JOIN axdb_20120614.dbo.PriceDiscGroup as PriceDiscGroup ON StructPriceGroup.PriceGroupID = PriceDiscGroup.GroupID AND 
															PriceDiscGroup.DataAreaID = StructPriceGroup.DataAreaID
	WHERE StructPriceGroup.DataAreaID IN ( 'hrp', 'bsc' )
		  AND StructPriceGroup.PriceGroupID	IN ( '1', '2', '3', '4', '5', '6', 'H', 'B' ) 
		  AND StructPriceGroup.VevoSzam = CASE WHEN @CustomerId <> '' THEN @CustomerId ELSE StructPriceGroup.VevoSzam END
	--@sAxStructCode LIKE StructPriceGroup.AxStruktKod AND 
	--StructPriceGroup.Vevoszam = @sCustAccount AND 
	ORDER BY StructPriceGroup.VevoSzam, PriceDiscGroup.Sorrend ASC

RETURN

GO

-- EXEC InternetUser.cms_CustomerPriceGroups 'V002020', 'hrp';

