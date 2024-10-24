USE [DWDB]
GO

/****** Object:  Table [dbo].[erpTransferOrderFlat]    Script Date: 10/21/2024 2:25:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[erpTransferOrder](
	[recordType] [varchar](150) NULL,
	[internalId] [bigint] NULL,
	[tranDate] [datetime] NULL,
	[type] [varchar](150) NULL,
	[documentNumber] [varchar](150) NULL,
	[status] [varchar](150) NULL,
	[location] [varchar](250) NULL,
	[transferLocation] [varchar](250) NULL,
	[createdby] [varchar](150) NULL,
	[deliveryInstruction] [varchar](max) NULL,

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE VIEW erp.netsuiteTransferOrder WITH ENCRYPTION AS 

SELECT [recordType]
      ,[internalId]
      ,[tranDate]
      ,[type]
      ,[documentNumber]
      ,[status]
      ,[location]
      ,[transferLocation]
      ,[createdby]
      ,[deliveryInstruction]
FROM [DWDB].[dbo].[erpTransferOrder]
GO
