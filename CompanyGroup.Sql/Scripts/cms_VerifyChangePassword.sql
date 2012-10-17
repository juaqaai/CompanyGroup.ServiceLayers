USE WebDb_Test;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.cms_VerifyChangePassword;
GO
CREATE PROCEDURE InternetUser.cms_VerifyChangePassword
	@ContactPersonId nvarchar(32) = '',
	@UserName nvarchar(32) = '',
	@OldPassword nvarchar(32) = '',
	@NewPassword nvarchar(32) = '',
	@DataAreaId nvarchar(3) = ''	-- 1:HRP, 2:BSC, 3:SRV, 4:SER
AS
	DECLARE @Code nvarchar(32) = '';

	IF ( NOT EXISTS( SELECT * FROM AxDb.dbo.WebShopUserInfo WHERE (ContactPersonId = @ContactPersonId) AND (WebLoginName = @UserName) AND (Pwd = @OldPassword) AND (RightHrp = CASE WHEN @DataAreaId = 'hrp' THEN 1 ELSE RightHrp END) AND (RightBsc = CASE WHEN @DataAreaId = 'bsc' THEN 1 ELSE RightBsc END) ) )
		SET @Code = 'UserNamePasswordNotFound';

	IF ( EXISTS( SELECT * FROM AxDb.dbo.WebShopUserInfo WHERE (WebLoginName = @UserName) AND (Pwd = @NewPassword) ) )
		SET @Code = 'PasswordAleradyExists';
		
	IF (@Code = '')
		SET @Code = 'UserNamePasswordOK';

	SELECT @Code as Code;
RETURN;
GO
GRANT EXECUTE ON InternetUser.cms_VerifyChangePassword TO InternetUser
GO

-- EXEC InternetUser.cms_VerifyChangePassword 'KAPCS06943', 'sero', 'sero2000', 'sero2004', 'hrp';