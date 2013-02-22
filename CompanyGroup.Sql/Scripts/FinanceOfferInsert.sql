USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej hozzaadas 
/*
	VisitorId				NVARCHAR(64) NOT NULL DEFAULT '',				-- login azonosito, CompanyId, PersonId
	CompanyId				NVARCHAR(20) NOT NULL DEFAULT '',
	PersonId				NVARCHAR(20) NOT NULL DEFAULT '',
	LeasingPersonName		NVARCHAR(250) NOT NULL DEFAULT '',				-- b�rbevev� n�v
	LeasingAddress			NVARCHAR(250) NOT NULL DEFAULT '',				-- b�rbevev� c�m
	LeasingPhone			NVARCHAR(100) NOT NULL DEFAULT '',				-- b�rbevev� telefonsz�ma
	LeasingStatNumber		NVARCHAR(100) NOT NULL DEFAULT '',				-- b�rbevev� c�gjegyz�ksz�ma
	NumOfMonth				INT NOT NULL DEFAULT 1,							-- leasing konstrukci�
	Currency				NVARCHAR(10) NOT NULL DEFAULT '',				-- rendel�s felad�shoz tartoz� p�nznem
	CreatedDate				DATETIME NOT NULL DEFAULT GETDATE(),			-- datum, ido
	[Status]				INT NOT NULL DEFAULT 1
*/
DROP PROCEDURE [InternetUser].[FinanceOfferInsert];
GO
CREATE PROCEDURE [InternetUser].[FinanceOfferInsert](@VisitorId	NVARCHAR(64) = '',			-- login azonosito, CompanyId, PersonId
													 @LeasingPersonName NVARCHAR(250) = '',
													 @LeasingAddress NVARCHAR(250) = '',
													 @LeasingPhone NVARCHAR(100) = '',
													 @LeasingStatNumber NVARCHAR(100) = '',
													 @NumOfMonth INT = 0,
													 @Currency NVARCHAR(10) = ''			-- rendel�s felad�shoz tartoz� p�nznem 
													 )			
AS
SET NOCOUNT ON
	DECLARE @OfferId INT = -1;
	DECLARE @CartId INT = 0;

	INSERT INTO InternetUser.FinanceOffer (CartId, PersonName, Address, Phone, StatNumber, NumOfMonth, CreatedDate) 
								   VALUES (@CartId, @LeasingPersonName, @LeasingAddress, @LeasingPhone, @LeasingStatNumber, @NumOfMonth, GetDate());
	SET @OfferId = @@IDENTITY;

	SELECT @OfferId as OfferId;

RETURN
GO
GRANT EXECUTE ON InternetUser.FinanceOfferInsert TO InternetUser
/*
EXEC [InternetUser].[FinanceOfferInsert] 'VisitorId', 
										 'LeasingPersonName',	
										 'LeasingAddress',
										 'LeasingPhone',
										 'LeasingStatNumber',
										 12,
									     'HUF';

select * from InternetUser.FinanceOffer;  
*/
GO
-- kos�r fej hozzaadas 
DROP PROCEDURE [InternetUser].[FinanceOfferLineInsert];
GO
CREATE PROCEDURE [InternetUser].[FinanceOfferLineInsert]( @OfferId INT = 0,						-- aj�nlat fej azonosito
														  @ProductId NVARCHAR(20) = '',			-- termek azonosito	(ProductName, ProductNameEnglish, PartNumber, ConfigId, CustomerPrice, ItemState - cikk st�tusza (akt�v, passz�v, kifut�), )
														  @Quantity INT = 1,					-- mennyiseg
														  @Price INT = 1,						-- �r
														  @DataAreaId NVARCHAR(3) = '',			-- hrp; bsc; ahonnan a term�k sz�rmazik
														  @Status INT = 0 						-- kos�r elem st�tusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
														  )			
AS
SET NOCOUNT ON
	DECLARE @CreatedDate DateTime = GetDate(), @LineId INT = -1 ;

	INSERT INTO InternetUser.ShoppingCartLine (CartId, ProductId, Quantity, Price, DataAreaId, Status, CreatedDate) VALUES 
											  (@OfferId, @ProductId, @Quantity, @Price, @DataAreaId, @Status, @CreatedDate);
	SET @LineId = @@IDENTITY;

	SELECT @LineId as LineId;

RETURN
GO
GRANT EXECUTE ON InternetUser.FinanceOfferLineInsert TO InternetUser
/*
DECLARE @LineId INT;

EXEC [InternetUser].[FinanceOfferLineInsert] 1, 'CR180', 1, 660700, 'hrp', 1, @LineId OUT;

SELECT  @LineId as LineId;
select * from InternetUser.ShoppingCartLine

select * from InternetUser.Catalogue
*/