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
DROP PROCEDURE [InternetUser].[Category2Extract];
GO
CREATE PROCEDURE [InternetUser].[Category2Extract] 
AS
SET NOCOUNT ON

		SELECT jelleg2id as CategoryId, 
			   jellegNev  as CategoryName
		FROM Axdb_20130131.dbo.updJelleg2 WITH (READUNCOMMITTED)
		WHERE DataAreaId = 'hun' AND jelleg2id <> '' AND jellegNev <> ''
		ORDER BY CategoryId, CategoryName;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[Category2Extract] TO InternetUser

-- EXEC [InternetUser].[Category2Extract];