USE [TechnicalMetadata]
GO

/*
select top 1000 * FROM SSISDB.[catalog].[executions]
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP FUNCTION [dbo].[CalculateDateDiff];
GO
CREATE FUNCTION [dbo].[CalculateDateDiff](@Date1 DateTime, @Date2 DateTime)
RETURNS BIGINT
AS
BEGIN
	DECLARE @DiffInSeconds BIGINT;
	
	IF (@Date1 IS NULL)
		RETURN 0;

	IF (@Date2 IS NULL)
		RETURN 0;

	DECLARE @DiffInMinutes INT = DATEDIFF(MINUTE, @Date2, @Date1);

	IF (@DiffInMinutes > 0)
		SET @DiffInSeconds = @DiffInMinutes * 60;

	SET @DiffInSeconds = DATEPART(SECOND, @Date2) - DATEPART(SECOND, @Date1) + @DiffInSeconds;

	RETURN @DiffInSeconds;
END
GO
-- select [dbo].[CalculateDateDiff]( '2013-05-21 07:27:48', '2013-05-21 07:27:52');

DROP PROCEDURE [dbo].[AdminReports_PackageLogById]
GO
CREATE PROCEDURE [dbo].[AdminReports_PackageLogById] @ExecutionId BIGINT = 0
AS 
BEGIN
	SELECT PackageLogKey,
		PackageName,
		Case
			When [Status] = 'SUCCEEDED' then 0
			When [Status] = 'FAILED' then 1
			Else 2 
		End as Indicator,
		StartTime,
		EndTime,
		dbo.CalculateDateDiff(StartTime, EndTime) as ExecutionTimeinSec,
		--Convert(varchar(8), DateAdd(ss,DateDiff(ss, StartTime, Isnull(EndTime,GetDate())), 0), 114) as ExecutionTime,
		[Status],
		ErrorCode,
		ErrorDesc,
		ErrorSource,
		[RowCount]
		ExecutedBy,
		InteractiveMode,
		RunDecision
	FROM [TechnicalMetadata].[dbo].[PackageLog]
	WHERE [ServerExecutionId] = @ExecutionId;

END
RETURN 0
GO

-- EXEC [dbo].[AdminReports_PackageLogById]; 