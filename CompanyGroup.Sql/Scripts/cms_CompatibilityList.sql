USE [WebDb_Test]
GO
/****** Object:  StoredProcedure [InternetUser].[cms_CompatibilityList]    Script Date: 2012.08.22. 23:04:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.cms_CompatibilityList;
GO
CREATE PROCEDURE InternetUser.cms_CompatibilityList( @DataAreaId nvarchar (3), 
													 @ProductId nvarchar (20), 
													 @Part BIT = 0	-- alkatresz jelolese : 0 = a termekazonositohoz tartozo alkatreszeket keressuk
																		--						1 = a termekazonositohoz mint alkatreszhez tartozo termekeket keressuk
													  )
AS
SET NOCOUNT ON
	IF ( @Part = 0 )
		SELECT cat.ItemId, cat.DataAreaId
		FROM AxDb.dbo.updCompatib as cmp
		INNER JOIN AxDb.dbo.InventTable AS cat WITH (READUNCOMMITTED) ON cmp.CompatItemId = cat.ItemId AND cat.DataAreaId = cmp.DataAreaId
		WHERE cat.DataAreaId = @DataAreaId AND cmp.ItemId = @ProductId;
	ELSE
		SELECT cat.ItemId, cat.DataAreaId
		FROM AxDb.dbo.updCompatib as cmp
		INNER JOIN AxDb.dbo.InventTable AS cat WITH (READUNCOMMITTED) ON cmp.ItemId = cat.ItemId AND cat.DataAreaId = cmp.DataAreaId
		WHERE cat.DataAreaId = @DataAreaId AND cmp.CompatItemId = @ProductId;		
RETURN
GO
GRANT EXECUTE ON InternetUser.cms_CompatibilityList TO InternetUser
GO

-- EXEC InternetUser.cms_CompatibilityList 'hrp', 'A500-1DN', 0;