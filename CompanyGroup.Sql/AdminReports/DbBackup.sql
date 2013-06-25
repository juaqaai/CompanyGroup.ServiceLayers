USE [ExtractInterface]
GO

/*
select top 1000 * FROM SSISDB.[catalog].[executions]
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[AdminReports_DbBackup]
GO

CREATE PROCEDURE [dbo].[AdminReports_DbBackup]
AS 
BEGIN
SELECT d.name as DBName,  
	ISNULL(CONVERT(varchar(16), bs_D.LastBackup, 120),'SOHA') as LastFullBackup,  
	ISNULL(CONVERT(varchar(16), bs_I.LastBackup, 120),'SOHA') as LastDiffBackup,  
	CASE WHEN d.recovery_model_desc = 'SIMPLE' THEN 'Nincs adat' ELSE ISNULL(CONVERT(varchar(16), bs_L.LastBackup, 120),'SOHA') END as LastTLogBackup  
FROM 
	sys.databases as d  
	LEFT OUTER JOIN (select database_name, max(backup_finish_date) LastBackup FROM msdb.dbo.backupset WHERE type = 'D' group by database_name) as bs_D ON bs_D.database_name = d.name 
	LEFT OUTER JOIN (select database_name, max(backup_finish_date) LastBackup FROM msdb.dbo.backupset WHERE type = 'I' group by database_name) as bs_I ON bs_I.database_name = d.name 
	LEFT OUTER JOIN (select database_name, max(backup_finish_date) LastBackup FROM msdb.dbo.backupset WHERE type = 'L' group by database_name) as bs_L ON bs_L.database_name = d.name 
WHERE d.name != 'tempdb' 
ORDER BY d.Name;

END
RETURN 0
GO

-- EXEC dbo. [AdminReports_DbBackup]

