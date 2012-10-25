USE WebDb_Test
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- vevõhöz tartozó levelezési adatok lekérdezése
DROP PROCEDURE InternetUser.cms_MailAddress
GO
CREATE PROCEDURE InternetUser.cms_MailAddress( @CustomerId nvarchar(20), @DataAreaId nvarchar(3) )											
AS
	SELECT MCity AS City, 
		   --MCounty as County,
		   MCountryRegionId as Country,
		   MStreet AS Street, 
		   MZipCode AS ZipCode 
	FROM axdb_20120614.dbo.CustTable
	WHERE DataAreaId = @DataAreaId AND AccountNum = @CustomerId;
	RETURN
GO
GRANT EXECUTE ON InternetUser.cms_MailAddress TO InternetUser
GO

/*
exec InternetUser.cms_MailAddress 'V002020', 'hrp'
select MCountryRegionId FROM AxDb20101004.dbo.CustTable group by MCountryRegionId

*/