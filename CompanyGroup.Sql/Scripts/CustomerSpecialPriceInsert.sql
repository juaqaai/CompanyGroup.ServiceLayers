/* =============================================
	description	   : Web adatbázisban a CustomerPriceGroup táblába beírja a bejelentkezett felhasználóhoz tartozó egyedi árakat
	running script : exec [InternetUser].[CustomerPriceGroupInsert]
	version		   : 1.0
	created by	   : JUHATT
	modified by	   :
	created date   : 2013.06.30.
	modified date  :
	modify reason  :
 ============================================= */
USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- select * from InternetUser.CustomerSpecialPrice where VisitorId = 746
DROP PROCEDURE [InternetUser].[CustomerSpecialPriceInsert];
GO
CREATE PROCEDURE [InternetUser].[CustomerSpecialPriceInsert](@VisitorKey INT = 0,
														   @ProductId NVARCHAR(20) = '',				
														   @Price INT = 0, 
														   @DataAreaId NVARCHAR(3) = ''
														  )			
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.CustomerSpecialPrice (VisitorId, ProductId, Price, DataAreaId)
										   VALUES (@VisitorKey, @ProductId, @Price, @DataAreaId);

RETURN
GO
GRANT EXECUTE ON InternetUser.CustomerSpecialPriceInsert TO InternetUser

-- exec [InternetUser].[CustomerSpecialPriceInsert] 44826, 'Q6455A', 33333, 'hrp'