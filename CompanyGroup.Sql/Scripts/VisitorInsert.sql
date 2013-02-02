USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- látogató hozzaadas 
/*
	Id							INT IDENTITY PRIMARY KEY,					-- egyedi GUID azonosito
	VisitorId					UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
	LoginIP						NVARCHAR (32) NOT NULL DEFAULT '' ,				-- ip cim
	RecId						BIGINT NOT NULL DEFAULT 0 ,						-- WebShopUserInfo.RecId bigint mezo (idegen kulcs)
	CustomerId					NVARCHAR(20) NOT NULL DEFAULT '', 
	CustomerName				NVARCHAR(100) NOT NULL DEFAULT '',
	PersonId					NVARCHAR(32) NOT NULL DEFAULT '', 
	PersonName					NVARCHAR(100) NOT NULL DEFAULT '',
	Email						NVARCHAR(100) NOT NULL DEFAULT '',				-- email
	IsWebAdministrator			BIT NOT NULL DEFAULT 1, 
	InvoiceInfoEnabled			BIT NOT NULL DEFAULT 1, 
	PriceListDownloadEnabled	BIT NOT NULL DEFAULT 1, 
	CanOrder					BIT NOT NULL DEFAULT 1, 
	RecieveGoods				BIT NOT NULL DEFAULT 1,
	PaymTermId					NVARCHAR(10) NOT NULL DEFAULT '', 
	Currency					NVARCHAR(10) NOT NULL		DEFAULT '', 
	LanguageId					NVARCHAR(10) NOT NULL DEFAULT '', 
	PriceGroupId				INT NOT NULL DEFAULT 0,
	InventLocationId			NVARCHAR(10) NOT NULL DEFAULT '', 
	RepresentativeId			INT NOT NULL DEFAULT 0,
	DataAreaId					NVARCHAR(3) NOT NULL DEFAULT '',				-- vallalatkod hrp; bsc; srv
	LoginType					INT NOT NULL DEFAULT 0,							-- bejelentkezes modja =1: ceges; =2: szemelyes; =3: alkalmazott
	PartnerModel				INT NOT NULL DEFAULT 0,							-- partner model (None = 0, Hrp = 1, Bsc = 2, Both = 3)
	AutoLogin					BIT NOT NULL DEFAULT 0,							-- automatikus bejelentkezes (session nem jar le soha)
	LoginDate					DATETIME NOT NULL DEFAULT GETDATE(),			-- bejelentkezes idopontja
	LogoutDate					DATETIME NOT NULL DEFAULT CONVERT(DATETIME, 0),	-- kijelentkezes idopontja
	ExpireDate					DATETIME NOT NULL DEFAULT CONVERT(DATETIME, 0), -- lejárat dátuma és ideje 
	Valid						BIT NOT NULL DEFAULT 1

*/
DROP PROCEDURE [InternetUser].[VisitorInsert];
GO
CREATE PROCEDURE [InternetUser].[VisitorInsert](@LoginIP	NVARCHAR(32) = '',			
												@RecId BIGINT = 0, 
												@CustomerId NVARCHAR(20) = '',
												@CustomerName NVARCHAR(100) = '',
												@PersonId NVARCHAR(32) = '',
												@PersonName NVARCHAR(100) = '',
												@Email						NVARCHAR(100) = '',	
												@IsWebAdministrator			BIT = 1, 
												@InvoiceInfoEnabled			BIT = 1, 
												@PriceListDownloadEnabled	BIT = 1, 
												@CanOrder					BIT = 1, 
												@RecieveGoods				BIT = 1,
												@PaymTermId					NVARCHAR(10) = '', 
												@Currency					NVARCHAR(10) = '', 
												@LanguageId					NVARCHAR(10) = '', 
												@DefaultPriceGroupId		NVARCHAR(4) = '',
												@InventLocationId			NVARCHAR(10) = '', 
												@RepresentativeId			NVARCHAR(10) = '',
												@DataAreaId					NVARCHAR(3) = '',				
												@LoginType					INT = 0,							
												@PartnerModel				INT = 0,							
												@AutoLogin					BIT = 0, 
												@LoginDate					DateTime, 
												@ExpireDate					DateTime	
												)			
AS
SET NOCOUNT ON
	DECLARE @VisitorId NVARCHAR(64) = REPLACE(CONVERT(NVARCHAR(64), NEWID()), '-', ''),
	@LogoutDate DATETIME = CONVERT(DATETIME, 0);

	DECLARE @ReprId INT = ISNULL((SELECT Id FROM InternetUser.Representative WHERE RepresentativeId = @RepresentativeId), 0);

	INSERT INTO InternetUser.Visitor (VisitorId, LoginIP, RecId, CustomerId, CustomerName, PersonId, PersonName, Email,  
									  IsWebAdministrator,  InvoiceInfoEnabled,  PriceListDownloadEnabled, CanOrder, RecieveGoods,  
									  PaymTermId, Currency, LanguageId, DefaultPriceGroupId, InventLocationId, RepresentativeId,  
									  DataAreaId, LoginType, PartnerModel, AutoLogin, LoginDate, LogoutDate, ExpireDate, Valid)
							  VALUES (@VisitorId,@LoginIP, @RecId, @CustomerId, @CustomerName, @PersonId, @PersonName, @Email, 
									  @IsWebAdministrator, @InvoiceInfoEnabled, @PriceListDownloadEnabled, @CanOrder, @RecieveGoods, 
									  @PaymTermId, @Currency, @LanguageId, @DefaultPriceGroupId, @InventLocationId, @ReprId, 
									  @DataAreaId, @LoginType, @PartnerModel, @AutoLogin, @LoginDate, @LogoutDate, @ExpireDate, 1);

	SELECT @@Identity as Id, @VisitorId as VisitorId;

RETURN
GO
GRANT EXECUTE ON InternetUser.VisitorInsert TO InternetUser
GO
/*
DECLARE @Date1 DateTime = GetDate(); 
DECLARE	@Date2 DateTime = DATEADD(day, 1, GetDate());
EXEC [InternetUser].[VisitorInsert] '127.0.0.1',			
									0, 
									'CustomerId',
									'CustomerName',
									'PersonId',
									'PersonName',
									'Email',	
									1, 
									1, 
									1, 
									1, 
									1,
									'PaymTermId', 
									'Currency', 
									'LanguageId', 
									0,
									'InventLocationId', 
									0,
									'hrp',				
									1,							
									'2',							
									0, 
									@Date1, 
									@Date2;

select * from InternetUser.Visitor;  

update InternetUser.Visitor set visitorId = 'alma' + LOWER(REPLACE(CONVERT(NVARCHAR(64), NEWID()), '-', ''))

*/