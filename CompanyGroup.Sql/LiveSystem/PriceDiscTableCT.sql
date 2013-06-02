/*
Lekérdezi az árváltozásokat a legutolsó szinkronizáció óta
*/

USE [Axdb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.PriceDiscTableCT
GO
CREATE PROCEDURE InternetUser.PriceDiscTableCT( @LastVersion BIGINT = 0 )
AS
SET NOCOUNT ON

	IF (@LastVersion = 0)
	BEGIN
		SET @LastVersion = ISNULL((SELECT MAX(LastVersion) FROM ExtractInterface.dbo.SyncMetadata WHERE TableName = 'PriceDiscTable' AND [Status] = 1), 0);
	END

	SELECT ct.SYS_CHANGE_VERSION as [Version], 
	ct.SYS_CHANGE_OPERATION as Operation,
	[ItemRelation], 
	[AccountRelation], 
	[Amount],
	[Currency],
	p.[DataAreaId]
--	CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.PRICEDISCTABLE'), 'Amount', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AmountChanged, 
--	CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('Axdb.dbo.PRICEDISCTABLE'), 'Currency', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as CurrencyChanged, 
--	CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('Axdb.dbo.PRICEDISCTABLE'), 'ItemRelation', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as ItemRelationChanged, 
--	CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('Axdb.dbo.PRICEDISCTABLE'), 'AccountRelation', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AccountRelationChanged 
	FROM Axdb.dbo.PRICEDISCTABLE as P 
	RIGHT OUTER JOIN changetable(changes Axdb.dbo.PRICEDISCTABLE, @LastVersion) as ct
	on P.RecId = ct.RecId and P.DataAreaId = ct.DataAreaId
	WHERE ct.SYS_CHANGE_OPERATION <> 'D'
	ORDER BY ct.SYS_CHANGE_VERSION;

RETURN

GO
GRANT EXECUTE ON InternetUser.PriceDiscTableCT TO [HRP_HEADOFFICE\AXPROXY]
GO
GRANT EXECUTE ON InternetUser.PriceDiscTableCT TO InternetUser
GO
-- exec InternetUser.PriceDiscTableCT 1310

/*
select top 1 * from Axdb.dbo.PRICEDISCTABLE
select * from changetable(changes Axdb.dbo.PRICEDISCTABLE, 0) as ct
*/