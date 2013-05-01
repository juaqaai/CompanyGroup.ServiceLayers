USE ExtractInterface;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [InternetUser].[SignIn];
GO
CREATE PROCEDURE [InternetUser].[SignIn]
	@UserName nvarchar(32) = '',
	@Password nvarchar(32) = ''
AS

DECLARE @RecId bigint, 
		@CompanyId nvarchar(10),
		@PersonId nvarchar(10);

		DECLARE @VirtualDataAreaId nvarchar(3) = 'hun';	--InternetUser.GetVirtualDataAreaId( @DataAreaId );

		SELECT @RecId = RecId,
			   @CompanyId = CustAccount, 
			   @PersonId = ContactPersonId 
		FROM Axdb_20130131.dbo.WebShopUserInfo WHERE ( WebLoginName = @UserName ) AND 
											( Pwd = @Password ) AND 
											( RightHrp = 1 OR RightBsc = 1 ) AND 
											( DataAreaId = @VirtualDataAreaId );

		IF ( ISNULL(@PersonId, '') <> '' ) -- kapcsolattartokent, szemelyes belepes
		BEGIN
			SELECT 0 as Id, '' as VisitorId, '' as LoginIP,
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
				   PartnerModel = InternetUser.CalculatePartnerModel( Cust.Hrp, Cust.Bsc, Cust.DataAreaId ), 
				   CONVERT(BIT, 0) as AutoLogin, 
				   GETDATE() as LoginDate, 
				   CONVERT(DateTime, 0) as LogoutDate, 
				   DATEADD(day, 1, GetDate()) as ExpireDate, 
				   CASE WHEN Cust.DATAAREAID = 'hrp' AND Cust.Hrp = 1 THEN CONVERT(BIT, 1) ELSE
					   CASE WHEN Cust.DATAAREAID = 'bsc' AND Cust.Bsc = 1 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END
				   END as Valid
			FROM Axdb_20130131.dbo.CustTable as Cust 
				 INNER JOIN Axdb_20130131.dbo.ContactPerson as Cp ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID = @VirtualDataAreaId	--Cust.DataAreaID 
				 LEFT OUTER JOIN AxDb.dbo.EmplTable as Empl ON  EmplId = Cust.Kepviselo AND Empl.DataAreaId = @VirtualDataAreaId
			WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
				  Cust.DataAreaID IN ('hrp', 'bsc') AND 
				  cp.ContactPersonId = @PersonId AND 
				  cp.CustAccount = CASE WHEN ISNULL(@CompanyId, '') = '' THEN cp.CustAccount ELSE @CompanyId END; 

				  -- select * from Axdb_20130131.dbo.CustTable where AccountNum = 'V001446'
				  -- select * from Axdb_20130131.dbo.ContactPerson where ContactPersonId = 'KAPCS03399'
		END
		ELSE
		BEGIN
			IF ( ISNULL(@CompanyId, '') <> '' )	-- cegkent lep be
	  		BEGIN
				SELECT 0 as Id, '' as VisitorId, '' as LoginIP,
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
					   PartnerModel = InternetUser.CalculatePartnerModel( Cust.Hrp, Cust.Bsc, Cust.DataAreaId ), 
					   CONVERT(BIT, 0) as AutoLogin, 
					   GETDATE() as LoginDate, 
					   CONVERT(DateTime, 0) as LogoutDate, 
					   DATEADD(day, 1, GetDate()) as ExpireDate, 
					   CASE WHEN Cust.DATAAREAID = 'hrp' AND Cust.Hrp = 1 THEN CONVERT(BIT, 1) ELSE
						   CASE WHEN Cust.DATAAREAID = 'bsc' AND Cust.Bsc = 1 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END
					   END as Valid
				FROM Axdb_20130131.dbo.CustTable as Cust 
					 LEFT OUTER JOIN AxDb.dbo.EmplTable as Empl ON  EmplId = Cust.Kepviselo AND Empl.DataAreaId = @VirtualDataAreaId
				WHERE Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
					  Cust.DataAreaID IN ('hrp', 'bsc') AND ACCOUNTNUM = @CompanyId;
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
						0 as PartnerModel,
						CONVERT(BIT, 0) as AutoLogin, 
						GETDATE() as LoginDate, 
						CONVERT(DateTime, 0) as LogoutDate, 
						DATEADD(day, 1, GetDate()) as ExpireDate, 
						CONVERT(BIT, 1) as Valid;
			END
		END

		RETURN;
GO
GRANT EXECUTE ON InternetUser.SignIn TO InternetUser
GO
-- exec InternetUser.SignIn 'elektroplaza', 'hrp5891ep';
-- exec InternetUser.SignIn 'ipon', 'gild4MAX19';
-- exec InternetUser.SignIn 'plorinczy', 'pikolo';
-- select * from Axdb_20130131.dbo.WebShopUserInfo WHERE ( WebLoginName = 'joci2' )	gild4MAX19
-- select * from AxDb.dbo.EmplTable

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