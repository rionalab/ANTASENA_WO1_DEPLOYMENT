SET
  ANSI_NULLS ON
GO
SET
  QUOTED_IDENTIFIER ON
GO
  -- =============================================    
  -- Author:  YAQUB    
  -- Create date: 2019-06-20    
  -- Description:     
  -- =============================================    
  ALTER procedure [dbo].[tp_getTipePeriode] @FgType VARCHAR(1) = '' AS IF @FgType = '' BEGIN
SELECT
  [PeriodId],
  [PeriodName],
  [PeriodType],
  fgmove
FROM
  [dbo].[MSTipePeriode]
WHERE
  FgType IS NULL
END IF @FgType != '' BEGIN
SELECT
  a.[PeriodId],
  a.PeriodName,
  CASE
    WHEN b.PeriodNameOri IS NOT NULL THEN b.PeriodNameOri
    WHEN c.PeriodNameOri IS NOT NULL THEN c.PeriodNameOri
    WHEN d.PeriodNameOri IS NOT NULL THEN d.PeriodNameOri
    WHEN e.PeriodNameOri IS NOT NULL THEN e.PeriodNameOri
    WHEN f.PeriodNameOri IS NOT NULL THEN f.PeriodNameOri
    WHEN g.PeriodNameOri IS NOT NULL THEN g.PeriodNameOri
    ELSE ''
  END AS PeriodNameOri,
  a.[PeriodType],
  a.fgmove,
  a.FgType,
  CASE
    WHEN b.Periodedata IS NOT NULL THEN FORMAT(CONVERT(DATE, b.Periodedata), 'dd-MMM-yyyy')
    WHEN c.MingguPeriodedata IS NOT NULL THEN 'Minggu: ' + CONVERT(VARCHAR, c.MingguPeriodedata) + ' ' + CONVERT(
      VARCHAR(3),
      DATENAME(MONTH, DATEADD(MONTH, c.BulanPeriodedata, -1))
    ) + '-' + CONVERT(VARCHAR, c.TahunPeriodeData)
    WHEN d.BulanPeriodedata IS NOT NULL THEN 'Bulan: ' + CONVERT(
      VARCHAR(3),
      DATENAME(MONTH, DATEADD(MONTH, d.BulanPeriodedata, -1))
    ) + '-' + CONVERT(VARCHAR, d.TahunPeriodeData)
    WHEN e.TriwulanPeriodedata IS NOT NULL THEN 'Triwulan: ' + CONVERT(VARCHAR, e.TriwulanPeriodedata) + ' ' + CONVERT(VARCHAR, e.TahunPeriodeData)
    WHEN f.SemesterPeriodedata IS NOT NULL THEN 'Semester: ' + CONVERT(VARCHAR, f.SemesterPeriodedata) + ' ' + CONVERT(VARCHAR, f.TahunPeriodeData)
    WHEN g.TahunPeriodedata IS NOT NULL THEN 'Tahun: ' + CONVERT(VARCHAR, g.TahunPeriodeData)
    ELSE ''
  END AS PeriodData,
  dbo.tp_statusBtnExc(a.PeriodType, a.PeriodName) as statusBtn
FROM
  [dbo].[MSTipePeriode] as a
  left join MSTipePeriodeHarianExc as b on a.PeriodName = b.PeriodName
  left join MSTipePeriodeMingguanExc as c on a.PeriodName = c.PeriodName
  left join MSTipePeriodeBulananExc as d on a.PeriodName = d.PeriodName
  left join MSTipePeriodeTriwulanExc as e on a.PeriodName = e.PeriodName
  left join MSTipePeriodeSemesterExc as f on a.PeriodName = f.PeriodName
  left join MSTipePeriodeTahunExc as g on a.PeriodName = g.PeriodName
where
  a.FgType = 'E'
end