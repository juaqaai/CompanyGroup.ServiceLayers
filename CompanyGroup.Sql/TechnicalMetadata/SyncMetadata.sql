/*
	AX adatb�zisb�l el�z�leg kiolvas�sra ker�l a legnagyobb felhaszn�lhat� "CT Version".
	Ez a @LastVersion param�ter �rt�ke.
	Ha van a SyncMetadata t�bl�ban akt�v (1) st�tusszal rendelkez� rekord, akkor annak st�tusza "posted" �rt�kre lesz be�ll�tva.
	Besz�r�sra ker�l az �j CT verzi� a SyncMetadata t�bl�ba. 
	A t�rolt elj�r�s visszaadja a kor�bban akt�v (1) st�tusszal rendelkez� rekord azonos�t�j�t.
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

	-- kor�bbi bejegyz�sek passz�vra �ll�t�sa
	UPDATE dbo.SyncMetadata SET [Status] = 2 WHERE TableName = @TableName AND [Status] = 1;

	-- �j bejegyz�s l�trehoz�sa
	INSERT INTO dbo.SyncMetadata (LastVersion, TableName) VALUES (@LastVersion, @TableName);

RETURN

GO