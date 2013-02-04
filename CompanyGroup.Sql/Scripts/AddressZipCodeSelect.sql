USE [ExtractInterface]
GO

/****** Object:  StoredProcedure [InternetUser].[cms_AddressZipCode]    Script Date: 2012.10.25. 6:37:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [InternetUser].[AddressZipCodeSelect]
GO

CREATE PROCEDURE [InternetUser].[AddressZipCodeSelect](@Prefix NVARCHAR(8) = '', @DataAreaId NVARCHAR(3) = 'hrp')
AS
SET NOCOUNT ON;

	DECLARE @VirtualDataAreaId nvarchar(3);

	SET @VirtualDataAreaId = 'hun';

	--SET @@RowCount = @iRowCount;

	SELECT ZipCode FROM axdb_20120614.dbo.ADDRESSZIPCODE
	WHERE DataAreaId = @VirtualDataAreaId AND 
		  ZipCode LIKE CASE WHEN @Prefix <> '' THEN @Prefix + '%' ELSE ZipCode END;
	-- COUNTRYREGIONID = 'HU' AND
RETURN

GO
GRANT EXECUTE ON InternetUser.AddressZipCodeSelect TO InternetUser
GO
-- EXEC [InternetUser].[AddressZipCodeSelect] '11', 'hrp'

