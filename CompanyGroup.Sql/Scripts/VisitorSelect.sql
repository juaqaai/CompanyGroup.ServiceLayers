USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- látogató kiolvasás 
DROP PROCEDURE [InternetUser].[VisitorSelect];
GO
CREATE PROCEDURE [InternetUser].[VisitorSelect] @VisitorId NVARCHAR(64) = ''
AS
SET NOCOUNT ON

SELECT Id, VisitorId, LoginIP, RecId,  CustomerId,  CustomerName,  PersonId,  PersonName,  Email,  
	   IsWebAdministrator,  InvoiceInfoEnabled,  PriceListDownloadEnabled,  CanOrder,  RecieveGoods,  PaymTermId,  Currency, LanguageId,   PriceGroupId,  InventLocationId,  RepresentativeId,  
	   DataAreaId,  LoginMode,  PartnerModel,  AutoLogin,  LoginDate, LogoutDate, ExpireDate, Valid
	   FROM InternetUser.Visitor WHERE VisitorId = @VisitorId

RETURN

-- EXEC [InternetUser].[VisitorSelect] '58D1DC5F-631F-408C-A438-21D451EC3CF2'
