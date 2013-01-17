USE WebDb_Test;
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [InternetUser].[SignIn];
GO
CREATE PROCEDURE [InternetUser].[SignIn]
	@UserName nvarchar(32) = '',
	@Password nvarchar(32) = '',
	@DataAreaId nvarchar(3) = ''	-- 1:HRP, 2:BSC, 3:SRV, 4:SER
AS

DECLARE @RecId bigint,
		@CompanyId nvarchar(10),
		@CompanyName nvarchar(60),
		@PersonId nvarchar(10),
		@PersonName nvarchar(60),
		@Email nvarchar(80),
		@IsWebAdministrator bit,
		@InvoiceInfoEnabled bit,
		@PriceListDownloadEnabled bit,
		@CanOrder bit,
		@RecieveGoods bit,
		@PaymTermId nvarchar(10),
		@Currency nvarchar(3),
	    @LanguageId nvarchar(10),
		@PriceGroup nvarchar(4),
		@InventLocation nvarchar(10),
		@LoginType int, 
		@PartnerModel int,
		@RepresentativeId nvarchar(32),
		@RepresentativeName nvarchar(64),
		@RepresentativePhone nvarchar(32),
		@RepresentativeMobile nvarchar(32),
		@RepresentativeExtension nvarchar(16),
		@RepresentativeEmail nvarchar(64);

		DECLARE @VirtualDataAreaId nvarchar(3) = InternetUser.GetVirtualDataAreaId( @DataAreaId );

		SELECT @RecId = RecId,
			   @CompanyId = CustAccount, 
			   @PersonId = ContactPersonId 
		FROM axdb_20120614.dbo.WebShopUserInfo WHERE ( WebLoginName = @UserName ) AND 
											( Pwd = @Password ) AND 
											( RightHrp = 1 OR RightBsc = 1 OR RightSzerviz = 1 ) AND 
											( DataAreaId = @VirtualDataAreaId );

		IF ( ISNULL(@PersonId, '') <> '' ) -- kapcsolattartokent, szemelyes belepes
		BEGIN
			SELECT --@PersonId = Cp.ContactPersonId, 
				   @PersonName = Cp.Name, 
				   @CanOrder = Cp.ArutRendelhet, 
				   @RecieveGoods = Cp.ArutAtvehet, 
				   @PriceListDownloadEnabled = Cp.Arlist, 
				   @IsWebAdministrator = Cp.[Admin], 
				   @InvoiceInfoEnabled = Cp.SzlaInfo, 
				   @CompanyId = Cp.CustAccount, 
				   @CompanyName = ISNULL( Cust.Name, '' ), 
				   @Email = Cp.Email,
				   @PartnerModel = InternetUser.CalculatePartnerModel( Cust.Hrp, Cust.Bsc ), 
				   @PaymTermId = Cust.PaymTermId, 
				   @Currency = Cust.Currency, 
				   @InventLocation = Cust.InventLocation, 
				   @LanguageId = Cust.LanguageId, 
				   @PriceGroup = Cust.PriceGroup, 
				   @RepresentativeId = Cust.Kepviselo, 
				   @LoginType = 2
			FROM axdb_20120614.dbo.ContactPerson as Cp 
				 INNER JOIN axdb_20120614.dbo.CustTable as Cust ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID = @VirtualDataAreaId	--Cust.DataAreaID
			WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
				  Cust.DataAreaID = @DataAreaId AND 
				  cp.ContactPersonId = @PersonId AND 
				  cp.CustAccount = CASE WHEN ISNULL(@CompanyId, '') = '' THEN cp.CustAccount ELSE @CompanyId END; 
		END
		ELSE
		BEGIN
			IF ( ISNULL(@CompanyId, '') <> '' )	-- cegkent lep be
	  		BEGIN

				SELECT --@CompanyId = AccountNum, 
					   @CompanyName = Name, 
					   @Email = Email,
					   @PartnerModel = InternetUser.CalculatePartnerModel( Hrp, Bsc ),
					   @PaymTermId = PaymTermId, 
					   @Currency = Currency, 
					   @InventLocation = InventLocation, 
					   @LanguageId = LanguageId, 
					   @PriceGroup = PriceGroup, 
					   @RepresentativeId = Kepviselo
				FROM axdb_20120614.dbo.CustTable
				WHERE StatisticsGroup <> 'Arhív' AND 
					  StatisticsGroup <> 'Archív' AND 
					  DataAreaID = @DataAreaId AND 
					  ACCOUNTNUM = @CompanyId;

				SET @LoginType = 1;
				SET @CanOrder = 1;
				SET @RecieveGoods = 1;
				SET @PriceListDownloadEnabled = 1;
				SET @IsWebAdministrator = 1;
				SET @InvoiceInfoEnabled = 1;
			END
		END

		IF (ISNULL(@RepresentativeId, '') <> '')
		BEGIN
			SELECT @RepresentativeId = EmplId, 
				   @RepresentativeName = [Name], 
				   @RepresentativePhone = Phone, 
				   @RepresentativeMobile = CellularPhone, 
				   @RepresentativeExtension = PhoneLocal, 
				   @RepresentativeEmail = Email
			FROM AxDb.dbo.EmplTable 
			WHERE EmplId = @RepresentativeId AND 
				  DataAreaId = @VirtualDataAreaId;
		END			  

		SELECT  0 as Id, '' as VisitorId, '' as LoginIP,
				ISNULL(@RecId, 0) as RecId, 
				ISNULL(@CompanyId, '') as CustomerId,
				ISNULL(@CompanyName, '') as CustomerName,
				ISNULL(@PersonId, '') as PersonId,
				ISNULL(@PersonName, '') as PersonName,
				ISNULL(@Email, '') as Email,
				ISNULL(@IsWebAdministrator, 0) as IsWebAdministrator,
				ISNULL(@InvoiceInfoEnabled, 0) as InvoiceInfoEnabled,
				ISNULL(@PriceListDownloadEnabled, 0) as PriceListDownloadEnabled,
				ISNULL(@CanOrder, 0) as CanOrder,
				ISNULL(@RecieveGoods, 0) as RecieveGoods,
				ISNULL(@PaymTermId, '') as PaymTermId, 
				ISNULL(@Currency, '') as Currency, 
				ISNULL(@LanguageId, '') as LanguageId,
				ISNULL(@PriceGroup, '2' ) as DefaultPriceGroupId, 
				ISNULL(@InventLocation, '') as InventLocationId, 
				ISNULL(@RepresentativeId, '') as RepresentativeId, 
				ISNULL(@RepresentativeName, '') as RepresentativeName,
				ISNULL(@RepresentativePhone, '') as RepresentativePhone, 
				ISNULL(@RepresentativeMobile, '') as RepresentativeMobile,
				ISNULL(@RepresentativeExtension, '') as RepresentativeExtension,
				ISNULL(@RepresentativeEmail, '') as RepresentativeEmail, 
				@DataAreaId as DataAreaId,
				ISNULL(@LoginType, 0) as LoginType, 
				ISNULL(@PartnerModel, 0 ) as PartnerModel,
				CONVERT(BIT, 0) as AutoLogin, 
				GETDATE() as LoginDate, 
				CONVERT(DateTime, 0) as LogoutDate, 
				DATEADD(day, 1, GetDate()) as ExpireDate, 
				CONVERT(BIT, 1) as Valid
		RETURN;
GO
GRANT EXECUTE ON InternetUser.SignIn TO InternetUser
GO
-- exec InternetUser.SignIn 'elektroplaza', '58915891', 'bsc';
-- exec InternetUser.SignIn 'ipon', 'gild4MAX19', 'hrp';
-- exec InternetUser.SignIn 'plorinczy', 'pikolo', 'hrp';
-- select * from axdb_20120614.dbo.WebShopUserInfo WHERE ( WebLoginName = 'ipon' )	gild4MAX19
-- select * from AxDb.dbo.EmplTable

DROP FUNCTION InternetUser.CalculatePartnerModel
GO
CREATE FUNCTION InternetUser.CalculatePartnerModel( @Hrp INT, @Bsc INT )
RETURNS INT
AS
BEGIN
	DECLARE @Result INT;

	SET @Result = 0;

	IF (@Hrp = 1 AND @Bsc = 1)
	BEGIN
		SET @Result = 3;
	END

	IF (@Hrp = 1 AND @Bsc = 0)
	BEGIN
		SET @Result = 1;
	END

	IF (@Hrp = 0 AND @Bsc = 1)
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
select * from AxDb.dbo.WebShopUserInfo where CustAccount = 'V005024'
*/