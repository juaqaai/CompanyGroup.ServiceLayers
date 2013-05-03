USE ExtractInterface
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   AddrTableId : Ezt a mez�t a rendszer a c�m rekordazonos�t�j�val �s a sor sz�m�val egy�tt az egyedi kulcs meghat�roz�s�ra haszn�lja.
--   Type : aktu�lis c�mtipus ( 0:none, 1:Invoice, 2:Delivery, 3:AltDlv, 4:SWIFT, 5:Payment(kifizetes), 6:Service(szolgaltatas), 7:Levelezes  )

   
DROP FUNCTION InternetUser.ExistCustomerAddress
GO
CREATE FUNCTION InternetUser.ExistCustomerAddress( @CustomerId nvarchar(16), 
													   @DataAreaId nvarchar(3), 
												       @AddrType int )
RETURNS BIT	
AS
BEGIN

	DECLARE @Ret BIT;

	SET @Ret = CASE WHEN ( EXISTS ( SELECT * FROM Axdb_20130131.dbo.CustTable AS Cust 
									INNER JOIN Axdb_20130131.dbo.Address AS Addr ON Addr.AddrRecId = Cust.RecId AND 
																		   Addr.AddrTableId = 77 AND 
																		   Addr.DataAreaID = Cust.DataAreaID AND 
																		   Addr.Type = @AddrType
									 WHERE Cust.AccountNum = @CustomerId AND 
										   Cust.DataAreaId = @DataAreaId ) ) THEN 1 ELSE 0 END;

	RETURN @Ret;

END
GO
GRANT EXECUTE ON InternetUser.ExistCustomerAddress TO InternetUser
GO

-- SELECT InternetUser.ExistCustomerAddress( 'V002575', 1, 2 );

-- select * from AxDb.dbo.Address
DROP PROCEDURE InternetUser.DeliveryAddressSelect
GO
CREATE PROCEDURE InternetUser.DeliveryAddressSelect( @CustomerId nvarchar(16), @DataAreaId nvarchar(3) )	
AS
SET NOCOUNT ON

	DECLARE @Exist BIT;
	SET @Exist = 0;

	-- l�tezik-e a sz�ll�t�si c�m a vev�k�dhoz, v�llalatk�dhoz?
	IF ( InternetUser.ExistCustomerAddress( @CustomerId, @DataAreaId, 2 ) = 1 )
	BEGIN
		SELECT Addr.RecId as RecId, 
			   Addr.City AS City, 
			   Addr.Street AS Street, 
			   Addr.ZipCode AS ZipCode, 
			   Addr.CountryRegionId AS CountryRegionId 
		FROM Axdb_20130131.dbo.CustTable AS Cust 
		INNER JOIN Axdb_20130131.dbo.Address AS Addr ON Addr.AddrRecId = Cust.RecId AND 
													   Addr.AddrTableId = 77 AND 
													   Addr.DataAreaID = Cust.DataAreaID AND 
													   Addr.Type = 2
		WHERE Cust.AccountNum = @CustomerId AND Cust.DataAreaId = @DataAreaId;
		
		SET @Exist = 1;
	END

	-- l�tezik-e a sz�ml�z�si c�m a vev�k�dhoz, v�llalatk�dhoz?
	IF ( @Exist = 0 AND InternetUser.ExistCustomerAddress( @CustomerId, @DataAreaId, 1 ) = 1 )
	BEGIN
		SELECT Addr.RecId as RecId, 
			   Addr.City AS City, 
			   Addr.Street AS Street, 
			   Addr.ZipCode AS ZipCode, 
			   Addr.CountryRegionId AS CountryRegionId 
		FROM Axdb_20130131.dbo.CustTable AS Cust 
		INNER JOIN Axdb_20130131.dbo.Address AS Addr ON Addr.AddrRecId = Cust.RecId AND 
													   Addr.AddrTableId = 77 AND 
													   Addr.DataAreaID = Cust.DataAreaID AND 
													   Addr.Type = 1
		WHERE Cust.AccountNum = @CustomerId AND 
			  Cust.DataAreaId = @DataAreaId;
		
		SET @Exist = 1;
	END

	IF ( @Exist = 0 )
	BEGIN
		-- vev�k�dhoz, v�llalatk�dhoz rendelt c�m
		SELECT Cust.RecId as RecId, 
			   Cust.City AS City, 
			   Cust.Street AS Street, 
			   Cust.ZipCode AS ZipCode, 
			   Cust.CountryRegionId AS CountryRegionId
		FROM Axdb_20130131.dbo.CustTable AS Cust 
		WHERE Cust.AccountNum = @CustomerId AND Cust.DataAreaId = @DataAreaId;
	END

	RETURN

GO
GRANT EXECUTE ON InternetUser.DeliveryAddressSelect TO InternetUser
GO

/*
EXEC InternetUser.DeliveryAddressSelect 'V005884', 'hrp';   

EXEC InternetUser.DeliveryAddressSelect 'V002575', 'hrp';
EXEC InternetUser.DeliveryAddressSelect 'V004178', 'hrp';
EXEC InternetUser.DeliveryAddressSelect 'V000051', 'hrp';

EXEC InternetUser.DeliveryAddressSelect 'V016884', 'ser';

*/