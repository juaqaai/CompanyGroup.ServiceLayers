USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
visszaadja a termékmenedzsereket
*/
DROP PROCEDURE [InternetUser].[cms_EmployeeList];
GO
CREATE PROCEDURE [InternetUser].[cms_EmployeeList]( @DataAreaId nvarchar(3) = 'hrp' )
AS
SET NOCOUNT ON

	DECLARE @VirtualDataAreaId VARCHAR(3);

	SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'ser' THEN @DataAreaId ELSE 'hun' END;

	SELECT EmplId as EmployeeId, 
		   Name as Name,
		   PhoneLocal as Extension,
	       Email as Email,
		   CellularPhone as Mobile
	FROM axdb_20120614.dbo.EmplTable
	WHERE DataAreaID = @VirtualDataAreaId;
RETURN;
-- exec [InternetUser].[cms_EmployeeList]