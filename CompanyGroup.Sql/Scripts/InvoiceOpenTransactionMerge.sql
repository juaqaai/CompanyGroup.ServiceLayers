 
USE Web
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[InvoiceOpenTransactionMerge];
GO
CREATE PROCEDURE [InternetUser].[InvoiceOpenTransactionMerge]
AS
SET NOCOUNT ON

	-- Invoice és a Stage közös elemeinek frissítése az Invoice táblában 
	UPDATE [InternetUser].[Invoice] SET InvoiceCredit = Stage.InvoiceCredit, 
										Debit = Stage.Debit
	FROM [InternetUser].[Stage_InvoiceOpenTransaction] as Stage 
	INNER JOIN [InternetUser].[Invoice] as I ON Stage.InvoiceId = I.InvoiceId and I.LineNum = Stage.LineNum

	MERGE INTO InternetUser.Invoice as Invoice
	USING (
  
		SELECT S.[CustomerId]
			  ,S.[DataAreaId]
			  ,S.[SalesId]
			  ,S.[InvoiceDate]
			  ,S.[DueDate]
			  ,S.[InvoiceAmount]
			  ,S.[InvoiceCredit]
			  ,S.[CurrencyCode]
			  ,S.[InvoiceId]
			  ,S.[Payment]
			  ,S.[SalesType]
			  ,S.[CusomerRef]
			  ,S.[InvoicingName]
			  ,S.[InvoicingAddress]
			  ,S.[ContactPersonId]
			  ,S.[Printed]
			  ,S.[ReturnItemId]
			  ,S.[ItemDate]
			  ,S.[LineNum]
			  ,S.[ItemId]
			  ,S.[ItemName]
			  ,S.[SerialNumber]
			  ,S.[Quantity]
			  ,S.[SalesPrice]
			  ,S.[LineAmount]
			  ,S.[QuantityPhysical]
			  ,S.[Remain]
			  ,S.[DeliveryType]
			  ,S.[TaxAmount]
			  ,S.[LineAmountMst]
			  ,S.[TaxAmountMst]
			  ,S.[DetailCurrencyCode]
			  ,S.[Debit]
			  ,S.[Description]
			  ,S.[FileName]
			  ,S.[RecId]
			  ,S.[CreatedDate]
			  ,S.[ExtractDate]
			  ,S.[PackageLogKey]
		  FROM [Web].[InternetUser].[Stage_InvoiceOpenTransaction] as S
		  --INNER JOIN [Web].[InternetUser].[Invoice] as I ON S.invoiceId = I.invoiceId and I.LineNum = S.LineNum
		   --WHERE I.invoiceId = 'HI018878/13'
  
	) as Stage
	ON (Invoice.InvoiceId = Stage.InvoiceId AND Invoice.LineNum = Stage.LineNum)
	--WHEN MATCHED THEN	-- számlaszám és számla sor alapján egyezés esetén a számla tábla tartozás mezõ értékének frissítése
		--UPDATE SET Invoice.InvoiceCredit = Stage.InvoiceCredit, 
		--		   Invoice.Debit = CASE WHEN Stage.InvoiceCredit <> 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END
	WHEN NOT MATCHED BY TARGET THEN	-- ha nincs az Invoice táblában, de van a Stage táblában, akkor fel kell venni az Invoice-ba.
	  INSERT ( [CustomerId] ,[DataAreaId] ,[SalesId] ,[InvoiceDate] ,[DueDate]
				,[InvoiceAmount] ,[InvoiceCredit] ,[CurrencyCode] ,[InvoiceId] ,[Payment]
				,[SalesType] ,[CusomerRef] ,[InvoicingName] ,[InvoicingAddress] ,[ContactPersonId]
				,[Printed] ,[ReturnItemId] ,[ItemDate] ,[LineNum] ,[ItemId]
				,[ItemName] ,[Quantity] ,[SalesPrice] ,[LineAmount] ,[QuantityPhysical]
				,[Remain] ,[DeliveryType] ,[TaxAmount] ,[LineAmountMst] ,[TaxAmountMst]
				,[DetailCurrencyCode] ,[Debit] ,[Description] ,[FileName] ,[RecId]
				,[CreatedDate] ,[ExtractDate] ,[PackageLogKey])
	  VALUES ( Stage.[CustomerId] ,Stage.[DataAreaId] ,Stage.[SalesId] ,Stage.[InvoiceDate] ,Stage.[DueDate]
				,Stage.[InvoiceAmount] ,Stage.[InvoiceCredit] ,Stage.[CurrencyCode] ,Stage.[InvoiceId] ,Stage.[Payment]
				,Stage.[SalesType] ,Stage.[CusomerRef] ,Stage.[InvoicingName] ,Stage.[InvoicingAddress] ,Stage.[ContactPersonId]
				,Stage.[Printed] ,Stage.[ReturnItemId] ,Stage.[ItemDate] ,Stage.[LineNum] ,Stage.[ItemId]
				,Stage.[ItemName] ,Stage.[Quantity] ,Stage.[SalesPrice] ,Stage.[LineAmount] ,Stage.[QuantityPhysical]
				,Stage.[Remain] ,Stage.[DeliveryType] ,Stage.[TaxAmount] ,Stage.[LineAmountMst] ,Stage.[TaxAmountMst]
				,Stage.[DetailCurrencyCode] ,Stage.[Debit] ,Stage.[Description] ,Stage.[FileName] ,Stage.[RecId]
				,Stage.[CreatedDate] ,Convert(SmallDateTime, GetDate()) ,-1)
	WHEN NOT MATCHED BY SOURCE THEN		-- ha nincs benne a Stage táblában, de benne van az Invoice-ben, akkor törlés
		UPDATE SET Invoice.InvoiceCredit = 0, 
				   Invoice.Debit = CONVERT(BIT, 0);

RETURN
GO
GRANT EXECUTE ON [InternetUser].[InvoiceOpenTransactionMerge] TO InternetUser

GO
GRANT EXECUTE ON  [InternetUser].[InvoiceOpenTransactionMerge] TO [HRP_HEADOFFICE\AXPROXY]
GO

/*
exec [InternetUser].[InvoiceOpenTransactionMerge]
select * from [Web].[InternetUser].[Stage_InvoiceOpenTransaction]

*/