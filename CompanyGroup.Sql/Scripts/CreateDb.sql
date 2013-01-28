/*
	description	   : generate srv2 server Web database 
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
--DROP DATABASE Web
--GO

	CREATE DATABASE Web
	ON 
	PRIMARY
	( NAME = Web,
		FILENAME = 'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Web.mdf',
		SIZE = 500MB,
		FILEGROWTH = 15% )
	LOG ON
	( NAME = Web_log,
		FILENAME = 'L:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Web_log.ldf',
		SIZE = 500MB,
		FILEGROWTH = 15% );

GO
ALTER DATABASE Web
SET RECOVERY SIMPLE
GO

USE Web;
IF EXISTS (SELECT Name from sys.extended_properties where Name = 'Description')
    EXEC sys.sp_dropextendedproperty @name = 'Description'
EXEC sys.sp_addextendedproperty @name = 'Description', @value = 'Web data  - Catalogue, Login, Representative data'
