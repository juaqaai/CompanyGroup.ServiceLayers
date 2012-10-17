USE WebDb_Test
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[EmployeeList]
GO
/*
visszaadja a webre kitehető termékleírásokat
*/
CREATE PROCEDURE [InternetUser].[EmployeeList]
AS
SET NOCOUNT ON

	SELECT EmplId as EmployeeId, 
		   Name as Name,
		   PhoneLocal as Extension,
	       Email as Email,
		   CellularPhone as Mobile  
	FROM AxDb.dbo.EmplTable
	WHERE DataAreaID IN ('hun', 'ser');
RETURN;
GO
GRANT EXECUTE ON [InternetUser].[EmployeeList] TO InternetUser
GO
-- exec InternetUser.EmployeeList;