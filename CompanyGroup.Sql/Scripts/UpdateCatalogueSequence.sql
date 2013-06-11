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
	UPDATE InternetUser.Catalogue SET Sequence0 = NULL, Sequence1 = NULL;

	-- sorrend átlagos készletkor szerint
	WITH Sequence0_CTE (Id, Sequence, AverageInventory, Discount, Category1Id)
	AS (
		SELECT Id, ROW_NUMBER() OVER (ORDER BY AverageInventory DESC), AverageInventory, Discount, Category1Id 
		FROM InternetUser.Catalogue 
		WHERE Category1Id = 'B011' AND Discount = 1 AND AverageInventory > 0 AND Stock > 0
	), 
	-- sorrend készletérték szerint
	Sequence1_CTE (Id, Sequence)
	AS (
		SELECT Id,  -1 * ROW_NUMBER() OVER (ORDER BY ShippingDate, Id) as Sequence
		FROM InternetUser.Catalogue 
		WHERE ShippingDate <> '1900-01-01 00:00:00' and Stock = 0
		GROUP BY Id, ShippingDate
	)
	-- select * from Sequence1_CTE; 

	-- átlagos készletkor szerinti sorrend frissítése
	-- készletérték szerinti sorrend frissítése
	UPDATE InternetUser.Catalogue SET Sequence0 = (SELECT Sequence FROM Sequence0_CTE WHERE InternetUser.Catalogue.Id = Sequence0_CTE.Id), 
									  Sequence1 = (SELECT Sequence FROM Sequence1_CTE WHERE InternetUser.Catalogue.Id = Sequence1_CTE.Id)
	-- WHERE Discount = 1 AND AverageInventory > 0 AND Stock > 0;

	DECLARE @MaxRowNumber0 INT = (SELECT MAX(Sequence0) FROM InternetUser.Catalogue);  

	DECLARE @MinRowNumber1 INT = (SELECT MIN(Sequence1) FROM InternetUser.Catalogue); 

	WITH Remain_CTE (Id, Sequence, AverageInventory, Discount, Category1Id)
	AS (
		SELECT Id, ROW_NUMBER() OVER (ORDER BY AverageInventory DESC, Discount DESC), AverageInventory, Discount, Category1Id 
		FROM InternetUser.Catalogue WHERE Sequence0 IS NULL
	)

	UPDATE InternetUser.Catalogue SET Sequence0 = (SELECT Sequence + @MaxRowNumber0 FROM Remain_CTE WHERE InternetUser.Catalogue.Id = Remain_CTE.Id)
	WHERE Sequence0 IS NULL;

	UPDATE InternetUser.Catalogue SET Sequence1 = CASE WHEN Stock > 0 THEN Stock ELSE @MinRowNumber1 - 1 END
	WHERE Sequence1 IS NULL;

RETURN
GO
GRANT EXECUTE ON InternetUser.UpdateCatalogueSequence TO InternetUser
GO

/*
  EXEC  [InternetUser].[UpdateCatalogueSequence2]
  SELECT * FROM InternetUser.Catalogue where Sequence1 = 1
  ShippingDate <> '1900-01-01 00:00:00' and Stock = 0

  ProductId = '45f-00012''0021165103764' order by Partnumber 
--where Sequence0 IS NULL ORDER BY Sequence0

select Id, ShippingDate from InternetUser.Catalogue 
where ShippingDate <> '1900-01-01 00:00:00' and Stock = 0
group by Id, ShippingDate
order by ShippingDate, Id
*/