USE [antasenaDBTest]
GO
/****** Object:  StoredProcedure [dbo].[pg_getTreeInformasi]    Script Date: 7/18/2025 6:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  [dbo].[pg_getTreeInformasi] '','Kelompok Informasi Keuangan','D','614'

ALTER procedure [dbo].[pg_getTreeInformasi]
   @flag varchar(1)='',
   @kelompokinformasi varchar(100)='',
   @PeriodTypeId varchar(100)='',
   @GroupId varchar(100)='0'
as    

If OBJECT_ID('tempdb.dbo.#temp_pg_kelinf') is not null                                         
		begin
   DROP TABLE #temp_pg_kelinf
end 
	select distinct a.kelompokinformasi, a.idinformation, d.PeriodTypeId
into #temp_pg_kelinf
from v_allinformation as a
   inner join
   (select b.idinformation
					, case when c.PeriodType='Harian' then 'D'
						when c.PeriodType='Bulanan' then 'M'
						when c.PeriodType='Triwulanan' then 'Q'
						when c.PeriodType='Mingguan' then 'W'
						when c.PeriodType='Semesteran' then 'S'
						when c.PeriodType='Tahunan' then 'A'
				else ''
				end as PeriodTypeId
					, c.PeriodType
   FROM PeriodeInformation as b
      inner join MSTipePeriode as c on b.PeriodId=c.PeriodId
			) as d on a.idinformation=d.idinformation
   inner join MSGroupInformation as g on a.idinformation=g.IDInformasi and d.PeriodTypeId=g.Periode
where g.GroupId=@GroupId

if @flag='0'
	begin
   declare @pilihan varchar(1),@GidUser int
   select @pilihan= FgInformationOpt
   from msgroup
   where GroupId=@groupid
   select @GidUser=count(1)
   from MSUser
   where GroupId=@GroupId

   select distinct a.kelompokinformasi [id], a.kelompokinformasi [desc]
				, case when b.kelompokinformasi is not null then 1 else 0 end as [check]
				, @pilihan as pilihan
				, case when @GidUser>0 then 1 else 0 end as GidUser
   from metadata_mgmt as a
      left join #temp_pg_kelinf as b on a.kelompokinformasi=b.kelompokinformasi
end
else if @flag='1'
	begin
   select distinct a.kelompokinformasi+'|'+d.PeriodTypeId as [id]
				, a.kelompokinformasi+' - '+d.PeriodType [desc]
				, d.PeriodTypeId
				, case when v.idinformation is not null then 1 else 0 
				end as [check]
   from v_allinformation as a
      inner join
      (select b.idinformation
				, case when c.PeriodType='Harian' then 'D'
					when c.PeriodType='Bulanan' then 'M'
					when c.PeriodType='Triwulanan' then 'Q'
					when c.PeriodType='Mingguan' then 'W'
					when c.PeriodType='Semesteran' then 'S'
						when c.PeriodType='Tahunan' then 'A'
				else ''
				end as PeriodTypeId
				, c.PeriodType
      FROM PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId=c.PeriodId
		) as d on a.idinformation=d.idinformation
      left join #temp_pg_kelinf as v on a.kelompokinformasi=v.kelompokinformasi and d.PeriodTypeId=v.PeriodTypeId
   where a.kelompokinformasi=@kelompokinformasi
   order by d.PeriodTypeId

end
else
	begin
   select distinct a.kelompokinformasi, a.idinformation+'|'+d.PeriodTypeId as [id] , a.namainformasi [desc]
				, case when (select count(1)
      from MSGroupInformation
      where GroupId=@GroupId and IDInformasi=a.idinformation and Periode=d.PeriodTypeId)
				>0 then 1 else 0 end as [check]
   from v_allinformation as a
      inner join
      (select b.idinformation
				, case when c.PeriodType='Harian' then 'D'
					when c.PeriodType='Bulanan' then 'M'
					when c.PeriodType='Triwulanan' then 'Q'
					when c.PeriodType='Mingguan' then 'W'
					when c.PeriodType='Semesteran' then 'S'
					when c.PeriodType='Tahunan' then 'A'
				else ''
				end as PeriodTypeId
				, c.PeriodType
      FROM PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId=c.PeriodId
		) as d on a.idinformation=d.idinformation
   where a.kelompokinformasi=@kelompokinformasi
      and d.PeriodTypeId=@PeriodTypeId
      and a.parentinformation is null
end

