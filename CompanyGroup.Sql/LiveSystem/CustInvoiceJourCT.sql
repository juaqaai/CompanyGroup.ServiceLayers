/*
Lekérdezi a számla info változásokat a legutolsó szinkronizáció óta
*/

USE [Axdb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.CustInvoiceJourCT
GO
CREATE PROCEDURE InternetUser.CustInvoiceJourCT( @LastVersion BIGINT = 0 )
AS
SET NOCOUNT ON
	-- DECLARE @LastVersion BIGINT = 0;
	IF (@LastVersion = 0)
	BEGIN
		SET @LastVersion = ISNULL((SELECT MAX(LastVersion) FROM ExtractInterface.dbo.SyncMetadata WHERE TableName = 'CustInvoiceJour' AND [Status] = 1), 0);
	END

	-- képek duplikációk nélküli kikeresése
	;
	WITH Picture_CTE (ItemId, [FileName]) AS
	(
		SELECT ItemId, [FileName] 
		FROM Axdb.dbo.UPDKEPEK
		GROUP BY ItemId, [FileName], ELSODLEGESKEP
		HAVING ELSODLEGESKEP = 1 AND COUNT(*) = 1 AND ItemId <> ''
	)

	SELECT ct.SYS_CHANGE_VERSION as [Version], 
		   ct.Sys_Change_Operation as Operation, 
		   H.InvoiceAccount, 
		   H.ORDERACCOUNT as CustomerId, 
		   H.DataAreaId as DataAreaId,
		   H.SALESID as SalesId,  -- rendelesszam, azonosito
		   H.INVOICEDATE as InvoiceDate,  -- szamla datuma
		   H.DUEDATE as DueDate,  -- esedekesseg, ha kisebb mint a GetDate, akkor lejárt
		   CONVERT( decimal(20,2), H.INVOICEAMOUNT ) as InvoiceAmount,  -- szamla vegosszege
           CONVERT( decimal(20,2), ISNULL( O.AMOUNTCUR, 0 ) ) as InvoiceCredit,  -- szamla tartozas
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
		   CONVERT(NVARCHAR(40),'') as SerialNumber,
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

	FROM changetable(changes Axdb.dbo.CUSTINVOICEJOUR, @LastVersion) as ct
	INNER JOIN Axdb.dbo.CUSTINVOICEJOUR as H ON ct.RecId = H.RecId AND ct.DataAreaId = H.DataAreaId
	INNER JOIN Axdb.dbo.CUSTINVOICETRANS as D ON H.INVOICEID = D.INVOICEID AND H.DATAAREAID = D.DATAAREAID
	INNER JOIN Axdb.dbo.PAYMTERM AS Pt ON H.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
	LEFT JOIN Axdb.dbo.CustTrans as t on t.DATAAREAID = h.DATAAREAID and t.INVOICE = H.INVOICEID 
	LEFT JOIN Axdb.dbo.CustTransOpen as o on o.DATAAREAID = t.DATAAREAID and o.RefRecId = t.RECID
	LEFT OUTER JOIN Axdb.dbo.InventTxt as txt ON txt.ITEMID = D.ITEMID AND txt.DATAAREAID = D.DATAAREAID AND txt.LANGUAGEID = 'HU'
	LEFT OUTER JOIN Picture_CTE as Picture ON Picture.ItemId = D.ItemId
	WHERE H.DATAAREAID IN ('bsc', 'hrp') 
		  AND YEAR(H.INVOICEDATE) >= 2008
		  -- AND o.RefRecId <> @iDebit 
		  -- AND H.DUEDATE <= case when @bOverDue = 0 then H.DUEDATE else getdate() end
		  -- AND H.INVOICEDATE BETWEEN @dtDateFrom AND @dtDateTo
	ORDER BY H.INVOICEDATE desc, H.INVOICEID desc, D.LINENUM;

RETURN

GO
GRANT EXECUTE ON InternetUser.CustInvoiceJourCT TO [HRP_HEADOFFICE\AXPROXY]
GO
GRANT EXECUTE ON InternetUser.CustInvoiceJourCT TO InternetUser
GO
-- exec InternetUser.CustInvoiceJourCT 1310

/*
select top 1 * from Axdb.dbo.CustInvoiceJour
select * from changetable(changes Axdb.dbo.CustInvoiceJour, 0) as ct

Operation	Version	CustomerId	DataAreaId	SalesId	CreatedDate	ShippingDateRequested	CurrencyCode	Payment	SalesHeaderType	SalesHeaderStatus	CustomerOrderNo	DlvTerm	LineNum	SalesStatus	ProductId	ProductName	SalesPrice	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	ItemDate	FileName
I	52401	V001446	hrp	VR636575	2013-04-28 00:00:00.000	2013-04-28 00:00:00.000	HUF	Átutalás 21 napra	Standard	1		RAKTÁRBÓL	1	1	X55ASX044D	ASUS X55A-SX044D   15.6" HD Pentium Dual Celeron B820, 2GB,320GB ,webcam, DVD DL	64290	1	64290	0	1	6	KULSO	2013-04-28 00:00:00.000	X55ASX044D.jpg

Operation	Version	CustAccount	DataAreaId	SalesId	CreatedDate	ShippingDateRequested	CurrencyCode	Description	SalesHeaderType	SalesStatus	VEVORENDELESSZAMA	DLVTERM	LineNum	SalesStatus	ProductId	ProductName	SalesPrice	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	ItemDate	FileName
I	52401	V001446	hrp	VR636575	2013-04-28 00:00:00.000	2013-04-28 00:00:00.000	HUF	Átutalás 21 napra	Standard	1		RAKTÁRBÓL	1	1	X55ASX044D	ASUS X55A-SX044D   15.6" HD Pentium Dual Celeron B820, 2GB,320GB ,webcam, DVD DL	64290	1	64290	0	1	6	KULSO	2013-04-28 00:00:00.000	X55ASX044D.jpg
*/