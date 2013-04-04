
-- CT engedélyezése adatbázisra
ALTER DATABASE Axdb_20130131

SET CHANGE_TRACKING = ON

(CHANGE_RETENTION = 30 HOURS, AUTO_CLEANUP = ON)

-- CT engedélyezése táblára
ALTER TABLE dbo.INVENTSUM

ENABLE CHANGE_TRACKING

WITH (TRACK_COLUMNS_UPDATED = ON)

ALTER TABLE dbo.PRICEDISCTABLE

ENABLE CHANGE_TRACKING

WITH (TRACK_COLUMNS_UPDATED = ON)

-- 
select * from sys.change_tracking_databases 

select * from sys.change_tracking_tables 

select * from sys.internal_tables 

declare @anchor bigint = CHANGE_TRACKING_CURRENT_VERSION();
select @anchor;

select
--ct.*, '|', p.*,
ct.SYS_CHANGE_VERSION, 
ct.SYS_CHANGE_OPERATION,
[ItemRelation], 
[Amount],
[Currency],
p.[DataAreaId],
CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.PRICEDISCTABLE'), 'Amount', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AmountChanged, 
CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('Axdb_20130131.dbo.PRICEDISCTABLE'), 'Currency', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as CurrencyChanged, 
CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('Axdb_20130131.dbo.PRICEDISCTABLE'), 'ItemRelation', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as ItemRelationChanged, 
CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('Axdb_20130131.dbo.PRICEDISCTABLE'), 'AccountRelation', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AccountRelationChanged 
from Axdb_20130131.dbo.PRICEDISCTABLE as P 
right outer join
changetable(changes Axdb_20130131.dbo.PRICEDISCTABLE, 0) as ct
on P.RecId = ct.RecId and P.DataAreaId = ct.DataAreaId

select * from changetable(changes Axdb_20130131.dbo.PRICEDISCTABLE, 0) as ct

select CHANGE_TRACKING_IS_COLUMN_IN_MASK
(columnproperty(object_id('Axdb_20130131.dbo.PRICEDISCTABLE'), 'Amount', 'ColumnId')
,0x00000000090000001F00000018000000190000001A000000);

SELECT [ITEMRELATION]
      ,[ACCOUNTRELATION]
      ,[QUANTITYAMOUNT]
      ,[AMOUNT]
      ,[CURRENCY]
      ,[DATAAREAID]
      ,[RECID]
  FROM [Axdb_20130131].[dbo].[PRICEDISCTABLE]



select stock.ItemId,
	   stock.AvailPhysical,
	   stock.DataAreaId, 
	   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'ItemId', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as ItemIdChanged, 
	   CHANGE_TRACKING_IS_COLUMN_IN_MASK (columnproperty(object_id('dbo.InventSum'), 'AvailPhysical', 'ColumnId') ,ct.SYS_CHANGE_COLUMNS) as AvailPhysicalChanged
from [Axdb_20130131].[dbo].[INVENTSUM] as Stock
right outer join
changetable(changes Axdb_20130131.dbo.INVENTSUM, 0) as ct
on Stock.ItemId = ct.ItemId and Stock.InventDimId = ct.InventDimId and Stock.DataAreaId = ct.DataAreaId

SELECT [ITEMID]
      ,[POSTEDQTY]
      ,[POSTEDVALUE]
      ,[DEDUCTED]
      ,[RECEIVED]
      ,[RESERVPHYSICAL]
      ,[RESERVORDERED]
      ,[ONORDER]
      ,[ORDERED]
      ,[QUOTATIONISSUE]
      ,[QUOTATIONRECEIPT]
      ,[INVENTDIMID]
      ,[CLOSED]
      ,[REGISTERED]
      ,[PICKED]
      ,[AVAILORDERED]
      ,[AVAILPHYSICAL]
      ,[PHYSICALVALUE]
      ,[ARRIVED]
      ,[PHYSICALINVENT]
      ,[CLOSEDQTY]
      ,[LASTUPDDATEPHYSICAL]
      ,[LASTUPDDATEEXPECTED]
      ,[KUSZOBARPOSTEDVALUE]
      ,[KUSZOBARPOSTEDQTY]
      ,[TRANSFERORDER]
      ,[TRANSFERRESERVORDERED]
      ,[DATAAREAID]
      ,[RECVERSION]
      ,[RECID]
      ,[SORTORDER]
      ,[KUSZOBEXCHRATE]
      ,[ONORDERSALES]
      ,[TRANSFERPICKED]
  FROM [Axdb_20130131].[dbo].[INVENTSUM]