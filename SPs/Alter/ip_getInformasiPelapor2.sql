
CREATE PROCEDURE [dbo].[ip_getInformasiPelapor2]
AS
BEGIN

    SELECT
        b.InstansiId,
        a.IdPelapor,
        a.IdPelapor + ' - ' + b.nama AS Nama,
        b.jeniskegiatanoperasionalbank AS Jnskeg,
        b.statuskantor AS Stat,

        '<p>' + STRING_AGG(  
            CONVERT(VARCHAR(MAX),  
                CASE   
                    WHEN a.Periode = 'D'  THEN c.namainformasi + ' - Harian Normal'  
                    WHEN a.Periode = 'D1' THEN c.namainformasi + ' - Harian PUAB Pagi'  
                    WHEN a.Periode = 'D2' THEN c.namainformasi + ' - Harian PUAB Sore'  
                    WHEN a.Periode = 'D3' THEN c.namainformasi + ' - Harian PUAB Valas'  
                    WHEN a.Periode = 'D4' THEN c.namainformasi + ' - Harian PUAB LN'  
                    WHEN a.Periode = 'D5' THEN c.namainformasi + ' - Harian PUAS/DOC'  
                    WHEN a.Periode = 'D6' THEN c.namainformasi + ' - Harian JIBOR'  
                    ELSE NULL  
                END  
            ), '</p><p>'  
        ) WITHIN GROUP (ORDER BY c.namainformasi ASC) + '</p>' AS harian,

        '<p>' + STRING_AGG(  
            CONVERT(VARCHAR(MAX),  
                CASE WHEN a.Periode = 'W' THEN c.namainformasi ELSE NULL END  
            ), '</p><p>'  
        ) WITHIN GROUP (ORDER BY c.namainformasi ASC) + '</p>' AS mingguan,

        '<p>' + STRING_AGG(  
            CONVERT(VARCHAR(MAX),  
                CASE WHEN a.Periode = 'M' THEN c.namainformasi ELSE NULL END  
            ), '</p><p>'  
        ) WITHIN GROUP (ORDER BY c.namainformasi ASC) + '</p>' AS bulanan,

        '<p>' + STRING_AGG(  
            CONVERT(VARCHAR(MAX),  
                CASE WHEN a.Periode = 'Q' THEN c.namainformasi ELSE NULL END  
            ), '</p><p>'  
        ) WITHIN GROUP (ORDER BY c.namainformasi ASC) + '</p>' AS triwulanan,

        '<p>' + STRING_AGG(  
            CONVERT(VARCHAR(MAX),  
                CASE WHEN a.Periode = 'S' THEN c.namainformasi ELSE NULL END  
            ), '</p><p>'  
        ) WITHIN GROUP (ORDER BY c.namainformasi ASC) + '</p>' AS semesteran,

        '<p>' + STRING_AGG(  
            CONVERT(VARCHAR(MAX),  
                CASE WHEN a.Periode = 'A' THEN c.namainformasi ELSE NULL END  
            ), '</p><p>'  
        ) WITHIN GROUP (ORDER BY c.namainformasi ASC) + '</p>' AS tahunan

    FROM PelaporInformation a
        INNER JOIN MsInstansi b ON a.IdPelapor = b.idpelapor
        LEFT JOIN (  
        SELECT DISTINCT idinformation, namainformasi
        FROM metadata_mgmt
        WHERE parentinformation IS NULL  
    ) AS c ON a.IdInformation = c.idinformation

    GROUP BY    
        b.InstansiId,   
        a.IdPelapor,   
        b.nama,   
        b.jeniskegiatanoperasionalbank,   
        b.statuskantor

    ORDER BY a.IdPelapor + ' - ' + b.nama;

END  