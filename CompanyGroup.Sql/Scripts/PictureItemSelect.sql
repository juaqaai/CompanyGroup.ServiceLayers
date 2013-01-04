USE [WebDb_Test]
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

-- EXEC [InternetUser].[PictureItemSelect] 40;
-- select * from InternetUser.Picture