USE WebDb_Test;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.cms_VerifyForgetPassword;
GO
CREATE PROCEDURE InternetUser.cms_VerifyForgetPassword
	@UserName nvarchar(32) = '',
	@DataAreaId nvarchar(3) = ''	-- 1:HRP, 2:BSC, 3:SRV, 4:SER
AS
	DECLARE @Code nvarchar(32) = '', @VirtualDataAreaId nvarchar(3), @PersonId nvarchar(10), @CompanyId nvarchar(10);

	SET @VirtualDataAreaId = InternetUser.GetVirtualDataAreaId( @DataAreaId );

	SELECT TOP 1 @PersonId = ContactPersonId, @CompanyId = CustAccount 
	FROM axdb_20120614.dbo.WebShopUserInfo WHERE (WebLoginName = @UserName) AND 
										(RightHrp = CASE WHEN @DataAreaId = 'hrp' THEN 1 ELSE RightHrp END) AND 
										(RightBsc = CASE WHEN @DataAreaId = 'bsc' THEN 1 ELSE RightBsc END) AND 
										(@DataAreaId IN ('hrp', 'bsc'));
	
	IF (ISNULL(@PersonId, '') = '' AND ISNULL(@CompanyId, '') = '' )																	   
		SET @Code = 'UserNameNotFound';
	ELSE
		IF (ISNULL(@PersonId, '') = '' AND ISNULL(@CompanyId, '') <> '' )
			SET @Code = 'NotAllowed';
		ELSE
			IF (EXISTS( SELECT * FROM axdb_20120614.dbo.ContactPerson as Cp 
						   INNER JOIN axdb_20120614.dbo.CustTable as Cust ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID = @VirtualDataAreaId	--Cust.DataAreaID
								 WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
									   Cust.DataAreaID = @DataAreaId AND cp.ContactPersonId = @PersonId AND Cp.[Admin] <> 1 ))
				SET @Code = 'UserNameOK';
			ELSE
				SET @Code = 'NotAllowed';
		
	SELECT @Code as Code;
RETURN;
GO
GRANT EXECUTE ON InternetUser.cms_VerifyForgetPassword TO InternetUser
GO

-- EXEC InternetUser.cms_VerifyForgetPassword 'plexy', 'bsc';
-- EXEC InternetUser.cms_VerifyForgetPassword 'plorinczy', 'bsc';
-- EXEC InternetUser.cms_VerifyForgetPassword 'ghsport', 'hrp';
-- EXEC InternetUser.cms_VerifyForgetPassword 'elektroplaza', 'bsc';

-- SELECT * FROM axdb_20120614.dbo.WebShopUserInfo WHERE (ContactPersonId = '23614/SZL') AND (WebLoginName = 'plorinczy') AND (Pwd = 'pikolo')