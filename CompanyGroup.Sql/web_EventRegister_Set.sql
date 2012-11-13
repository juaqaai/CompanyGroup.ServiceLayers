USE [InternetDb]
GO

/****** Object:  StoredProcedure [InternetUser].[web_EventRegister_Set]    Script Date: 2012.11.12. 22:04:47 ******/
DROP PROCEDURE [InternetUser].[web_EventRegister_Set]
GO

/****** Object:  StoredProcedure [InternetUser].[web_EventRegister_Set]    Script Date: 2012.11.12. 22:04:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [InternetUser].[web_EventRegister_Set]( @sEventID VARCHAR (16), 
													 @sCompanyName VARCHAR (128), 
													 @sName VARCHAR (128), 
													 @sPhone VARCHAR (32), 
													 @sEmail VARCHAR (64), 
													 @sFollower VARCHAR (16),
													 @sMenu VARCHAR (16),
													 @sPartnerId VARCHAR (16),
													 @iRet INT OUTPUT )
AS
SET NOCOUNT ON
SET @iRet = 0
-- SELECT @iRet = ISNULL( iID, 0 ) FROM InternetUser.web_EventRegister WHERE sName = @sName AND sCompanyName = @sCompanyName AND sEventID = @sEventID AND iValid > 0 
--IF ( @iRet = 0 )
--BEGIN
	INSERT INTO InternetUser.web_EventRegister ( sEventID, sCompanyName, sName, sPhone, sEmail, sFollower, sMenu, sPartnerId ) VALUES
											   ( @sEventID, @sCompanyName, @sName, @sPhone, @sEmail, @sFollower, @sMenu, @sPartnerId );
	SET @iRet = SCOPE_IDENTITY();
--END
--ELSE
--BEGIN
--	UPDATE InternetUser.web_EventRegister SET sEventID = @sEventID, 
--											  sCompanyName = @sCompanyName, 
--											  sName = @sName, 
--											  sPhone = @sPhone, 
--											  sEmail = @sEmail, 
--											  sFollower = @sFollower, 
--											  sMenu = @sMenu
--	WHERE iID = @iRet;
END

GO


