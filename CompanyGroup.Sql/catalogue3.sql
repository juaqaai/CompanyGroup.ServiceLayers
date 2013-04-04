USE [InternetDb]
GO

/****** Object:  StoredProcedure [InternetUser].[web_GetProducts3]    Script Date: 2013.04.03. 12:41:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [InternetUser].[web_GetProducts3]( @iLogID int = 0,
											   @iDataAreaID int = 1, -- vallalatkod, 
											   @sCustomerID nvarchar(10) = '',	-- vasarlo azonosito
											   @sManufacturerID nvarchar (4) = '',	-- gyártó kód
 											   @sCategory1ID nvarchar (4) = '',       -- Jelleg1 kód
 											   @sCategory2ID nvarchar (4) = '',       -- Jelleg2 kód
											   @sCategory3ID nvarchar (4) = '',       -- Jelleg3 kód
											   @bAction bit = 0,      -- csak az akcios termekek / nem csak az akcios termekek 
											   @bBargainCounter bit = 0,     -- csak a leertekelt termekek / nem csak a leertekelt termekek 
											   @bTop10 bit = 0,       -- csak a TOP 10 termekek / nem csak a TOP 10 termekek
											   @bFocusWeek bit = 0,   -- csak a fokuszhet termekei / nem csak a fokuszhet termekei     
											   @bNew bit = 0,         -- csak az ujdonsag termekek / nem csak az ujdonsag termekek
											   @bStock bit = 0,     -- csak a keszleten levo termekek / nem csak a keszleten levo termekek
											   @iOrderBy int = 2,	-- 0: gyarto es jellegek novekvo, 1: gyarto es jellegek csokkeno
																	-- 2: azonosito novekvo, 3: azonosito csokkeno,
																	-- 4 nev novekvo, 5: nev csokkeno, 
																	-- 6: ar novekvo, 7 ar csokkeno
																	-- 8: jelleg1 + jelleg2 + jelleg3 + gyarto novekvo 9: jelleg1 + jelleg2 + jelleg3 + gyarto csokkeno
																	-- 10: jelleg2 + jelleg2 + jelleg1 + gyarto novekvo 11: jelleg2 + jelleg3 + jelleg1 + gyarto csokkeno
																	-- 12: jelleg3 + jelleg2 + jelleg1 + gyarto novekvo 13: jelleg3 + jelleg12 + jelleg1 + gyarto csokkeno
																	-- 14 : belsõ készlet növekvõ, 15 : belsõ készlet csökkenõ,
																	-- 16 : külsõ készlet növekvõ, 17 : külsõ készlet csökkenõ
																	-- 18 : készlet növekvõ, 19 : készlet csökkenõ
											   @sFindText nvarchar(64) = '', -- keresendo kifejezes
											   @iCurrentPageIndex INT = 1, -- lapozas
											   @iItemsOnPage INT = 30, 	-- lapozas
											   @iPropertySheetID INT = 0  -- összehasonlításhoz szükséges paraméter	
										    )
AS
SET NOCOUNT ON

	DECLARE @iCount INT, @iRequestLogID BIGINT, @sStoreConstInner VARCHAR(8), @sStoreConstOuter VARCHAR(8);

	DECLARE @sDataAreaID nvarchar(3);

	SET @sDataAreaID = InternetUser.web_GetDataAreaID( @iDataAreaID );

	SET @sStoreConstInner = CASE WHEN @iDataAreaID = 1 THEN 'BELSO' ELSE '1000' END; 
	SET @sStoreConstOuter = CASE WHEN @iDataAreaID = 1 THEN 'KULSO' ELSE '7000' END; 

	-- elemszámok kiolvasása 
	SELECT @iCount = COUNT(*)
	FROM InternetUser.web_Catalogue AS c WITH (READUNCOMMITTED)		
	WHERE c.sDataAreaID = @sDataAreaID AND 
			  c.sManufacturerID = CASE WHEN @sManufacturerID <> '' THEN @sManufacturerID ELSE c.sManufacturerID END AND 
			  c.sCategory1ID = CASE WHEN @sCategory1ID <> '' THEN @sCategory1ID ELSE c.sCategory1ID END AND 
			  c.sCategory2ID = CASE WHEN @sCategory2ID <> '' THEN @sCategory2ID ELSE c.sCategory2ID END AND 
			  c.sCategory3ID = CASE WHEN @sCategory3ID <> '' THEN @sCategory3ID ELSE c.sCategory3ID END AND
			  c.bAction = CASE WHEN @bAction = 1 THEN 1 ELSE c.bAction END AND  
			  c.bBargain = CASE WHEN @bBargainCounter = 1 THEN 1 ELSE c.bBargain END AND  
			  c.bTop10 = CASE WHEN @bTop10 = 1 THEN 1 ELSE c.bTop10 END AND  
			  c.bFocusWeek = CASE WHEN @bFocusWeek = 1 THEN 1 ELSE c.bFocusWeek END AND  
			  c.bNew = CASE WHEN @bNew = 1 THEN 1 ELSE c.bNew END AND  
			  c.bFocusWeek = CASE WHEN @bFocusWeek = 1 THEN 1 ELSE c.bFocusWeek END AND 
			  c.bValid = 1 AND 
--			  c.iPropertySheetID = CASE WHEN ( @iPropertySheetID > 0 ) THEN @iPropertySheetID ELSE c.iPropertySheetID END AND	
			  1 = CASE WHEN c.iItemState = 1 THEN InternetUser.web_IsGreatherThenZero( c.iInnerStock, c.iOuterStock ) ELSE 1 END AND 
			  @bStock = CASE WHEN @bStock = 1 THEN InternetUser.web_IsGreatherThenZero( c.iInnerStock, c.iOuterStock ) ELSE @bStock END AND
			  ( c.sName LIKE CASE WHEN @sFindText <> ''THEN '%' + @sFindText + '%' ELSE c.sName END OR 
				c.sProductID LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sProductID END OR 
				c.sPartNumber LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sPartNumber END OR 
				c.sDescription LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sDescription END OR 
				c.sManufacturerName LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sManufacturerName END OR 
				c.sCategory1Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory1Name END OR 
				c.sCategory2Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory2Name END OR 
				c.sCategory3Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory3Name END
			  )

	DECLARE @iMaxItemCount INT, @iLastPageIndex INT;

	SET @iMaxItemCount = 31;
    SET @iLastPageIndex = 0;

	--@iLastPageIndex kiszamitasa 
              
	IF ( @iItemsOnPage > 0 ) 
	BEGIN
		SET @iLastPageIndex = ( @iCount / @iItemsOnPage );
	END

	IF ( ( @iLastPageIndex * @iItemsOnPage ) < @iCount )
	BEGIN
		SET @iLastPageIndex = @iLastPageIndex + 1;
	END
	
	-- @iMaxItemCount kiszamitasa 

    IF ( @iCurrentPageIndex < @iLastPageIndex )
    BEGIN
		SET @iMaxItemCount = ( @iCurrentPageIndex * @iItemsOnPage ) + 1;
	END
	ELSE	-- utolso lapon allunk 
	BEGIN
		IF ( @iCurrentPageIndex > 1 )
		BEGIN
			SET @iMaxItemCount = @iCount + 1; 
			SET @iItemsOnPage = @iMaxItemCount - ( ( @iCurrentPageIndex - 1 ) * @iItemsOnPage ) - 1;
		END
		ELSE  -- csak egy lap van
		BEGIN
			SET @iMaxItemCount = @iCount + 1;
			SET @iItemsOnPage = @iCount;
		END
    END

	IF ( @iOrderBy IN ( 0, 2, 4, 6, 8, 10, 12 ) ) 
	BEGIN
		WITH ProdListBySequenceID AS
		( 
			SELECT DISTINCT TOP( @iMaxItemCount ) c.sProductID AS [sID], 
						c.sAxStructCode AS [sAxStructCode], 
						c.sPartNumber, 
						c.sName, 
				   InternetUser.web_ProdPriceSelector( @sCustomerID, c.sProductID, ISNULL(pg.iPriceGroupId, 0), ISNULL(s.iPrice, 0), c.iPrice1, c.iPrice2, c.iPrice3, c.iPrice4, c.iPrice5, c.iPrice6 ) AS iPrice, 
--							CASE WHEN ( @sCustomerID <> '' ) THEN InternetUser.web_ProdStockAvailable( @sDataAreaID, c.sProductID, 'BELSO' ) ELSE 0 END AS iInnerStock, 
--							CASE WHEN ( @sCustomerID <> '' ) THEN InternetUser.web_ProdStockAvailable( @sDataAreaID, c.sProductID, 'KULSO' ) ELSE 0 END AS iOuterStock, 
						c.iInnerStock as iInStock, c.iOuterStock as iOutStock,
						c.sGaranty as sGarantyTime, c.sGarantyMode,
						c.sDescription, 
						c.sManufacturerID, c.sManufacturerName, 
						c.sCategory1ID, c.sCategory1Name, 
						c.sCategory2ID, c.sCategory2Name, 
						c.sCategory3ID, c.sCategory3Name, 
						c.bAction, c.bBargain AS [bBargainCounter], c.bTop10, c.bFocusWeek, c.bNew, 
--				   InternetUser.web_IsGreatherThenZero( iInnerStock, iOuterStock ) AS [bStock],
					   c.iItemState, 
					   c.bPictureExist, 
					   c.dtCreated, c.dtStockUpdated, c.dtPriceUpdated,
					   c.iPropertySheetID, 
					   CASE WHEN ( c.iPropertySheetID = @iPropertySheetID AND @iPropertySheetID > 0 ) OR ( @iPropertySheetID = 0 AND c.iPropertySheetID > 0 ) THEN 1 ELSE 0 END as bAllowCompare, 
			   ROW_NUMBER() OVER ( ORDER BY 
								   CASE WHEN @iOrderBy = 0 THEN c.sManufacturerName + c.sCategory1Name + c.sCategory2Name + c.sCategory3Name 
										WHEN @iOrderBy = 2 THEN c.sProductID + c.sManufacturerName + c.sCategory1Name + c.sCategory2Name + c.sCategory3Name
										WHEN @iOrderBy = 4 THEN c.sName + c.sManufacturerName + c.sCategory1Name + c.sCategory2Name + c.sCategory3Name 
										WHEN @iOrderBy = 6 THEN InternetUser.web_ProdPriceOrder( InternetUser.web_ProdPriceSelector( @sCustomerID, c.sProductID, ISNULL( pg.iPriceGroupID, 0 ), ISNULL( s.iPrice, 0 ), c.iPrice1, c.iPrice2, c.iPrice3, c.iPrice4, c.iPrice5, c.iPrice6 ) )
										WHEN @iOrderBy = 8 THEN c.sCategory1Name + c.sManufacturerName + c.sCategory2Name + c.sCategory3Name
										WHEN @iOrderBy = 10 THEN c.sCategory2Name + c.sManufacturerName + c.sCategory1Name + c.sCategory3Name
										WHEN @iOrderBy = 12 THEN c.sCategory3Name + c.sManufacturerName + c.sCategory1Name + c.sCategory2Name   
										WHEN @iOrderBy = 18 THEN CONVERT( VARCHAR(8), CONVERT( INT, c.iInnerStock + c.iOuterStock ) )
										ELSE c.sProductID END ASC ) AS ProductSequenceIDRank
		FROM InternetUser.web_Catalogue AS c WITH (READUNCOMMITTED)
		LEFT JOIN InternetUser.CustomerPriceGroup AS pg WITH (READUNCOMMITTED) ON c.sProductID = pg.sProductID AND pg.sCustomerID = @sCustomerID
		LEFT JOIN InternetUser.SpecialPrice AS s WITH (READUNCOMMITTED) ON c.sProductID = s.sProductID AND s.sCustomerID = @sCustomerID
		WHERE  
			  c.sManufacturerID = CASE WHEN @sManufacturerID <> '' THEN @sManufacturerID ELSE c.sManufacturerID END AND 
			  c.sCategory1ID = CASE WHEN @sCategory1ID <> '' THEN @sCategory1ID ELSE c.sCategory1ID END AND 
			  c.sCategory2ID = CASE WHEN @sCategory2ID <> '' THEN @sCategory2ID ELSE c.sCategory2ID END AND 
			  c.sCategory3ID = CASE WHEN @sCategory3ID <> '' THEN @sCategory3ID ELSE c.sCategory3ID END AND
			  c.bAction = CASE WHEN @bAction = 1 THEN 1 ELSE c.bAction END AND  
			  c.bBargain = CASE WHEN @bBargainCounter = 1 THEN 1 ELSE c.bBargain END AND  
			  c.bTop10 = CASE WHEN @bTop10 = 1 THEN 1 ELSE c.bTop10 END AND  
			  c.bFocusWeek = CASE WHEN @bFocusWeek = 1 THEN 1 ELSE c.bFocusWeek END AND  
			  c.bNew = CASE WHEN @bNew = 1 THEN 1 ELSE c.bNew END AND  
			  c.bValid = 1 AND
--			  c.iPropertySheetID = CASE WHEN ( @iPropertySheetID > 0 ) THEN @iPropertySheetID ELSE c.iPropertySheetID END AND	
			  1 = CASE WHEN c.iItemState = 1 THEN InternetUser.web_IsGreatherThenZero( c.iInnerStock, c.iOuterStock ) ELSE 1 END AND 
			  @bStock = CASE WHEN @bStock = 1 THEN InternetUser.web_IsGreatherThenZero( c.iInnerStock, c.iOuterStock ) ELSE @bStock END AND
--InternetUser.web_IsProdStockAvailable( @sDataAreaID, c.sProductID, '' )
			  c.sDataAreaID = @sDataAreaID AND 
			  ( c.sName LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sName END OR 
				c.sProductID LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sProductID END OR 
				c.sPartNumber LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sPartNumber END OR
				c.sDescription LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sDescription END OR
				c.sManufacturerName LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sManufacturerName END OR
				c.sCategory1Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory1Name END OR
				c.sCategory2Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory2Name END OR 
				c.sCategory3Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory3Name END
			  )
		)
		SELECT *, --CASE WHEN ( @sCustomerID <> '' ) THEN CONVERT( INT, InternetUser.web_ProdPriceFinder2( @sDataAreaID, @sCustomerID, sID, sAxStructCode, 1 ) ) ELSE 0 END AS [iPrice], 
				  CASE WHEN ( @sCustomerID <> '' ) THEN iInStock ELSE 0 END AS iInnerStock, --InternetUser.web_ProdStockAvailable( @sDataAreaID, sID, @sStoreConstInner )
				  CASE WHEN ( @sCustomerID <> '' ) THEN iOutStock ELSE 0 END AS iOuterStock, -- InternetUser.web_ProdStockAvailable( @sDataAreaID, sID, @sStoreConstOuter ) 
			   InternetUser.web_IsGreatherThenZero( iInStock, iOutStock ) AS [bStock] 
		FROM ProdListBySequenceID
		WHERE ProductSequenceIDRank BETWEEN @iMaxItemCount - @iItemsOnPage AND @iMaxItemCount - 1
		ORDER BY ProductSequenceIDRank;
	END
	ELSE	--@iOrderBy IN ( 1, 3, 5, 7, 9, 11, 13 )
	BEGIN
		WITH ProdListBySequenceID AS
		( SELECT DISTINCT TOP( @iMaxItemCount ) c.sProductID AS [sID], 
					c.sAxStructCode AS [sAxStructCode], 
					c.sPartNumber, 
					c.sName, 
				   InternetUser.web_ProdPriceSelector( @sCustomerID, c.sProductID, ISNULL(pg.iPriceGroupId, 0), ISNULL(s.iPrice, 0), c.iPrice1, c.iPrice2, c.iPrice3, c.iPrice4, c.iPrice5, c.iPrice6 ) AS iPrice, 
--						CASE WHEN ( @sCustomerID <> '' ) THEN InternetUser.web_ProdStockAvailable( @sDataAreaID, c.sProductID, 'BELSO' ) ELSE 0 END AS iInnerStock, 
--						CASE WHEN ( @sCustomerID <> '' ) THEN InternetUser.web_ProdStockAvailable( @sDataAreaID, c.sProductID, 'KULSO' ) ELSE 0 END AS iOuterStock, 				   
					c.iInnerStock as iInStock, c.iOuterStock as iOutStock,
					c.sGaranty as sGarantyTime, c.sGarantyMode,
					c.sDescription, 
					c.sManufacturerID, c.sManufacturerName, 
					c.sCategory1ID, c.sCategory1Name, 
					c.sCategory2ID, c.sCategory2Name, 
					c.sCategory3ID, c.sCategory3Name, 
					c.bAction, c.bBargain AS [bBargainCounter], c.bTop10, c.bFocusWeek, c.bNew, 
--				   ( CASE WHEN c.iInnerStock + c.iOuterStock > 0 THEN 1 ELSE 0 END ) AS [bStock],
					c.iItemState, 
					c.bPictureExist, 
					c.dtCreated, c.dtStockUpdated, c.dtPriceUpdated,	
				    c.iPropertySheetID, 
				    CASE WHEN ( c.iPropertySheetID = @iPropertySheetID AND @iPropertySheetID > 0 ) OR ( @iPropertySheetID = 0 AND c.iPropertySheetID > 0 ) THEN 1 ELSE 0 END as bAllowCompare, 
			   ROW_NUMBER() OVER ( ORDER BY 
								   CASE WHEN @iOrderBy = 1 THEN c.sManufacturerName + c.sCategory1Name + c.sCategory2Name + c.sCategory3Name 
										WHEN @iOrderBy = 3 THEN c.sProductID + c.sManufacturerName + c.sCategory1Name + c.sCategory2Name + c.sCategory3Name 
										WHEN @iOrderBy = 5 THEN c.sName + c.sManufacturerName + c.sCategory1Name + c.sCategory2Name + c.sCategory3Name
										WHEN @iOrderBy = 7 THEN InternetUser.web_ProdPriceOrder( InternetUser.web_ProdPriceSelector( @sCustomerID, c.sProductID, ISNULL( pg.iPriceGroupID, 0 ), ISNULL( s.iPrice, 0 ), c.iPrice1, c.iPrice2, c.iPrice3, c.iPrice4, c.iPrice5, c.iPrice6 ) )
										WHEN @iOrderBy = 9 THEN c.sCategory1Name + c.sManufacturerName + c.sCategory2Name + c.sCategory3Name
										WHEN @iOrderBy = 11 THEN c.sCategory2Name + c.sManufacturerName + c.sCategory1Name + c.sCategory3Name 
										WHEN @iOrderBy = 13 THEN c.sCategory3Name + c.sManufacturerName + c.sCategory1Name + c.sCategory2Name 
										WHEN @iOrderBy = 19 THEN CONVERT( VARCHAR(8), CONVERT( INT, c.iInnerStock + c.iOuterStock ) )
										ELSE c.sProductID END DESC ) AS ProductSequenceIDRank
		FROM InternetUser.web_Catalogue AS c WITH (READUNCOMMITTED)
		LEFT JOIN InternetUser.CustomerPriceGroup AS pg WITH (READUNCOMMITTED) ON c.sProductID = pg.sProductID AND pg.sCustomerID = @sCustomerID
		LEFT JOIN InternetUser.SpecialPrice AS s WITH (READUNCOMMITTED) ON c.sProductID = s.sProductID AND s.sCustomerID = @sCustomerID
		WHERE c.sDataAreaID = @sDataAreaID AND 
			  c.sManufacturerID = CASE WHEN @sManufacturerID <> '' THEN @sManufacturerID ELSE c.sManufacturerID END AND 
			  c.sCategory1ID = CASE WHEN @sCategory1ID <> '' THEN @sCategory1ID ELSE c.sCategory1ID END AND 
			  c.sCategory2ID = CASE WHEN @sCategory2ID <> '' THEN @sCategory2ID ELSE c.sCategory2ID END AND 
			  c.sCategory3ID = CASE WHEN @sCategory3ID <> '' THEN @sCategory3ID ELSE c.sCategory3ID END AND
			  c.bAction = CASE WHEN @bAction = 1 THEN 1 ELSE c.bAction END AND  
			  c.bBargain = CASE WHEN @bBargainCounter = 1 THEN 1 ELSE c.bBargain END AND  
			  c.bTop10 = CASE WHEN @bTop10 = 1 THEN 1 ELSE c.bTop10 END AND  
			  c.bFocusWeek = CASE WHEN @bFocusWeek = 1 THEN 1 ELSE c.bFocusWeek END AND  
			  c.bNew = CASE WHEN @bNew = 1 THEN 1 ELSE c.bNew END AND  
			  c.bFocusWeek = CASE WHEN @bFocusWeek = 1 THEN 1 ELSE c.bFocusWeek END AND 
			  c.bValid = 1 AND 
--			  c.iPropertySheetID = CASE WHEN ( @iPropertySheetID > 0 ) THEN @iPropertySheetID ELSE c.iPropertySheetID END AND	
			  1 = CASE WHEN c.iItemState = 1 THEN InternetUser.web_IsGreatherThenZero( c.iInnerStock, c.iOuterStock ) ELSE 1 END AND 
			  @bStock = CASE WHEN @bStock = 1 THEN InternetUser.web_IsGreatherThenZero( c.iInnerStock, c.iOuterStock ) ELSE @bStock END AND
--InternetUser.web_IsProdStockAvailable( @sDataAreaID, c.sProductID, '' )
			  ( c.sName LIKE CASE WHEN @sFindText <> ''THEN '%' + @sFindText + '%' ELSE c.sName END OR 
				c.sProductID LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sProductID END OR 
				c.sPartNumber LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sPartNumber END OR 
				c.sDescription LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sDescription END OR 
				c.sManufacturerName LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sManufacturerName END OR 
				c.sCategory1Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory1Name END OR 
				c.sCategory2Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory2Name END OR 
				c.sCategory3Name LIKE CASE WHEN @sFindText <> '' THEN '%' + @sFindText + '%' ELSE c.sCategory3Name END
			  )
		)
		SELECT *, -- CASE WHEN ( @sCustomerID <> '' ) THEN CONVERT( INT, InternetUser.web_ProdPriceFinder2( @sDataAreaID, @sCustomerID, sID, sAxStructCode, 1 ) ) ELSE 0 END AS [iPrice],  
				  CASE WHEN ( @sCustomerID <> '' ) THEN iInStock ELSE 0 END AS iInnerStock, -- InternetUser.web_ProdStockAvailable( @sDataAreaID, sID, @sStoreConstInner )
				  CASE WHEN ( @sCustomerID <> '' ) THEN iOutStock ELSE 0 END AS iOuterStock, -- InternetUser.web_ProdStockAvailable( @sDataAreaID, sID, @sStoreConstOuter )				   
				  InternetUser.web_IsGreatherThenZero( iInStock, iOutStock ) AS [bStock] 
--InternetUser.web_IsProdStockAvailable( @sDataAreaID, c.sProductID, '' )
		FROM ProdListBySequenceID
		WHERE ProductSequenceIDRank BETWEEN @iMaxItemCount - @iItemsOnPage AND @iMaxItemCount - 1
		ORDER BY ProductSequenceIDRank;
	END

	SELECT @iCount as iCount;

	--loggolas
	INSERT INTO InternetUser.web_CatalogueRequestLog ( iLogID, sDataAreaID, sManufacturerID, sCategory1ID, sCategory2ID, sCategory3ID, 
													   bAction, bBargain, bTop10, bFocusWeek, bNew, bStock, sFindText, iCurrentPageIndex, 
													   iItemsOnPage, iOrderBy ) 
											  VALUES ( @iLogID, @sDataAreaID, @sManufacturerID, @sCategory1ID, @sCategory2ID, @sCategory3ID, 
													   @bAction, @bBargainCounter, @bTop10, @bFocusWeek, @bNew, @bStock, @sFindText, @iCurrentPageIndex, 
													   @iItemsOnPage, @iOrderBy );

	SET @iRequestLogID = SCOPE_IDENTITY();

	SELECT @iRequestLogID as iRequestLogID;

RETURN


GO

-- select count(*) from InternetUser.web_CatalogueRequestLog;
-- truncate table InternetUser.web_CatalogueRequestLog;
-- select * from InternetUser.CustomerPriceGroup;


