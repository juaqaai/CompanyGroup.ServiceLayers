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
   [PackageLogKey]		INT IDENTITY NOT NULL			-- A bet�lt�si napl� egyedi sorazonos�t�ja (Primary Key)
,  [PackageName]		VARCHAR(255) NULL				-- Integration Services (SSIS) csomag neve
,  [JobLogKey]			INT NULL						-- A Job fut�s�nak egyedi azonos�t�ja
,  [StartTime]			DATETIME2 NULL					-- Integration Services (SSIS) csomag lefut�s�nak kezdete
,  [EndTime]			DATETIME2 NULL					-- Integration Services (SSIS) csomag lefut�s�nak v�ge
,  [ExecutedBy]			VARCHAR(255) NULL				-- Integration Services (SSIS) csomagot futtat� Windows felhaszn�l� neve
,  [InteractiveMode]	VARCHAR(255) NULL				-- Integration Services (SSIS) csomagot  futtat� alkalmaz�s INTERACTIVE_MODE
,  [ErrorCode]			INT NULL						-- Az Integration Services (SSIS) csomag fut�sa sor�n keletkezett (Els�dleges) hiba k�dja
,  [ErrorDesc]			VARCHAR(4000) NULL				-- Az Integration Services (SSIS) csomag fut�sa sor�n keletkezett (Els�dleges) hiba megnevez�se
,  [ErrorSource]		VARCHAR(100) NULL				-- Annak az Integration Service (SSIS) task-nak a neve, amely a hib�t okozta
,  [RowCount]			INT NULL						-- A package �ltal bet�lt�tt sorok sz�ma (UPDATE/INSERT)
,  [ServerExecutionID]	BIGINT NULL						-- Kapcsolat az SSIS be�p�tett napl�val
,  [RunDecision]		VARCHAR(50) NULL				-- Ez az oszlop tartalmazza, hogy az SSIS csomag hogyan d�nt�tt a fut�sa kezdet�n. K�t �rt�ke lehet az oszlopnak: 1 = �j vagy ism�telt bet�lt�s 2 = A csomag tartalma nem hajt�dott v�gre, mert erre a d�tumra egyszer m�r sikeresen futott az SSIS csomag
,  [Status]				NVARCHAR(20) NULL				-- St�tusz, ami a package fut�s�hoz kapcsol�dik.
, CONSTRAINT [PK_dbo.PackageLog] PRIMARY KEY CLUSTERED 
( [PackageLogKey] )
) ON [PRIMARY];
GO

/* �tker�lt az ExtractInterface adatb�zisba */
-- CHANGE_TRACKING_CURRENT_VERSION(); �rt�ke ker�l let�rol�sra az Axdb adatb�zisb�l- utols�, v�gleges�tett tranzakci� verzi�sz�m�t adja vissza
--IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.SyncMetadata') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--	DROP TABLE dbo.SyncMetadata;
--GO
--CREATE TABLE dbo.SyncMetadata
--(
--	Id INT IDENTITY PRIMARY KEY,	
--	LastVersion BIGINT NOT NULL DEFAULT 0, 
--	TableName NVARCHAR(32) NOT NULL DEFAULT '', -- vagy 'InventSum', vagy 'PRICEDISCTABLE'
--	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE(), -- datum, ido
--	[Status] INT NOT NULL DEFAULT 1 -- st�tusz (Passive = 0, New = 1, Posted = 2)

--)

-- select object_id('Axdb_20130131.dbo.PRICEDISCTABLE')
-- select object_id('Axdb_20130131.dbo.InventSum')
