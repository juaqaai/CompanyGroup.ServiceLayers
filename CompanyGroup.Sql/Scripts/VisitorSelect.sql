USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- látogató kiolvasás 
DROP PROCEDURE [InternetUser].[VisitorSelect];
GO
CREATE PROCEDURE [InternetUser].[VisitorSelect]( @VisitorId NVARCHAR(64) = '')
AS
SET NOCOUNT ON

SELECT V.Id, V.VisitorId, V.LoginIP, V.RecId,  V.CustomerId,  V.CustomerName,  V.PersonId,  V.PersonName,  V.Email,  
	   V.IsWebAdministrator, V.InvoiceInfoEnabled, V.PriceListDownloadEnabled, V.CanOrder, V.RecieveGoods, V.PaymTermId, V.Currency, 
	   V.LanguageId, V.DefaultPriceGroupId, V.InventLocationId, 
	   V.RepresentativeId,
       ISNULL(R.Name, '') as RepresentativeName,
       ISNULL(R.Phone, '') as RepresentativePhone,
       ISNULL(R.Mobile, '') as RepresentativeMobile,
       ISNULL(R.Extension, '') as RepresentativeExtension,
       ISNULL(R.Email, '') as RepresentativeEmail,	 	    
	   V.DataAreaId, V.LoginType as LoginType, V.PartnerModel, V.AutoLogin, V.LoginDate, V.LogoutDate, V.ExpireDate, V.Valid, 
	   ISNULL(P.Id, 0) as LineId, ISNULL(P.VisitorId, 0) as VisitorKey, ISNULL(P.PriceGroupId, '') as PriceGroupId, ISNULL(P.ManufacturerId, '') as ManufacturerId, ISNULL(P.Category1Id, '') as Category1Id, ISNULL(P.Category2Id, '') as Category2Id, ISNULL(P.Category3Id, '') as Category3Id, 
	   ISNULL(P.[Order], 0) as [Order], P.DataAreaId
	   FROM InternetUser.Visitor as V
	   LEFT OUTER JOIN Representative as R ON R.Id = V.RepresentativeId
	   LEFT OUTER JOIN InternetUser.CustomerPriceGroup as P ON V.Id = P.VisitorId
	   WHERE V.VisitorId = @VisitorId
/*
  Id	VisitorId	LoginIP	RecId	CustomerId	CustomerName	PersonId	PersonName	Email	
  IsWebAdministrator	InvoiceInfoEnabled	PriceListDownloadEnabled	CanOrder	RecieveGoods	PaymTermId	Currency
  LanguageId	DefaultPriceGroupId	InventLocationId	
  DataAreaId	LoginType	PartnerModel	AutoLogin	LoginDate	LogoutDate	ExpireDate	Valid
  RepresentativeId	RepresentativeName	RepresentativePhone	RepresentativeMobile	RepresentativeExtension	RepresentativeEmail
  Id V	PriceGroupId 	ManufacturerId Category1Id	Category2Id	Category3Id	Order 
*/

RETURN
GO
GRANT EXECUTE ON InternetUser.VisitorSelect TO InternetUser
GO
-- EXEC [InternetUser].[VisitorSelect] '9965656AE0234B83A7905D056D63387D'

-- select top 100 * from InternetUser.Visitor order by id desc


-- látogató kiolvasás 
DROP PROCEDURE [InternetUser].[CustomerPriceGroups];
GO
CREATE PROCEDURE [InternetUser].[CustomerPriceGroups]( @Id INT = 0)
AS
SET NOCOUNT ON

	SELECT Id as LineId, VisitorId as VisitorKey, PriceGroupId, ManufacturerId, Category1Id, Category2Id, Category3Id, [Order], DataAreaId
	FROM InternetUser.CustomerPriceGroup
	WHERE VisitorId = @Id
RETURN
GO
GRANT EXECUTE ON InternetUser.CustomerPriceGroups TO InternetUser
GO

-- exec [InternetUser].[CustomerPriceGroups] 1028;