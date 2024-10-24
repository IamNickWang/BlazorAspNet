USE [DWStagingDB]
GO

/****** Object:  Table [dbo].[erpTransferOrderFlat]    Script Date: 10/18/2024 1:07:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[erpTransferOrderFlat](
	[recordType] [varchar](150) NULL,
	[id] [bigint] NULL,
	[internalIdValue] [varchar](150) NULL,
	[internalIdText] [varchar](150) NULL,
	[tranDate] [varchar] (150) NULL,
	[typeValue] [varchar](150) NULL,
	[typeText] [varchar](150) NULL,
	[tranId] [varchar](150) NULL,
	[statusValue] [varchar](150) NULL,
	[statusText] [varchar](150) NULL,
	[locationValue] [varchar](150) NULL,
	[locationText] [varchar](150) NULL,
	[transferLocationValue] [varchar](150) NULL,
	[transferLocationText] [varchar](150) NULL,
	[createdbyValue] [varchar](150) NULL,
	[createdbyText] [varchar](150) NULL,
	[deliveryInstruction] [varchar](max) NULL,
	[dataPullPart] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


