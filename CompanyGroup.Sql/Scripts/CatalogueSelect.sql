/*
DECLARE @XMLDoc XML = '
<Structure>
<Manufacturer>
    <Id>A004</Id>
    <Id>A006</Id>
</Manufacturer>
<Category1>
    <Id>C004</Id>
    <Id>C006</Id>
</Category1>
<Category2>
    <Id>D004</Id>
    <Id>D006</Id>
</Category2>
<Category3>
    <Id>E004</Id>
    <Id>E006</Id>
</Category3>
</Structure>';

SELECT CONVERT(nvarchar(4), Manufacturer.Id.query('./text()')) as ManufacturerId
FROM @XMLDoc.nodes('/Structure/Manufacturer/Id') as Manufacturer(Id)

SELECT @XMLDoc.query('/Structure/Manufacturer/Id/text()');

DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT, @XMLDoc

        SELECT * FROM OPENXML (@handle, '/Structure/Manufacturer', 2) WITH (Id varchar(4));
		 
EXEC sp_xml_removedocument @handle

SELECT Category1Id, Category1Name FROM InternetUser.Catalogue 
where ManufacturerName = 'Panasonic' 
group by Category1Id, Category1Name

Category1Name
Fényképezõgép, kamera
Irodatechnika
Monitor
Multifunkciós készülék
Szórakoztató elektronika

*/

USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Create a table type. */
DROP TYPE CustomerPriceGroupTableType;
GO
CREATE TYPE CustomerPriceGroupTableType AS TABLE 
( 
	PriceGroupId NVARCHAR(4),
	StructureCode NVARCHAR(16),									
	Sequence INT, 
	DataAreaId NVARCHAR(3)
);
GO

-- cikkek lista
DROP PROCEDURE [InternetUser].[CatalogueSelect2];
GO
CREATE PROCEDURE [InternetUser].[CatalogueSelect2] (@VisitorKey int = 0,
													@DataAreaId nvarchar(4) = 'hrp',
												   @Manufacturers nvarchar (250) = '',
												   @Category1 nvarchar (250) = '',
												   @Category2 nvarchar (250) = '',
												   @Category3 nvarchar (250) = '',	       
												   @Discount bit = 0,      
												   @SecondHand bit = 0,     
												   @New bit = 0,         
												   @Stock bit = 0,     
												   @Sequence int = 0,	
												   @FindText nvarchar(64) = '', 
												   @PriceFilter nvarchar(16) = '',
												   @PriceFilterRelation INT = 0,	
												   @CurrentPageIndex INT = 1, 
												   @ItemsOnPage INT = 30, 
												   @ExcludedItems nvarchar (250) = ''  
)
AS
SET NOCOUNT ON

	IF ISNULL(@FindText, '') = '' 
		SET @FindText = '""';

	DECLARE @CustomerPriceGroup AS CustomerPriceGroupTableType;

	INSERT INTO @CustomerPriceGroup (PriceGroupId, StructureCode, Sequence, DataAreaId)
	SELECT PriceGroupId, 
		   CONCAT( CASE WHEN ManufacturerId = '' THEN '____' ELSE ManufacturerId END,  
		   CASE WHEN Category1Id = '' THEN '____' ELSE Category1Id END, 
		   CASE WHEN Category2Id = '' THEN '____' ELSE Category2Id END, 
		   CASE WHEN Category3Id = '' THEN '____' ELSE Category3Id END ) , 
		   [Order], 
		   DataAreaId  
	FROM InternetUser.CustomerPriceGroup
	WHERE VisitorId = @VisitorKey;

	-- DECLARE @Xml Xml = CONVERT(Xml, @StructureXml);
	DECLARE @Separator varchar(1) = ',';

	WITH Manufacturers_CTE (Mi, Mj)
	AS (
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Manufacturers)
		UNION ALL
		SELECT CONVERT(INT, Mj) + 1, CHARINDEX(@Separator, @Manufacturers, CONVERT(INT, Mj) + 1)
		FROM Manufacturers_CTE
		WHERE Mj > 0
		--SELECT CONVERT(nvarchar(4), Manufacturer.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Manufacturer/Id') as Manufacturer(Id)
	),
	Category1_CTE(C1i, C1j)
	AS(
		--SELECT CONVERT(nvarchar(4), Category1.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Category1/Id') as Category1(Id)
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Category1)
		UNION ALL
		SELECT CONVERT(INT, C1j) + 1, CHARINDEX(@Separator, @Category1, CONVERT(INT, C1j) + 1)
		FROM Category1_CTE
		WHERE C1j > 0
	),
	Category2_CTE(C2i, C2j)
	AS(
		--SELECT CONVERT(nvarchar(4), Category2.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Category2/Id') as Category2(Id)
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Category2)
		UNION ALL
		SELECT CONVERT(INT, C2j) + 1, CHARINDEX(@Separator, @Category2, CONVERT(INT, C2j) + 1)
		FROM Category2_CTE
		WHERE C2j > 0
	),
	Category3_CTE(C3i, C3j)
	AS(
		--SELECT CONVERT(nvarchar(4), Category3.Id.query('./text()'))
		--FROM @Xml.nodes('/Structure/Category3/Id') as Category3(Id)
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @Category3)
		UNION ALL
		SELECT CONVERT(INT, C3j) + 1, CHARINDEX(@Separator, @Category3, CONVERT(INT, C3j) + 1)
		FROM Category3_CTE
		WHERE C3j > 0
	),
	ExcludedItems_CTE(Excudedi, Excludedj)
	AS(
		SELECT CONVERT(INT, 0), CHARINDEX( @Separator, @ExcludedItems)
		UNION ALL
		SELECT CONVERT(INT, Excludedj) + 1, CHARINDEX(@Separator, @ExcludedItems, CONVERT(INT, Excludedj) + 1)
		FROM ExcludedItems_CTE
		WHERE Excludedj > 0
	)

	SELECT Catalogue.Id, Catalogue.ProductId, AxStructCode,	Catalogue.DataAreaId, StandardConfigId, Name, EnglishName, PartNumber, ManufacturerId, ManufacturerName, ManufacturerEnglishName,	
			Category1Id, Category1Name, Category1EnglishName, Category2Id, Category2Name, Category2EnglishName, Category3Id, Category3Name, Category3EnglishName, 
			Stock, AverageInventory, 
			Price1, Price2, Price3, Price4, Price5, InternetUser.CalculateCustomerPrice(AxStructCode, ISNULL(P.Price, 0), Price1, Price2, Price3, Price4, Price5, Catalogue.DataAreaId, @CustomerPriceGroup) as CustomerPrice, 
			Garanty, GarantyMode, 
			Discount, New, ItemState, [Description], EnglishDescription, ProductManagerId, ShippingDate, IsPurchaseOrdered, CreatedDate, Updated, Available, PictureId, SecondHand, Valid
	FROM InternetUser.Catalogue as Catalogue
	LEFT OUTER JOIN InternetUser.CustomerSpecialPrice as P ON Catalogue.ProductId = P.ProductId AND Catalogue.DataAreaId = P.DataAreaId AND P.VisitorId = @VisitorKey
	--LEFT OUTER JOIN Manufacturers_CTE as Manufacturers ON Catalogue.ManufacturerId = Manufacturers.Id
	--LEFT OUTER JOIN Category1_CTE as Category1 ON Catalogue.Category1Id = Category1.Id
	--LEFT OUTER JOIN Category2_CTE as Category2 ON Catalogue.Category2Id = Category2.Id
	--LEFT OUTER JOIN Category3_CTE as Category3 ON Catalogue.Category3Id = Category3.Id
	WHERE (Catalogue.ManufacturerId IN (SELECT SUBSTRING(@Manufacturers, Mi, COALESCE(NULLIF(Mj, 0), LEN(@Manufacturers) + 1) - Mi) FROM Manufacturers_CTE) OR (@Manufacturers = '')) AND
		  (Catalogue.Category1Id IN (SELECT SUBSTRING(@Category1, C1i, COALESCE(NULLIF(C1j, 0), LEN(@Category1) + 1) - C1i) FROM Category1_CTE) OR (@Category1 = '')) AND
	      (Catalogue.Category2Id IN (SELECT SUBSTRING(@Category2, C2i, COALESCE(NULLIF(C2j, 0), LEN(@Category2) + 1) - C2i) FROM Category2_CTE) OR (@Category2 = '')) AND
		  (Catalogue.Category3Id IN (SELECT SUBSTRING(@Category3, C3i, COALESCE(NULLIF(C3j, 0), LEN(@Category3) + 1) - C3i) FROM Category3_CTE) OR (@Category3 = '')) AND

		  (Catalogue.ProductId NOT IN (SELECT SUBSTRING(@ExcludedItems, Excudedi, COALESCE(NULLIF(Excludedj, 0), LEN(@ExcludedItems) + 1) - Excudedi) FROM ExcludedItems_CTE) OR (@ExcludedItems = '')) AND

 		  Discount = CASE WHEN @Discount = 1 THEN 1 ELSE Discount END AND  
		  SecondHand = CASE WHEN @SecondHand = 1 THEN 1 ELSE SecondHand END AND 
		  New = CASE WHEN @New = 1 THEN 1 ELSE New END AND  
		  Valid = 1 AND
		  1 = CASE WHEN ItemState = 1 AND Stock <= 0 THEN 0 ELSE 1 END AND 
		  1 = CASE WHEN @Stock = 1 AND Stock <= 0 THEN 0 ELSE 1 END AND
		  1 = CASE WHEN @PriceFilterRelation = 1 AND Price5 < CONVERT(INT, @PriceFilter) THEN 0 ELSE 1 END AND
		  1 = CASE WHEN @PriceFilterRelation = 2 AND Price5 > CONVERT(INT, @PriceFilter) THEN 0 ELSE 1 END AND
		  Catalogue.DataAreaId = CASE WHEN @DataAreaId <> '' THEN @DataAreaId ELSE Catalogue.DataAreaId END AND 
		  -- SearchContent LIKE CASE WHEN @FindText <> '' THEN '%' + @FindText + '%' ELSE SearchContent END
		  1 = CASE WHEN @FindText = '""' OR CONTAINS(SearchContent, @FindText) THEN 1 ELSE 0 END

	ORDER BY 
	CASE WHEN @Sequence =  0 THEN --átlagos életkor csökkenõ, akciós csökkenõ, gyártó növekvõ, termékazonosító szerint növekvõleg,
		Sequence0 END ASC,
	CASE WHEN @Sequence =  1 THEN -- átlagos életkor növekvõ, akciós csökkenõ, gyártó növekvõ, termékazonosító szerint növekvõleg,
		Sequence0 END DESC, 
	CASE WHEN @Sequence =  2 THEN -- azonosito novekvo
		Catalogue.ProductId END ASC,
	CASE WHEN @Sequence = 3 THEN -- azonosito csokkeno 
		Catalogue.ProductId END DESC, 
	CASE WHEN @Sequence = 4 THEN  -- nev szerint novekvo
		Name END DESC,
	CASE WHEN @Sequence = 5 THEN  -- nev szerint csokkeno
		Name END ASC, 
	CASE WHEN @Sequence = 6 THEN  
		InternetUser.CalculateCustomerPrice(AxStructCode, ISNULL(P.Price, 0), Price1, Price2, Price3, Price4, Price5, Catalogue.DataAreaId, @CustomerPriceGroup) END ASC,
	CASE WHEN @Sequence = 7 THEN  
		InternetUser.CalculateCustomerPrice(AxStructCode, ISNULL(P.Price, 0), Price1, Price2, Price3, Price4, Price5, Catalogue.DataAreaId, @CustomerPriceGroup) END DESC, 
	CASE WHEN @Sequence = 8 THEN   
		Sequence1 END ASC, 
	CASE WHEN @Sequence = 9 THEN    
		Sequence1 END DESC, 
	CASE WHEN @Sequence = 12 THEN  
		Garanty END ASC, 
	CASE WHEN @Sequence = 13 THEN  
		Garanty END DESC
	OFFSET (@CurrentPageIndex-1) * @ItemsOnPage ROWS
	FETCH NEXT @ItemsOnPage ROWS ONLY;
			
RETURN
GO
GRANT EXECUTE ON InternetUser.CatalogueSelect2 TO InternetUser
/*
EXEC InternetUser.CatalogueSelect2 @VisitorKey = 44826,
								  @DataAreaId = '',		
								  @Manufacturers = 'A075,A142',			-- A098,A142
								  @Category1 = '',				--  B004,B021									      
								  @Category2 = '',
								  @Category3 = '',
								  @Discount = 0,      
								  @SecondHand = 0,     
								  @New = 0,         
								  @Stock = 0,     
								  @Sequence = 6,	
								  @FindText = '',			-- "ASUS*"
								  @PriceFilter = '0',
								  @PriceFilterRelation = 0,	
								  @CurrentPageIndex = 1, 
								  @ItemsOnPage = 50

EXEC InternetUser.CatalogueSelect2 @Sequence = 7

update InternetUser.Catalogue set New = 1 WHERE Description like '%new%' Category1Id = 'B011'
*/

GO
DROP FUNCTION InternetUser.CalculateCustomerPrice;
GO
CREATE FUNCTION InternetUser.CalculateCustomerPrice(@AxStructCode nvarchar(16), 
														 @SpecialPrice INT, 
														 @Price1 INT, 
														 @Price2 INT, 
														 @Price3 INT, 
														 @Price4 INT, 
														 @Price5 INT, 
														 @DataAreaId nvarchar(3), 
														 @CustomerPriceGroup CustomerPriceGroupTableType READONLY)
RETURNS INT
AS
BEGIN

	IF (@SpecialPrice > 0)
	BEGIN
		RETURN @SpecialPrice;
	END

	DECLARE @Result nvarchar(4);

	SET @Result = ISNULL((SELECT TOP 1 PriceGroupId 
						  FROM @CustomerPriceGroup 
						  WHERE @AxStructCode LIKE StructureCode  
								AND DataAreaId = @DataAreaId
						  ORDER BY Sequence), '2');

	RETURN CASE WHEN @Result = '1' THEN @Price1
				WHEN @Result = '2' THEN @Price2
				WHEN @Result = '3' THEN @Price3
				WHEN @Result = '4' THEN @Price4
				WHEN @Result = '5' THEN @Price5
			ELSE @Price2 END;
END
GO
GRANT EXECUTE ON InternetUser.CalculateCustomerPrice TO InternetUser
GO

/*
	DECLARE @CustomerPriceGroup AS CustomerPriceGroupTableType;

	INSERT INTO @CustomerPriceGroup (PriceGroupId, StructureCode, Sequence, DataAreaId)
	SELECT PriceGroupId, 
		   CONCAT( CASE WHEN ManufacturerId = '' THEN '____' ELSE ManufacturerId END,  
		   CASE WHEN Category1Id = '' THEN '____' ELSE Category1Id END, 
		   CASE WHEN Category2Id = '' THEN '____' ELSE Category2Id END, 
		   CASE WHEN Category3Id = '' THEN '____' ELSE Category3Id END ) as StructureCode, 
		   [Order], 
		   DataAreaId  
	FROM InternetUser.CustomerPriceGroup
	WHERE VisitorId = 44826;

	SELECT TOP 1 * FROM @CustomerPriceGroup 
	WHERE  'A075____________' LIKE StructureCode
	ORDER BY Sequence
 */

-- SELECT * FROM InternetUser.Catalogue	WHERE ManufacturerId = 'A075'
/*

	DECLARE @str nvarchar(max) = 'hello, hello2, hello3', @sep nvarchar(1) = ',';
	WITH CTE (i, j ) AS 
	(
		SELECT CAST(0 AS INT) as i, CHARINDEX( @sep, @str) as j
		UNION ALL
		SELECT CONVERT(INT, j + 1), CHARINDEX(@sep, @str, j + 1)
		FROM CTE
		WHERE j > 0
	)
	SELECT SUBSTRING(@str, i, COALESCE(NULLIF(j, 0), LEN(@str) + 1) - i) as value
	FROM CTE */

/*
UPDATE InternetUser.Catalogue SET Valid = 0 
WHERE id IN
(SELECT Id, ProductId  FROM (SELECT  Id , ProductId, RANK() OVER (PARTITION BY ProductId, DataAreaId, Name, EnglishName, PartNumber, Available ORDER BY Id) AS rank
FROM InternetUser.Catalogue) AS t
WHERE rank <> 1 order by ProductId)
*/

/*
select * from InternetUser.Catalogue where ShippingDate <> Convert(DateTime, 0)
select * from InternetUser.Catalogue where IsPurchaseOrdered = 1
select * from InternetUser.Catalogue where ProductId = 'TBEU3163-01P'

select * from [InternetUser].[CustomerPriceGroup] where VisitorId = 475 ManufacturerId = 'A113' and  
*/