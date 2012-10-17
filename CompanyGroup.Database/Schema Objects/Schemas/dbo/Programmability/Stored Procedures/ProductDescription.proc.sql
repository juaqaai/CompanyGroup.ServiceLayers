USE WebDb_Test
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[ProductDescriptionList]
GO
/*
visszaadja a webre kitehető termékleírásokat
*/
CREATE PROCEDURE [InternetUser].[ProductDescriptionList]
AS
SET NOCOUNT ON
	SELECT ItemId as ProductId, 
		   Txt as [Description], 
		   CASE LanguageId WHEN 'HU' THEN 'hun' 
		   WHEN 'en-gb' THEN 'eng'
		   WHEN 'en-ca' THEN 'ser'
		   ELSE '' END as [LangId]		 	 
	FROM AxDb.dbo.INVENTTXT
	WHERE DataAreaId IN ('hrp', 'bsc', 'ser') AND ItemId <> '' AND Txt <> ''
	ORDER BY ItemId, [LangId] ;

RETURN;
GO
GRANT EXECUTE ON [InternetUser].[ProductDescriptionList] TO InternetUser
GO
-- exec InternetUser.ProductDescriptionList;