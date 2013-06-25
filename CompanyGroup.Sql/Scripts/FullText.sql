USE Web;
GO
CREATE FULLTEXT CATALOG WebProductsFullTextCatalog;

CREATE UNIQUE INDEX Index_ProductId_DataAreaId ON InternetUser.Catalogue(ProductId, DataAreaId);

CREATE FULLTEXT INDEX ON InternetUser.Catalogue
(
    SearchContent                    --Full-text index column name 
    --TYPE COLUMN ''    --Name of column that contains file type information
    --Language 1038                 --2057 is the LCID for British English
)
KEY INDEX Index_ProductId_DataAreaId ON WebProductsFullTextCatalog --Unique index
WITH CHANGE_TRACKING AUTO            --Population type;
GO

select * from InternetUser.Catalogue

SELECT c.*, key_tbl.*
FROM InternetUser.Catalogue as c
   JOIN containstable 
    (InternetUser.Catalogue, SearchContent, '"ASUS"') as key_tbl
   ON c.Id = key_tbl.[Key]
ORDER BY key_tbl.[Rank] desc