SET
	ANSI_NULLS ON
GO
SET
	QUOTED_IDENTIFIER ON
GO
	-- =============================================
	-- Author:		YAQUB
	-- Create date: 2019-06-20
	-- Description:	
	-- =============================================
	ALTER procedure [dbo].[tp_deleteTipePeriode] @PeriodId int,
	@by varchar(max) as Declare @ecode int Begin Tran
set
	@ecode = @ @ERROR DECLARE @PeriodName varchar(max),
	@PeriodType varchar(max),
	@PeriodeInf int,
	@del varchar(max)
select
	@PeriodeInf = count(1)
from
	[dbo].[PeriodeInformation]
where
	PeriodId = @PeriodId if @PeriodeInf = 0 begin
select
	@PeriodName = PeriodName,
	@PeriodType = PeriodType
from
	dbo.MSTipePeriode
WHERE
	PeriodId = @PeriodId IF @PeriodType = 'Harian' BEGIN --log
	if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriodeHarian (
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
		UpdFlag
	)
select
	TipePeriodeId,
	PeriodName,
	JamBukaPelaporan,
	JamBukaKoreksi,
	JamTutupAtasPelaporan,
	JamTutupAtasKoreksi,
	JumlahHariKoreksi,
	TipeHarian,
	UseKoreksi,
	@by upduser,
	getdate() upddate,
	'D' updflag
from
	dbo.MSTipePeriodeHarian
where
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriodeHarian]
WHERE
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Mingguan' BEGIN --log
if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriodeMingguan (
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
		UpdFlag
	)
select
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
	MPeriodeData2,
	MPeriodeData3,
	MPeriodeData4,
	BatasKeterlambatan,
	@by,
	getdate(),
	'D'
from
	dbo.MSTipePeriodeMingguan
where
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriodeMingguan]
WHERE
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Bulanan' BEGIN --log
if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriodeBulanan (
		TipePeriodeId,
		PeriodName,
		MulaiBatasPelaporan,
		AkhirBatasPelaporan,
		BatasKeterlambatan,
		UpdUser,
		UpdDate,
		UpdFlag
	)
select
	TipePeriodeId,
	PeriodName,
	MulaiBatasPelaporan,
	AkhirBatasPelaporan,
	BatasKeterlambatan,
	@by,
	getdate(),
	'D'
from
	dbo.MSTipePeriodeBulanan
where
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriodeBulanan]
WHERE
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Triwulanan' BEGIN --log
if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriodeTriwulan (
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
		UpdFlag
	)
select
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
	@by,
	getdate(),
	'D'
from
	dbo.MSTipePeriodeTriwulan
where
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriodeTriwulan]
WHERE
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Semesteran' BEGIN --log
if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriodeSemester (
		TipePeriodeId,
		PeriodName,
		Semester1MulaiBatasPelaporan,
		Semester1AkhirBatasPelaporan,
		Semester2MulaiBatasPelaporan,
		Semester2AkhirBatasPelaporan,
		BatasKeterlambatan,
		UpdUser,
		UpdDate,
		UpdFlag
	)
select
	TipePeriodeId,
	PeriodName,
	Semester1MulaiBatasPelaporan,
	Semester1AkhirBatasPelaporan,
	Semester2MulaiBatasPelaporan,
	Semester2AkhirBatasPelaporan,
	BatasKeterlambatan,
	@by,
	getdate(),
	'D'
from
	dbo.MSTipePeriodeSemester
where
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriodeSemester]
WHERE
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Tahunan' BEGIN --log
if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriodeTahun (
		TipePeriodeId,
		PeriodName,
		MulaiBatasPelaporan,
		AkhirBatasPelaporan,
		BatasKeterlambatan,
		UpdUser,
		UpdDate,
		UpdFlag
	)
select
	TipePeriodeId,
	PeriodName,
	MulaiBatasPelaporan,
	AkhirBatasPelaporan,
	BatasKeterlambatan,
	@by,
	getdate(),
	'D'
from
	dbo.MSTipePeriodeTahun
where
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriodeTahun]
WHERE
	PeriodName = @PeriodName
set
	@ecode = @ecode + @ @ERROR
end
END --log
if @ecode = 0 Begin
insert into
	dbo.ATMSTipePeriode (
		PeriodId,
		PeriodName,
		PeriodType,
		FgMove,
		UpdDate,
		UpdUser,
		UpdFlag
	)
select
	PeriodId,
	PeriodName,
	PeriodType,
	FgMove,
	getdate() upddate,
	@by upduser,
	'D' updflag
from
	dbo.MSTipePeriode
where
	PeriodId = @PeriodId
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0 Begin
DELETE FROM
	[dbo].[MSTipePeriode]
WHERE
	[PeriodId] = @PeriodId
end
set
	@del = 'Berhasil Hapus'
end
else begin
set
	@del = 'Tipe periode ini tidak boleh dihapus.'
end if @ecode = 0
and @ @ERROR = 0 Begin commit tran
end
else Begin rollback tran
end
select
	case
		when @PeriodeInf = 0 then 1
		else 0
	end isexists,
	@del message