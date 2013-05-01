USE [Web]
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
GO
GRANT EXECUTE ON InternetUser.PictureSelect TO InternetUser
-- EXEC [InternetUser].[PictureSelect] '0021165103535';
-- select * from InternetUser.Picture

-- elsõdleges kép 
/*
DROP PROCEDURE [InternetUser].[PrimaryPictureSelect];
GO
CREATE PROCEDURE [InternetUser].[PrimaryPictureSelect](@ProductId nvarchar(20) = '') 
AS
SET NOCOUNT ON

	DECLARE @Id int, @FileName nvarchar(300), @Primary bit, @RecId bigint;

	SELECT @Id = Id, @FileName = [FileName], @Primary = [Primary], @RecId = RecId 
	FROM InternetUser.Picture 
	WHERE ProductId = @ProductId AND [Primary] = 1;

	IF (@Id IS NULL)
	BEGIN
	SELECT @Id = Id, @FileName = [FileName], @Primary = [Primary], @RecId = RecId 
	FROM InternetUser.Picture 
	WHERE ProductId = @ProductId
	END		

RETURN
GO
GRANT EXECUTE ON InternetUser.PrimaryPictureSelect TO InternetUser
*/