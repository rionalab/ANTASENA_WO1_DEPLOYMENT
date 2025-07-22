USE [antasenaDBTest]
GO
/****** Object:  StoredProcedure [dbo].[tp_updateTipePeriode_EX]    Script Date: 7/13/2025 8:17:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




ALTER procedure [dbo].[tp_updateTipePeriode_EX]
	@PeriodId int,
	@PeriodType varchar(max),
	@PeriodName varchar(max),
	@PeriodName_Ori varchar(max),
	@fgmove varchar(max) 

--Harian
,
	@PeriodData_H varchar(max)
,
	@TglBuka_H varchar(max)
,
	@JamBuka_H varchar(max)
,
	@TglTutup_H varchar(max)
,
	@JamTutup_H varchar(max)
,
	@UseKoreksi varchar(max)
,
	@JumlahHariKoreksi varchar(max)
,
	@JamBukaKoreksi varchar(max)
,
	@JamTutupKoreksi varchar(max)

--Mingguan
,
	@PD_Minggu_M varchar(max)
,
	@PD_Bulan_M varchar(max)
,
	@PD_Tahun_M varchar(max)
,
	@TglMulai_M varchar(max)
,
	@TambahBulanMulai_M varchar(max)
,
	@TglAkhir_M varchar(max)
,
	@TambahBulanAkhir_M varchar(max)
,
	@BatasKeterlambatan_M varchar(max)

--Bulan
,
	@PD_Bulan_B varchar(max)
,
	@PD_Tahun_B varchar(max)
,
	@TglMulai_B varchar(max)
,
	@TglAkhir_B varchar(max)
,
	@TambahBulan_B varchar(max)
,
	@BatasKeterlambatan_B varchar(max)

--Triwulan
,
	@PD_Bulan_T varchar(max)
,
	@PD_Tahun_T varchar(max)
,
	@TglMulai_T varchar(max)
,
	@TglAkhir_T varchar(max)
,
	@TambahBulan_T varchar(max)
,
	@BatasKeterlambatan_T varchar(max)

--Semesteran
,
	@PD_Bulan_S varchar(max)
,
	@PD_Tahun_S varchar(max)
,
	@TglMulai_S varchar(max)
,
	@TglAkhir_S varchar(max)
,
	@TambahBulan_S varchar(max)
,
	@BatasKeterlambatan_S varchar(max)

--Tahunan
,
	@PD_Tahun_TH varchar(max)
,
	@TglMulai_TH varchar(max)
,
	@TglAkhir_TH varchar(max)
,
	@TambahBulan_TH varchar(max)
,
	@BatasKeterlambatan_TH varchar(max)


  
,
	@by varchar(max)
as

--DECLARE @PeriodName_ varchar(max)=(select PeriodName from dbo.MSTipePeriode WHERE PeriodId = @PeriodId)


Declare @ecode int

Begin Tran

set @ecode = @@ERROR

select @PeriodName_Ori=PeriodNameOri
from [MSTipePeriodeHarianExc]
WHERE PeriodName=@PeriodName
IF @PeriodType='Harian'
BEGIN
	if @ecode = 0 Begin
		UPDATE [dbo].[MSTipePeriodeHarianExc]
		   SET PeriodName = @PeriodName
				,PeriodNameOri=@PeriodName_Ori
				,FgMove=@fgmove
				,Periodedata=@PeriodData_H
				,[TanggalBukaPelaporan]=@TglBuka_H
				,[JamBukaPelaporan]=@JamBuka_H
				,[TanggalTutupPelaporan]=@TglTutup_H
				,[JamTutupPelaporan]=@JamTutup_H
				,[UseKoreksi]=@UseKoreksi
				,[JumlahHariKoreksi]=@JumlahHariKoreksi
				,[JamBukaKoreksi]=@JamBukaKoreksi
				,[JamTutupKoreksi]=@JamTutupKoreksi
		 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
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
			UpdFlag
			)
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
	   , [JamTutupKoreksi], @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeHarianExc
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Mingguan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeMingguanExc
   SET PeriodName = @PeriodName
		,[PeriodNameOri] = @PeriodName_Ori
		,[FgMove] = @fgmove
		,[MingguPeriodedata] = @PD_Minggu_M
		,[BulanPeriodedata] = @PD_Bulan_M
		,[TahunPeriodeData] = @PD_Tahun_M
		,[MulaiBatasPelaporan] = @TglMulai_M
		,[PenambahanBulanMulai] = @TambahBulanMulai_M
		,[AkhirBatasPelaporan] = @TglAkhir_M
		,[PenambahanBulanAkhir] = @TambahBulanAkhir_M
		,[BatasKeterlambatan] = @BatasKeterlambatan_M
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
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
		select TipePeriodeId,
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
		, [BatasKeterlambatan], @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeMingguanExc
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Bulanan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeBulananExc
   SET PeriodName = @PeriodName
		,[PeriodNameOri] = @PeriodName_Ori
		,[FgMove] = @fgmove
		,[BulanPeriodedata] = @PD_Bulan_B
		,[TahunPeriodeData] = @PD_Tahun_B
		,[MulaiBatasPelaporan] = @TglMulai_B
		,[AkhirBatasPelaporan] = @TglAkhir_B
		,[PenambahanBulan] = @TambahBulan_B
		,[BatasKeterlambatan] = @BatasKeterlambatan_B
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
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
	, [BatasKeterlambatan], @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeBulananExc
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Triwulanan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeTriwulanExc
   SET PeriodName = @PeriodName
		,[PeriodNameOri] = @PeriodName_Ori
		,[FgMove] = @fgmove
		,[TriwulanPeriodedata] = @PD_Bulan_T
		,[TahunPeriodeData] = @PD_Tahun_T
		,[MulaiBatasPelaporan] = @TglMulai_T
		,[AkhirBatasPelaporan] = @TglAkhir_T
		,[PenambahanBulan] = @TambahBulan_T
		,[BatasKeterlambatan] = @BatasKeterlambatan_T
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
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
		, [BatasKeterlambatan], @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeTriwulanExc
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END



IF @PeriodType='Semesteran'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeSemesterExc
   SET PeriodName = @PeriodName
		,[PeriodNameOri] = @PeriodName_Ori
		,[FgMove] = @fgmove
		,[SemesterPeriodedata] = @PD_Bulan_S
		,[TahunPeriodeData] = @PD_Tahun_S
		,[MulaiBatasPelaporan] = @TglMulai_S
		,[AkhirBatasPelaporan] = @TglAkhir_S
		,[PenambahanBulan] = @TambahBulan_S
		,[BatasKeterlambatan] = @BatasKeterlambatan_S
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
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
			UpdUser,
			UpdDate,
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
		, [BatasKeterlambatan], @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeSemesterExc
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END



IF @PeriodType='Tahunan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeTahunExc
   SET PeriodName = @PeriodName
		,[PeriodNameOri] = @PeriodName_Ori
		,[FgMove] = @fgmove
		,[TahunPeriodeData] = @PD_Tahun_TH
		,[MulaiBatasPelaporan] = @TglMulai_TH
		,[AkhirBatasPelaporan] = @TglAkhir_TH
		,[PenambahanBulan] = @TambahBulan_TH
		,[BatasKeterlambatan] = @BatasKeterlambatan_TH
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeTahunExc
			(
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
		select TipePeriodeId,
			PeriodName  
    , [PeriodNameOri]
	, [FgMove]
	, [TahunPeriodeData]
	, [MulaiBatasPelaporan]
	, [AkhirBatasPelaporan]
	, [PenambahanBulan]
	, [BatasKeterlambatan], @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeTahunExc
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END




if @ecode = 0 Begin
	update dbo.MSTipePeriode 
	set FgMove=@fgmove
	where PeriodName=@PeriodName
	set @ecode = @ecode + @@ERROR
end

if @ecode = 0 Begin
	--log
	insert into dbo.ATMSTipePeriode
		(PeriodId,PeriodName,PeriodType,FgMove,FgType,UpdDate,UpdUser,UpdFlag)
	select @PeriodId, @PeriodName, @PeriodType, @FgMove, 'E', getdate(), @by, 'E'
	set @ecode = @ecode + @@ERROR
end

if @ecode = 0 and @@ERROR = 0
Begin
	commit tran
end
else Begin
	rollback tran
end
