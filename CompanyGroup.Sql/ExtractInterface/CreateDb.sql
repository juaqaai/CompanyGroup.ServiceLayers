/*
	description	   : generate AXDB\HRPAXDB Extract interface database 
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
--DROP DATABASE ExtractInterface
--GO

	CREATE DATABASE ExtractInterface
	ON 
	PRIMARY
	( NAME = ExtractInterface,
		FILENAME = 'D:\AXAPTA\MSSQL11.HRPAXDB\MSSQL\Data\ExtractInterface.mdf',
		SIZE = 500MB,
		FILEGROWTH = 15% )
	LOG ON
	( NAME = ExtractInterface_log,
		FILENAME = 'L:\AXAPTA\MSSQL11.HRPAXDB\MSSQL\Data\ExtractInterface_log.ldf',
		SIZE = 500MB,
		FILEGROWTH = 15% );

GO
ALTER DATABASE ExtractInterface
SET RECOVERY SIMPLE
GO

USE ExtractInterface;
IF EXISTS (SELECT Name from sys.extended_properties where Name = 'Description')
    EXEC sys.sp_dropextendedproperty @name = 'Description'
EXEC sys.sp_addextendedproperty @name = 'Description', @value = 'ExtractInterface stored procedures'
