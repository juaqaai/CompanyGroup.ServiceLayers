	/* =============================================
	description	   : srv2 Web adatbázisban Catalogue tábla Picture.Id szerinti frissítése
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
USE Web
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[PictureUpdate];
GO
CREATE PROCEDURE [InternetUser].[PictureUpdate] 
AS
SET NOCOUNT ON
	UPDATE InternetUser.Catalogue SET InternetUser.Catalogue.PictureId = P.Id 
	FROM InternetUser.Picture as P 
	INNER JOIN InternetUser.Catalogue as C ON P.ProductId = C.ProductId AND P.[Primary] = 1;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[PictureUpdate] TO InternetUser

GO