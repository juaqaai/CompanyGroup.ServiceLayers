USE ExtractInterface
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- kis / nagy módosításhoz szükséges vevõregisztrációs adatok lekérdezése
DROP PROCEDURE InternetUser.CustomerContractSelect
GO
CREATE PROCEDURE InternetUser.CustomerContractSelect( @CustomerId nvarchar(20))											
AS
	SELECT  
		   --Cust.AccountNum as CustomerId,  
		   Cust.CegJegyzek as CompanyRegisterNumber, 
		   Cust.Name AS CustomerName, 
		   Cust.VatNum AS VatNumber, 
		   Cust.EuVatNum AS EuVatNumber,
		   Cust.CountryRegionId  as InvoiceCountry, --CASE WHEN IN ('HU', 'HUN' ) THEN 'Magyarország' ELSE Cust.CountryRegionId END as InvoiceCountry,
		   Cust.City AS InvoiceCity, 
		   Cust.Street AS InvoiceStreet, 
		   Cust.ZipCode AS InvoiceZipCode, 
		   Cust.DataAreaId
		   --Cust.Phone AS InvoicePhone, 
		   --Cust.CellularPhone AS InvoiceCellularPhone, 
		   --Cust.Email AS InvoiceEmail, 

		   --CONVERT( SMALLDATETIME, ISNULL( tmp.ModifiedDate, GetDate() ) ) as dtDateTime

	FROM Axdb.dbo.CustTable AS Cust
		 --LEFT OUTER JOIN Axdb.dbo.updTmpCustTable as tmp ON tmp.CustRegId = Cust.RegAzon AND tmp.CustRegId <> '' AND tmp.DataAreaId = 'hun'
	WHERE Cust.DataAreaId = 'hrp' AND-- @DataAreaId AND 
		  Cust.AccountNum = @CustomerId AND
		  (HRP = 1 OR BSC = 1)
		  --EXISTS ( SELECT TOP 1 CASE WHEN CONVERT( BIT, HRP ) = 1 OR CONVERT( BIT, BSC ) = 1 THEN 1 ELSE 0 END FROM Axdb.dbo.CustTable WHERE DataAreaID IN ('Hrp', 'Bsc') AND AccountNum = @CustomerId );

	RETURN
GO
GRANT EXECUTE ON InternetUser.CustomerContractSelect TO InternetUser
GO

-- exec InternetUser.CustomerContractSelect 'V001446''V002020';