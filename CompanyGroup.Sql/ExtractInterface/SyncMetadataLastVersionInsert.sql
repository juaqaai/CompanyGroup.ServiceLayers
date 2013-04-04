/*
	AX adatbázisból elõzõleg kiolvasásra kerül a legnagyobb felhasználható "CT Version".
	Ez a @LastVersion paraméter értéke.
	Ha van a SyncMetadata táblában aktív (1) státusszal rendelkezõ rekord, akkor annak státusza "posted" értékre lesz beállítva.
	Beszúrásra kerül az új CT verzió a SyncMetadata táblába. 
	A tárolt eljárás visszaadja a korábban aktív (1) státusszal rendelkezõ rekord azonosítóját.
*/

USE [ExtractInterface]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.SyncMetadataLastVersionInsert
GO
CREATE PROCEDURE InternetUser.SyncMetadataLastVersionInsert( @LastVersion BIGINT = 0 )
AS
SET NOCOUNT ON

	DECLARE @Id INT = ISNULL((SELECT MAX(Id) FROM InternetUser.SyncMetadata WHERE [Status] = 1), 0);

	IF (@Id > 0)
	BEGIN
		UPDATE InternetUser.SyncMetadata SET [Status] = 2 WHERE Id = @Id;
	END

	INSERT INTO InternetUser.SyncMetadata (LastVersion) VALUES ( @LastVersion);

	SELECT @Id as Id;

RETURN

GO
GRANT EXECUTE ON InternetUser.SyncMetadataLastVersionInsert TO InternetUser
GO

/*
	TRUNCATE TABLE InternetUser.SyncMetadata;

	EXEC InternetUser.SyncMetadataLastVersionInsert 37;

	SELECT * FROM InternetUser.SyncMetadata

*/