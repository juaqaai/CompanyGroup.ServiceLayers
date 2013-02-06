USE [Web]
GO
/****** Object:  StoredProcedure [InternetUser].[cms_CompatibilityList]    Script Date: 2012.08.22. 23:04:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.CompatibilityList;
GO
CREATE PROCEDURE InternetUser.CompatibilityList( @DataAreaId nvarchar (3), 
												 @ProductId nvarchar (20), 
												 @Reverse BIT = 0	-- alkatresz jelolese : 0 = a termekazonositohoz tartozo alkatreszeket keressuk
																		--						1 = a termekazonositohoz mint alkatreszhez tartozo termekeket keressuk
													  )
AS
SET NOCOUNT ON
	IF ( @Reverse = 0 )	-- kellékanyag
		SELECT ProductId, DataAreaId, CompatibleProductId,  CompatibilityType
		FROM InternetUser.Compatibility WITH (READUNCOMMITTED)
		WHERE DataAreaId = @DataAreaId AND ProductId = @ProductId;
	ELSE
		SELECT ProductId, DataAreaId, CompatibleProductId, CompatibilityType
		FROM InternetUser.Compatibility
		WHERE DataAreaId = @DataAreaId AND CompatibleProductId = @ProductId;		
RETURN
GO
GRANT EXECUTE ON InternetUser.CompatibilityList TO InternetUser
GO

-- EXEC InternetUser.CompatibilityList 'hrp', 'A500-1DN', 1;

/*
select count(*), Tipus from axdb.dbo.updCompatib group by Tipus -- Kellékanyag, Opció
select * from axdb.dbo.updCompatib where Tipus = 'Kellékanyag'
select * from axdb.dbo.updCompatib where Tipus = 'Opció'
*/