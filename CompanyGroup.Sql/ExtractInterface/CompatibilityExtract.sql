/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban SecondHand tábla szerinti lekérdezés
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
-- cikkek lista
DROP PROCEDURE [InternetUser].[CompatibilityExtract];
GO
CREATE PROCEDURE [InternetUser].[CompatibilityExtract] 
AS
SET NOCOUNT ON

	SELECT ItemId as ProductId, CompatItemId as CompatibleProductId, CASE WHEN Tipus = 'Kellékanyag' THEN 1 ELSE 2 END as CompatibilityType, DataAreaId 
	FROM axdb.dbo.updCompatib WITH (READUNCOMMITTED) 
	ORDER BY ItemId, CompatItemId

RETURN
GO
GRANT EXECUTE ON [InternetUser].[CompatibilityExtract] TO InternetUser