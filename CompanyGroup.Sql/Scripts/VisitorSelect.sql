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

SELECT V.Id, V.VisitorId, LoginIP, RecId,  CustomerId,  CustomerName,  PersonId,  PersonName,  V.Email,  
	   IsWebAdministrator, InvoiceInfoEnabled, PriceListDownloadEnabled, CanOrder, RecieveGoods, PaymTermId, Currency, LanguageId, V.DefaultPriceGroupId, InventLocationId,  
	   DataAreaId, LoginType as LoginType, PartnerModel, AutoLogin, LoginDate, LogoutDate, ExpireDate, Valid,
	   ISNULL(P.Id, 0) as PriceGroupId, ISNULL(P.Category1Id, '') as PriceGroupCategory1Id, ISNULL(P.Category2Id, '') as PriceGroupCategory2Id, ISNULL(P.Category3Id, '') as PriceGroupCategory3Id, ISNULL(P.ManufacturerId, '') as PriceGroupManufacturerId, 
	   ISNULL(P.[Order], 0) as PriceGroupOrder, ISNULL(P.PriceGroupId, '') as PriceGroup, 
	   V.RepresentativeId,
       ISNULL(R.Name, '') as RepresentativeName,
       ISNULL(R.Phone, '') as RepresentativePhone,
       ISNULL(R.Mobile, '') as RepresentativeMobile,
       ISNULL(R.Extension, '') as RepresentativeExtension,
       ISNULL(R.Email, '') as RepresentativeEmail
	   FROM InternetUser.Visitor as V
	   LEFT OUTER JOIN Representative as R ON R.Id = V.RepresentativeId
	   LEFT OUTER JOIN InternetUser.CustomerPriceGroup as P ON V.Id = P.VisitorId
	   WHERE V.VisitorId = @VisitorId

RETURN

-- EXEC [InternetUser].[VisitorSelect] '03925DB0-4487-470F-BCD5-ED90FF478EFD'
