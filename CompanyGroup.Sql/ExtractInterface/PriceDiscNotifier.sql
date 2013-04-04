USE Axdb_20130131
GO
SET ANSI_NULLS ON
GO
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET NUMERIC_ROUNDABORT OFF
SET ARITHABORT ON
GO
-- cikkek lista
DROP PROCEDURE dbo.PriceDiscNotifier;
GO

CREATE PROCEDURE dbo.PriceDiscNotifier(@RecId bigint = 5641532500) 
AS
SET NOCOUNT ON

	/*SELECT Price.ITEMRELATION
      ,Price.ACCOUNTRELATION
      ,Price.AMOUNT
      ,Price.CURRENCY
      ,Price.DATAAREAID
	FROM dbo.InventTable as Invent 
	INNER JOIN dbo.PRICEDISCTABLE as Price ON Invent.ItemId = Price.ITEMRELATION AND 
											Invent.DataAreaId = Price.DataAreaId AND 
											Price.DataAreaId IN ('hrp', 'bsc') AND 
											ACCOUNTRELATION IN ('1', '2', '3', '4', '5') AND 
											Invent.WEBARUHAZ = 1 AND 
											Invent.ITEMSTATE IN ( 0, 1 )
	WHERE Currency = 'HUF' AND 
		  Invent.AMOUNT1 > 0 AND
		  Invent.AMOUNT2 > 0 AND
		  Invent.AMOUNT3 > 0 AND
		  Invent.AMOUNT4 > 0 AND
		  Invent.AMOUNT5 > 0
		  */
		  SELECT ItemId 
		  FROM dbo.InventTable 
		  WHERE DataAreaId IN ('hrp', 'bsc') AND WEBARUHAZ = 1 AND ITEMSTATE IN ( 0, 1 )	AND 
		  AMOUNT1 > 0 AND
		  AMOUNT2 > 0 AND
		  AMOUNT3 > 0 AND
		  AMOUNT4 > 0 AND
		  AMOUNT5 > 0

RETURN
GO
--select max(RecId) FROM [dbo].[PRICEDISCTABLE]
-- exec dbo.PriceDiscNotifier

-- update [dbo].[PRICEDISCTABLE] set Amount = 15000 where RecId = 5641526406