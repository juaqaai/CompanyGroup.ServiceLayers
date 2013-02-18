USE ExtractInterface
GO
/****** Object:  StoredProcedure [InternetUser].[web_GetSales]    Script Date: 2012.09.18. 20:55:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [InternetUser].[InvoiceList];
GO
CREATE PROCEDURE [InternetUser].[InvoiceList]( @CustomerId NVARCHAR(10) = '',	--vevokod
											   @DataAreaId NVARCHAR(3) = 'Hrp')	--vallalat kod
											   --@bDebit BIT = 0,					--0: mind, <>0 kifizetetlen
											   --@bOverDue BIT = 0,					--0: mind, <>0 lejart 
											   --@dtDateFrom DATETIME = NULL, 
											   --@dtDateTo DATETIME = NULL )										
AS
SET NOCOUNT ON
--	SELECT H.SALESID as sSalesID,  -- rendelesszam
--		   H.ORDERACCOUNT as sOrderAccount,  -- vevokod
--		   H.INVOICEDATE as dtInvoiceDate,  -- szamla datuma
--		   H.DUEDATE as dtDueDate,  -- esedekesseg
--		   CONVERT( INT, H.INVOICEAMOUNT ) as iInvoiceAmount,  -- szamla vegosszege
--		   H.INVOICEID as sInvoiceID,  -- szla. szama
--		   H.DELIVERYNAME as sDeliveryName,  -- nev - vallalat nev szallitasi cimen
--		   H.DELIVERYADDRESS as sDeliveryAddress,  -- szallitasi cim
--		   H.PAYMENT as sPayMent,  -- fizetesi feltetelek
--		   H.DLVZIPCODE as sDeliveryZipCode,  -- szall. cim irszam
--		   CONVERT( INT, H.SALESTYPE ) as iSalesType,  -- rendelessor tipusa
--		   H.CUSTOMERREF as sCusomerRef,  -- vevo hivatkozas
--		   CONVERT( INT, H.INCLTAX ) as iIncludeTax,  -- ado tartalom
--		   H.INVOICINGNAME as sInvoicingName,  -- szla. nev
--		   H.INVOICINGADDRESS as sInvoicingAddress,  -- szla. cim
--		   H.INVZIPCODE as sInvZipCode,  -- szla. irszam
--		   H.CONTACTPERSONID as sContactPersonID,  -- kapcsolattarto
--		   H.DELIVERYCITY as sDeliveryCity,  -- szall. varos
--		   H.DELIVERYSTREET as sDeliveryStreet,  -- szall. utca
--		   H.INVOICECITY as sInvoiceCity,  -- szla. varos
--		   H.INVOICESTREET as sInvoiceStreet,  -- szla. utca
--		   CONVERT( INT, H.PRINTED ) as bPrinted,  --
--		   H.VISSZARUID as sVisszaruID, --
--		   D.INVOICEDATE as dtItemDate,  -- datum
--		   CONVERT( INT, D.LINENUM ) as iLineNum,
--		   D.ITEMID as sItemID,  -- cikk
--		   D.NAME as sName,  -- cikk neve
--		   CONVERT( INT, D.QTY ) as iQuantity,  -- mennyiseg
--		   CONVERT( INT, D.SALESPRICE ) as iSalesPrice,  -- egysegar
--		   CONVERT( INT, D.LINEAMOUNT ) as iLineAmount,  -- osszeg
--		   CONVERT( INT, D.QTYPHYSICAL ) as iQuantityPhysical,  -- mennyiseg
--		   CONVERT( INT, D.REMAIN ) as iRemain,  -- fennmarado mennyiseg
--		   D.SALESID as sSalesID,  -- rendeles azonosito
--		   CONVERT( INT, D.DELIVERYTYPE ) as iDeliveryType, -- 
--		   CONVERT( INT, D.TAXAMOUNT ) as iTaxAmount,  --
--		   D.ORIGSALESID as sOrigSalesID,  --
--		   CONVERT( INT, D.LINEAMOUNTMST ) as iLineAmountMst,  -- osszeg az alapertelmezett penznemben
--		   CONVERT( INT, D.TAXAMOUNTMST ) as iTaxAmountMst, -- afa osszege az alapertelmezett penznemben
--		   D.SerialNum as sSerialNumber, -- sorozatszam
--		   CONVERT( INT, D.VisszaruQty ) as iVisszaruQty
--
--	FROM AxDb.dbo.CUSTINVOICEJOUR as H
--	INNER JOIN  AxDb.dbo.CUSTINVOICETRANS as D ON H.INVOICEID = D.INVOICEID AND H.DATAAREAID = D.DATAAREAID
--	WHERE H.ORDERACCOUNT = @sCustomerID AND H.DATAAREAID = @sDataAreaID AND DATEDIFF( day, '2008.01.01 00:00:00', H.INVOICEDATE ) > 0
--	ORDER BY H.INVOICEDATE desc, H.INVOICEID desc, D.LINENUM

DECLARE @bDebit BIT = 0; --0: mind, <>0 kifizetetlen
	DECLARE	@bOverDue BIT = 0;					--0: mind, <>0 lejart 
	DECLARE @dtDateFrom DATETIME = NULL; 
	DECLARE @dtDateTo DATETIME = NULL;

	DECLARE @iDebit INT;

	SET @iDebit = CASE WHEN @bDebit = 1 THEN NULL ELSE 0 END;

--if(@iDebit <> 0) 
--	set @iDebit = null

	IF ( @dtDateFrom IS NULL ) 
	BEGIN
		SET @dtDateFrom = '2008.01.01 00:00:00';
	END

	IF ( @dtDateTo IS NULL ) 
	BEGIN
		SET @dtDateTo = GETDATE();
	END

	SELECT H.ORDERACCOUNT as CustomerId, 
		   H.DataAreaId as DataAreaId,
		   H.SALESID as SalesId,  -- rendelesszam, azonosito
		   H.INVOICEDATE as InvoiceDate,  -- szamla datuma
		   H.DUEDATE as DueDate,  -- esedekesseg
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
		   CASE WHEN o.RefRecId IS NULL THEN 0 ELSE 1 END as Debit
		   --D.SerialNum as SerialNumber, -- sorozatszam
		   --CONVERT( INT, D.VisszaruQty ) as VisszaruQty

	FROM axdb_20120614.dbo.CUSTINVOICEJOUR as H
	INNER JOIN  axdb_20120614.dbo.CUSTINVOICETRANS as D ON H.INVOICEID = D.INVOICEID AND H.DATAAREAID = D.DATAAREAID
	INNER JOIN axdb_20120614.dbo.PAYMTERM AS Pt ON H.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
	LEFT JOIN axdb_20120614.dbo.custTrans as t on t.DATAAREAID = h.DATAAREAID and t.INVOICE = H.INVOICEID 
	LEFT JOIN axdb_20120614.dbo.custTransOpen as o on o.DATAAREAID = t.DATAAREAID and o.RefRecId = t.RECID
	WHERE H.ORDERACCOUNT = CASE WHEN @CustomerId = '' THEN H.ORDERACCOUNT ELSE @CustomerId END
		  AND H.DATAAREAID = @DataAreaId 
		  AND YEAR(H.INVOICEDATE) >= 2008
		  -- AND o.RefRecId <> @iDebit 
		  -- AND H.DUEDATE <= case when @bOverDue = 0 then H.DUEDATE else getdate() end
		  -- AND H.INVOICEDATE BETWEEN @dtDateFrom AND @dtDateTo
	ORDER BY H.INVOICEDATE desc, H.INVOICEID desc, D.LINENUM;

RETURN
GO

-- exec [InternetUser].[InvoiceList] 'V000787', 'hrp';
-- exec [InternetUser].[InvoiceList] '', 'hrp';

-- select top 100 * from AxDb.dbo.CUSTINVOICEJOUR
USE Web
GO
DROP PROCEDURE [InternetUser].[InvoiceSelect];
GO
CREATE PROCEDURE [InternetUser].[InvoiceSelect]( @CustomerId NVARCHAR(10) = '',	--vevokod
											     @Debit BIT = 0,				--0: mind, 1 kifizetetlen
											     @OverDue BIT = 0 )				--0: mind, 1 lejart 
											   --@dtDateFrom DATETIME = NULL, 
											   --@dtDateTo DATETIME = NULL )										
AS
SET NOCOUNT ON

	SELECT CustomerId, 
		   DataAreaId,
		   SalesId,  -- rendelesszam, azonosito
		   InvoiceDate,  -- szamla datuma
		   DueDate,  -- esedekesseg
		   InvoiceAmount,  -- szamla vegosszege
           InvoiceCredit,  -- szamla tartozas
		   CurrencyCode,  
		   InvoiceId,  -- szla. szama
		   Payment,	-- fizetesi feltetelek
		   SalesType,  -- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet	
		   CusomerRef,  -- vevo hivatkozas
		   InvoicingName,  -- szla. nev
		   InvoicingAddress,  -- szla. cim
		   ContactPersonId,  -- kapcsolattarto
		   Printed,  --
		   ReturnItemId, --
		   ItemDate,  -- datum
		   LineNum,
		   ItemId,  -- cikk
		   ItemName,  -- cikk neve
		   Quantity,  -- mennyiseg
		   SalesPrice,  -- egysegar
		   LineAmount,  -- osszeg
		   QuantityPhysical,  -- mennyiseg
		   Remain,  -- fennmarado mennyiseg

		   DeliveryType, -- 
		   TaxAmount,  --
		   LineAmountMst,  -- osszeg az alapertelmezett penznemben
		   TaxAmountMst, -- afa osszege az alapertelmezett penznemben
		   DetailCurrencyCode

	FROM InternetUser.Invoice 
	WHERE CustomerId = @CustomerId 
		  AND Debit = CASE WHEN @Debit = 1 THEN @Debit ELSE Debit END
		  AND DueDate <= CASE WHEN @OverDue = 1 then GETDATE() ELSE DueDate END
		  -- AND H.INVOICEDATE BETWEEN @dtDateFrom AND @dtDateTo
	ORDER BY InvoiceDate desc, InvoiceId desc, LineNum;

RETURN
GO
GRANT EXECUTE ON InternetUser.InvoiceSelect TO InternetUser
GO

-- EXEC InternetUser.InvoiceSelect 'V001446', 1, 1;