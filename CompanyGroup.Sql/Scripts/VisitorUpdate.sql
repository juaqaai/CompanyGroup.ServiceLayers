USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[VisitorDisableStatus];
GO
CREATE PROCEDURE [InternetUser].[VisitorDisableStatus](@VisitorId	NVARCHAR(64) = '')
AS
SET NOCOUNT ON

	UPDATE InternetUser.Visitor SET Valid = 0 WHERE VisitorId = @VisitorId;

RETURN

GO
DROP PROCEDURE [InternetUser].[VisitorChangeLanguage];
GO
CREATE PROCEDURE [InternetUser].[VisitorChangeLanguage](@VisitorId	NVARCHAR(64) = '', @Language NVARCHAR(10))
AS
SET NOCOUNT ON

	UPDATE InternetUser.Visitor SET LanguageId = @Language WHERE VisitorId = @VisitorId;

RETURN

GO
DROP PROCEDURE [InternetUser].[VisitorChangeCurrency];
GO
CREATE PROCEDURE [InternetUser].[VisitorChangeCurrency](@VisitorId	NVARCHAR(64) = '', @Currency NVARCHAR(10))
AS
SET NOCOUNT ON

	UPDATE InternetUser.Visitor SET Currency = @Currency WHERE VisitorId = @VisitorId;

RETURN