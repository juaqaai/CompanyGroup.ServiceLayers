USE [WebDb_Test]
GO
/****** Object:  StoredProcedure [InternetUser].[PictureList]    Script Date: 2012.07.06. 10:07:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
visszaadja a webre kitehetõ képek listáját
*/
DROP PROCEDURE [InternetUser].[cms_PictureList];
GO
CREATE PROCEDURE [InternetUser].[cms_PictureList]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON

	SELECT Pic.RecId, CONVERT( BIT, Pic.ElsodlegesKep ) as [Primary], [FileName], Pic.ItemId, DataAreaId
	FROM AxDb.dbo.updKepek as Pic WITH (READUNCOMMITTED)
	INNER JOIN AxDb.dbo.InventTable as Invent WITH (READUNCOMMITTED)
	ON Pic.ItemId = Invent.ItemId
	WHERE Invent.DataAreaID = @DataAreaId AND 
			Invent.WEBARUHAZ = 1 AND 
			Invent.ITEMSTATE IN ( 0, 1 ) AND 
			Invent.AMOUNT1 > 0 AND
			Invent.AMOUNT2 > 0 AND
			Invent.AMOUNT3 > 0 AND
			Invent.AMOUNT4 > 0 AND
			Invent.AMOUNT5 > 0;
RETURN;

-- exec [InternetUser].[cms_PictureList]