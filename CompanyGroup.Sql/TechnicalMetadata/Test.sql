
declare @Target table ( TitleID INT NOT NULL PRIMARY KEY,
						Title NVARCHAR(100) NOT NULL,
						Quantity INT NOT NULL);

declare @Source table ( TitleID INT NOT NULL PRIMARY KEY,
					    Title NVARCHAR(100) NOT NULL,
						Quantity INT NOT NULL);

INSERT @Target VALUES
  (1, 'The Catcher in the Rye', 6),
  (2, 'Pride and Prejudice', 3),
  (3, 'The Great Gatsby', 0),
  (5, 'Jane Eyre', 0),
  (6, 'Catch 22', 0),
  (8, 'Slaughterhouse Five', 4);

INSERT @Source VALUES
  (1, 'The Catcher in the Rye', 3),
  (3, 'The Great Gatsby', 0),
  (4, 'Gone with the Wind', 4),
  (5, 'Jane Eyre', 5),
  (7, 'Age of Innocence', 8);

	MERGE @Target bi
	USING @Source bo
	ON bi.TitleID = bo.TitleID
	WHEN MATCHED AND
		bi.Quantity + bo.Quantity = 0 THEN			-- 3-as TitleId nem kerul bele a @Target-ba
		DELETE
	WHEN MATCHED THEN
		UPDATE
		SET bi.Quantity = bi.Quantity + bo.Quantity	-- 1-es TitleId Quantity = 9 lesz, 
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (TitleID, Title, Quantity)
		VALUES (bo.TitleID, bo.Title, bo.Quantity)
	WHEN NOT MATCHED BY SOURCE						-- 6-os TitleId nem kerul bele a @Target-ba
	  AND bi.Quantity = 0 THEN
    DELETE;

SELECT * FROM @Target;
SELECT * FROM @Source;

