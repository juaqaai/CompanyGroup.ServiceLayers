USE [Web]
GO
/****** Object:  StoredProcedure [InternetUser].[cms_CompatibilityList]    Script Date: 2012.08.22. 23:04:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE InternetUser.CompatibilityList;
GO
CREATE PROCEDURE InternetUser.CompatibilityList( @DataAreaId nvarchar (3), 
												 @ProductId nvarchar (20), 
												 @Reverse BIT = 0	-- alkatresz jelolese : 0 = a termekazonositohoz tartozo alkatreszeket keressuk
																		--						1 = a termekazonositohoz mint alkatreszhez tartozo termekeket keressuk
													  )
AS
SET NOCOUNT ON
	IF ( @Reverse = 0 )	-- kellékanyag
		SELECT Compatibility.ProductId, Compatibility.DataAreaId, Compatibility.CompatibleProductId,  Compatibility.CompatibilityType
		FROM InternetUser.Compatibility as Compatibility WITH (READUNCOMMITTED)
		INNER JOIN InternetUser.Catalogue as Catalogue ON Compatibility.CompatibleProductId = Catalogue.ProductId
		WHERE Compatibility.DataAreaId = @DataAreaId AND Compatibility.ProductId = @ProductId;
	ELSE
		SELECT Compatibility.ProductId as CompatibleProductId, Compatibility.DataAreaId, Compatibility.CompatibleProductId as ProductId, Compatibility.CompatibilityType
		FROM InternetUser.Compatibility as Compatibility
		INNER JOIN InternetUser.Catalogue as Catalogue ON Compatibility.ProductId = Catalogue.ProductId
		WHERE Compatibility.DataAreaId = @DataAreaId AND CompatibleProductId = @ProductId;		
RETURN
GO
GRANT EXECUTE ON InternetUser.CompatibilityList TO InternetUser
GO

-- EXEC InternetUser.CompatibilityList 'hrp', 'KYFS1320D', 1;
-- EXEC InternetUser.CompatibilityList 'hrp', 'KYCB130', 1

/*
select * from InternetUser.Compatibility where ProductId = 'LFBKAH531-3'

select count(*), Tipus from axdb.dbo.updCompatib group by Tipus -- Kellékanyag, Opció
select * from axdb.dbo.updCompatib where Tipus = 'Kellékanyag'
select * from axdb.dbo.updCompatib where Tipus = 'Opció'

select * from axdb.dbo.updCompatib where ItemId = 'LFBKAH531-3'
TIPUS	COMPATITEMID	ITEMID	MODIFIEDDATE	MODIFIEDTIME	MODIFIEDBY	CREATEDDATE	CREATEDTIME	CREATEDBY	DATAAREAID	RECVERSION	RECID
Opció	K453-L100	LFBKAH531-3	2012-09-17 00:00:00.000	61909	KrKr1	2012-09-17 00:00:00.000	61909	KrKr1	hrp	1	5637184619
Opció	F7128-L500	LFBKAH531-3	2012-09-17 00:00:00.000	61840	KrKr1	2012-09-17 00:00:00.000	61840	KrKr1	hrp	1	5637184609
Opció	F7128-L400	LFBKAH531-3	2012-09-17 00:00:00.000	61845	KrKr1	2012-09-17 00:00:00.000	61845	KrKr1	hrp	1	5637184610
Opció	K433-L100	LFBKAH531-3	2012-09-17 00:00:00.000	61852	KrKr1	2012-09-17 00:00:00.000	61852	KrKr1	hrp	1	5637184611
Opció	K552-L411	LFBKAH531-3	2012-09-17 00:00:00.000	61857	KrKr1	2012-09-17 00:00:00.000	61857	KrKr1	hrp	1	5637184612
Opció	K450-L200	LFBKAH531-3	2012-09-17 00:00:00.000	61863	KrKr1	2012-09-17 00:00:00.000	61863	KrKr1	hrp	1	5637184613
Opció	K440-L100	LFBKAH531-3	2012-09-17 00:00:00.000	61869	KrKr1	2012-09-17 00:00:00.000	61869	KrKr1	hrp	1	5637184614
Opció	F2613-L600	LFBKAH531-3	2012-09-17 00:00:00.000	61874	KrKr1	2012-09-17 00:00:00.000	61874	KrKr1	hrp	1	5637184615
Opció	F1650-L550	LFBKAH531-3	2012-09-17 00:00:00.000	61881	KrKr1	2012-09-17 00:00:00.000	61881	KrKr1	hrp	1	5637184616
Opció	F1650-L200	LFBKAH531-3	2012-09-17 00:00:00.000	61886	KrKr1	2012-09-17 00:00:00.000	61886	KrKr1	hrp	1	5637184617
Opció	K438-L100	LFBKAH531-3	2012-09-17 00:00:00.000	61893	KrKr1	2012-09-17 00:00:00.000	61893	KrKr1	hrp	1	5637184618


*/