USE ExtractInterface;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- 	@TempCompanyId nvarchar(20)	-- V016227
DROP PROCEDURE InternetUser.ForgetPasswordSelect;
GO
CREATE PROCEDURE InternetUser.ForgetPasswordSelect
	@UserName nvarchar(32) = '' 
AS
	DECLARE @Code nvarchar(32) = '', 
			@PersonId nvarchar(10) = '', 
			@CompanyId nvarchar(10) = '', 
			@Pwd nvarchar(32) = '', 
			@Email nvarchar(100) = '', 
			@CompanyName nvarchar(100) = '', 
			@PersonName nvarchar(100) = '';

	SELECT TOP 1 @PersonId = ContactPersonId, @CompanyId = CustAccount, @Pwd = PWD 
	FROM Axdb.dbo.WebShopUserInfo WHERE (WebLoginName = @UserName) AND 
												 ((RightHrp = 1 ) OR (RightBsc = 1)) AND 
												 (DataAreaId = 'hun');

	IF (ISNULL(@CompanyId, '') = '' ) AND (ISNULL(@PersonId, '') = '')																	   
	BEGIN
		SELECT 'UserNameNotFound' as Code, @UserName as UserName, '' as [Password], '' as Email, '' as CompanyName, '' as PersonName;
		RETURN;
	END

	IF (ISNULL(@Pwd, '') = '')
	BEGIN
		SELECT 'NotAllowed' as Code, @UserName as UserName, ISNULL(@Pwd, '') as [Password], '' as Email, '' as CompanyName, '' as PersonName;
		RETURN;
	END

	IF (ISNULL(@CompanyId, '') <> '')	-- céghez rendelt jelszó kiolvasása 
	BEGIN
		SELECT @Email = Email, @CompanyName = Name 
		FROM Axdb.dbo.CustTable 
		WHERE AccountNum = @CompanyId AND 
			  StatisticsGroup <> 'Arhív' AND 
			  StatisticsGroup <> 'Archív';

		SELECT 'OK' as Code, @UserName as UserName, ISNULL(@Pwd, '') as [Password], ISNULL(@Email, '') as Email, ISNULL(@CompanyName, '') as CompanyName, '' as PersonName;

		RETURN;
	END
	ELSE	-- személyhez rendelt jelszó kiolvasása
	BEGIN
		IF (ISNULL(@PersonId, '') <> '')
		BEGIN
			SELECT @Email = Cp.Email, @PersonName = Cp.Name, @CompanyName = Cust.Name 
			FROM Axdb.dbo.ContactPerson as Cp 
				 INNER JOIN Axdb.dbo.CustTable as Cust ON Cust.AccountNum = Cp.CustAccount AND 
																   Cp.DataAreaID ='hun' AND 
																   Cust.DataAreaID IN ('bsc', 'hrp')
			WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
				  Cp.ContactPersonId = @PersonId;

			SELECT 'OK' as Code, @UserName as UserName, ISNULL(@Pwd, '') as [Password], ISNULL(@Email, '') as Email, ISNULL(@CompanyName, '') as CompanyName, @PersonName as PersonName;

			RETURN;
		END
		ELSE
		BEGIN
			SELECT 'UserNameNotFound' as Code, @UserName as UserName, '' as [Password], '' as Email, '' as CompanyName, '' as PersonName;
			RETURN;
		END
	END

RETURN;
GO
GRANT EXECUTE ON InternetUser.ForgetPasswordSelect TO InternetUser
GO

-- EXEC InternetUser.ForgetPasswordSelect 'ipon';
-- EXEC InternetUser.ForgetPasswordSelect 'plorinczy'
-- EXEC InternetUser.ForgetPasswordSelect 'ghsport';	-- 'V016227'
-- EXEC InternetUser.ForgetPasswordSelect 'elektroplaza'

/*
select * FROM Axdb.dbo.WebShopUserInfo WHERE (WebLoginName = 'ipon') AND 
												 ((RightHrp = 1 ) OR (RightBsc = 1)) AND 
												 (DataAreaId = 'hun')

SELECT * FROM Axdb.dbo.WebShopUserInfo WHERE (ContactPersonId = '23614/SZL') AND (WebLoginName = 'plorinczy') AND (Pwd = 'pikolo')

*/
