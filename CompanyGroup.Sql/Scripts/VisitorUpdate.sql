USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

-- kijelentkezés
GO
DROP PROCEDURE [InternetUser].[VisitorDisableStatus];
GO
CREATE PROCEDURE [InternetUser].[VisitorDisableStatus](@VisitorId	NVARCHAR(64) = '')
AS
SET NOCOUNT ON

	UPDATE InternetUser.Visitor SET Valid = 0, LogoutDate = GETDATE() WHERE VisitorId = @VisitorId;

RETURN
GO
GRANT EXECUTE ON InternetUser.[VisitorDisableStatus] TO InternetUser
GO

-- nyelvválasztás
GO
DROP PROCEDURE [InternetUser].[VisitorChangeLanguage];
GO
CREATE PROCEDURE [InternetUser].[VisitorChangeLanguage](@VisitorId	NVARCHAR(64) = '', @Language NVARCHAR(10))
AS
SET NOCOUNT ON

	UPDATE InternetUser.Visitor SET LanguageId = @Language WHERE VisitorId = @VisitorId;

RETURN
GO
GRANT EXECUTE ON InternetUser.[VisitorChangeLanguage] TO InternetUser
GO

-- valutanem választás
GO
DROP PROCEDURE [InternetUser].[VisitorChangeCurrency];
GO
CREATE PROCEDURE [InternetUser].[VisitorChangeCurrency](@VisitorId	NVARCHAR(64) = '', @Currency NVARCHAR(10))
AS
SET NOCOUNT ON

	UPDATE InternetUser.Visitor SET Currency = @Currency WHERE VisitorId = @VisitorId;

RETURN
GO
GRANT EXECUTE ON InternetUser.[VisitorChangeCurrency] TO InternetUser
GO