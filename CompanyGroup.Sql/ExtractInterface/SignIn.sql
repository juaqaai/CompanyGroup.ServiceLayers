USE ExtractInterface;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/*
A belépésnél maradt egy elvarratlan szál:
Ha az ax-ben vevõhöz tartozó belépési azonosítóval akarok bejelentkezni, és a vevõn az alábbi helyzet áll fenn: 
jelszón HRP, BSC pipa bennt, Vevõn a HRP-ben a staisztikai csoport Vevõ, a BSC-ben arhív és a vevõn a HRP, BSC pipa NINCS berakva, akkor az interneten a bejelentkezés ablak NEM írja ki, 
hogy sikertelen bejelentkezés, és ott marad felül. 
De ha becsukom a bejelentkezõ ablakot és F5-tel frissítem az oldalt, akkor meg be vagyok jelentkezve! 
Ez rossz, mivel a vevõn mindkét cégben NINCS bent a Hrp, BSC pipa, ez NEM beengedhetõ vevõ és neki is ki kéne írni, hogy sikertelen bejelentkezés.
*/

DROP PROCEDURE [InternetUser].[SignIn];
GO
CREATE PROCEDURE [InternetUser].[SignIn]
	@UserName nvarchar(32) = '',
	@Password nvarchar(32) = ''
AS
		DECLARE @VirtualDataAreaId nvarchar(3) = 'hun', 
				@RecId bigint = 0, 
				@CustomerId nvarchar(10),
				@PersonId nvarchar(10), 
				@RightHrp BIT = 0, 
				@RightBsc BIT = 0;

		--@CustomerName nvarchar(60) = '',
		--@CustomerEmail nvarchar(80) = '',
		--@PersonName nvarchar(60) = '',
		--@PersonEmail nvarchar(80) = '',
		--@CanOrder bit = 0,
		--@RecieveGoods bit = 0,
		--@PriceListDownloadEnabled bit = 0,
		--@IsAdministrator bit = 0,
		--@InvoiceInfoEnabled bit = 0,
		--@PaymTermId nvarchar(10) = '',
		--@Currency nvarchar(10) = '', 
		--@LanguageId nvarchar(10) = '', 
		--@DefaultPriceGroupId nvarchar(10) = '', 
		--@InventLocationId nvarchar(10) = '',

		-- van-e jogosultsága a hrp-be, vagy a bsc-be belépni? Ha van, akkor a @RecId, @CustomerId, @PersonId NULL-tól eltérõ értéket kap
		SELECT @RecId = RecId,
			   @CustomerId = CustAccount, 
			   @PersonId = ContactPersonId, 
			   @RightHrp = RightHrp,
			   @RightBsc = RightBsc
		FROM Axdb.dbo.WebShopUserInfo WHERE ( WebLoginName = @UserName ) AND 
											( Pwd = @Password ) AND 
											( RightHrp = 1 OR RightBsc = 1 ) AND 
											( DataAreaId = @VirtualDataAreaId );

		IF ( ISNULL(@PersonId, '') <> '' ) -- kapcsolattartokent, szemelyes belepes
		BEGIN

/*
VisitorId, LoginIP, RecId, CustomerId, CustomerName, PersonId, PersonName, Email,  
IsWebAdministrator,  InvoiceInfoEnabled,  PriceListDownloadEnabled, CanOrder, RecieveGoods,  
PaymTermIdBsc, PaymTermIdHrp, Currency, LanguageId, DefaultPriceGroupIdHrp, DefaultPriceGroupIdBsc, InventLocationIdHrp, InventLocationIdBsc, 
RepresentativeId,  LoginType, RightHrp, RightBsc, ContractHrp, ContractBsc, CartId, RegistrationId, IsCatalogueOpened, IsShoppingCartOpened, 
AutoLogin, LoginDate, LogoutDate, [ExpireDate], Valid

SELECT ROW_NUMBER() OVER(ORDER BY SalesYTD DESC) AS Row, 
    FirstName, LastName, ROUND(SalesYTD,2,1) AS "Sales YTD" 
FROM Sales.vSalesPerson
WHERE TerritoryName IS NOT NULL AND SalesYTD <> 0;

*/
			SELECT ROW_NUMBER() OVER(ORDER BY Cust.DataAreaId DESC) as Id, 
				   '' as VisitorId, '' as LoginIP,
				   ISNULL(@RecId, 0) as RecId, 
				   CustomerId = Cp.CustAccount, 
				   CustomerName = ISNULL( Cust.Name, '' ), 
				   PersonId = Cp.ContactPersonId, 
				   PersonName = Cp.Name, 
				   Email = Cp.Email,
				   IsWebAdministrator = Cp.[Admin], 
				   InvoiceInfoEnabled = Cp.SzlaInfo, 
				   PriceListDownloadEnabled = Cp.Arlist, 
				   CanOrder = Cp.ArutRendelhet, 
				   RecieveGoods = Cp.ArutAtvehet, 
				   PaymTermId = Cust.PaymTermId, 
				   Currency = Cust.Currency,
				   LanguageId = Cust.LanguageId, 
				   DefaultPriceGroupId = Cust.PriceGroup, 
				   InventLocationId = Cust.InventLocation, 
				   RepresentativeId = Cust.Kepviselo, 
				   RepresentativeName = ISNULL(Empl.[Name], ''), 
				   RepresentativePhone = ISNULL(Empl.Phone, ''), 
				   RepresentativeMobile = ISNULL(Empl.CellularPhone, ''), 
				   RepresentativeExtension = ISNULL(Empl.PhoneLocal, ''), 
				   RepresentativeEmail = ISNULL(Empl.Email, ''), 
				   Cust.DataAreaId, 
				   LoginType = 2, 
				   --PartnerModel = InternetUser.CalculatePartnerModel( Cust.Hrp, Cust.Bsc, Cust.DataAreaId ), 
				   CONVERT(BIT, ISNULL(@RightHrp, 0)) as RightHrp, 
				   CONVERT(BIT, ISNULL(@RightBsc, 0)) as RightBsc,
				   CONVERT(BIT, ISNULL(Cust.Hrp, 0)) as ContractHrp, 
				   CONVERT(BIT, ISNULL(Cust.Bsc, 0)) as ContractBsc,
				   0 as CartId, '' as RegistrationId, CONVERT(BIT, 0) as IsCatalogueOpened, CONVERT(BIT, 0) as IsShoppingCartOpened, 
				   CONVERT(BIT, 0) as AutoLogin, 
				   GETDATE() as LoginDate, 
				   CONVERT(DateTime, '1753.02.01') as LogoutDate, 
				   DATEADD(day, 1, GetDate()) as [ExpireDate], 
				   CASE WHEN Cust.DATAAREAID = 'hrp' AND Cust.Hrp = 1 
						THEN CONVERT(BIT, 1) 
						ELSE
							CASE WHEN Cust.DATAAREAID = 'bsc' AND Cust.Bsc = 1 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END
				   END as Valid
			FROM Axdb.dbo.CustTable as Cust 
				 INNER JOIN Axdb.dbo.ContactPerson as Cp ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID = @VirtualDataAreaId	--Cust.DataAreaID 
				 LEFT OUTER JOIN AxDb.dbo.EmplTable as Empl ON  EmplId = Cust.Kepviselo AND Empl.DataAreaId = @VirtualDataAreaId
			WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
				  Cust.DataAreaID IN ('hrp', 'bsc') AND 
				  cp.ContactPersonId = @PersonId AND 
				  cp.CustAccount = CASE WHEN ISNULL(@CustomerId, '') = '' THEN cp.CustAccount ELSE @CustomerId END; 

				  -- select * from Axdb.dbo.CustTable where AccountNum = 'V001446'
				  -- select * from Axdb.dbo.ContactPerson where ContactPersonId = 'KAPCS03399'
		END
		ELSE
		BEGIN
			IF ( ISNULL(@CustomerId, '') <> '' )	-- cegkent lep be
	  		BEGIN
				SELECT ROW_NUMBER() OVER(ORDER BY Cust.DataAreaId DESC) as Id,
					   '' as VisitorId, '' as LoginIP,
					   ISNULL(@RecId, 0) as RecId, 
					   CustomerId = AccountNum, 
					   CustomerName = ISNULL( Cust.Name, '' ), 
					   PersonId = '', 
					   PersonName = '', 
					   Email = Cust.Email,
					   IsWebAdministrator = 1, 
					   InvoiceInfoEnabled = 1, 
					   PriceListDownloadEnabled = 1, 
					   CanOrder = 1, 
					   RecieveGoods = 1, 
					   PaymTermId = Cust.PaymTermId, 
					   Currency = Cust.Currency,
					   LanguageId = Cust.LanguageId, 
					   DefaultPriceGroupId = Cust.PriceGroup, 
					   InventLocationId = Cust.InventLocation, 
					   RepresentativeId = Cust.Kepviselo, 
					   RepresentativeName = ISNULL(Empl.[Name], ''), 
					   RepresentativePhone = ISNULL(Empl.Phone, ''), 
					   RepresentativeMobile = ISNULL(Empl.CellularPhone, ''), 
					   RepresentativeExtension = ISNULL(Empl.PhoneLocal, ''), 
					   RepresentativeEmail = ISNULL(Empl.Email, ''), 
					   Cust.DataAreaId, 
					   LoginType = 1, 
					   --PartnerModel = InternetUser.CalculatePartnerModel( Cust.Hrp, Cust.Bsc, Cust.DataAreaId ), 
					   CONVERT(BIT, ISNULL(@RightHrp, 0)) as RightHrp, 
				       CONVERT(BIT, ISNULL(@RightBsc, 0)) as RightBsc,
				       CONVERT(BIT, ISNULL(Cust.Hrp, 0)) as ContractHrp, 
				       CONVERT(BIT, ISNULL(Cust.Bsc, 0)) as ContractBsc,
					   0 as CartId, '' as RegistrationId, CONVERT(BIT, 0) as IsCatalogueOpened, CONVERT(BIT, 0) as IsShoppingCartOpened, 
					   CONVERT(BIT, 0) as AutoLogin, 
					   GETDATE() as LoginDate, 
					   CONVERT(DateTime, '1753.02.01') as LogoutDate, 
					   DATEADD(day, 1, GetDate()) as [ExpireDate], 
					   CASE WHEN Cust.DATAAREAID = 'hrp' AND Cust.Hrp = 1 
							THEN CONVERT(BIT, 1) 
							ELSE
								CASE WHEN Cust.DATAAREAID = 'bsc' AND Cust.Bsc = 1 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END
					   END as Valid
				FROM Axdb.dbo.CustTable as Cust 
					 LEFT OUTER JOIN AxDb.dbo.EmplTable as Empl ON  EmplId = Cust.Kepviselo AND Empl.DataAreaId = @VirtualDataAreaId
				WHERE Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
					  Cust.DataAreaID IN ('hrp', 'bsc') AND ACCOUNTNUM = @CustomerId;
			END
			ELSE
			BEGIN
				SELECT  0 as Id, '' as VisitorId, '' as LoginIP,
						0 as RecId, 
						'' as CustomerId,
						'' as CustomerName,
						''  as PersonId,
						''  as PersonName,
						''  as Email,
						0 as IsWebAdministrator,
						0 as InvoiceInfoEnabled,
						0 as PriceListDownloadEnabled,
						0 as CanOrder,
						0 as RecieveGoods,
						'' as PaymTermId, 
						'' as Currency, 
						'' as LanguageId,
						'2' as DefaultPriceGroupId, 
						'' as InventLocationId, 
						'' as RepresentativeId, 
						'' as RepresentativeName,
						'' as RepresentativePhone, 
						'' as RepresentativeMobile,
						'' as RepresentativeExtension,
						'' as RepresentativeEmail, 
						''  as DataAreaId,
						0 as LoginType, 
						--0 as PartnerModel,
						CONVERT(BIT, ISNULL(@RightHrp, 0)) as RightHrp, 
						CONVERT(BIT, ISNULL(@RightBsc, 0)) as RightBsc,
						CONVERT(BIT, 0) as ContractHrp, 
						CONVERT(BIT, 0) as ContractBsc,
						0 as CartId, '' as RegistrationId, CONVERT(BIT, 0) as IsCatalogueOpened, CONVERT(BIT, 0) as IsShoppingCartOpened, 
						CONVERT(BIT, 0) as AutoLogin, 
						GETDATE() as LoginDate, 
						CONVERT(DateTime, '1753.02.01') as LogoutDate, 
						DATEADD(day, 1, GetDate()) as [ExpireDate], 
						CONVERT(BIT, 0) as Valid;
			END
		END

		RETURN;
GO
GRANT EXECUTE ON InternetUser.SignIn TO InternetUser
GO

--SELECT * 
--FROM Axdb.dbo.WebShopUserInfo
--INNER JOIN Axdb.dbo.CustTable
-- WHERE (RIGHTHRP = 1 AND RIGHTBSC = 0) OR (RIGHTHRP = 0 AND RIGHTBSC = 1)

-- exec InternetUser.SignIn 'elektroplaza', 'hrp5891ep';
-- exec InternetUser.SignIn 'ipon', 'gild4MAX19';
-- exec InternetUser.SignIn 'plorinczy', 'pikolo';
-- exec InternetUser.SignIn 'nador', '4koszeG';
-- exec InternetUser.SignIn 'netlogic', 'hrppass123';
-- exec InternetUser.SignIn 'kekszinfo', '8mmks812';
-- exec InternetUser.SignIn 'Mysoft', '3689478';
-- exec InternetUser.SignIn 'reveszg', 'Domonkos2000'
-- exec InternetUser.SignIn '22', '22'

select ACCOUNTNUM, KEPVISELO, count(ACCOUNTNUM), count(KEPVISELO)
from AXDB.dbo.CUSTTABLE 
where DataAreaID IN ('hrp', 'bsc') and STATISTICSGROUP <> 'Archív' and STATISTICSGROUP <> 'Arhív' and KEPVISELO <> '' -- ACCOUNTNUM = 'V002095'
group by ACCOUNTNUM, KEPVISELO
having count(KEPVISELO) = 1 and count(ACCOUNTNUM) = 1
order by ACCOUNTNUM, KEPVISELO

select ACCOUNTNUM, KEPVISELO from AXDB.dbo.CUSTTABLE where ACCOUNTNUM = 'V001565'

-- www.111.hu	0hfvmb1l

-- select * from Axdb.dbo.WebShopUserInfo WHERE ( WebLoginName = 'lacika' ) -- V000276	kincsesfoto, gild4MAX19
select * from Axdb.dbo.WebShopUserInfo WHERE CUSTACCOUNT = 'V016645'	--V001565

select * from Axdb.dbo.WebShopUserInfo WHERE ContactPersonId = 'KAPCS05327'

-- select * from AxDb.dbo.CustTable where Bsc = 1 and Hrp = 1 AccountNum = 'V002095' -- V002319
/*
select * FROM Axdb.dbo.CustTable as Cust 
INNER JOIN Axdb.dbo.ContactPerson as Cp ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID = 'hun'
where Cp.CONTACTPERSONID = 'KAPCS06504'	-- 24517/SZL

*/

/*
;
with Duplicates1(WebLoginName, pwd, ContactPersonId, CUSTACCOUNT )
as (select WebLoginName, pwd, ContactPersonId, CUSTACCOUNT
from Axdb.dbo.WebShopUserInfo
where CUSTACCOUNT <> '' and WebLoginName <> '' and pwd <> ''
group by WebLoginName, pwd, ContactPersonId, CUSTACCOUNT
having count(*) > 1), 
Duplicates2(WebLoginName, pwd, ContactPersonId, CUSTACCOUNT )
as (select WebLoginName, pwd, ContactPersonId, CUSTACCOUNT
from Axdb.dbo.WebShopUserInfo
where ContactPersonId <> '' and WebLoginName <> '' and pwd <> ''
group by WebLoginName, pwd, ContactPersonId, CUSTACCOUNT
having count(*) > 1)
select * from Duplicates1
union all 
select * from Duplicates2

select Duplicates.WebLoginName, C.NAME  from Duplicates 
left outer join Axdb.dbo.CustTable as C on C.AccountNum = Duplicates.CUSTACCOUNT 

where Duplicates.CUSTACCOUNT <> '' and C.STATISTICSGROUP <> 'Archív' and C.STATISTICSGROUP <> 'Arhív'

select count(*)
from Axdb.dbo.WebShopUserInfo
group by WebLoginName, pwd
*/

/*
kiszámítja, hogy a partner melyik vállalatban van regisztrálva
0: egyikben sem, 1: csak a hrp-ben, 2: csak a bsc-ben, 3: mindkét vállalatban
*/
DROP FUNCTION InternetUser.CalculatePartnerModel
GO
CREATE FUNCTION InternetUser.CalculatePartnerModel( @Hrp INT, @Bsc INT, @DataAreaId NVARCHAR(3))
RETURNS INT
AS
BEGIN
	DECLARE @Result INT;

	SET @Result = 0;

	IF (@Hrp = 1 AND @Bsc = 1)
	BEGIN
		SET @Result = 3;
	END

	IF (@Hrp = 1 AND @Bsc = 0 AND @DataAreaId = 'hrp')
	BEGIN
		SET @Result = 1;
	END

	IF (@DataAreaId = 'bsc' AND @Bsc = 1 AND @Hrp = 0)
	BEGIN
		SET @Result = 2;
	END

	RETURN @Result;
END
GO
GRANT EXECUTE ON InternetUser.CalculatePartnerModel TO InternetUser
GO

-- select * from AxDb.dbo.PBAUserProfiles

/*
select * from AxDb.dbo.WebShopUserInfo where CustAccount = 'V001446'	'V004875'	--V005024
select * from AxDb.dbo.WebShopUserInfo where ContactPersonId = 'KAPCS11030'
*/