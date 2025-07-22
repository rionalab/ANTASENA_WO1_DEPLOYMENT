USE [antasenaDBTest]
GO
   /****** Object:  StoredProcedure [dbo].[p_getAllPeriodeByID]    Script Date: 5/22/2025 3:33:21 PM ******/
SET
   ANSI_NULLS ON
GO
SET
   QUOTED_IDENTIFIER ON
GO
   -- =============================================            
   -- Author:        YAQUB            
   -- Create date:   2019-06-20            
   -- Description:   Get all periode by ID            
   -- Usage:         [dbo].[p_getAllPeriodeByID] '7'            
   -- =============================================            
   ALTER PROCEDURE [dbo].[p_getAllPeriodeByID] @PeriodId INT,
   @UserId VARCHAR(255) AS BEGIN DECLARE @PeriodeType VARCHAR(MAX) = (
      SELECT
         TOP 1 PeriodType
      FROM
         MSTipePeriode
      WHERE
         PeriodId = @PeriodId
   );

DECLARE @tiperharian VARCHAR(2);

IF @PeriodeType = 'Harian' BEGIN
SET
   @tiperharian = (
      SELECT
         TOP 1 @tiperharian
      FROM
         MSTipePeriodeHarian
      WHERE
         PeriodName = (
            SELECT
               TOP 1 PeriodName
            FROM
               MSTipePeriode
            WHERE
               PeriodId = @PeriodId
         )
   );

SELECT
   DISTINCT a.idinformation AS informasi,
   a.namainformasi AS labelement,
   CASE
      WHEN PeriodId IS NOT NULL THEN 1
      ELSE 0
   END AS checkbox,
   cakupanabsensi
FROM
   dbo.metadata_mgmt AS a
   LEFT JOIN (
      SELECT
         PeriodId,
         Versioncode,
         idinformation,
         cakupanabsensi
      FROM
         dbo.PeriodeInformation
      WHERE
         PeriodId = @PeriodId
   ) AS b ON a.idinformation = b.idinformation
WHERE
   a.idinformation NOT IN (
      SELECT
         c.idinformation
      FROM
         dbo.PeriodeInformation c
         INNER JOIN MSTipePeriode d ON c.PeriodId = d.PeriodId
         INNER JOIN MSTipePeriodeHarian e ON d.PeriodName = e.PeriodName
      WHERE
         c.PeriodId != @PeriodId
         AND e.TipeHarian = @tiperharian
   )
   AND a.parentinformation IS NULL
   AND a.idinformation IN (
      SELECT
         aa.idinformation
      FROM
         F_GetTrussteeInformation(@UserId) aa
   );

END
ELSE IF @PeriodeType IN (
   'Mingguan',
   'Bulanan',
   'Triwulanan',
   'Semesteran',
   'Tahunan'
) BEGIN
SELECT
   DISTINCT a.idinformation AS informasi,
   a.namainformasi AS labelement,
   CASE
      WHEN PeriodId IS NOT NULL THEN 1
      ELSE 0
   END AS checkbox,
   cakupanabsensi
FROM
   dbo.metadata_mgmt AS a
   LEFT JOIN (
      SELECT
         PeriodId,
         Versioncode,
         idinformation,
         cakupanabsensi
      FROM
         dbo.PeriodeInformation
      WHERE
         PeriodId = @PeriodId
   ) AS b ON a.idinformation = b.idinformation
WHERE
   a.parentinformation IS NULL
   AND a.idinformation NOT IN (
      SELECT
         c.idinformation
      FROM
         dbo.PeriodeInformation c
         INNER JOIN MSTipePeriode d ON c.PeriodId = d.PeriodId
      WHERE
         d.PeriodId != @PeriodId
         AND d.PeriodType = @PeriodeType
   )
   AND a.idinformation IN (
      SELECT
         aa.idinformation
      FROM
         F_GetTrussteeInformation(@UserId) aa
   );

END
END