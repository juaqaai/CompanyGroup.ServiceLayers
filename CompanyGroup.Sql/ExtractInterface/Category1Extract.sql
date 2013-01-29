/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban Catalogue tábla szerinti lekérdezés
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[Category1Extract];
GO
CREATE PROCEDURE [InternetUser].[Category1Extract] 
AS
SET NOCOUNT ON

		SELECT jelleg1id as CategoryId, 
			   jellegNev  as CategoryName
		FROM axdb_20120614.dbo.updJelleg1 WITH (READUNCOMMITTED)
		WHERE DataAreaId = 'hun' AND jelleg1id <> '' AND jellegNev <> ''
		ORDER BY CategoryId, CategoryName;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[Category1Extract] TO InternetUser

-- EXEC [InternetUser].[Category1Extract];