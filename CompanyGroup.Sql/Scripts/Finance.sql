USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
	finanszirozasi ajanlat szamitas	 
*/
DROP PROCEDURE InternetUser.PaymentPeriod
GO
CREATE PROCEDURE InternetUser.PaymentPeriod
AS
SET NOCOUNT ON

	SELECT PaymentPeriodId, NumOfMonth 
	FROM Axdb.dbo.FinancePaymentPeriod
	ORDER BY PaymentPeriodId; 

RETURN
GO
GRANT EXECUTE ON InternetUser.PaymentPeriod TO InternetUser
GO

-- exec InternetUser.PaymentPeriod;

/*
legkisebb és legnagyobb finanszírozható értékhatár lekérdezése
*/
DROP PROCEDURE InternetUser.MinMaxFinanceLeasingValues
GO
CREATE PROCEDURE InternetUser.MinMaxFinanceLeasingValues
AS
SET NOCOUNT ON
	SELECT MIN(FromValue) as MinValue, MAX(ToValue) as MaxValue FROM Axdb.dbo.FinanceLeasingIntervall;
RETURN
GO
GRANT EXECUTE ON InternetUser.MinMaxFinanceLeasingValues TO InternetUser
GO
-- exec InternetUser.MinMaxFinanceLeasingValues;

/*
	finanszirozasi ajanlat szamitas	 

	Object:  StoredProcedure InternetUser.GetLeasingByFinancedAmount    Script Date: 03/02/2010 08:34:33

	select * from AxDb.dbo.FinanceLeasingIntervall
	select * from AxDb.dbo.FinanceParameter
	select * from AxDb.dbo.FinancePaymentPeriod
 
*/
DROP PROCEDURE InternetUser.LeasingByFinancedAmount
GO
CREATE PROCEDURE InternetUser.LeasingByFinancedAmount( @FinancedAmount int = 0 )
AS
SET NOCOUNT ON

	DECLARE @IntervallD INT;

	/*
	Finanszírozási összegnek megfelelõen kiválasztja azt az egy intervallumot, amivel a számítás történik
	*/
	SELECT @IntervallD = LeasingIntervalId FROM Axdb.dbo.FinanceLeasingIntervall where @FinancedAmount between FromValue and ToValue;

	SET @IntervallD = ISNULL(@IntervallD, 0);

	SELECT FinanceParameterId as Id, fli.FromValue as IntervalFrom, fli.ToValue as IntervalTo,
		   fpp.NumOfMonth, PercentValue, InterestRate, PresentValue
	FROM Axdb.dbo.FinanceParameter as fp 
	INNER JOIN Axdb.dbo.FinancePaymentPeriod as fpp on fp.PAYMENTPERIODID = fpp.PAYMENTPERIODID
	INNER JOIN Axdb.dbo.FINANCELEASINGINTERVALL as fli on fli.LEASINGINTERVALID = fp.LEASINGINTERVALID
	WHERE fp.LeasingIntervalId = @IntervallD AND @IntervallD > 0;

/*	select FinanceParameterId, fp.LeasingIntervalId, fp.PaymentPeriodId, InterestRate, PresentValue, NumOfMonth, PercentValue 
	from AxDb.dbo.FinanceParameter as fp
	
	inner join AxDb.dbo.FinanceLeasingIntervall as fli on fp.LeasingIntervalId = fli.LeasingIntervalId
	where @iFinancedAmount between fli.FromValue and fli.ToValue
	order by fp.LeasingIntervalId, fp.PaymentPeriodId; */

RETURN
GO
GRANT EXECUTE ON InternetUser.LeasingByFinancedAmount TO InternetUser
GO

/*
exec InternetUser.LeasingByFinancedAmount 5000001;
exec InternetUser.LeasingByFinancedAmount 280000;

select * from AxDb.dbo.FinanceLeasingIntervall

select * from AxDb.dbo.FinanceParameter

select * from AxDb.dbo.FinancePaymentPeriod
*/

DROP PROCEDURE InternetUser.ValidateFinancedAmount
GO
CREATE PROCEDURE InternetUser.ValidateFinancedAmount( @FinancedAmount int = 0, @Ret int = 0 OUT )
AS
SET NOCOUNT ON

	select @Ret = COUNT(*) -- NumOfMonth, PercentValue 
	from Axdb.dbo.FinanceParameter as fp
	inner join Axdb.dbo.FinancePaymentPeriod as fpp on fp.PaymentPeriodId = fpp.PaymentPeriodId
	inner join Axdb.dbo.FinanceLeasingIntervall as fli on fp.LeasingIntervalId = fli.LeasingIntervalId
	where @FinancedAmount between fli.FromValue and fli.ToValue; 

RETURN
GO
GRANT EXECUTE ON InternetUser.ValidateFinancedAmount TO InternetUser
GO
/*
DECLARE @Ret INT;
exec InternetUser.ValidateFinancedAmount 15000001, @Ret OUT
print Cast( @Ret as varchar(8) );
*/

/*
	ajanlat fejlec log hozzaadas	
	select * from InternetUser.OfferingHeader;

ID LogID PersonName Address Phone StatNumber NumOfMonth CreatedDate Status

*/

/*
DROP PROCEDURE InternetUser.AddOfferingHeader
GO
CREATE PROCEDURE InternetUser.AddOfferingHeader( @LogID INT = 0,					-- financeLog Id
												 @PersonName NVARCHAR(64) = '',		-- finanszírozási ajánlatban szereplõ végfelhasználó neve
												 @Address NVARCHAR(64) = '',											  
												 @Phone NVARCHAR(64) = '',			-- végfelhasználó telefonja
												 @StatNumber NVARCHAR(64) = '',		
												 @NumOfMonth INT = 0,				-- kiválasztott finanszírozási hónap intervallum
												 @ID INT = 0 OUT )
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.OfferingHeader ( LogID, PersonName, [Address], Phone, StatNumber, NumOfMonth, CreatedDate, [Status] ) VALUES 
											( @LogID, @PersonName, @Address, @Phone, @StatNumber, @NumOfMonth, GetDate(), 1 );
	SET @ID = SCOPE_IDENTITY();

	RETURN
GO
GRANT EXECUTE ON InternetUser.AddOfferingHeader to InternetUser
GO
*/
/*
select * from InternetUser.OfferingLine
ID LogID ProdDevice ProdManufacturer ProductName Qty Price CreatedDate Status
*/
/*
DROP PROCEDURE InternetUser.AddOfferingLine
GO
CREATE PROCEDURE InternetUser.AddOfferingLine( @LogID INT = 0,						-- financeLog Id
											   @ProdDevice NVARCHAR(64) = '',		-- eszköz cikkleiras
											   @ProdManufacturer NVARCHAR(64) = '',	-- eszköz gyarto leiras
											   @ProductName NVARCHAR(64),			-- eszköz tipusaleiras
											   @Qty INT,							-- eszköz darabszáma
											   @Price INT = 0,						-- finanszírozott összeg
											   @ID INT = 0 OUT )
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.OfferingLine ( LogID, ProdDevice, ProdManufacturer, ProductName, Qty, Price, CreatedDate, [Status] ) VALUES 
										  ( @LogID, @ProdDevice, @ProdManufacturer, @ProductName, @Qty, CONVERT( DECIMAL(12, 4), @Price ), GetDate(), 1 );
	SET @ID = SCOPE_IDENTITY();

	RETURN
GO
GRANT EXECUTE ON InternetUser.AddOfferingLine to InternetUser
GO
*/
/*
select * from InternetUser.FinanceLog
*/

					  