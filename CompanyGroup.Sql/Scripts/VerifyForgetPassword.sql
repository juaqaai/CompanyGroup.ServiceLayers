USE ExtractInterface;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.VerifyForgetPassword;
GO
CREATE PROCEDURE InternetUser.VerifyForgetPassword
	@UserName nvarchar(32) = ''
AS
	DECLARE @Code nvarchar(32) = '', @PersonId nvarchar(10), @CompanyId nvarchar(10);

	SELECT TOP 1 @PersonId = ContactPersonId, @CompanyId = CustAccount 
	FROM Axdb.dbo.WebShopUserInfo WHERE (WebLoginName = @UserName) AND 
												 ((RightHrp = 1 ) OR (RightBsc = 1)) AND 
												 (DataAreaId = 'hun');
	
	IF (ISNULL(@PersonId, '') = '' AND ISNULL(@CompanyId, '') = '' )																	   
		SET @Code = 'UserNameNotFound';
	ELSE
		IF (ISNULL(@PersonId, '') = '' AND ISNULL(@CompanyId, '') <> '' )	-- céghez rendelt jelszó megváltoztatása nem lehetséges
			SET @Code = 'NotAllowed';
		ELSE
			IF (EXISTS( SELECT * FROM Axdb.dbo.ContactPerson as Cp 
						   INNER JOIN Axdb.dbo.CustTable as Cust ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID ='hun'	
						   AND Cust.DataAreaID IN ('bsc', 'hrp')
								 WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
									   cp.ContactPersonId = @PersonId AND Cp.[Admin] <> 1 ))
				SET @Code = 'UserNameOK';
			ELSE
				SET @Code = 'NotAllowed';
		
	SELECT @Code as Code;
RETURN;
GO
GRANT EXECUTE ON InternetUser.VerifyForgetPassword TO InternetUser
GO

-- EXEC InternetUser.VerifyForgetPassword 'ipon';
-- EXEC InternetUser.VerifyForgetPassword 'plorinczy';
-- EXEC InternetUser.VerifyForgetPassword 'ghsport';
-- EXEC InternetUser.VerifyForgetPassword 'elektroplaza';

-- SELECT * FROM Axdb.dbo.WebShopUserInfo WHERE (ContactPersonId = '23614/SZL') AND (WebLoginName = 'plorinczy') AND (Pwd = 'pikolo')