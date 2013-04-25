
USE [TechnicalMetadata]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- cikkek ár frissítés
DROP PROCEDURE [dbo].[CataloguePriceUpdate];
GO
CREATE PROCEDURE [dbo].[CataloguePriceUpdate]  
AS
SET NOCOUNT ON
	
	UPDATE Web.InternetUser.Catalogue 
	SET Price1 = CASE WHEN Stage.AccountRelation = '1' THEN CONVERT(INT, Stage.Amount) ELSE C.Price1 END, 
		Price2 = CASE WHEN Stage.AccountRelation = '2' THEN CONVERT(INT, Stage.Amount) ELSE C.Price2 END, 
		Price3 = CASE WHEN Stage.AccountRelation = '3' THEN CONVERT(INT, Stage.Amount) ELSE C.Price3 END, 
		Price4 = CASE WHEN Stage.AccountRelation = '4' THEN CONVERT(INT, Stage.Amount) ELSE C.Price4 END, 
		Price5 = CASE WHEN Stage.AccountRelation = '5' THEN CONVERT(INT, Stage.Amount) ELSE C.Price5 END 
	FROM Web.InternetUser.Stage_PriceDiscTable as Stage
	INNER JOIN Web.InternetUser.Catalogue as C ON Stage.ItemRelation = C.ProductId AND C.DataAreaId = Stage.DataAreaId
	WHERE Stage.Operation <> 'I';

RETURN
GO
GRANT EXECUTE ON dbo.[CataloguePriceUpdate] TO [HRP_HEADOFFICE\AXPROXY]
GO
-- exec [dbo].[CataloguePriceUpdate]  
-- select * from Web.InternetUser.Stage_PriceDiscTable