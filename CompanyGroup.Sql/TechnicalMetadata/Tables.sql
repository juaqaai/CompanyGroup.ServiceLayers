/*
	description	   : Staging job log, package log table
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.25.
	modified date  :
	modify reason  :
	 	
*/
USE TechnicalMetadata
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.Systems') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE dbo.Systems;

/* Create table dbo.Systems */
CREATE TABLE dbo.Systems (
   [SystemKey]			INT IDENTITY NOT NULL
,  [SystemName]			NVARCHAR(50) NULL
, CONSTRAINT [PK_dbo.Systems] PRIMARY KEY CLUSTERED 
( [SystemKey] )
) ON [PRIMARY];

GO

/* Drop table dbo.JobLog */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.JobLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE dbo.JobLog;

/* Create table dbo.JobLog */
CREATE TABLE dbo.JobLog (
   [JobLogKey]			INT IDENTITY NOT NULL
,  [JobName]			NVARCHAR(50) NULL
,  [StartTime]			DATETIME2 NULL
,  [EndTime]			DATETIME2 NULL
,  [ServerExecutionID]  BIGINT NULL
,  [ErrorCode]			INT NULL
,  [ErrorDesc]			NVARCHAR(4000) NULL
,  [ErrorSource]		NVARCHAR(100) NULL
,  SystemKey			INT NOT NULL DEFAULT 0
, CONSTRAINT [PK_dbo.JobLog] PRIMARY KEY CLUSTERED 
( [JobLogKey] )
) ON [PRIMARY];

GO

/* Drop table dbo.PackageLog */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.PackageLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE dbo.PackageLog;

/* Create table dbo.PackageLog */
CREATE TABLE dbo.PackageLog (
   [PackageLogKey]		INT IDENTITY NOT NULL			-- A betöltési napló egyedi sorazonosítója (Primary Key)
,  [PackageName]		VARCHAR(255) NULL				-- Integration Services (SSIS) csomag neve
,  [JobLogKey]			INT NULL						-- A Job futásának egyedi azonosítója
,  [StartTime]			DATETIME2 NULL					-- Integration Services (SSIS) csomag lefutásának kezdete
,  [EndTime]			DATETIME2 NULL					-- Integration Services (SSIS) csomag lefutásának vége
,  [ExecutedBy]			VARCHAR(255) NULL				-- Integration Services (SSIS) csomagot futtató Windows felhasználó neve
,  [InteractiveMode]	VARCHAR(255) NULL				-- Integration Services (SSIS) csomagot  futtató alkalmazás INTERACTIVE_MODE
,  [ErrorCode]			INT NULL						-- Az Integration Services (SSIS) csomag futása során keletkezett (Elsõdleges) hiba kódja
,  [ErrorDesc]			VARCHAR(4000) NULL				-- Az Integration Services (SSIS) csomag futása során keletkezett (Elsõdleges) hiba megnevezése
,  [ErrorSource]		VARCHAR(100) NULL				-- Annak az Integration Service (SSIS) task-nak a neve, amely a hibát okozta
,  [RowCount]			INT NULL						-- A package által betöltött sorok száma (UPDATE/INSERT)
,  [ServerExecutionID]	BIGINT NULL						-- Kapcsolat az SSIS beépített naplóval
,  [RunDecision]		VARCHAR(50) NULL				-- Ez az oszlop tartalmazza, hogy az SSIS csomag hogyan döntött a futása kezdetén. Két értéke lehet az oszlopnak: 1 = új vagy ismételt betöltés 2 = A csomag tartalma nem hajtódott végre, mert erre a dátumra egyszer már sikeresen futott az SSIS csomag
,  [Status]				NVARCHAR(20) NULL				-- Státusz, ami a package futásához kapcsolódik.
, CONSTRAINT [PK_dbo.PackageLog] PRIMARY KEY CLUSTERED 
( [PackageLogKey] )
) ON [PRIMARY];
GO

/* Átkerült az ExtractInterface adatbázisba */
-- CHANGE_TRACKING_CURRENT_VERSION(); értéke kerül letárolásra az Axdb adatbázisból- utolsó, véglegesített tranzakció verziószámát adja vissza
--IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.SyncMetadata') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--	DROP TABLE dbo.SyncMetadata;
--GO
--CREATE TABLE dbo.SyncMetadata
--(
--	Id INT IDENTITY PRIMARY KEY,	
--	LastVersion BIGINT NOT NULL DEFAULT 0, 
--	TableName NVARCHAR(32) NOT NULL DEFAULT '', -- vagy 'InventSum', vagy 'PRICEDISCTABLE'
--	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE(), -- datum, ido
--	[Status] INT NOT NULL DEFAULT 1 -- státusz (Passive = 0, New = 1, Posted = 2)

--)

-- select object_id('Axdb_20130131.dbo.PRICEDISCTABLE')
-- select object_id('Axdb_20130131.dbo.InventSum')
