USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
visszaadja a webre kitehetõ gyártókat
*/
DROP PROCEDURE [InternetUser].[cms_ManufacturerList];
GO
CREATE PROCEDURE [InternetUser].[cms_ManufacturerList]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON

	DECLARE @VirtualDataAreaId VARCHAR(3);

	SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	IF (@VirtualDataAreaId = 'hun') 
	BEGIN
		SELECT m.GYARTOID as ManufacturerId,
			   m.GyartoNev as ManufacturerName,
			   CASE WHEN em.MegJelenitesiNev IS NULL THEN m.GyartoNev ELSE em.MegJelenitesiNev END as ManufacturerNameEnglish,
			   @DataAreaId as DataAreaId
		FROM axdb_20120614.dbo.updGyartok as m WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updGyartokLng as em WITH (READUNCOMMITTED) on m.GYARTOID = em.GYARTOID and em.LanguageId = 'en-gb'
		WHERE DataAreaId = @VirtualDataAreaId AND m.GYARTOID <> '' AND m.GyartoNev <> ''
		ORDER BY ManufacturerId, ManufacturerName;
	END
	ELSE
	BEGIN
		SELECT m.GYARTOID as ManufacturerId,
			   m.MegJelenitesiNev as ManufacturerName,
			   CASE WHEN em.MegJelenitesiNev IS NULL THEN m.MegJelenitesiNev ELSE em.MegJelenitesiNev END as ManufacturerNameEnglish,
			   @DataAreaId as DataAreaId
		FROM axdb_20120614.dbo.updGyartokLng as m WITH (READUNCOMMITTED) 
		LEFT OUTER JOIN axdb_20120614.dbo.updGyartokLng as em WITH (READUNCOMMITTED) on m.GYARTOID = em.GYARTOID and em.LanguageId = 'en-gb'
		WHERE m.LanguageId = 'en-ca' AND m.GYARTOID <> '' AND m.MegJelenitesiNev <> ''
		ORDER BY ManufacturerId, ManufacturerName;
	END
RETURN;
-- EXEC  [InternetUser].[cms_ManufacturerList] 'ser';