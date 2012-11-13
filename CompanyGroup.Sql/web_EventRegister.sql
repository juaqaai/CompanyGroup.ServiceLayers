USE [InternetDb]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__iVali__2C2B08DD]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sTime__2B36E4A4]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sDate__2A42C06B]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF_web_EventRegister_sMenu]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF_web_EventRegister_sFollower]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sEmai__294E9C32]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sPhon__285A77F9]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sName__276653C0]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sComp__26722F87]
GO

ALTER TABLE [InternetUser].[web_EventRegister] DROP CONSTRAINT [DF__web_Event__sEven__257E0B4E]
GO

/****** Object:  Table [InternetUser].[web_EventRegister]    Script Date: 2012.11.12. 22:13:03 ******/
DROP TABLE [InternetUser].[web_EventRegister]
GO

/****** Object:  Table [InternetUser].[web_EventRegister]    Script Date: 2012.11.12. 22:13:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [InternetUser].[web_EventRegister](
	[iID] [int] IDENTITY(1,1) NOT NULL,
	[sEventID] [varchar](16) NOT NULL DEFAULT '',
	[sCompanyName] [varchar](128) NOT NULL DEFAULT '',
	[sName] [varchar](128) NOT NULL DEFAULT '',
	[sPhone] [varchar](32) NOT NULL DEFAULT '',
	[sEmail] [varchar](64) NOT NULL DEFAULT '',
	[sFollower] [varchar](16) NOT NULL DEFAULT '',
	[sMenu] [varchar](16) NOT NULL DEFAULT '',
	[sPartnerId] [varchar](16) NOT NULL DEFAULT '',
	[sDate] [varchar](16) NOT NULL DEFAULT (CONVERT([varchar](16),getdate(),(111))),
	[sTime] [varchar](16) NOT NULL DEFAULT (CONVERT([varchar](16),getdate(),(108))),
	[iValid] [int] NOT NULL DEFAULT 0,
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sEven__257E0B4E]  DEFAULT ('') FOR [sEventID]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sComp__26722F87]  DEFAULT ('') FOR [sCompanyName]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sName__276653C0]  DEFAULT ('') FOR [sName]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sPhon__285A77F9]  DEFAULT ('') FOR [sPhone]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sEmai__294E9C32]  DEFAULT ('') FOR [sEmail]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF_web_EventRegister_sFollower]  DEFAULT ('') FOR [sFollower]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF_web_EventRegister_sMenu]  DEFAULT ('') FOR [sMenu]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sDate__2A42C06B]  DEFAULT (CONVERT([varchar](16),getdate(),(111))) FOR [sDate]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__sTime__2B36E4A4]  DEFAULT (CONVERT([varchar](16),getdate(),(108))) FOR [sTime]
GO

ALTER TABLE [InternetUser].[web_EventRegister] ADD  CONSTRAINT [DF__web_Event__iVali__2C2B08DD]  DEFAULT ((1)) FOR [iValid]
GO


