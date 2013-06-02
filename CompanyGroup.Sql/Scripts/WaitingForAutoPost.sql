USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kos�r fej m�dos�t�s 
DROP PROCEDURE [InternetUser].[WaitingForAutoPostSelect];
GO
CREATE PROCEDURE [InternetUser].[WaitingForAutoPostSelect]
AS
SET NOCOUNT ON

	SELECT Id, ForeignKey, ForeignKeyType, Content, CreatedDate, PostedDate, [Status] 
	FROM InternetUser.WaitingForAutoPost
	WHERE [Status] = 1;

RETURN
GO
GRANT EXECUTE ON InternetUser.WaitingForAutoPostSelect TO InternetUser
GO

-- exec InternetUser.WaitingForAutoPostSelect;

-- v�rakoz� sor m�dos�t�s 
DROP PROCEDURE [InternetUser].[WaitingForAutoPostInsert];
GO
CREATE PROCEDURE [InternetUser].[WaitingForAutoPostInsert](@ForeignKey INT = 0,			-- vagy a ShoppingCart.Id, vagy a Registration.Id	
														   @ForeignKeyType INT = 0,		-- 1: kos�r, 2: regisztr�ci�	
														   @Content XML = ''
)			
AS
SET NOCOUNT ON

	DECLARE @Id INT = -1;

	INSERT INTO InternetUser.WaitingForAutoPost (ForeignKey, ForeignKeyType,   Content,  CreatedDate,  PostedDate,			 [Status]) 
										 VALUES (@ForeignKey, @ForeignKeyType, @Content, GetDate(),    CONVERT(DateTime, 0), 1);
	SET @Id = @@IDENTITY;

	SELECT @Id as Id;

RETURN
GO
GRANT EXECUTE ON InternetUser.[WaitingForAutoPostInsert] TO InternetUser
GO

/*
be�ll�tja a WaitingForAutoPost rekord st�tusz�t
0: t�r�lt, 1: akt�v (autopost-ra v�r), 2: bek�ld�tt
*/
DROP PROCEDURE [InternetUser].[WaitingForAutoPostSetStatus];
GO
CREATE PROCEDURE [InternetUser].[WaitingForAutoPostSetStatus](@Id INT = 0, @Status INT = 0)		
AS
SET NOCOUNT ON

	DECLARE @Ret INT = 0;

	UPDATE InternetUser.WaitingForAutoPost SET Status = @Status WHERE Id = @Id;

	SET @Ret = 1;

	SELECT @Ret as Ret;

RETURN
GO
GRANT EXECUTE ON [InternetUser].[WaitingForAutoPostSetStatus] TO InternetUser
GO