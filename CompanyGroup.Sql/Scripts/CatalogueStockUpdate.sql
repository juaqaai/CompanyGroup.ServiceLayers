
USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek készlet frissítés
DROP PROCEDURE [InternetUser].[CatalogueStockUpdate];
GO
CREATE PROCEDURE [InternetUser].[CatalogueStockUpdate] (@DataAreaId nvarchar(4) = 'hrp',
														@ProductId nvarchar (20) = '',         
														@Stock int = 0)  
AS
SET NOCOUNT ON
	
	UPDATE InternetUser.Catalogue SET Stock = @Stock WHERE DataAreaId = @DataAreaId AND ProductId = @ProductId;

	SELECT 1 as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueStockUpdate TO InternetUser
GO