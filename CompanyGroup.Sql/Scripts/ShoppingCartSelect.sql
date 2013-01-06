USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueBannerSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueBannerSelect] (@DataAreaId nvarchar(4) = 'hrp',
														 @StructureXml nvarchar (4000) = '' 
)
AS
SET NOCOUNT ON



RETURN