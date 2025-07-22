USE [antasenaDBTest]
GO
/****** Object:  StoredProcedure [dbo].[tp_updateTipePeriode]    Script Date: 7/13/2025 3:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[tp_updateTipePeriode]
	@PeriodId int,
	@PeriodName varchar(max),
	@PeriodType varchar(max),
	@fgmove varchar(max),

	@tipeharian varchar(max),
	@JamBukaPelaporan varchar(max),
	@JamTutupAtasPelaporan varchar(max),
	@UseKoreksi varchar(max),
	@JumlahHariKoreksi varchar(max),
	@JamBukaKoreksi varchar(max),
	@JamTutupAtasKoreksi varchar(max),
	@JumlahHariPelaporan int,

	@Minggu1MulaiBatasPelaporan varchar(max),
	@Minggu1AkhirBatasPelaporan varchar(max),
	@Minggu2MulaiBatasPelaporan varchar(max),
	@Minggu2AkhirBatasPelaporan varchar(max),
	@Minggu3MulaiBatasPelaporan varchar(max),
	@Minggu3AkhirBatasPelaporan varchar(max),
	@Minggu4MulaiBatasPelaporan varchar(max),
	@Minggu4AkhirBatasPelaporan varchar(max),
	@MPeriodeData2 varchar(max),
	@MPeriodeData3 varchar(max),
	@MPeriodeData4 varchar(max),
	@MBatasKeterlambatan varchar(max),

	@MulaiBatasPelaporan varchar(max),
	@AkhirBatasPelaporan varchar(max),
	@BBatasKeterlambatan varchar(max),

	@Triwulan1MulaiBatasPelaporan varchar(max),
	@Triwulan1AkhirBatasPelaporan varchar(max),
	@Triwulan2MulaiBatasPelaporan varchar(max),
	@Triwulan2AkhirBatasPelaporan varchar(max),
	@Triwulan3MulaiBatasPelaporan varchar(max),
	@Triwulan3AkhirBatasPelaporan varchar(max),
	@Triwulan4MulaiBatasPelaporan varchar(max),
	@Triwulan4AkhirBatasPelaporan varchar(max),
	@TBatasKeterlambatan varchar(max),

	@Semester1MulaiBatasPelaporan varchar(max),
	@Semester1AkhirBatasPelaporan varchar(max),
	@Semester2AkhirBatasPelaporan varchar(max),
	@Semester2MulaiBatasPelaporan varchar(max),
	@BatasKeterlambatan varchar(max),

	@TahunMulaiBatasPelaporan varchar(max),
	@TahunAkhirBatasPelaporan varchar(max),

	@by varchar(max)
as

--DECLARE @PeriodName_ varchar(max)=(select PeriodName from dbo.MSTipePeriode WHERE PeriodId = @PeriodId)


Declare @ecode int

Begin Tran

set @ecode = @@ERROR

IF @PeriodType='Harian'
BEGIN
	if @ecode = 0 Begin
		UPDATE [dbo].[MSTipePeriodeHarian]
		   SET PeriodName = @PeriodName,
			  JamBukaPelaporan = @JamBukaPelaporan,
			  JamBukaKoreksi = @JamBukaKoreksi,
			  JamTutupAtasPelaporan = @JamTutupAtasPelaporan,
			  JamTutupAtasKoreksi = @JamTutupAtasKoreksi,
			  JumlahHariKoreksi=@JumlahHariKoreksi,
			  TipeHarian=@TipeHarian,
			  UseKoreksi = @UseKoreksi,
			  JumlahHariPelaporan=@JumlahHariPelaporan
		 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeHarian
			(
			TipePeriodeId,
			PeriodName,
			JamBukaPelaporan,
			JamBukaKoreksi,
			JamTutupAtasPelaporan,
			JamTutupAtasKoreksi,
			JumlahHariKoreksi,
			TipeHarian,
			UseKoreksi,
			UpdUser,
			UpdDate,
			UpdFlag,
			JumlahHariPelaporan
			)
		select TipePeriodeId,
			PeriodName,
			JamBukaPelaporan,
			JamBukaKoreksi,
			JamTutupAtasPelaporan,
			JamTutupAtasKoreksi,
			JumlahHariKoreksi,
			TipeHarian,
			UseKoreksi, @by upduser, getdate() upddate, 'E' updflag, JumlahHariPelaporan
		from dbo.MSTipePeriodeHarian
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Mingguan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeMingguan
   SET PeriodName = @PeriodName
      ,Minggu1MulaiBatasPelaporan = @Minggu1MulaiBatasPelaporan
      ,Minggu1AkhirBatasPelaporan = @Minggu1AkhirBatasPelaporan
      ,Minggu2MulaiBatasPelaporan = @Minggu2MulaiBatasPelaporan
      ,Minggu2AkhirBatasPelaporan = @Minggu2AkhirBatasPelaporan
      ,Minggu3MulaiBatasPelaporan = @Minggu3MulaiBatasPelaporan
      ,Minggu3AkhirBatasPelaporan = @Minggu3AkhirBatasPelaporan
      ,Minggu4MulaiBatasPelaporan = @Minggu4MulaiBatasPelaporan
      ,Minggu4AkhirBatasPelaporan = @Minggu4AkhirBatasPelaporan
	,[MPeriodeData2]=@MPeriodeData2
	,[MPeriodeData3]=@MPeriodeData3
	,[MPeriodeData4]=@MPeriodeData4
      ,BatasKeterlambatan = @MBatasKeterlambatan
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeMingguan
			(
			TipePeriodeId,
			PeriodName,
			Minggu1MulaiBatasPelaporan,
			Minggu1AkhirBatasPelaporan,
			Minggu2MulaiBatasPelaporan,
			Minggu2AkhirBatasPelaporan,
			Minggu3MulaiBatasPelaporan,
			Minggu3AkhirBatasPelaporan,
			Minggu4MulaiBatasPelaporan,
			Minggu4AkhirBatasPelaporan,
			[MPeriodeData2],
			[MPeriodeData3],
			[MPeriodeData4],
			BatasKeterlambatan,
			UpdUser,
			UpdDate,
			UpdFlag)
		select TipePeriodeId,
			PeriodName,
			Minggu1MulaiBatasPelaporan,
			Minggu1AkhirBatasPelaporan,
			Minggu2MulaiBatasPelaporan,
			Minggu2AkhirBatasPelaporan,
			Minggu3MulaiBatasPelaporan,
			Minggu3AkhirBatasPelaporan,
			Minggu4MulaiBatasPelaporan,
			Minggu4AkhirBatasPelaporan,
			MPeriodeData2,
			MPeriodeData3,
			MPeriodeData4,
			BatasKeterlambatan, @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeMingguan
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Bulanan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeBulanan
   SET PeriodName = @PeriodName,
      MulaiBatasPelaporan = @MulaiBatasPelaporan,
      AkhirBatasPelaporan = @AkhirBatasPelaporan,
      BatasKeterlambatan = @BBatasKeterlambatan
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeBulanan
			(
			TipePeriodeId,
			PeriodName,
			MulaiBatasPelaporan,
			AkhirBatasPelaporan,
			BatasKeterlambatan,
			UpdUser,
			UpdDate,
			UpdFlag)
		select TipePeriodeId,
			PeriodName,
			MulaiBatasPelaporan,
			AkhirBatasPelaporan,
			BatasKeterlambatan, @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeBulanan
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Triwulanan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeTriwulan
   SET PeriodName = @PeriodName,
      Triwulan1MulaiBatasPelaporan = @Triwulan1MulaiBatasPelaporan,
      Triwulan1AkhirBatasPelaporan = @Triwulan1AkhirBatasPelaporan,
      Triwulan2MulaiBatasPelaporan = @Triwulan2MulaiBatasPelaporan,
      Triwulan2AkhirBatasPelaporan = @Triwulan2AkhirBatasPelaporan,
      Triwulan3MulaiBatasPelaporan = @Triwulan3MulaiBatasPelaporan,
      Triwulan3AkhirBatasPelaporan = @Triwulan3AkhirBatasPelaporan,
      Triwulan4MulaiBatasPelaporan = @Triwulan4MulaiBatasPelaporan,
      Triwulan4AkhirBatasPelaporan = @Triwulan4AkhirBatasPelaporan,
      BatasKeterlambatan = @TBatasKeterlambatan
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeTriwulan
			(
			TipePeriodeId,
			PeriodName,
			Triwulan1MulaiBatasPelaporan,
			Triwulan1AkhirBatasPelaporan,
			Triwulan2MulaiBatasPelaporan,
			Triwulan2AkhirBatasPelaporan,
			Triwulan3MulaiBatasPelaporan,
			Triwulan3AkhirBatasPelaporan,
			Triwulan4MulaiBatasPelaporan,
			Triwulan4AkhirBatasPelaporan,
			BatasKeterlambatan,
			UpdUser,
			UpdDate,
			UpdFlag)
		select TipePeriodeId,
			PeriodName,
			Triwulan1MulaiBatasPelaporan,
			Triwulan1AkhirBatasPelaporan,
			Triwulan2MulaiBatasPelaporan,
			Triwulan2AkhirBatasPelaporan,
			Triwulan3MulaiBatasPelaporan,
			Triwulan3AkhirBatasPelaporan,
			Triwulan4MulaiBatasPelaporan,
			Triwulan4AkhirBatasPelaporan,
			BatasKeterlambatan, @by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeTriwulan
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Semesteran'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeSemester
   SET PeriodName = @PeriodName,
      Semester1MulaiBatasPelaporan = @Semester1MulaiBatasPelaporan,
      Semester1AkhirBatasPelaporan = @Semester1AkhirBatasPelaporan,
      Semester2MulaiBatasPelaporan = @Semester2MulaiBatasPelaporan,
      Semester2AkhirBatasPelaporan = @Semester2AkhirBatasPelaporan,
	  	BatasKeterlambatan = @BatasKeterlambatan
 WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeSemester
			(
			TipePeriodeId,
			PeriodName,
			Semester1MulaiBatasPelaporan,
			Semester1AkhirBatasPelaporan,
			Semester2MulaiBatasPelaporan,
			Semester2AkhirBatasPelaporan,
			BatasKeterlambatan,
			UpdUser,
			UpdDate,
			UpdFlag)
		select TipePeriodeId,
			PeriodName,
			Semester1MulaiBatasPelaporan,
			Semester1AkhirBatasPelaporan,
			Semester2MulaiBatasPelaporan,
			Semester2AkhirBatasPelaporan,
			BatasKeterlambatan,
			@by upduser,
			getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeSemester
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END

IF @PeriodType='Tahunan'
BEGIN
	if @ecode = 0 Begin
		UPDATE dbo.MSTipePeriodeTahun
	SET	PeriodName = @PeriodName,
		MulaiBatasPelaporan = @TahunMulaiBatasPelaporan,
		AkhirBatasPelaporan = @TahunAkhirBatasPelaporan,
		BatasKeterlambatan= @BatasKeterlambatan
	WHERE PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
	--log
	if @ecode = 0 Begin
		insert into dbo.ATMSTipePeriodeTahun
			(
			TipePeriodeId,
			PeriodName,
			MulaiBatasPelaporan,
			AkhirBatasPelaporan,
			BatasKeterlambatan,
			UpdUser,
			UpdDate,
			UpdFlag)
		select TipePeriodeId,
			PeriodName,
			MulaiBatasPelaporan,
			AkhirBatasPelaporan,
			BatasKeterlambatan,
			@by upduser, getdate() upddate, 'E' updflag
		from dbo.MSTipePeriodeTahun
		where PeriodName=@PeriodName
		set @ecode = @ecode + @@ERROR
	end
END




if @ecode = 0 Begin
	update dbo.MSTipePeriode set FgMove=@fgmove
	where PeriodName=@PeriodName
	set @ecode = @ecode + @@ERROR
end

if @ecode = 0 Begin
	--log
	insert into dbo.ATMSTipePeriode
		(PeriodId,PeriodName,PeriodType,FgMove,UpdDate,UpdUser,UpdFlag)
	select @PeriodId, @PeriodName, @PeriodType, @FgMove, getdate(), @by, 'E'
	set @ecode = @ecode + @@ERROR
end

if @ecode = 0 and @@ERROR = 0
Begin
	commit tran
end
else Begin
	rollback tran
end
