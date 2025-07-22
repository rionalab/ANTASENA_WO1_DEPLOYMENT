/****** Object:  StoredProcedure [dbo].[Id_getAllIndikasiDenda]    Script Date: 4/24/2025 10:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[Id_getAllIndikasiDenda]
as  
  
select DendaId, KelompokInformasi,
   case 
      when TipePeriodeLaporan='D' then 'Harian'  
      when TipePeriodeLaporan='W' then 'Mingguan'
      when TipePeriodeLaporan='M' then 'Bulanan'
      when TipePeriodeLaporan='S' then 'Semesteran'
      when TipePeriodeLaporan='A' then 'Tahunan'
      else 'Triwulan'
   end TipePeriodeLaporan
from msdenda
order by KelompokInformasi asc
