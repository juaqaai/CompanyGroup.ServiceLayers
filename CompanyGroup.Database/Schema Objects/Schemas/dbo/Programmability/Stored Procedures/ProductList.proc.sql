USE WebDb_Test
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[ProductList]
GO
/*
visszaadja a webre kitehető cikktörzset
*/
CREATE PROCEDURE [InternetUser].[ProductList]
AS
SET NOCOUNT ON
	--vállalatkód ellenörzése
	--IF(@DataAreaId = '') 
	--	RETURN;

	--virtuális vállalatkód kalkuláció
	--DECLARE @VirtualDataAreaId VARCHAR(3);

	--SET @VirtualDataAreaId = CASE WHEN @DataAreaId = 'hrp' OR @DataAreaId = 'bsc' THEN 'hun' ELSE @DataAreaId END;

	--terméklista kilvasás
	SELECT DISTINCT '' as Id,									-- adatbázis egyedi azonosító
					Invent.ItemId as ProductId,					-- termékazonosító
					Invent.AXSTRUKTKOD AS AxStructCode,			-- struktúra azonosító
					GYARTOID as ManufacturerId,
					'' as ManufacturerName,
					'' as ManufacturerNameEnglish,
					JELLEG1ID as Category1Id, 
					'' as Category1Name, 
					'' as Category1NameEnglish,
					JELLEG2ID as Category2Id, 
					'' as Category2Name, 
					'' as Category2NameEnglish,
					JELLEG3ID as Category3Id, 
					'' as Category3Name, 
					'' as Category3NameEnglish,
					Invent.StandardConfigId as StandardConfigId,
					Invent.GYARTOICIKKSZAM as PartNumber, 
					Invent.ItemName, 
					'' as ItemNameEnglish, 
					0 as BscInnerStock,
					0 as BscOuterStock, 
					0 as BscSecondhandStock,
					0 as HrpInnerStock, 
					0 as HrpOuterStock, 
					0 as HrpSecondhandStock, 
					0 as SerbianStock, 
					Invent.AMOUNT1 as Price1,
					Invent.AMOUNT2 as Price2,
					Invent.AMOUNT3 as Price3,
					Invent.AMOUNT4 as Price4,
					Invent.AMOUNT5 as Price5,
					Invent.AMOUNT6 as Price6,
					'' as Currency,
					ISNULL( gt.MEGJEGYZES, '' ) as GarantyTime, 
					ISNULL( gm.MEGJEGYZES, '' ) as GarantyMode,
					CONVERT(DateTime, 0) as ShippingDate,				-- várt szállítási idő
					ItemState, 
					CONVERT( bit, AKCIOS ) as [Discount], 
					CONVERT( bit, LEERTEKELT ) as Bargain, 
					CASE WHEN DATEADD( day, 30, Invent.CREATEDDATE ) >=  GETDATE() THEN CONVERT( bit, 1 ) ELSE CONVERT( bit, 0 ) END as [New], -- 30 napnal nem regebbi volt beallitva a jelenlegi 5 helyett!
					CONVERT( bit, HetiTop10 ) as Special, 
				    '' as [Description], 
					'' as [DescriptionEnglish], 
				    Invent.TERMEKMENEDZSERID as ProductManagerId,
				    '' as ProductManagerName,
					'' as ProductManagerExtension,
				    '' as ProductManagerEmail,
					'' as ProductManagerMobile,
			        Invent.CREATEDDATE as CreatedDate, 
					Invent.CREATEDTIME as CreatedTime, 
				    Invent.ModifiedDate as ModifiedDate, 
					Invent.ModifiedTime as ModifiedTime, 
					Invent.DataAreaId as DataAreaId
	FROM AxDb.dbo.InventTable as Invent WITH (READUNCOMMITTED) 
    LEFT OUTER JOIN AxDb.dbo.UPDJOTALLASIDEJE as gt ON gt.UPDJOTALLASIDEJEID = Invent.UPDJOTALLASIDEJEID AND gt.DATAAREAID = Invent.DATAAREAID
	LEFT OUTER JOIN AxDb.dbo.UPDJOTALLASMODJA as gm ON gm.UPDJOTALLASMODJAID = Invent.UPDJOTALLASMODJAID AND gm.DATAAREAID = Invent.DATAAREAID	
	WHERE Invent.DataAreaID IN ('hrp', 'bsc', 'ser') AND 
		  Invent.WEBARUHAZ = 1 AND 
		  Invent.ITEMSTATE IN ( 0, 1 ) AND 
		  Invent.AMOUNT1 > 0 AND
		  Invent.AMOUNT2 > 0 AND
		  Invent.AMOUNT3 > 0 AND
		  Invent.AMOUNT4 > 0 AND
		  Invent.AMOUNT5 > 0 AND
		  Invent.AMOUNT6 > 0 AND
		  Invent.GyartoId <> ''
	ORDER BY GYARTOID, JELLEG1ID, JELLEG2ID, JELLEG3ID, Invent.ItemId, Invent.DataAreaId;
	RETURN;
GO
GRANT EXECUTE ON InternetUser.ProductList TO InternetUser
GO

-- exec InternetUser.ProductList;