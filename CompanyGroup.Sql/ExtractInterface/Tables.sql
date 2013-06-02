/*
	description	   : Staging job log, package log table
	running script : select * from ExtractInterface.dbo.SyncMetadata
					 truncate table ExtractInterface.dbo.SyncMetadata;
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.25.
	modified date  :
	modify reason  :
	 	
*/
USE ExtractInterface
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.SyncMetadata') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE dbo.SyncMetadata;
GO
CREATE TABLE dbo.SyncMetadata
(
	Id INT IDENTITY PRIMARY KEY,	
	LastVersion BIGINT NOT NULL DEFAULT 0, 
	TableName NVARCHAR(32) NOT NULL DEFAULT '', -- vagy 'InventSum', vagy 'PRICEDISCTABLE'
	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE(), -- datum, ido
	[Status] INT NOT NULL DEFAULT 1 -- státusz (Passive = 0, New = 1, Posted = 2)

)

-- select * from dbo.Stage_InvoiceOpenTransaction
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.Stage_InvoiceOpenTransaction') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE dbo.Stage_InvoiceOpenTransaction;
GO
CREATE TABLE dbo.Stage_InvoiceOpenTransaction
(
	CustomerId			NVARCHAR(20) NOT NULL DEFAULT '',
	DataAreaId			NVARCHAR(3) NOT NULL DEFAULT '',	
	SalesId				NVARCHAR(20) NOT NULL DEFAULT '',	-- BVR, VR
	InvoiceDate			DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0), 	
	DueDate				DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0), 
	InvoiceAmount		decimal(20,2) NOT NULL DEFAULT 0, 
	InvoiceCredit		decimal(20,2) NOT NULL DEFAULT 0,
	CurrencyCode		NVARCHAR(20) NOT NULL DEFAULT '',
	InvoiceId			NVARCHAR(20) NOT NULL DEFAULT '', 
	Payment				NVARCHAR(60) NOT NULL DEFAULT '',	-- Átutalás 8 nap
	SalesType			INT NOT NULL DEFAULT 0,				-- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet
	CusomerRef			NVARCHAR(300) NOT NULL DEFAULT '',
	InvoicingName		NVARCHAR(80) NOT NULL DEFAULT '',
	InvoicingAddress	NVARCHAR(250) NOT NULL DEFAULT '',
	ContactPersonId		NVARCHAR(20) NOT NULL DEFAULT '',
	Printed				INT NOT NULL DEFAULT 0,	
	ReturnItemId		NVARCHAR(20) NOT NULL DEFAULT '',
	ItemDate			DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0),
	LineNum				INT NOT NULL DEFAULT 0,
	ItemId				NVARCHAR(20) NOT NULL DEFAULT '',
	ItemName			NVARCHAR(300) NOT NULL DEFAULT '',
	SerialNumber		NVARCHAR(40) NOT NULL DEFAULT '',
	Quantity			INT NOT NULL DEFAULT 0,
	SalesPrice			decimal(20,2) NOT NULL DEFAULT 0,	-- egysegar
	LineAmount			decimal(20,2) NOT NULL DEFAULT 0,	-- osszeg
	QuantityPhysical	INT NOT NULL DEFAULT 0,				-- mennyiseg
	Remain				INT NOT NULL DEFAULT 0,				-- fennmarado mennyiseg
	DeliveryType		INT NOT NULL DEFAULT 0,	
	TaxAmount			decimal(20,2) NOT NULL DEFAULT 0,
	LineAmountMst		decimal(20,2) NOT NULL DEFAULT 0,	-- osszeg az alapertelmezett penznemben
	TaxAmountMst		decimal(20,2) NOT NULL DEFAULT 0,	-- afa osszege az alapertelmezett penznemben
	DetailCurrencyCode  NVARCHAR(20) NOT NULL DEFAULT '',
	Debit				BIT NOT NULL DEFAULT 0, 
	[Description]		NVARCHAR(4000) NOT NULL DEFAULT '',
	[FileName]			NVARCHAR(300) NOT NULL DEFAULT '',
	RecId				BIGINT NOT NULL DEFAULT 0, 
	CreatedDate			DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0),
    [ExtractDate]		DATETIME2 NOT NULL DEFAULT GetDate(), 		
    [PackageLogKey]		INT NOT NULL DEFAULT 0
)
GO