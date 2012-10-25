/*
2012.07.30.
*/
USE WebDb_Test
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.cms_ExchangeRate
GO
CREATE PROCEDURE InternetUser.cms_ExchangeRate
AS
SET NOCOUNT ON

DECLARE @MaxFromDate DateTime2;

	SELECT @MaxFromDate = MAX(FromDate) FROM [AXDB].[dbo].[EXCHRATES];

	SELECT Convert( DateTime2, [FROMDATE]) as FromDate
      ,[CURRENCYCODE] as CurrencyCode
      ,[EXCHRATEMNB] as Rate
      ,[DATAAREAID] as DataAreaId
	FROM axdb_20120614.[dbo].[EXCHRATES]
	WHERE FromDate = @MaxFromDate;

  RETURN
GO
GRANT EXECUTE ON InternetUser.cms_ExchangeRate TO InternetUser
GO

/*
 exec InternetUser.cms_ExchangeRate;
*/