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
DROP PROCEDURE [InternetUser].[PictureExtract];
GO
CREATE PROCEDURE [InternetUser].[PictureExtract] 
AS
SET NOCOUNT ON

	SELECT DISTINCT MAX(P.RecId) as RecId, P.ItemId as ProductId, P.[FileName], Convert(bit, P.ElsodlegesKep) as [Primary], GetDate() as CreatedDate, Convert(bit, 1) as Valid
	FROM Axdb.dbo.UPDKEPEK as P 
	INNER JOIN Axdb.dbo.InventTable as C WITH(READUNCOMMITTED) ON C.ItemId = P.ItemId
	WHERE C.DataAreaId IN ('bsc', 'hrp') AND 
		  C.WEBARUHAZ = 1 AND 
		  C.ITEMSTATE IN ( 0, 1 ) AND 
		  --1 = CASE WHEN Invent.ItemState = 1 AND ( @InnerStock + @OuterStock ) > 0 THEN 1 ELSE 0 END AND
		  C.AMOUNT1 > 0 AND
		  C.AMOUNT2 > 0 AND
		  C.AMOUNT3 > 0 AND
		  C.AMOUNT4 > 0 AND
		  C.AMOUNT5 > 0 
	GROUP BY P.ItemId, P.[FileName], Convert(bit, P.ElsodlegesKep);

RETURN
GO
GRANT EXECUTE ON [InternetUser].[PictureExtract] TO InternetUser

GO

-- exec [InternetUser].[PictureExtract];
GO
GRANT EXECUTE ON [InternetUser].[PictureExtract] TO [HRP_HEADOFFICE\AXPROXY]
GO