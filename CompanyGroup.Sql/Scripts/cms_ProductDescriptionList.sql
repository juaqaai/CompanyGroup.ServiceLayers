USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
visszaadja a webre kitehetõ termékleírásokat
*/
DROP PROCEDURE [InternetUser].[cms_ProductDescriptionList];
GO
CREATE PROCEDURE [InternetUser].[cms_ProductDescriptionList]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON

	SELECT ItemId as ProductId, 
		   Txt as [Description], 
		   CASE LanguageId WHEN 'HU' THEN 'hun' 
		   WHEN 'en-gb' THEN 'eng'
		   WHEN 'en-ca' THEN 'ser'
		   ELSE '' END as [LangId]	 	 
	FROM Axdb_20130131.dbo.INVENTTXT
	WHERE DataAreaId = @DataAreaId AND ItemId <> '' AND Txt <> ''
	ORDER BY ItemId, [LangId];

RETURN;

-- EXEC [InternetUser].[cms_ProductDescriptionList] 'ser'