USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
	Id				INT IDENTITY PRIMARY KEY,
	VisitorId		NVARCHAR(64) NOT NULL DEFAULT '',										-- login azonosito
	ManufacturerId  NVARCHAR(4) NOT NULL DEFAULT '',
	Category1Id		NVARCHAR(4) NOT NULL DEFAULT '', 
	Category2Id		NVARCHAR(4) NOT NULL DEFAULT '', 
	Category3Id		NVARCHAR(4) NOT NULL DEFAULT '',
	PriceGroupId	NVARCHAR(4) NOT NULL DEFAULT '',
	[Order]			INT NOT NULL DEFAULT 1
*/

DROP PROCEDURE [InternetUser].[CustomerPriceGroupInsert];
GO
CREATE PROCEDURE [InternetUser].[CustomerPriceGroupInsert](@VisitorId NVARCHAR(64) = '',
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
