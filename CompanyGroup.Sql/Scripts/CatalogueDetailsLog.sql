USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- select * from InternetUser.CatalogueDetailsLog
DROP PROCEDURE [InternetUser].[CatalogueDetailsLogSelect];
GO
CREATE PROCEDURE [InternetUser].[CatalogueDetailsLogSelect] (@VisitorId NVARCHAR(64) = '')
AS
SET NOCOUNT ON

	DECLARE @CustomerId NVARCHAR(20), @PersonId NVARCHAR(20);

	SELECT @CustomerId = CustomerId, @PersonId = PersonId FROM InternetUser.Visitor WHERE VisitorId = @VisitorId;

	-- IF (@CustomerId IS NULL) AND (@PersonId IS NULL)

	SELECT TOP 4 L.ProductId, C.Name as ProductName, C.EnglishName as EnglishProductName, C.PictureId, C.DataAreaId
	FROM InternetUser.CatalogueDetailsLog as L
	INNER JOIN InternetUser.Catalogue as C ON C.ProductId = L.ProductId AND C.DataAreaId = L.DataAreaId
	WHERE CustomerId = @CustomerId AND 
		  PersonId = CASE WHEN @PersonId IS NULL THEN PersonId ELSE @PersonId END
	ORDER BY L.Id DESC;

RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueDetailsLogSelect TO InternetUser

-- EXEC [InternetUser].[CatalogueDetailsLogSelect] '6FDC581D96954A74804881EFEB78212A';

GO
-- 
DROP PROCEDURE [InternetUser].[CatalogueDetailsLogInsert];
GO
CREATE PROCEDURE [InternetUser].[CatalogueDetailsLogInsert] ( @VisitorId NVARCHAR(64) = '',  
															  @CustomerId NVARCHAR(20) = '',
															  @PersonId	NVARCHAR(20) ='',
															  @DataAreaId NVARCHAR(4) = '',
															  @ProductId NVARCHAR(20) = '' )
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.CatalogueDetailsLog (VisitorId, CustomerId, PersonId, DataAreaId, ProductId) VALUES
												 (@VisitorId, @CustomerId, @PersonId, @DataAreaId, @ProductId);

RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueDetailsLogInsert TO InternetUser