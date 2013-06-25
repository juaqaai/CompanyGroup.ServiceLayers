USE [Web]
GO
/* 
	Object:  StoredProcedure exec [InternetUser].[CatalogueLoad]    
	select * from InternetUser.Catalogue;
*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[CatalogueLoad] 
GO
CREATE PROCEDURE [InternetUser].[CatalogueLoad] 
AS
SET NOCOUNT ON

	IF (EXISTS(SELECT * FROM InternetUser.Stage_Catalogue))
	BEGIN

		TRUNCATE TABLE InternetUser.Catalogue;

	    INSERT INTO InternetUser.Catalogue 
	    SELECT [ProductId]
			,[AxStructCode]
			,[DataAreaId]
			,[StandardConfigId]
			,[Name]
			,[EnglishName]
			,[PartNumber]
			,[ManufacturerId]
			,[ManufacturerName]
			,[ManufacturerEnglishName]
			,[Category1Id]
			,[Category1Name]
			,[Category1EnglishName]
			,[Category2Id]
			,[Category2Name]
			,[Category2EnglishName]
			,[Category3Id]
			,[Category3Name]
			,[Category3EnglishName]
			,[Stock]
			,[AverageInventory]
			,[Price1]
			,[Price2]
			,[Price3]
			,[Price4]
			,[Price5]
			,[Garanty]
			,[GarantyMode]
			,[Discount]
			,[New]
			,[ItemState]
			,[Description]
			,[EnglishDescription]
			,[ProductManagerId]
			,[ShippingDate]
			,[CreatedDate]
			,[Updated]
			,[Available]
			,[PictureId]
			,[SecondHand]
			,[Valid]
			,[ExtractDate]
			,[PackageLogKey]
			,[Sequence0]
			,[SearchContent]
			,[IsPurchaseOrdered]
			,0
		 FROM InternetUser.Stage_Catalogue;
	END

	RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueLoad TO InternetUser
GO
GRANT EXECUTE ON [InternetUser].[CatalogueLoad] TO [HRP_HEADOFFICE\AXPROXY]
GO