USE [WebDb_Test]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.cms_InventNameEnglish
GO
CREATE PROCEDURE InternetUser.cms_InventNameEnglish( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON
	SELECT inventlng.ITEMID as ItemId,
			inventlng.MEGJELENITESINEV as ItemName,
			@DataAreaId as DataAreaId
	FROM axdb_20120614.dbo.UPDINVENTLNG as inventlng WITH (READUNCOMMITTED) 
	INNER JOIN axdb_20120614.dbo.InventTable as invent WITH (READUNCOMMITTED) on inventlng.ITEMID = invent.ITEMID and inventlng.LANGUAGEID = 'en-gb'
	WHERE Invent.DataAreaID = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE Invent.DataAreaID END AND 
		Invent.DataAreaID <> CASE WHEN @DataAreaId <> '' THEN 'Axapta' ELSE 'srv' END AND 
		Invent.WEBARUHAZ = 1 AND 
		Invent.ITEMSTATE IN ( 0, 1 ) AND 
		--1 = CASE WHEN Invent.ItemState = 1 AND ( @InnerStock + @OuterStock ) > 0 THEN 1 ELSE 0 END AND
		Invent.AMOUNT1 > 0 AND
		Invent.AMOUNT2 > 0 AND
		Invent.AMOUNT3 > 0 AND
		Invent.AMOUNT4 > 0 AND
		Invent.AMOUNT5 > 0 
	ORDER BY invent.ITEMID;
RETURN;
GO

-- EXEC  InternetUser].[cms_InventNameEnglish 'ser';

GRANT EXECUTE ON InternetUser.cms_InventNameEnglish TO InternetUser
GO


