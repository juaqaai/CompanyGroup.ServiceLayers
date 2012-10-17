USE WebDb_Test
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.cms_BankAccountList;
GO
CREATE PROCEDURE InternetUser.cms_BankAccountList( @CustomerId NVARCHAR(20), @DataAreaId NVARCHAR(3) = 'hrp' )
AS
SET NOCOUNT ON

	DECLARE @VirtualDataAreaId nvarchar(3);

	SET @VirtualDataAreaId = InternetUser.GetVirtualDataAreaId( @DataAreaId );

	SELECT AccountNum as Number, RecID as RecId
	FROM AxDb.dbo.CustBankAccount 
	WHERE CustAccount = @CustomerId AND DataAreaID = @VirtualDataAreaId; 
	
RETURN
GO
GRANT EXECUTE ON InternetUser.cms_BankAccountList TO InternetUser
GO
-- exec InternetUser.cms_BankAccountList 'V005884', 'hrp';