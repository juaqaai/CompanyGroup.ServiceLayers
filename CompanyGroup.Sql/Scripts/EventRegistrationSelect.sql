/*
DECLARE @XMLDoc XML = '
<EventRegistrationData>
    <PersonName>Teszt Elek</PersonName>
    <CompanyName>XYZ Kft.</CompanyName>
    <Phone>36 30 36 448</Phone>
    <Email>teszt@elek.hu</Email>
</EventRegistrationData>';

DECLARE @Xml nvarchar(max) = CONVERT(nvarchar(max), @XMLDoc);

exec [InternetUser].[EventRegistrationAddNew] 'test', 'HRP Hungary', @XML;

SELECT CONVERT(nvarchar(32), EventRegistrationData.data.query('PersonName/text()')) as PersonName, 
	   CONVERT(nvarchar(32), EventRegistrationData.data.query('CompanyName/text()')) as CompanyName, 
	   CONVERT(nvarchar(32), EventRegistrationData.data.query('Phone/text()')) as Phone, 
	   CONVERT(nvarchar(32), EventRegistrationData.data.query('Email/text()')) as Email
FROM @XMLDoc.nodes('/EventRegistrationData') as EventRegistrationData(data)

SELECT @XMLDoc.query('/EventRegistrationData/CompanyName/text()');

SELECT CONVERT(nvarchar(32), EventRegistrationData.data.query('PersonName/text()')) as PersonName, 
	   CONVERT(nvarchar(32), EventRegistrationData.data.query('CompanyName/text()')) as CompanyName, 
	   CONVERT(nvarchar(32), EventRegistrationData.data.query('Phone/text()')) as Phone, 
	   CONVERT(nvarchar(32), EventRegistrationData.data.query('Email/text()')) as Email
FROM @XMLDoc.nodes('/EventRegistrationData') as EventRegistrationData(data)
*/
USE [Web]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [InternetUser].[EventRegistrationSelect]
GO
CREATE PROCEDURE [InternetUser].[EventRegistrationSelect]( @EventId VARCHAR (20) = '' )
AS
SET NOCOUNT ON

	-- select * from InternetUser.EventRegistration
	SELECT Id, 
		   EventId, 
		   EventName, 
		   InternetUser.CalculateXmlFieldValue([Xml], 'PersonName') as PersonName, 
		   InternetUser.CalculateXmlFieldValue([Xml], 'CompanyName') as CompanyName,
		   InternetUser.CalculateXmlFieldValue([Xml], 'Phone') as Phone,
		   InternetUser.CalculateXmlFieldValue([Xml], 'Email') as Email,
	--(SELECT CONVERT(nvarchar(32), EventRegistrationData.data.query('PersonName/text()')) FROM CTE.[Xml].nodes('/EventRegistrationData') as EventRegistrationData(data)), 
	--(SELECT CONVERT(nvarchar(32), EventRegistrationData.data.query('CompanyName/text()')) FROM CTE.[Xml].nodes('/EventRegistrationData') as EventRegistrationData(data)), 
	--(SELECT CONVERT(nvarchar(32), EventRegistrationData.data.query('Phone/text()')) FROM CTE.[Xml].nodes('/EventRegistrationData') as EventRegistrationData(data)), 
	--(SELECT CONVERT(nvarchar(32), EventRegistrationData.data.query('Email/text()')) FROM CTE.[Xml].nodes('/EventRegistrationData') as EventRegistrationData(data)), 
	CreatedDate
	FROM InternetUser.EventRegistration 

	RETURN;
GO
GRANT EXECUTE ON [InternetUser].[EventRegistrationSelect] TO InternetUser

-- exec [InternetUser].[EventRegistrationSelect];

DROP FUNCTION InternetUser.CalculateXmlFieldValue
GO
CREATE FUNCTION InternetUser.CalculateXmlFieldValue( @Xml XML, @FieldName nvarchar(100))
RETURNS NVARCHAR(100)
AS
BEGIN
	DECLARE @Result NVARCHAR(100), @Query VARCHAR(MAX);

	SELECT @Result = 
	CASE @FieldName WHEN 'PersonName' THEN CONVERT(nvarchar(32), EventRegistrationData.data.query('PersonName/text()')) 
					WHEN 'CompanyName' THEN CONVERT(nvarchar(100), EventRegistrationData.data.query('CompanyName/text()')) 
					WHEN 'Phone' THEN CONVERT(nvarchar(100), EventRegistrationData.data.query('Phone/text()'))
					WHEN 'Email' THEN CONVERT(nvarchar(100), EventRegistrationData.data.query('Email/text()')) END
	FROM @Xml.nodes('/EventRegistration') as EventRegistrationData(data)

	/*SET @Query = 'SELECT ' + @Result + '= CONVERT(nvarchar(32), EventRegistrationData.data.query(''' + @FieldName + '/text()'')' + 
				 ' FROM ''' + @Xml + '''.nodes('/EventRegistration') as EventRegistrationData(data)';

	SELECT @Result = CONVERT(nvarchar(32), EventRegistrationData.data.query(@Query)) 
	FROM @Xml.nodes('/EventRegistration') as EventRegistrationData(data) */

	RETURN @Result;
END
GO
GRANT EXECUTE ON InternetUser.CalculateXmlFieldValue TO InternetUser
GO

DECLARE @XMLDoc XML = '
<EventRegistration>
    <PersonName>Teszt Elek</PersonName>
    <CompanyName>XYZ Kft.</CompanyName>
    <Phone>36 30 36 448</Phone>
    <Email>teszt@elek.hu</Email>
</EventRegistration>';

select InternetUser.CalculateXmlFieldValue(@XMLDoc, 'PersonName')