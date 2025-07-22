USE [antasenaDBTest]
GO
/****** Object:  StoredProcedure [dbo].[tp_getDD_PeriodeException]    Script Date: 6/24/2025 11:54:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================  
-- Author:  YAQUB  
-- Create date: 2020-07-20  
-- Description:   
-- =============================================  

ALTER procedure [dbo].[tp_getDD_PeriodeException]
   @PeriodType varchar(max)=''
as  
set @PeriodType =  dbo.fn_StripCharacters(@PeriodType, '^a-zA-Z0-9 [-]_@.!#$&/\[]')  

if @PeriodType=''
	begin
   select PeriodType as id, PeriodType as [desc]
   from MSTipePeriode
   group by PeriodType
   order by 
		case PeriodType
			when 'Harian' then 1
			when 'Mingguan' then 2
			when 'Bulanan' then 3
			when 'Triwulanan' then 4
			when 'Semesteran' then 5
			when 'Tahunan' then 6
			else 99
		end
end
if @PeriodType!=''
	begin
   select PeriodName as id, PeriodName as [desc]
   from MSTipePeriode
   where PeriodType=@PeriodType and FgType is null
end


