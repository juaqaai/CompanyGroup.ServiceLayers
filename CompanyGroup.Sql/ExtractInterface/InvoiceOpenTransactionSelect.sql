/*
	Az ExtractInterface adatbázis nyitott számlákat tartalmazó tábla (InvoiceOpenTransaction) lekérdezése, 
	cél, hogy a frissített nyitott tranzakciók a Web adatbázis nyitott számlákat tartalmazó stage táblába kerüljenek átmásolásra.

*/
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- megrendelések frissítés
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