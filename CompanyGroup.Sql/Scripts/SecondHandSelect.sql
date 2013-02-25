USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- használt cikkek lista
DROP PROCEDURE [InternetUser].[SecondHandSelect];
GO
CREATE PROCEDURE [InternetUser].[SecondHandSelect] 
AS
SET NOCOUNT ON

	SELECT Id, ProductId, ConfigId, InventLocationId, Quantity, Price, StatusDescription, DataAreaId
	FROM InternetUser.SecondHand
	WHERE Valid = 1;

RETURN
GO
GRANT EXECUTE ON InternetUser.SecondHandSelect TO InternetUser
-- EXEC [InternetUser].[SecondHandSelect];

-- SELECT * FROM InternetUser.SecondHand where ProductId = 'T110-11J'