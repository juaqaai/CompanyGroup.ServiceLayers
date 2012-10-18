USE WebDb_Test
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.cms_ContactPerson
GO
CREATE PROCEDURE InternetUser.cms_ContactPerson( @ContactPersonId nvarchar(20), @DataAreaId NVARCHAR(3) = 'hrp')											
AS
	DECLARE @VirtualDataAreaId nvarchar(3);

	SET @VirtualDataAreaId = InternetUser.GetVirtualDataAreaId( @DataAreaId );

	DECLARE @Yes BIT, @No BIT;
	SET @Yes = CONVERT( BIT, 1);
	SET @No = CONVERT( BIT, 0);

		SELECT cp.ContactPersonId as ContactPersonId, 
			   --cp.CustAccount as CustomerId,
			   cp.LastName as LastName, 
			   cp.FirstName as FirstName, 
			   cp.Email as Email, 
			   cp.CellularPhone as CellularPhone, 
			   cp.Phone as Phone, cp.PhoneLocal as PhoneLocal,
			   CONVERT( BIT, cp.ARUTRENDELHET ) as AllowOrder, -- arut rendelhet 
			   CONVERT( BIT, cp.ARUTATVEHET ) as AllowReceiptOfGoods, -- arut atvehet

			   CONVERT( BIT, cp.SMSAruBeerkezes ) as SmsArriveOfGoods, -- sms arubeerkezesrol
			   CONVERT( BIT, cp.SMSRendIg ) as SmsOrderConfirm, --sms rendeles visszaigazolasrol
			   CONVERT( BIT, cp.SMSAruKiszallitas ) as SmsOfDelivery, -- sms aru kiszallitasarol

			   CONVERT( BIT, cp.EmailAruBeerkezes ) as EmailArriveOfGoods, -- email az aru beerkezeserol
			   CONVERT( BIT, cp.EmailRendIg ) as EmailOfOrderConfirm, -- email a rendeles visszaigazolasarol
			   CONVERT( BIT, cp.EmailAruKiszallitas ) as EmailOfDelivery, -- email az aru kiszallitasarol
			   CONVERT( BIT, cp.WEBADMIN ) as WebAdmin,
			   CONVERT( BIT, cp.Arlist ) as PriceListDownload, 
			   CONVERT( BIT, cp.SzlaInfo ) as InvoiceInfo,
			   ISNULL( wsui.WEBLOGINNAME, '' ) as UserName, 
			   ISNULL( wsui.PWD, '' ) as [Password],
			   CONVERT( BIT, cp.LeftCompany ) as LeftCompany,
			   CASE WHEN ( ISNULL( ( SELECT TEMAID FROM AxDb.dbo.UPDHIRLEVELTEMA WHERE ContactPersonID = cp.ContactPersonId AND DataAreaID = @DataAreaId AND TemaID = 'Periféria' ), '' ) ) = '' THEN @No ELSE @Yes END as Newsletter
		FROM AxDb.dbo.ContactPerson as cp
			 LEFT OUTER JOIN AxDb.dbo.WebShopUserInfo as wsui ON wsui.ContactPersonID = cp.ContactPersonId AND wsui.DataAreaID = @VirtualDataAreaId
		WHERE cp.ContactPersonId = @ContactPersonId AND 
			  cp.DataAreaID = @VirtualDataAreaId;

	RETURN
GO
GRANT EXECUTE ON InternetUser.cms_ContactPerson TO InternetUser
GO

-- exec InternetUser.cms_ContactPerson 'CP100027', 'hrp';