USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- select * from InternetUser.CustomerPriceGroup where VisitorId = 746
DROP PROCEDURE [InternetUser].[CustomerPriceGroupInsert];
GO
CREATE PROCEDURE [InternetUser].[CustomerPriceGroupInsert](@VisitorKey INT = 0,
														   @PriceGroupId NVARCHAR(4) = '',
														   @ManufacturerId NVARCHAR(4) = '',
														   @Category1Id NVARCHAR(4) = '',
														   @Category2Id NVARCHAR(4) = '',
														   @Category3Id NVARCHAR(4) = '',							
														   @Order INT = 0, 
														   @DataAreaId NVARCHAR(3) = ''
														  )			
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.CustomerPriceGroup (VisitorId, ManufacturerId, Category1Id, Category2Id, Category3Id, PriceGroupId, [Order], DataAreaId)
										 VALUES (@VisitorKey, @ManufacturerId, @Category1Id, @Category2Id, @Category3Id, @PriceGroupId, @Order, @DataAreaId);

RETURN
GO
GRANT EXECUTE ON InternetUser.CustomerPriceGroupInsert TO InternetUser

-- 