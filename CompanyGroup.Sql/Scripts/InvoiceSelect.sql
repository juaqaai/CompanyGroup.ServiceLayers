/*
USE ExtractInterface
GO
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
*/
-- select top 100 * from AxDb.dbo.CUSTINVOICEJOUR
USE Web

GO
DROP PROCEDURE [InternetUser].[InvoiceDetailsSelect];
GO
CREATE PROCEDURE [InternetUser].[InvoiceDetailsSelect]( @Id INT = 0 )										
AS
SET NOCOUNT ON

	--SELECT CustomerId, 
	--	   DataAreaId,
	--	   SalesId,  -- rendelesszam, azonosito
	--	   InvoiceDate,  -- szamla datuma
	--	   DueDate,  -- esedekesseg
	--	   InvoiceAmount,  -- szamla vegosszege
    --     InvoiceCredit,  -- szamla tartozas
	--	   CurrencyCode,  
	--	   InvoiceId,  -- szla. szama
	--	   Payment,	-- fizetesi feltetelek
	--	   SalesType,  -- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet	
	--	   CusomerRef,  -- vevo hivatkozas
	--	   InvoicingName,  -- szla. nev
	--	   InvoicingAddress,  -- szla. cim
	--	   ContactPersonId,  -- kapcsolattarto
	--	   Printed,  --
	--	   ReturnItemId, --
	;

	DECLARE @InvoiceId nvarchar(30) = ISNULL((SELECT InvoiceId FROM InternetUser.Invoice  WHERE Id = @Id), '');

	WITH CTE (Id, Stock, Available)
	AS (
		SELECT DISTINCT I.Id, Stock, Available 
		FROM InternetUser.Catalogue as C
		INNER JOIN InternetUser.Invoice as I ON I.ItemId = C.ProductId AND I.DataAreaId = C.DataAreaId
		WHERE I.Id = @Id
	)

		   SELECT I.Id, InvoiceId, 
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
		   DetailCurrencyCode as CurrencyCode, 
		   ISNULL(I.[Description], '') as [Description],
		   ISNULL([FileName], '' ) as [FileName],
		   ISNULL(RecId, 0) as RecId, 
		   CASE WHEN FileName <> '' THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as PictureExists, 
		   CASE WHEN ISNULL(CTE.Stock, 0) > 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as InStock, 
		   CASE WHEN ISNULL(CTE.Available, 0) > 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as AvailableInWebShop, 
		   I.DataAreaId
	FROM InternetUser.Invoice as I 
		 -- LEFT OUTER JOIN InternetUser.Catalogue as C ON I.ItemId = C.ProductId AND I.DataAreaId = C.DataAreaId
		 LEFT OUTER JOIN CTE ON CTE.Id = I.Id
	WHERE InvoiceId = @InvoiceId
	ORDER BY LineNum;

RETURN
GO
GRANT EXECUTE ON InternetUser.InvoiceDetailsSelect TO InternetUser
GO

-- exec InternetUser.InvoiceDetailsSelect 1186
-- select * from InternetUser.Catalogue where ProductId = 'T5D-01736'

-- EXEC InternetUser.InvoiceSelect 'V001446', 1, 1; 
/*
select top 1000 * from InternetUser.Invoice where InvoiceId = 'BI004821/13'

EXEC [InternetUser].[InvoiceSelect2] @CustomerId = 'V001446',	
									@Debit = 0,				--0: mind, 1 kifizetetlen
									@OverDue = 0,				--0: mind, 1 lejart 
									@ItemId = '', 
									@ItemName = '',
									@SalesId = '',
									@SerialNumber = '',
									@InvoiceId = 'HI0040',
									@DateIntervall = 0,
									@Sequence = 0, 
									@CurrentPageIndex = 1, 
									@ItemsOnPage = 30;
*/

-- select top 10 * from InternetUser.Invoice 
-- update InternetUser.Invoice set SerialNumber = ''

GO
DROP PROCEDURE [InternetUser].[InvoiceCount];
GO
CREATE PROCEDURE [InternetUser].[InvoiceCount]( @CustomerId NVARCHAR(10) = '',	--vevokod
											    @Debit BIT = 0,				--0: mind, 1 kifizetetlen
											    @OverDue BIT = 0,				--0: mind, 1 lejart 
												@ItemId NVARCHAR(20) = '', 
												@ItemName NVARCHAR(300) = '',
												@SalesId NVARCHAR(20) = '',
												@SerialNumber NVARCHAR(40) = '',
												@InvoiceId NVARCHAR(20) = '',
												@DateIntervall INT = 0 )
											    --@dtDateFrom DATETIME = NULL, 
											    --@dtDateTo DATETIME = NULL )										
AS
SET NOCOUNT ON

	SELECT COUNT(DISTINCT InvoiceId) as [Count]
	FROM InternetUser.Invoice 
	WHERE CustomerId = @CustomerId 
		  AND Debit = CASE WHEN @Debit = 1 THEN @Debit ELSE Debit  END
		  AND 1 = CASE WHEN (@OverDue = 1 AND DueDate <= GETDATE() AND InvoiceCredit > 0) OR (@OverDue = 0) THEN 1 ELSE 0 END
		  AND ItemId LIKE CASE WHEN @ItemId <> '' THEN '%' + @ItemId + '%' ELSE ItemId END
		  AND ItemName LIKE CASE WHEN @ItemName <> '' THEN '%' + @ItemName + '%' ELSE ItemName END
		  AND SalesId LIKE CASE WHEN @SalesId <> '' THEN '%' + @SalesId + '%' ELSE SalesId END
		  AND SerialNumber LIKE CASE WHEN @SerialNumber <> '' THEN '%' + @SerialNumber + '%' ELSE [SerialNumber] END
		  AND InvoiceId LIKE CASE WHEN @InvoiceId <> '' THEN '%' + @InvoiceId + '%' ELSE InvoiceId END
		  -- AND 1 = CASE WHEN @DateIntervall = 1 AND (DATEDIFF(d, InvoiceDate, CreatedDate) > 37) THEN 0 ELSE 1 END
		  AND 1 = CASE WHEN @DateIntervall <> 0 AND (DATEDIFF(d, InvoiceDate, GetDate()) > @DateIntervall) THEN 0 ELSE 1 END
	--GROUP BY InvoiceId
RETURN
GO
GRANT EXECUTE ON InternetUser.InvoiceCount TO InternetUser
GO

/* EXEC InternetUser.InvoiceCount @CustomerId = 'V001446',	
									@Debit = 0,				--0: mind, 1 kifizetetlen
									@OverDue = 0,				--0: mind, 1 lejart 
									@ItemId = '', 
									@ItemName = 'monitor',
									@SalesId = '',
									@SerialNumber = '',
									@InvoiceId = '',
									@DateIntervall = 0
									*/

-- számla listához tartozó kép
DROP PROCEDURE [InternetUser].[InvoicePictureSelect];
GO
CREATE PROCEDURE [InternetUser].[InvoicePictureSelect]( @Id INT = 0 )										
AS
SET NOCOUNT ON
	SELECT TOP 1 Id, [FileName], CONVERT(BIT, 1) as [Primary], RecId
	FROM InternetUser.Invoice
	WHERE Id = @Id AND [FileName] <> '';
RETURN
GO
GRANT EXECUTE ON InternetUser.InvoicePictureSelect TO InternetUser
GO

-- exec [InternetUser].[InvoicePictureSelect] 30014;

DROP PROCEDURE [InternetUser].[InvoiceSelect2];
GO
CREATE PROCEDURE [InternetUser].[InvoiceSelect2]( @CustomerId NVARCHAR(10) = '',	--vevokod
											     @Debit BIT = 0,				--0: mind, 1 kifizetetlen
											     @OverDue BIT = 0,				--0: mind, 1 lejart 
												 @ItemId NVARCHAR(20) = '', 
												 @ItemName NVARCHAR(300) = '',
												 @SalesId NVARCHAR(20) = '',
												 @SerialNumber NVARCHAR(40) = '',
												 @InvoiceId NVARCHAR(20) = '',
												 @DateIntervall INT = 0,	-- 30, 60, 90, 180, 365
												 @Sequence int = 0,	
												 @CurrentPageIndex INT = 1, 
												 @ItemsOnPage INT = 30 )
											   --@dtDateFrom DATETIME = NULL, 
											   --@dtDateTo DATETIME = NULL )										
AS
SET NOCOUNT ON

	SELECT I.Id, 
		   I.InvoiceDate,  -- szamla datuma
		   I.DataAreaId as SourceCompany,  
		   DueDate,  -- esedekesseg
		   InvoiceAmount,  -- szamla vegosszege
           InvoiceCredit,  -- szamla tartozas
		   CurrencyCode,  
		   I.InvoiceId,  -- szla. szama
		   --SUM(LineAmount) as LineAmount,
		   --SUM(TaxAmount) as TaxAmount,  --
		   --SUM(LineAmountMst) as LineAmountMst,  -- osszeg az alapertelmezett penznemben
		   --SUM(TaxAmountMst) as TaxAmountMst, -- afa osszege az alapertelmezett penznemben
		   CASE WHEN DATEDIFF(d, GETDATE(), DueDate) > 0 THEN CONVERT(BIT, 0) ELSE CONVERT(BIT, 1) END as OverDue, 
		   I.LineNum, 
		   I.ItemDate, 
		   I.ItemId,  -- cikk
		   I.ItemName,  -- cikk neve
		   I.Quantity,  -- mennyiseg
		   I.SalesPrice,  -- egysegar
		   I.QuantityPhysical,  -- mennyiseg
		   I.Remain,  -- fennmarado mennyiseg
		   I.DeliveryType, -- 
		   I.LineAmount,  -- osszeg
		   I.TaxAmount,  --
		   I.LineAmountMst,  -- osszeg az alapertelmezett penznemben
		   I.TaxAmountMst, -- afa osszege az alapertelmezett penznemben
		   I.DetailCurrencyCode as CurrencyCode, 
		   ISNULL(I.[Description], '') as [Description],
		   ISNULL(I.[FileName], '' ) as [FileName],
		   ISNULL(I.RecId, 0) as RecId, 
		   CASE WHEN I.[FileName] <> '' THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as PictureExists, 
		   CASE WHEN ISNULL(C.Stock, 0) > 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as InStock, 
		   CASE WHEN ISNULL(C.Available, 0) > 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END as AvailableInWebShop, 
		   I.DataAreaId
	FROM InternetUser.Invoice as I
	LEFT OUTER JOIN InternetUser.Catalogue as C ON I.ItemId = C.ProductId -- AND I.DataAreaId = C.DataAreaId 
	WHERE CustomerId = @CustomerId 
		  AND Debit = CASE WHEN @Debit = 1 THEN @Debit ELSE Debit  END
		  AND 1 = CASE WHEN (@OverDue = 1 AND DATEDIFF(d, DueDate, GetDate()) > 0 AND InvoiceCredit > 0) OR (@OverDue = 0) THEN 1 ELSE 0 END	--  DueDate <= GETDATE()
		  AND ItemId LIKE CASE WHEN @ItemId <> '' THEN '%' + @ItemId + '%' ELSE ItemId END
		  AND ItemName LIKE CASE WHEN @ItemName <> '' THEN '%' + @ItemName + '%' ELSE ItemName END
		  AND SalesId LIKE CASE WHEN @SalesId <> '' THEN '%' + @SalesId + '%' ELSE SalesId END
		  AND SerialNumber LIKE CASE WHEN @SerialNumber <> '' THEN '%' + @SerialNumber + '%' ELSE [SerialNumber] END
		  AND InvoiceId LIKE CASE WHEN @InvoiceId <> '' THEN '%' + @InvoiceId + '%' ELSE InvoiceId END
		  AND 1 = CASE WHEN @DateIntervall <> 0 AND (DATEDIFF(d, InvoiceDate, GetDate()) > @DateIntervall) THEN 0 ELSE 1 END
	--GROUP BY InvoiceDate, DataAreaId, DueDate, InvoiceAmount, InvoiceCredit, CurrencyCode, InvoiceId

	ORDER BY InvoiceDate DESC, DueDate DESC, InvoiceId, LineNum
	--CASE WHEN @Sequence =  0 THEN
	--	InvoiceDate END DESC,
	--CASE WHEN @Sequence =  1 THEN
	--	InvoiceDate END ASC
	OFFSET (@CurrentPageIndex - 1) * @ItemsOnPage ROWS
	FETCH NEXT @ItemsOnPage ROWS ONLY;

RETURN
GO
GRANT EXECUTE ON InternetUser.InvoiceSelect2 TO InternetUser
GO

/* BI001213/13,  BC001256/10 

select DATEDIFF(d, GetDate() - 1, GetDate())

select top 100 * from InternetUser.Invoice where invoiceId = 'HI018878/13' 
*/

-- EXEC InternetUser.InvoiceSelect2 'V016645', 1, 1; 
/*
V000135
EXEC [InternetUser].[InvoiceSelect2] @CustomerId = 'V001446',	
									@Debit = 0,				--0: mind, 1 kifizetetlen
									@OverDue = 0,				--0: mind, 1 lejart 
									@ItemId = '', 
									@ItemName = '',	-- monitor
									@SalesId = '',
									@SerialNumber = '',
									@InvoiceId = '',	--HI057773
									@DateIntervall = 2,
									@Sequence = 0, 
									@CurrentPageIndex = 1, 
									@ItemsOnPage = 30;
*/
DROP PROCEDURE [InternetUser].[InvoiceSelect3];
GO
CREATE PROCEDURE [InternetUser].[InvoiceSelect3](@CustomerId NVARCHAR(10) = '',	--vevokod
											     @Debit BIT = 0,				--0: mind, 1 kifizetetlen
											     @OverDue BIT = 0,				--0: mind, 1 lejart 
												 @ItemId NVARCHAR(20) = '', 
												 @ItemName NVARCHAR(300) = '',
												 @SalesId NVARCHAR(20) = '',
												 @SerialNumber NVARCHAR(40) = '',
												 @InvoiceId NVARCHAR(20) = '',
												 @DateIntervall INT = 0 )									
AS
SET NOCOUNT ON

	SELECT InvoiceId  -- szla. szama
	FROM InternetUser.Invoice 
	WHERE CustomerId = @CustomerId 
		  AND Debit = CASE WHEN @Debit = 1 THEN @Debit ELSE Debit  END
		  AND DueDate <= CASE WHEN @OverDue = 1 then GETDATE() ELSE DueDate END
		  AND ItemId LIKE CASE WHEN @ItemId <> '' THEN '%' + @ItemId + '%' ELSE ItemId END
		  AND ItemName LIKE CASE WHEN @ItemName <> '' THEN '%' + @ItemName + '%' ELSE ItemName END
		  AND SalesId LIKE CASE WHEN @SalesId <> '' THEN '%' + @SalesId + '%' ELSE SalesId END
		  AND SerialNumber LIKE CASE WHEN @SerialNumber <> '' THEN '%' + @SerialNumber + '%' ELSE [SerialNumber] END
		  AND InvoiceId LIKE CASE WHEN @InvoiceId <> '' THEN '%' + @InvoiceId + '%' ELSE InvoiceId END
		  AND 1 = CASE WHEN @DateIntervall = 1 AND (DATEDIFF(d, InvoiceDate, CreatedDate) > 37) THEN 0 ELSE 1 END
	GROUP BY InvoiceId
RETURN
GO
GRANT EXECUTE ON InternetUser.InvoiceSelect3 TO InternetUser
GO

/*
EXEC [InternetUser].[InvoiceSelect3] @CustomerId = 'V001446',	
									@Debit = 0,				--0: mind, 1 kifizetetlen
									@OverDue = 0,				--0: mind, 1 lejart 
									@ItemId = '', 
									@ItemName = '',
									@SalesId = '',
									@SerialNumber = '',
									@InvoiceId = 'HI057773',
									@DateIntervall = 0;
*/
DROP PROCEDURE [InternetUser].[InvoiceSumValues];
GO
CREATE PROCEDURE [InternetUser].[InvoiceSumValues](@CustomerId NVARCHAR(10) = '')
AS
SET NOCOUNT ON
	;
	WITH CTE (AmountCredit, AmountOverdue, CurrencyCode)
	AS (
	SELECT --SUM(InvoiceAmount) as SumInvoiceAmount,  -- szamla vegosszege
		   MAX(InvoiceCredit) as AmountCredit,  -- szamla tartozas
		   0 as AmountOverdue, 
		   --'Credit' as InvoiceType,
		   CurrencyCode  
	FROM InternetUser.Invoice
	WHERE CustomerId = @CustomerId 
	GROUP BY InvoiceId, CurrencyCode
	UNION ALL
	SELECT --SUM(InvoiceAmount) as SumInvoiceAmount,  -- szamla vegosszege
		   0 as AmountCredit, 
		   MAX(InvoiceCredit) as AmountOverdue,  -- szamla tartozas
		   --'Overdue' as InvoiceType,
		   CurrencyCode  
	FROM InternetUser.Invoice
	WHERE CustomerId = @CustomerId  AND 
		  -- DueDate < GETDATE() 
		  DATEDIFF(d, DueDate, GetDate()) > 0
	GROUP BY InvoiceId, CurrencyCode )
	SELECT SUM(AmountCredit) as AmountCredit, SUM(AmountOverdue) as AmountOverdue, CurrencyCode FROM CTE GROUP BY CurrencyCode;

RETURN
GO
GRANT EXECUTE ON InternetUser.[InvoiceSumValues] TO InternetUser
GO
-- EXEC [InternetUser].[InvoiceSumValues] 'V000276', --V003829 326412.00 'V006074'; 'V001446'	AmountCredit	89612.00
-- select * from InternetUser.Invoice where CustomerId = 'V003829'
select DATEDIFF(d, GetDate(), GetDate() + 1)