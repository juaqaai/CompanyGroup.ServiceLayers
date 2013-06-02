/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban Invoice tábla szerinti lekérdezés
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[InvoiceExtract];
GO
CREATE PROCEDURE [InternetUser].[InvoiceExtract] (@RecId BIGINT = 0)
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
		   --CIT.SerialNum as SerialNumber, -- sorozatszam
		   --CONVERT( INT, CIT.VisszaruQty ) as VisszaruQty
/*
CustTrans.Invoice = CustInvoiceJour.InvoiceID AND
CustTrans.AccountNum = CustInvoiceJour.InvoiceAccount AND
CustTrans.TransDate = CustInvoiceJour.InvoiceDate 
*/

	FROM Axdb.dbo.CUSTINVOICEJOUR as CIJ
	INNER JOIN  Axdb.dbo.CUSTINVOICETRANS as CIT ON CIJ.INVOICEID = CIT.INVOICEID AND CIJ.DATAAREAID = CIT.DATAAREAID
	INNER JOIN Axdb.dbo.PAYMTERM AS Pt ON CIJ.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
	LEFT JOIN Axdb.dbo.CustTrans as CT on CT.DATAAREAID = CIJ.DATAAREAID and CT.INVOICE = CIJ.INVOICEID AND CT.AccountNum = CIJ.InvoiceAccount
	LEFT JOIN Axdb.dbo.CustTransOpen as CTO on CTO.DATAAREAID = CT.DATAAREAID and CTO.RefRecId = CT.RECID
	LEFT OUTER JOIN Axdb.dbo.InventTxt as txt ON txt.ITEMID = CIT.ITEMID AND txt.DATAAREAID = CIT.DATAAREAID AND txt.LANGUAGEID = 'HU'
	LEFT OUTER JOIN Picture_CTE as Picture ON Picture.ItemId = CIT.ItemId
	WHERE CIJ.DATAAREAID IN ('bsc', 'hrp') 
		  AND YEAR(CIJ.INVOICEDATE) >= 2008
		  AND CIJ.RECID > @RecId
		  -- AND CTO.RefRecId <> @iDebit 
		  -- AND CIJ.DUEDATE <= case when @bOverDue = 0 then CIJ.DUEDATE else getdate() end
		  -- AND CIJ.INVOICEDATE BETWEEN @dtDateFrom AND @dtDateTo
	ORDER BY CIJ.INVOICEDATE desc, CIJ.INVOICEID desc, CIT.LINENUM;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[InvoiceExtract] TO InternetUser

GO

/*
select top 100 * from Axdb.dbo.InventTxt
select top 100 * from Axdb.dbo.CustTransOpen order by RecId desc
select top 100 * from Axdb.dbo.CustTrans order by RecId desc
*/
-- exec [InternetUser].[InvoiceExtract];