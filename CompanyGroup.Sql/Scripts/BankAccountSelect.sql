USE ExtractInterface
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE InternetUser.BankAccountSelect;
GO
CREATE PROCEDURE InternetUser.BankAccountSelect( @CustomerId NVARCHAR(20), @DataAreaId NVARCHAR(3) = 'hrp' )
AS
SET NOCOUNT ON

	DECLARE @VirtualDataAreaId nvarchar(3);

	SET @VirtualDataAreaId = 'hun';

	SELECT AccountNum as Number, RecID as RecId
	FROM axdb_20120614.dbo.CustBankAccount 
	WHERE CustAccount = @CustomerId AND DataAreaID = @VirtualDataAreaId; 
	
RETURN
GO
GRANT EXECUTE ON InternetUser.BankAccountSelect TO InternetUser
GO
-- exec InternetUser.BankAccountSelect 'V005884', 'hrp';