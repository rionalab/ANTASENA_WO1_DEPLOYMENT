USE [antasenaDBTest]
GO
  /****** Object:  StoredProcedure [dbo].[tp_getAllTipePeriodeByID]    Script Date: 7/13/2025 3:31:34 PM ******/
SET
  ANSI_NULLS ON
GO
SET
  QUOTED_IDENTIFIER ON
GO
  ALTER procedure [dbo].[tp_getAllTipePeriodeByID] --'14'  
  @PeriodId int as
set
  @PeriodId = dbo.fn_StripCharacters(@PeriodId, '^a-zA-Z0-9 [-]_@.!#$&/\[]') DECLARE @PeriodName varchar(max),
  @PeriodType varchar(max),
  @fgmove varchar(10),
  @FgType varchar(10);

SELECT
  @PeriodName = PeriodName,
  @PeriodType = PeriodType,
  @fgmove = fgmove,
  @FgType = FgType
FROM
  dbo.MSTipePeriode
WHERE
  [PeriodId] = @PeriodId IF @PeriodType = 'Harian'
  and @FgType is null BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeHarian
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Mingguan'
and @FgType is null BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeMingguan
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Bulanan'
and @FgType is null BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeBulanan
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Triwulanan'
and @FgType is null BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeTriwulan
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Semesteran'
and @FgType is null BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeSemester
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Tahunan'
and @FgType is null BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeTahun
WHERE
  PeriodName = @PeriodName
END -- EXC
IF @PeriodType = 'Harian'
and @FgType = 'E' BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  TipePeriodeId,
  PeriodName,
  PeriodNameOri,
  FgMove,
  format(convert(date, Periodedata), 'dd-MM-yyyy') as Periodedata,
  format(convert(date, TanggalBukaPelaporan), 'dd-MM-yyyy') as TanggalBukaPelaporan,
  JamBukaPelaporan,
  format(convert(date, TanggalTutupPelaporan), 'dd-MM-yyyy') as TanggalTutupPelaporan,
  JamTutupPelaporan,
  UseKoreksi,
  JumlahHariKoreksi,
  JamBukaKoreksi,
  JamTutupKoreksi
FROM
  MSTipePeriodeHarianExc
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Mingguan'
and @FgType = 'E' BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeMingguanExc
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Bulanan'
and @FgType = 'E' BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeBulananExc
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Triwulanan'
and @FgType = 'E' BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeTriwulanExc
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Semesteran'
and @FgType = 'E' BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeSemesterExc
WHERE
  PeriodName = @PeriodName
END IF @PeriodType = 'Tahunan'
and @FgType = 'E' BEGIN
SELECT
  @PeriodId as PeriodId,
  @PeriodType AS PeriodType,
  @fgmove as fgmove,
  *
FROM
  MSTipePeriodeTahunExc
WHERE
  PeriodName = @PeriodName
END