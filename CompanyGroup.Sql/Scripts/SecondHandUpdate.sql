/* =============================================
	description	   : srv2 Web adatbázisban SecondHand tábla szerinti update
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- használt cikkek lista
DROP PROCEDURE [InternetUser].[SecondHandUpdate];
GO
CREATE PROCEDURE [InternetUser].[SecondHandUpdate] 
AS
SET NOCOUNT ON
	-- Catalogue tábla SecondHand mezõ frissítése InternetUser.SecondHand tábla alapján
	UPDATE InternetUser.Catalogue SET SecondHand = CONVERT(bit, 1)
	FROM InternetUser.Catalogue as C 
	INNER JOIN InternetUser.SecondHand as S ON C.ProductId = S.ProductId AND C.DataAreaId = S.DataAreaId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[SecondHandUpdate] TO InternetUser