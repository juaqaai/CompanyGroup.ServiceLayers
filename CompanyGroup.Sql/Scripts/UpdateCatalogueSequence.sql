USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek rendezés beállítása
DROP PROCEDURE [InternetUser].[UpdateCatalogueSequence];
GO
CREATE PROCEDURE [InternetUser].[UpdateCatalogueSequence]
AS

	SET NOCOUNT ON;
	UPDATE InternetUser.Catalogue SET Sequence0 = NULL;

	WITH Sequence0_CTE (Id, Sequence, AverageInventory, Discount, Category1Id)
	AS (
		SELECT Id, ROW_NUMBER() OVER (ORDER BY AverageInventory DESC), AverageInventory, Discount, Category1Id 
		FROM InternetUser.Catalogue WHERE Category1Id = 'B011'
	)
	UPDATE InternetUser.Catalogue SET Sequence0 = (SELECT Sequence FROM Sequence0_CTE WHERE InternetUser.Catalogue.Id = Sequence0_CTE.Id)
	WHERE Discount = 1 AND AverageInventory > 0 AND Stock > 0;

	DECLARE @RowNumber INT = (SELECT MAX(Sequence0) FROM InternetUser.Catalogue);  

	WITH Remain_CTE (Id, Sequence, AverageInventory, Discount, Category1Id)
	AS (
		SELECT Id, ROW_NUMBER() OVER (ORDER BY AverageInventory DESC, Discount DESC), AverageInventory, Discount, Category1Id 
		FROM InternetUser.Catalogue WHERE Sequence0 IS NULL
	)

	UPDATE InternetUser.Catalogue SET Sequence0 = (SELECT Sequence + @RowNumber FROM Remain_CTE WHERE InternetUser.Catalogue.Id = Remain_CTE.Id)
	WHERE Sequence0 IS NULL;

RETURN
GO
GRANT EXECUTE ON InternetUser.UpdateCatalogueSequence TO InternetUser
GO
-- EXEC  [InternetUser].[UpdateCatalogueSequence]
-- SELECT * FROM InternetUser.Catalogue where ProductId = '45f-00012''0021165103764' order by Partnumber 
--where Sequence0 IS NULL ORDER BY Sequence0