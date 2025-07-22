SET
   ANSI_NULLS ON
GO
SET
   QUOTED_IDENTIFIER ON
GO
   ALTER Procedure [dbo].[lra_getdatadetail] --Declare        
   @tblhnamelra varchar(255),
   @tblnamefileinformasi varchar(255),
   @idpelapor varchar(10),
   @Status varchar(max) = '' as --set @tblhnamelra = 'ZZLRA_aGVyeXVzbUBnbWFpbC5jb20='        
   --set @tblnamefileinformasi = 'ZZLRADTL_aGVyeXVzbUBnbWFpbC5jb20='        
   --set @idpelapor = '111000000'        
   --set @Status = 'Tepat Waktu Menyampaikan Laporan,Tidak Menyampaikan Laporan,Koreksi Di Luar Batas Penyampaian'           
set
   @tblhnamelra = dbo.fn_StripCharacters(@tblhnamelra, '^a-zA-Z0-9 [-]_@.!#$&/\[]')
set
   @tblnamefileinformasi = dbo.fn_StripCharacters(
      @tblnamefileinformasi,
      '^a-zA-Z0-9 [-]_@.!#$&/\[]'
   )
set
   @idpelapor = dbo.fn_StripCharacters(@idpelapor, '^a-zA-Z0-9 [-]_@.!#$&/\[]')
set
   @Status = '''' + REPLACE(@Status, ',', ''',''') + '''' If OBJECT_ID('tempdb..#temptableprocess') is not null begin DROP TABLE #temptableprocess
end Create Table #temptableprocess
(
   idpelapor varchar(10) null,
   versioncode varchar(10) null,
   versioncodealias varchar(10) null,
   Nama varchar(255) null,
   KantorCabang varchar(255) null,
   PeriodeLaporan varchar(10) null,
   kelompokinformasi varchar(255) null,
   IdInformasi varchar(255) null,
   IdInformasiAlias varchar(255) null,
   namainformasi varchar(255) null,
   namainformasiAlias varchar(255) null,
   PeriodeData varchar(10) null,
   laporanstatus varchar(255) null,
   koreksistatus varchar(255) null,
   laporantimestamp varchar(20) null,
   koreksitimestamp varchar(20) null,
   firsttime varchar(1) null
) If OBJECT_ID('tempdb..#temptableprocess2') is not null begin DROP TABLE #temptableprocess2
end Create Table #temptableprocess2
(
   idpelapor varchar(10) null,
   versioncode varchar(10) null,
   Nama varchar(255) null,
   KantorCabang varchar(255) null,
   PeriodeLaporan varchar(10) null,
   kelompokinformasi varchar(255) null,
   IdInformasi varchar(255) null,
   namainformasi varchar(255) null,
   PeriodeData varchar(10) null,
   laporanstatus varchar(255) null,
   koreksistatus varchar(255) null,
   laporantimestamp varchar(20) null,
   koreksitimestamp varchar(20) null
) If OBJECT_ID('tempdb..#temptablefileinfo') is not null begin DROP TABLE #temptablefileinfo
end Create Table #temptablefileinfo
(
   idpelapor varchar(10) null,
   versioncode varchar(10) null,
   versioncodealias varchar(10) null,
   PeriodeData varchar(10) null,
   periodelaporan varchar(30) null,
   periodelaporanstr varchar(255) null,
   recordtimestamp varchar(255) null,
   rownumber int null,
   kelompokinformasi varchar(255) null,
   IdInformasi varchar(255) null,
   IdInformasiAlias varchar(255) null,
   namainformasi varchar(255) null,
   namainformasiAlias varchar(255) null,
   laporanstatus varchar(255) null,
   koreksistatus varchar(255) null,
   laporantimestamp varchar(255) null,
   koreksitimestamp varchar(255) null
) If OBJECT_ID('tempdb..#tabledetail') is not null begin DROP TABLE #tabledetail
end Create table #tabledetail
(
   seq int null,
   Bank varchar(255) null,
   kelompokinformasi varchar(255) null,
   namainformasi varchar(255) null,
   versioncode varchar(10) null,
   periodelaporan varchar(50) null,
   periodedata varchar(10) null,
   recordtimestamp varchar(max) null,
   recordtimestampori varchar(max) null,
   jumlahbarislaporan int null,
   jumlahbariskoreksi int null,
   laporan varchar(255) null,
   koreksi varchar(255) null
) Declare @q nvarchar(max) = ' declare @bbb table (idpelapor           varchar(10) null,                               
            versioncode         varchar(10) null,                               
            versioncodealias    varchar(10) null,                               
            Nama                varchar(255) null,                               
            KantorCabang        varchar(255) null,                              
            PeriodeLaporan      varchar(5) null,                              
            kelompokinformasi   varchar(255) null,                              
            IdInformasi         varchar(255) null,                              
            IdInformasiAlias    varchar(255) null,                              
            namainformasi       varchar(255) null,                              
            namainformasiAlias  varchar(255) null,                              
            PeriodeData         varchar(10) null,                              
            laporanstatus       varchar(255) null,                              
            koreksistatus       varchar(255) null,                              
            laporantimestamp    varchar(20) null,                              
            koreksitimestamp    varchar(20) null,                          
   firsttime           varchar(1) null  )                                
                                insert into  @bbb                               
        select convert(varchar(10),a.idpelapor), convert(varchar(10), a.versioncode),        
                      convert(varchar(10),c.versioncode),                               
                      convert(varchar(255),a.nama) , convert(varchar(255),a.kantorcabang),                              
             case when left(a.periodelaporan,1) = ''D'' then ''D'' else convert(varchar(10),a.periodelaporan) end, convert(varchar(255),a.kelompokinformasi),                              
             convert(varchar(255),a.idinformasi),                              
             convert(varchar(255),c.idinformation),                              
             convert(varchar(255),a.namainformasi),                              
             convert(varchar(255),c.namainformasi),                              
             convert(varchar(10),a.periodedata), convert(varchar(255),a.laporanstatus),                 
             convert(varchar(255),a.koreksistatus), convert(varchar(20),a.laporantimestamp),                              
             convert(varchar(20),a.koreksitimestamp), ''N''                         
            from  [' + @tblhnamelra + ']  a inner join (                               
              select distinct a.kelompokinformasi,  case when b.idinformation IS not null then b.idinformation                               
           else a.idinformation end as idinformation,                              
          case when b.idinformation IS not null then b.versioncode else a.versioncode end as versioncode,                             
          case when b.idinformation IS not null then b.namainformasi else a.namainformasi end as namainformasi,                               
          a.idinformation idinformationori, a.versioncode versioncodeori, a.namainformasi  namainformasiori                               
          from metadata_mgmt a left join metadata_mgmt b on a.parentinformation = b.idinformation                              
          ) c on a.IdInformasi = c.idinformationori                              
          where convert(varchar(10),a.idpelapor) = ''' + @idpelapor + ''' and        
        ( isnull(a.laporanstatus,''NULL'') in ( ' + @status + ' ) or isnull(a.koreksistatus,''NULL'') in (' + @status + '))                                 
         select * from @bbb  '
insert into
   #temptableprocess
   exec(@q)
set
   @q = ' declare @bbb table (idpelapor           varchar(10)  null,                              
      versioncode         varchar(10)  null,                               
      versioncodealias    varchar(10)  null,           
      PeriodeData         varchar(10)  null,                              
      periodelaporan      varchar(30) null,                
      periodelaporanstr   varchar(255) null,                         
      recordtimestamp     varchar(255) null,                              
      rownumber           int  null,                              
      kelompokinformasi   varchar(255) null,                              
      IdInformasi         varchar(255) null,                              
      IdInformasiAlias    varchar(255) null,                              
      namainformasi       varchar(255) null,                              
      namainformasiAlias  varchar(255) null,    
   laporanstatus       varchar(255) null,    
      koreksistatus       varchar(255) null,    
      laporantimestamp    varchar(255) null,    
      koreksitimestamp    varchar(255) null )                                
         insert into  @bbb                               
        select distinct convert(varchar(10),a.idpelapor),                     
                        convert(varchar(10), a.versioncode),                              
                        convert(varchar(10),c.versioncode),           
      a.periodedata,        
      a.periodelaporan,        
		case 
			when left(a.periodelaporan,1) = ''D'' then ''Harian''        
			when left(a.periodelaporan,1) = ''W'' then ''Mingguan''        
         when left(a.periodelaporan,1) = ''M'' then ''Bulanan''        
         when left(a.periodelaporan,1) = ''Q'' then ''Triwulanan''
			when left(a.periodelaporan,1) = ''S'' then ''Semesteran''
			when left(a.periodelaporan,1) = ''A'' then ''Tahunan''
		end,        
       recordtimestamp ,          
          a.rownumber,        
                         convert(varchar(255),c.kelompokinformasi),              
       convert(varchar(255),c.idinformationori),              
       convert(varchar(255),c.idinformation),                             
                         convert(varchar(255),c.namainformasiori),             
       convert(varchar(255),c.namainformasi)  , a.laporanstatus, a.koreksistatus, a.laporantimestamp, a.koreksitimestamp                                                       
            from  [' + @tblnamefileinformasi + ']  a inner join (                               
              select distinct a.kelompokinformasi,  case when b.idinformation IS not null then b.idinformation           
           else a.idinformation end as idinformation,                              
          case when b.idinformation IS not null then b.versioncode else a.versioncode end as versioncode,                               
          case when b.idinformation IS not null then b.namainformasi else a.namainformasi end as namainformasi,                     
a.idinformation idinformationori, a.versioncode versioncodeori, a.namainformasi  namainformasiori                               
          from metadata_mgmt a left join metadata_mgmt b on a.parentinformation = b.idinformation                              
          ) c on a.versioncode = c.versioncodeori where a.idpelapor = ''' + @idpelapor + '''                                     
         select * from @bbb  '
insert into
   #temptablefileinfo
   exec(@q) --  update #temptableprocess  set firsttime = 'Y'                         
   --  from #temptableprocess a                        
   --  where laporantimestamp = ( select  MIN(isnull(laporantimestamp,'')) from #temptableprocess b                         
   --  where b.versioncode = a.versioncode                        
   --  and b.idpelapor = a.idpelapor                        
   --  and b.PeriodeData = a.PeriodeData                        
   --  and b.PeriodeLaporan = a.PeriodeLaporan                        
   --  group by b.versioncode )                        
   --  and isnull(a.koreksitimestamp,'null') = 'null'          
   --  and a.IdInformasiAlias =    'trxSpotDerivatif'          
   --  update #temptableprocess  set firsttime = 'Y'                         
   --  from #temptableprocess a                        
   --  where laporantimestamp = ( select MIN(isnull(laporantimestamp,'')) from #temptableprocess b                         
   --  where b.versioncode = a.versioncode                        
   --  and b.idpelapor = a.idpelapor                        
   --  and b.PeriodeData = a.PeriodeData                        
   --  and b.PeriodeLaporan = a.PeriodeLaporan                        
   --  group by b.versioncode )               
   --  and a.IdInformasiAlias !=  'trxSpotDerivatif'              
   --insert into #temptableprocess2 ( idpelapor, versioncode, Nama,  PeriodeLaporan,                                    
   --                                 kelompokinformasi, IdInformasi,  namainformasi,                               
   --                                   PeriodeData, laporantimestamp, koreksitimestamp )                                 
   --select idpelapor, versioncodealias versioncode, Nama,  PeriodeLaporan, kelompokinformasi, IdInformasiAlias idinformasi,                               
   --namainformasiAlias namainformasi,                                
   --PeriodeData,                             
   --( select  MAX(isnull(laporantimestamp,'') ) from #temptableprocess  b where                              
   --   b.idpelapor = a.idpelapor and b.versioncodealias = a.versioncodealias and b.Nama = a.Nama and b.KantorCabang = a.KantorCabang                              
   --   and b.PeriodeLaporan = a.PeriodeLaporan and b.kelompokinformasi = a.kelompokinformasi and b.IdInformasiAlias = a.IdInformasiAlias                              
   --   and b.namainformasiAlias = a.namainformasiAlias and b.PeriodeData = a.PeriodeData                             
   --   group by b.idpelapor, b.versioncodealias, b.Nama, b.KantorCabang, b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --            b.namainformasiAlias,  b.PeriodeData                            
   --   ) laporantimestamp ,   a.koreksitimestamp                              
   -- from  #temptableprocess a where a.IdInformasi not in ( 'trxPuabPuasDoc','trxSpotDerivatif','trxSpotDerivatifUnderlying','trxSuratBerhargaPsrSekunder')                              
   -- union                              
   -- select distinct idpelapor, versioncodealias versioncode, Nama,  PeriodeLaporan, kelompokinformasi, IdInformasiAlias idinformasi,                               
   --namainformasiAlias namainformasi,                                
   --PeriodeData,                             
   -- laporantimestamp ,   a.koreksitimestamp                              
   -- from  #temptableprocess a                                
   -- where a.IdInformasi  in ( 'trxPuabPuasDoc','trxSuratBerhargaPsrSekunder')                                
   -- union                            
   --  select distinct a.idpelapor, a.versioncodealias, a.Nama, a.PeriodeLaporan, a.kelompokinformasi, a.IdInformasiAlias,                               
   --   a.namainformasiAlias,  a.PeriodeData, B.laporantimestamp,  b.koreksitimestamp                          
   --    from #temptableprocess a inner join                           
   --   (  select b.idpelapor, b.versioncodealias, b.Nama, b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --             b.namainformasiAlias,  b.PeriodeData, MAX(laporantimestamp ) laporantimestamp, max(koreksitimestamp) koreksitimestamp                   
   --    from #temptableprocess b                           
   --     where b.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')                              
   --     group by b.idpelapor, b.versioncodealias, b.Nama, b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --            b.namainformasiAlias,  b.PeriodeData                            
   --   ) b on b.idpelapor = a.idpelapor and b.versioncodealias = a.versioncodealias and b.Nama = a.Nama                           
   --     and b.PeriodeLaporan = a.PeriodeLaporan                           
   --   and  b.kelompokinformasi = a.kelompokinformasi and b.IdInformasiAlias = a.IdInformasiAlias                              
   --            and b.namainformasiAlias = a.namainformasiAlias and b.PeriodeData = a.PeriodeData                             
   --   where a.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')                             
   --   and a.firsttime = 'Y'                          
   -- union                          
   -- select distinct a.idpelapor, a.versioncodealias, a.Nama, a.PeriodeLaporan, a.kelompokinformasi, a.IdInformasiAlias,                               
   --            a.namainformasiAlias,  a.PeriodeData, B.laporantimestamp,  a.koreksitimestamp                          
   --    from #temptableprocess a inner join                           
   --   (  select b.idpelapor, b.versioncodealias, b.Nama,  b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --             b.namainformasiAlias,  b.PeriodeData, MAX(laporantimestamp ) laporantimestamp from #temptableprocess b                           
   --     where b.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')                          
   --     group by b.idpelapor, b.versioncodealias,  b.Nama,  b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --            b.namainformasiAlias,  b.PeriodeData                            
   --   ) b on b.idpelapor = a.idpelapor and b.versioncodealias = a.versioncodealias and b.Nama = a.Nama                  
   --   and b.PeriodeLaporan = a.PeriodeLaporan                           
   --   and  b.kelompokinformasi = a.kelompokinformasi and b.IdInformasiAlias = a.IdInformasiAlias                              
   --            and b.namainformasiAlias = a.namainformasiAlias and b.PeriodeData = a.PeriodeData                             
   --   where a.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')                             
   --      and a.firsttime = 'N'                          
   --   and b.laporantimestamp > (  select max(c.laporantimestamp) from #temptableprocess c where firsttime = 'Y'                        
   --    and c.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')  )                        
   --    and isnull(a.koreksitimestamp,'null') = 'null'                           
   --union                     
   --select distinct a.idpelapor, a.versioncodealias, a.Nama, a.PeriodeLaporan, a.kelompokinformasi, a.IdInformasiAlias,                               
   --            a.namainformasiAlias,  a.PeriodeData, a.laporantimestamp,  a.koreksitimestamp                          
   --    from #temptableprocess a inner join                           
   --   (  select b.idpelapor, b.versioncodealias, b.Nama,  b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --             b.namainformasiAlias,  b.PeriodeData, MAX(laporantimestamp ) laporantimestamp from #temptableprocess b                           
   --     where b.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')                              
   --     group by b.idpelapor, b.versioncodealias, b.Nama,  b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasiAlias,                               
   --            b.namainformasiAlias,  b.PeriodeData                            
   --   ) b on b.idpelapor = a.idpelapor and b.versioncodealias = a.versioncodealias and b.Nama = a.Nama                           
   --     and b.PeriodeLaporan = a.PeriodeLaporan                           
   --   and  b.kelompokinformasi = a.kelompokinformasi and b.IdInformasiAlias = a.IdInformasiAlias                              
   --            and b.namainformasiAlias = a.namainformasiAlias and b.PeriodeData = a.PeriodeData                             
   --   where a.IdInformasi  in (  'trxSpotDerivatif','trxSpotDerivatifUnderlying')                             
   --   and a.firsttime = 'N'                                           
insert into
   #tabledetail
   (
      seq,
      Bank,
      kelompokinformasi,
      namainformasi,
      versioncode,
      periodelaporan,
      periodedata,
      recordtimestampori,
      recordtimestamp
   )
select
   ROW_NUMBER() over (
      order by
         nama,
         sequence_information,
         recordtimestamp
   ),
   nama,
   kelompokinformasi,
   namainformasialias,
   versioncode,
   periodelaporanstr,
   periodedata,
   recordtimestamp,
   [text()]
from
   (
      select
         distinct #temptablefileinfo.idpelapor+' - '+msinstansi.nama nama,
         #temptablefileinfo.kelompokinformasi, namainformasiAlias, #temptablefileinfo.versioncode, periodelaporanstr,
         #temptablefileinfo.PeriodeData, recordtimestamp, sequence_information,
         convert(
            varchar(10),
            convert(
               datetime,
               replace(nullif(recordtimestamp, 'null'), 'T', ' ')
            ),
            105
         ) + ' ' + convert(
            varchar(10),
            convert(
               datetime,
               replace(nullif(recordtimestamp, 'null'), 'T', ' ')
            ),
            108
         ) AS [text()]
      from
         #temptablefileinfo
         left join msinstansi on #temptablefileinfo.idpelapor = msinstansi.idpelapor
         inner join (
            select
               distinct versioncode,
               isnull(sequence_information, 999) sequence_information
            from
               v_allinformation
         ) metadata on metadata.versioncode = #temptablefileinfo.versioncodealias
      where
         exists (
            select
               1
            from
               #temptableprocess
            where
               #temptablefileinfo.PeriodeData = #temptableprocess.PeriodeData
               and #temptableprocess.versioncodealias = #temptablefileinfo.versioncodealias )   ) a  
               --insert into #tabledetail        
               --( seq, Bank ,kelompokinformasi, namainformasi, versioncode ,        
               --     periodelaporan, periodedata, recordtimestampori, recordtimestamp)        
               --select distinct ROW_NUMBER() over (order by #temptablefileinfo.idpelapor, sequence_information, recordtimestamp ), #temptablefileinfo.idpelapor+' - '+msinstansi.nama,        
               -- #temptablefileinfo.kelompokinformasi, namainformasiAlias, #temptablefileinfo.versioncode, periodelaporanstr,         
               -- #temptablefileinfo.PeriodeData, recordtimestamp,                       
               -- convert(varchar(10), convert(datetime,replace(nullif(recordtimestamp,'null'),'T', ' ')),105)+ ' ' +                               
               -- convert(varchar(10), convert(datetime,replace(nullif(recordtimestamp,'null'),'T', ' ')),108)  AS [text()]          
               -- from #temptablefileinfo         
               --left join msinstansi on #temptablefileinfo.idpelapor = msinstansi.idpelapor        
               --inner join ( select distinct versioncode, isnull(sequence_information,999) sequence_information from v_allinformation ) metadata        
               --on metadata.versioncode = #temptablefileinfo.versioncodealias        
               --where exists ( select 1 from #temptableprocess        
               --  where #temptablefileinfo.PeriodeData = #temptableprocess.PeriodeData        
               --  and #temptableprocess.versioncodealias = #temptablefileinfo.versioncodealias )        
               --and recordtimestamp in ( select distinct laporantimestamp from #temptableprocess2        
               --  union         
               --  select distinct koreksitimestamp from #temptableprocess2 )        
               --and #temptablefileinfo.PeriodeData in ( select PeriodeData from #temptableprocess )         
               --select * From #tabledetail where versioncode in ( 'tsd01','tsu01')      
               --select * from #temptablefileinfo where versioncode in ( 'tsd01','tsu01')      
               --select * from #temptableprocess2 where versioncode  in ( 'tsd01','tsu01')       
               --select * from #temptableprocess where versioncode  in ( 'akr01','krp01')      
            update
               #tabledetail set jumlahbarislaporan =  a.rownumber        
            from
               #tabledetail inner join
               (
                  select
                     versioncode,
                     recordtimestamp,
                     PeriodeData,
                     sum(
                        #temptablefileinfo.rownumber   ) rownumber
                        from
                           #temptablefileinfo
                        where
                           #temptablefileinfo.recordtimestamp = #temptablefileinfo.laporantimestamp
                        group by
                           versioncode,
                           recordtimestamp,
                           PeriodeData
                     ) a on #tabledetail.versioncode = a.versioncode
                     and #tabledetail.recordtimestampori = a.recordtimestamp
                     and #tabledetail.periodedata = a.PeriodeData        
                  update
                     #tabledetail set JumlahBarisKoreksi = a.rownumber        
                  from
                     #tabledetail inner join
                     (
                        select
                           versioncode,
                           recordtimestamp,
                           PeriodeData,
                           sum(
                              #temptablefileinfo.rownumber   ) rownumber
                              from
                                 #temptablefileinfo
                              where
                                 #temptablefileinfo.recordtimestamp = #temptablefileinfo.koreksitimestamp
                              group by
                                 versioncode,
                                 recordtimestamp,
                                 PeriodeData
                           ) a on #tabledetail.versioncode = a.versioncode
                           and #tabledetail.recordtimestampori = a.recordtimestamp
                           and #tabledetail.periodedata = a.PeriodeData        
                        update
                           #tabledetail set jumlahbarislaporan = a.rownumber        
                        from
                           #tabledetail inner join
                           (
                              select
                                 versioncode,
                                 recordtimestamp,
                                 PeriodeData,
                                 sum(
                                    #temptablefileinfo.rownumber   ) rownumber
                                    from
                                       #temptablefileinfo
                                    group by
                                       versioncode,
                                       recordtimestamp,
                                       PeriodeData
                                 ) a on #tabledetail.versioncode = a.versioncode
                                 and #tabledetail.recordtimestampori = a.recordtimestamp
                                 and #tabledetail.periodedata = a.PeriodeData
                                 and jumlahbariskoreksi is null
                                 and jumlahbarislaporan is null --update #tabledetail set jumlahbarislaporan = #temptablefileinfo.rownumber        
                                 --from #tabledetail inner join #temptablefileinfo  on #tabledetail.versioncode = #temptablefileinfo.versioncode        
                                 --and #tabledetail.recordtimestampori = #temptablefileinfo.recordtimestamp        
                                 --and #tabledetail.periodedata = #temptablefileinfo.PeriodeData        
                                 --inner join #temptableprocess2 lapor on #temptablefileinfo.versioncodealias = lapor.versioncode        
                                 --and #temptablefileinfo.recordtimestamp = lapor.laporantimestamp        
                                 --and #temptablefileinfo.PeriodeData = lapor.PeriodeData      
                                 --and jumlahbariskoreksi is null                     
                                 --update #tabledetail set JumlahBarisKoreksi = #temptablefileinfo.rownumber        
                                 --from #tabledetail inner join #temptablefileinfo  on #tabledetail.versioncode = #temptablefileinfo.versioncode        
                                 --and #tabledetail.recordtimestampori = #temptablefileinfo.recordtimestamp        
                                 --and #tabledetail.periodedata = #temptablefileinfo.PeriodeData        
                                 --inner join #temptableprocess2 koreksi on #temptablefileinfo.versioncodealias = koreksi.versioncode        
                                 --and #temptablefileinfo.recordtimestamp = koreksi.koreksitimestamp        
                                 --and #temptablefileinfo.PeriodeData = koreksi.PeriodeData        
                                 --update #tabledetail set jumlahbariskoreksi = #temptablefileinfo.rownumber        
                                 --from #tabledetail inner join #temptablefileinfo  on #tabledetail.versioncode = #temptablefileinfo.versioncode         
                                 --and #tabledetail.periodedata = #temptablefileinfo.PeriodeData        
                                 --and jumlahbariskoreksi is null      
                                 --and jumlahbarislaporan is null     
                              select
                                 seq,
                                 Bank,
                                 #tabledetail.kelompokinformasi, #tabledetail.namainformasi, #tabledetail.versioncode,
                                 periodelaporan,
                                 periodedata,
                                 recordtimestamp,
                                 convert(
                                    varchar,
                                    Format(convert(DECIMAL, JumlahBarisLaporan), '#,0')
                                 ) JumlahBarisLaporan,
                                 convert(
                                    varchar,
                                    Format(convert(DECIMAL, JumlahBarisKoreksi), '#,0')
                                 ) JumlahBarisKoreksi
                              from
                                 #tabledetail
                                 --select  seq, Bank, #tabledetail.kelompokinformasi, #tabledetail.namainformasi, #tabledetail.versioncode,         
                                 --        periodelaporan, periodedata, recordtimestamp ,   
                                 --  convert(varchar,Format(convert(DECIMAL, JumlahBarisLaporan), '#,###', 'DE-de'))  JumlahBarisLaporan,   
                                 --  convert(varchar,Format(convert(DECIMAL, JumlahBarisKoreksi), '#,###', 'DE-de')) JumlahBarisKoreksi            
                                 --from #tabledetail