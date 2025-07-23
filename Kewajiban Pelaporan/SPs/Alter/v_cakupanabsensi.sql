SET
   ANSI_NULLS ON
GO
SET
   QUOTED_IDENTIFIER ON
GO
   ALTER view [dbo].[v_cakupanabsensi] as with A as (
      select
         distinct PeriodeInformation.idinformation,
         PeriodeInformation.versioncode,
         cakupanabsensi,
         case
            when MSTipePeriode.PeriodType = 'tahunan' then 'A'
            when MSTipePeriode.PeriodType = 'semesteran' then 'S'
            when MSTipePeriode.PeriodType = 'bulanan' then 'M'
            when MSTipePeriode.PeriodType = 'mingguan' then 'W'
            when MSTipePeriode.PeriodType = 'triwulanan' then 'Q'
            when MSTipePeriode.PeriodType = 'harian' then MSTipePeriodeHarian.TipeHarian
         end as tipeperiod
      From
         PeriodeInformation
         inner join mstipeperiode on PeriodeInformation.periodid = mstipeperiode.periodid
         left outer join MSTipePeriodeHarian on MSTipePeriode.PeriodName = MSTipePeriodeHarian.periodname
   )
select
   distinct A.idinformation,
   b.VersionCode versioncode,
   A.cakupanabsensi,
   A.tipeperiod
from
   A A
   inner join metadata_mgmt b on a.idinformation = b.idinformation
GO