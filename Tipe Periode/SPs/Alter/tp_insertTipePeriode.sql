SET
	ANSI_NULLS ON
GO
SET
	QUOTED_IDENTIFIER ON
GO
	ALTER procedure [dbo].[tp_insertTipePeriode] @PeriodName varchar(max),
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
	@by varchar(max) as Declare @ecode int,
	@ @id int,
	@isexists int = 0 Begin Tran
set
	@ecode = @ @ERROR
select
	@isexists = count(1)
from
	dbo.MSTipePeriode
where
	PeriodName = @PeriodName if @ecode = 0
	and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriode (PeriodName, PeriodType, fgmove)
VALUES
	(@PeriodName, @PeriodType, @fgmove)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end if @ecode = 0
and @isexists = 0 Begin --log
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
	getdate(),
	@by,
	'I'
from
	dbo.MSTipePeriode
where
	PeriodId = @ @id
set
	@ecode = @ecode + @ @ERROR
end IF @PeriodType = 'Harian'
AND NOT EXISTS(
	SELECT
		*
	FROM
		dbo.MSTipePeriodeHarian
	WHERE
		PeriodName = @PeriodName
) BEGIN if @ecode = 0
and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriodeHarian (
		PeriodName,
		[JamBukaPelaporan],
		[JamBukaKoreksi],
		[JamTutupAtasPelaporan],
		[JamTutupAtasKoreksi],
		[JumlahHariKoreksi],
		[TipeHarian],
		[UseKoreksi],
		[JumlahHariPelaporan]
	)
VALUES
	(
		@PeriodName,
		@JamBukaPelaporan,
		@JamBukaKoreksi,
		@JamTutupAtasPelaporan,
		@JamTutupAtasKoreksi,
		@JumlahHariKoreksi,
		@TipeHarian,
		@UseKoreksi,
		@JumlahHariPelaporan
	)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end --log
if @ecode = 0
and @isexists = 0 Begin
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
	'I' updflag
from
	dbo.MSTipePeriodeHarian
where
	TipePeriodeId = @ @id
set
	@ecode = @ecode + @ @ERROR
end
END --if @ecode = 0 Begin
IF @PeriodType = 'Mingguan'
AND NOT EXISTS(
	SELECT
		*
	FROM
		dbo.MSTipePeriodeMingguan
	WHERE
		PeriodName = @PeriodName
) BEGIN if @ecode = 0
and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriodeMingguan (
		PeriodName,
		[Minggu1MulaiBatasPelaporan],
		[Minggu1AkhirBatasPelaporan],
		[Minggu2MulaiBatasPelaporan],
		[Minggu2AkhirBatasPelaporan],
		[Minggu3MulaiBatasPelaporan],
		[Minggu3AkhirBatasPelaporan],
		[Minggu4MulaiBatasPelaporan],
		[Minggu4AkhirBatasPelaporan],
		[MPeriodeData2],
		[MPeriodeData3],
		[MPeriodeData4],
		[BatasKeterlambatan]
	)
VALUES
	(
		@PeriodName,
		@Minggu1MulaiBatasPelaporan,
		@Minggu1AkhirBatasPelaporan,
		@Minggu2MulaiBatasPelaporan,
		@Minggu2AkhirBatasPelaporan,
		@Minggu3MulaiBatasPelaporan,
		@Minggu3AkhirBatasPelaporan,
		@Minggu4MulaiBatasPelaporan,
		@Minggu4AkhirBatasPelaporan,
		@MPeriodeData2,
		@MPeriodeData3,
		@MPeriodeData4,
		@MBatasKeterlambatan
	)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end --log
if @ecode = 0
and @isexists = 0 Begin
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
	'I'
from
	dbo.MSTipePeriodeMingguan
where
	TipePeriodeId = @ @id
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Bulanan'
AND NOT EXISTS(
	SELECT
		*
	FROM
		dbo.MSTipePeriodeBulanan
	WHERE
		PeriodName = @PeriodName
) BEGIN if @ecode = 0
and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriodeBulanan (
		PeriodName,
		[MulaiBatasPelaporan],
		[AkhirBatasPelaporan],
		[BatasKeterlambatan]
	)
VALUES
	(
		@PeriodName,
		@MulaiBatasPelaporan,
		@AkhirBatasPelaporan,
		@BBatasKeterlambatan
	)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end --log
if @ecode = 0
and @isexists = 0 Begin
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
	'I'
from
	dbo.MSTipePeriodeBulanan
where
	TipePeriodeId = @ @id
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Triwulanan'
AND NOT EXISTS(
	SELECT
		*
	FROM
		dbo.MSTipePeriodeTriwulan
	WHERE
		PeriodName = @PeriodName
) BEGIN if @ecode = 0
and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriodeTriwulan (
		PeriodName,
		Triwulan1MulaiBatasPelaporan,
		Triwulan1AkhirBatasPelaporan,
		Triwulan2MulaiBatasPelaporan,
		Triwulan2AkhirBatasPelaporan,
		Triwulan3MulaiBatasPelaporan,
		Triwulan3AkhirBatasPelaporan,
		Triwulan4MulaiBatasPelaporan,
		Triwulan4AkhirBatasPelaporan,
		BatasKeterlambatan
	)
VALUES
	(
		@PeriodName,
		@Triwulan1MulaiBatasPelaporan,
		@Triwulan1AkhirBatasPelaporan,
		@Triwulan2MulaiBatasPelaporan,
		@Triwulan2AkhirBatasPelaporan,
		@Triwulan3MulaiBatasPelaporan,
		@Triwulan3AkhirBatasPelaporan,
		@Triwulan4MulaiBatasPelaporan,
		@Triwulan4AkhirBatasPelaporan,
		@TBatasKeterlambatan
	)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end --log
if @ecode = 0
and @isexists = 0 Begin
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
	'I'
from
	dbo.MSTipePeriodeTriwulan
where
	TipePeriodeId = @ @id
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Semesteran'
AND NOT EXISTS(
	SELECT
		*
	FROM
		dbo.MSTipePeriodeSemester
	WHERE
		PeriodName = @PeriodName
) BEGIN if @ecode = 0
and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriodeSemester (
		PeriodName,
		Semester1MulaiBatasPelaporan,
		Semester1AkhirBatasPelaporan,
		Semester2MulaiBatasPelaporan,
		Semester2AkhirBatasPelaporan,
		BatasKeterlambatan
	)
VALUES
	(
		@PeriodName,
		@Semester1MulaiBatasPelaporan,
		@Semester1AkhirBatasPelaporan,
		@Semester2MulaiBatasPelaporan,
		@Semester2AkhirBatasPelaporan,
		@BatasKeterlambatan
	)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end --log
if @ecode = 0
and @isexists = 0 Begin
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
	'I'
from
	dbo.MSTipePeriodeSemester
where
	TipePeriodeId = @ @id
set
	@ecode = @ecode + @ @ERROR
end
END IF @PeriodType = 'Tahunan'
AND NOT EXISTS(
	SELECT
		*
	FROM
		dbo.MSTipePeriodeTahun
	WHERE
		PeriodName = @PeriodName
) BEGIN if @ecode = 0
and @isexists = 0 Begin
INSERT INTO
	dbo.MSTipePeriodeTahun (
		PeriodName,
		MulaiBatasPelaporan,
		AkhirBatasPelaporan,
		BatasKeterlambatan
	)
VALUES
	(
		@PeriodName,
		@TahunMulaiBatasPelaporan,
		@TahunAkhirBatasPelaporan,
		@BatasKeterlambatan
	)
select
	@ @id = @ @IDENTITY
set
	@ecode = @ecode + @ @ERROR
end --log
if @ecode = 0
and @isexists = 0 Begin
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
	'I'
from
	dbo.MSTipePeriodeTahun
where
	TipePeriodeId = @ @id
set
	@ecode = @ecode + @ @ERROR
end
END if @ecode = 0
and @ @ERROR = 0
and @isexists = 0 Begin commit tran
end
else Begin rollback tran
end
select
	@isexists as isexists