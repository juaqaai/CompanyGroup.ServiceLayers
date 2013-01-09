USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej m�dos�t�s 
DROP PROCEDURE [InternetUser].[ShoppingCartUpdate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartUpdate](@CartId INT = 0,						-- login azonosito, CompanyId, PersonId
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

/*
DECLARE @date DateTime = GetDate();
EXEC [InternetUser].[ShoppingCartUpdate] 1, 
										 'Shopping cart 1',	
										  1,					-- fizetesi opciok: KP, �TUT, El�re ut., Ut�nv�t (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
										  2,					-- szallitasi opciok: sz�ll�t�s, vagy rakt�rb�l (Delivery = 1, Warehouse = 2)
										  @date,				-- sz�ll�t�si d�tum, ha 1900.01.01, akkor nem �ll�tottak be kisz�ll�t�si d�tumot
										  '1222',
										  'Budapest',
										  'Hungary',
										  'Wessel�nyi u. 10.',
										  0,
										  0,					-- lett-e sz�mla csatolva
										  1,					-- akt�v-e a kos�r (egyszerre csak egy kos�r lehet akt�v)
										  'HUF';				-- rendel�s felad�shoz tartoz� p�nznem

select * from InternetUser.ShoppingCart;  
*/
GO
-- kos�r sor m�dos�t�s (csak a mennyis�get lehet megv�ltoztatni)
DROP PROCEDURE [InternetUser].[ShoppingCartLineUpdate];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartLineUpdate]( @LineId INT = 0,						-- kosar fej azonosito
														  @Quantity INT = 1,					-- mennyiseg
														  @Status INT = 0 						-- kos�r elem st�tusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0 

	UPDATE InternetUser.ShoppingCartLine SET Quantity = @Quantity, [Status] = @Status
	WHERE Id = @LineId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
/*
EXEC [InternetUser].[ShoppingCartLineUpdate] 1, 1, 1;

select * from InternetUser.ShoppingCartLine
*/