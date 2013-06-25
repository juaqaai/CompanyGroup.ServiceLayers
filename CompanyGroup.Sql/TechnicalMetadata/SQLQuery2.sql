	SELECT  PackageLogKey,
		PackageName,
		Case
			When [Status] = 'SUCCEEDED' then 0
			When [Status] = 'FAILED' then 1
			Else 2 
		End as Indicator,
		StartTime,
		EndTime,
		--convert(varchar(8),dateadd(second,datediff(MINUTE, StartTime, EndTime),0),108),
		DATEDIFF(MINUTE, StartTime, EndTime) as ExecutionTimeinSec,
		Convert(varchar(16), DateAdd(MINUTE, DATEDIFF(MINUTE, StartTime, Isnull(EndTime,GetDate())), 0), 114) as ExecutionTime,
		[Status],
		ErrorCode,
		ErrorDesc,
		ErrorSource,
		[RowCount],
		ExecutedBy,
		InteractiveMode,
		RunDecision
		-- ,[JobLogKey] ,[InteractiveMode],[ServerExecutionID]
	FROM [TechnicalMetadata].[dbo].[PackageLog] 
	WHERE [ServerExecutionId] = 36179	--@ExecutionId
	ORDER BY PackageLogKey ASC 



	Select distinct(PackageName) as BatchDesc
From [TechnicalMetadata].[dbo].[PackageLog]
WHERE [ServerExecutionId] = @ExecutionId

SELECT Max([execution_id]) as MaxServerExecutionId
FROM	[SSISDB].[Catalog].[executions]

SELECT top 20 [execution_id]
FROM	[SSISDB].[catalog].[executions]
order by [execution_id] desc