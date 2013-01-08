USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej hozzaadas 
DROP PROCEDURE [InternetUser].[InsertShoppingCart];
GO
CREATE PROCEDURE [InternetUser].[InsertShoppingCart](@VisitorId	NVARCHAR(64) = '',			-- login azonosito, CompanyId, PersonId
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

-- select * from InternetUser.ShoppingCart