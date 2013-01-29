/* =============================================
	description	   : 
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.25.
	modified date  :
	modify reason  :
 ============================================= */
USE TechnicalMetadata
GO

DROP PROCEDURE dbo.usp_JobLog_Start
GO
CREATE PROCEDURE [dbo].[usp_JobLog_Start]
	@JobName nvarchar(50),
	@ServerExecutionID bigint, 
	@SystemKey int
AS 
BEGIN
	INSERT dbo.JobLog ( JobName, StartTime, EndTime, ServerExecutionID, ErrorCode, ErrorDesc, ErrorSource, SystemKey )
	VALUES (@JobName, 
			GETDATE(),				-- [StartTime] A leválogatás futásának kezdete
			CONVERT(DateTime, 0),	-- [EndTime] A leválogatás futásának vége
			@ServerExecutionID, 
			'',
			'',
			'', 
			@SystemKey);

	SELECT @@IDENTITY as JobLogKey;

END
RETURN 0

/*	
EXEC dbo.usp_JobLOG_Start 'Job1', 1000;

SELECT * FROM dbo.JobLog

TRUNCATE TABLE dbo.JobLog;
  */

GO
DROP PROCEDURE dbo.usp_JobLog_Close
GO
CREATE PROCEDURE dbo.usp_JobLog_Close (@JobLogKey INT)
AS 
BEGIN
	IF ( EXISTS (SELECT * FROM dbo.JobLog WHERE JobLogKey= @JobLogKey) )
	BEGIN
		UPDATE dbo.JobLog 
		SET EndTime = GETDATE()		-- A leválogatás futásának vége
		WHERE JobLogKey = @JobLogKey;
	END

END

RETURN 0

/* 				
EXEC dbo.usp_JobLog_Close 1;

SELECT * FROM dbo.JobLog
*/

GO
DROP PROCEDURE dbo.usp_JobLog_OnError
GO
CREATE PROCEDURE dbo.usp_JobLog_OnError @JobLogKey INT,
										@ErrorCode  INT,
										@ErrorDesc  NVARCHAR(4000),
										@ErrorSource  NVARCHAR(100)
AS 
BEGIN
	IF ( EXISTS(SELECT * FROM dbo.JobLog WHERE JobLogKey= @JobLogKey) )
	BEGIN
		UPDATE dbo.JobLog 
		SET EndTime = GETDATE(),		-- A leválogatás futásának vége
			ErrorCode = @ErrorCode,
			ErrorDesc = @ErrorDesc,
			ErrorSource = @ErrorSource
		WHERE JobLogKey = @JobLogKey;
	END
END

RETURN 0

/* 				
EXEC dbo.usp_JobLog_OnError 1, 3000, 'Error desc', 'Error source';

SELECT * FROM dbo.JobLog
*/

/* =============================================
	description	   : package log
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.25.
	modified date  :
	modify reason  :
 ============================================= */

GO
DROP PROCEDURE dbo.usp_PackageLogStart;
GO
CREATE PROCEDURE dbo.usp_PackageLogStart ( @PackageName			NVARCHAR(255),
										   @InteractiveMode		BIT = 0,
										   @JobLogKey			INT = -1, 
										   @ServerExecutionID	BIGINT )
AS
	DECLARE @RunDecision INT,		-- hogyan döntött a package (fusson, vagy ne?)
			@PackageLogKey INT;		-- a betöltési napló egyedi sorazonosítója

	DECLARE @BatchDesc nvarchar(100);

	SET @RunDecision = dbo.CalculateRunDecision(@JobLogKey, @PackageName);

	INSERT INTO dbo.PackageLog
           ([PackageName]		-- Integration Services (SSIS) csomag neve
           ,[JobLogKey]			-- A Job futásának egyedi azonosítója
           ,[StartTime]			-- Integration Services (SSIS) csomag lefutásának kezdete
		   ,[EndTime]			-- Integration Services (SSIS) csomag lefutásának vége
           ,[ExecutedBy]		-- Integration Services (SSIS) csomagot futtató Windows felhasználó neve
           ,[InteractiveMode]	-- Integration Services (SSIS) csomagot  futtató alkalmazás INTERACTIVE_MODE
		   ,[ErrorCode]			-- Az Integration Services (SSIS) csomag futása során keletkezett (Elsõdleges) hiba kódja
		   ,[ErrorDesc]			-- Az Integration Services (SSIS) csomag futása során keletkezett (Elsõdleges) hiba megnevezése
		   ,[ErrorSource]		-- Annak az Integration Service (SSIS) task-nak a neve, amely a hibát okozta
		   ,[RowCount]			-- Az Integration Service (SSIS) package által betöltött ÚJ sorok száma (INSERT)
		   ,[ServerExecutionID]
           ,[RunDecision]		-- Ez az oszlop tartalmazza, hogy az SSIS csomag hogyan döntött a futása kezdetén. Két értéke lehet az oszlopnak: 1 új vagy ismételt betöltés 2 A csomag tartalma nem hajtódott végre, mert erre a dátumra egyszer már sikeresen futott az SSIS csomag
		   ,[Status]			
	) 
	VALUES
           (@PackageName
           ,@JobLogKey
           ,GETDATE()
		   ,CONVERT(DateTime, 0)
           ,SUSER_SNAME ()
	       ,dbo.ConvertInteractiveMode(@InteractiveMode)	-- interaktív mód, ha kézzel futtatjuk, akkor 0, egyébként pedig 1
		   ,0
		   ,''
		   ,''
		   ,0
		   ,@ServerExecutionID
           ,@RunDecision
		   ,dbo.GetProcessingConstans());

	SET @PackageLogKey = @@IDENTITY; 

	-- visszaadott értékek: újonnan létrehozott PackageLogKey, kell-e futnia, RunDecision kalkulált érték
    SELECT @PackageLogKey AS PackageLogKey, @RunDecision AS RunDecision;

RETURN 

/*
SELECT * FROM dbo.SPackageLog

EXEC dbo.usp_PackageLogStart @PackageName = 'Package1.dtsx', @ServerExecutionID = 1000;	

*/

GO
DROP PROCEDURE dbo.usp_PackageLog_Close;
GO
CREATE PROCEDURE dbo.usp_PackageLog_Close ( @PackageLogKey INT, @RowCount INT )
AS
BEGIN  
	IF ( EXISTS (SELECT * FROM dbo.PackageLog WHERE PackageLogKey = @PackageLogKey ) )	
	BEGIN
		UPDATE dbo.PackageLog
		SET EndTime	= GetDate(),
			[RowCount] = ISNULL(@RowCount, 0), 
			Status = dbo.GetSucceededConstans()
		WHERE PackageLogKey = @PackageLogKey;
	END
END
RETURN 0
/*
SELECT * FROM dbo.PackageLog

exec dbo.usp_PackageLog_Close 1, 100;	
*/

GO
DROP  PROCEDURE dbo.usp_PackageLogOnError;
GO
CREATE PROCEDURE dbo.usp_PackageLogOnError ( @PackageLogKey INT = -1,
											 @RowCount INT = 0, 
											 @ErrorCode INT = 0, 
											 @ErrorDesc VARCHAR(4000) = '', 
											 @ErrorSource VARCHAR(100) = '' )
AS
	UPDATE dbo.PackageLog
	SET EndTime	= GetDate(),
		[RowCount] = ISNULL(@RowCount, 0), 
		ErrorCode = @ErrorCode, 
		ErrorDesc = @ErrorDesc, 
		ErrorSource = @ErrorSource, 
		[Status] = dbo.GetFailedConstans()
	WHERE PackageLogKey= @PackageLogKey;

RETURN 0

-- exec dbo.usp_PackageLogOnError 1, 30, 10, 'Error desc', 'Error source';
-- SELECT * FROM dbo.PackageLog

GO
DROP  PROCEDURE dbo.usp_ValidateModelParameters;
GO
CREATE PROCEDURE dbo.usp_ValidateModelParameters(@ModelName NVARCHAR(50))
AS

	DECLARE @Model_id int 
	DECLARE @UserName nvarchar(50)= 'HRP_HEADOFFICE\juhasza' 
	DECLARE @User_ID int 
	DECLARE @Version_ID int 

	SET @User_ID = (SELECT ID  FROM ProductParameters.mdm.tblUser u WHERE u.UserName = @UserName);

	SET @Model_ID = (SELECT Top 1 Model_ID FROM ProductParameters.mdm.viw_SYSTEM_SCHEMA_VERSION WHERE Model_Name = @ModelName);

	SET @Version_ID = (SELECT MAX(ID) FROM ProductParameters.mdm.viw_SYSTEM_SCHEMA_VERSION WHERE Model_ID = @Model_ID);

	SELECT @User_ID as UserId, @Model_ID as ModelId, @Version_ID as VersionID;

RETURN 0

-- exec dbo.usp_ValidateModelParameters 'Web';