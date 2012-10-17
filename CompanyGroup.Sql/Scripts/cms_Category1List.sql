USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
visszaadja a webre kitehetõ kategóriákat
*/
DROP PROCEDURE [InternetUser].[cms_Category1List];
GO
CREATE PROCEDURE [InternetUser].[cms_Category1List]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON
	DECLARE @VirtualDataAreaId VARCHAR(3);

	SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	IF (@VirtualDataAreaId = 'hun') 
	BEGIN
		SELECT c.jelleg1id as Category1Id, 
			   c.jellegNev  as Category1Name,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END as Category1NameEnglish,
			   @DataAreaId as DataAreaId
		FROM AxDb.dbo.updJelleg1 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN AxDb.dbo.updJelleg1Lng as ec WITH (READUNCOMMITTED) on c.jelleg1id = ec.jelleg1id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = @VirtualDataAreaId AND c.jelleg1id <> '' AND c.jellegNev <> ''
		ORDER BY Category1Id, Category1Name;
	END
	ELSE
	BEGIN
		SELECT c.jelleg1id as Category1Id, 
			   c.MegJelenitesiNev  as Category1Name,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.MegJelenitesiNev ELSE ec.MegJelenitesiNev END as Category1NameEnglish,
			   @DataAreaId as DataAreaId
		FROM AxDb.dbo.updJelleg1Lng as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN AxDb.dbo.updJelleg1Lng as ec WITH (READUNCOMMITTED) on c.jelleg1id = ec.jelleg1id and ec.LanguageId = 'en-gb'
		WHERE c.LanguageId = 'en-ca' AND c.jelleg1id <> '' AND c.MegJelenitesiNev <> ''
		ORDER BY Category1Id, Category1Name;
	END
	 
RETURN;

GO
GRANT EXECUTE ON InternetUser.cms_Category1List TO InternetUser
GO

-- EXEC [InternetUser].[cms_Category1List] 'ser'