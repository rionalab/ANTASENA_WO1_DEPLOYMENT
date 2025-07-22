SET
   ANSI_NULLS ON
GO
SET
   QUOTED_IDENTIFIER ON
GO
   ALTER procedure [dbo].[usr_getTreeInformasi] --'0','','','616',''    
   --declare  
   @flag varchar(1) = '',
   @kelompokinformasi varchar(100) = '',
   @PeriodTypeId varchar(100) = '',
   @GroupId varchar(100) = '0',
   @UserName varchar(max) = '' as --set @flag = 0  
   --set @GroupId = 248  
   --set @UserName ='testbca@gmail.com'   
   declare @fgOpt varchar(10)
select
   @fgOpt = FgPelaporOpt
from
   msgroup
where
   GroupId = @groupid declare @pilihan varchar(1)
select
   @pilihan = FgInformationOpt
from
   msgroup
where
   GroupId = @groupid If OBJECT_ID('tempdb.dbo.#temp_pg_kelinf') is not null begin DROP TABLE #temp_pg_kelinf
end CREATE TABLE dbo.#temp_pg_kelinf
(
   kelompokinformasi [varchar](max) NULL,
   idinformation [varchar](max) NULL,
   PeriodTypeId [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] if @pilihan = 'P' begin
insert into
   #temp_pg_kelinf
select
   distinct a.kelompokinformasi,
   a.idinformation,
   d.PeriodTypeId
from
   v_allinformation as a
   inner join (
      select
         b.idinformation,
         case
            when c.PeriodType = 'Harian' then 'D'
            when c.PeriodType = 'Bulanan' then 'M'
            when c.PeriodType = 'Triwulanan' then 'Q'
            when c.PeriodType = 'Mingguan' then 'W'
            when c.PeriodType = 'Semesteran' then 'S'
            when c.PeriodType = 'Tahunan' then 'A'
            else ''
         end as PeriodTypeId,
         c.PeriodType
      FROM
         PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId = c.PeriodId
   ) as d on a.idinformation = d.idinformation -- inner join MSGroupInformation as g on a.idinformation=g.IDInformasi and d.PeriodTypeId=g.Periode    
   --where g.GroupId=@GroupId    
end
else begin
insert into
   #temp_pg_kelinf
select
   distinct a.kelompokinformasi,
   a.idinformation,
   d.PeriodTypeId
from
   v_allinformation as a
   inner join (
      select
         b.idinformation,
         case
            when c.PeriodType = 'Harian' then 'D'
            when c.PeriodType = 'Bulanan' then 'M'
            when c.PeriodType = 'Triwulanan' then 'Q'
            when c.PeriodType = 'Mingguan' then 'W'
            when c.PeriodType = 'Semesteran' then 'S'
            when c.PeriodType = 'Tahunan' then 'A'
            else ''
         end as PeriodTypeId,
         c.PeriodType
      FROM
         PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId = c.PeriodId
   ) as d on a.idinformation = d.idinformation
end If OBJECT_ID('tempdb.dbo.#temp_usr_kelinf') is not null begin DROP TABLE #temp_usr_kelinf
end
select
   distinct a.kelompokinformasi,
   a.idinformation,
   d.PeriodTypeId into #temp_usr_kelinf
from
   v_allinformation as a
   inner join (
      select
         b.idinformation,
         case
            when c.PeriodType = 'Harian' then 'D'
            when c.PeriodType = 'Bulanan' then 'M'
            when c.PeriodType = 'Triwulanan' then 'Q'
            when c.PeriodType = 'Mingguan' then 'W'
            when c.PeriodType = 'Semesteran' then 'S'
            when c.PeriodType = 'Tahunan' then 'A'
            else ''
         end as PeriodTypeId,
         c.PeriodType
      FROM
         PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId = c.PeriodId
   ) as d on a.idinformation = d.idinformation
   inner join MsUserInformation as g on a.idinformation = g.IDInformasi
   and d.PeriodTypeId = g.Periode
where
   g.UserName = @UserName If OBJECT_ID('tempdb.dbo.#tempinformasi') is not null begin DROP TABLE #tempinformasi
end
select
   distinct kelompokinformasi into #tempinformasi
from
   v_allinformation if @flag = '0' begin if @pilihan = 'P' begin if exists (
      select
         1
      from
         MSUser
      where
         UserName = @UserName
   ) Begin
select
   @pilihan = FgInformationOpt
from
   MSUser
where
   UserName = @UserName
end
select
   distinct a.kelompokinformasi [id],
   a.kelompokinformasi [desc],
   case
      when u.kelompokinformasi is not null then 1
      else 0
   end as [check],
   @pilihan as pilihan
from
   #tempinformasi as a
   inner join #temp_pg_kelinf as b on a.kelompokinformasi=b.kelompokinformasi
   left join #temp_usr_kelinf as u on a.kelompokinformasi=u.kelompokinformasi
end
else begin if exists(
   select
      FgInformationOpt
   from
      MSUser
   where
      UserName = @UserName
) begin
select
   @pilihan = FgInformationOpt
from
   MSUser
where
   UserName = @UserName
select
   distinct a.kelompokinformasi [id],
   a.kelompokinformasi [desc],
   case
      when u.kelompokinformasi is not null then 1
      else 0
   end as [check],
   @pilihan as pilihan
from
   #tempinformasi as a
   inner join #temp_pg_kelinf as b on a.kelompokinformasi=b.kelompokinformasi
   left join #temp_usr_kelinf as u on a.kelompokinformasi=u.kelompokinformasi
end
else begin
select
   distinct a.kelompokinformasi [id],
   a.kelompokinformasi [desc],
   0 as [check],
   'P' as pilihan
from
   #tempinformasi as a
end
end
end
else if @flag = '1' begin
select
   distinct a.kelompokinformasi + '|' + d.PeriodTypeId as [id],
   a.kelompokinformasi + ' - ' + d.PeriodType [desc],
   d.PeriodTypeId,
   case
      when u.idinformation is not null then 1
      else 0
   end as [check]
from
   v_allinformation as a
   inner join (
      select
         b.idinformation,
         case
            when c.PeriodType = 'Harian' then 'D'
            when c.PeriodType = 'Bulanan' then 'M'
            when c.PeriodType = 'Triwulanan' then 'Q'
            when c.PeriodType = 'Mingguan' then 'W'
            when c.PeriodType = 'Semesteran' then 'S'
            when c.PeriodType = 'Tahunan' then 'A'
            else ''
         end as PeriodTypeId,
         c.PeriodType
      FROM
         PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId = c.PeriodId
   ) as d on a.idinformation = d.idinformation
   inner join #temp_pg_kelinf as v on a.kelompokinformasi=v.kelompokinformasi and d.PeriodTypeId=v.PeriodTypeId
   left join #temp_usr_kelinf as u on a.kelompokinformasi=u.kelompokinformasi and d.PeriodTypeId=u.PeriodTypeId
where
   a.kelompokinformasi = @kelompokinformasi
order by
   d.PeriodTypeId
end
else begin
select
   distinct a.kelompokinformasi,
   a.idinformation + '|' + d.PeriodTypeId as [id],
   a.namainformasi [desc],
   case
      when (
         select
            count(1)
         from
            MsUserInformation
         where
            UserName = @UserName
            and IDInformasi = a.idinformation
            and Periode = d.PeriodTypeId
      ) > 0 then 1
      else 0
   end as [check]
from
   v_allinformation as a
   inner join (
      select
         b.idinformation,
         case
            when c.PeriodType = 'Harian' then 'D'
            when c.PeriodType = 'Bulanan' then 'M'
            when c.PeriodType = 'Triwulanan' then 'Q'
            when c.PeriodType = 'Mingguan' then 'W'
            when c.PeriodType = 'Semesteran' then 'S'
            when c.PeriodType = 'Tahunan' then 'A'
            else ''
         end as PeriodTypeId,
         c.PeriodType
      FROM
         PeriodeInformation as b
         inner join MSTipePeriode as c on b.PeriodId = c.PeriodId
   ) as d on a.idinformation = d.idinformation
   inner join #temp_pg_kelinf as c on a.idinformation=c.idinformation
where
   a.kelompokinformasi = @kelompokinformasi
   and d.PeriodTypeId = @PeriodTypeId
   and a.parentinformation is null
end