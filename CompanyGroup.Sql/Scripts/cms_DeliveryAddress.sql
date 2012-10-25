USE WebDb_Test
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   AddrTableId : Ezt a mezõt a rendszer a cím rekordazonosítójával és a sor számával együtt az egyedi kulcs meghatározására használja.
--   Type : aktuális címtipus ( 0:none, 1:Invoice, 2:Delivery, 3:AltDlv, 4:SWIFT, 5:Payment(kifizetes), 6:Service(szolgaltatas), 7:Levelezes  )

   
DROP FUNCTION InternetUser.cms_ExistCustomerAddress
GO
CREATE FUNCTION InternetUser.cms_ExistCustomerAddress( @CustomerId nvarchar(16), 
													   @DataAreaId nvarchar(3), 
												       @AddrType int )
RETURNS BIT	
AS
BEGIN

	DECLARE @Ret BIT;

	SET @Ret = CASE WHEN ( EXISTS ( SELECT * FROM axdb_20120614.dbo.CustTable AS Cust 
									INNER JOIN axdb_20120614.dbo.Address AS Addr ON Addr.AddrRecId = Cust.RecId AND 
																		   Addr.AddrTableId = 77 AND 
																		   Addr.DataAreaID = Cust.DataAreaID AND 
																		   Addr.Type = @AddrType
									 WHERE Cust.AccountNum = @CustomerId AND 
										   Cust.DataAreaId = @DataAreaId ) ) THEN 1 ELSE 0 END;

	RETURN @Ret;

END
GO
GRANT EXECUTE ON InternetUser.cms_ExistCustomerAddress TO InternetUser
GO

-- SELECT InternetUser.ExistCustomerAddress( 'V002575', 1, 2 );

-- select * from AxDb.dbo.Address
DROP PROCEDURE InternetUser.cms_DeliveryAddress
GO
CREATE PROCEDURE InternetUser.cms_DeliveryAddress( @CustomerId nvarchar(16), @DataAreaId nvarchar(3) )	
AS
SET NOCOUNT ON

	DECLARE @Exist BIT;
	SET @Exist = 0;

	-- létezik-e a szállítási cím a vevõkódhoz, vállalatkódhoz?
	IF ( InternetUser.cms_ExistCustomerAddress( @CustomerId, @DataAreaId, 2 ) = 1 )
	BEGIN
		SELECT Addr.RecId as RecId, 
			   Addr.City AS City, 
			   Addr.Street AS Street, 
			   Addr.ZipCode AS ZipCode, 
			   Addr.CountryRegionId AS CountryRegionId 
		FROM axdb_20120614.dbo.CustTable AS Cust 
		INNER JOIN axdb_20120614.dbo.Address AS Addr ON Addr.AddrRecId = Cust.RecId AND 
													   Addr.AddrTableId = 77 AND 
													   Addr.DataAreaID = Cust.DataAreaID AND 
													   Addr.Type = 2
		WHERE Cust.AccountNum = @CustomerId AND Cust.DataAreaId = @DataAreaId;
		
		SET @Exist = 1;
	END

	-- létezik-e a számlázási cím a vevõkódhoz, vállalatkódhoz?
	IF ( @Exist = 0 AND InternetUser.cms_ExistCustomerAddress( @CustomerId, @DataAreaId, 1 ) = 1 )
	BEGIN
		SELECT Addr.RecId as RecId, 
			   Addr.City AS City, 
			   Addr.Street AS Street, 
			   Addr.ZipCode AS ZipCode, 
			   Addr.CountryRegionId AS CountryRegionId 
		FROM axdb_20120614.dbo.CustTable AS Cust 
		INNER JOIN axdb_20120614.dbo.Address AS Addr ON Addr.AddrRecId = Cust.RecId AND 
													   Addr.AddrTableId = 77 AND 
													   Addr.DataAreaID = Cust.DataAreaID AND 
													   Addr.Type = 1
		WHERE Cust.AccountNum = @CustomerId AND 
			  Cust.DataAreaId = @DataAreaId;
		
		SET @Exist = 1;
	END

	IF ( @Exist = 0 )
	BEGIN
		-- vevõkódhoz, vállalatkódhoz rendelt cím
		SELECT Cust.RecId as RecId, 
			   Cust.City AS City, 
			   Cust.Street AS Street, 
			   Cust.ZipCode AS ZipCode, 
			   Cust.CountryRegionId AS CountryRegionId
		FROM axdb_20120614.dbo.CustTable AS Cust 
		WHERE Cust.AccountNum = @CustomerId AND Cust.DataAreaId = @DataAreaId;
	END

	RETURN

GO
GRANT EXECUTE ON InternetUser.cms_DeliveryAddress TO InternetUser
GO

/*
EXEC InternetUser.cms_DeliveryAddress 'V005884', 'hrp';   

EXEC InternetUser.cms_DeliveryAddress 'V002575', 'hrp';
EXEC InternetUser.cms_DeliveryAddress 'V004178', 'hrp';
EXEC InternetUser.cms_DeliveryAddress 'V000051', 'hrp';

EXEC InternetUser.cms_DeliveryAddress 'V016884', 'ser';

*/