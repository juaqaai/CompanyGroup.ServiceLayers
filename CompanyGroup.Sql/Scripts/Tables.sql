/* =============================================
	description	   : srv2 Web adatbázisban táblák létrehozása
	running script : 
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.01.27.
	modified date  :
	modify reason  :
 ============================================= */

USE [Web]
-- =========================================
-- adatbazis tablak letrehozasa
-- =========================================

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- kosar fej
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.ShoppingCart') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.ShoppingCart;
GO
CREATE TABLE InternetUser.ShoppingCart
(
	Id						INT IDENTITY PRIMARY KEY,						-- egyedi azonosito
	VisitorId				NVARCHAR(64) NOT NULL DEFAULT '',				-- login azonosito, CompanyId, PersonId
	Name					NVARCHAR(32) NOT NULL DEFAULT '',				-- kosar neve
	PaymentTerms			INT NOT NULL DEFAULT 0,							-- fizetesi opciok: KP, ÁTUT, Elõre ut., Utánvét (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
	DeliveryTerms			INT NOT NULL DEFAULT 0,							-- szallitasi opciok: szállítás, vagy raktárból (Delivery = 1, Warehouse = 2)
	DeliveryDateRequested	DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0), -- szállítási dátum, ha 1900.01.01, akkor nem állítottak be kiszállítási dátumot
	DeliveryZipCode			NVARCHAR(4) NOT NULL DEFAULT '',
	DeliveryCity			NVARCHAR(32) NOT NULL DEFAULT '',
	DeliveryCountry			NVARCHAR(32) NOT NULL DEFAULT '',
	DeliveryStreet			NVARCHAR(64) NOT NULL DEFAULT '',
	DeliveryAddrRecId		BIGINT NOT NULL DEFAULT '',
	InvoiceAttached			BIT NOT NULL DEFAULT 0,							-- lett-e számla csatolva
	Active					BIT NOT NULL DEFAULT 0,							-- aktív-e a kosár (egyszerre csak egy kosár lehet aktív)
	Currency				NVARCHAR(10) NOT NULL DEFAULT '',				-- rendelés feladáshoz tartozó pénznem
	CreatedDate				DATETIME NOT NULL DEFAULT GETDATE(),			-- datum, ido
	[Status]				INT NOT NULL DEFAULT 1							-- kosár státusz (Deleted = 0, Created = 1, Stored = 2, Posted = 3, WaitingForAutoPost = 4)
)
GO

-- ajánlat fej
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.FinanceOffer') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.FinanceOffer;
GO
CREATE TABLE InternetUser.FinanceOffer
(
	Id						INT IDENTITY PRIMARY KEY,						-- egyedi azonosito
	VisitorId				NVARCHAR(64) NOT NULL DEFAULT '',				-- login azonosito, CompanyId, PersonId
	LeasingPersonName		NVARCHAR(250) NOT NULL DEFAULT '',				-- bérbevevõ név
	LeasingAddress			NVARCHAR(250) NOT NULL DEFAULT '',				-- bérbevevõ cím
	LeasingPhone			NVARCHAR(100) NOT NULL DEFAULT '',				-- bérbevevõ telefonszáma
	LeasingStatNumber		NVARCHAR(100) NOT NULL DEFAULT '',				-- bérbevevõ cégjegyzékszáma
	NumOfMonth				INT NOT NULL DEFAULT 1,							-- leasing konstrukció
	Currency				NVARCHAR(10) NOT NULL DEFAULT '',				-- rendelés feladáshoz tartozó pénznem
	CreatedDate				DATETIME NOT NULL DEFAULT GETDATE(),			-- datum, ido
	[Status]				INT NOT NULL DEFAULT 1							-- kosár státusz (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
)
GO

-- kosar tetel
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.ShoppingCartLine') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.ShoppingCartLine;
GO
CREATE TABLE InternetUser.ShoppingCartLine
(
	Id				INT IDENTITY  PRIMARY KEY,					-- egyedi azonosito
	CartId			INT NOT NULL DEFAULT 0,						-- kosar fej azonosito
	ProductId		NVARCHAR(20) NOT NULL DEFAULT '',			-- termek azonosito	(ProductName, ProductNameEnglish, PartNumber, ConfigId, CustomerPrice, ItemState - cikk státusza (aktív, passzív, kifutó), )
	Quantity		INT NOT NULL DEFAULT 1,						-- mennyiseg
	Price			INT NOT NULL DEFAULT 1,						-- ár
	DataAreaId		NVARCHAR(3) NOT NULL DEFAULT '',			-- hrp; bsc; ahonnan a termék származik
	ConfigId		NVARCHAR(20) NOT NULL DEFAULT '',			-- ALAP, vagy XX
	InventLocationId NVARCHAR(20) NOT NULL DEFAULT '',			-- KULSO, vagy HASZNALT
	[Status]		INT  NOT NULL DEFAULT 0, 					-- kosár elem státusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
	CreatedDate		DATETIME NOT NULL DEFAULT GETDATE(),		-- datum, ido
)
GO

-- Finance ajánlatkérés
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.FinanceOffer') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.FinanceOffer;
GO
CREATE TABLE InternetUser.FinanceOffer
(
	Id				INT IDENTITY  PRIMARY KEY,					-- egyedi azonosito
	CartId			INT NOT NULL DEFAULT 0,						-- kosar fej azonosito
    PersonName		NVARCHAR(100) NOT NULL DEFAULT '',			-- bérbevevõ név
    [Address]		NVARCHAR(100) NOT NULL DEFAULT '',			-- bérbevevõ cím
    Phone			NVARCHAR(50) NOT NULL DEFAULT '',			-- bérbevevõ telefon
    StatNumber		NVARCHAR(50) NOT NULL DEFAULT '',			-- bérbevevõ cégjegyzékszám
    NumOfMonth		Int NOT NULL DEFAULT 0,						-- futamidõ
	CreatedDate		DATETIME NOT NULL DEFAULT GETDATE(),		-- datum, ido
)
GO

/*
bejelentkezesek naplozasa
select * from InternetUser.Visitor
*/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.Visitor') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.Visitor;
GO
CREATE TABLE InternetUser.Visitor
(
	Id							INT IDENTITY PRIMARY KEY,					-- egyedi GUID azonosito
	VisitorId					NVARCHAR(64) NOT NULL DEFAULT CONVERT(NVARCHAR(64), NEWID()), 
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
	DefaultPriceGroupId			NVARCHAR(4) NOT NULL DEFAULT '',
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
)
GO


-- árcsoportok
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.CustomerPriceGroup') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.CustomerPriceGroup;
GO
CREATE TABLE InternetUser.CustomerPriceGroup
(
	Id				INT IDENTITY PRIMARY KEY,
	VisitorId		INT NOT NULL DEFAULT 0,		
	ManufacturerId  NVARCHAR(4) NOT NULL DEFAULT '',
	Category1Id		NVARCHAR(4) NOT NULL DEFAULT '', 
	Category2Id		NVARCHAR(4) NOT NULL DEFAULT '', 
	Category3Id		NVARCHAR(4) NOT NULL DEFAULT '',
	PriceGroupId	NVARCHAR(4) NOT NULL DEFAULT '',
	[Order]			INT NOT NULL DEFAULT 1
)
GO
-- 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.Representative') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.Representative;
GO
CREATE TABLE InternetUser.Representative
(
	Id INT IDENTITY PRIMARY KEY,		
	RepresentativeId NVARCHAR(10) NOT NULL DEFAULT '',
	Name NVARCHAR(80) NOT NULL DEFAULT '', 
	Phone NVARCHAR(64) NOT NULL DEFAULT '',
	Mobile NVARCHAR(20) NOT NULL DEFAULT '', 
	Email NVARCHAR(80) NOT NULL DEFAULT '', 
	Extension NVARCHAR(20) NOT NULL DEFAULT ''
)
GO

-- termekkatalogus tabla
/*
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.Catalogue') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.Catalogue;
GO
CREATE TABLE InternetUser.Catalogue
(
	Id						int identity not null,				
	ProductId				nvarchar(20) not null default '',			
	AxStructCode			nvarchar(16) not null default '', 
	DataAreaId				nvarchar(3) not null default '',		-- hrp / bsc / srv
	StandardConfigId		nvarchar(20) not null default '', 
	Name					nvarchar(80) not null default '',					
	EnglishName				nvarchar(80) not null default '',
	PartNumber				nvarchar(20) not null default '',		-- dbo.GYARTOICIKKSZAMOK.GYARTOICIKKSZAM
	ManufacturerId			nvarchar(4) not null default '',		-- updStruktura gyarto id
	ManufacturerName		nvarchar(80) not null default '',		-- updStruktura gyarto nev 
	ManufacturerEnglishName nvarchar(80) not null default '',		-- updStruktura gyarto nev 
	Category1Id				nvarchar(4) not null default '',		-- updStruktura jelleg1 id	
	Category1Name			nvarchar(80) not null default '', 		-- updStruktura jelleg1 nev 
	Category1EnglishName	nvarchar(80) not null default '', 		-- updStruktura jelleg1 nev 
	Category2Id				nvarchar(4) not null default '',		-- updStruktura jelleg2 id
	Category2Name			nvarchar(80) not null default '', 		-- updStruktura jelleg2 nev 
	Category2EnglishName	nvarchar(80) not null default '', 		-- updStruktura jelleg1 nev 
	Category3Id				nvarchar(4) not null default '',		-- updStruktura jelleg3 id
	Category3Name			nvarchar(80) not null default '', 		-- updStruktura jelleg3 nev 
	Category3EnglishName	nvarchar(80) not null default '', 		-- updStruktura jelleg1 nev 
	InnerStock				int not null default 0,
	OuterStock				int not null default 0,
	AverageInventory		int not null default 0,
	Price1					int not null default 0,					-- axapta inventory amount1
	Price2					int not null default 0,					-- axapta inventory amount2
	Price3					int not null default 0,					-- axapta inventory amount3
	Price4					int not null default 0,					-- axapta inventory amount4
	Price5					int not null default 0,					-- axapta inventory amount5
	Garanty					nvarchar(32) not null default '',		-- axapta inventory garancia
	GarantyMode				nvarchar(32) not null default '',		-- axapta inventory garancia
	Discount				bit not null default 0,					-- axapta inventory Akcios
	New						bit not null default 0,
	ItemState				int not null default 0,					-- axapta ItemState mezo 0 : aktiv, 1 : kifuto, 2 : passziv	
	[Description]			nvarchar(max) not null default '',		-- axapta inventtxt txt dbo.INVENTTXT.TXT
	EnglishDescription		nvarchar(max) not null default '',
	ProductManagerId		int not null default 0,					-- termekhez kapcsolt termekmanager
	ShippingDate			SmallDateTime not null default GetDate(),
	CreatedDate				SmallDateTime not null default GetDate(),		
	Updated					SmallDateTime not null default GetDate(),
	Available				bit not null default 1,					-- kereskedelmi forgalomban elerheto-e a termek
	PictureId				int not null default 0,
	SecondHand				bit not null default 0,					-- elerheto-e a termek hasznalt konfig-on,
	Valid					bit not null default 1, 
    [ExtractDate] datetime2 NOT NULL DEFAULT GetDate(), 		
    [PackageLogKey]			int not null default 0
)
GO
*/
-- termekkatalogus log tabla
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.CatalogueRequestLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.CatalogueRequestLog;
GO
CREATE TABLE InternetUser.CatalogueRequestLog
(
	Id				 BIGINT IDENTITY NOT NULL PRIMARY KEY,
	VisitorId		 INT NOT NULL DEFAULT 0,					-- belépés azonosító 
	DataAreaId		 NVARCHAR(4) NOT NULL DEFAULT '',			
	ManufacturerId	 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category1Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category2Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category3Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Discount		 BIT NOT NULL DEFAULT 0,						
	Bargain			 BIT NOT NULL DEFAULT 0,						
	New				 BIT NOT NULL DEFAULT 0,
	IsInStock		 BIT NOT NULL DEFAULT 0,
	IsInSecondHand	 BIT NOT NULL DEFAULT 0,
	IsInNewsletter	 BIT NOT NULL DEFAULT 0,
	HrpFilter		 BIT NOT NULL DEFAULT 0,
	BscFilter		 BIT NOT NULL DEFAULT 0,
	PriceFilter		 INT NOT NULL DEFAULT 0, 
	PriceFilterRelation INT NOT NULL DEFAULT 0, 
	FindText		 NVARCHAR(64) NOT NULL DEFAULT '', 
	CurrentPageIndex INT NOT NULL DEFAULT 0, 
	ItemsOnPage		 INT NOT NULL DEFAULT 0, 
	Sequence		 INT NOT NULL DEFAULT 0, 
	Currency		 NVARCHAR(10) NOT NULL DEFAULT '',
	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE()	
)
GO

-- árlista letöltés log tabla
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.PriceListRequestLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.PriceListRequestLog;
GO
CREATE TABLE InternetUser.PriceListRequestLog
(
	Id				 BIGINT IDENTITY NOT NULL PRIMARY KEY,
	VisitorId		 INT NOT NULL DEFAULT 0,					-- belépés azonosító 
	DataAreaId		 NVARCHAR(4) NOT NULL DEFAULT '',			
	ManufacturerId	 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category1Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category2Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category3Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Discount		 BIT NOT NULL DEFAULT 0,						
	Bargain			 BIT NOT NULL DEFAULT 0,						
	New				 BIT NOT NULL DEFAULT 0,
	IsInStock		 BIT NOT NULL DEFAULT 0,
	IsInSecondHand	 BIT NOT NULL DEFAULT 0,
	IsInNewsletter	 BIT NOT NULL DEFAULT 0,
	HrpFilter		 BIT NOT NULL DEFAULT 0,
	BscFilter		 BIT NOT NULL DEFAULT 0,
	PriceFilter		 INT NOT NULL DEFAULT 0, 
	PriceFilterRelation INT NOT NULL DEFAULT 0, 
	FindText		 NVARCHAR(64) NOT NULL DEFAULT '', 
	CurrentPageIndex INT NOT NULL DEFAULT 0, 
	ItemsOnPage		 INT NOT NULL DEFAULT 0, 
	Sequence		 INT NOT NULL DEFAULT 0, 
	Currency		 NVARCHAR(10) NOT NULL DEFAULT '',
	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE()	
)
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.CatalogueDetailsLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.CatalogueDetailsLog;
GO
CREATE TABLE InternetUser.CatalogueDetailsLog
(
	Id			BIGINT IDENTITY NOT NULL PRIMARY KEY,
	VisitorId	NVARCHAR(64) NOT NULL DEFAULT '',  
	CustomerId	NVARCHAR(20) NOT NULL DEFAULT '',
	PersonId	NVARCHAR(20) NOT NULL DEFAULT '',
	DataAreaId	NVARCHAR(4) NOT NULL DEFAULT '',		-- vállalat azonosító
	ProductId	NVARCHAR(20) NOT NULL DEFAULT '',			
	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE()		
)

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.SecondHand') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.SecondHand;
GO
CREATE TABLE InternetUser.SecondHand
(
	Id					INT IDENTITY NOT NULL PRIMARY KEY,
	DataAreaId			NVARCHAR(4) NOT NULL DEFAULT '',		-- vállalat azonosító
	ProductId			NVARCHAR(20) NOT NULL DEFAULT '',		
	ConfigId			NVARCHAR(20) NOT NULL DEFAULT '',	
	InventLocationId	NVARCHAR(20) NOT NULL DEFAULT '',
	Quantity			INT NOT NULL DEFAULT 0, 
	Price				INT NOT NULL DEFAULT 0,
	StatusDescription 	NVARCHAR(1024) NOT NULL DEFAULT '',			
	CreatedDate			DATETIME NOT NULL DEFAULT GETDATE(), 
	Valid				BIT NOT NULL DEFAULT 1	
)


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.Picture') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.Picture;
GO
CREATE TABLE InternetUser.Picture
(
	Id					INT IDENTITY NOT NULL PRIMARY KEY,
	RecId				BIGINT NOT NULL DEFAULT 0,					-- rekord azonosító 
	ProductId			NVARCHAR(20) NOT NULL DEFAULT '',		
	[FileName]			NVARCHAR(300) NOT NULL DEFAULT '',	 
	[Primary]			BIT NOT NULL DEFAULT 0, 				
	CreatedDate			DATETIME NOT NULL DEFAULT GETDATE(), 
	Valid				BIT NOT NULL DEFAULT 1, 
    [ExtractDate] datetime2 NOT NULL DEFAULT GetDate(), 		
    [PackageLogKey]			int not null default 0
)

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.Compatibility') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.Compatibility;
GO
CREATE TABLE InternetUser.Compatibility
(
	Id					INT IDENTITY NOT NULL PRIMARY KEY,
	ProductId			NVARCHAR(20) NOT NULL DEFAULT '',
	CompatibleProductId	NVARCHAR(20) NOT NULL DEFAULT '',
	CompatibilityType	INT NOT NULL DEFAULT 0, 
	DataAreaId			NVARCHAR(3) NOT NULL DEFAULT '',
    [ExtractDate]		DATETIME2 NOT NULL DEFAULT GetDate(), 		
    [PackageLogKey]		INT NOT NULL DEFAULT 0
)

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.Invoice') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.Invoice;
GO
CREATE TABLE InternetUser.Invoice
(
	Id					INT IDENTITY NOT NULL PRIMARY KEY,
	CustomerId			NVARCHAR(20) NOT NULL DEFAULT '',
	DataAreaId			NVARCHAR(3) NOT NULL DEFAULT '',
	SalesId				NVARCHAR(20) NOT NULL DEFAULT '',
	InvoiceDate			DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0), 	
	DueDate				DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0), 
	InvoiceAmount		decimal(20,2) NOT NULL DEFAULT 0, 
	InvoiceCredit		decimal(20,2) NOT NULL DEFAULT 0,
	CurrencyCode		NVARCHAR(20) NOT NULL DEFAULT '',
	InvoiceId			NVARCHAR(20) NOT NULL DEFAULT '', 
	Payment				NVARCHAR(60) NOT NULL DEFAULT '', 
	SalesType			INT NOT NULL DEFAULT 0,				-- sor tipusa, 0 napló, 1 árajánlat, 2 elõfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet
	CusomerRef			NVARCHAR(300) NOT NULL DEFAULT '',
	InvoicingName		NVARCHAR(80) NOT NULL DEFAULT '',
	InvoicingAddress	NVARCHAR(250) NOT NULL DEFAULT '',
	ContactPersonId		NVARCHAR(20) NOT NULL DEFAULT '',
	Printed				INT NOT NULL DEFAULT 0,	
	ReturnItemId		NVARCHAR(20) NOT NULL DEFAULT '',
	ItemDate			DATETIME NOT NULL DEFAULT CONVERT(DateTime, 0),
	LineNum				INT NOT NULL DEFAULT 0,
	ItemId				NVARCHAR(20) NOT NULL DEFAULT '',
	ItemName			NVARCHAR(300) NOT NULL DEFAULT '',
	Quantity			INT NOT NULL DEFAULT 0,
	SalesPrice			decimal(20,2) NOT NULL DEFAULT 0, 
	LineAmount			decimal(20,2) NOT NULL DEFAULT 0, 
	QuantityPhysical	INT NOT NULL DEFAULT 0, 
	Remain				INT NOT NULL DEFAULT 0,
	DeliveryType		INT NOT NULL DEFAULT 0,	
	TaxAmount			decimal(20,2) NOT NULL DEFAULT 0,
	LineAmountMst		decimal(20,2) NOT NULL DEFAULT 0,
	TaxAmountMst		decimal(20,2) NOT NULL DEFAULT 0,
	DetailCurrencyCode  NVARCHAR(20) NOT NULL DEFAULT '',
	Debit				BIT NOT NULL DEFAULT 0, 
    [ExtractDate]		DATETIME2 NOT NULL DEFAULT GetDate(), 		
    [PackageLogKey]		INT NOT NULL DEFAULT 0
)