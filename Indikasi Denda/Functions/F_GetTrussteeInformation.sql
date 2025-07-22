SET
   ANSI_NULLS ON
GO
SET
   QUOTED_IDENTIFIER ON
GO
   ALTER FUNCTION [dbo].[F_GetTrussteeInformation] (@UserName VARCHAR(100)) RETURNS @listInformasi TABLE (
      idinformation VARCHAR(255),
      namainformasi VARCHAR(255),
      kelompokinformasi VARCHAR(255),
      periode VARCHAR(50),
      periodestr VARCHAR(50),
      parentinformation VARCHAR(255),
      parentnamainformasi VARCHAR(255)
   ) AS BEGIN
SET
   @UserName = dbo.fn_StripCharacters(@UserName, '^a-zA-Z0-9 [-]_@.!#$&/\[]');

DECLARE @existsInformation VARCHAR(1);

IF NOT EXISTS (
   SELECT
      1
   FROM
      MSUser WITH (NOLOCK)
   WHERE
      UserName = @UserName
      AND FgActive = 'Y'
) RETURN;

IF EXISTS (
   SELECT
      1
   FROM
      MSUserInformation WITH (NOLOCK)
   WHERE
      UserName = @UserName
)
SET
   @existsInformation = '1';

ELSE
SET
   @existsInformation = '0';

IF (@existsInformation = '0') BEGIN
INSERT INTO
   @listInformasi (
      idinformation,
      namainformasi,
      kelompokinformasi,
      periode,
      periodestr,
      parentinformation,
      parentnamainformasi
   )
SELECT
   DISTINCT b.idinformation,
   b.namainformasi,
   b.kelompokinformasi,
   CASE
      WHEN c.PeriodType = 'Bulanan' THEN 'M'
      WHEN c.PeriodType = 'Harian' THEN 'D'
      WHEN c.PeriodType = 'Mingguan' THEN 'W'
      WHEN c.PeriodType = 'Triwulanan' THEN 'Q'
      WHEN c.PeriodType = 'Semesteran' THEN 'S'
      WHEN c.PeriodType = 'Tahunan' THEN 'A'
   END,
   c.PeriodType,
   b.parentinformation,
   b.parentnamainformasi
FROM
   PeriodeInformation a WITH (NOLOCK)
   RIGHT JOIN v_allinformation b WITH (NOLOCK) ON a.idinformation = b.idinformation
   LEFT JOIN MSTipePeriode c WITH (NOLOCK) ON a.PeriodId = c.PeriodId;

END IF (@existsInformation = '1') BEGIN
INSERT INTO
   @listInformasi (
      idinformation,
      namainformasi,
      kelompokinformasi,
      periode,
      periodestr,
      parentinformation,
      parentnamainformasi
   )
SELECT
   a.IDInformasi,
   b.namainformasi,
   b.kelompokinformasi,
   a.Periode,
   CASE
      WHEN a.Periode = 'M' THEN 'Bulanan'
      WHEN a.Periode = 'D' THEN 'Harian'
      WHEN a.Periode = 'W' THEN 'Mingguan'
      WHEN a.Periode = 'Q' THEN 'Triwulanan'
      WHEN a.Periode = 'S' THEN 'Semesteran'
      WHEN a.Periode = 'A' THEN 'Tahunan'
   END,
   b.parentinformation,
   b.parentnamainformasi
FROM
   MSUserInformation a WITH (NOLOCK)
   INNER JOIN v_allinformation b WITH (NOLOCK) ON a.IDInformasi = b.idinformation
WHERE
   a.UserName = @UserName;

END RETURN;

END