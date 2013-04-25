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