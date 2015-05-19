USE [shifenzheng]
GO

/****** Object:  Table [dbo].[Result]    Script Date: 2015/5/10 12:14:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Result](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Source] [varchar](5000) NOT NULL,
	[Level1] [varchar](5000) NOT NULL,
	[Level2] [varchar](5000) NOT NULL,
	[Level3] [varchar](5000) NOT NULL,
	[Level4] [varchar](5000) NOT NULL,
	[Level5] [varchar](5000) NOT NULL,
	[Score] [float] NOT NULL,
	[LastKey] [int] NOT NULL,
 CONSTRAINT [PK_Result] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

