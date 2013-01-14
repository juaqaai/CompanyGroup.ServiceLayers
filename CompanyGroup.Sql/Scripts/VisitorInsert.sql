USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- l�togat� hozzaadas 
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
	LoginMode					INT NOT NULL DEFAULT 0,							-- bejelentkezes modja =1: ceges; =2: szemelyes; =3: alkalmazott
	PartnerModel				INT NOT NULL DEFAULT 0,							-- partner model (None = 0, Hrp = 1, Bsc = 2, Both = 3)
	AutoLogin					BIT NOT NULL DEFAULT 0,							-- automatikus bejelentkezes (session nem jar le soha)
	LoginDate					DATETIME NOT NULL DEFAULT GETDATE(),			-- bejelentkezes idopontja
	LogoutDate					DATETIME NOT NULL DEFAULT CONVERT(DATETIME, 0),	-- kijelentkezes idopontja
	ExpireDate					DATETIME NOT NULL DEFAULT CONVERT(DATETIME, 0), -- lej�rat d�tuma �s ideje 
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
												@PriceGroupId				INT = 0,
												@InventLocationId			NVARCHAR(10) = '', 
												@RepresentativeId			INT = 0,
												@DataAreaId					NVARCHAR(3) = '',				
												@LoginMode					INT = 0,							
												@PartnerModel				INT = 0,							
												@AutoLogin					BIT = 0							
												)			
AS
SET NOCOUNT ON
	DECLARE @VisitorId UNIQUEIDENTIFIER = NEWID();



	INSERT INTO InternetUser.Visitor (VisitorId, LoginIP, RecId,  CustomerId,  CustomerName,  PersonId,  PersonName,  Email,  IsWebAdministrator,  InvoiceInfoEnabled,  PriceListDownloadEnabled,  CanOrder,  RecieveGoods,  PaymTermId,  Currency, LanguageId,   PriceGroupId,  InventLocationId,  RepresentativeId,  DataAreaId,  LoginMode,  PartnerModel,  AutoLogin,  LoginDate, LogoutDate,								ExpireDate,				Valid)
							  VALUES (@VisitorId,@LoginIP, @RecId, @CustomerId, @CustomerName, @PersonId, @PersonName, @Email, @IsWebAdministrator, @InvoiceInfoEnabled, @PriceListDownloadEnabled, @CanOrder, @RecieveGoods, @PaymTermId, @Currency, @LanguageId, @PriceGroupId, @InventLocationId, @RepresentativeId, @DataAreaId, @LoginMode, @PartnerModel, @AutoLogin, GetDate(), DATETIMEFROMPARTS(1900,1, 1, 0, 0, 0, 0), DATEADD(day, 1, GETDATE()), 1);

	SELECT @VisitorId as VisitorId;

RETURN

/*
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
									2,							
									0

select * from InternetUser.Visitor;  
*/