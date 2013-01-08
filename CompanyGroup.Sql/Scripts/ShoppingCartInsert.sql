USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár fej hozzaadas 
DROP PROCEDURE [InternetUser].[ShoppingCartInsert];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartInsert](@VisitorId	NVARCHAR(64) = '',			-- login azonosito, CompanyId, PersonId
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

/*
DECLARE @CartId INT, @date DateTime = GetDate();
EXEC [InternetUser].[ShoppingCartInsert] 'teszt', 
										 'Shopping cart 1',	
										  1,					-- fizetesi opciok: KP, ÁTUT, Elõre ut., Utánvét (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
										  2,				-- szallitasi opciok: szállítás, vagy raktárból (Delivery = 1, Warehouse = 2)
										  @date,		-- szállítási dátum, ha 1900.01.01, akkor nem állítottak be kiszállítási dátumot
										  '1222',
										  'Budapest',
										  'Hungary',
										  'Wesselényi u. 10.',
										  0,
										  0,				-- lett-e számla csatolva
										  1,						-- aktív-e a kosár (egyszerre csak egy kosár lehet aktív)
										  'HUF',			-- rendelés feladáshoz tartozó pénznem 
										  @CartId OUT;
SELECT @CartId as CartId;
select * from InternetUser.ShoppingCart;  
*/

-- kosár fej hozzaadas 
DROP PROCEDURE [InternetUser].[ShoppingCartLineInsert];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartLineInsert]( @CartId INT = 0,						-- kosar fej azonosito
														  @ProductId NVARCHAR(20) = '',			-- termek azonosito	(ProductName, ProductNameEnglish, PartNumber, ConfigId, CustomerPrice, ItemState - cikk státusza (aktív, passzív, kifutó), )
														  @Quantity INT = 1,					-- mennyiseg
														  @Price INT = 1,						-- ár
														  @DataAreaId NVARCHAR(3) = '',			-- hrp; bsc; ahonnan a termék származik
														  @Status INT = 0, 						-- kosár elem státusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
														  @LineId INT = -1 OUT)			
AS
SET NOCOUNT ON
	DECLARE @CreatedDate DateTime = GetDate();
	INSERT INTO InternetUser.ShoppingCartLine (CartId, ProductId, Quantity, Price, DataAreaId, Status, CreatedDate) VALUES 
											  (@CartId, @ProductId, @Quantity, @Price, @DataAreaId, @Status, @CreatedDate);
	SET @LineId = @@IDENTITY;

RETURN
/*
DECLARE @LineId INT;

EXEC [InternetUser].[ShoppingCartLineInsert] 1, 'Product1', 1, 10000, 'hrp', 1, @LineId;

SELECT  @LineId as LineId;
select * from InternetUser.ShoppingCartLine

*/