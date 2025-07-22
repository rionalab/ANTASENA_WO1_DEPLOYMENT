USE [antasenaDBTest]
GO
/****** Object:  StoredProcedure [dbo].[mm_getrecordcsv]    Script Date: 6/20/2025 9:44:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[mm_getrecordcsv]
   @versioncode varchar (max),
   @startDate DATE,
   @endDate DATE
as  
  
set @versioncode =  dbo.fn_StripCharacters(@versioncode, '^a-zA-Z0-9 [-]_@.!#$&/\[]')  
  
if @versioncode = 'all'  
begin
   SELECT [idinformation],
      [idelement],
      [nillable_yn],
      [idtoindex],
      [compositekey_yn],
      [idtodata],
      [length] ,
      [fractiondigit]
   FROM [dbo].[metadata_mgmt]
   WHERE [dateend] is null or dateend >= GETDATE()
   order by [idinformation],[sequence] asc
end  
else  
SELECT [idelement],
   [nillable_yn],
   [idtoindex],
   [compositekey_yn],
   [idtodata],
   [length],
   [fractiondigit]
FROM [dbo].[metadata_mgmt]
WHERE [versioncode] = @versioncode
   and (
		(@startDate is null and datestart is null)
   or
   (@startDate is not null or datestart >= @startDate)
	)
   and (
		(@endDate is null and dateend is null)
   or
   (@endDate is not null and dateend <= @endDate)
	)
order by [idinformation],[sequence] asc  
