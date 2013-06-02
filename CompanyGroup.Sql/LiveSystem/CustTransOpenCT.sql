/*
NEM HASZNÁLJUK !!!
*/

USE [Axdb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.CustTransOpenCT
GO
CREATE PROCEDURE InternetUser.CustTransOpenCT( @LastVersion BIGINT = 0 )
AS
SET NOCOUNT ON
	-- DECLARE @LastVersion BIGINT = 0;
	IF (@LastVersion = 0)
	BEGIN
		SET @LastVersion = ISNULL((SELECT MAX(LastVersion) FROM ExtractInterface.dbo.SyncMetadata WHERE TableName = 'CustTransOpen' AND [Status] = 1), 0);
	END

	SELECT ct.SYS_CHANGE_VERSION as [Version], 
		   ct.Sys_Change_Operation as Operation, 
		   cij.InvoiceAccount  as CustomerId, -- vevõkód
--		   cij.ORDERACCOUNT as CustomerId, 
		   cij.DataAreaId as DataAreaId,
--		   cij.SALESID as SalesId,  -- rendelesszam, azonosito
--		   cij.INVOICEDATE as InvoiceDate,  -- szamla datuma
--		   cij.DUEDATE as DueDate,  -- esedekesseg, ha kisebb mint a GetDate, akkor lejárt
--		   CONVERT( decimal(20,2), cij.INVOICEAMOUNT ) as InvoiceAmount,  -- szamla vegosszege
           CONVERT( decimal(20,2), ISNULL( cto.AMOUNTCUR, 0 ) ) as InvoiceCredit,  -- szamla tartozas
--		   cij.CurrencyCode as CurrencyCode,  
		   cij.INVOICEID as InvoiceId,  -- szla. szama
--		   Pt.[Description] as Payment,	-- fizetesi feltetelek
--		   CONVERT( INT, cij.SALESTYPE ) as SalesType,  -- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet	
--		   cij.CUSTOMERREF as CusomerRef,  -- vevo hivatkozas
--		   cij.INVOICINGNAME as InvoicingName,  -- szla. nev
--		   cij.INVOICINGADDRESS as InvoicingAddress,  -- szla. cim
--		   cij.CONTACTPERSONID as ContactPersonId,  -- kapcsolattarto
		   CONVERT( INT, cij.PRINTED ) as Printed,  --
--		   cij.VISSZARUID as ReturnItemId, --
		   cit.INVOICEDATE as ItemDate,  -- datum
		   CONVERT( INT, ISNULL(cit.LINENUM, 0) ) as LineNum,
		   cit.ITEMID as ItemId,  -- cikk
		   CONVERT(NVARCHAR(300), cit.NAME) as Name,  -- cikk neve
--		   CONVERT(NVARCHAR(40),'') as SerialNumber,
		   CONVERT( INT, ISNULL(cit.QTY, 0) ) as Quantity,  -- mennyiseg
		   CONVERT( decimal(20,2), ISNULL(cit.SALESPRICE, 0) ) as SalesPrice,  -- egysegar
		   CONVERT( decimal(20,2), ISNULL(cit.LINEAMOUNT, 0) ) as LineAmount,  -- osszeg
		   CONVERT( INT, ISNULL(cit.QTYPHYSICAL, 0) ) as QuantityPhysical,  -- mennyiseg
		   CONVERT( INT, ISNULL(cit.REMAIN, 0) ) as Remain,  -- fennmarado mennyiseg
		   CONVERT( INT, ISNULL(cit.DELIVERYTYPE, 0) ) as DeliveryType, -- 
		   CONVERT( decimal(20,2), ISNULL(cit.TAXAMOUNT, 0) ) as TaxAmount,  --
		   CONVERT( decimal(20,2), ISNULL(cit.LINEAMOUNTMST, 0) ) as LineAmountMst,  -- osszeg az alapertelmezett penznemben
		   CONVERT( decimal(20,2), ISNULL(cit.TAXAMOUNTMST, 0) ) as TaxAmountMst, -- afa osszege az alapertelmezett penznemben
		   cit.CurrencyCode as DetailCurrencyCode, 
		   CASE WHEN cto.RefRecId IS NULL THEN 0 ELSE 1 END as Debit
--		   ISNULL(Txt.TXT, '') as [Description], 
--		   ISNULL(Picture.FileName, '') as [FileName], 
--		   cij.RecId, 
--		   cij.CreatedDate 

--select cto.*
	FROM changetable(changes Axdb.dbo.CustTransOpen, 0) as ct
	LEFT OUTER JOIN Axdb.dbo.CustTransOpen as cto on cto.DATAAREAID = ct.DATAAREAID and cto.RecId = ct.RecId
	LEFT OUTER JOIN Axdb.dbo.CustTrans as t on t.DATAAREAID = ct.DATAAREAID and t.RecId = cto.RefRecId
	LEFT OUTER JOIN Axdb.dbo.CUSTINVOICEJOUR as cij on cij.InvoiceId = t.Invoice and cij.InvoiceAccount = t.AccountNum and cij.DATAAREAID = t.DATAAREAID
	LEFT OUTER JOIN Axdb.dbo.CUSTINVOICETRANS as cit on cit.INVOICEID = cij.INVOICEID and 
												 cit.DATAAREAID = cij.DATAAREAID and 
												 cit.SalesId = cij.SalesId and 
												 cit.InvoiceDate = cij.InvoiceDate and 
												 cit.numberSequenceGroup = cij.numberSequenceGroup 
	where ct.DataAreaId IN ('bsc', 'hrp')

	ORDER BY cij.INVOICEDATE desc, cij.INVOICEID desc, cit.LINENUM;

RETURN

GO
GRANT EXECUTE ON InternetUser.CustTransOpenCT TO [HRP_HEADOFFICE\AXPROXY]
GO
GRANT EXECUTE ON InternetUser.CustTransOpenCT TO InternetUser
GO

/*
select * from changetable(changes Axdb.dbo.CustTransOpen, 0) as ct

exec InternetUser.CustTransOpenCT;


*/

select t.*
	FROM changetable(changes Axdb.dbo.CustTransOpen, 0) as ct
	LEFT OUTER JOIN Axdb.dbo.CustTransOpen as cto on cto.DATAAREAID = ct.DATAAREAID and cto.RecId = ct.RecId
	LEFT OUTER JOIN Axdb.dbo.CustTrans as t on t.DATAAREAID = ct.DATAAREAID and t.RecId = ct.RecId
	where ct.DataAreaId IN ('bsc', 'hrp') and ct.Sys_Change_Operation = 'D'