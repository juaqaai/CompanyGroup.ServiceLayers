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
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[RepresentativeExtract];
GO
CREATE PROCEDURE [InternetUser].[RepresentativeExtract] 
AS
SET NOCOUNT ON

			SELECT EmplId as RepresentativeId, 
				   [Name], 
				   Phone, 
				   CellularPhone as Mobile, 
				   PhoneLocal as Extension, 
				   Email
			FROM Axdb_20130131.dbo.EmplTable 
			WHERE DataAreaId = 'HUN'
RETURN
GO
GRANT EXECUTE ON [InternetUser].[RepresentativeExtract] TO InternetUser

GO

-- EXEC [InternetUser].[RepresentativeExtract];