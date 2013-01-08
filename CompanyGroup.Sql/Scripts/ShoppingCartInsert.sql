USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár fej hozzaadas 
DROP PROCEDURE [InternetUser].[InsertShoppingCart];
GO
CREATE PROCEDURE [InternetUser].[InsertShoppingCart](@VisitorId	NVARCHAR(64) = '',			-- login azonosito, CompanyId, PersonId
													 @Name NVARCHAR(32) = '',				-- kosar neve
													 @PaymentTerms INT = 0,					-- fizetesi opciok: KP, ÁTUT, Elõre ut., Utánvét (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
													 @DeliveryTerms INT = 0,				-- szallitasi opciok: szállítás, vagy raktárból (Delivery = 1, Warehouse = 2)
													 @DeliveryDateRequested	DATETIME,		-- szállítási dátum, ha 1900.01.01, akkor nem állítottak be kiszállítási dátumot
													 @DeliveryZipCode NVARCHAR(4) = '',
													 @DeliveryCity NVARCHAR(32) = '',
													 @DeliveryCountry NVARCHAR(32) = '',
													 @DeliveryStreet NVARCHAR(64) = '',
													 @DeliveryAddrRecId	BIGINT = 0,
													 @InvoiceAttached BIT = 0,				-- lett-e számla csatolva
													 @Active BIT = 0,						-- aktív-e a kosár (egyszerre csak egy kosár lehet aktív)
													 @Currency NVARCHAR(10) = '',			-- rendelés feladáshoz tartozó pénznem 
													 @CartId INT = -1 OUT)			
AS
SET NOCOUNT ON
	INSERT INTO InternetUser.ShoppingCart (VisitorId, Name, PaymentTerms, 
										   DeliveryTerms, DeliveryDateRequested, DeliveryZipCode, DeliveryCity, DeliveryCountry, DeliveryStreet, DeliveryAddrRecId, 
										   InvoiceAttached, Active, Currency, CreatedDate, Status) VALUES (@VisitorId, @Name, @PaymentTerms, 
										   @DeliveryTerms, @DeliveryDateRequested, @DeliveryZipCode, @DeliveryCity, @DeliveryCountry, @DeliveryStreet, @DeliveryAddrRecId, 
										   @InvoiceAttached, @Active, @Currency, GetDate(), 1);
	SET @CartId = @@IDENTITY;

RETURN

-- select * from InternetUser.ShoppingCart