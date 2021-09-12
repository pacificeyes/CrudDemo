# CrudDemo App Description
.Net Core Web API demo with async controller, service and repository with unit testing using MSTest and Moq

# DB Setup Steps
## In order to run this code, below database script needs to be run on the SQL Server database, to create database objects:

-------------------------------------------------------- Script Section starts here ----------------------------------------------------------------------------------
USE [Customer]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 09/12/2021 19:09:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT [dbo].[Customers] ([Id], [FirstName], [LastName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Rachel', N'Willis', CAST(0x0000ADA000FF88E3 AS DateTime), CAST(0x0000ADA00133EDE1 AS DateTime), 1)
INSERT [dbo].[Customers] ([Id], [FirstName], [LastName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Gabriel', N'Anderson', CAST(0x0000ADA000FF88E3 AS DateTime), CAST(0x0000ADA101333BAB AS DateTime), 1)
INSERT [dbo].[Customers] ([Id], [FirstName], [LastName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'Arthur', N'Wilson', CAST(0x0000ADA000FF88E3 AS DateTime), CAST(0x0000ADA0012EA1C2 AS DateTime), 1)
INSERT [dbo].[Customers] ([Id], [FirstName], [LastName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (4, N'Kenith', N'Benjain', CAST(0x0000ADA0012EFC54 AS DateTime), NULL, 0)
INSERT [dbo].[Customers] ([Id], [FirstName], [LastName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (5, N'Nora', N'Watson', CAST(0x0000ADA101336D4B AS DateTime), NULL, 0)
SET IDENTITY_INSERT [dbo].[Customers] OFF
--------------------------------------------------------- Script Section ends here -----------------------------------------------------------------------------------

## DB Connection String change:
The connection string from CrudApi -> appSettings.json needs to be changed
From:
"ConnectionStrings": {
    "DefaultConnection": "Server=SAKAL-PC\\SQLEXPRESS;Database=Customer;Trusted_Connection=True;"
  }
To:
"ConnectionStrings": {
    "DefaultConnection": "Server=<Your SQL Server Name>;Database=Customer;Trusted_Connection=True;"
  }
