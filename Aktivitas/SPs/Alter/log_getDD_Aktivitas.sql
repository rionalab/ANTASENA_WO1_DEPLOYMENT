SET
   ANSI_NULLS ON
GO
SET
   QUOTED_IDENTIFIER ON
GO
   -- =============================================  
   -- Author:  YAQUB  
   -- Create date:  
   -- Description:   
   -- =============================================  
   ALTER procedure [dbo].[log_getDD_Aktivitas] as
select
   distinct tablename + ',' + menudesc as [id],
   menudesc as [desc]
from
   ATTableList
where
   tablename is not null
order by
   menudesc