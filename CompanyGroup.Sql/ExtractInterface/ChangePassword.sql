/* =============================================
	description	   : AXDB\HRPAXDB ExtractInterface adatbázisban Invoice tábla szerinti lekérdezés
	running script : InternetUser, Acetylsalicilum91 nevében
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */
 
USE ExtractInterface
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[ChangePassword];
GO
CREATE PROCEDURE [InternetUser].[ChangePassword] (@ContactPersonId NVARCHAR(10), 
												  @WebLoginName NVARCHAR(32), 
												  @OldPassword NVARCHAR(20), 
												  @NewPassword NVARCHAR(20))
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0, @RecId BIGINT;

    SET @RecId = ISNULL((SELECT RecId FROM AxDb.dbo.WebShopUserInfo WHERE ( ContactPersonId = @ContactPersonId ) AND
																		  ( WebLoginName = @WebLoginName ) AND
																		  ( Pwd = @OldPassword )), 0);
	IF (@RecId > 0)	
	BEGIN
		UPDATE AxDb.dbo.WebShopUserInfo SET Pwd = @NewPassword 
		WHERE RecId = @RecId;

		SET @Ret = 1;
	END
	ELSE
	BEGIN
		SET @Ret = -1;
	END

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[ChangePassword] TO InternetUser

GO

-- select * from AxDb.dbo.WebShopUserInfo where WebLoginName = 'joci2'