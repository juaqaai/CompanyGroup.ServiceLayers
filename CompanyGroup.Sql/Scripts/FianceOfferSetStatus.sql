USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej st�tusz be�ll�t�sa (t�rl�s, felad�s, felad�sra v�rakoz�s) 
DROP PROCEDURE [InternetUser].[FianceOfferSetStatus];
GO
CREATE PROCEDURE [InternetUser].[FianceOfferSetStatus](@OfferId INT = 0, @Status INT = 0)		
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0;

	UPDATE InternetUser.ShoppingCart SET Status = @Status WHERE Id = @OfferId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN

/*
EXEC [InternetUser].[FianceOfferSetStatus] 1, 2;

select * from InternetUser.FinanceOffer;  
*/
GO
-- kos�r sor st�tusz be�ll�t�sa (t�rl�s, felad�s, felad�sra v�rakoz�s) 
DROP PROCEDURE [InternetUser].[FinanceOfferSetLineStatus];
GO
CREATE PROCEDURE [InternetUser].[FinanceOfferSetLineStatus]( @LineId INT = 0,						-- kosar fej azonosito
														     @Status INT = 0 						-- kos�r elem st�tusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
)			
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0 

	UPDATE InternetUser.ShoppingCartLine SET [Status] = @Status WHERE Id = @LineId;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
/*
EXEC [InternetUser].[FinanceOfferSetLineStatus] 1, 1;

select * from InternetUser.ShoppingCartLine

*/