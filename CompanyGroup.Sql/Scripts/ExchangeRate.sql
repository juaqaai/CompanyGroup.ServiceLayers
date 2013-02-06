/*
2012.07.30.
*/
USE ExtractInterface
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.ExchangeRate
GO
CREATE PROCEDURE InternetUser.ExchangeRate
AS
SET NOCOUNT ON

DECLARE @MaxFromDate DateTime2;

	SELECT @MaxFromDate = MAX(FromDate) FROM [AXDB].[dbo].[EXCHRATES];

	SELECT Convert( DateTime2, [FROMDATE]) as FromDate
      ,[CURRENCYCODE] as CurrencyCode
      ,[EXCHRATEMNB] as Rate
      ,[DATAAREAID] as DataAreaId
	FROM axdb.[dbo].[EXCHRATES]
	WHERE FromDate = @MaxFromDate;

  RETURN
GO
GRANT EXECUTE ON InternetUser.ExchangeRate TO InternetUser
GO

/*
 exec InternetUser.ExchangeRate;
*/