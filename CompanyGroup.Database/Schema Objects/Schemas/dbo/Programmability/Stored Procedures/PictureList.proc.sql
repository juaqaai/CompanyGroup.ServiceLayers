USE WebDb_Test
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[PictureList]
GO
/*
visszaadja a webre kitehető képek listáját
*/
CREATE PROCEDURE [InternetUser].[PictureList]
AS
SET NOCOUNT ON

	SELECT Pic.RecId, CONVERT( BIT, Pic.ElsodlegesKep ) as [Primary], [FileName], Pic.ItemId, DataAreaId
	FROM AxDb.dbo.updKepek as Pic WITH (READUNCOMMITTED)
	INNER JOIN AxDb.dbo.InventTable as Invent WITH (READUNCOMMITTED)
	ON Pic.ItemId = Invent.ItemId
	WHERE Invent.DataAreaID IN ('hun', 'bsc', 'ser') AND 
		  Invent.WEBARUHAZ = 1 AND 
		  Invent.ITEMSTATE IN ( 0, 1 ) AND 
		  Invent.AMOUNT1 > 0 AND
		  Invent.AMOUNT2 > 0 AND
		  Invent.AMOUNT3 > 0 AND
		  Invent.AMOUNT4 > 0 AND
		  Invent.AMOUNT5 > 0 AND
		  Invent.AMOUNT6 > 0;
RETURN;
GO
GRANT EXECUTE ON [InternetUser].[PictureList] TO InternetUser
GO
-- exec InternetUser.PictureList;