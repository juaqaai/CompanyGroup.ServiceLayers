/*
legnagyobb felhaszn�lhat� "CT Version" az adatok �tvitel�hez
*/
USE [Axdb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.CurrentVersionCT
GO
CREATE PROCEDURE InternetUser.CurrentVersionCT
AS
SET NOCOUNT ON

	declare @CurrentVersion bigint = CHANGE_TRACKING_CURRENT_VERSION();

	select @CurrentVersion as CurrentVersion;

RETURN

GO
GRANT EXECUTE ON InternetUser.CurrentVersionCT TO [HRP_HEADOFFICE\AXPROXY]
GO

-- exec InternetUser.CurrentVersionCT