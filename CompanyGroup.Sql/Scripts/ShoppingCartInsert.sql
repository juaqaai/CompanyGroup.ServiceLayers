USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej hozzaadas 
DROP PROCEDURE [InternetUser].[ShoppingCartInsert];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartInsert](@VisitorId	NVARCHAR(64) = '',			-- login azonosito, CompanyId, PersonId
													 @Name NVARCHAR(32) = '',				-- kosar neve
													 @PaymentTerms INT = 0,					-- fizetesi opciok: KP, �TUT, El�re ut., Ut�nv�t (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
													 @DeliveryTerms INT = 0,				-- szallitasi opciok: sz�ll�t�s, vagy rakt�rb�l (Delivery = 1, Warehouse = 2)
													 @DeliveryDateRequested	DATETIME,		-- sz�ll�t�si d�tum, ha 1900.01.01, akkor nem �ll�tottak be kisz�ll�t�si d�tumot
													 @DeliveryZipCode NVARCHAR(4) = '',
													 @DeliveryCity NVARCHAR(32) = '',
													 @DeliveryCountry NVARCHAR(32) = '',
													 @DeliveryStreet NVARCHAR(64) = '',
													 @DeliveryAddrRecId	BIGINT = 0,
													 @InvoiceAttached BIT = 0,				-- lett-e sz�mla csatolva
													 @Active BIT = 0,						-- akt�v-e a kos�r (egyszerre csak egy kos�r lehet akt�v)
													 @Currency NVARCHAR(10) = ''			-- rendel�s felad�shoz tartoz� p�nznem 
													 )			
AS
SET NOCOUNT ON
	DECLARE @CartId INT = -1;

	INSERT INTO InternetUser.ShoppingCart (VisitorId, Name, PaymentTerms, 
										   DeliveryTerms, DeliveryDateRequested, DeliveryZipCode, DeliveryCity, DeliveryCountry, DeliveryStreet, DeliveryAddrRecId, 
										   InvoiceAttached, Active, Currency, CreatedDate, Status) VALUES (@VisitorId, @Name, @PaymentTerms, 
										   @DeliveryTerms, @DeliveryDateRequested, @DeliveryZipCode, @DeliveryCity, @DeliveryCountry, @DeliveryStreet, @DeliveryAddrRecId, 
										   @InvoiceAttached, @Active, @Currency, GetDate(), 1);
	SET @CartId = @@IDENTITY;

	-- felhaszn�l� kosarai active = false-ra �ll�t�sa
	UPDATE InternetUser.ShoppingCart SET Active = 0 WHERE VisitorId = @VisitorId AND Id <> @CartId AND [Status] IN (1, 2);

	SELECT @CartId as CartId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[ShoppingCartInsert] TO InternetUser

/*
DECLARE @date DateTime = GetDate();
EXEC [InternetUser].[ShoppingCartInsert] 'teszt', 
										 'Shopping cart 1',	
										  1,					-- fizetesi opciok: KP, �TUT, El�re ut., Ut�nv�t (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
										  2,				-- szallitasi opciok: sz�ll�t�s, vagy rakt�rb�l (Delivery = 1, Warehouse = 2)
										  @date,		-- sz�ll�t�si d�tum, ha 1900.01.01, akkor nem �ll�tottak be kisz�ll�t�si d�tumot
										  '1222',
										  'Budapest',
										  'Hungary',
										  'Wessel�nyi u. 10.',
										  0,
										  0,				-- lett-e sz�mla csatolva
										  1,						-- akt�v-e a kos�r (egyszerre csak egy kos�r lehet akt�v)
										  'HUF'			-- rendel�s felad�shoz tartoz� p�nznem 
select * from InternetUser.ShoppingCart;  
*/
GO
GRANT EXECUTE ON [InternetUser].[ShoppingCartInsert] TO InternetUser
GO
-- kos�r sor hozzaadas 
DROP PROCEDURE [InternetUser].[ShoppingCartLineInsert];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartLineInsert]( @CartId INT = 0,						-- kosar fej azonosito
														  @ProductId NVARCHAR(20) = '',			-- termek azonosito	(ProductName, ProductNameEnglish, PartNumber, ConfigId, CustomerPrice, ItemState - cikk st�tusza (akt�v, passz�v, kifut�), )
														  @Quantity INT = 1,					-- mennyiseg
														  @Price INT = 1,						-- �r
														  @DataAreaId NVARCHAR(3) = '',			-- hrp; bsc; ahonnan a term�k sz�rmazik
														  @Status INT = 0 						-- kos�r elem st�tusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
														  )			
AS
SET NOCOUNT ON
	DECLARE @CreatedDate DateTime = GetDate(), @LineId INT = -1 ;

	IF (EXISTS(SELECT * FROM InternetUser.ShoppingCartLine WHERE CartId = @CartId AND ProductId = @ProductId AND DataAreaId = @DataAreaId AND Status IN (1, 2)))
		UPDATE InternetUser.ShoppingCartLine SET Quantity = Quantity + @Quantity 
		WHERE CartId = @CartId AND ProductId = @ProductId AND DataAreaId = @DataAreaId AND Status IN (1, 2)
	ELSE
	BEGIN
		INSERT INTO InternetUser.ShoppingCartLine (CartId, ProductId, Quantity, Price, DataAreaId, Status, CreatedDate) VALUES 
												  (@CartId, @ProductId, @Quantity, @Price, @DataAreaId, @Status, @CreatedDate);
		SET @LineId = @@IDENTITY;
	END
	SELECT @LineId as LineId;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[ShoppingCartLineInsert] TO InternetUser
/*
DECLARE @LineId INT;

EXEC [InternetUser].[ShoppingCartLineInsert] 1, 'CR180', 1, 660700, 'hrp', 1, @LineId OUT;

SELECT  @LineId as LineId;
select * from InternetUser.ShoppingCartLine

select * from InternetUser.Catalogue
*/