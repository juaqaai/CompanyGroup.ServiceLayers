USE WebDb_Test
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- vevõregisztrációs adatok lekérdezése
DROP PROCEDURE InternetUser.cms_CustomerData
GO
CREATE PROCEDURE InternetUser.cms_CustomerData( @CustomerId nvarchar(20), @DataAreaId nvarchar(3) = 'hrp' )											
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

	FROM axdb_20120614.dbo.CustTable AS Cust
		 LEFT OUTER JOIN axdb_20120614.dbo.updTmpCustTable as tmp ON tmp.CustRegId = Cust.RegAzon AND tmp.CustRegId <> '' AND tmp.DataAreaId = 'hun'
	WHERE Cust.DataAreaId = @DataAreaId AND 
		  Cust.AccountNum = @CustomerId AND
		  EXISTS ( SELECT TOP 1 CASE WHEN CONVERT( BIT, HRP ) = 1 OR CONVERT( BIT, BSC ) = 1 THEN 1 ELSE 0 END FROM axdb_20120614.dbo.CustTable WHERE DataAreaID IN ('Hrp', 'Bsc') AND AccountNum = @CustomerId );

	RETURN
GO
GRANT EXECUTE ON InternetUser.cms_CustomerData TO InternetUser
GO

-- exec InternetUser.cms_CustomerData 'V002020', 'hrp';