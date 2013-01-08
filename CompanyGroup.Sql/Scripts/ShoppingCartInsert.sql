USE [WebDb_Test]
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
													 @Currency NVARCHAR(10) = '',			-- rendel�s felad�shoz tartoz� p�nznem 
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
										  'HUF',			-- rendel�s felad�shoz tartoz� p�nznem 
										  @CartId OUT;
SELECT @CartId as CartId;
select * from InternetUser.ShoppingCart;  
*/

-- kos�r fej hozzaadas 
DROP PROCEDURE [InternetUser].[ShoppingCartLineInsert];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartLineInsert]( @CartId INT = 0,						-- kosar fej azonosito
														  @ProductId NVARCHAR(20) = '',			-- termek azonosito	(ProductName, ProductNameEnglish, PartNumber, ConfigId, CustomerPrice, ItemState - cikk st�tusza (akt�v, passz�v, kifut�), )
														  @Quantity INT = 1,					-- mennyiseg
														  @Price INT = 1,						-- �r
														  @DataAreaId NVARCHAR(3) = '',			-- hrp; bsc; ahonnan a term�k sz�rmazik
														  @Status INT = 0, 						-- kos�r elem st�tusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
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