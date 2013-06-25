USE [TechnicalMetadata]
GO

/*
select top 1000 * FROM SSISDB.[catalog].[executions]
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[AdminReports_MaxServerExecutionId]
GO

CREATE PROCEDURE [dbo].[AdminReports_MaxServerExecutionId]
AS 
BEGIN
	DECLARE @ExecutionId BIGINT = 0, @PackageName NVARCHAR(32) = '';
	
	SELECT @ExecutionId = MAX([execution_id])
	FROM SSISDB.[catalog].[executions];

	SELECT @PackageName = [package_name]
	FROM SSISDB.[catalog].[executions]
	WHERE [execution_id] = @ExecutionId;

	SELECT @ExecutionId as MaxServerExecutionId, @PackageName as PackageName;

END
RETURN 0
GO

-- EXEC [dbo].[AdminReports_MaxServerExecutionId]

/*
	visszaadja a betoltesenkent a legnagyobb ExecutionId-t.
*/
DROP PROCEDURE [dbo].[AdminReports_ServerExecutionIdList]
GO

CREATE PROCEDURE [dbo].[AdminReports_ServerExecutionIdList]
AS 
BEGIN
	SELECT MAX([execution_id]) as ExecutionId, [package_name] as PackageName
	FROM SSISDB.[catalog].[executions]
	GROUP BY [package_name]
	ORDER BY MAX([execution_id]) DESC;

END
RETURN 0
GO

-- EXEC [dbo].[AdminReports_ServerExecutionIdList]

DROP PROCEDURE [dbo].[AdminReports_ExecutionsById] 
GO

CREATE PROCEDURE [dbo].[AdminReports_ExecutionsById] @ExecutionId BIGINT = 0
AS 
BEGIN
	SELECT [execution_id] as ExecutionId
	--	,[folder_name] 
	--	,[project_name]
		,Replace([package_name],'.dtsx','') as PackageName
	--	,[environment_folder_name]
	--	,ISNULL([environment_name],'(none)') AS [environment_name]
		,Convert(varchar(19),[start_time],120) as StartTime
		,Convert(varchar(19),[end_time],120) as EndTime
		,Convert(varchar(8), Dateadd(ss, DATEDIFF(ss,[start_time],ISNULL([end_time],[start_time])), 0), 114) AS [TotalExecutionTime]
	--	,[status]
		,CASE	WHEN [status] = 1 THEN 'Created'
				WHEN [status] = 2 THEN 'Running'
				WHEN [status] = 3 THEN 'Cancelled'
				WHEN [status] = 4 THEN 'Failed'
				WHEN [status] = 5 THEN 'Pending'
				WHEN [status] = 6 THEN 'EndedUnexpectedly'
				WHEN [status] = 7 THEN 'Succeeded'
				WHEN [status] = 8 THEN 'Stopping'
				ELSE 'Completed'
		END AS [StatusName]
		,CASE	WHEN [status] IN (1,2,5,8)	THEN 'Pending'
			WHEN [status] IN (3,4)	THEN 'Failed'
			WHEN [status] = 6	THEN 'Warning'
			ELSE 'Success' --7,9
		END AS [StatusCategory]
		,CASE	WHEN [status] IN (1,2,5,8)	THEN 0
			WHEN [status] IN (3,4,6)	THEN 2
			ELSE 1 --7,9
		END AS [StatusIndicator]
		,[executed_as_name] as ExecutorName
	FROM SSISDB.[catalog].[executions]
	WHERE Execution_Id = @ExecutionId
END
RETURN 0
GO
-- EXEC [dbo].[AdminReports_ServerExecutionIdList]
-- EXEC [dbo].[AdminReports_ExecutionsById] 39450
-- EXEC [dbo].[AdminReports_ExecutionsById] 39345

DROP PROCEDURE [dbo].[AdminReports_LastExecutions] 
GO

CREATE PROCEDURE [dbo].[AdminReports_LastExecutions] 
AS 
BEGIN

	;
	WITH LastExecutions_CTE (ExecutionId)
	AS (
		SELECT MAX([execution_id]) as ExecutionId	--, [package_name] as PackageName
		FROM SSISDB.[catalog].[executions]
		GROUP BY [package_name]
	)

	SELECT [execution_id] as ExecutionId
	--	,[folder_name] 
	--	,[project_name]
		,Replace([package_name],'.dtsx','') as PackageName
	--	,[environment_folder_name]
	--	,ISNULL([environment_name],'(none)') AS [environment_name]
		,Convert(varchar(19),[start_time],120) as StartTime
		,Convert(varchar(19),[end_time],120) as EndTime
		,Convert(varchar(8), Dateadd(ss, DATEDIFF(ss,[start_time],ISNULL([end_time],[start_time])), 0), 114) AS [TotalExecutionTime]
	--	,[status]
		,CASE	WHEN [status] = 1 THEN 'Created'
				WHEN [status] = 2 THEN 'Running'
				WHEN [status] = 3 THEN 'Cancelled'
				WHEN [status] = 4 THEN 'Failed'
				WHEN [status] = 5 THEN 'Pending'
				WHEN [status] = 6 THEN 'EndedUnexpectedly'
				WHEN [status] = 7 THEN 'Succeeded'
				WHEN [status] = 8 THEN 'Stopping'
				ELSE 'Completed'
		END AS [StatusName]
		,CASE	WHEN [status] IN (1,2,5,8)	THEN 'Pending'
			WHEN [status] IN (3,4)	THEN 'Failed'
			WHEN [status] = 6	THEN 'Warning'
			ELSE 'Success' --7,9
		END AS [StatusCategory]
		,CASE WHEN [status] IN (1,2,5,8) THEN 0
			WHEN [status] IN (3,4,6) THEN 2
			ELSE 1 --7,9
		END AS [StatusIndicator]
		,[executed_as_name] as ExecutorName
	FROM SSISDB.[catalog].[executions] as Executions
	INNER JOIN LastExecutions_CTE ON Executions.execution_id = LastExecutions_CTE.ExecutionId
	ORDER BY Executions.execution_id DESC 
END
RETURN 0
GO

-- EXEC [dbo].[AdminReports_LastExecutions]