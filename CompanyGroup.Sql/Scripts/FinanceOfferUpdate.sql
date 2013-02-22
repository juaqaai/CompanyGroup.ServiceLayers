USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ajánlat fej módosítás 
DROP PROCEDURE [InternetUser].[FinanceOfferUpdate];
GO
CREATE PROCEDURE [InternetUser].[FinanceOfferUpdate]( @OfferId INT = 0,			
													  @LeasingPersonName NVARCHAR(250) = '',
													  @LeasingAddress NVARCHAR(250) = '',
													  @LeasingPhone NVARCHAR(100) = '',
													  @LeasingStatNumber NVARCHAR(100) = '',
													  @NumOfMonth INT = 0,
													  @Currency NVARCHAR(10) = ''
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0;

	UPDATE InternetUser.FinanceOffer SET LeasingPersonName = @LeasingPersonName, 
										 LeasingAddress = @LeasingAddress, 
										 LeasingPhone = @LeasingPhone, 
										 LeasingStatNumber = @LeasingStatNumber, 
										 NumOfMonth = @NumOfMonth, 
										 Currency = @Currency
										 WHERE Id = @OfferId;
	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON InternetUser.FinanceOfferUpdate TO InternetUser
/*
EXEC [InternetUser].[FinanceOfferUpdate] 'VisitorId', 
										 'LeasingPersonName',	
										 'LeasingAddress',
										 'LeasingPhone',
										 'LeasingStatNumber',
										 24,
									     'HUF';

select * from InternetUser.FinanceOffer;  
*/
GO
-- kosár sor módosítás (csak a mennyiséget lehet megváltoztatni)
DROP PROCEDURE [InternetUser].[FinanceOfferLineUpdate];
GO
CREATE PROCEDURE [InternetUser].[FinanceOfferLineUpdate]( @LineId INT = 0,					-- kosar sor azonosito
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
GRANT EXECUTE ON InternetUser.FinanceOfferLineUpdate TO InternetUser
/*
EXEC [InternetUser].[FinanceOfferLineUpdate] 1, 1;

select * from InternetUser.ShoppingCartLine
*/
GO
