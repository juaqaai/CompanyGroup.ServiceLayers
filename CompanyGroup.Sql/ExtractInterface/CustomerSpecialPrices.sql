/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban lekérdezi a vevõhöz tartozó speciális árakat
	running script : exec [InternetUser].[CustomerSpecialPriceSelect] 'V017103';
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
-- cikkek lista
DROP PROCEDURE [InternetUser].[CustomerSpecialPriceSelect];
GO
CREATE PROCEDURE [InternetUser].[CustomerSpecialPriceSelect] @CustomerId NVARCHAR(10)
AS
SET NOCOUNT ON

	DECLARE @MaxFromDate DateTime2;

	SELECT @MaxFromDate = MAX(FromDate) FROM [AXDB].[dbo].[EXCHRATES];

	WITH ExchangeRate (Currency, Rate) 
	AS (
		SELECT [CURRENCYCODE] as Currency
			   ,[EXCHRATEMNB] as Rate
		FROM axdb.[dbo].[EXCHRATES]
		WHERE FromDate = @MaxFromDate
		GROUP BY [CURRENCYCODE], [EXCHRATEMNB]
	)
	-- select * from ExchangeRate

	SELECT 0 as Id 
		   ,0 as VisitorKey 
		   ,[ItemRelation] as ProductId
		   --,[AccountRelation]
		   ,CASE WHEN P.[Currency] = 'HUF' THEN CONVERT(INT, P.Amount) ELSE InternetUser.CalculateHUF(E.Rate, P.Amount) END as Price
		   ,[DataAreaId] 
		   --,InventDimId
	 FROM Axdb.dbo.PRICEDISCTABLE as P
		  LEFT OUTER JOIN ExchangeRate as E ON P.[Currency] = E.Currency
	 WHERE [AccountRelation] NOT IN ('1', '2', '3', '4', '5', 'M') 
		   AND SUBSTRING([AccountRelation], 1, 1) = 'V'
		   AND DataAreaId IN ('hrp', 'bsc')
		   AND [AccountRelation] = @CustomerId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[CustomerSpecialPriceSelect] TO InternetUser
GO
GRANT EXECUTE ON [InternetUser].[CustomerSpecialPriceSelect] TO [HRP_HEADOFFICE\AXPROXY]
GO

-- EXEC [InternetUser].[CustomerSpecialPriceSelect] 'V017103';
-- select * from Axdb.dbo.CustTable Where AccountNum = 'V017103'
GO
DROP FUNCTION InternetUser.CalculateHUF
GO
CREATE FUNCTION InternetUser.CalculateHUF( @Rate numeric(28, 12), @Amount numeric(28, 12) )
RETURNS INT
AS
BEGIN
	DECLARE @Result INT;

	IF (@Rate IS NULL OR @Amount IS NULL)
		SET @Result = 0;	

	SET @Result = CONVERT(INT, (@Rate / 100) * @Amount);

	RETURN @Result;
END
GO
GRANT EXECUTE ON InternetUser.CalculateHUF TO InternetUser
GO
GRANT EXECUTE ON [InternetUser].CalculateHUF TO [HRP_HEADOFFICE\AXPROXY]
GO

-- select InternetUser.CalculateHUF(400, 100);

/*
	SELECT COUNT([ItemRelation])
		   ,[AccountRelation]
		   --, Amount
		   --,[DataAreaId] 
	 FROM Axdb.dbo.PRICEDISCTABLE as P
	 INNER JOIN Axdb.dbo.CustTable as C ON P.AccountRelation = C.AccountNum
	 INNER JOIN Axdb.dbo.ContactPerson as Cp ON C.AccountNum = Cp.CustAccount AND Cp.DataAreaID = 'HUN'
	 WHERE [AccountRelation] NOT IN ('1', '2', '3', '4', '5', 'M') 
		   AND SUBSTRING([AccountRelation], 1, 1) = 'V'
		   AND P.DataAreaId IN ('hrp', 'bsc')
		   AND AccountRelation <> 'V017103' AND AccountRelation <> 'V'
		   AND C.StatisticsGroup <> 'Arhív' AND C.StatisticsGroup <> 'Archív' 
	GROUP BY AccountRelation
*/