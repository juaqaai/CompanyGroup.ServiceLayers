USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
visszaadja a webre kitehetõ kategóriákat
*/
DROP PROCEDURE [InternetUser].[cms_Category3List];
GO
CREATE PROCEDURE [InternetUser].[cms_Category3List]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON
	DECLARE @VirtualDataAreaId VARCHAR(3);

	SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	IF (@VirtualDataAreaId = 'hun') 
	BEGIN
		SELECT c.jelleg3id as Category3Id, 
			   c.jellegNev  as Category3Name,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END as Category3NameEnglish,
			   @DataAreaId as DataAreaId
		FROM AxDb.dbo.updJelleg3 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN AxDb.dbo.updJelleg3Lng as ec WITH (READUNCOMMITTED) on c.jelleg3id = ec.jelleg3id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = @VirtualDataAreaId AND c.jelleg3id <> '' AND c.jellegNev <> ''
		ORDER BY Category3Id, Category3Name;
	END
	ELSE
	BEGIN
		SELECT c.jelleg3id as Category3Id, 
			   c.MegJelenitesiNev  as Category3Name,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.MegJelenitesiNev ELSE ec.MegJelenitesiNev END as Category3NameEnglish,
			   @DataAreaId as DataAreaId
		FROM AxDb.dbo.updJelleg3Lng as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN AxDb.dbo.updJelleg3Lng as ec WITH (READUNCOMMITTED) on c.jelleg3id = ec.jelleg3id and ec.LanguageId = 'en-gb'
		WHERE c.LanguageId = 'en-ca' AND c.jelleg3id <> '' AND c.MegJelenitesiNev <> ''
		ORDER BY Category3Id, Category3Name;
	END
RETURN;

-- EXEC [InternetUser].[cms_Category3List] 'ser'