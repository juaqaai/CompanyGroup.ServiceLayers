USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
visszaadja a webre kitehetõ kategóriákat
*/
DROP PROCEDURE [InternetUser].[cms_Category2List];
GO
CREATE PROCEDURE [InternetUser].[cms_Category2List]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON
	DECLARE @VirtualDataAreaId VARCHAR(3);

	SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	IF (@VirtualDataAreaId = 'hun') 
	BEGIN
		SELECT c.jelleg2id as Category2Id, 
			   c.jellegNev  as Category2Name,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.jellegNev ELSE ec.MegJelenitesiNev END as Category2NameEnglish,
			   @DataAreaId as DataAreaId
		FROM axdb_20120614.dbo.updJelleg2 as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updJelleg2Lng as ec WITH (READUNCOMMITTED) on c.jelleg2id = ec.jelleg2id and ec.LanguageId = 'en-gb'
		WHERE DataAreaId = @VirtualDataAreaId AND c.jelleg2id <> '' AND c.jellegNev <> ''
		ORDER BY Category2Id, Category2Name;
	END
	ELSE
	BEGIN
		SELECT c.jelleg2id as Category2Id, 
			   c.MegJelenitesiNev  as Category2Name,
			   CASE WHEN ec.MegJelenitesiNev IS NULL THEN c.MegJelenitesiNev ELSE ec.MegJelenitesiNev END as Category2NameEnglish,
			   @DataAreaId as DataAreaId
		FROM axdb_20120614.dbo.updJelleg2Lng as c WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updJelleg2Lng as ec WITH (READUNCOMMITTED) on c.jelleg2id = ec.jelleg2id and ec.LanguageId = 'en-gb'
		WHERE c.LanguageId = 'en-ca' AND c.jelleg2id <> '' AND c.MegJelenitesiNev <> ''
		ORDER BY Category2Id, Category2Name;
	END
	 
RETURN;

-- EXEC [InternetUser].[cms_Category2List] 'ser'