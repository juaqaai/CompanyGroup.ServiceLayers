USE WebDb_Test
GO
/****** Object:  StoredProcedure [InternetUser].[web_GetSales]    Script Date: 2012.09.18. 20:55:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [InternetUser].[cms_InvoiceList];
GO
CREATE PROCEDURE [InternetUser].[cms_InvoiceList]( @CustomerId NVARCHAR(10),			--vevokod
												   @DataAreaId NVARCHAR(3) = 'Hrp')	--vallalat kod
											   --@bDebit BIT = 0,						--0: mind, <>0 kifizetetlen
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
--
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

	SELECT H.SALESID as SalesId,  -- rendelesszam, azonosito
		   H.INVOICEDATE as InvoiceDate,  -- szamla datuma
		   H.DUEDATE as DueDate,  -- esedekesseg
		   CONVERT( INT, H.INVOICEAMOUNT ) as InvoiceAmount,  -- szamla vegosszege
           CONVERT( INT, ISNULL( O.AMOUNTMST, 0 ) ) as InvoiceCredit,  -- szamla tartozas
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
		   CONVERT( INT, D.LINENUM ) as LineNum,
		   D.ITEMID as ItemId,  -- cikk
		   D.NAME as Name,  -- cikk neve
		   CONVERT( INT, D.QTY ) as Quantity,  -- mennyiseg
		   CONVERT( INT, D.SALESPRICE ) as SalesPrice,  -- egysegar
		   CONVERT( INT, D.LINEAMOUNT ) as LineAmount,  -- osszeg
		   CONVERT( INT, D.QTYPHYSICAL ) as QuantityPhysical,  -- mennyiseg
		   CONVERT( INT, D.REMAIN ) as Remain,  -- fennmarado mennyiseg

		   CONVERT( INT, D.DELIVERYTYPE ) as DeliveryType, -- 
		   CONVERT( INT, D.TAXAMOUNT ) as TaxAmount,  --
		   CONVERT( INT, D.LINEAMOUNTMST ) as LineAmountMst,  -- osszeg az alapertelmezett penznemben
		   CONVERT( INT, D.TAXAMOUNTMST ) as TaxAmountMst, -- afa osszege az alapertelmezett penznemben
		   D.CurrencyCode as DetailCurrencyCode
		   --D.SerialNum as SerialNumber, -- sorozatszam
		   --CONVERT( INT, D.VisszaruQty ) as VisszaruQty

	FROM AxDb.dbo.CUSTINVOICEJOUR as H
	INNER JOIN  AxDb.dbo.CUSTINVOICETRANS as D ON H.INVOICEID = D.INVOICEID AND H.DATAAREAID = D.DATAAREAID
	INNER JOIN AxDb.dbo.PAYMTERM AS Pt ON H.Payment = Pt.PaymTermID AND Pt.DATAAREAID = 'mst'
	LEFT JOIN AxDb.dbo.custTrans as t on t.DATAAREAID = h.DATAAREAID and t.INVOICE = H.INVOICEID 
	LEFT JOIN AxDb.dbo.custTransOpen as o on o.DATAAREAID = t.DATAAREAID and o.RefRecId = t.RECID
	WHERE H.ORDERACCOUNT = @CustomerId
		  AND H.DATAAREAID = @DataAreaId 
		  AND year(H.INVOICEDATE) >= 2008
		  -- AND o.RefRecId <> @iDebit 
		  -- AND H.DUEDATE <= case when @bOverDue = 0 then H.DUEDATE else getdate() end
		  AND H.INVOICEDATE BETWEEN @dtDateFrom AND @dtDateTo
	ORDER BY H.INVOICEDATE desc, H.INVOICEID desc, D.LINENUM

RETURN
GO

-- exec [InternetUser].[cms_InvoiceList] 'V000787', 'hrp';

-- select * from AxDb.dbo.WebManufacturer where CreatedBy <> 'JuAt' order by id asc

-- select top 100 * from AxDb.dbo.CUSTINVOICEJOUR