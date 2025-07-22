
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



  
  
ALTER procedure [dbo].[tp_insertTipePeriode_EX]  
@PeriodType varchar(max),  
@PeriodName varchar(max),  
@PeriodName_Ori varchar(max),
@fgmove varchar(max) 

--Harian
,@PeriodData_H varchar(max)
,@TglBuka_H varchar(max)
,@JamBuka_H varchar(max)
,@TglTutup_H varchar(max)
,@JamTutup_H varchar(max)
,@UseKoreksi varchar(max)
,@JumlahHariKoreksi varchar(max)
,@JamBukaKoreksi varchar(max)
,@JamTutupKoreksi varchar(max)

--Mingguan
,@PD_Minggu_M varchar(max)
,@PD_Bulan_M varchar(max)
,@PD_Tahun_M varchar(max)
,@TglMulai_M varchar(max)
,@TambahBulanMulai_M varchar(max)
,@TglAkhir_M varchar(max)
,@TambahBulanAkhir_M varchar(max)
,@BatasKeterlambatan_M varchar(max)

--Bulan
,@PD_Bulan_B varchar(max)
,@PD_Tahun_B varchar(max)
,@TglMulai_B varchar(max)
,@TglAkhir_B varchar(max)
,@TambahBulan_B varchar(max)
,@BatasKeterlambatan_B varchar(max)

--Triwulan
,@PD_Bulan_T varchar(max)
,@PD_Tahun_T varchar(max)
,@TglMulai_T varchar(max)
,@TglAkhir_T varchar(max)
,@TambahBulan_T varchar(max)
,@BatasKeterlambatan_T varchar(max)

--Semesteran
,@PD_Bulan_S varchar (max)
,@PD_Tahun_S varchar (max)
,@TglMulai_S varchar (max)
,@TglAkhir_S varchar (max)
,@TambahBulan_S varchar (max)
,@BatasKeterlambatan_S varchar (max)

--Tahunan
,@PD_Tahun_TH varchar (max)
,@TglMulai_TH varchar (max)
,@TglAkhir_TH varchar (max)
,@TambahBulan_TH varchar (max)
,@BatasKeterlambatan_TH varchar (max)
  
,@by varchar(max)  
  
as  
  
set @PeriodType = dbo.fn_StripCharacters(@PeriodType, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PeriodName = dbo.fn_StripCharacters(@PeriodName, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PeriodName_Ori = dbo.fn_StripCharacters(@PeriodName_Ori, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @fgmove = dbo.fn_StripCharacters(@fgmove, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PeriodData_H = dbo.fn_StripCharacters(@PeriodData_H, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglBuka_H = dbo.fn_StripCharacters(@TglBuka_H, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @JamBuka_H = dbo.fn_StripCharacters(@JamBuka_H, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglTutup_H = dbo.fn_StripCharacters(@TglTutup_H, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @JamTutup_H = dbo.fn_StripCharacters(@JamTutup_H, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @UseKoreksi = dbo.fn_StripCharacters(@UseKoreksi, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @JumlahHariKoreksi = dbo.fn_StripCharacters(@JumlahHariKoreksi, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @JamBukaKoreksi = dbo.fn_StripCharacters(@JamBukaKoreksi, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @JamTutupKoreksi = dbo.fn_StripCharacters(@JamTutupKoreksi, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Minggu_M = dbo.fn_StripCharacters(@PD_Minggu_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Bulan_M = dbo.fn_StripCharacters(@PD_Bulan_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Tahun_M = dbo.fn_StripCharacters(@PD_Tahun_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglMulai_M = dbo.fn_StripCharacters(@TglMulai_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TambahBulanMulai_M = dbo.fn_StripCharacters(@TambahBulanMulai_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglAkhir_M = dbo.fn_StripCharacters(@TglAkhir_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TambahBulanAkhir_M = dbo.fn_StripCharacters(@TambahBulanAkhir_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @BatasKeterlambatan_M = dbo.fn_StripCharacters(@BatasKeterlambatan_M, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Bulan_B = dbo.fn_StripCharacters(@PD_Bulan_B, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Tahun_B = dbo.fn_StripCharacters(@PD_Tahun_B, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglMulai_B = dbo.fn_StripCharacters(@TglMulai_B, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglAkhir_B = dbo.fn_StripCharacters(@TglAkhir_B, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TambahBulan_B = dbo.fn_StripCharacters(@TambahBulan_B, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @BatasKeterlambatan_B = dbo.fn_StripCharacters(@BatasKeterlambatan_B, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Bulan_T = dbo.fn_StripCharacters(@PD_Bulan_T, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @PD_Tahun_T = dbo.fn_StripCharacters(@PD_Tahun_T, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglMulai_T = dbo.fn_StripCharacters(@TglMulai_T, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TglAkhir_T = dbo.fn_StripCharacters(@TglAkhir_T, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @TambahBulan_T = dbo.fn_StripCharacters(@TambahBulan_T, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    
set @BatasKeterlambatan_T = dbo.fn_StripCharacters(@BatasKeterlambatan_T, '^a-zA-Z0-9 [-]_@.!#$&/\[]')    

Declare @ecode int,@@id int,@isexists int=0 ,@msg varchar(100)='Nama periode telah terdaftar!'
  
Begin Tran  
  
set @ecode = @@ERROR  

select @isexists= count(1) from dbo.MSTipePeriode where PeriodName=@PeriodName 
  
if @ecode = 0 and @isexists=0 Begin  
   INSERT INTO dbo.MSTipePeriode  
        (PeriodName,  
         PeriodType,  
		fgmove,
		FgType)  
     VALUES  
           (@PeriodName,  
     @PeriodType,  
     @fgmove,
	 'E')  
 select @@id=@@IDENTITY  
 set @ecode = @ecode + @@ERROR  
end  
  
if @ecode = 0  and @isexists=0 Begin  
 --log  
 insert into dbo.ATMSTipePeriode(PeriodId,PeriodName,PeriodType,FgMove,FgType,UpdDate,UpdUser,UpdFlag)  
  select PeriodId,PeriodName,PeriodType,FgMove,'E',getdate(),@by,'I'   
  from dbo.MSTipePeriode   
  where PeriodId=@@id  
  
   set @ecode = @ecode + @@ERROR  
end  
  
IF @PeriodType='Harian' AND NOT EXISTS(SELECT * FROM dbo.MSTipePeriodeHarian WHERE PeriodName=@PeriodName)  
 BEGIN  
	if @ecode = 0 and @isexists=0 Begin
  select @isexists= count(1) from dbo.MSTipePeriodeHarianExc
  where PeriodNameOri=@PeriodName_Ori and Periodedata=@PeriodData_H
  if @isexists > 0 begin set @msg= 'Exception dengan periode default dan periode data tersebut telah ada.' end
  end

 if @ecode = 0  and @isexists=0 Begin  
  INSERT INTO dbo.MSTipePeriodeHarianExc  
       (PeriodName  
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
	   ,[JamTutupKoreksi]
	   )  
    VALUES  
       (@PeriodName 
       ,@PeriodName_Ori
       ,@fgmove
       ,@PeriodData_H
       ,@TglBuka_H
	   ,@JamBuka_H
	   ,@TglTutup_H
	   ,@JamTutup_H
	   ,@UseKoreksi
	   ,@JumlahHariKoreksi
	   ,@JamBukaKoreksi
	   ,@JamTutupKoreksi
	   )  
  select @@id=@@IDENTITY  
  set @ecode = @ecode + @@ERROR  
 end  
  
	 --log  
	 if @ecode = 0 and @isexists=0 Begin  
	  insert into dbo.ATMSTipePeriodeHarianExc (   
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
	   ,[JamTutupKoreksi],@by upduser,getdate() upddate, 'I' updflag   
	  from dbo.MSTipePeriodeHarianExc 
	  where TipePeriodeId=@@id  
	  set @ecode = @ecode + @@ERROR  
	 end  
 END  
--if @ecode = 0 Begin  
  
IF @PeriodType='Mingguan' AND NOT EXISTS(SELECT * FROM dbo.MSTipePeriodeMingguan WHERE PeriodName=@PeriodName)  
 BEGIN  
	if @ecode = 0 and @isexists=0 Begin
   select @isexists= count(1) from dbo.MSTipePeriodeMingguanExc
  where PeriodNameOri=@PeriodName_Ori and MingguPeriodedata=@PD_Minggu_M and BulanPeriodedata=@PD_Bulan_M and TahunPeriodeData=@PD_Tahun_M
  if @isexists > 0 begin set @msg= 'Exception dengan periode default dan periode data tersebut telah ada.' end
  end

  if @ecode = 0 and @isexists=0 Begin  
   INSERT INTO dbo.MSTipePeriodeMingguanExc
      (PeriodName  
		,[PeriodNameOri]
		,[FgMove]
		,[MingguPeriodedata]
		,[BulanPeriodedata]
		,[TahunPeriodeData]
		,[MulaiBatasPelaporan]
		,[PenambahanBulanMulai]
		,[AkhirBatasPelaporan]
		,[PenambahanBulanAkhir]
		,[BatasKeterlambatan]
	 )  
   VALUES  
      (@PeriodName  
		,@PeriodName_Ori
		,@fgmove
		,@PD_Minggu_M
		,@PD_Bulan_M
		,@PD_Tahun_M
		,@TglMulai_M
		,@TambahBulanMulai_M
		,@TglAkhir_M
		,@TambahBulanAkhir_M
		,@BatasKeterlambatan_M
		)  
   select @@id=@@IDENTITY  
   set @ecode = @ecode + @@ERROR  
  end  
  --log  
  if @ecode = 0 and @isexists=0 Begin  
   insert into dbo.ATMSTipePeriodeMingguanExc (  
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
   select TipePeriodeId,  
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
	,[BatasKeterlambatan]
    ,@by ,getdate(),'I'    
   from dbo.MSTipePeriodeMingguanExc  
   where TipePeriodeId=@@id  
   set @ecode = @ecode + @@ERROR  
  end  
 END  
  
IF @PeriodType='Bulanan' AND NOT EXISTS(SELECT * FROM dbo.MSTipePeriodeBulanan WHERE PeriodName=@PeriodName)  
 BEGIN  
	if @ecode = 0 and @isexists=0 Begin
  select @isexists= count(1) from dbo.MSTipePeriodeBulananExc
  where PeriodNameOri=@PeriodName_Ori and BulanPeriodedata=@PD_Bulan_B and TahunPeriodeData=@PD_Tahun_B
  if @isexists > 0 begin set @msg= 'Exception dengan periode default dan periode data tersebut telah ada.' end
  end

  if @ecode = 0 and @isexists=0 Begin  
   INSERT INTO dbo.MSTipePeriodeBulananExc
      (PeriodName  
      ,[PeriodNameOri]
	  ,[FgMove]
	  ,[BulanPeriodedata]
	  ,[TahunPeriodeData]
	  ,[MulaiBatasPelaporan]
	  ,[AkhirBatasPelaporan]
	  ,[PenambahanBulan]
	  ,[BatasKeterlambatan]
	  )  
   VALUES  
      (@PeriodName  
      ,@PeriodName_Ori
	  ,@fgmove
	  ,@PD_Bulan_B
	  ,@PD_Tahun_B
	  ,@TglMulai_B
	  ,@TglAkhir_B
	  ,@TambahBulan_B
	  ,@BatasKeterlambatan_B
	  )  
   select @@id=@@IDENTITY  
   set @ecode = @ecode + @@ERROR  
  end  
  --log  
  if @ecode = 0 and @isexists=0 Begin  
   insert into dbo.ATMSTipePeriodeBulananExc(  
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
   select   
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
    @by,  
    getdate(),  
    'I'    
   from dbo.MSTipePeriodeBulananExc   
   where TipePeriodeId=@@id  
  set @ecode = @ecode + @@ERROR  
  end  
 END  
  
IF @PeriodType='Triwulanan' AND NOT EXISTS(SELECT * FROM dbo.MSTipePeriodeTriwulan WHERE PeriodName=@PeriodName)  
 BEGIN  
	if @ecode = 0 and @isexists=0 Begin
	  select @isexists= count(1) from dbo.MSTipePeriodeTriwulanExc
	  where PeriodNameOri=@PeriodName_Ori and TriwulanPeriodedata=@PD_Bulan_T and TahunPeriodeData=@PD_Tahun_T
	  if @isexists > 0 begin set @msg= 'Exception dengan periode default dan periode data tersebut telah ada.' end
	end

 if @ecode = 0 and @isexists=0 Begin  
  INSERT INTO dbo.MSTipePeriodeTriwulanExc 
		(PeriodName  
		,[PeriodNameOri]
		,[FgMove]
		,[TriwulanPeriodedata]
		,[TahunPeriodeData]
		,[MulaiBatasPelaporan]
		,[AkhirBatasPelaporan]
		,[PenambahanBulan]
		,[BatasKeterlambatan]
		)  
     VALUES  
           (@PeriodName  
			,@PeriodName_Ori
			,@fgmove
			,@PD_Bulan_T
			,@PD_Tahun_T
			,@TglMulai_T
			,@TglAkhir_T
			,@TambahBulan_T
			,@BatasKeterlambatan_T
		)  
  select @@id=@@IDENTITY  
  set @ecode = @ecode + @@ERROR  
 end  
 --log  
 if @ecode = 0 and @isexists=0 Begin  
  insert into dbo.ATMSTipePeriodeTriwulanExc(  
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
  select   
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
   @by,  
   getdate(),  
   'I'    
  from dbo.MSTipePeriodeTriwulanExc   
  where TipePeriodeId=@@id  
 set @ecode = @ecode + @@ERROR  
 end  
 END  

IF @PeriodType='Semesteran' AND NOT EXISTS(SELECT * FROM dbo.MSTipePeriodeSemester WHERE PeriodName=@PeriodName)  
 BEGIN  
	if @ecode = 0 and @isexists=0 Begin
		select @isexists= count(1) from dbo.MSTipePeriodeSemesterExc
		where PeriodNameOri=@PeriodName_Ori and SemesterPeriodedata=@PD_Bulan_S and TahunPeriodeData=@PD_Tahun_S
		
		if @isexists > 0 begin set @msg= 'Exception dengan periode default dan periode data tersebut telah ada.' end
	end

 if @ecode = 0 and @isexists=0 Begin  
  INSERT INTO dbo.MSTipePeriodeSemesterExc 
		(PeriodName  
		,[PeriodNameOri]
		,[FgMove]
		,[SemesterPeriodedata]
		,[TahunPeriodeData]
		,[MulaiBatasPelaporan]
		,[AkhirBatasPelaporan]
		,[PenambahanBulan]
		,[BatasKeterlambatan]
		)  
     VALUES  
           (@PeriodName  
			,@PeriodName_Ori
			,@fgmove
			,@PD_Bulan_S
			,@PD_Tahun_S
			,@TglMulai_S
			,@TglAkhir_S
			,@TambahBulan_S
			,@BatasKeterlambatan_S
		)  
  select @@id=@@IDENTITY  
  set @ecode = @ecode + @@ERROR  
 end  
 --log  
 if @ecode = 0 and @isexists=0 Begin  
  insert into dbo.ATMSTipePeriodeSemesterExc(  
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
   UpdUser,  
   UpdDate,  
   UpdFlag)  
  select   
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
   @by,  
   getdate(),  
   'I'    
  from dbo.MSTipePeriodeSemesterExc   
  where TipePeriodeId=@@id  
 set @ecode = @ecode + @@ERROR  
 end  
 END  
 
IF @PeriodType='Tahunan' AND NOT EXISTS(SELECT * FROM dbo.MSTipePeriodeTahun WHERE PeriodName=@PeriodName)  
 BEGIN  
	if @ecode = 0 and @isexists=0 Begin
  select @isexists= count(1) from dbo.MSTipePeriodeTahunExc
  where PeriodNameOri=@PeriodName_Ori  and TahunPeriodeData=@PD_Tahun_TH
  if @isexists > 0 begin set @msg= 'Exception dengan periode default dan periode data tersebut telah ada.' end
  end

  if @ecode = 0 and @isexists=0 Begin  
   INSERT INTO dbo.MSTipePeriodeTahunExc
      (PeriodName  
      ,[PeriodNameOri]
	  ,[FgMove]
	  ,[TahunPeriodeData]
	  ,[MulaiBatasPelaporan]
	  ,[AkhirBatasPelaporan]
	  ,[PenambahanBulan]
	  ,[BatasKeterlambatan]
	  )  
   VALUES  
      (@PeriodName  
      ,@PeriodName_Ori
	  ,@fgmove
	  ,@PD_Tahun_TH
	  ,@TglMulai_TH
	  ,@TglAkhir_TH
	  ,@TambahBulan_TH
	  ,@BatasKeterlambatan_TH
	  )  
   select @@id=@@IDENTITY  
   set @ecode = @ecode + @@ERROR  
  end  
  --log  
  if @ecode = 0 and @isexists=0 Begin  
   insert into dbo.ATMSTipePeriodeTahunExc(  
    TipePeriodeId,  
    PeriodName  
    ,[PeriodNameOri]
	,[FgMove]
	,[TahunPeriodeData]
	,[MulaiBatasPelaporan]
	,[AkhirBatasPelaporan]
	,[PenambahanBulan]
	,[BatasKeterlambatan],
    UpdUser,  
    UpdDate,  
    UpdFlag)  
   select   
    TipePeriodeId,  
    PeriodName  
    ,[PeriodNameOri]
	,[FgMove]
	,[TahunPeriodeData]
	,[MulaiBatasPelaporan]
	,[AkhirBatasPelaporan]
	,[PenambahanBulan]
	,[BatasKeterlambatan],
    @by,  
    getdate(),  
    'I'    
   from dbo.MSTipePeriodeTahunExc   
   where TipePeriodeId=@@id  
  set @ecode = @ecode + @@ERROR  
  end  
 END  
   

if @ecode = 0 and @@ERROR = 0 and @isexists=0  
Begin  
  commit tran  
end  
else Begin  
  rollback tran  
end  
  
select @isexists as isexists ,@msg as msg
