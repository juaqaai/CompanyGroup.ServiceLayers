/*
	Az ExtractInterface adatb�zis nyitott sz�ml�kat tartalmaz� t�bla (InvoiceOpenTransaction) lek�rdez�se, 
	c�l, hogy a friss�tett nyitott tranzakci�k a Web adatb�zis nyitott sz�ml�kat tartalmaz� stage t�bl�ba ker�ljenek �tm�sol�sra.

*/
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- megrendel�sek friss�t�s
DROP PROCEDURE [InternetUser].[InvoiceOpenTransactionSelect];
GO
CREATE PROCEDURE [InternetUser].[InvoiceOpenTransactionSelect]
AS
SET NOCOUNT ON

SELECT [CustomerId]
      ,[DataAreaId]
      ,[SalesId]
      ,[InvoiceDate]
      ,[DueDate]
      ,[InvoiceAmount]
      ,[InvoiceCredit]
      ,[CurrencyCode]
      ,[InvoiceId]
      ,[Payment]
      ,[SalesType]
      ,[CusomerRef]
      ,[InvoicingName]
      ,[InvoicingAddress]
      ,[ContactPersonId]
      ,[Printed]
      ,[ReturnItemId]
      ,[ItemDate]
      ,[LineNum]
      ,[ItemId]
      ,[ItemName]
      ,[SerialNumber]
      ,[Quantity]
      ,[SalesPrice]
      ,[LineAmount]
      ,[QuantityPhysical]
      ,[Remain]
      ,[DeliveryType]
      ,[TaxAmount]
      ,[LineAmountMst]
      ,[TaxAmountMst]
      ,[DetailCurrencyCode]
      ,[Debit]
      ,[Description]
      ,[FileName]
      ,[RecId]
      ,[CreatedDate]
      ,[ExtractDate]
      ,[PackageLogKey]
  FROM [ExtractInterface].[dbo].[Stage_InvoiceOpenTransaction]

  RETURN
GO
GRANT EXECUTE ON InternetUser.[InvoiceOpenTransactionSelect] TO [HRP_HEADOFFICE\AXPROXY]
GO

-- exec [InternetUser].[InvoiceOpenTransactionSelect];