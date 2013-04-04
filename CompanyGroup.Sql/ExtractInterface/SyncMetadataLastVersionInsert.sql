/*
	AX adatb�zisb�l el�z�leg kiolvas�sra ker�l a legnagyobb felhaszn�lhat� "CT Version".
	Ez a @LastVersion param�ter �rt�ke.
	Ha van a SyncMetadata t�bl�ban akt�v (1) st�tusszal rendelkez� rekord, akkor annak st�tusza "posted" �rt�kre lesz be�ll�tva.
	Besz�r�sra ker�l az �j CT verzi� a SyncMetadata t�bl�ba. 
	A t�rolt elj�r�s visszaadja a kor�bban akt�v (1) st�tusszal rendelkez� rekord azonos�t�j�t.
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