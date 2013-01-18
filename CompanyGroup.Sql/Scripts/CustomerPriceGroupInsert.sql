USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- select * from InternetUser.CustomerPriceGroup
DROP PROCEDURE [InternetUser].[CustomerPriceGroupInsert];
GO
CREATE PROCEDURE [InternetUser].[CustomerPriceGroupInsert](@VisitorId INT = 0,
														   @ManufacturerId NVARCHAR(4) = '',
														   @Category1Id NVARCHAR(4) = '',
														   @Category2Id NVARCHAR(4) = '',
														   @Category3Id NVARCHAR(4) = '',
														   @PriceGroupId NVARCHAR(4) = '',							
														   @Order INT = 0
														  )			
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.CustomerPriceGroup (VisitorId, ManufacturerId, Category1Id, Category2Id, Category3Id, PriceGroupId, [Order])
										 VALUES (@VisitorId, @ManufacturerId, @Category1Id, @Category2Id, @Category3Id, @PriceGroupId, @Order);

RETURN
