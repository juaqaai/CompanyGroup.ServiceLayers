USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kosár fej státusz beállítása (törlés, feladás, feladásra várakozás) 
DROP PROCEDURE [InternetUser].[ShoppingCartSetStatus];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartSetStatus](@CartId INT = 0, @Status INT = 0)		
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0;

	--UPDATE InternetUser.ShoppingCart SET Status = @Status WHERE Id = @CartId;

	UPDATE InternetUser.ShoppingCartLine SET [Status] = @Status WHERE CartId = @CartId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[ShoppingCartSetStatus] TO InternetUser
GO
/*
EXEC [InternetUser].[ShoppingCartSetStatus] 1;

select * from InternetUser.ShoppingCart;  
*/
GO
-- kosár sor státusz beállítása (törlés, feladás, feladásra várakozás) 
DROP PROCEDURE [InternetUser].[ShoppingCartSetLineStatus];
GO
CREATE PROCEDURE [InternetUser].[ShoppingCartSetLineStatus]( @LineId INT = 0,						-- kosar fej azonosito
														     @Status INT = 0 						-- kosár elem státusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0 

	UPDATE InternetUser.ShoppingCartLine SET [Status] = @Status WHERE Id = @LineId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[ShoppingCartSetLineStatus] TO InternetUser
GO
/*
EXEC [InternetUser].[ShoppingCartSetLineStatus] 1, 1;

select * from InternetUser.ShoppingCartLine

*/