USE ExtractInterface
GO
/****** Object:  StoredProcedure [InternetUser].[web_GetNewsletterList]    Script Date: 2012.10.08. 6:15:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[NewsletterSelect];
GO
CREATE PROCEDURE [InternetUser].[NewsletterSelect]( @TopN INT = 0, 
													@DataAreaId nvarchar(3) = 'hrp', --vallalat azonosito
													@BusinessUnitId nvarchar(16) = '', -- hozzarendelt uzletag
													@ManufacturerId nvarchar(16) = '' -- hozzarendelt gyarto
													)
AS
BEGIN
	SET NOCOUNT ON;

	IF ( @TopN > 0 )
		SET ROWCOUNT @TopN;

	SELECT HIRLEVID AS [Id], TEMA AS [Title], 
		   MEGJEGYZES AS [Description], HTMLFILE AS [HtmlPath], LEJARATDATUMA AS [EndDate], 
		   NAGYKEP AS [PicturePath], ENGEDELYEZESDATUMA AS [AllowedDate], 
		   ENGEDELYEZESIDEJE AS [AllowedTime]  
	FROM Axdb.dbo.UPDHERLEV
	WHERE DATAAREAID = CASE WHEN @DataAreaId = '' THEN DATAAREAID ELSE @DataAreaId END AND
		  --ENGEDELYEZVE = 2 AND -- 0: osszes, 1: csak a meg nem kikuldottek, 2: csak a kikuldottek, 
		  ENGEDELYEZO <> '' AND
		  ELKULDVE = 1 AND 
		  HIRLEVID = CASE WHEN @BusinessUnitId = '' THEN HIRLEVID ELSE ( SELECT TOP 1 HIRLEVID FROM Axdb.dbo.UPDHIRLEVJELLEG WHERE TEMAID = @BusinessUnitId ) END AND
		  HIRLEVID = CASE WHEN @ManufacturerId = '' THEN HIRLEVID ELSE ( SELECT TOP 1 HIRLEVID FROM Axdb.dbo.UPDHIRLEVGYARTO WHERE GYARTONEV = @ManufacturerId ) END
		  AND WEBRE = 1
		  AND DATEDIFF(month, ENGEDELYEZESDATUMA, GETDATE()) < 4
	ORDER BY HIRLEVID DESC

END
RETURN
GO
GRANT EXECUTE ON InternetUser.NewsletterSelect TO InternetUser
GO

-- exec [InternetUser].[NewsletterSelect] 0, 'hrp', '', '';