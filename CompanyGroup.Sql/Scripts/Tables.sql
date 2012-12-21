USE [WebDb_Test]
-- =========================================
-- adatbazis tablak letrehozasa
-- =========================================

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- kosar fej
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_ShoppingCart') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_ShoppingCart;
GO
CREATE TABLE InternetUser.cms_ShoppingCart
(
	Id						BIGINT IDENTITY PRIMARY KEY,				-- egyedi azonosito
	VisitorId				BIGINT,										-- login azonosito
	Name					NVARCHAR(32) NOT NULL DEFAULT '',			-- kosar neve
	PaymentTerms			INT NOT NULL DEFAULT 0,						-- fizetesi opciok
	DeliveryTerms			INT NOT NULL DEFAULT 0,						-- szallitasi opciok
	DeliveryDateRequested	DATETIME NOT NULL DEFAULT GETDATE(), 
	DeliveryZipCode			NVARCHAR(4) NOT NULL DEFAULT '',
	DeliveryCity			NVARCHAR(32) NOT NULL DEFAULT '',
	DeliveryCountry			NVARCHAR(32) NOT NULL DEFAULT '',
	DeliveryStreet			NVARCHAR(64) NOT NULL DEFAULT '',
	DeliveryAddrRecId		BIGINT NOT NULL DEFAULT '',
	InvoiceAttached			BIT NOT NULL DEFAULT 0,
	CreatedDate				DATETIME NOT NULL DEFAULT GETDATE(),		-- datum, ido
	[Status]				INT NOT NULL DEFAULT 1
)
GO

-- kosar tetel
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_ShoppingCartLine') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_ShoppingCartLine;
GO
CREATE TABLE InternetUser.cms_ShoppingCartLine
(
	Id				INT IDENTITY  PRIMARY KEY,					-- egyedi azonosito
	CartId			BIGINT,										-- kosar fej azonosito
	ProductId		NVARCHAR(20) NOT NULL DEFAULT '',			-- termek azonosito
	Quantity		INT NOT NULL DEFAULT 1,						-- mennyiseg
	DataAreaId		NVARCHAR(3) NOT NULL DEFAULT '',			-- hrp; bsc; srv
	CreatedDate		DATETIME NOT NULL DEFAULT GETDATE(),		-- datum, ido
	Valid			BIT NOT NULL DEFAULT 1						-- ervenyesseg
)
GO

/*
bejelentkezesek naplozasa
select * from InternetUser.cms_Visitor
*/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_Visitor') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_Visitor;
GO
CREATE TABLE InternetUser.cms_Visitor
(
	Id							BIGINT IDENTITY PRIMARY KEY,					-- egyedi azonosito
	ObjectId					UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	LoginIP						NVARCHAR (32) NOT NULL DEFAULT '' ,				-- ip cim
	RecId						BIGINT NOT NULL DEFAULT 0 ,						-- WebShopUserInfo.RecId bigint mezo (idegen kulcs)
	CustomerId					NVARCHAR(32) NOT NULL DEFAULT '', 
	PersonId					NVARCHAR(32) NOT NULL DEFAULT '', 
	Email						NVARCHAR(64) NOT NULL DEFAULT '',				-- email
	IsWebAdministrator			BIT NOT NULL DEFAULT 1, 
	InvoiceInfoEnabled			BIT NOT NULL DEFAULT 1, 
	PriceListDownloadEnabled	BIT NOT NULL DEFAULT 1, 
	CanOrder					BIT NOT NULL DEFAULT 1, 
	RecieveGoods				BIT NOT NULL DEFAULT 1,
	PaymTermId					NVARCHAR(10) NOT NULL DEFAULT '', 
	Currency					NVARCHAR(10) NOT NULL		DEFAULT '', 
	LanguageId					NVARCHAR(10) NOT NULL DEFAULT '', 
	PriceGroup					INT NOT NULL DEFAULT 0,
	CustomerPriceGroupId		INT NOT NULL DEFAULT 0,
	RepresentativeId			INT NOT NULL DEFAULT 0,
	DataAreaId					NVARCHAR(3) NOT NULL DEFAULT '',				-- vallalatkod hrp; bsc; srv
	LoginMode					INT NOT NULL DEFAULT 0,							-- bejelentkezes modja =1: ceges; =2: szemelyes; =3: alkalmazott
	AutoLogin					BIT NOT NULL DEFAULT 0,							-- automatikus bejelentkezes (session nem jar le soha)
	LoginDate					DATETIME NOT NULL DEFAULT GETDATE(),			-- bejelentkezes idopontja
	LogoutDate					DATETIME NOT NULL DEFAULT CONVERT(DATETIME, 0),	-- kijelentkezes idopontja
	Valid						BIT NOT NULL DEFAULT 1
)
GO

-- árcsoportok
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_CustomerPriceGroup') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_CustomerPriceGroup;
GO
CREATE TABLE InternetUser.cms_CustomerPriceGroup
(
	Id INT IDENTITY PRIMARY KEY,
	ManufacturerId NVARCHAR(4) NOT NULL DEFAULT '',
	Category1Id NVARCHAR(4) NOT NULL DEFAULT '', 
	Category2Id NVARCHAR(4) NOT NULL DEFAULT '', 
	Category3Id NVARCHAR(4) NOT NULL DEFAULT '',
	PriceGroupId NVARCHAR(4) NOT NULL DEFAULT '',
	[Order] INT NOT NULL DEFAULT 1
)
GO
-- 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_Representative') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_Representative;
GO
CREATE TABLE InternetUser.cms_Representative
(
	Id INT IDENTITY PRIMARY KEY,		
	RepresentativeId NVARCHAR(10) NOT NULL DEFAULT '',
	Name NVARCHAR(64) NOT NULL DEFAULT '', 
	Phone NVARCHAR(64) NOT NULL DEFAULT '',
	Mobile NVARCHAR(20) NOT NULL DEFAULT '', 
	Email NVARCHAR(64) NOT NULL DEFAULT '' 
)
GO

-- termekkatalogus tabla
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_Catalogue') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_Catalogue;
GO
CREATE TABLE InternetUser.cms_Catalogue
(
	Id INT IDENTITY NOT NULL,				-- termékjellemzõ lap azonosító 
	ProductId nvarchar(20) not null default '',			-- axapta inventory termekazonosito ITEMID
	AxStructCode nvarchar(16) not null default '', 
	DataAreaId nvarchar(3) not null default '',			-- hrp / bsc / srv
	Name nvarchar(80) not null default '',					-- axapta inventory itemname
	PartNumber nvarchar(20) not null default '',			-- axapta gyartoicikkszamok gyartoicikkszam dbo.GYARTOICIKKSZAMOK.GYARTOICIKKSZAM
	ManufacturerId nvarchar(4) not null default '',		-- axapta updStruktura gyarto id
	ManufacturerName nvarchar(64) not null default '',		-- axapta updStruktura gyarto nev dbo.UPDSTRUKTURA.GYARTO
	sCategory1ID nvarchar(4) not null default '',			-- axapta updStruktura jelleg1 id	
	sCategory1Name nvarchar(64) not null default '', 		-- axapta updStruktura jelleg1 nev dbo.UPDSTRUKTURA.JELLEG1
	sCategory2ID nvarchar(4) not null default '',			-- axapta updStruktura jelleg2 id
	sCategory2Name nvarchar(64) not null default '', 		-- axapta updStruktura jelleg2 nev dbo.UPDSTRUKTURA.JELLEG2
	sCategory3ID nvarchar(4) not null default '',			-- axapta updStruktura jelleg3 id
	sCategory3Name nvarchar(64) not null default '', 		-- axapta updStruktura jelleg3 nev dbo.UPDSTRUKTURA.JELLEG3
	iInnerStock int not null default 0,
	iOuterStock int not null default 0,
	iPrice1 int not null default 0,							-- axapta inventory amount1
	iPrice2 int not null default 0,							-- axapta inventory amount2
	iPrice3 int not null default 0,							-- axapta inventory amount3
	iPrice4 int not null default 0,							-- axapta inventory amount4
	iPrice5 int not null default 0,							-- axapta inventory amount5
	iPrice6 int not null default 0,							-- axapta inventory amount6
	sGaranty nvarchar(32) not null default '',				-- axapta inventory garancia
	sGarantyMode nvarchar(32) not null default '',			-- axapta inventory garancia
	bAction bit not null default 0,							-- axapta inventory Akcios
	bBargain bit not null default 0,						-- axapta inventory Leertekelt
	bTop10 bit not null default 0,							-- axapta inventory HetiTop10
	bFocusWeek bit not null default 0,						-- axapta inventory Fokuszhet
	bNew bit not null default 0,
	iItemState int not null default 0,						-- axapta ItemState mezo 0 : aktiv, 1 : kifuto, 2 : passziv	
	sDescription nvarchar(1024) not null default '',		-- axapta inventtxt txt dbo.INVENTTXT.TXT
	bPictureExist bit not null default 0,
	sEmployeeID nvarchar(16) not null default '',			-- termekhez kapcsolt termekmanager
	dtCreated SmallDateTime not null default GetDate(),		-- amikor a rekord letrejott
	dtStockUpdated SmallDateTime not null default GetDate(),-- amikor a keszlet mennyiseg frissitesre kerult
	dtPriceUpdated SmallDateTime not null default GetDate(),-- amikor a hat ar frissitesre kerult
	bValid bit not null default 1
)
GO

-- termekkatalogus tabla
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_CatalogueRequestLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_CatalogueRequestLog;
GO
CREATE TABLE InternetUser.cms_CatalogueRequestLog
(
	Id				 BIGINT IDENTITY NOT NULL PRIMARY KEY,
	VisitorId		 INT NOT NULL DEFAULT 0,					-- belépés azonosító 
	DataAreaId		 NVARCHAR(4) NOT NULL DEFAULT '',			
	ManufacturerId	 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category1Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category2Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Category3Id		 NVARCHAR(4) NOT NULL DEFAULT '',		
	Action			 BIT NOT NULL DEFAULT 0,						
	Bargain			 BIT NOT NULL DEFAULT 0,						
	New				 BIT NOT NULL DEFAULT 0,
	IsInStock		 BIT NOT NULL DEFAULT 0,
	FindText		 NVARCHAR(64) NOT NULL DEFAULT '', 
	CurrentPageIndex INT NOT NULL DEFAULT 0, 
	ItemsOnPage		 INT NOT NULL DEFAULT 0, 
	Sequence		 INT NOT NULL DEFAULT 0
)

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'InternetUser.cms_CatalogueDetailsLog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE InternetUser.cms_CatalogueDetailsLog;
GO
CREATE TABLE InternetUser.cms_CatalogueDetailsLog
(
	Id			BIGINT IDENTITY NOT NULL PRIMARY KEY,
	VisitorId	INT NOT NULL DEFAULT 0,					-- belépés azonosító 
	DataAreaId	NVARCHAR(4) NOT NULL DEFAULT '',		-- vállalat azonosító
	ProductId	NVARCHAR(20) NOT NULL DEFAULT '',			
	CreatedDate	DATETIME NOT NULL DEFAULT GETDATE()		
)