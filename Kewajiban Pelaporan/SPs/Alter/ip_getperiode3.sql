alter PROCEDURE [dbo].[ip_getperiode3] @periode VARCHAR(2),
@idpelapor VARCHAR(9),
@status VARCHAR(MAX),
@jenis VARCHAR(MAX) AS BEGIN IF (@periode = 'D') BEGIN IF @status = 'KP' BEGIN
SELECT
   selected,
   test,
   Label,
   b.kelompokinformasi
FROM
   (
      SELECT
         CASE
            WHEN d.idinformation IS NULL THEN 0
            ELSE 1
         END AS selected,
         v.idinformation,
         v.idinformation + ',' + tipeperiod AS test,
         v.namainformasi + CASE
            WHEN tipeperiod = 'D' THEN ' - Harian Normal'
            WHEN tipeperiod = 'D1' THEN ' - Harian PUAB Pagi'
            WHEN tipeperiod = 'D2' THEN ' - Harian PUAB Sore'
            WHEN tipeperiod = 'D3' THEN ' - Harian PUAB Valas'
            WHEN tipeperiod = 'D4' THEN ' - Harian PUAB LN'
            WHEN tipeperiod = 'D5' THEN ' - Harian PUAS/DOC'
            WHEN tipeperiod = 'D6' THEN ' - Harian JIBOR'
            WHEN tipeperiod = 'W' THEN ' - Mingguan'
            WHEN tipeperiod = 'M' THEN ' - Bulanan'
            WHEN tipeperiod = 'Q' THEN ' - Triwulanan'
            WHEN tipeperiod = 'S' THEN ' - Semesteran'
            WHEN tipeperiod = 'A' THEN ' - Tahunan'
         END AS Label
      FROM
         (
            SELECT
               DISTINCT idinformation,
               namainformasi,
               parentinformation
            FROM
               metadata_mgmt
         ) v
         INNER JOIN v_cakupanabsensi c ON v.idinformation = c.idinformation
         LEFT JOIN (
            SELECT
               DISTINCT idinformation
            FROM
               pelaporinformation a
               JOIN msinstansi b ON a.IdPelapor = b.idpelapor
            WHERE
               LEFT(periode, 1) = @periode
               AND LEFT(a.IdPelapor, 3) = LEFT(@idpelapor, 3)
               AND b.statuskantor = @status
               AND b.jeniskegiatanoperasionalbank = @jenis
         ) d ON c.idinformation = d.idinformation
      WHERE
         c.tipeperiod IN ('D', 'D1', 'D2', 'D3', 'D4', 'D5', 'D6')
         AND v.parentinformation IS NULL
      GROUP BY
         v.idinformation,
         v.namainformasi,
         c.tipeperiod,
         d.idinformation
   ) a
   JOIN (
      SELECT
         DISTINCT idinformation,
         kelompokinformasi
      FROM
         metadata_mgmt
   ) b ON a.idinformation = b.idinformation
ORDER BY
   kelompokinformasi,
   a.idinformation
END
ELSE BEGIN
SELECT
   selected,
   test,
   Label,
   b.kelompokinformasi
FROM
   (
      SELECT
         CASE
            WHEN d.idinformation IS NULL THEN 0
            ELSE 1
         END AS selected,
         v.idinformation,
         v.idinformation + ',' + tipeperiod AS test,
         v.namainformasi + CASE
            WHEN tipeperiod = 'D' THEN ' - Harian Normal'
            WHEN tipeperiod = 'D1' THEN ' - Harian PUAB Pagi'
            WHEN tipeperiod = 'D2' THEN ' - Harian PUAB Sore'
            WHEN tipeperiod = 'D3' THEN ' - Harian PUAB Valas'
            WHEN tipeperiod = 'D4' THEN ' - Harian PUAB LN'
            WHEN tipeperiod = 'D5' THEN ' - Harian PUAS/DOC'
            WHEN tipeperiod = 'D6' THEN ' - Harian JIBOR'
            WHEN tipeperiod = 'W' THEN ' - Mingguan'
            WHEN tipeperiod = 'M' THEN ' - Bulanan'
            WHEN tipeperiod = 'Q' THEN ' - Triwulanan'
            WHEN tipeperiod = 'S' THEN ' - Semesteran'
            WHEN tipeperiod = 'A' THEN ' - Tahunan'
         END AS Label
      FROM
         (
            SELECT
               DISTINCT idinformation,
               namainformasi,
               parentinformation
            FROM
               metadata_mgmt
         ) v
         INNER JOIN v_cakupanabsensi c ON v.idinformation = c.idinformation
         LEFT JOIN (
            SELECT
               DISTINCT idinformation
            FROM
               pelaporinformation a
               JOIN msinstansi b ON a.IdPelapor = b.idpelapor
            WHERE
               LEFT(periode, 1) = @periode
               AND LEFT(a.IdPelapor, 3) = LEFT(@idpelapor, 3)
               AND (
                  b.statuskantor = 'KC'
                  OR b.statuskantor = 'KPO'
               )
               AND b.jeniskegiatanoperasionalbank = @jenis
         ) d ON c.idinformation = d.idinformation
      WHERE
         c.tipeperiod IN ('D', 'D1', 'D2', 'D3', 'D4', 'D5', 'D6')
         AND v.parentinformation IS NULL
      GROUP BY
         v.idinformation,
         v.namainformasi,
         c.tipeperiod,
         d.idinformation
   ) a
   JOIN (
      SELECT
         DISTINCT idinformation,
         kelompokinformasi
      FROM
         metadata_mgmt
   ) b ON a.idinformation = b.idinformation
ORDER BY
   kelompokinformasi,
   a.idinformation
END
END
ELSE BEGIN IF @status = 'KP' BEGIN
SELECT
   selected,
   test,
   Label,
   b.kelompokinformasi
FROM
   (
      SELECT
         CASE
            WHEN d.idinformation IS NULL THEN 0
            ELSE 1
         END AS selected,
         v.idinformation,
         v.idinformation + ',' + tipeperiod AS test,
         v.namainformasi + CASE
            WHEN tipeperiod = 'D' THEN ' - Harian Normal'
            WHEN tipeperiod = 'D1' THEN ' - Harian PUAB Pagi'
            WHEN tipeperiod = 'D2' THEN ' - Harian PUAB Sore'
            WHEN tipeperiod = 'D3' THEN ' - Harian PUAB Valas'
            WHEN tipeperiod = 'D4' THEN ' - Harian PUAB LN'
            WHEN tipeperiod = 'D5' THEN ' - Harian PUAS/DOC'
            WHEN tipeperiod = 'D6' THEN ' - Harian JIBOR'
            WHEN tipeperiod = 'W' THEN ' - Mingguan'
            WHEN tipeperiod = 'M' THEN ' - Bulanan'
            WHEN tipeperiod = 'Q' THEN ' - Triwulanan'
            WHEN tipeperiod = 'S' THEN ' - Semesteran'
            WHEN tipeperiod = 'A' THEN ' - Tahunan'
         END AS Label
      FROM
         (
            SELECT
               DISTINCT idinformation,
               namainformasi,
               parentinformation
            FROM
               metadata_mgmt
         ) v
         INNER JOIN v_cakupanabsensi c ON v.idinformation = c.idinformation
         LEFT JOIN (
            SELECT
               DISTINCT idinformation
            FROM
               pelaporinformation a
               JOIN msinstansi b ON a.IdPelapor = b.idpelapor
            WHERE
               LEFT(periode, 1) = @periode
               AND LEFT(a.IdPelapor, 3) = LEFT(@idpelapor, 3)
               AND b.statuskantor = @status
               AND b.jeniskegiatanoperasionalbank = @jenis
         ) d ON c.idinformation = d.idinformation
      WHERE
         c.tipeperiod = @periode
         AND v.parentinformation IS NULL
      GROUP BY
         v.idinformation,
         v.namainformasi,
         c.tipeperiod,
         d.idinformation
   ) a
   JOIN (
      SELECT
         DISTINCT idinformation,
         kelompokinformasi
      FROM
         metadata_mgmt
   ) b ON a.idinformation = b.idinformation
ORDER BY
   kelompokinformasi,
   a.idinformation
END
ELSE BEGIN
SELECT
   selected,
   test,
   Label,
   b.kelompokinformasi
FROM
   (
      SELECT
         CASE
            WHEN d.idinformation IS NULL THEN 0
            ELSE 1
         END AS selected,
         v.idinformation,
         v.idinformation + ',' + tipeperiod AS test,
         v.namainformasi + CASE
            WHEN tipeperiod = 'D' THEN ' - Harian Normal'
            WHEN tipeperiod = 'D1' THEN ' - Harian PUAB Pagi'
            WHEN tipeperiod = 'D2' THEN ' - Harian PUAB Sore'
            WHEN tipeperiod = 'D3' THEN ' - Harian PUAB Valas'
            WHEN tipeperiod = 'D4' THEN ' - Harian PUAB LN'
            WHEN tipeperiod = 'D5' THEN ' - Harian PUAS/DOC'
            WHEN tipeperiod = 'D6' THEN ' - Harian JIBOR'
            WHEN tipeperiod = 'W' THEN ' - Mingguan'
            WHEN tipeperiod = 'M' THEN ' - Bulanan'
            WHEN tipeperiod = 'Q' THEN ' - Triwulanan'
            WHEN tipeperiod = 'S' THEN ' - Semesteran'
            WHEN tipeperiod = 'A' THEN ' - Tahunan'
         END AS Label
      FROM
         (
            SELECT
               DISTINCT idinformation,
               namainformasi,
               parentinformation
            FROM
               metadata_mgmt
         ) v
         INNER JOIN v_cakupanabsensi c ON v.idinformation = c.idinformation
         LEFT JOIN (
            SELECT
               DISTINCT idinformation
            FROM
               pelaporinformation a
               JOIN msinstansi b ON a.IdPelapor = b.idpelapor
            WHERE
               LEFT(periode, 1) = @periode
               AND LEFT(a.IdPelapor, 3) = LEFT(@idpelapor, 3)
               AND (
                  b.statuskantor = 'KC'
                  OR b.statuskantor = 'KPO'
               )
               AND b.jeniskegiatanoperasionalbank = @jenis
         ) d ON c.idinformation = d.idinformation
      WHERE
         c.tipeperiod = @periode
         AND v.parentinformation IS NULL
      GROUP BY
         v.idinformation,
         v.namainformasi,
         c.tipeperiod,
         d.idinformation
   ) a
   JOIN (
      SELECT
         DISTINCT idinformation,
         kelompokinformasi
      FROM
         metadata_mgmt
   ) b ON a.idinformation = b.idinformation
ORDER BY
   kelompokinformasi,
   a.idinformation
END
END
END