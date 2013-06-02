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

/*
int id, string visitorId, string loginIP, long recId, string customerId, string customerName, string personId, string personName, string email, bool isWebAdministrator, bool invoiceInfoEnabled, 
                       bool priceListDownloadEnabled, bool canOrder, bool recieveGoods, string paymTermIdBsc, string paymTermIdHrp, string currency,   
                       string languageId, string defaultPriceGroupIdHrp, string defaultPriceGroupIdBsc, string inventLocationIdHrp, string inventLocationIdBsc, 
                       string representativeId, string representativeName, string representativePhone, string representativeMobile, string representativeExtension, string representativeEmail,
                       int loginType, bool rightHrp, bool rightBsc, bool contractHrp, bool contractBsc, int cartId, string registrationId, bool isCatalogueOpened, bool isShoppingCartOpened, 
                       bool autoLogin, DateTime loginDate, DateTime logoutDate, DateTime expireDate, bool valid
*/

SELECT V.Id, V.VisitorId, V.LoginIP, V.RecId,  V.CustomerId,  V.CustomerName,  V.PersonId,  V.PersonName,  V.Email,  
	   V.IsWebAdministrator, V.InvoiceInfoEnabled, V.PriceListDownloadEnabled, V.CanOrder, V.RecieveGoods, V.PaymTermIdBsc, V.PaymTermIdHrp, V.Currency, 
	   V.LanguageId, V.DefaultPriceGroupIdHrp, V.DefaultPriceGroupIdBsc, V.InventLocationIdHrp, V.InventLocationIdBsc, 
	   V.RepresentativeId,
       ISNULL(R.Name, '') as RepresentativeName,
       ISNULL(R.Phone, '') as RepresentativePhone,
       ISNULL(R.Mobile, '') as RepresentativeMobile,
       ISNULL(R.Extension, '') as RepresentativeExtension,
       ISNULL(R.Email, '') as RepresentativeEmail,	 	    
	   V.LoginType as LoginType, V.RightHrp, V.RightBsc, V.ContractHrp, V.ContractBsc, V.CartId, V.RegistrationId, V.IsCatalogueOpened, V.IsShoppingCartOpened, V.AutoLogin, V.LoginDate, V.LogoutDate, V.[ExpireDate], V.Valid 
--	   ISNULL(P.Id, 0) as LineId, ISNULL(P.VisitorId, 0) as VisitorKey, ISNULL(P.PriceGroupId, '') as PriceGroupId, ISNULL(P.ManufacturerId, '') as ManufacturerId, ISNULL(P.Category1Id, '') as Category1Id, ISNULL(P.Category2Id, '') as Category2Id, ISNULL(P.Category3Id, '') as Category3Id, 
--	   ISNULL(P.[Order], 0) as [Order], P.DataAreaId
	   FROM InternetUser.Visitor as V
	   LEFT OUTER JOIN Representative as R ON R.Id = V.RepresentativeId
--	   LEFT OUTER JOIN InternetUser.CustomerPriceGroup as P ON V.Id = P.VisitorId
	   WHERE V.VisitorId = @VisitorId
/*
Id, VisitorId, LoginIP, RecId, CustomerId, CustomerName, PersonId, PersonName, Email,  
									  IsWebAdministrator,  InvoiceInfoEnabled,  PriceListDownloadEnabled, CanOrder, RecieveGoods,  
									  PaymTermIdBsc, PaymTermIdHrp, Currency, LanguageId, DefaultPriceGroupIdHrp, DefaultPriceGroupIdBsc, InventLocationIdHrp, InventLocationIdBsc, 
									  RepresentativeId,  LoginType, RightHrp, RightBsc, ContractHrp, ContractBsc, CartId, RegistrationId, IsCatalogueOpened, IsShoppingCartOpened, 
									  AutoLogin, LoginDate, LogoutDate, [ExpireDate], Valid 
*/

RETURN
GO
GRANT EXECUTE ON InternetUser.VisitorSelect TO InternetUser
GO
-- EXEC [InternetUser].[VisitorSelect] '865D102B5C834B99858C390B269C68A6'

-- select top 100 * from InternetUser.Visitor order by id desc


-- látogatóhoz tartozó árbesorolások kiolvasása
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

-- TRUNCATE TABLE InternetUser.CustomerPriceGroup;
-- select * FROM InternetUser.CustomerPriceGroup
-- exec [InternetUser].[CustomerPriceGroups] 1028;