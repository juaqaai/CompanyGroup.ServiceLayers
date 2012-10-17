-- DROP PROCEDURE [InternetUser].[SignIn]
CREATE PROCEDURE [InternetUser].[SignIn]
	@UserName nvarchar(32) = '',
	@Password nvarchar(32) = '',
	@DataAreaId nvarchar(3) = ''	-- 1:HRP, 2:BSC, 3:SRV, 4:SER
AS

/*CompanyId, CompanyName, PersonId, PersonName, LoggedIn, LoginType, ProfileId, 
(History)Url, (History)Date, (RequestParameter)Key, (RequestParameter)Value
(Permission)IsWebAdministrator, (Permission)InvoiceInfoEnabled, (Permission)PriceListDownloadEnabled, (Permission)CanOrder*/

DECLARE @CompanyId nvarchar(10),
		@CompanyName nvarchar(60),
		@PersonId nvarchar(10),
		@PersonName nvarchar(60),
		@CanOrder bit,
		@RecieveGoods bit,
		@PriceListDownloadEnabled bit,
		@IsWebAdministrator bit,
		@InvoiceInfoEnabled bit,
		@LoginType int, 
		@VirtualDataAreaId nvarchar(3);

	SET @VirtualDataAreaId = InternetUser.GetVirtualDataAreaId( @DataAreaId );

	IF ( EXISTS( SELECT * FROM AxDb.dbo.WebShopUserInfo WHERE ( WebLoginName = @UserName ) AND 
															  ( Pwd = @Password ) AND 
															  ( RightHrp = 1 OR RightBsc = 1 OR RightSzerviz = 1 ) AND 
															  ( DataAreaId = @VirtualDataAreaId ) ) ) 
	BEGIN
	-- kapcsolattartokent, szemelyes belepes 
		SELECT @PersonId = Cp.ContactPersonId, @PersonName = Cp.Name, 
			   @CanOrder = Cp.ArutRendelhet, @RecieveGoods = Cp.ArutAtvehet, @PriceListDownloadEnabled = Cp.Arlist, 
			   @IsWebAdministrator = Cp.[Admin], @InvoiceInfoEnabled = Cp.SzlaInfo, 
			   @CompanyId = Cp.CustAccount, @CompanyName = ISNULL( Cust.Name, '' )
		FROM AxDb.dbo.WebShopUserInfo as wui
			 INNER JOIN AxDb.dbo.ContactPerson as Cp ON wui.ContactPersonId = Cp.ContactPersonId AND cp.DataAreaID = wui.DataAreaID
			 INNER JOIN AxDb.dbo.CustTable as Cust ON Cust.AccountNum = Cp.CustAccount AND Cp.DataAreaID = @VirtualDataAreaId	--Cust.DataAreaID
		WHERE Cp.LeftCompany = 0 AND Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
			  Cust.DataAreaID = @DataAreaId AND 
			  wui.Pwd = @Password AND 
			  wui.WebLoginName = @UserName; 
			
		IF ( ISNULL( @PersonId, '' ) <> '' )	-- kapcsolattartokent, szemelyesen lep be, mert a @PersonId nem üres
		BEGIN					  
			SET @LoginType = 2;
			SELECT ISNULL(@CompanyId, '') as CompanyId,
				   ISNULL(@CompanyName, '') as CompanyName,
				   ISNULL(@PersonId, '') as PersonId,
				   ISNULL(@PersonName, '') as PersonName,
				   ISNULL(@CanOrder, 0) as CanOrder,
				   ISNULL(@RecieveGoods, 0) as RecieveGoods,
				   ISNULL(@PriceListDownloadEnabled, 0) as PriceListDownloadEnabled,
				   ISNULL(@IsWebAdministrator, 0) as IsWebAdministrator,
				   ISNULL(@InvoiceInfoEnabled, 0) as InvoiceInfoEnabled,
				   ISNULL(@LoginType, 0) as LoginType;
			RETURN;
		END

		IF ( @LoginType = 0 )	-- vevokent cegkent lep be (LoginType még mindíg 0)
		BEGIN
			SELECT @CompanyId = Cust.AccountNum, @CompanyName = Cust.Name
			FROM AxDb.dbo.CustTable as Cust 
				 INNER JOIN AxDb.dbo.WebShopUserInfo as wui ON wui.CustAccount = Cust.AccountNum AND wui.DataAreaID = @VirtualDataAreaId -- Cust.DataAreaID
			WHERE Cust.StatisticsGroup <> 'Arhív' AND Cust.StatisticsGroup <> 'Archív' AND
				  Cust.DataAreaID = @DataAreaId AND 
				  wui.Pwd = @Password AND 
				  wui.WebLoginName = @UserName; 
			SET @LoginType = 1;
			SET @CanOrder = 1;
			SET @RecieveGoods = 1;
			SET @PriceListDownloadEnabled = 1;
			SET @IsWebAdministrator = 1;
			SET @InvoiceInfoEnabled = 1;
			SELECT ISNULL(@CompanyId, '') as CompanyId,
				   ISNULL(@CompanyName, '') as CompanyName,
				   ISNULL(@PersonId, '') as PersonId,
				   ISNULL(@PersonName, '') as PersonName,
				   ISNULL(@CanOrder, 0) as CanOrder,
				   ISNULL(@RecieveGoods, 0) as RecieveGoods,
				   ISNULL(@PriceListDownloadEnabled, 0) as PriceListDownloadEnabled,
				   ISNULL(@IsWebAdministrator, 0) as IsWebAdministrator,
				   ISNULL(@InvoiceInfoEnabled, 0) as InvoiceInfoEnabled,
				   ISNULL(@LoginType, 0) as LoginType;
			RETURN;
		END

		SELECT ISNULL(@CompanyId, '') as CompanyId,
				ISNULL(@CompanyName, '') as CompanyName,
				ISNULL(@PersonId, '') as PersonId,
				ISNULL(@PersonName, '') as PersonName,
				ISNULL(@CanOrder, 0) as CanOrder,
				ISNULL(@RecieveGoods, 0) as RecieveGoods,
				ISNULL(@PriceListDownloadEnabled, 0) as PriceListDownloadEnabled,
				ISNULL(@IsWebAdministrator, 0) as IsWebAdministrator,
				ISNULL(@InvoiceInfoEnabled, 0) as InvoiceInfoEnabled,
				ISNULL(@LoginType, 0) as LoginType;

	END
	ELSE
	BEGIN
		SELECT ISNULL(@CompanyId, '') as CompanyId,
				ISNULL(@CompanyName, '') as CompanyName,
				ISNULL(@PersonId, '') as PersonId,
				ISNULL(@PersonName, '') as PersonName,
				ISNULL(@CanOrder, 0) as CanOrder,
				ISNULL(@RecieveGoods, 0) as RecieveGoods,
				ISNULL(@PriceListDownloadEnabled, 0) as PriceListDownloadEnabled,
				ISNULL(@IsWebAdministrator, 0) as IsWebAdministrator,
				ISNULL(@InvoiceInfoEnabled, 0) as InvoiceInfoEnabled,
				ISNULL(@LoginType, 0) as LoginType;
	END
RETURN 0

-- GRANT EXECUTE ON InternetUser.SignIn TO InternetUser

-- EXEC [InternetUser].[SignIn] 'elektroplaza', '58915891', 'hrp'