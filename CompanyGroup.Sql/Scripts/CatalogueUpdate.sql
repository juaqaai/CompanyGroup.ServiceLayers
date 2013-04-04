USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueStockUpdate];
GO
CREATE PROCEDURE [InternetUser].[CatalogueUpdate] (@ProductId nvarchar(20) = '',
												   @DataAreaId nvarchar(4) = 'hrp',
												   @Stock int = 0,
												   @InventLocationId nvarchar(20) = 'KULSO')
AS
SET NOCOUNT ON

	IF (@InventLocationId IN ('KULSO', '7000'))
	
		UPDATE InternetUser.Catalogue SET Stock = @Stock WHERE ProductId = @ProductId AND DataAreaId = @DataAreaId;
	
	ELSE
		UPDATE InternetUser.SecondHand SET Quantity = @Stock WHERE ProductId = @ProductId AND DataAreaId = @DataAreaId;

	RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueUpdate TO InternetUser