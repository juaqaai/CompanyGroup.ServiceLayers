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
		FROM Axdb_20130131.dbo.UPDKEPEK
		GROUP BY ItemId, [FileName], ELSODLEGESKEP
		HAVING ELSODLEGESKEP = 1 AND COUNT(*) = 1 AND ItemId <> ''
	)

	SELECT H.ORDERACCOUNT as CustomerId, 
		   H.DataAreaId as DataAreaId,
		   H.SALESID as SalesId,  -- rendelesszam, azonosito
		   H.INVOICEDATE as InvoiceDate,  -- szamla datuma
		   H.DUEDATE as DueDate,  -- esedekesseg, ha kisebb mint a GetDate, akkor lejárt
		   CONVERT( decimal(20,2), H.INVOICEAMOUNT ) as InvoiceAmount,  -- szamla vegosszege
           CONVERT( decimal(20,2), ISNULL( O.AMOUNTMST, 0 ) ) as InvoiceCredit,  -- szamla tartozas
		   H.CurrencyCode as CurrencyCode,  
		   H.INVOICEID as InvoiceId,  -- szla. szama
		   -- H.PAYMENT as Payment,  -- fizetesi feltetelek
		   Pt.[Description] as Payment,	-- fizetesi feltetelek
		   CONVERT( INT, H.SALESTYPE ) as SalesType,  -- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet	
		   H.CUSTOMERREF as CusomerRef,  -- vevo hivatkozas
		   H.INVOICINGNAME as InvoicingName,  -- szla. nev
		   H.INVOICINGADDRESS as InvoicingAddress,  -- szla. cim
		   H.CONTACTPERSONID as ContactPersonId,  -- kapcsolattarto
		   CONVERT( INT, H.PRINTED ) as Printed,  --
		   H.VISSZARUID as ReturnItemId, --
		   D.INVOICEDATE as ItemDate,  -- datum
		   CONVERT( INT, ISNULL(D.LINENUM, 0) ) as LineNum,
		   D.ITEMID as ItemId,  -- cikk
		   CONVERT(NVARCHAR(300), D.NAME) as Name,  -- cikk neve
		   '' as SerialNumber,
		   CONVERT( INT, ISNULL(D.QTY, 0) ) as Quantity,  -- mennyiseg
		   CONVERT( decimal(20,2), ISNULL(D.SALESPRICE, 0) ) as SalesPrice,  -- egysegar
		   CONVERT( decimal(20,2), ISNULL(D.LINEAMOUNT, 0) ) as LineAmount,  -- osszeg
		   CONVERT( INT, ISNULL(D.QTYPHYSICAL, 0) ) as QuantityPhysical,  -- mennyiseg
		   CONVERT( INT, ISNULL(D.REMAIN, 0) ) as Remain,  -- fennmarado mennyiseg
		   CONVERT( INT, ISNULL(D.DELIVERYTYPE, 0) ) as DeliveryType, -- 
		   CONVERT( decimal(20,2), ISNULL(D.TAXAMOUNT, 0) ) as TaxAmount,  --
		   CONVERT( decimal(20,2), ISNULL(D.LINEAMOUNTMST, 0) ) as LineAmountMst,  -- osszeg az alapertelmezett penznemben
		   CONVERT( decimal(20,2), ISNULL(D.TAXAMOUNTMST, 0) ) as TaxAmountMst, -- afa osszege az alapertelmezett penznemben
		   D.CurrencyCode as DetailCurrencyCode, 
		   CASE WHEN o.RefRecId IS NULL THEN 0 ELSE 1 END as Debit, 
		   ISNULL(Txt.TXT, '') as [Description], 
		   ISNULL(Picture.FileName, '') as [FileName], 
		   H.RecId, 
		   H.CreatedDate 
		   --D.SerialNum as SerialNumber, -- sorozatszam
		   --CONVERT( INT, D.VisszaruQty ) as VisszaruQty

	FROM Axdb_20130131.dbo.CUSTINVOICEJOUR as H
	INNER JOIN  Axdb_20130131.dbo.CUSTINVOICETRANS as D ON H.INVOICEID = D.INVOICEID AND H.DATAAREAID = D.DATAAREAID
	INNER JOIN Axdb_20130131.dbo.PAYMTERM AS Pt ON H.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
	LEFT JOIN Axdb_20130131.dbo.CustTrans as t on t.DATAAREAID = h.DATAAREAID and t.INVOICE = H.INVOICEID 
	LEFT JOIN Axdb_20130131.dbo.CustTransOpen as o on o.DATAAREAID = t.DATAAREAID and o.RefRecId = t.RECID
	LEFT OUTER JOIN Axdb_20130131.dbo.InventTxt as txt ON txt.ITEMID = D.ITEMID AND txt.DATAAREAID = D.DATAAREAID AND txt.LANGUAGEID = 'HU'
	LEFT OUTER JOIN Picture_CTE as Picture ON Picture.ItemId = D.ItemId
	WHERE H.DATAAREAID IN ('bsc', 'hrp') 
		  AND YEAR(H.INVOICEDATE) >= 2008
		  AND H.RECID > @RecId
		  -- AND o.RefRecId <> @iDebit 
		  -- AND H.DUEDATE <= case when @bOverDue = 0 then H.DUEDATE else getdate() end
		  -- AND H.INVOICEDATE BETWEEN @dtDateFrom AND @dtDateTo
	ORDER BY H.INVOICEDATE desc, H.INVOICEID desc, D.LINENUM;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[InvoiceExtract] TO InternetUser

GO

/*
select top 100 * from Axdb_20130131.dbo.InventTxt
select top 100 * from Axdb_20130131.dbo.CustTransOpen 
*/
-- exec [InternetUser].[InvoiceExtract];