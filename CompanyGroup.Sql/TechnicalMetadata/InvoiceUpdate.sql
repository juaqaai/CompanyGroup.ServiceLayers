
USE [TechnicalMetadata]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- megrendelések frissítés
DROP PROCEDURE [dbo].[InvoiceUpdate];
GO
CREATE PROCEDURE [dbo].[InvoiceUpdate]  
AS
SET NOCOUNT ON

	UPDATE Web.InternetUser.Invoice
		SET [CustomerId] = s.CustomerId
      ,[DataAreaId] = s.[DataAreaId]
      ,[SalesId] = s.[SalesId]
      ,[InvoiceDate] = s.[InvoiceDate]
      ,[DueDate] = s.[DueDate]
      ,[InvoiceAmount] = s.[InvoiceAmount]
      ,[InvoiceCredit] = s.[InvoiceCredit]
      ,[CurrencyCode] = s.[CurrencyCode]
      ,[InvoiceId] = s.[InvoiceId]
      ,[Payment] = s.[Payment]
      ,[SalesType] = s.[SalesType]
      ,[CusomerRef] = s.[CusomerRef]
      ,[InvoicingName] = s.[InvoicingName]
      ,[InvoicingAddress] = s.[InvoicingAddress]
      ,[ContactPersonId] = s.[ContactPersonId]
      ,[Printed] = s.[Printed]
      ,[ReturnItemId] = s.[ReturnItemId]
      ,[ItemDate] = s.[ItemDate]
      ,[LineNum] = s.[LineNum]
      ,[ItemId] = s.[ItemName]
      ,[ItemName] = s.[ItemName]
      ,[Quantity] = s.[Quantity]
      ,[SalesPrice] = s.[SalesPrice]
      ,[LineAmount] = s.[LineAmount]
      ,[QuantityPhysical] = s.[QuantityPhysical]
      ,[Remain] = s.[Remain]
      ,[DeliveryType] = s.[DeliveryType]
      ,[TaxAmount] = s.[TaxAmount]
      ,[LineAmountMst] = s.[LineAmountMst]
      ,[TaxAmountMst] = s.[TaxAmountMst]
      ,[DetailCurrencyCode] = s.[DetailCurrencyCode]
      ,[Debit] = s.[Debit]
      ,[Description] = s.[Description]
      ,[FileName] = s.[FileName]
      ,[RecId] = s.[RecId]
      ,[CreatedDate] = s.[CreatedDate]
      ,[ExtractDate] = s.[ExtractDate]
      ,[PackageLogKey] = s.[PackageLogKey]
      ,[SerialNumber] = s.[SerialNumber]
		FROM  Web.InternetUser.Stage_Invoice as s
			INNER JOIN Web.InternetUser.Invoice as t ON 
			t.RecId = s.RecId AND t.DataAreaId = s.DataAreaId
		WHERE Operation = 'U';

	INSERT INTO Web.InternetUser.Invoice
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
      ,[SerialNumber]
	FROM Web.InternetUser.Stage_Invoice as s 
	WHERE Operation = 'I';		 

RETURN
GO
GRANT EXECUTE ON dbo.[InvoiceUpdate] TO [HRP_HEADOFFICE\AXPROXY]
GO