USE [antasenaDBTest]
GO
/****** Object:  StoredProcedure [dbo].[tp_deleteTipePeriode_EX]    Script Date: 7/13/2025 8:23:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








-- =============================================      
-- Author:  YAQUB      
-- Create date: 2020-07-22      
-- Description:       
-- =============================================      

ALTER procedure [dbo].[tp_deleteTipePeriode_EX]
  @PeriodId int,
  @by varchar(max)
as      
      
Declare @ecode int      
      
Begin Tran      
set @ecode = @@ERROR      
      
DECLARE @PeriodName varchar(max),@PeriodType varchar(max),@PeriodeInf int,@del varchar(max)      
      
select @PeriodeInf=count(1)
from [dbo].[PeriodeInformation]
where PeriodId= @PeriodId      
  
set @PeriodeInf = 0  
      
if @PeriodeInf = 0 begin
  select @PeriodName=PeriodName, @PeriodType=PeriodType
  from dbo.MSTipePeriode
  WHERE PeriodId = @PeriodId

  IF @PeriodType='Harian'      
 BEGIN
    --log      
    if @ecode = 0 Begin
      insert into dbo.ATMSTipePeriodeHarianExc
        (
        TipePeriodeId,
        PeriodName
        ,PeriodNameOri
        ,FgMove
        ,Periodedata
        ,[TanggalBukaPelaporan]
        ,[JamBukaPelaporan]
        ,[TanggalTutupPelaporan]
        ,[JamTutupPelaporan]
        ,[UseKoreksi]
        ,[JumlahHariKoreksi]
        ,[JamBukaKoreksi]
        ,[JamTutupKoreksi],
        UpdUser,
        UpdDate,
        UpdFlag )
      select TipePeriodeId,
        PeriodName      
 , PeriodNameOri    
 , FgMove    
 , Periodedata    
 , [TanggalBukaPelaporan]    
 , [JamBukaPelaporan]    
 , [TanggalTutupPelaporan]    
 , [JamTutupPelaporan]    
 , [UseKoreksi]    
 , [JumlahHariKoreksi]    
 , [JamBukaKoreksi]    
 , [JamTutupKoreksi], @by upduser, getdate() upddate, 'D' updflag
      from dbo.MSTipePeriodeHarianExc
      where PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
    if @ecode = 0 Begin
      DELETE FROM [dbo].[MSTipePeriodeHarianExc]      
   WHERE PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
  END

  IF @PeriodType='Mingguan'      
 BEGIN
    --log      
    if @ecode = 0 Begin
      insert into dbo.ATMSTipePeriodeMingguanExc
        (
        TipePeriodeId,
        PeriodName
        ,[PeriodNameOri]
        ,[FgMove]
        ,[MingguPeriodedata]
        ,[BulanPeriodedata]
        ,[TahunPeriodeData]
        ,[MulaiBatasPelaporan]
        ,[PenambahanBulanMulai]
        ,[AkhirBatasPelaporan]
        ,[PenambahanBulanAkhir]
        ,[BatasKeterlambatan],
        UpdUser,
        UpdDate,
        UpdFlag)
      select
        TipePeriodeId,
        PeriodName      
 , [PeriodNameOri]    
 , [FgMove]    
 , [MingguPeriodedata]    
 , [BulanPeriodedata]    
 , [TahunPeriodeData]    
 , [MulaiBatasPelaporan]    
 , [PenambahanBulanMulai]    
 , [AkhirBatasPelaporan]    
 , [PenambahanBulanAkhir]    
 , [BatasKeterlambatan],
        @by , getdate(), 'D'
      from dbo.MSTipePeriodeMingguanExc
      where PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
    if @ecode = 0 Begin
      DELETE FROM [dbo].[MSTipePeriodeMingguanExc]      
   WHERE PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
  END



  IF @PeriodType='Bulanan'      
 BEGIN
    --log      
    if @ecode = 0 Begin
      insert into dbo.ATMSTipePeriodeBulananExc
        (
        TipePeriodeId,
        PeriodName
        ,[PeriodNameOri]
        ,[FgMove]
        ,[BulanPeriodedata]
        ,[TahunPeriodeData]
        ,[MulaiBatasPelaporan]
        ,[AkhirBatasPelaporan]
        ,[PenambahanBulan]
        ,[BatasKeterlambatan],
        UpdUser,
        UpdDate,
        UpdFlag)
      select TipePeriodeId,
        PeriodName      
    , [PeriodNameOri]    
 , [FgMove]    
 , [BulanPeriodedata]    
 , [TahunPeriodeData]    
 , [MulaiBatasPelaporan]    
 , [AkhirBatasPelaporan]    
 , [PenambahanBulan]    
 , [BatasKeterlambatan],
        @by,
        getdate(),
        'D'
      from dbo.MSTipePeriodeBulananExc
      where PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
    if @ecode = 0 Begin
      DELETE FROM [dbo].[MSTipePeriodeBulananExc]      
   WHERE PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
  END

  IF @PeriodType='Triwulanan'      
 BEGIN
    --log      
    if @ecode = 0 Begin
      insert into dbo.ATMSTipePeriodeTriwulanExc
        (
        TipePeriodeId,
        PeriodName
        ,[PeriodNameOri]
        ,[FgMove]
        ,[TriwulanPeriodedata]
        ,[TahunPeriodeData]
        ,[MulaiBatasPelaporan]
        ,[AkhirBatasPelaporan]
        ,[PenambahanBulan]
        ,[BatasKeterlambatan],
        UpdUser,
        UpdDate,
        UpdFlag)
      select TipePeriodeId,
        PeriodName      
 , [PeriodNameOri]    
 , [FgMove]    
 , [TriwulanPeriodedata]    
 , [TahunPeriodeData]    
 , [MulaiBatasPelaporan]    
 , [AkhirBatasPelaporan]    
 , [PenambahanBulan]    
 , [BatasKeterlambatan],
        @by,
        getdate(),
        'D'
      from dbo.MSTipePeriodeTriwulanExc
      where PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
    if @ecode = 0 Begin
      DELETE FROM [dbo].[MSTipePeriodeTriwulanExc]      
   WHERE PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
  END


  IF @PeriodType='Semesteran'      
 BEGIN
    --log      
    if @ecode = 0 Begin
      insert into dbo.ATMSTipePeriodeSemesterExc
        (
        TipePeriodeId,
        PeriodName
        ,[PeriodNameOri]
        ,[FgMove]
        ,[SemesterPeriodedata]
        ,[TahunPeriodeData]
        ,[MulaiBatasPelaporan]
        ,[AkhirBatasPelaporan]
        ,[PenambahanBulan]
        ,[BatasKeterlambatan],
        UpdDate,
        UpdUser,
        UpdFlag)
      select TipePeriodeId,
        PeriodName      
 , [PeriodNameOri]    
 , [FgMove]    
 , [SemesterPeriodedata]    
 , [TahunPeriodeData]    
 , [MulaiBatasPelaporan]    
 , [AkhirBatasPelaporan]    
 , [PenambahanBulan]    
 , [BatasKeterlambatan],
        getdate(),
        @by,
        'D'
      from dbo.MSTipePeriodeSemesterExc
      where PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
    if @ecode = 0 Begin
      DELETE FROM [dbo].[MSTipePeriodeSemesterExc]      
   WHERE PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
  END



  IF @PeriodType='Tahunan'      
 BEGIN
    --log      
    if @ecode = 0 
		Begin
      insert into dbo.ATMSTipePeriodeTahunExc
        (
        TipePeriodeId,
        [PeriodNameOri],
        [FgMove],
        [TahunPeriodeData],
        [MulaiBatasPelaporan],
        [AkhirBatasPelaporan],
        [PenambahanBulan],
        [BatasKeterlambatan],
        UpdDate,
        UpdUser,
        UpdFlag,
        PeriodName
        )
      select
        TipePeriodeId,
        [PeriodNameOri],
        [FgMove],
        [TahunPeriodeData],
        [MulaiBatasPelaporan],
        [AkhirBatasPelaporan],
        [PenambahanBulan],
        [BatasKeterlambatan],
        getdate(),
        @by,
        'D',
        PeriodName
      from dbo.MSTipePeriodeTahunExc
      where PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end

    if @ecode = 0 
		Begin
      DELETE FROM [dbo].[MSTipePeriodeTahunExc]      
			WHERE PeriodName=@PeriodName
      set @ecode = @ecode + @@ERROR
    end
  END



  --log      
  if @ecode = 0 Begin
    insert into dbo.ATMSTipePeriode
      (PeriodId,PeriodName,PeriodType,FgMove,FgType,UpdDate,UpdUser,UpdFlag)
    select PeriodId, PeriodName, PeriodType, FgMove, FgType, getdate() upddate, @by upduser, 'D' updflag
    from dbo.MSTipePeriode
    where PeriodId = @PeriodId
    set @ecode = @ecode + @@ERROR
  end
  if @ecode = 0 Begin
    DELETE FROM [dbo].[MSTipePeriode]      
    WHERE [PeriodId] = @PeriodId
  end
  set @del='Berhasil Hapus tipe periode exception'
end       
else       
begin
  set @del='Tipe periode ini tidak boleh dihapus.'
end      
      
if @ecode = 0 and @@ERROR = 0      
Begin
  commit tran
end      
else Begin
  rollback tran
end      
      
select case when @PeriodeInf=0 then 1 else 0 end isexists, @del message      
      
      
