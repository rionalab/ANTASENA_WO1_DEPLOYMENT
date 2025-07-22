INSERT INTO
   [dbo].[ATTableList] ([menudesc], [tablename], [ColKeterangan])
SELECT
   'Pengaturan Tipe Periode Semesteran',
   'ATMSTipePeriodeSemester',
   'y.PeriodName'
WHERE
   NOT EXISTS (
      SELECT
         1
      FROM
         [dbo].[ATTableList]
      WHERE
         menudesc = 'Pengaturan Tipe Periode Semesteran'
         AND tablename = 'ATMSTipePeriodeSemester'
         AND ColKeterangan = 'y.PeriodName'
   );

INSERT INTO
   [dbo].[ATTableList] ([menudesc], [tablename], [ColKeterangan])
SELECT
   'Pengaturan Tipe Periode Tahunan',
   'ATMSTipePeriodeTahun',
   'y.PeriodName'
WHERE
   NOT EXISTS (
      SELECT
         1
      FROM
         [dbo].[ATTableList]
      WHERE
         menudesc = 'Pengaturan Tipe Periode Tahunan'
         AND tablename = 'ATMSTipePeriodeTahun'
         AND ColKeterangan = 'y.PeriodName'
   );