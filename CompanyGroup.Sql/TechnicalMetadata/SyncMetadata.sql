/*
	AX adatbázisból elõzõleg kiolvasásra kerül a legnagyobb felhasználható "CT Version".
	Ez a @LastVersion paraméter értéke.
	Ha van a SyncMetadata táblában aktív (1) státusszal rendelkezõ rekord, akkor annak státusza "posted" értékre lesz beállítva.
	Beszúrásra kerül az új CT verzió a SyncMetadata táblába. 
	A tárolt eljárás visszaadja a korábban aktív (1) státusszal rendelkezõ rekord azonosítóját.
*/

USE TechnicalMetadata
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE dbo.usp_LastSyncVersion
GO
CREATE PROCEDURE dbo.usp_LastSyncVersion( @TableName nvarchar(32) = 'InventSum' )
AS
SET NOCOUNT ON

	DECLARE @LastSyncVersion BIGINT = ISNULL((SELECT MAX(LastVersion) FROM dbo.SyncMetadata WHERE TableName = @TableName AND [Status] = 1), 0);

	SELECT @LastSyncVersion as LastSyncVersion;

RETURN

GO

-- exec dbo.usp_LastSyncVersion 'PriceDiscTable'

/*
	TRUNCATE TABLE dbo.SyncMetadata;

	EXEC dbo.usp_InsertSyncVersion 37, 'PriceDiscTable';

	SELECT * FROM dbo.SyncMetadata

*/
DROP PROCEDURE dbo.usp_InsertSyncVersion
GO
CREATE PROCEDURE dbo.usp_InsertSyncVersion( @LastVersion BIGINT = 0, @TableName nvarchar(32) = 'InventSum' )
AS
SET NOCOUNT ON

	-- korábbi bejegyzések passzívra állítása
	UPDATE dbo.SyncMetadata SET [Status] = 2 WHERE TableName = @TableName AND [Status] = 1;

	-- új bejegyzés létrehozása
	INSERT INTO dbo.SyncMetadata (LastVersion, TableName) VALUES (@LastVersion, @TableName);

RETURN

GO