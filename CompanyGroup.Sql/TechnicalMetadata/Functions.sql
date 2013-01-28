/* =============================================
	description	   : kell-e futnia a package-nak, vagy sem?
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.26.
	modified date  :
	modify reason  :
 ============================================= */
USE [TechnicalMetadata]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
DROP FUNCTION dbo.CalculateRunDecision
GO
CREATE FUNCTION dbo.CalculateRunDecision(@JobLogKey int = -1, @PackageName nvarchar(255) = '')
RETURNS INT
AS
BEGIN
	DECLARE @RunDecision INT = 0, @Status NVARCHAR(20) = '';

	IF (@JobLogKey = -1)			-- kézi betöltés esetén kell futnia
		SET @RunDecision = 1;
	ELSE IF (@PackageName = '')
		SET @RunDecision = 0;
	ELSE IF (@JobLogKey > 0)		-- nem kézi betöltés esetén
	BEGIN
		-- új töltés esetén kell futnia
		IF( NOT EXISTS( SELECT * FROM dbo.PackageLog WHERE JobLogKey = @JobLogKey AND PackageName = @PackageName ) )	
			SET @RunDecision = 1;
		ELSE		-- ismételt töltés esetén
		BEGIN
			SELECT TOP 1 @Status = [Status]	-- utolsó futás sikeres volt -e?
			FROM dbo.PackageLog 
			WHERE JobLogKey = @JobLogKey AND PackageName = @PackageName
			ORDER BY PackageLogKey DESC;
			
			SET @Status = ISNULL(@Status, '' );

			IF (@Status = dbo.GetSucceededConstans())	-- Az utolsó töltés sikeres volt az adott batch-re, ekkor nem kell futnia.
				SET @RunDecision = 0;
			ELSE										-- Az utolsó töltés nem volt sikeres az adott batch-re, ekkor kell futnia.
				SET @RunDecision = 1;
		END
	END 
	
	RETURN @RunDecision;	
END

GO

DROP FUNCTION dbo.ConvertInteractiveMode
GO
CREATE FUNCTION dbo.ConvertInteractiveMode(	@value int )
RETURNS NVARCHAR(50)
AS
BEGIN
	RETURN CASE WHEN @value = 1 THEN 'USER_INTERFACE' ELSE 'PROGRAM' END;
END

/*
select dbo.ConvertInteractiveMode(0);
*/

GO
DROP FUNCTION dbo.GetFailedConstans
GO
CREATE FUNCTION dbo.GetFailedConstans()
RETURNS VARCHAR(10)
AS
BEGIN
	RETURN 'FAILED';
END

-- SELECT dbo.GetFailedConstans();

GO
DROP FUNCTION dbo.GetProcessingConstans
GO
CREATE FUNCTION dbo.GetProcessingConstans()
RETURNS VARCHAR(10)
AS
BEGIN
	RETURN 'PROCESSING';
END

-- SELECT dbo.GetProcessingConstans();

GO
DROP FUNCTION dbo.GetSucceededConstans
GO
CREATE FUNCTION dbo.GetSucceededConstans()
RETURNS VARCHAR(10)
AS
BEGIN
	RETURN 'SUCCEEDED';
END

-- SELECT dbo.GetSucceededConstans();