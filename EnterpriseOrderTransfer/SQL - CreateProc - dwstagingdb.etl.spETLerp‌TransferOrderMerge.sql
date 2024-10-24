/****** Script for SelectTopNRows command from SSMS  ******/
use dwstagingdb
go

ALTER PROC etl.spETLerpTransferOrderMerge with encryption 
AS

MERGE dwdb.dbo.erpTransferorder as t 
USING (
SELECT [recordType]
      ,[id]
      ,[tranDate]
      ,[typeText]
      ,[statusText]
	  ,[tranId]
      ,[locationText]
      ,[transferLocationText]
      ,[createdbyText]
      ,[deliveryInstruction]
FROM [DWStagingDB].[dbo].[erpTransferOrderFlat]
) as s 
ON t.recordType = s.recordType AND t.internalId = CAST(s.id AS BIGINT) AND t.trandate = CAST(s.trandate AS DATETIME) 

WHEN MATCHED THEN UPDATE SET 
      
      t.[type] = s.[typetext]
	  , t.documentNumber = s.tranid
      , t.[status] = s.[statusText]
      , t.[location] = s.[locationText] 
      , t.[transferLocation] = s.[transferlocationText]
      , t.[createdBy] = s.[createdbytext]
      , t.[deliveryInstruction] = s.[deliveryInstruction]

WHEN NOT MATCHED BY TARGET THEN INSERT (
	  [recordType]
      ,[internalId]
      ,[tranDate]
      ,[type]
	  ,[documentNumber]
      ,[status]
      ,[location]
      ,[transferLocation]
      ,[createdby]
      ,[deliveryInstruction]
   )
	  VALUES(
	  [recordType]
      ,CAST([id] AS BIGINT)
      ,CAST([tranDate] AS DATETIME)
      ,[typeText]
	  ,[tranId]
      ,[statusText]
      ,[locationText]
      ,[transferLocationText]
      ,[createdbyText]
      ,[deliveryInstruction]	  
	  )
WHEN NOT MATCHED BY SOURCE THEN DELETE 

;

