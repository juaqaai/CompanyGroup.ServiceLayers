USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [InternetUser].[cms_PurchaseOrderLine];
GO
CREATE PROCEDURE [InternetUser].[cms_PurchaseOrderLine](@DataAreaId nvarchar(3) = 'hrp') 
AS
SET NOCOUNT ON

	SELECT  
	--Purch.BrEngedelyezes,			-- Engedélyezés státusz, 2: Engedelyezve, 3: Nemkellengedelyezni
	Purch.ItemId, 
	CONVERT(INT, Purch.PurchQty) as PurchQty,					-- mennyiség
	Purch.DeliveryDate,				-- kért szállítási idõpont
	Purch.ConfirmedDlv,				-- visszaigazolva
	--Purch.PurchStatus,				-- sor állpota (1:nyitott rendelés, 2:fogadott, 3:szamlazva, 4:ervenytelenitve)
	CONVERT(INT, Purch.QtyOrdered) as QtyOrdered,				-- mennyiség
	CONVERT(INT, Purch.RemainInventPhysical) as RemainInventPhysical,		-- fennmaradó szállítása
	CONVERT(INT, Purch.RemainPurchPhysical) as RemainPurchPhysical,		-- fennmaradó szállítása
	--( SELECT SUM(Qty) FROM Axdb.dbo.InventTrans WHERE InventTransId = Purch.InventTransId AND StatusReceipt = 5 ) as Ordered,	-- rendelt
	Purch.DataAreaId
	FROM Axdb_20130131.dbo.PurchLine as Purch WITH (READUNCOMMITTED) 
	INNER JOIN Axdb_20130131.dbo.InventTable as Invent WITH (READUNCOMMITTED) ON Invent.ItemId = Purch.ItemId AND Invent.DataAreaId = Purch.DataAreaId
	WHERE Purch.ConfirmedDlv > '1900-01-01' AND 
		Purch.BrEngedelyezes = 2 AND			-- engedélyezés státusz, 2: Engedelyezve, 3: Nemkellengedelyezni
		Purch.PURCHASETYPE = 3 AND	
		Purch.PurchStatus IN (1, 2) AND	
		CONVERT(INT, Purch.RemainPurchPhysical) > 0 AND
		Invent.DataAreaID = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE Invent.DataAreaID END AND 
		Invent.DataAreaID <> CASE WHEN @DataAreaId <> '' THEN 'Axapta' ELSE 'srv' END AND 
		Invent.WEBARUHAZ = 1 AND 
		Invent.ITEMSTATE IN ( 0, 1 ) AND 
		Invent.AMOUNT1 > 0 AND
		Invent.AMOUNT2 > 0 AND
		Invent.AMOUNT3 > 0 AND
		Invent.AMOUNT4 > 0 AND
		Invent.AMOUNT5 > 0 	 		
RETURN

-- EXEC [InternetUser].[cms_PurchaseOrderLine];

-- select * FROM Axdb.dbo.InventTrans