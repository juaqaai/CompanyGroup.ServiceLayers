USE ExtractInterface
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- vevõregisztrációs adatok lekérdezése
DROP PROCEDURE InternetUser.CustomerSelect
GO
CREATE PROCEDURE InternetUser.CustomerSelect( @CustomerId nvarchar(20), @DataAreaId nvarchar(3) = 'hrp' )											
AS
	SELECT  
		   --ISNULL( tmp.RegType, 0 ) as RegType,
		   --ISNULL( tmp.OtherType, '' ) as OtherType, 
		   Cust.AccountNum as CustomerId,  
		   Cust.CegJegyzek as CompanyRegisterNumber, 
		   CASE WHEN CONVERT( BIT, Cust.HrpHirlevel ) = 1 OR CONVERT( BIT, Cust.BscHirlevel ) = 1 THEN 1 ELSE 0 END as Newsletter, 
		   Cust.AlairasiCimpeldanyFileName as SignatureEntityFile, 
		   Cust.Name AS CustomerName, 
		   Cust.VatNum AS VatNumber, 
		   Cust.EuVatNum AS EuVatNumber,
		   CASE WHEN Cust.CountryRegionId IN ('HU', 'HUN' ) THEN 'Magyarország' ELSE Cust.CountryRegionId END as InvoiceCountry,
		   Cust.City AS InvoiceCity, 
		   Cust.Street AS InvoiceStreet, 
		   Cust.ZipCode AS InvoiceZipCode, 
		   Cust.Phone AS InvoicePhone, 
		   Cust.CellularPhone AS InvoiceCellularPhone, 
		   Cust.Email AS InvoiceEmail, 

		   CASE WHEN Cust.mCountryRegionID IN ('HU', 'HUN' ) THEN 'Magyarország' ELSE Cust.mCountryRegionId END as MailCountry, 
		   Cust.mCity AS MailCity, 
		   Cust.mStreet AS MailStreet, 
		   Cust.mZipCode AS MailZipCode, 
		   CONVERT( SMALLDATETIME, ISNULL( tmp.ModifiedDate, GetDate() ) ) as dtDateTime

	FROM Axdb_20130131.dbo.CustTable AS Cust
		 LEFT OUTER JOIN Axdb_20130131.dbo.updTmpCustTable as tmp ON tmp.CustRegId = Cust.RegAzon AND tmp.CustRegId <> '' AND tmp.DataAreaId = 'hun'
	WHERE Cust.DataAreaId = @DataAreaId AND 
		  Cust.AccountNum = @CustomerId AND
		  EXISTS ( SELECT TOP 1 CASE WHEN CONVERT( BIT, HRP ) = 1 OR CONVERT( BIT, BSC ) = 1 THEN 1 ELSE 0 END FROM Axdb_20130131.dbo.CustTable WHERE DataAreaID IN ('Hrp', 'Bsc') AND AccountNum = @CustomerId );

	RETURN
GO
GRANT EXECUTE ON InternetUser.CustomerSelect TO InternetUser
GO

-- exec InternetUser.CustomerSelect 'V002020', 'hrp';