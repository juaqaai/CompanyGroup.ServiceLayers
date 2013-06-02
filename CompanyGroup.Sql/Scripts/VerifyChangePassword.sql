USE ExtractInterface;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.VerifyChangePassword;
GO
CREATE PROCEDURE InternetUser.VerifyChangePassword
	@ContactPersonId nvarchar(32) = '',
	@UserName nvarchar(32) = '',
	@OldPassword nvarchar(32) = '',
	@NewPassword nvarchar(32) = '',
	@DataAreaId nvarchar(3) = ''	-- 1:HRP, 2:BSC, 3:SRV, 4:SER
AS
	DECLARE @Code nvarchar(32) = '';

	IF ( NOT EXISTS( SELECT * FROM Axdb.dbo.WebShopUserInfo WHERE (ContactPersonId = @ContactPersonId) AND (WebLoginName = @UserName) AND (Pwd = @OldPassword) AND (RightHrp = 1  OR RightBsc = 1) AND (@DataAreaId IN ('hrp', 'bsc')) ) )
		SET @Code = 'UserNamePasswordNotFound';

	IF ( EXISTS( SELECT * FROM Axdb.dbo.WebShopUserInfo WHERE (WebLoginName = @UserName) AND (Pwd = @NewPassword) ) )
		SET @Code = 'PasswordAleradyExists';
		
	IF (@Code = '')
		SET @Code = 'UserNamePasswordOK';

	SELECT @Code as Code;
RETURN;
GO
GRANT EXECUTE ON InternetUser.VerifyChangePassword TO InternetUser
GO

-- EXEC InternetUser.VerifyChangePassword 'KAPCS06943', 'sero', 'sero2000', 'sero2004', 'hrp';
-- EXEC InternetUser.VerifyChangePassword '23614/SZL', 'plorinczy', 'pikolo', 'pikolo2', 'hrp';

-- EXEC InternetUser.VerifyChangePassword 'KAPCS03399', 'ipon', 'gild4MAX21', 'gild4MAX19', 'hrp';

-- SELECT * FROM Axdb.dbo.WebShopUserInfo WHERE (ContactPersonId = 'KAPCS03399') AND (WebLoginName = 'ipon') AND (Pwd = 'gild4MAX19')