USE Web

DROP PROCEDURE [InternetUser].[InvoiceOpenTransactionSelect];
GO
CREATE PROCEDURE [InternetUser].[InvoiceOpenTransactionSelect]										
AS
SET NOCOUNT ON

	SELECT Id, CustomerId, DataAreaId, SalesId, InvoiceDate, DueDate, InvoiceAmount, InvoiceCredit, CurrencyCode, 
	InvoiceId, Payment, SalesType, CusomerRef, InvoicingName, InvoicingAddress, ContactPersonId, Printed, ReturnItemId, ItemDate, LineNum, ItemId, ItemName, 
	Quantity, SalesPrice, LineAmount, QuantityPhysical, Remain, DeliveryType, TaxAmount, LineAmountMst, TaxAmountMst, DetailCurrencyCode, 
	Debit, Description, FileName, RecId, CreatedDate, ExtractDate, PackageLogKey, CONVERT(nvarchar(40), SerialNumber) as SerialNumber
	
	 FROM InternetUser.Invoice 
	WHERE Debit = 1

RETURN
GO
GRANT EXECUTE ON InternetUser.InvoiceOpenTransactionSelect TO InternetUser
GO
-- EXEC InternetUser.InvoiceOpenTransactionSelect;

/*
select * from InternetUser.Invoice where InvoiceId like 'HI014694%'

select * from InternetUser.Invoice where InvoiceId = 'BI003955/13'
*/
