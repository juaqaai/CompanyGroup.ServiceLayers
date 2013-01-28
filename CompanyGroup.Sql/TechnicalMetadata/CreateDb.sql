/*
	description	   : generate TechnicalMetadata database 
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.25.
	modified date  :
	modify reason  :
	 	
*/
USE master;
GO
--DROP DATABASE TechnicalMetadata
--GO

	CREATE DATABASE TechnicalMetadata
	ON 
	PRIMARY
	( NAME = TechnicalMetadata,
		FILENAME = 'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\TechnicalMetadata.mdf',
		SIZE = 500MB,
		FILEGROWTH = 15% )
	LOG ON
	( NAME = TechnicalMetadata_log,
		FILENAME = 'L:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\TechnicalMetadata_log.ldf',
		SIZE = 500MB,
		FILEGROWTH = 15% );

GO
ALTER DATABASE TechnicalMetadata
SET RECOVERY SIMPLE
GO

USE TechnicalMetadata;
IF EXISTS (SELECT Name from sys.extended_properties where Name = 'Description')
    EXEC sys.sp_dropextendedproperty @name = 'Description'
EXEC sys.sp_addextendedproperty @name = 'Description', @value = 'Technical metadata  - JobLog, PackageLog, System data'
