USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár fej módosítás 
DROP PROCEDURE [InternetUser].[ShoppingCartUpdate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartUpdate](@CartId INT = 0,						-- login azonosito, CompanyId, PersonId
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
													 @Currency NVARCHAR(10) = ''			-- rendelés feladáshoz tartozó pénznem 
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0;

	UPDATE InternetUser.ShoppingCart SET Name = @Name, 
										 PaymentTerms = @PaymentTerms, 
										 DeliveryTerms = @DeliveryTerms, 
										 DeliveryDateRequested = @DeliveryDateRequested, 
										 DeliveryZipCode = @DeliveryZipCode, 
										 DeliveryCity = @DeliveryCity, 
										 DeliveryCountry = @DeliveryCountry, 
										 DeliveryStreet = @DeliveryStreet, 
										 DeliveryAddrRecId = @DeliveryAddrRecId, 
										 InvoiceAttached = @InvoiceAttached, 
										 Active = @Active, 
										 Currency = @Currency
										 WHERE Id = @CartId;
	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.ShoppingCartUpdate TO InternetUser
GO
/*
DECLARE @date DateTime = GetDate();
EXEC [InternetUser].[ShoppingCartUpdate] 1, 
										 'Shopping cart 1',	
										  1,					-- fizetesi opciok: KP, ÁTUT, Elõre ut., Utánvét (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
										  2,					-- szallitasi opciok: szállítás, vagy raktárból (Delivery = 1, Warehouse = 2)
										  @date,				-- szállítási dátum, ha 1900.01.01, akkor nem állítottak be kiszállítási dátumot
										  '1222',
										  'Budapest',
										  'Hungary',
										  'Wesselényi u. 10.',
										  0,
										  0,					-- lett-e számla csatolva
										  1,					-- aktív-e a kosár (egyszerre csak egy kosár lehet aktív)
										  'HUF';				-- rendelés feladáshoz tartozó pénznem

select * from InternetUser.ShoppingCart;  
*/

-- kosár sor módosítás (csak a mennyiséget lehet megváltoztatni)
DROP PROCEDURE [InternetUser].[ShoppingCartLineUpdate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartLineUpdate]( @LineId INT = 0,						-- kosar fej azonosito
														  @Quantity INT = 1					-- mennyiseg
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0 

	UPDATE InternetUser.ShoppingCartLine SET Quantity = @Quantity
	WHERE Id = @LineId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.[ShoppingCartLineUpdate] TO InternetUser
GO
/*
EXEC [InternetUser].[ShoppingCartLineUpdate] 1, 1, 1;

select * from InternetUser.ShoppingCartLine
*/
-- kosár "tárolt" flag beállítása azoknál a kosárelemeknél, ahol a Status = 1
DROP PROCEDURE [InternetUser].[ShoppingCartStore];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartStore]( @CartId INT = 0,						-- kosar fej azonosito
													 @Name NVARCHAR(64) = '',				-- név
													 @Status INT = 2 						-- kosár elem státusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0 

	UPDATE InternetUser.ShoppingCart SET Name = @Name, [Status] = @Status
	WHERE Id = @CartId;

	UPDATE InternetUser.ShoppingCartLine SET [Status] = @Status
	WHERE CartId = @CartId AND [Status] = 1;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.[ShoppingCartStore] TO InternetUser
GO

-- kosár "aktív" flag beállítása
DROP PROCEDURE [InternetUser].[ShoppingCartActivate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartActivate]( @CartId INT = 0,						-- kosar fej azonosito
														@VisitorId NVARCHAR(64) = ''
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0 

	UPDATE InternetUser.ShoppingCart SET [Active] = 0 WHERE VisitorId = @VisitorId AND Id <> @CartId AND Status IN (1, 2);

	UPDATE InternetUser.ShoppingCart SET [Active] = 1 WHERE Id = @CartId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.[ShoppingCartActivate] TO InternetUser
GO
-- VisitorId beállítása azoknál a kosaraknál, ahol a PermanentVisitorId volt beállítva
DROP PROCEDURE [InternetUser].[ShoppingCartAssociate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartAssociate]( @PermanentVisitorId NVARCHAR(64) = '',
														 @VisitorId NVARCHAR(64) = ''
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0;

	UPDATE InternetUser.ShoppingCart SET VisitorId = @VisitorId WHERE VisitorId = @PermanentVisitorId AND [Status] = 2;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.[ShoppingCartAssociate] TO InternetUser
GO