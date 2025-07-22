using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Dynamic;
using API02.Models;
using Newtonsoft.Json.Linq;

namespace API02.Libs
{
    public class lData
    {
        private lGlobal obj = new lGlobal();
        private lDbConn db = new lDbConn();
        private lConvert lc = new lConvert();
        private lEncrypt enc = new lEncrypt();
        private lClone clone = new lClone();
        private readonly Helper? _helper;

        public lData(IServiceProvider serviceProvider)
        {
            _helper = serviceProvider.GetService<Helper>();
        }

        public DataTable GetDdlWilayah()
        {
            JObject jReturn = new JObject();
            var iSQL = " select distinct wilayahkerja,wilayahkerja wilayahkerjadesc from ip.dpp01 ";

            var myconnImpala = obj.myConnImpala();
            myconnImpala.Open();
            OdbcCommand Odbccmd = new OdbcCommand(iSQL, myconnImpala);
            var datasource = new DataTable();
            try
            {
                datasource = obj.GetListImpala(Odbccmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                obj.CreateLog("#GetDdlWilayah#failed#" + ex.Message);
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
            }

            return datasource;
        }

        public bool BulkDataMatching(string TblName, RekapBelumMatching dt)
        {
            var jReturn = new JObject();
            var iidpelapor = "";
            var iSQL = "";
            var grpSQL = "";
            String str = obj.getGroupInstansi(dt.username);     //HttpContext.Current.Session["groupid"].ToString();
            grpSQL += "where a.idpelapor in ('" + str.Replace(",", "','") + "') ";
            //DataTable inf = obj.getIdInformasi(dt.username);
            List<string> iFilterInf = new List<string> { "trxSpotDerivatif", "trxPuabPuasDoc", "trxSuratBerhargaPsrSekunder" };
            var infRows = obj.getIdInformasi(dt.username).AsEnumerable().Where(row => iFilterInf.Contains(row.Field<string>("IDInformasi"))).Distinct(DataRowComparer.Default);
            string paramAll = "";
            string paramDt = "";
            string nSQL = "";

            if (infRows.Count() > 0)
            {
                foreach (DataRow a in infRows)
                {
                    if (a["IDInformasi"].ToString() == "trxSpotDerivatif")
                    {
                        //iSQL += "SELECT 'Spot dan Derivatif' as informasi,sum(isnull(s0.jumlah_transaksi,0)) as jumlah_transaksi, sum(isnull(s2.jumlah_transaksi_matched, 0)) as belummatch, sum(isnull(s3.jumlah_transaksi_unmatched, 0)) as belum_matched_dari_pihak_lawan FROM ( select a.idpelapor, a.nama from ip.dpp01 a " 
                        //    + //grpSQL + 
                        //    " {0} ) as s left join ( select a.idpelapor,  b.nama, count(a.idpelapor) as jumlah_transaksi from ip.tsd01_matched a left join ip.dpp01 b on a.idpelapor = b.idpelapor where a.periodedata = strleft(cast(now() as string), 10) and isnull(status,'')<> '' group by  a.idpelapor, b.nama ) as s0 on s.idpelapor = s0.idpelapor left join ( select c.idpelapor, count(c.idpelapor) jumlah_transaksi_matched from ip.tsd01_matched c where c.periodedata = strleft(cast(now() as string), 10) and isnull(c.status,'') = 'UNMATCHED' group by c.idpelapor) as s2 on s.idpelapor = s2.idpelapor left join (select d.idpihaklawan, count(d.idpihaklawan) jumlah_transaksi_unmatched from ip.tsd01_matched d where d.periodedata = strleft(cast(now() as string), 10) and isnull(d.status,'') = 'UNMATCHED' group by d.idpihaklawan) as s3  on substr(s.idpelapor,1,3) = s3.idpihaklawan";
                        //iSQL += " UNION ";

                        paramAll = dt.menuid + "|1|value";
                        paramDt = grpSQL;
                        nSQL += " " + obj.getStrImpala(paramAll, paramDt);
                    }
                    else if (a["IDInformasi"].ToString() == "trxPuabPuasDoc")
                    {
                        //iSQL += "SELECT 'PUAB/PUAS' as informasi,sum(isnull(s0.jumlah_transaksi,0)) as jumlah_transaksi, sum(isnull(s2.jumlah_transaksi_matched, 0)) as belummatch,sum(isnull(s3.jumlah_transaksi_unmatched, 0)) as belum_matched_dari_pihak_lawan FROM ( select a.idpelapor, a.nama from ip.dpp01 a " 
                        //    + //grpSQL + 
                        //    " {0} ) as s  left join ( select a.idpelapor,  b.nama, count(a.idpelapor) as jumlah_transaksi from ip.tpu01_matched a left join ip.dpp01 b on a.idpelapor = b.idpelapor where a.periodedata = strleft(cast(now() as string), 10) and isnull(status,'')<> '' group by  a.idpelapor, b.nama ) as s0 on s.idpelapor = s0.idpelapor left join (select c.idpelapor, count(c.idpelapor) jumlah_transaksi_matched from ip.tpu01_matched c where c.periodedata = strleft(cast(now() as string), 10) and isnull(c.status,'') = 'UNMATCHED' group by c.idpelapor) as s2 on s.idpelapor = s2.idpelapor left join (select d.idpihaklawan, count(d.idpihaklawan) jumlah_transaksi_unmatched from ip.tpu01_matched d where d.periodedata = strleft(cast(now() as string), 10) and isnull(d.status,'') = 'UNMATCHED' group by d.idpihaklawan) as s3  on substr(s.idpelapor,1,3) = s3.idpihaklawan";
                        //iSQL += " UNION ";

                        paramAll = dt.menuid + "|2|value";
                        paramDt = grpSQL;
                        nSQL += " " + obj.getStrImpala(paramAll, paramDt);
                    }
                    else if (a["IDInformasi"].ToString() == "trxSuratBerhargaPsrSekunder")
                    {
                        //iSQL += "SELECT 'TRX SSB di Pasar Sekunder' as informasi, sum(isnull(s0.jumlah_transaksi, 0)) as jumlah_transaksi, sum(isnull(s2.jumlah_transaksi_matched, 0)) as belummatch, sum(isnull(s3.jumlah_transaksi_unmatched, 0)) as belum_matched_dari_pihak_lawan FROM(select distinct a.idpelapor, a.nama from ip.dpp01 a " 
                        //    + //grpSQL + 
                        //    " {0} ) as s left join ( select a.idpelapor, b.nama, count(a.idpelapor) as jumlah_transaksi from ip.trs01_matched a left join ip.dpp01 b on a.idpelapor = b.idpelapor where a.periodedata = strleft(cast(now() as string), 10) and isnull(status,'')<> '' group by  a.idpelapor, b.nama ) as s0 on s.idpelapor = s0.idpelapor left join ( select c.idpelapor, count(c.idpelapor) jumlah_transaksi_matched from ip.trs01_matched c where c.periodedata = strleft(cast(now() as string), 10) and isnull(c.status,'') = 'UNMATCHED' group by c.idpelapor) as s2 on s.idpelapor = s2.idpelapor left join ( select idpihaklawan2, sum(jumlah_transaksi_unmatched) as jumlah_transaksi_unmatched from(select case  when substr(d.idpelapor, 1, 3) <> d.idpembeli then d.idpembeli else d.idpenjual end as idpihaklawan2, case when substr(d.idpelapor, 1, 3) <> d.idpembeli then count(d.idpembeli) else count(d.idpenjual) end as jumlah_transaksi_unmatched from ip.trs01_matched d where d.periodedata = strleft(cast(now() as string), 10) and isnull(d.status, '') = 'UNMATCHED' group by idpihaklawan2, d.idpelapor, d.idpembeli, d.idpenjual) as aa group by idpihaklawan2) as s3  on substr(s.idpelapor,1,3) = s3.idpihaklawan2";
                        //iSQL += " UNION ";
                        paramAll = dt.menuid + "|3|value";
                        paramDt = grpSQL;
                        nSQL += " " + obj.getStrImpala(paramAll, paramDt);
                    }

                }
                nSQL = nSQL.ToString().Substring(0, nSQL.Length - 6);
            }
            else
            {
                paramAll += dt.menuid + "|4|value";
                paramDt += grpSQL;
                nSQL += obj.getStrImpala(paramAll, paramDt);
                //iSQL = "SELECT '' as informasi, '' as jumlah_transaksi, '' as belummatch, '' as belum_matched_dari_pihak_lawan FROM ip.trs01_matched WHERE 1=2";
            }

            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();

                SqlCommand cmdsql = new SqlCommand("bm_getdtcreatetmptable", myConn);
                cmdsql.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmdsql);

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    //bulkCopy.DestinationTableName = "dbo."+ TblName;
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    //bulkCopy.BatchSize = 20000;
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    bulkCopy.WriteToServer(dsTbl);
                }
                myConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#GetDataMatching#failed#" + dt.username + "#" + ex.Message);
                //var path = "C:\\AntasenaAPI\\ErrAPI2.txt";
                //if (System.IO.File.Exists(path))
                //{
                //    var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append(now + "#GetDataMatching#" + ex.Message + "\n");
                //    System.IO.File.AppendAllText(path, sb.ToString());
                //    sb.Clear();
                //}
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        public Boolean BulkAbsensi(string TblName, LapStatusPenyampaian Dt)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();

            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
                var pd3 = "";
                if (Dt.Periode == "Harian")
                {
                    iPeriode = "D";
                    var idate = Dt.Harian.Split('-');
                    pd2 = idate[2] + "-" + idate[1] + "-" + idate[0];
                    //pd2 = "'" + pd1 + "'";
                    pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                }
                else if (Dt.Periode == "Mingguan")
                {
                    //iPeriode = "W" + Dt.Minggu;
                    iPeriode = "W";

                    var MMinggu = Dt.Minggu; var MBulan = Dt.MBulan;
                    //if (MMinggu == "4")
                    //{
                    //    MBulan = (Convert.ToInt32(MBulan) + 1).ToString();
                    //    if (MBulan == "13") MBulan = "1";
                    //}

                    string bln = "00" + Dt.MBulan;
                    //string bln = "00" + MBulan;

                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        //pd1 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + GetPeriodeMingguan(Dt.MBulan, "1");
                        pd2 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-" + obj.GetPeriodeMingguan(Dt.Minggu, Dt.MTahun, Dt.MBulan, "1");
                        //var dt1 = DateTime.Parse(pd2).AddDays(-1);
                        //pd2 = dt1.ToString("yyyy-MM-dd");
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";

                        iPeriode = "W";
                    }
                    else
                    {
                        pd2 = "concat(cast(year(" + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.MTahun + bln.Substring(bln.Length - 2, 2) + "'";

                        iPeriode = "W" + Dt.Minggu;
                    }
                }
                else if (Dt.Periode == "Bulanan")
                {
                    iPeriode = "M";
                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        if (Dt.BBulan == "12")
                        {
                            var thn = Convert.ToInt32(Dt.BTahun) + 1;
                            pd2 = thn.ToString() + "-01-01";
                        }
                        else
                        {
                            var bln0 = Convert.ToInt32(Dt.BBulan) + 1;
                            string bln = "00" + bln0;
                            pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                        }
                        var dt1 = DateTime.Parse(pd2).AddDays(-1);
                        pd2 = dt1.ToString("yyyy-MM-dd");
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                    }
                    else
                    {
                        string bln = "00" + Dt.BBulan;
                        pd2 = "concat(cast(year(" + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.BTahun + bln.Substring(bln.Length - 2, 2) + "'";
                    }
                    //string bln = "00" + Dt.BBulan;
                    //pd1 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                    //pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-31";
                }
                else if (Dt.Periode == "Triwulan")
                {
                    iPeriode = "Q";
                    if (Dt.Triwulan == "1")
                    {
                        //iPeriode = "Q1";
                        pd1 = Dt.TTahun + "-01-01";
                        pd2 = Dt.TTahun + "-03-31";
                        var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                        pd3 = tmpThn.ToString() + "-12-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Triwulan == "2")
                    {
                        //iPeriode = "Q2";
                        pd1 = Dt.TTahun + "-04-01";
                        pd2 = Dt.TTahun + "-06-30";
                        pd3 = Dt.TTahun + "-03-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }

                    }
                    else if (Dt.Triwulan == "3")
                    {
                        //iPeriode = "Q3";
                        pd1 = Dt.TTahun + "-07-01";
                        pd2 = Dt.TTahun + "-09-30";
                        pd3 = Dt.TTahun + "-06-30";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Triwulan == "4")
                    {
                        //iPeriode = "Q4";
                        pd1 = Dt.TTahun + "-10-01";
                        pd2 = Dt.TTahun + "-12-31";
                        pd3 = Dt.TTahun + "-09-30";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "").Replace("'", "");
                            pd2 = pd2.Replace("-", "").Replace("'", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                }
                else if (Dt.Periode == "Semesteran")
                {
                    iPeriode = "S";
                    if (Dt.Semester == "1")
                    {
                        pd1 = Dt.STahun + "-01-01";
                        pd2 = Dt.STahun + "-06-30";

                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Semester == "2")
                    {
                        pd1 = Dt.STahun + "-07-01";
                        pd2 = Dt.STahun + "-12-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "").Replace("'", "");
                            pd2 = pd2.Replace("-", "").Replace("'", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                }
                else if (Dt.Periode == "Tahunan")
                {
                    iPeriode = "A";
                    pd1 = Dt.Tahunan + "-01-01";
                    pd2 = Dt.Tahunan + "-12-31";

                    if (Dt.pdata_penyampaian != "periodedata")
                    {
                        pd1 = pd1.Replace("-", "").Replace("'", "");
                        pd2 = pd2.Replace("-", "").Replace("'", "");
                        pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ") as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ") as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                    }
                    else
                    {
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                    }
                }

                var iStatus = "";
                for (var st = 0; st < Dt.Status.Split(',').Count(); st++)
                {
                    if (st == 0)
                    {
                        iStatus += "lower('" + Dt.Status.Split(',')[st].Split(';')[1] + "')";
                    }
                    else
                    {
                        iStatus += ",lower('" + Dt.Status.Split(',')[st].Split(';')[1] + "')";
                    }
                }

                ////From Impala
                var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                //var idInf = "'" + Dt.idinformation.Replace(",", "','") + "'";
                var kelinf = "'" + Dt.KelInformasi.Replace(",", "','") + "'";
                var wilayahkerja = "'" + Dt.wilayahkerja.Replace(",", "','") + "'";
                var icakupan = ((Dt.Cakupan == "0") ? "gabungan" : "individual");

                string paramAll = "";
                string paramDt = "";

                if (Dt.Periode == "Triwulan")
                {
                    paramAll = Dt.menuid + "|2|value";
                }
                else
                {
                    paramAll = Dt.menuid + "|1|value";
                }

                bool iStatusKIKoreksi = true;
                if (iPeriode != "H" && Dt.TblNameDtl != "")
                {
                    iStatusKIKoreksi = BulkAbsensiStatusKIKoreksi(Dt.TblNameDtl, Dt, iPeriode, pd2, kelinf, idPlp, wilayahkerja);
                    return iStatusKIKoreksi;
                }
                else
                {
                    paramDt += iPeriode + "|";
                    paramDt += pd2 + "|";
                    paramDt += kelinf + "|";
                    //paramDt += iStatus + "|";
                    //paramDt += iStatus + "|";
                    paramDt += idPlp + "|";
                    paramDt += wilayahkerja + "|";
                    paramDt += icakupan;
                    string nSQL = obj.getStrImpala(paramAll, paramDt);

                    DataTable dsTbl;

                    if (_helper.IsLocalhost())
                    {
                        dsTbl = Helper.GetDummyLapAbsensiData();
                    }
                    else
                    {
                        myconnImpala.Open();
                        OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                        dsTbl = obj.GetListImpala(Odbccmd);
                        myconnImpala.Close();
                    }


                    SqlCommand cmd = new SqlCommand("la_dropCreateTblTemp", myConn);
                    cmd.Parameters.AddWithValue("@TblName", TblName);
                    obj.Exec(myConn, cmd);

                    //SqlCommand cmdDtl = new SqlCommand("lsp_getTEST", myConn);
                    //DataTable dsTbl = obj.GetList(myConn, cmdDtl);
                    //insert to SQL Server
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                    {
                        //bulkCopy.DestinationTableName = "dbo."+ TblName;
                        bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                        bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                        bulkCopy.DestinationTableName = $"[{TblName}]";

                        try
                        {
                            bulkCopy.WriteToServer(dsTbl);
                            ireturn = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            obj.CreateLog("#GetBulkAbsensi#failed#" + Dt.username + "#" + ex.Message);
                        }
                    }
                    myConn.Close();
                }
                return ireturn;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#GetBulkAbsensi2#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }
        private Boolean BulkAbsensiStatusKIKoreksi(string TblName, LapStatusPenyampaian Dt, string Periodelap, string periodedata, string kelinf, string idPlp, string wilker)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                //var idval = (Dt.pdata_penyampaian == "periodedata") ? "3" : "4";
                var paramAll = Dt.menuid + "|3|value";
                string paramDt = "";
                paramDt += idPlp + "|";
                paramDt += kelinf + "|";
                paramDt += Periodelap + "|";
                paramDt += periodedata + "|";
                paramDt += wilker;
                string nSQL = obj.getStrImpala(paramAll, paramDt);

                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();

                //TblName = TblName.Replace("ZZLA_", "ZZLATT_");
                SqlCommand cmd = new SqlCommand("la_dropCreateTblTemp_tt", myConn);
                cmd.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmd);

                //SqlCommand cmdDtl = new SqlCommand("lsp_getTEST", myConn);
                //DataTable dsTbl = obj.GetList(myConn, cmdDtl);
                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        obj.CreateLog("#BulkAbsensiStatusKIKoreksi#failed#" + Dt.username + "#" + ex.Message);
                    }
                }
                myConn.Close();
                return ireturn;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkAbsensiStatusKIKoreksi2#failed#" + Dt.username + "#" + ex.Message);
                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        public Boolean BulkRincianAbsensi(string TblName, LapStatusPenyampaian Dt)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();

            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
                var pd3 = "";
                if (Dt.Periode == "Harian")
                {
                    iPeriode = "D";
                    var idate = Dt.Harian.Split('-');
                    pd2 = idate[2] + "-" + idate[1] + "-" + idate[0];
                    pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                }
                else if (Dt.Periode == "Mingguan")
                {
                    iPeriode = "W";

                    var MMinggu = Dt.Minggu; var MBulan = Dt.MBulan;
                    string bln = "00" + Dt.MBulan;

                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        pd2 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-" + obj.GetPeriodeMingguan(Dt.Minggu, Dt.MTahun, Dt.MBulan, "1");
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";

                        iPeriode = "W";
                    }
                    else
                    {
                        pd2 = "concat(cast(year(" + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.MTahun + bln.Substring(bln.Length - 2, 2) + "'";

                        iPeriode = "W" + Dt.Minggu;
                    }
                }
                else if (Dt.Periode == "Bulanan")
                {
                    iPeriode = "M";
                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        if (Dt.BBulan == "12")
                        {
                            var thn = Convert.ToInt32(Dt.BTahun) + 1;
                            pd2 = thn.ToString() + "-01-01";
                        }
                        else
                        {
                            var bln0 = Convert.ToInt32(Dt.BBulan) + 1;
                            string bln = "00" + bln0;
                            pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                        }
                        var dt1 = DateTime.Parse(pd2).AddDays(-1);
                        pd2 = dt1.ToString("yyyy-MM-dd");
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                    }
                    else
                    {
                        string bln = "00" + Dt.BBulan;
                        pd2 = "concat(cast(year(" + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.BTahun + bln.Substring(bln.Length - 2, 2) + "'";
                    }
                }
                else if (Dt.Periode == "Triwulan")
                {
                    iPeriode = "Q";
                    if (Dt.Triwulan == "1")
                    {
                        //iPeriode = "Q1";
                        pd1 = Dt.TTahun + "-01-01";
                        pd2 = Dt.TTahun + "-03-31";
                        var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                        pd3 = tmpThn.ToString() + "-12-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Triwulan == "2")
                    {
                        //iPeriode = "Q2";
                        pd1 = Dt.TTahun + "-04-01";
                        pd2 = Dt.TTahun + "-06-30";
                        pd3 = Dt.TTahun + "-03-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }

                    }
                    else if (Dt.Triwulan == "3")
                    {
                        //iPeriode = "Q3";
                        pd1 = Dt.TTahun + "-07-01";
                        pd2 = Dt.TTahun + "-09-30";
                        pd3 = Dt.TTahun + "-06-30";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Triwulan == "4")
                    {
                        pd1 = Dt.TTahun + "-10-01";
                        pd2 = Dt.TTahun + "-12-31";
                        pd3 = Dt.TTahun + "-09-30";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "").Replace("'", "");
                            pd2 = pd2.Replace("-", "").Replace("'", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                }
                else if (Dt.Periode == "Semesteran")
                {
                    iPeriode = "S";

                    if (Dt.Semester == "1")
                    {
                        pd1 = Dt.STahun + "-01-01";
                        pd2 = Dt.STahun + "-06-30";
                        var tmpThn = Convert.ToInt32(Dt.STahun) - 1;

                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Semester == "2")
                    {
                        pd1 = Dt.STahun + "-07-01";
                        pd2 = Dt.STahun + "-12-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "").Replace("'", "");
                            pd2 = pd2.Replace("-", "").Replace("'", "");
                            pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                }
                else if (Dt.Periode == "Tahunan")
                {
                    iPeriode = "A";
                    pd1 = Dt.Tahunan + "-01-01";
                    pd2 = Dt.Tahunan + "-12-31";

                    if (Dt.pdata_penyampaian != "periodedata")
                    {
                        pd1 = pd1.Replace("-", "").Replace("'", "");
                        pd2 = pd2.Replace("-", "").Replace("'", "");
                        pd2 = "from_timestamp(concat(cast(year(" + Dt.pdata_penyampaian + ") as string),'-',strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ") as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                    }
                    else
                    {
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                    }
                }


                var iStatus = "";
                for (var st = 0; st < Dt.Status.Split(',').Count(); st++)
                {
                    if (st == 0)
                    {
                        iStatus += "lower('" + Dt.Status.Split(',')[st].Split(';')[1] + "')";
                    }
                    else
                    {
                        iStatus += ",lower('" + Dt.Status.Split(',')[st].Split(';')[1] + "')";
                    }
                }
                ////From Impala
                var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                //var idInf = "'" + Dt.idinformation.Replace(",", "','") + "'";
                var kelinf = "'" + Dt.KelInformasi.Replace(",", "','") + "'";
                var wilayahkerja = "'" + Dt.wilayahkerja.Replace(",", "','") + "'";
                //var iStatus = Dt.Status.Split(';')[1];
                var icakupan = ((Dt.Cakupan == "0") ? "gabungan" : "individual");

                //var iSQL =
                //   " select b.IdPelapor, b.versioncode, b.Nama, b.KantorCabang, " +
                //   " b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasi, b.namainformasi, " +
                //   " b.PeriodeData, b.laporanstatus, b.koreksistatus, b.laporantimestamp, b.koreksitimestamp " +
                //   " from (" +
                //   " select " +
                //   " idpelapor as IdPelapor " +
                //   " ,versioncode as versioncode " +
                //   " ,namabank as Nama " +
                //   " ,'kantorcabang' as KantorCabang " +
                //   " ,periodelaporan as PeriodeLaporan " +
                //   " ,kelompokinformasi as kelompokinformasi " +
                //   " ,idinformasi as IdInformasi  " +
                //   " ,informasi as namainformasi " +
                //   " ,periodedata as PeriodeData " +
                //   " ,laporanstatus as laporanstatus " +
                //   " ,koreksistatus as koreksistatus " +
                //   " ,laporantimestamp as laporantimestamp " +
                //   " ,koreksitimestamp as koreksitimestamp " +
                //   " from (" +
                //   " select *,rank() over (partition by strleft(periodelaporan,1),versioncode order by recordtimestamp_ desc) as rank " +
                //   " from (select case when isnull(lower(recordtimestamp),'0')='null' then '0' else isnull(recordtimestamp,'0') end as recordtimestamp_,* " +
                //   " from  ip_rpt.view_final_absensi ) as z " +
                //   " where periodelaporan like '{0}%' " +
                //   " and {1} " +
                //   " and kelompokinformasi in ({2}) " +
                //   " and idpelapor in ({5}) " +
                //   " and wilayahkerja in ({6}) " +
                //   " and lower(cakupan) ='{7}' " +
                //   " ) as a" +
                //   " where a.rank=1) as b " +
                //   " where (lower(b.laporanstatus) in ({3}) or lower(b.koreksistatus) in ({4}) ) ";
                //iSQL = string.Format(iSQL, iPeriode, pd2, kelinf, iStatus, iStatus, idPlp, wilayahkerja, icakupan);
                //" LIMIT 5 ";
                //iSQL = string.Format(iSQL, iPeriode, pd2, kelinf, iStatus, iStatus, idPlp, Dt.wilayahkerja,Dt.pdata_penyampaian, icakupan);

                string paramAll = "";

                if (Dt.Periode == "Triwulan")
                {
                    paramAll = Dt.menuid + "|2|value";
                }
                else
                {
                    paramAll = Dt.menuid + "|1|value";
                }

                string paramDt = "";
                paramDt += iPeriode + "|";
                paramDt += pd2 + "|";
                paramDt += kelinf + "|";
                //paramDt += iStatus + "|";
                //paramDt += iStatus + "|";
                paramDt += idPlp + "|";
                paramDt += wilayahkerja + "|";
                paramDt += icakupan;
                string nSQL = obj.getStrImpala(paramAll, paramDt);

                DataTable dsTbl;

            
                    myconnImpala.Open();
                    OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                    dsTbl = obj.GetListImpala(Odbccmd);
                    myconnImpala.Close();

                SqlCommand cmd = new SqlCommand("la_dropCreateTblTemp", myConn);
                cmd.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmd);

                //SqlCommand cmdDtl = new SqlCommand("lsp_getTEST", myConn);
                //DataTable dsTbl = obj.GetList(myConn, cmdDtl);
                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    //bulkCopy.DestinationTableName = "dbo."+ TblName;
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {
                        obj.CreateLog("#GetBulkRincianAbsensi#failed#" + Dt.username + "#" + ex.Message);
                        //Console.WriteLine(ex.Message);
                    }
                }
                myConn.Close();
                return ireturn;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#GetBulkRincianAbsensi2#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        public Boolean BulkRincianAbsensiDtl(string TblName, LapStatusPenyampaian Dt)
        {
            var ireturn = false;
            var myConn1 = obj.myConn();
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                SqlCommand cmd1 = new SqlCommand("lra_getParamPeriode", myConn1);
                cmd1.Parameters.AddWithValue("@TblName", Dt.TblName);
                cmd1.Parameters.AddWithValue("@IdPelapor", Dt.IdPelapor);
                cmd1.Parameters.AddWithValue("@Status", Dt.Status);
                DataTable dsData = obj.GetList(myConn1, cmd1);
                myConn1.Close();

                string paramAll = Dt.menuid + "|3|value";

                string paramDt = "";
                paramDt += Dt.IdPelapor + "|";
                paramDt += dsData.Rows[0]["PeriodeData"].ToString() + "|";
                paramDt += Dt.Periode;

                string nSQL = obj.getStrImpala(paramAll, paramDt);

                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();

                SqlCommand cmd = new SqlCommand("lra_dropCreateTblTemp", myConn);
                cmd.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmd);

                //SqlCommand cmdDtl = new SqlCommand("lsp_getTEST", myConn);
                //DataTable dsTbl = obj.GetList(myConn, cmdDtl);
                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    //bulkCopy.DestinationTableName = "dbo."+ TblName;
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {
                        obj.CreateLog("#BulkRincianAbsensiDtl#failed#" + Dt.username + "#" + ex.Message);

                    }
                }
                myConn.Close();
                return ireturn;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkRincianAbsensiDtl2#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
                if (myConn1 != null) myConn1.Close();
            }
        }

        public Boolean BulkStatusPenyampaian(string TblName, LapStatusPenyampaian Dt)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
                var pd3 = "";

                if (Dt.Periode == "Harian")
                {
                    iPeriode = "D";
                    var idate = Dt.Harian.Split('-');
                    pd2 = idate[2] + "-" + idate[1] + "-" + idate[0];

                }
                else if (Dt.Periode == "Mingguan")
                {
                    iPeriode = "W";
                    var MMinggu = Dt.Minggu; var MBulan = Dt.MBulan;


                    string bln = "00" + Dt.MBulan;

                    pd2 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-" + obj.GetPeriodeMingguan(Dt.Minggu, Dt.MTahun, Dt.MBulan, "1");

                }
                else if (Dt.Periode == "Bulanan")
                {
                    iPeriode = "M";

                    if (Dt.BBulan == "12")
                    {
                        var thn = Convert.ToInt32(Dt.BTahun) + 1;
                        pd2 = thn.ToString() + "-01-01";
                    }
                    else
                    {
                        var bln0 = Convert.ToInt32(Dt.BBulan) + 1;
                        string bln = "00" + bln0;
                        pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                    }
                    var dt1 = DateTime.Parse(pd2).AddDays(-1);
                    pd2 = dt1.ToString("yyyy-MM-dd");
                }
                else if (Dt.Periode == "Triwulan")
                {
                    iPeriode = "Q";
                    if (Dt.Triwulan == "1")
                    {
                        //iPeriode = "Q1";
                        pd1 = Dt.TTahun + "-01-01";
                        pd2 = Dt.TTahun + "-03-31";
                        var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                        pd3 = tmpThn.ToString() + "-12-31";
                        //pd2 = pd3;

                    }
                    else if (Dt.Triwulan == "2")
                    {
                        //iPeriode = "Q2";
                        pd1 = Dt.TTahun + "-04-01";
                        pd2 = Dt.TTahun + "-06-30";
                        pd3 = Dt.TTahun + "-03-31";
                        //pd2 = pd3;
                    }
                    else if (Dt.Triwulan == "3")
                    {
                        //iPeriode = "Q3";
                        pd1 = Dt.TTahun + "-07-01";
                        pd2 = Dt.TTahun + "-09-30";
                        pd3 = Dt.TTahun + "-06-30";
                        //pd2 = pd3;
                    }
                    else if (Dt.Triwulan == "4")
                    {
                        //iPeriode = "Q4";
                        pd1 = Dt.TTahun + "-10-01";
                        pd2 = Dt.TTahun + "-12-31";
                        pd3 = Dt.TTahun + "-09-30";
                        //pd2 = pd3;
                    }
                }
                else if (Dt.Periode == "Semesteran")
                {
                    iPeriode = "S";
                    if (Dt.Semester == "1")
                    {
                        //string F_STahun = (int.Parse(Dt.STahun) - 1).ToString();
                        //pd2 = F_STahun + "-12-31";
                        pd2 = Dt.STahun + "-06-30";
                        //pd2 = STahun + "-06-30";
                    }
                    else if (Dt.Semester == "2")
                    {
                        //pd2 = STahun + "-12-31";
                        pd2 = Dt.STahun + "-12-31";
                    }
                }
                else if (Dt.Periode == "Tahunan")
                {
                    iPeriode = "A";
                    pd2 = Dt.TnTahun + "-12-31";
                }

                ////From Impala
                var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                var idInf = "'" + Dt.idinformation.Replace(",", "','") + "'";
                var kelinf = "'" + Dt.KelInformasi.Replace(",", "','") + "'";
                var wilker = "'" + Dt.wilayahkerja.Replace(",", "','") + "'";

                string paramAll = "";
                string paramDt = "";

                paramAll = Dt.menuid + "|1|sql_ZZ";
                paramDt += idPlp + "|";
                paramDt += iPeriode + "|";
                paramDt += pd2 + "|";
                paramDt += kelinf + "|";
                paramDt += idInf + "|";
                paramDt += wilker;
                string isplit = ";";
                string p1 = "impala" + isplit + idInf;
                string p2 = "phoenix" + isplit + idInf;
                ireturn = clone.generateTmpTbl(Dt.menuid, paramAll, paramDt, TblName, p1, p2);
                return ireturn;

            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkStatusPenyampaian2#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        public Boolean BulkRincianGagalValidasi(string TblName, LapRincianGagalValidasi Dt)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
                var pd3 = "";

                if (Dt.Periode == "Harian")
                {
                    iPeriode = "D";
                    var idate = Dt.Harian.Split('-');
                    pd2 = idate[2] + "-" + idate[1] + "-" + idate[0];

                }
                else if (Dt.Periode == "Mingguan")
                {
                    iPeriode = "W";
                    var MMinggu = Dt.Minggu; var MBulan = Dt.MBulan;


                    string bln = "00" + Dt.MBulan;

                    pd2 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-" + obj.GetPeriodeMingguan(Dt.Minggu, Dt.MTahun, Dt.MBulan, "1");

                }
                else if (Dt.Periode == "Bulanan")
                {
                    iPeriode = "M";

                    if (Dt.BBulan == "12")
                    {
                        var thn = Convert.ToInt32(Dt.BTahun) + 1;
                        pd2 = thn.ToString() + "-01-01";
                    }
                    else
                    {
                        var bln0 = Convert.ToInt32(Dt.BBulan) + 1;
                        string bln = "00" + bln0;
                        pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                    }
                    var dt1 = DateTime.Parse(pd2).AddDays(-1);
                    pd2 = dt1.ToString("yyyy-MM-dd");
                }
                else if (Dt.Periode == "Triwulan")
                {
                    iPeriode = "Q";
                    if (Dt.Triwulan == "1")
                    {
                        //iPeriode = "Q1";
                        pd1 = Dt.TTahun + "-01-01";
                        pd2 = Dt.TTahun + "-03-31";
                        var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                        pd3 = tmpThn.ToString() + "-12-31";
                        //pd2 = pd3;

                    }
                    else if (Dt.Triwulan == "2")
                    {
                        //iPeriode = "Q2";
                        pd1 = Dt.TTahun + "-04-01";
                        pd2 = Dt.TTahun + "-06-30";
                        pd3 = Dt.TTahun + "-03-31";
                        //pd2 = pd3;
                    }
                    else if (Dt.Triwulan == "3")
                    {
                        //iPeriode = "Q3";
                        pd1 = Dt.TTahun + "-07-01";
                        pd2 = Dt.TTahun + "-09-30";
                        pd3 = Dt.TTahun + "-06-30";
                        //pd2 = pd3;
                    }
                    else if (Dt.Triwulan == "4")
                    {
                        //iPeriode = "Q4";
                        pd1 = Dt.TTahun + "-10-01";
                        pd2 = Dt.TTahun + "-12-31";
                        pd3 = Dt.TTahun + "-09-30";
                        //pd2 = pd3;
                    }
                }
                else if (Dt.Periode == "Semesteran")
                {
                    iPeriode = "S";
                    if (Dt.Semester == "1")
                    {
                        //string F_STahun = (int.Parse(Dt.STahun) - 1).ToString();
                        //pd2 = F_STahun + "-12-31";
                        pd2 = Dt.STahun + "-06-30";
                        //pd2 = STahun + "-06-30";
                    }
                    else if (Dt.Semester == "2")
                    {
                        //pd2 = STahun + "-12-31";
                        pd2 = Dt.STahun + "-12-31";
                    }
                }
                else if (Dt.Periode == "Tahunan")
                {
                    iPeriode = "A";
                    pd2 = Dt.TnTahun + "-12-31";
                }

                ////From Impala
                var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                var idInf = "'" + Dt.idinformation.Replace(",", "','") + "'";
                var kelinf = "'" + Dt.KelInformasi.Replace(",", "','") + "'";
                var wilker = "'" + Dt.wilayahkerja.Replace(",", "','") + "'";


                string paramAll = Dt.menuid + "|1|value";
                string paramDt = "";
                paramDt += iPeriode + "|";
                paramDt += pd2 + "|";
                paramDt += kelinf + "|";
                paramDt += idPlp + "|";
                paramDt += idInf + "|";
                paramDt += wilker;
                string nSQL = @"" + obj.getStrImpala(paramAll, paramDt);

                //Master No Delete
                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();
                //Master No Delete

                ////SQL
                //Dt.ReportQuery = "select 150000000000/1000000 as hasil,* from ZZRPTC_TEST01";
                //var myConn = obj.myConn();
                //SqlCommand cmd1 = new SqlCommand(nSQL, myConn);
                //SqlDataReader dr = cmd1.GetListSql(cmd1);

                ///GET DATA FROM SQL LOCAL
                //SqlCommand sqlcmd = new SqlCommand(nSQL, myConn);
                //var dsTbl = obj.GetListSql(sqlcmd);
                ///GET DATA FROM SQL LOCAL

                SqlCommand cmd = new SqlCommand("rgv_dropCreateTblTemp", myConn);
                cmd.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmd);

                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {
                        obj.CreateLog("#BulkRincianGagalValidasi#failed#" + Dt.username + "#" + ex.Message);

                    }
                }
                myConn.Close();
                return ireturn;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkRincianGagalValidasi2#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        public Boolean BulkRekapBelumMatching(string TblName, RekapBelumMatching Dt)
        {
            var pd1 = "";
            var pd2 = "";
            var idate = Dt.PeriodeData.Split('-');
            pd1 = idate[2] + "-" + idate[1] + "-" + idate[0];
            pd2 = pd1;
            var idPlp = Dt.IdPelapor.Replace(",", "','");

            var jReturn = new JObject();
            var iidpelapor = "";
            var iSQL = "";
            var grpSQL = "";
            String str = obj.getGroupInstansi(Dt.username);
            grpSQL += "where a.idpelapor in ('" + str.Replace(",", "','") + "') ";

            string paramAll = "";
            string paramDt = "";
            string nSQL = "";

            var dsTbl = new DataTable();
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                if (Dt.Informasi != "ip_rpt.view_trs01_matched")
                {
                    var iccp = (Dt.Informasi == "ip_rpt.view_tsd01_matched") ? ((Dt.Kontrak.ToLower() == "ccp") ? obj.getStrImpala(Dt.menuid + "|0|ccp", "") : ((Dt.Kontrak.ToLower() == "non ccp") ? obj.getStrImpala(Dt.menuid + "|0|non ccp", "") : "")) : ""; // CCP 
                    if (Dt.IdPelapor == "%")
                    {
                        paramAll = Dt.menuid + "|1|value";
                        paramDt += grpSQL + "|";
                        paramDt += Dt.Informasi + "|";
                        paramDt += pd2 + "|";
                        paramDt += Dt.Informasi + "|";
                        paramDt += pd2 + "|";
                        paramDt += Dt.Informasi + "|";
                        paramDt += pd2 + "|";
                        paramDt += iccp + "|";
                        nSQL = obj.getStrImpala(paramAll, paramDt);
                    }
                    else
                    {
                        paramAll = Dt.menuid + "|2|value";
                        iidpelapor = " in('" + idPlp + "') ";
                        paramDt += iidpelapor + "|";
                        paramDt += Dt.Informasi + "|";
                        paramDt += pd2 + "|";
                        paramDt += Dt.Informasi + "|";
                        paramDt += pd2 + "|";
                        paramDt += Dt.Informasi + "|";
                        paramDt += pd2 + "|";
                        paramDt += iccp + "|";
                        nSQL = obj.getStrImpala(paramAll, paramDt);
                    }
                }
                else
                {
                    if (Dt.IdPelapor == "%")
                    {
                        paramAll = Dt.menuid + "|3|value";
                        paramDt += grpSQL + "|";
                        paramDt += pd2 + "|";
                        paramDt += pd2 + "|";
                        paramDt += pd2;
                        nSQL = obj.getStrImpala(paramAll, paramDt);
                    }
                    else
                    {
                        paramAll = Dt.menuid + "|4|value";
                        iidpelapor = " in('" + idPlp + "') ";
                        paramDt += iidpelapor + "|";
                        paramDt += pd2 + "|";
                        paramDt += pd2 + "|";
                        paramDt += pd2;
                        nSQL = obj.getStrImpala(paramAll, paramDt);
                    }
                }




                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();

                SqlCommand cmdsql = new SqlCommand("rm_getdtcreatetmptable", myConn);
                cmdsql.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmdsql);

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    bulkCopy.WriteToServer(dsTbl);
                }
                myConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkRekapBelumMatching#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        public Boolean BulkRincianBelumMatching(string TblName, RincianBelumMatching Dt, string listidpelapor = "", string listkodepelapor = "", string listkodepelaporlawan = "")
        {
            var pd1 = "";
            var pd2 = "";
            var idate = Dt.PeriodeData.Split('-');
            pd1 = idate[2] + "-" + idate[1] + "-" + idate[0];
            pd2 = pd1;

            var jReturn = new JObject();
            var column = "";

            var plp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";  //idpelapor yang dipilih
            var lwn = "'" + Dt.IdLawan.Replace(",", "','") + "'";  //idpihak lawna yang dipilih
            var idPlp = "'" + listidpelapor.Replace(",", "','") + "'";  //idpelapor sesuai trustee
            var kodePlp = "'" + listkodepelapor.Replace(",", "','") + "'";  // kodepalpor yang dipilih / sesuai trstee
            var kodePlplawan = "'" + listkodepelaporlawan.Replace(",", "','") + "'";  //kode pihak lawan yang dipilih / semua
            var dsTbl = new DataTable();
            if (Dt.Informasi == "ip_rpt.view_tsd01_matched")
            {
                column = "tsd";
            }
            else if (Dt.Informasi == "ip_rpt.view_tpu01_matched")
            {
                column = "tpu";
            }
            else if (Dt.Informasi == "ip_rpt.view_trs01_matched")
            {
                column = "trs";
            }

            string dt1 = "";
            string dt2 = "";
            string dt3 = "";
            string st = "";

            switch (Dt.Status)
            {
                case "matched":
                    st = " isnull(status,'') = 'MATCHED'";
                    break;

                case "unmatched":
                    st = " isnull(status,'') = 'UNMATCHED'";
                    break;

                case "na":
                    st = " isnull(status,'') = 'NA'";
                    break;

                default:
                    st = "  isnull(status,'') in ( 'NA','UNMATCHED','MATCHED','') ";
                    break;
            }

            string iccp = (Dt.Informasi == "ip_rpt.view_tsd01_matched") ? ((Dt.Kontrak.ToLower() == "ccp") ? obj.getStrImpala(Dt.menuid + "|0|ccp", "") : ((Dt.Kontrak.ToLower() == "non ccp") ? obj.getStrImpala(Dt.menuid + "|0|non ccp", "") : "")) : ""; // CCP

            // idpelapor selected - pihak lawan selected
            if (Dt.IdPelapor != "" && !Dt.IdPelapor.ToLower().Equals("all")
                && !Dt.IdLawan.ToLower().Equals("all"))
            {
                if (Dt.Cakupan == "")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( (idpelapor in ( " + plp + " )  and idpihaklawanoriginal in ( " + kodePlplawan + " ) ) or " +
                                "         (idpelapor in ( " + lwn + " )  and idpihaklawanoriginal in ( " + kodePlp + " )) )  ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( (idpelapor in ( " + plp + " )  and idpihaklawan in ( " + kodePlplawan + " ) ) or " +
                                  "         (idpelapor in ( " + lwn + " )  and idpihaklawan in ( " + kodePlp + " )) )  ";

                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ((idpelapor in ( " + plp + " )  and (idpenjual in ( " + kodePlplawan + " )   or idpembeli in  ( " + kodePlplawan + " )) ) " +
                                  " or (idpelapor in ( " + lwn + " )  and ( idpenjual in ( " + kodePlp + " )   or idpembeli in  ( " + kodePlp + " )) )) ";
                            break;
                        default:

                            break;
                    }
                }
                else if (Dt.Cakupan == "P")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  and  idpihaklawanoriginal in (" + kodePlplawan + ") )  ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  and  idpihaklawan in (" + kodePlplawan + ") )  ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  and  (idpenjual in (" + kodePlplawan + ")  or  idpembeli in (" + kodePlplawan + "))   )  ";
                            break;
                        default:

                            break;
                    }
                }
                else
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpelapor in ( " + lwn + " )  and  idpihaklawanoriginal in (" + kodePlp + ") )  ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpelapor in ( " + lwn + " )  and  idpihaklawan in (" + kodePlp + ") )  ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( idpelapor in ( " + lwn + " )  and  (idpenjual in (" + kodePlp + ")  or  idpembeli in (" + kodePlp + "))   )  ";
                            break;
                        default:

                            break;
                    }
                }
            }

            //idpelapor selected id lawan all
            if (Dt.IdPelapor != "" && !Dt.IdPelapor.ToLower().Equals("all")
                && Dt.IdLawan.ToLower().Equals("all"))
            {
                if (Dt.Cakupan == "")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  or idpihaklawanoriginal in ( " + kodePlp + " )  ) ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  or idpihaklawan in ( " + kodePlp + " )  ) ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  or idpenjual in ( " + kodePlp + " )   or idpembeli in  ( " + kodePlp + " ) ) ";
                            break;
                        default:

                            break;
                    }
                }
                else if (Dt.Cakupan == "P")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  ) ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " )  ) ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( idpelapor in ( " + plp + " ) ) ";
                            break;
                        default:

                            break;
                    }
                }
                else
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpihaklawanoriginal in ( " + kodePlp + " )  ) ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpihaklawan in ( " + kodePlp + " )  ) ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( (  idpenjual in ( " + kodePlp + " ) and idpenjual <> left(idpelapor,3) )" +
                                "          or ( idpembeli in  ( " + kodePlp + " ) and idpembeli <> left(idpelapor,3) ) ) ";
                            break;
                        default:

                            break;
                    }
                }
            }

            //idpelapor all id lawan selected
            if (Dt.IdLawan != "" && !Dt.IdLawan.ToLower().Equals("all") && Dt.IdPelapor.ToLower().Equals("all"))
            {
                if (Dt.Cakupan == "")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpihaklawanoriginal in (" + kodePlplawan + ") )   ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpihaklawan in (" + kodePlplawan + ") )   ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND   ( idpenjual in (" + kodePlplawan + ") or idpembeli in (" + kodePlplawan + " ) )  ";
                            break;
                        default:

                            break;
                    }
                }
                else if (Dt.Cakupan == "P")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpihaklawanoriginal in (" + kodePlplawan + ") )   ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpihaklawan in (" + kodePlplawan + ") )   ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND   ( idpenjual in (" + kodePlplawan + ") or idpembeli in (" + kodePlplawan + " ) )  ";
                            break;
                        default:

                            break;
                    }

                }
                else
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpihaklawanoriginal in (" + kodePlplawan + ") )   ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpihaklawan in (" + kodePlplawan + ") )   ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND   ( idpenjual in (" + kodePlplawan + ") or idpembeli in (" + kodePlplawan + " ) )  ";
                            break;
                        default:

                            break;
                    }

                }
            }
            //idpelapor all id lawan all
            if (Dt.IdPelapor.ToLower().Equals("all") && Dt.IdLawan.ToLower().Equals("all"))
            {
                if (Dt.Cakupan == "")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpelapor in (" + idPlp + ")  or idpihaklawanoriginal in (" + kodePlp + ") )    ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpelapor in (" + idPlp + ")  or idpihaklawan in (" + kodePlp + ") )    ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( idpelapor in (" + idPlp + ")  and (idpenjual in (" + kodePlp + ")  or idpembeli in (" + kodePlp + ") ))  ";
                            break;
                        default:
                            break;
                    }

                }
                else if (Dt.Cakupan == "P")
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpelapor in (" + idPlp + ") )    ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND ( idpelapor in (" + idPlp + ") )    ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND ( idpelapor in (" + idPlp + ") )  ";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (Dt.Informasi)
                    {
                        case "ip_rpt.view_tsd01_matched":
                            dt1 = " AND ( idpihaklawanoriginal in (" + kodePlp + ") )    ";
                            break;

                        case "ip_rpt.view_tpu01_matched":
                            dt1 = " AND (  idpihaklawan in (" + kodePlp + ") )    ";
                            break;

                        case "ip_rpt.view_trs01_matched":
                            dt1 = " AND  ( idpenjual in (" + kodePlp + ")  or idpembeli in (" + kodePlp + ") )  ";
                            break;
                        default:

                            break;
                    }
                }
            }

            if (Dt.NomorRef != "")
            {
                dt2 = " AND nomorreftransaksi = '" + Dt.NomorRef + "'";
            }

            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                string paramAll = Dt.menuid + "|1|" + column;
                string paramDt = "";
                paramDt += st + "|";
                paramDt += pd2 + "|";
                paramDt += dt1 + "|";
                paramDt += dt2 + "|";
                paramDt += dt3 + "|";
                paramDt += iccp + "|";
                string nSQL = obj.getStrImpala(paramAll, paramDt);

                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();

                SqlCommand cmdsql = new SqlCommand("dm_getdtcreatetmptable", myConn);
                cmdsql.Parameters.AddWithValue("@TblName", TblName);
                cmdsql.Parameters.AddWithValue("@Informasi", Dt.Informasi);
                obj.Exec(myConn, cmdsql);

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    //bulkCopy.DestinationTableName = "dbo."+ TblName;
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    bulkCopy.WriteToServer(dsTbl);
                }
                myConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkRincianBelumMatching#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }
        private string cleansingStr(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                var inxp = input.ToLower();
                if (inxp.Contains("delete") ||
                    inxp.Contains("insert") ||
                    inxp.Contains("update") ||
                    inxp.Contains("truncate") ||
                     inxp.Contains("show") ||
                     inxp.Contains("table") ||
                     inxp.Contains("drop") ||
                     inxp.Contains("database") ||
                     inxp.Contains("describe") ||
                     inxp.Contains("select"))
                {

                    return "";
                }
                else
                {
                    if (inxp.Contains(";") || inxp.Contains("--") || inxp.Contains("'"))
                    {
                        return "";
                    }
                    else
                    {
                        return input;
                    }

                }
            }
            else
            {
                return "";

            }
        }
        //#region Surat Pendukung
        //public Boolean BulkSuratPendukung(string TblName, SuratPendukung Dt)
        //{
        //    var ireturn = false;
        //    try
        //    {
        //        var username = Dt.username;

        //        string paramAll = "";

        //        paramAll = Dt.menuid + "|" + Dt.flag + "|value";

        //        string paramDt = "";
        //        if (Dt.menuid == "30701000")
        //        {
        //            paramDt += username;
        //        }
        //        string nSQL = obj.getStrImpala(paramAll, paramDt);

        //        var myconnImpala = obj.myConnImpala();
        //        OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
        //        var dsTbl = obj.GetListImpala(Odbccmd);
        //        myconnImpala.Close();

        //        var myConn = obj.myConn();
        //        SqlCommand cmd = new SqlCommand("sp1_dropCreateTblTemp", myConn);
        //        cmd.Parameters.AddWithValue("@TblName", TblName);
        //        obj.Exec(myConn, cmd);

        //        //insert to SQL Server
        //        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
        //        {
        //            bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
        //            bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
        //            bulkCopy.DestinationTableName = $"[{TblName}]";
        //            try
        //            {
        //                bulkCopy.WriteToServer(dsTbl);
        //                ireturn = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }
        //        myConn.Close();
        //        return ireturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //#region persetujuan SP
        ////public Boolean SetPersetujuanSP(SuratPendukung Dt)
        ////{
        ////    var ireturn = false;
        ////    try
        ////    {
        ////        //var pathInf = cleansingStr(Dt.pk_portal[0].ToString());
        ////        //var recordtimestamp = cleansingStr(Dt.pk_portal[1].ToString());
        ////        var pathInf = cleansingStr(Dt.path_informasi);
        ////        var path_sp = cleansingStr(Dt.path_sp);
        ////        var recordtimestamp = cleansingStr(Dt.pk_portal[1].ToString());
        ////        var idpelapor = cleansingStr(Dt.IdPelapor);
        ////        var username = cleansingStr(Dt.username);
        ////        var istatus = cleansingStr(Dt.flag);
        ////        var dtime = DateTime.Now;

        ////        var urlToHit = db.GetConfig("api_url_sp_persetujuan");
        ////        var restclient = new RestClient(urlToHit);
        ////        RestRequest request = new RestRequest();
        ////        request.Method = Method.POST;
        ////        request.AddHeader("Content-Type", "application/json");
        ////        //request.AddParameter("undefined", "{\n    \"username\": \"" + username + "\",\n    \"idBank\": \"" + idpelapor + "\",\n    \"supporting_doc\": \"" + pathInf + "\",\n    \"recordtimestamp\": \"" + recordtimestamp + "\",\n    \"status\": \"" + status + "\"\n}", ParameterType.RequestBody);
        ////        request.AddParameter("undefined", "{\n    \"supporting_doc\": \"" + path_sp + "\",\n    \"username\": \"" + username + "\",\n    \"timestamp\": \"" + dtime + "\",\n    \"idBank\": \"" + idpelapor + "\",\n    \"source_files\": \"" + pathInf + "\",\n    \"recordtimestamp\": \"" + recordtimestamp + "\",\n    \"status\": \"" + istatus + "\"\n}", ParameterType.RequestBody);
        ////        //request.AddParameter("undefined", "{\n    \"supporting_doc\": \"" + namaFile + "\",\n    \"username\": \"" + username + "\",\n    \"timestamp\": \"" + jamfinishupload + "\",\n    \"idBank\": \"" + idpelapor + "\",\n    \"source_files\": \"" + pathInf + "\",\n    \"recordtimestamp\": \"" + recordtimestamp + "\"\n}", ParameterType.RequestBody);

        ////        var tResponse = restclient.Execute(request);

        ////        InsertLogJob_SP(JsonConvert.SerializeObject(request.Parameters), tResponse.Content, username);

        ////        var returnJObs = tResponse.Content;
        ////        if (!String.IsNullOrEmpty(returnJObs) && !returnJObs.ToLower().Contains("not ok"))
        ////        {
        ////            ireturn = true;
        ////            InsertUnggahSuratPendukung(path_sp, username, dtime, dtime.ToString(), pathInf, recordtimestamp, "S", istatus);//ini
        ////        }
        ////        else
        ////        {
        ////            InsertUnggahSuratPendukung(path_sp, username, dtime, dtime.ToString(), pathInf, recordtimestamp, "F", istatus);//ini
        ////        }

        ////        return ireturn;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return false;
        ////    }

        ////}
        ////public int InsertLogJob_SP(string request, string response, string username)
        ////{
        ////    response = (String.IsNullOrWhiteSpace(response)) ? "NoResponse" : response;
        ////    lDbConn myDB = new lDbConn();
        ////    SqlConnection myConn;
        ////    myConn = myDB.conStringSQL();
        ////    myConn.Open();
        ////    SqlCommand cmd = new SqlCommand("usp_hitJobLog_SP", myConn);
        ////    cmd.Parameters.Add("@request", SqlDbType.Text).Value = request;
        ////    cmd.Parameters.Add("@response", SqlDbType.Text).Value = response;
        ////    cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = username;
        ////    cmd.CommandType = CommandType.StoredProcedure;
        ////    int affectedrow = cmd.ExecuteNonQuery();
        ////    myConn.Close();
        ////    return affectedrow;
        ////}
        ////public int InsertUnggahSuratPendukung(string fileName, string userName, DateTime uploadDate, string jamfinish, string pathInformasi, string recordtimestamp, string FgStatus, string Status)
        ////{
        ////    lDbConn myDB = new lDbConn();
        ////    SqlConnection myConn;
        ////    myConn = myDB.conStringSQL();
        ////    myConn.Open();
        ////    SqlCommand cmd = new SqlCommand("usp_insertAllUnggahSuratPendukung", myConn);
        ////    cmd.Parameters.Add("@filename", SqlDbType.VarChar, 225).Value = fileName;
        ////    cmd.Parameters.Add("@username", SqlDbType.VarChar, 100).Value = userName;
        ////    cmd.Parameters.Add("@uploadDate", SqlDbType.DateTime).Value = uploadDate;
        ////    cmd.Parameters.Add("@uploadFinishDate", SqlDbType.VarChar, 225).Value = jamfinish;
        ////    cmd.Parameters.Add("@pathInformasi", SqlDbType.VarChar, 255).Value = pathInformasi;
        ////    cmd.Parameters.Add("@recordtimestamp", SqlDbType.VarChar, 100).Value = recordtimestamp;
        ////    cmd.Parameters.Add("@fgstatus", SqlDbType.VarChar).Value = FgStatus;
        ////    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 10).Value = Status;
        ////    cmd.CommandType = CommandType.StoredProcedure;
        ////    int affectedrow = cmd.ExecuteNonQuery();
        ////    myConn.Close();
        ////    return affectedrow;
        ////}
        //#endregion persetujuan SP

        //#endregion Surat Pendukung

        #region Denda
        public Boolean BulkDenda(string TblName, LapStatusPenyampaian Dt)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
                var pd3 = "";
                string a1 = null;
                string a2 = null;

                if (Dt.dateStart != null && Dt.dateEnd != null)
                {
                    //Get tanggal approval
                    var aw = Dt.dateStart.Split('-');
                    var ak = Dt.dateEnd.Split('-');
                    a1 = aw[2] + "-" + aw[1] + "-" + aw[0];
                    a2 = ak[2] + "-" + ak[1] + "-" + ak[0];
                }

                if (Dt.Periode == "Harian")
                {
                    iPeriode = "D";
                    var idate = Dt.Harian.Split('-');
                    pd2 = idate[2] + "-" + idate[1] + "-" + idate[0];
                    pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                }
                else if (Dt.Periode == "Mingguan")
                {
                    iPeriode = "W";

                    var MMinggu = Dt.Minggu; var MBulan = Dt.MBulan;

                    string bln = "00" + Dt.MBulan;

                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        pd2 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-" + obj.GetPeriodeMingguan(Dt.Minggu, Dt.MTahun, Dt.MBulan, "1");
                        pd2 = " strleft(" + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";

                        iPeriode = "W";
                    }
                    else
                    {
                        pd2 = "concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.MTahun + bln.Substring(bln.Length - 2, 2) + "'";

                        iPeriode = "W" + Dt.Minggu;
                    }
                }
                else if (Dt.Periode == "Bulanan")
                {
                    iPeriode = "M";
                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        if (Dt.BBulan == "12")
                        {
                            var thn = Convert.ToInt32(Dt.BTahun) + 1;
                            pd2 = thn.ToString() + "-01-01";
                        }
                        else
                        {
                            var bln0 = Convert.ToInt32(Dt.BBulan) + 1;
                            string bln = "00" + bln0;
                            pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                        }
                        var dt1 = DateTime.Parse(pd2).AddDays(-1);
                        pd2 = dt1.ToString("yyyy-MM-dd");
                        //JANGAN DIHAPUS UNTUK TESTING DI DEV!!
                        pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        //JANGAN DIHAPUS UNTUK TESTING DI DEV!!

                        //pd2 = " left(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                    }
                    else
                    {
                        string bln = "00" + Dt.BBulan;
                        pd2 = "concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.BTahun + bln.Substring(bln.Length - 2, 2) + "'";
                    }
                }
                else if (Dt.Periode == "Triwulan")
                {
                    iPeriode = "Q";
                    if (Dt.Triwulan == "1")
                    {
                        //iPeriode = "Q1";
                        pd1 = Dt.TTahun + "-01-01";
                        pd2 = Dt.TTahun + "-03-31";
                        var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                        pd3 = tmpThn.ToString() + "-12-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Triwulan == "2")
                    {
                        //iPeriode = "Q2";
                        pd1 = Dt.TTahun + "-04-01";
                        pd2 = Dt.TTahun + "-06-30";
                        pd3 = Dt.TTahun + "-03-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }

                    }
                    else if (Dt.Triwulan == "3")
                    {
                        //iPeriode = "Q3";
                        pd1 = Dt.TTahun + "-07-01";
                        pd2 = Dt.TTahun + "-09-30";
                        pd3 = Dt.TTahun + "-06-30";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");
                            pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                    else if (Dt.Triwulan == "4")
                    {
                        //iPeriode = "Q4";
                        pd1 = Dt.TTahun + "-10-01";
                        pd2 = Dt.TTahun + "-12-31";
                        pd3 = Dt.TTahun + "-09-30";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            //string bln = "00" + Dt.MBulan;
                            pd1 = pd1.Replace("-", "").Replace("'", "");
                            pd2 = pd2.Replace("-", "").Replace("'", "");
                            pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            //JANGAN DIHAPUS UNTUK TESTING DI DEV!!
                            pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                            //JANGAN DIHAPUS UNTUK TESTING DI DEV!!

                            //pd2 = " left(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                }
                else if (Dt.Periode == "Semesteran")
                {
                    iPeriode = "S";
                    if (Dt.Semester == "1")
                    {
                        //iPeriode = "S1";
                        pd1 = Dt.STahun + "-01-01";
                        pd2 = Dt.STahun + "-06-30";
                        var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                        pd3 = tmpThn.ToString() + "-12-31";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");

                            //Master no delete
                            pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";

                            //Testing local SQL
                            //pd2 = "FORMAT(DATEFROMPARTS(YEAR(a." + Dt.pdata_penyampaian + "), MONTH(a." + Dt.pdata_penyampaian + "), 1), 'yyyyMM') BETWEEN '" + pd1.Substring(0, 6) + "' AND '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            //Master no delete
                            pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";

                            //Testing local SQL
                            //pd2 = " LEFT(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }

                    }
                    else if (Dt.Semester == "2")
                    {
                        //iPeriode = "S2";
                        pd1 = Dt.STahun + "-06-30";
                        pd2 = Dt.STahun + "-12-31";
                        pd3 = Dt.STahun + "-01-01";
                        if (Dt.pdata_penyampaian != "periodedata")
                        {
                            pd1 = pd1.Replace("-", "");
                            pd2 = pd2.Replace("-", "");

                            //Master no delete
                            pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";

                            //Testing local SQL
                            //pd2 = "FORMAT(DATEFROMPARTS(YEAR(a." + Dt.pdata_penyampaian + "), MONTH(a." + Dt.pdata_penyampaian + "), 1), 'yyyyMM') BETWEEN '" + pd1.Substring(0, 6) + "' AND '" + pd2.Substring(0, 6) + "'";
                        }
                        else
                        {
                            //Master no delete
                            pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";

                            //Testing local SQL
                            //pd2 = " LEFT(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                        }
                    }
                }
                else if (Dt.Periode == "Tahunan")
                {
                    iPeriode = "A";
                    //iPeriode = "S1";
                    pd1 = Dt.TnTahun + "-01-01";
                    pd2 = Dt.TnTahun + "-12-31";
                    //var tmpThn = Convert.ToInt32(Dt.TTahun) - 1;
                    //pd3 = tmpThn.ToString() + "-12-31";
                    if (Dt.pdata_penyampaian != "periodedata")
                    {
                        pd1 = pd1.Replace("-", "");
                        pd2 = pd2.Replace("-", "");

                        //Master no delete
                        pd2 = "from_timestamp(concat(cast(year(a." + Dt.pdata_penyampaian + ")as string),'-',strright(concat('00',cast(month(a." + Dt.pdata_penyampaian + ")as string)),2),'-01'),'yyyyMM') between '" + pd1.Substring(0, 6) + "' and '" + pd2.Substring(0, 6) + "'";

                        //Testing local SQL
                        //pd2 = "FORMAT(DATEFROMPARTS(YEAR(a." + Dt.pdata_penyampaian + "), MONTH(a." + Dt.pdata_penyampaian + "), 1), 'yyyyMM') BETWEEN '" + pd1.Substring(0, 6) + "' AND '" + pd2.Substring(0, 6) + "'";
                    }
                    else
                    {
                        //Master no delete
                        pd2 = " strleft(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";

                        //Testing local SQL
                        //pd2 = " LEFT(a." + Dt.pdata_penyampaian + ",10) = '" + pd2 + "'";
                    }
                }

                ////From Impala
                var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                //var idInf = "'" + Dt.idinformation.Replace(",", "','") + "'";
                //var kelinf = "'" + Dt.KelInformasi.Replace(",", "','") + "'";
                var wilayahkerja = "'" + Dt.wilayahkerja.Replace(",", "','") + "'";
                var icakupan = ((Dt.Cakupan == "0") ? "gabungan" : "individual");

                string paramAll = Dt.menuid + "|" + Dt.fg + "|value";

                string paramDt = "";

                if (a1 != null && a2 != null)
                {
                    paramDt += icakupan + "|";
                    paramDt += idPlp + "|";
                    paramDt += Dt.KelInformasi + "|";
                    paramDt += wilayahkerja + "|";
                    paramDt += iPeriode + "|";
                    paramDt += pd2 + "|";

                    //Merge paramDt tanggal approval
                    paramDt += a1 + "|";
                    paramDt += a2 + "|";
                }
                else
                {
                    paramDt += icakupan + "|";
                    paramDt += idPlp + "|";
                    paramDt += Dt.KelInformasi + "|";
                    paramDt += wilayahkerja + "|";
                    paramDt += iPeriode + "|";
                    paramDt += pd2 + "|";
                }


                string nSQL = obj.getStrImpala(paramAll, paramDt);

                //GET DATA FROM IMPALA MASTER
                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();
                //GET DATA FROM IMPALA MASTER


                ////SQL
                //Dt.ReportQuery = "select 150000000000/1000000 as hasil,* from ZZRPTC_TEST01";
                //var myConn = obj.myConn();
                //SqlCommand cmd1 = new SqlCommand(nSQL, myConn);
                //SqlDataReader dr = cmd1.GetListSql(cmd1);

                ///GET DATA FROM SQL LOCAL
                //SqlCommand sqlcmd = new SqlCommand(nSQL, myConn);
                //var dsTbl = obj.GetListSql(sqlcmd);

                SqlCommand cmd = new SqlCommand(Dt.TblNameDtl + "dropCreateTblTemp", myConn);
                cmd.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmd);

                //SqlCommand cmdDtl = new SqlCommand("lsp_getTEST", myConn);
                //DataTable dsTbl = obj.GetList(myConn, cmdDtl);
                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    //bulkCopy.DestinationTableName = "dbo."+ TblName;
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {

                        obj.CreateLog("#GetBulkDenda#" + Dt.fg + "#failed#" + Dt.username + "#" + ex.Message);

                    }
                }
                myConn.Close();
                return ireturn;
            }
            catch (Exception ex)
            {
                obj.CreateLog("#GetBulkDenda2#" + Dt.fg + "#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        #endregion Denda

        #region Laporan Dinamis
        public JObject ValidasiDinamis(ReportGenerator Dt)
        {
            var myconnImpala = obj.myConnImpala();
            var JOreturn = new JObject();
            var ireturn = false;
            try
            {
                //Dt.ReportQuery = enc.decrypt(Dt.ReportQuery);
                Dt.ReportQuery = enc.Base64Decode(Dt.ReportQuery);

                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(Dt.ReportQuery, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                ireturn = true;//sql
            }
            catch (Exception ex)
            {
                ireturn = false;
                obj.CreateLog("#ValidasiDinamis#failed#" + Dt.username + "#" + ex.Message);

            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
            }

            JOreturn.Add("status_bulk", ireturn);

            return JOreturn;
        }

        public JObject SeacrhTbl(ReportGenerator Dt)
        {
            var JOreturn = new JObject();
            bool ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(Dt.ReportQuery, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                myconnImpala.Close();

                SqlCommand cmd = new SqlCommand("rptc_impalaTblIP", myConn);
                cmd.Parameters.AddWithValue("@flag", "del");
                cmd.Parameters.AddWithValue("@param", "");
                cmd.Parameters.AddWithValue("@by", Dt.username);
                obj.Exec(myConn, cmd);

                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    bulkCopy.BulkCopyTimeout = Convert.ToInt32(db.GetConfig("BulkCopyTimeout"));
                    bulkCopy.BatchSize = Convert.ToInt32(db.GetConfig("Bulk_BatchSize"));
                    bulkCopy.DestinationTableName = "dbo.MsRptTblIp";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {
                        ireturn = false;
                        obj.CreateLog("#SeacrhTbl#" + Dt.fg + "#failed#" + Dt.username + "#" + ex.Message);

                    }
                }
                myConn.Close();
            }
            catch (Exception ex)
            {
                //ireturn = true;//sql
                ireturn = false;//impala
                obj.CreateLog("#SeacrhTbl2#" + Dt.fg + "#failed#" + Dt.username + "#" + ex.Message);

            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }

            JOreturn.Add("status_bulk", ireturn);
            return JOreturn;
        }

        public JObject GetListDinamis(ReportGenerator Dt)
        {
            var JOreturn = new JObject();
            bool ireturn = false;
            var retObject = new List<dynamic>();
            var myconnImpala = obj.myConnImpala();
            try
            {
                if (Dt.fg == "2")
                {
                    //Dt.ReportQuery = enc.decrypt(Dt.ReportQuery);
                    Dt.ReportQuery = enc.Base64Decode(Dt.ReportQuery);
                }
                //Impala ODBC
                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(Dt.ReportQuery, myconnImpala);
                Odbccmd.CommandTimeout = Convert.ToInt32(db.GetConfig("CommandTimeout"));
                OdbcDataReader dr = Odbccmd.ExecuteReader();

                ////SQL
                //Dt.ReportQuery = "select 150000000000/1000000 as hasil,* from ZZRPTC_TEST01";
                //var myConn = obj.myConn();
                //SqlCommand cmd = new SqlCommand(Dt.ReportQuery, myConn);
                //SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    ireturn = true;
                }
                while (dr.Read())
                {
                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dataRow.Add(
                            dr.GetName(i),
                            dr.IsDBNull(i) ? null : dr[i].ToString() // use null instead of {}
                        );
                    }
                    retObject.Add((ExpandoObject)dataRow);

                }
                myconnImpala.Close();//impala
                //myConn.Close();//SQL
                ireturn = true;
            }
            catch (Exception ex)
            {
                ireturn = false;
                obj.CreateLog("#GetListDinamis#" + Dt.fg + "#failed#" + Dt.username + "#" + ex.Message);

                JOreturn.Add("message", ex.Message);
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
            }
            JOreturn.Add("status_bulk", ireturn);
            JOreturn.Add("data", lc.convertDynamicToJArray(retObject));
            return JOreturn;
        }

        #endregion Laporan Dinamis

        #region Data Balance

        public JObject ValidasiDataBalance(DataBalance Dt)
        {
            var myconnImpala = obj.myConnImpala();
            var JOreturn = new JObject();
            var ireturn = false;
            try
            {
                Dt.query = enc.Base64Decode(Dt.query);

                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(Dt.query, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);
                ireturn = true;//sql
            }
            catch (Exception ex)
            {
                ireturn = false;
                obj.CreateLog("#ValidasiDataBalance#failed#" + Dt.userName + "#" + ex.Message);

            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
            }

            JOreturn.Add("status_bulk", ireturn);

            return JOreturn;
        }

        public JObject GetListDataBalance(DataBalance Dt)
        {
            var JOreturn = new JObject();
            bool ireturn = false;
            var retObject = new List<dynamic>();
            var myconnImpala = obj.myConnImpala();
            try
            {
                //if (Dt.fg == "2")
                //{
                //Dt.ReportQuery = enc.decrypt(Dt.ReportQuery);
                Dt.query = enc.Base64Decode(Dt.query);
                //}
                //Impala ODBC
                myconnImpala.Open();
                OdbcCommand Odbccmd = new OdbcCommand(Dt.query, myconnImpala);
                Odbccmd.CommandTimeout = Convert.ToInt32(db.GetConfig("CommandTimeout"));
                OdbcDataReader dr = Odbccmd.ExecuteReader();

                ////SQL
                //Dt.ReportQuery = "select 150000000000/1000000 as hasil,* from ZZRPTC_TEST01";
                //var myConn = obj.myConn();
                //SqlCommand cmd = new SqlCommand(Dt.ReportQuery, myConn);
                //SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    ireturn = true;
                }
                while (dr.Read())
                {
                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dataRow.Add(
                            dr.GetName(i),
                            dr.IsDBNull(i) ? null : dr[i].ToString() // use null instead of {}
                        );
                    }
                    retObject.Add((ExpandoObject)dataRow);

                }
                myconnImpala.Close();//impala
                //myConn.Close();//SQL
                ireturn = true;
            }
            catch (Exception ex)
            {
                ireturn = false;
                obj.CreateLog("#GetListDataBalance#" + "#failed#" + Dt.userName + "#" + ex.Message);

                JOreturn.Add("message", ex.Message);
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
            }
            JOreturn.Add("status_bulk", ireturn);
            JOreturn.Add("data", lc.convertDynamicToJArray(retObject));
            return JOreturn;
        }

        public JObject GetIdPelaporAbsensiKelengkapan(string periodeData)
        {
            var JOreturn = new JObject();
            bool ireturn = false;
            var retObject = new List<dynamic>();
            var myconnPhoenix = obj.myConnPhoenix();
            try
            {
                var paramAll = "GetIdPelapor_Databalance|1|value";
                string paramDt = periodeData;
                string nSQL = obj.getStrImpala(paramAll, paramDt);
                //Phoenix ODBC
                myconnPhoenix.Open();
                OdbcCommand Odbccmd = new OdbcCommand(nSQL, myconnPhoenix);
                Odbccmd.CommandTimeout = Convert.ToInt32(db.GetConfig("CommandTimeout_phoenix"));
                OdbcDataReader dr = Odbccmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    ireturn = true;
                }
                while (dr.Read())
                {
                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dataRow.Add(
                            dr.GetName(i).ToLower(),
                            dr.IsDBNull(i) ? null : dr[i].ToString() // use null instead of {}
                        );
                    }
                    retObject.Add((ExpandoObject)dataRow);

                }
                myconnPhoenix.Close();//phoenix
                //myConn.Close();//SQL
                ireturn = true;
            }
            catch (Exception ex)
            {
                ireturn = false;
                obj.CreateLog("#GetIdPelaporByAbsensiKelengkapan#failed#" + ex.Message);

                JOreturn.Add("message", ex.Message);
            }
            finally
            {
                if (myconnPhoenix != null) myconnPhoenix.Close();
            }
            JOreturn.Add("status_bulk", ireturn);
            JOreturn.Add("data", lc.convertDynamicToJArray(retObject));
            return JOreturn;
        }


        #endregion

        #region Kelengkaapn Informasi
        public Boolean BulkKelengkapanInformasi(string TblName, KelengkapanInformasi Dt)
        {
            var ireturn = false;
            var myconnImpala = obj.myConnImpala();
            var myConn = obj.myConn();
            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
                var pd3 = "";

                iPeriode = "M";
                if (Dt.BBulan == "12")
                {
                    var thn = Convert.ToInt32(Dt.BTahun) + 1;
                    pd2 = thn.ToString() + "-01-01";
                }
                else
                {
                    var bln0 = Convert.ToInt32(Dt.BBulan) + 1;
                    string bln = "00" + bln0;
                    pd2 = Dt.BTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-01";
                }
                var dt1 = DateTime.Parse(pd2).AddDays(-1);
                pd2 = dt1.ToString("yyyy-MM-dd");

                ////From Impala
                var idPlp = "'" + Dt.idpelapor.Replace(",", "','") + "'";
                //var idInf = "'" + Dt.idinformation.Replace(",", "','") + "'";
                //var kelinf = "'" + Dt.kelinformasi.Replace(",", "','") + "'";
                //var wilker = "'" + Dt.wilayahkerja.Replace(",", "','") + "'";

                string paramAll = "";
                string paramDt = "";

                paramAll = Dt.menuid + "|1|sql_ZZ"; //50110000
                paramDt += Dt.jeniskegiatanoperasional + "|";
                paramDt += idPlp + "|";
                paramDt += iPeriode + "|";
                paramDt += Dt.kelinformasi + "|"; //kelinf + "|";
                paramDt += pd2 + "|";
                //paramDt += wilker;
                string isplit = ";";
                string p1 = "impala" + isplit + paramDt;
                //string p2 = "phoenix" + isplit + paramDt;
                ireturn = clone.generateTmpTbl(Dt.menuid, paramAll, paramDt, TblName, p1/*, p2*/);
                return ireturn;

            }
            catch (Exception ex)
            {
                obj.CreateLog("#BulkKelengkapanInformasi#failed#" + Dt.username + "#" + ex.Message);

                return false;
            }
            finally
            {
                if (myconnImpala != null) myconnImpala.Close();
                if (myConn != null) myConn.Close();
            }
        }

        # endregion Kelengkaapn Informasi
    }
}
