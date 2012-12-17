USE [WebDb_Test]
GO

/****** Object:  UserDefinedFunction [InternetUser].[GetSecondHandPrice]    Script Date: 2012.12.15. 13:12:34 ******/
DROP FUNCTION [InternetUser].[GetSecondHandPrice]
GO

/****** Object:  UserDefinedFunction [InternetUser].[GetSecondHandPrice]    Script Date: 2012.12.15. 13:12:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [InternetUser].[GetSecondHandPrice]( @DataAreaId nvarchar(3) = 'hrp', -- vallalatkod
													 @ProductId nvarchar(20),		  -- termekazonosito
													 @ConfigId nvarchar(20) )		  -- konfig.
RETURNS REAL	
AS
BEGIN

	DECLARE @Price real;

	SET @Price = ISNULL( ( SELECT TOP 1 pdt.Amount 
						   FROM AxDb.dbo.PriceDiscTable AS pdt 
						   INNER JOIN AxDb.dbo.InventDim AS idim on idim.inventDimId = pdt.InventDimId and pdt.DataAreaID = idim.DataAreaID
						   WHERE pdt.ItemRelation = @ProductId AND pdt.dataAreaId = @DataAreaId and idim.ConfigId = @ConfigId
						   ORDER BY pdt.Amount 
						), 0 );

	RETURN @Price;
END

GO


