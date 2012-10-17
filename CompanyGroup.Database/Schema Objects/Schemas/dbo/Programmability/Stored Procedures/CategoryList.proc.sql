USE WebDb_Test
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[Category1List]
GO

/*
visszaadja a webre kitehető kategóriákat
*/
CREATE PROCEDURE [InternetUser].[Category1List]
AS
SET NOCOUNT ON

	(SELECT jelleg1id as Category1Id, 
		   jellegNev  as Category1Name, 
		   DataAreaId as [LangId]
	FROM AxDb.dbo.updJelleg1  WITH (READUNCOMMITTED)
	WHERE DataAreaId IN ('hun', 'ser') AND jelleg1id <> '' AND jellegNev <> '')

	UNION ALL
	(SELECT jelleg1id as Category1Id, 
		   MegjelenitesiNev  as Category1Name, 
		   'eng' as [LangId]
	FROM AxDb.dbo.updJelleg1Lng  WITH (READUNCOMMITTED)
	WHERE LanguageId = 'en-gb' AND jelleg1id <> '' AND MegjelenitesiNev <> '')
	ORDER BY Category1Id, [LangId]

RETURN;
GO
GRANT EXECUTE ON [InternetUser].[Category1List] TO InternetUser
GO
-- exec InternetUser.Category1List;

DROP PROCEDURE [InternetUser].[Category2List]
GO
/*
visszaadja a webre kitehető kategóriákat
*/
CREATE PROCEDURE [InternetUser].[Category2List]
AS
SET NOCOUNT ON

	(SELECT jelleg2id as Category2Id, 
		   jellegNev  as Category2Name, 
		   DataAreaId as [LangId]
	FROM AxDb.dbo.updJelleg2  WITH (READUNCOMMITTED)
	WHERE DataAreaId IN ('hun', 'ser') AND jelleg2id <> '' AND jellegNev <> '')

	UNION ALL
	(SELECT jelleg2id as Category2Id, 
		   MegjelenitesiNev  as Category2Name, 
		   'eng' as [LangId]
	FROM AxDb.dbo.updJelleg2Lng  WITH (READUNCOMMITTED)
	WHERE LanguageId = 'en-gb' AND jelleg2id <> '' AND MegjelenitesiNev <> '')
	ORDER BY Category2Id, [LangId]

RETURN;
GO
GRANT EXECUTE ON [InternetUser].[Category2List] TO InternetUser
GO

-- exec InternetUser.Category2List;

DROP PROCEDURE [InternetUser].[Category3List]
GO
/*
visszaadja a webre kitehető kategóriákat
*/
CREATE PROCEDURE [InternetUser].[Category3List]
AS
SET NOCOUNT ON

	(SELECT jelleg3id as Category3Id, 
		   jellegNev  as Category3Name, 
		   DataAreaId as [LangId]
	FROM AxDb.dbo.updJelleg3  WITH (READUNCOMMITTED)
	WHERE DataAreaId IN ('hun', 'ser') AND jelleg3id <> '' AND jellegNev <> '')
	UNION ALL
	(SELECT jelleg3id as Category3Id, 
		   MegjelenitesiNev  as Category3Name, 
		   'eng' as [LangId]
	FROM AxDb.dbo.updJelleg3Lng  WITH (READUNCOMMITTED)
	WHERE LanguageId = 'en-gb' AND jelleg3id <> '' AND MegjelenitesiNev <> '')
	ORDER BY Category3Id, [LangId]

RETURN;
GO
GRANT EXECUTE ON [InternetUser].[Category3List] TO InternetUser
GO

-- exec InternetUser.Category3List;