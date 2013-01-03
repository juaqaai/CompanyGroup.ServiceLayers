USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista 
DROP PROCEDURE [InternetUser].[PictureSelect];
GO
CREATE PROCEDURE [InternetUser].[PictureSelect](@ProductId nvarchar(20) = '') 
AS
SET NOCOUNT ON
	SELECT Id, [FileName], [Primary], RecId 
	FROM InternetUser.Picture WHERE ProductId = @ProductId;
RETURN

-- EXEC [InternetUser].[PictureSelect] '0021165103535';
-- select * from InternetUser.Picture