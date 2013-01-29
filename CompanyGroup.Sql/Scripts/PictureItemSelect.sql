USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista 
DROP PROCEDURE [InternetUser].[PictureItemSelect];
GO
CREATE PROCEDURE [InternetUser].[PictureItemSelect](@PictureId int = 0) 
AS
SET NOCOUNT ON
	SELECT TOP 1 Id, [FileName], [Primary], RecId 
	FROM InternetUser.Picture WHERE Id = @PictureId;
RETURN
GO
GRANT EXECUTE ON InternetUser.PictureItemSelect TO InternetUser
-- EXEC [InternetUser].[PictureItemSelect] 40;
-- select * from InternetUser.Picture