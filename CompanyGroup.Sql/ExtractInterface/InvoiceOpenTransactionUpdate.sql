/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban InvoiceOpenTransaction frissítése
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :

	select * from ExtractInterface.dbo.Stage_InvoiceOpenTransaction
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[InvoiceOpenTransactionUpdate];
GO
CREATE PROCEDURE [InternetUser].[InvoiceOpenTransactionUpdate]
AS
SET NOCOUNT ON

	-- képek duplikációk nélküli kikeresése
	;
	WITH Picture_CTE (ItemId, [FileName]) AS
	(
		SELECT ItemId, [FileName] 
		FROM Axdb.dbo.UPDKEPEK
		GROUP BY ItemId, [FileName], ELSODLEGESKEP
		HAVING ELSODLEGESKEP = 1 AND COUNT(*) = 1 AND ItemId <> ''
	)

	MERGE INTO dbo.Stage_InvoiceOpenTransaction as Stage
	USING (
  
		SELECT CIJ.InvoiceAccount, 
				   CIJ.ORDERACCOUNT as CustomerId, 
				   CIJ.DataAreaId as DataAreaId,
				   CIJ.SALESID as SalesId,  -- rendelesszam, azonosito
				   CIJ.INVOICEDATE as InvoiceDate,  -- szamla datuma
				   CIJ.DUEDATE as DueDate,  -- esedekesseg, ha kisebb mint a GetDate, akkor lejárt
				   CONVERT( decimal(20,2), CIJ.INVOICEAMOUNT ) as InvoiceAmount,  -- szamla vegosszege
				   CONVERT( decimal(20,2), ISNULL( CTO.AMOUNTCUR, 0 ) ) as InvoiceCredit,  -- szamla tartozas
				   CIJ.CurrencyCode as CurrencyCode,  
				   CIJ.INVOICEID as InvoiceId,  -- szla. szama
				   -- CIJ.PAYMENT as Payment,  -- fizetesi feltetelek
				   Pt.[Description] as Payment,	-- fizetesi feltetelek
				   CONVERT( INT, CIJ.SALESTYPE ) as SalesType,  -- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet	
				   CIJ.CUSTOMERREF as CusomerRef,  -- vevo hivatkozas
				   CIJ.INVOICINGNAME as InvoicingName,  -- szla. nev
				   CIJ.INVOICINGADDRESS as InvoicingAddress,  -- szla. cim
				   CIJ.CONTACTPERSONID as ContactPersonId,  -- kapcsolattarto
				   CONVERT( INT, CIJ.PRINTED ) as Printed,  --
				   CIJ.VISSZARUID as ReturnItemId, --
				   CIT.INVOICEDATE as ItemDate,  -- datum
				   CONVERT( INT, ISNULL(CIT.LINENUM, 0) ) as LineNum,
				   CIT.ITEMID as ItemId,  -- cikk
				   CONVERT(NVARCHAR(300), CIT.NAME) as Name,  -- cikk neve
				   '' as SerialNumber,
				   CONVERT( INT, ISNULL(CIT.QTY, 0) ) as Quantity,  -- mennyiseg
				   CONVERT( decimal(20,2), ISNULL(CIT.SALESPRICE, 0) ) as SalesPrice,  -- egysegar
				   CONVERT( decimal(20,2), ISNULL(CIT.LINEAMOUNT, 0) ) as LineAmount,  -- osszeg
				   CONVERT( INT, ISNULL(CIT.QTYPHYSICAL, 0) ) as QuantityPhysical,  -- mennyiseg
				   CONVERT( INT, ISNULL(CIT.REMAIN, 0) ) as Remain,  -- fennmarado mennyiseg
				   CONVERT( INT, ISNULL(CIT.DELIVERYTYPE, 0) ) as DeliveryType, -- 
				   CONVERT( decimal(20,2), ISNULL(CIT.TAXAMOUNT, 0) ) as TaxAmount,  --
				   CONVERT( decimal(20,2), ISNULL(CIT.LINEAMOUNTMST, 0) ) as LineAmountMst,  -- osszeg az alapertelmezett penznemben
				   CONVERT( decimal(20,2), ISNULL(CIT.TAXAMOUNTMST, 0) ) as TaxAmountMst, -- afa osszege az alapertelmezett penznemben
				   CIT.CurrencyCode as DetailCurrencyCode, 
				   CASE WHEN CTO.RefRecId IS NULL THEN 0 ELSE 1 END as Debit, 
				   ISNULL(Txt.TXT, '') as [Description], 
				   ISNULL(Picture.FileName, '') as [FileName], 
				   CIJ.RecId, 
				   CIJ.CreatedDate 
			FROM Axdb.dbo.CUSTINVOICEJOUR as CIJ
			INNER JOIN  Axdb.dbo.CUSTINVOICETRANS as CIT ON CIJ.INVOICEID = CIT.INVOICEID AND CIJ.DATAAREAID = CIT.DATAAREAID
			INNER JOIN Axdb.dbo.PAYMTERM AS Pt ON CIJ.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
			LEFT JOIN Axdb.dbo.CustTrans as CT on CT.DATAAREAID = CIJ.DATAAREAID and CT.INVOICE = CIJ.INVOICEID AND CT.AccountNum = CIJ.InvoiceAccount
			LEFT JOIN Axdb.dbo.CustTransOpen as CTO on CTO.DATAAREAID = CT.DATAAREAID and CTO.RefRecId = CT.RECID
			LEFT OUTER JOIN Axdb.dbo.InventTxt as txt ON txt.ITEMID = CIT.ITEMID AND txt.DATAAREAID = CIT.DATAAREAID AND txt.LANGUAGEID = 'HU'
			LEFT OUTER JOIN Picture_CTE as Picture ON Picture.ItemId = CIT.ItemId
			WHERE CIJ.DATAAREAID IN ('bsc', 'hrp') 
				  AND YEAR(CIJ.INVOICEDATE) >= 2008
				  AND ISNULL( CTO.AMOUNTCUR, 0 ) <> 0
  
	) as Transactions
	ON (Stage.InvoiceId = Transactions.InvoiceId AND Stage.LineNum = Transactions.LineNum)
	WHEN MATCHED THEN	-- számlaszám és számla sor alapján egyezés esetén a Stage tábla tartozás mezõ értékének frissítése
		UPDATE SET Stage.InvoiceCredit = Transactions.InvoiceCredit, 
				   Stage.Debit = CASE WHEN Transactions.InvoiceCredit <> 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END
	WHEN NOT MATCHED BY TARGET THEN	-- ha nincs a Stage táblában, de van a Transactions táblában, akkor fel kell venni a Stage-be.
	  INSERT ( [CustomerId] ,[DataAreaId] ,[SalesId] ,[InvoiceDate] ,[DueDate]
				,[InvoiceAmount] ,[InvoiceCredit] ,[CurrencyCode] ,[InvoiceId] ,[Payment]
				,[SalesType] ,[CusomerRef] ,[InvoicingName] ,[InvoicingAddress] ,[ContactPersonId]
				,[Printed] ,[ReturnItemId] ,[ItemDate] ,[LineNum] ,[ItemId]
				,[ItemName] ,[Quantity] ,[SalesPrice] ,[LineAmount] ,[QuantityPhysical]
				,[Remain] ,[DeliveryType] ,[TaxAmount] ,[LineAmountMst] ,[TaxAmountMst]
				,[DetailCurrencyCode] ,[Debit] ,[Description] ,[FileName] ,[RecId]
				,[CreatedDate] ,[ExtractDate] ,[PackageLogKey])
	  VALUES ( Transactions.[CustomerId] ,Transactions.[DataAreaId] ,Transactions.[SalesId] ,Transactions.[InvoiceDate] ,Transactions.[DueDate]
				,Transactions.[InvoiceAmount] ,Transactions.[InvoiceCredit] ,Transactions.[CurrencyCode] ,Transactions.[InvoiceId] ,Transactions.[Payment]
				,Transactions.[SalesType] ,Transactions.[CusomerRef] ,Transactions.[InvoicingName] ,Transactions.[InvoicingAddress] ,Transactions.[ContactPersonId]
				,Transactions.[Printed] ,Transactions.[ReturnItemId] ,Transactions.[ItemDate] ,Transactions.[LineNum] ,Transactions.[ItemId]
				,Transactions.[Name] ,Transactions.[Quantity] ,Transactions.[SalesPrice] ,Transactions.[LineAmount] ,Transactions.[QuantityPhysical]
				,Transactions.[Remain] ,Transactions.[DeliveryType] ,Transactions.[TaxAmount] ,Transactions.[LineAmountMst] ,Transactions.[TaxAmountMst]
				,Transactions.[DetailCurrencyCode] ,Transactions.[Debit] ,Transactions.[Description] ,Transactions.[FileName] ,Transactions.[RecId]
				,Transactions.[CreatedDate] ,Convert(SmallDateTime, GetDate()) ,-1)
	WHEN NOT MATCHED BY SOURCE THEN		-- ha nincs benne a Transactions táblában, de benne van a Stage-ben, akkor törlés
		UPDATE SET Stage.InvoiceCredit = 0, Stage.Debit = CONVERT(BIT, 0);

RETURN
GO
GRANT EXECUTE ON [InternetUser].[InvoiceOpenTransactionUpdate] TO InternetUser

GO
GRANT EXECUTE ON  [InternetUser].[InvoiceOpenTransactionUpdate] TO [HRP_HEADOFFICE\AXPROXY]
GO

-- exec [InternetUser].[InvoiceOpenTransactionUpdate];