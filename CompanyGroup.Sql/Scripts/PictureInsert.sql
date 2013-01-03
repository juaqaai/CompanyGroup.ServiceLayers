USE [WebDb_Test]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- kepek lista feltoltese
DROP PROCEDURE [InternetUser].[PictureInsert];
GO
CREATE PROCEDURE [InternetUser].[PictureInsert] 
AS
SET NOCOUNT ON

	TRUNCATE TABLE InternetUser.Picture;

	INSERT INTO InternetUser.Picture
	SELECT DISTINCT MAX(RecId), ItemId, [FileName], Convert(bit, ElsodlegesKep), GetDate(), Convert(bit, 1)
	FROM axdb_20120614.dbo.UPDKEPEK as P 
	INNER JOIN InternetUser.Catalogue as C ON C.ProductId = P.ItemId
	GROUP BY ItemId, [FileName], Convert(bit, ElsodlegesKep);

	UPDATE InternetUser.Catalogue SET InternetUser.Catalogue.PictureId = P.Id 
	FROM InternetUser.Picture as P 
	INNER JOIN InternetUser.Catalogue as C ON P.ProductId = C.ProductId AND P.[Primary] = 1;
	/*
select o.orgName, oc.dupeCount, o.id
from organizations o
inner join (
    SELECT orgName, COUNT(*) AS dupeCount
    FROM organizations
    GROUP BY orgName
    HAVING COUNT(*) > 1
) oc on o.orgName = oc.orgName
	*/
	/*
	SELECT ProductId, [Primary], COUNT(*)
	FROM InternetUser.Picture
	WHERE [Primary] in (0, 1)
	GROUP BY ProductId, [Primary]
	HAVING (COUNT(*) > 1 )
	ORDER BY ProductId, [Primary]*/


RETURN

-- EXEC [InternetUser].[PictureInsert];
-- select * from InternetUser.Picture