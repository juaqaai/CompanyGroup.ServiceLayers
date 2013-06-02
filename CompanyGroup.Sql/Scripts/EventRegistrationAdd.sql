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

DROP PROCEDURE [InternetUser].[EventRegistrationAddNew]
GO
CREATE PROCEDURE [InternetUser].[EventRegistrationAddNew]( @EventId VARCHAR (20), 
													   @EventName VARCHAR (300), 
													   @Xml NVARCHAR (MAX))
AS
SET NOCOUNT ON

	INSERT INTO InternetUser.EventRegistration (EventId, EventName, [Xml]) VALUES
											   (@EventId, @EventName, @Xml);

	SELECT @@IDENTITY as Id;

	RETURN;
GO
GRANT EXECUTE ON [InternetUser].[EventRegistrationAddNew] TO InternetUser

-- select * from InternetUser.EventRegistration

/*-------------------------------------------------------------------------------------------------------
DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT, @XMLDoc

        SELECT * FROM OPENXML (@handle, '/Structure/Manufacturer', 2) WITH (Id varchar(4));
		 
EXEC sp_xml_removedocument @handle

*/