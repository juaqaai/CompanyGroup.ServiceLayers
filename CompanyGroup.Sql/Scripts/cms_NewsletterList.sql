USE WebDb_Test
GO
/****** Object:  StoredProcedure [InternetUser].[web_GetNewsletterList]    Script Date: 2012.10.08. 6:15:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[cms_NewsletterList];
GO
CREATE PROCEDURE [InternetUser].[cms_NewsletterList]( @TopN INT = 0, 
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
	FROM axdb_20120614.dbo.UPDHERLEV
	WHERE DATAAREAID = CASE WHEN @DataAreaId = '' THEN DATAAREAID ELSE @DataAreaId END AND
		  --ENGEDELYEZVE = 2 AND -- 0: osszes, 1: csak a meg nem kikuldottek, 2: csak a kikuldottek, 
		  ENGEDELYEZO <> '' AND
		  ELKULDVE = 1 AND 
		  HIRLEVID = CASE WHEN @BusinessUnitId = '' THEN HIRLEVID ELSE ( SELECT TOP 1 HIRLEVID FROM axdb_20120614.dbo.UPDHIRLEVJELLEG WHERE TEMAID = @BusinessUnitId ) END AND
		  HIRLEVID = CASE WHEN @ManufacturerId = '' THEN HIRLEVID ELSE ( SELECT TOP 1 HIRLEVID FROM axdb_20120614.dbo.UPDHIRLEVGYARTO WHERE GYARTONEV = @ManufacturerId ) END
		  AND WEBRE = 1
	ORDER BY HIRLEVID DESC

END
RETURN
GO

-- exec [InternetUser].[cms_NewsletterList] 10, 'hrp', '', '';