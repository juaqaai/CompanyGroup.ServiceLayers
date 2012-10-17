USE WebDb_Test
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[ManufacturerList]
GO
/*
visszaadja a webre kitehető gyártókat
*/
CREATE PROCEDURE [InternetUser].[ManufacturerList]
AS
SET NOCOUNT ON

	(SELECT GYARTOID as ManufacturerId,
		   GyartoNev as ManufacturerName,
		   DataAreaId as [LangId]
	FROM AxDb.dbo.updGyartok  WITH (READUNCOMMITTED)
	WHERE DataAreaId IN ('hun', 'ser') AND GYARTOID <> '' AND GyartoNev <> '')
	UNION ALL
	(SELECT GYARTOID as ManufacturerId,
		   MegjelenitesiNev as ManufacturerName,
		   'eng' as [LangId]
	FROM AxDb.dbo.updGyartokLng  WITH (READUNCOMMITTED)
	WHERE LanguageId = 'en-gb' AND GYARTOID <> '' AND MegjelenitesiNev <> '')
	ORDER BY ManufacturerId, [LangId]
RETURN;
GO
GRANT EXECUTE ON [InternetUser].[ManufacturerList] TO InternetUser
GO
-- exec InternetUser.ManufacturerList;