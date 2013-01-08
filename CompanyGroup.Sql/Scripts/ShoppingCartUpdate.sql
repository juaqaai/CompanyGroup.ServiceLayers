USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej hozzaadas 
DROP PROCEDURE [InternetUser].[ShoppingCartUpdate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartUpdate](@CartId	INT = 0,			-- login azonosito, CompanyId, PersonId
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
													 @Ret INT OUT
)			
AS
SET NOCOUNT ON
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
DROP PROCEDURE [InternetUser].[ShoppingCartLineUpdate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartLineUpdate]( @LineId INT = 0,						-- kosar fej azonosito
														  @Quantity INT = 1,					-- mennyiseg
														  @Price INT = 1,						-- �r
														  @Status INT = 0, 						-- kos�r elem st�tusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
														  @Ret INT = -1 OUT)			
AS
SET NOCOUNT ON

	UPDATE InternetUser.ShoppingCartLine SET Quantity = @Quantity, 
											 Price = @Price, 
											 Status = @Status
	WHERE Id = @LineId;

	SET @Ret = 1;

RETURN
/*
DECLARE @LineId INT;

EXEC [InternetUser].[ShoppingCartLineUpdate] 1, 'Product1', 1, 10000, 'hrp', 1, @LineId;

SELECT  @LineId as LineId;
select * from InternetUser.ShoppingCartLine

*/