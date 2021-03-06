/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [Id]
      ,[ProductId]
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
  select count (*) FROM [Web].[InternetUser].[Catalogue]	-- 10567

  ;with CTE (ProductId) AS
  (
  select ProductId 
    FROM [Web].[InternetUser].[Catalogue]
	group by  ProductId
	having count(ProductId) > 1 )

	select c.* from CTE as cte
	left outer join [Web].[InternetUser].[Catalogue] as c on cte.ProductId = c.ProductId;


USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[CatalogueRemoveDuplicates] 
GO
CREATE PROCEDURE [InternetUser].[CatalogueRemoveDuplicates] 
AS
SET NOCOUNT ON

	;WITH Duplicates AS 
	(
		SELECT ROW_NUMBER() OVER( PARTITION BY ProductId
							ORDER BY ProductId) AS OrderId
		FROM [InternetUser].[Stage_Catalogue] 
	)
	DELETE Duplicates
	WHERE OrderId > 1;

GO
GRANT EXECUTE ON InternetUser.CatalogueRemoveDuplicates TO InternetUser
GO
GRANT EXECUTE ON [InternetUser].[CatalogueRemoveDuplicates] TO [HRP_HEADOFFICE\AXPROXY]
GO

-- exec [InternetUser].[CatalogueRemoveDuplicates];