USE ExtractInterface
GO
/****** SELECT * FROM Axdb.dbo.UPDHERLEV WHERE ENGEDELYEZO <> '' AND ELKULDVE = 1 AND WEBRE = 1 ORDER BY HIRLEVID DESC ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[NewsletterSelectCondition];
GO
CREATE PROCEDURE [InternetUser].[NewsletterSelectCondition]( @TopN INT = 0, 
															 @DataAreaId nvarchar(3) = '', 
															 @NewsletterIdList nvarchar (250) = '' )
AS
BEGIN
	SET NOCOUNT ON;

	IF ( @TopN > 0 )
		SET ROWCOUNT @TopN;

	DECLARE @Separator varchar(1) = ',';
	--DECLARE @NewsletterIdList nvarchar (250) = 'SHIR0061, SHIR0058, SHIR0059';

	WITH Newsletter_CTE(Ni, Nj)
	AS(
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @NewsletterIdList)
		UNION ALL
		SELECT CONVERT(INT, Nj) + 1, CHARINDEX(@Separator, @NewsletterIdList, CONVERT(INT, Nj) + 1)
		FROM Newsletter_CTE
		WHERE Nj > 0
	)
	-- SELECT SUBSTRING(@NewsletterIdList, Ni, COALESCE(NULLIF(Nj, 0), LEN(@NewsletterIdList) + 1) - Ni) FROM Newsletter_CTE

	SELECT HIRLEVID AS [Id], TEMA AS [Title], 
		   MEGJEGYZES AS [Description], HTMLFILE AS [HtmlPath], LEJARATDATUMA AS [EndDate], 
		   NAGYKEP AS [PicturePath], ENGEDELYEZESDATUMA AS [AllowedDate], 
		   ENGEDELYEZESIDEJE AS [AllowedTime]
	FROM Axdb.dbo.UPDHERLEV
	WHERE DATAAREAID = CASE WHEN @DataAreaId = '' THEN DATAAREAID ELSE @DataAreaId END AND
		  --ENGEDELYEZVE = 2 AND -- 0: osszes, 1: csak a meg nem kikuldottek, 2: csak a kikuldottek, 
		  ENGEDELYEZO <> '' AND
		  ELKULDVE = 1 AND 
		  (HirlevId IN (SELECT SUBSTRING(@NewsletterIdList, Ni, COALESCE(NULLIF(Nj, 0), LEN(@NewsletterIdList) + 1) - Ni) FROM Newsletter_CTE) OR (@NewsletterIdList = ''))
		  AND WEBRE = 1 AND
		  1 = CASE WHEN @TopN = 0 AND @NewsletterIdList = '' THEN 
			  CASE WHEN DATEDIFF(month, ENGEDELYEZESDATUMA, GETDATE()) < 4 THEN 1 ELSE 0 END
		  ELSE 1 END

	ORDER BY HIRLEVID DESC

END
RETURN
GO
GRANT EXECUTE ON InternetUser.NewsletterSelectCondition TO InternetUser
GO

-- exec [InternetUser].[NewsletterSelectCondition] 0, '', 'SHIR0061,SHIR0058,SHIR0059';