using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Antasena.Models;
using Antasena.Models.Libs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenHtmlToPdf;
using RestSharp;
using SelectPdf;

namespace Antasena.Controllers
{
    public class LapAbsensiController : Controller
    {
        // GET: DQMSetting
        //private IDQMSetting obj = new IDQMSetting();
        private IGlobal obj = new IGlobal();
        private lConvert objCon = new lConvert();
        private lhash lhash = new lhash();
        public string pUser = "";
        private string TblName = "";
        public string iMenuId = "50101000"; //tambahan untuk jaga trustee menu
        public int perpage = 100;

        public ActionResult Index()
        {
            //return View(); 
            try
            {
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    pUser = Session["username"].ToString();
                    var iCek = new MenuLeftClass().cekMac(pUser, iMenuId, "A");
                    if (iCek > 0)
                    {
                        ViewBag.MenuLeft = new MenuLeftClass().recursiveMenu(pUser, "M");
                        ViewBag.ButtonAcc = new MenuLeftClass().button(pUser, iMenuId, "B");
                        return View();
                    }
                    else
                    {
                        return Redirect("Home");
                    }

                }
                else
                {
                    return Redirect("Login");
                }
            }
            catch (Exception ex)
            {
                return Redirect("Login");

            }
        }
        private void pv_loadUser()
        {
            if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
            {
                pUser = Session["username"].ToString();
            }
        }

        public JObject getDD_JenisKegiatan(string cakupan)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var jret = new JObject();
            try
            {
                if (!iaccess || pUser == "")
                {
                    jret.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jret.Add("message", "Anda tidak memiliki akses");
                    return jret;
                }

                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    var myConn = obj.myConn();
                    SqlCommand cmd = new SqlCommand("la_getDD_JenisKegiatan", myConn);
                    //cmd.Parameters.AddWithValue("@groupid", Session["groupid"].ToString());
                    cmd.Parameters.AddWithValue("@username", pUser);
                    cmd.Parameters.AddWithValue("@cakupan", cakupan);
                    DataTable datasource = obj.GetList(myConn, cmd);
                    myConn.Close();
                    jret.Add("success", true);
                    jret.Add("data", objCon.DatatabletoJarray(datasource));
                }
            }
            catch (Exception ex)
            {
                jret.Add("success", false);
                jret.Add("message", ex.Message);
            }
            return jret;
        }
        public JObject getDD_SandiBank(string jnsKegiatan, string cakupan)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var jret = new JObject();
            try
            {
                if (!iaccess || pUser == "")
                {
                    jret.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jret.Add("message", "Anda tidak memiliki akses");
                    return jret;
                }

                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    {
                        var myConn = obj.myConn();
                        SqlCommand cmd = new SqlCommand("la_getDD_SandiBank", myConn);
                        //cmd.Parameters.AddWithValue("@groupid", Session["groupid"].ToString());
                        cmd.Parameters.AddWithValue("@username", pUser);
                        cmd.Parameters.AddWithValue("@jnsKegiatan", jnsKegiatan);
                        cmd.Parameters.AddWithValue("@cakupan", cakupan);
                        DataTable datasource = obj.GetList(myConn, cmd);
                        myConn.Close();
                        jret.Add("success", true);
                        jret.Add("data", objCon.DatatabletoJarray(datasource));
                    }
                }
            }
            catch (Exception ex)
            {
                jret.Add("success", false);
                jret.Add("message", ex.Message);
            }
            return jret;
        }

        public JObject getIdPelaporan(string sandiBank, string cakupan, string jnsKegiatan)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var jret = new JObject();
            try
            {
                pUser = Session["username"].ToString();
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    var myConn = obj.myConn();
                    SqlCommand cmd = new SqlCommand("la_getIdPelapor", myConn);
                    cmd.Parameters.AddWithValue("@username", pUser);
                    //cmd.Parameters.AddWithValue("@groupid", Session["groupid"].ToString());
                    cmd.Parameters.AddWithValue("@sandiBank", sandiBank);
                    cmd.Parameters.AddWithValue("@jnsKegiatan", jnsKegiatan);
                    cmd.Parameters.AddWithValue("@cakupan", cakupan);
                    DataTable datasource = obj.GetList(myConn, cmd);
                    myConn.Close();
                    jret.Add("success", true);
                    jret.Add("data", objCon.DatatabletoJarray(datasource));
                }
            }
            catch (Exception ex)
            {
                if (!iaccess || pUser == "")
                {
                    jret.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jret.Add("message", "Anda tidak memiliki akses");
                    return jret;
                }
                else
                {
                    jret.Add("success", false);
                    jret.Add("message", ex.Message);
                }

            }
            return jret;
        }

        public String getDD_WilayahKerja()
        {
            //if(session)
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                var jReturn = new JObject();
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                //var iSQL = " select distinct wilayahkerja,wilayahkerja wilayahkerjadesc from ip.dpp01 ";

                //var myconnImpala = obj.myConnImpala();
                //OdbcCommand Odbccmd = new OdbcCommand(iSQL, myconnImpala);
                //var datasource = obj.GetListImpala(Odbccmd);

                //jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                //jReturn.Add("data", getDDWilayahKerja());
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("la_getDDWilayahKerja", myConn);
                cmd.Parameters.AddWithValue("@username", pUser);
                DataTable datasource = obj.GetList(myConn, cmd);
                myConn.Close();
                jReturn.Add("success", true);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                return jReturn.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            //endif
        }

        public String getDD_StatusPLap()
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                var jReturn = new JObject();
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("la_getDD_StatusPLap", myConn);
                DataTable datasource = obj.GetList(myConn, cmd);
                myConn.Close();
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                return jReturn.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public String getKeterangan(string fungsi)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                var jReturn = new JObject();
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("la_getKeterangan", myConn);
                cmd.Parameters.AddWithValue("@fungsi", fungsi);
                DataTable datasource = obj.GetList(myConn, cmd);
                myConn.Close();
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                return jReturn.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string getStatusKL(string param)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var jReturn = new JObject();
            try
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("la_getStatusKL", myConn);
                cmd.Parameters.AddWithValue("@param", param);
                DataTable ds = obj.GetList(myConn, cmd);
                myConn.Close();

                return ds.Rows[0]["statuskl"].ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string strHtml = "";
        private string strHtml1 = "";
        private string strHtml2 = "";
        private string strHtml3 = "";
        private string strHtml4 = "";
        [HttpPost]
        public String getList(LapStatusPenyampaian Dt)
        {

            var jReturn = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var errMessage = "Something went wrong.";

            var iTbl = Guid.NewGuid().ToString();
            TblName = lhash.Base64Encode(pUser);
            TblName = "ZZLA_" + TblName;
            var flag = "A";
            //TblName = "ZZLA_Value01";//testing

            var iStatus = "";
            for (var st = 0; st < Dt.Status.Split(',').Count(); st++)
            {
                if (st == 0)
                {
                    iStatus += Dt.Status.Split(',')[st].Split(';')[1];
                }
                else
                {
                    iStatus += "," + Dt.Status.Split(',')[st].Split(';')[1];
                }
            }

            try
            {
                pUser = Session["username"].ToString();

                Dt.TblName = TblName;
                Dt.username = pUser;
                Dt.menuid = iMenuId;
                Dt.TblNameDtl = "";
                if (Dt.Periode != "Harian")
                {
                    Dt.TblNameDtl = "ZZLATT_" + lhash.Base64Encode(pUser);
                    if (CallAPIBulkCopy(Dt))
                    {
                        Dt.TblNameDtl = "";
                    }
                    else
                    {
                        strHtml = obj.emptyTable("Bank");
                        jReturn.Add("flag", false);
                        jReturn.Add("data", strHtml);
                        jReturn.Add("tbl", iTbl);
                        jReturn.Add("Error", errMessage);
                        return jReturn.ToString();
                    }
                }

                if (CallAPIBulkCopy(Dt))
                //if (true)
                {
                    var myConn = obj.myConn();
                    SqlCommand cmd = new SqlCommand("la_getDataHDR", myConn);
                    cmd.Parameters.AddWithValue("@flag", "H1");
                    cmd.Parameters.AddWithValue("@TblName", TblName);
                    cmd.Parameters.AddWithValue("@Status", iStatus);
                    DataTable dsHeader = obj.GetList(myConn, cmd);
                    myConn.Close();

                    //get header 0 column
                    //var idpelapor = "";
                    //var idpelapornew = "";
                    var Kelinf = "";
                    var Kelinfnew = "";
                    strHtml1 = strHtml1 + "<thead><tr>";
                    strHtml1 = strHtml1 + "<th rowspan='3' class='text-center'>Bank</th>";
                    strHtml2 = strHtml2 + "<tr>";
                    strHtml3 = strHtml3 + "<tr>";
                    strHtml4 = strHtml4 + "<tbody>";

                    var ihdr = dsHeader.AsEnumerable().ToList();
                    for (var h = 0; h < ihdr.Count(); h++)
                    {
                        Kelinf = dsHeader.Rows[h]["kelompokinformasi"].ToString();
                        var myConn1 = obj.myConn();
                        SqlCommand cmd1 = new SqlCommand("la_getDataHDR", myConn1);
                        cmd1.Parameters.AddWithValue("@flag", "H0");
                        cmd1.Parameters.AddWithValue("@TblName", TblName);
                        cmd1.Parameters.AddWithValue("@IdPelapor", "");
                        cmd1.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                        cmd1.Parameters.AddWithValue("@Status", iStatus);
                        DataTable dsHeader1 = obj.GetList(myConn1, cmd1);
                        myConn1.Close();

                        #region Header Kelompok Informasi
                        if (Kelinf != Kelinfnew)
                        {
                            strHtml1 = strHtml1 + "<th colspan='" + (Convert.ToInt32(dsHeader1.Rows[0]["countKI"].ToString()) * 2 + 2).ToString() + "' class='text-center'>" + Kelinf + "</th>";
                        }
                        #endregion

                        #region header nama informasi
                        if (Kelinf != Kelinfnew)
                        {
                            var myConn2 = obj.myConn();
                            SqlCommand cmd2 = new SqlCommand("la_getDataHDR", myConn2);
                            cmd2.Parameters.AddWithValue("@flag", "H2");
                            cmd2.Parameters.AddWithValue("@TblName", TblName);
                            cmd2.Parameters.AddWithValue("@IdPelapor", "");
                            cmd2.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                            cmd2.Parameters.AddWithValue("@Status", iStatus);
                            DataTable dsHeader2 = obj.GetList(myConn2, cmd2);
                            myConn1.Close();
                            var ihdr2 = dsHeader2.AsEnumerable().ToList();
                            for (var i = 0; i < ihdr2.Count(); i++)
                            {
                                //untuk export CSV
                                var p = "<p class='noExp' style='display:none'>" + Kelinf + " - ";
                                #region Header Status
                                if (i == 0)
                                {
                                    //strHtml2 = strHtml2 + "<th class='text-center' colspan='2'>Status KI</th>";
                                    //strHtml3 = strHtml3 + "<th class='text-center'><p style='display:none'>" + Kelinf + "-Status KI-" + "</p>Lapor</th>";//add 20190909
                                    //strHtml3 = strHtml3 + "<th class='text-center'><p style='display:none'>" + Kelinf + "-Status KI-" + "</p>Koreksi</th>";//add 20190909
                                    strHtml2 = strHtml2 + "<th class='text-center' colspan='2'>Status KI</th>";
                                    strHtml3 = strHtml3 + "<th class='text-center'>" + p + "Status KI - </p>Laporan</th>";//add 20190909
                                    strHtml3 = strHtml3 + "<th class='text-center'>" + p + "Status KI - </p>Koreksi</th>";//add 20190909
                                }
                                #endregion
                                //strHtml2 = strHtml2 + "<th class='text-center' colspan='2'>" + dsHeader2.Rows[i]["namainformasi"].ToString() + "</th>";
                                //strHtml3 = strHtml3 + "<th class='text-center'><p style='display:none'>" + Kelinf + "-" + dsHeader2.Rows[h]["namainformasi"].ToString() + "-</p>Lapor</th>";//add 20190909
                                //strHtml3 = strHtml3 + "<th class='text-center'><p style='display:none'>" + Kelinf + "-" + dsHeader2.Rows[h]["namainformasi"].ToString() + "-</p>Koreksi</th>";//add 20190909
                                strHtml2 = strHtml2 + "<th class='text-center' colspan='2'>" + dsHeader2.Rows[i]["namainformasi"].ToString() + "</th>";
                                strHtml3 = strHtml3 + "<th class='text-center'>" + p + dsHeader2.Rows[i]["namainformasi"].ToString() + " - </p>Lapor</th>";//add 20190909
                                strHtml3 = strHtml3 + "<th class='text-center'>" + p + dsHeader2.Rows[i]["namainformasi"].ToString() + " - </p>Koreksi</th>";//add 20190909

                            }
                        }
                        #endregion

                        Kelinfnew = dsHeader.Rows[h]["kelompokinformasi"].ToString();//klInf new
                    }

                    #region Value
                    var myConn3 = obj.myConn();
                    SqlCommand cmd3 = new SqlCommand("la_getData", myConn3);
                    SetParam(cmd3, Dt, TblName);
                    DataTable dsDetail = obj.GetList(myConn3, cmd3);
                    myConn3.Close();
                    var iDtl_ = dsDetail.AsEnumerable().ToList();
                    //var iDtl_5 = dsDetail.AsEnumerable().AsQueryable().Skip(starPage).Take(EndPage);
                    //var iDtl_ = iDtl_5.ToList();
                    for (var p = 0; p < iDtl_.Count(); p++)
                    {
                        strHtml4 = strHtml4 + "<tr>";
                        strHtml4 = strHtml4 + "<td class='text-left'>" + dsDetail.Rows[p]["Pelapor"].ToString() + "</td>";//pelapor

                        var vals = iDtl_[p].ItemArray;// ToString().Split('|');
                        for (var x = 1; x < vals.Count(); x++)
                        {
                            var iDtl = vals[x].ToString().Split('|');
                            var iDtl0 = iDtl[0].ToString().Split(';');

                            if (iDtl0.Count() > 1)
                            {
                                for (var v = 0; v < iDtl.Count(); v++)
                                {
                                    var iDtl2 = iDtl[v].ToString().Split(';');
                                    if (iDtl2[0] == "0")
                                    {
                                        strHtml4 = strHtml4 + "<td class='text-center'></td>";
                                    }
                                    else
                                    {
                                        strHtml4 = strHtml4 + "<td class='text-center'><button onclick = 'iconeClick(this)' id='" + iDtl[v].ToString() + "' class='btn btn-sm' style='background-color:" + iDtl2[3] + "'><span class='" + iDtl2[2] + "'></span></button><p style='display: none'>" + iDtl2[1] + "</p></td>";
                                    }
                                }
                            }
                            else
                            {
                                for (var c = 0; c < 2; c++)
                                {
                                    strHtml4 = strHtml4 + "<th class='text-center'>" + iDtl[c] + "</th>";//add 20190909
                                }
                            }
                        }
                        strHtml4 = strHtml4 + "</tr>";
                    }
                    #endregion value

                    strHtml1 = strHtml1 + "</tr>";
                    strHtml2 = strHtml2 + "</tr>";
                    strHtml3 = strHtml3 + "</tr>";//add 20190909
                    strHtml = strHtml1 + strHtml2 + strHtml3 + "</thead>";
                    strHtml4 = strHtml4 + "</tbody>";
                    strHtml = strHtml + strHtml4;

                    if (ihdr.Count() == 0)
                    {
                        strHtml = obj.emptyTable("Bank");
                        jReturn.Add("flag", false);
                        jReturn.Add("data", strHtml);
                        jReturn.Add("tbl", iTbl);
                        jReturn.Add("Error", "0 Record");
                    }
                    else
                    {
                        var myConn5 = obj.myConn();
                        SqlCommand cmd5 = new SqlCommand("la_getDataHDR", myConn5);
                        cmd5.Parameters.AddWithValue("@flag", "H3");
                        cmd5.Parameters.AddWithValue("@TblName", TblName);
                        cmd5.Parameters.AddWithValue("@Status", iStatus);
                        DataTable dsTblCount = obj.GetList(myConn5, cmd5);
                        myConn5.Close();

                        var dd = "";
                        var of = Convert.ToInt32(dsTblCount.Rows[0]["irows"].ToString());
                        var countpg = Convert.ToInt32(dsTblCount.Rows[0]["totalpage"].ToString());
                        for (int y = 0; y < countpg; y++)
                        {
                            var val = 0;
                            if (y == 0)
                            {
                                val = perpage;
                            }
                            else
                            {
                                val = perpage * (y + 1);
                            }
                            var label = y + 1;
                            dd += "<option value=" + val.ToString() + ">" + label.ToString() + "</option>";
                        }
                        int show = 0;
                        int to = 0;
                        if (of > perpage && Dt.page == perpage.ToString())
                        {
                            show = 1;
                            to = perpage;
                        }
                        else if (Convert.ToInt32(Dt.page) > perpage)
                        {
                            show = Convert.ToInt32(Dt.page) - (perpage - 1);
                            if (of < Convert.ToInt32(Dt.page))
                            {
                                to = of;
                            }
                            else
                            {
                                to = Convert.ToInt32(Dt.page);
                            }
                        }
                        else
                        {
                            show = of;
                            if (of > 0)
                            {
                                show = 1;
                            }
                            to = of;
                        }

                        jReturn.Add("flag", true);
                        jReturn.Add("show", show);
                        jReturn.Add("to", to);
                        jReturn.Add("of", of);
                        jReturn.Add("ipage", dd);
                        jReturn.Add("data", strHtml);
                        jReturn.Add("tbl", iTbl);
                    }
                    return jReturn.ToString();
                }
                else
                {
                    strHtml = obj.emptyTable("Bank");
                    jReturn.Add("flag", false);
                    jReturn.Add("data", strHtml);
                    jReturn.Add("tbl", iTbl);
                    jReturn.Add("Error", errMessage);
                    return jReturn.ToString();
                }
            }
            catch (Exception ex)
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                else
                {
                    strHtml = obj.emptyTable("Bank");
                    jReturn.Add("flag", false);
                    jReturn.Add("data", strHtml);
                    jReturn.Add("tbl", iTbl);
                    jReturn.Add("Error", ex.Message);
                    return jReturn.ToString();
                }

            }
        }


        [HttpPost]
        //public JObject DownloadPdf(LapStatusPenyampaian Dt)
        public ActionResult DownloadPdf(LapStatusPenyampaian Dt)
        {
            var jReturn = new JObject();
            pv_loadUser();
            //pUser = "heryusm@gmail.com";
            var iTbl = Guid.NewGuid().ToString();
            TblName = lhash.Base64Encode(pUser);
            TblName = "ZZLA_" + TblName;
            //TblName = "ZZLA_aGVyeXVzbUBnbWFpbC5jb20=";
            var flag = "A";
            JObject job = new JObject();
            try
            {
                var iStatus = "";
                for (var st = 0; st < Dt.Status.Split(',').Count(); st++)
                {
                    if (st == 0)
                    {
                        iStatus += Dt.Status.Split(',')[st].Split(';')[1];
                    }
                    else
                    {
                        iStatus += "," + Dt.Status.Split(',')[st].Split(';')[1];
                    }
                }
                //pUser = Session["username"].ToString();
                //var APi = cek_api(pUser, Dt.IdPelapor, Dt.idinformation, flag);
                //if (!APi)
                //{
                //    return new RedirectResult("/Login/timeout");
                //}

                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {


                    Dt.TblName = TblName;
                    Dt.username = pUser;
                    Dt.menuid = iMenuId;
                    if (CallAPIBulkCopy(Dt))

                    // if (bulkCopyList(TblName, Dt))
                    {
                        var myConn = obj.myConn();
                        SqlCommand cmd = new SqlCommand("la_getDataHDR", myConn);
                        cmd.Parameters.AddWithValue("@flag", "H1");
                        cmd.Parameters.AddWithValue("@TblName", TblName);
                        cmd.Parameters.AddWithValue("@Status", iStatus);
                        DataTable dsHeader = obj.GetList(myConn, cmd);
                        myConn.Close();

                        var Kelinf = "";
                        var Kelinfnew = "";

                        int maxColumn = 7;

                        strHtml1 = "";
                        strHtml2 = "";
                        strHtml3 = "";
                        strHtml4 = "";

                        var jaheader = new JArray();

                        var ihdr = dsHeader.AsEnumerable().ToList();
                        var ihdr2 = new List<DataRow>();
                        for (var h = 0; h < ihdr.Count(); h++) // count header
                        {
                            strHtml1 = ""; strHtml2 = ""; strHtml3 = "";


                            Kelinf = dsHeader.Rows[h]["kelompokinformasi"].ToString();
                            var myConn1 = obj.myConn();
                            SqlCommand cmd1 = new SqlCommand("la_getDataHDR", myConn1);
                            cmd1.Parameters.AddWithValue("@flag", "H0");
                            cmd1.Parameters.AddWithValue("@TblName", TblName);
                            cmd1.Parameters.AddWithValue("@IdPelapor", "");
                            cmd1.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                            cmd1.Parameters.AddWithValue("@Status", iStatus);
                            DataTable dsHeader1 = obj.GetList(myConn1, cmd1);
                            myConn1.Close();



                            var myConn2 = obj.myConn();
                            SqlCommand cmd2 = new SqlCommand("la_getDataHDR", myConn2);
                            cmd2.Parameters.AddWithValue("@flag", "H2");
                            cmd2.Parameters.AddWithValue("@TblName", TblName);
                            cmd2.Parameters.AddWithValue("@IdPelapor", "");
                            cmd2.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                            cmd2.Parameters.AddWithValue("@Status", iStatus);
                            DataTable dsHeader2 = obj.GetList(myConn2, cmd2);
                            myConn1.Close();

                            var xcount = 0; var zzz = 0; var countsisa = 0;
                            DataRow newRow = dsHeader2.NewRow();
                            newRow["IdInformasi"] = "Status KI";
                            newRow["namainformasi"] = "Status KI";
                            dsHeader2.Rows.InsertAt(newRow, 0);
                            DataRow a = dsHeader2.NewRow();
                            a["IdInformasi"] = "Bank";
                            a["namainformasi"] = "Bank";
                            dsHeader2.Rows.InsertAt(a, 0);

                            ihdr2 = dsHeader2.AsEnumerable().ToList();
                            for (var i = 0; i < ihdr2.Count; i++)
                            {
                                #region Header Status
                                if (i == 0)
                                {
                                    // strHtml1 = "<th rowspan='3' class='text-center'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff;width:30%;'>Bank</th><th  style='border: 1px solid black;background-color:#2d4154;color:#ffffff' colspan='" + ((maxColumn + 1) * 2 + 2) + "' class='text-center'>" + Kelinf + "</th>";
                                    strHtml1 = "<th rowspan='3' class='text-center'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff;width:30%;'>Bank</th><th  style='background-color:#2d4154;color:#ffffff' colspan='" + ((maxColumn + 1) * 2 + 2) + "' class='text-center'>" + Kelinf + "</th>";

                                }
                                else
                                {
                                    //strHtml2 = strHtml2 + "<th class='text-center' colspan='2' style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff'>" + dsHeader2.Rows[i]["namainformasi"].ToString() + "</th>";
                                    //strHtml3 = strHtml3 + "<th class='text-center'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff'>Lapor</th>";//add 20190909
                                    //strHtml3 = strHtml3 + "<th class='text-center'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff'>Koreksi</th>";//add 20190909
                                    strHtml2 = strHtml2 + "<th class='text-center' colspan='2' style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000; background-color:#2d4154;color:#ffffff'>" + dsHeader2.Rows[i]["namainformasi"].ToString() + "</th>";
                                    strHtml3 = strHtml3 + "<th class='text-center'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff'>Lapor</th>";//add 20190909
                                    strHtml3 = strHtml3 + "<th class='text-center'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff'>Koreksi</th>";//add 20190909

                                }
                                #endregion

                                zzz = xcount % maxColumn;
                                if ((i != 0) && i % maxColumn == 0)
                                {
                                    var jheader1 = new JObject();
                                    if (Kelinf != Kelinfnew)
                                    {
                                        //strHtml1 = "<th rowspan='3' class='text-center'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff;width:30%;'>Bank</th><th  style='border: 1px solid black;background-color:#2d4154;color:#ffffff' colspan='" + ((maxColumn + 1) * 2 + 2) + "' class='text-center'>" + Kelinf + "</th>";
                                        strHtml1 = "<th rowspan='3' class='text-center'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff;width:30%;'>Bank</th><th  style='border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff' colspan='" + ((maxColumn + 1) * 2 + 2) + "' class='text-center'>" + Kelinf + "</th>";

                                    }

                                    jheader1.Add("header1", HttpUtility.HtmlEncode(strHtml1));
                                    jheader1.Add("header2", HttpUtility.HtmlEncode(strHtml2));
                                    jheader1.Add("header3", HttpUtility.HtmlEncode(strHtml3));

                                    countsisa += maxColumn;
                                    strHtml1 = ""; strHtml2 = ""; strHtml3 = "";
                                    jaheader.Add(jheader1);

                                }
                                xcount++;
                            }

                            countsisa += 1;
                            for (var i = countsisa; i < ihdr2.Count; i++)
                            {
                                var jheader1 = new JObject();
                                if (i == countsisa)
                                {
                                    strHtml1 = ""; strHtml2 = ""; strHtml3 = "";
                                    // strHtml1 = "<th rowspan='3' class='text-center'   style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff;width:30%;'>Bank</th><th  style='border: 1px solid black;background-color:#2d4154;color:#ffffff' colspan='" + ((zzz + 1) * 2 + 2) + "' class='text-center'>" + Kelinf + "</th>";
                                    strHtml1 = "<th rowspan='3' class='text-center'   style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff;width:30%;'>Bank</th><th  style='border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff' colspan='" + ((zzz + 1) * 2 + 2) + "' class='text-center'>" + Kelinf + "</th>";


                                }
                                //strHtml2 = strHtml2 + "<th class='text-center' colspan='2'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff'>" + dsHeader2.Rows[i]["namainformasi"].ToString() + "</th>";
                                //strHtml3 = strHtml3 + "<th class='text-center'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff'>Lapor</th>";//add 20190909
                                //strHtml3 = strHtml3 + "<th class='text-center'  style='border: 1px solid black; padding: 10px;background-color:#2d4154;color:#ffffff'>Koreksi</th>";//add 20190909

                                strHtml2 = strHtml2 + "<th class='text-center' colspan='2'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff'>" + dsHeader2.Rows[i]["namainformasi"].ToString() + "</th>";
                                strHtml3 = strHtml3 + "<th class='text-center'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff'>Laporan</th>";//add 20190909
                                strHtml3 = strHtml3 + "<th class='text-center'  style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:#2d4154;color:#ffffff'>Koreksi</th>";//add 20190909


                                if (i == (ihdr2.Count - 1))
                                {
                                    jheader1.Add("header1", HttpUtility.HtmlEncode(strHtml1));
                                    jheader1.Add("header2", HttpUtility.HtmlEncode(strHtml2));
                                    jheader1.Add("header3", HttpUtility.HtmlEncode(strHtml3));

                                    jaheader.Add(jheader1);
                                    strHtml1 = ""; strHtml2 = ""; strHtml3 = "";
                                }
                                xcount++;

                            }
                            Kelinfnew = dsHeader.Rows[h]["kelompokinformasi"].ToString();//klInf new
                        }
                        var KelinfC = "";
                        var jarKelompok = new JArray();
                        for (var h = 0; h < ihdr.Count(); h++) // count header
                        {
                            var jacontent = new JArray();
                            strHtml4 = "";
                            KelinfC = dsHeader.Rows[h]["kelompokinformasi"].ToString();
                            var myConn3 = obj.myConn();
                            Dt.KelInformasi = KelinfC;
                            SqlCommand cmd3 = new SqlCommand("la_exportdataPDF", myConn3);
                            SetParam(cmd3, Dt, TblName);
                            DataTable dsDetail = obj.GetList(myConn3, cmd3);
                            myConn3.Close();
                            var iDtl_ = dsDetail.AsEnumerable().ToList();
                            var xcount = 0; var zzz = 0; var countsisa = 0;


                            for (var i = 0; i < iDtl_.Count; i++)
                            {
                                countsisa = 0;
                                strHtml4 = "";
                                var jae = new JArray();
                                var vals = iDtl_[i].ItemArray;
                                for (var x = 0; x < vals.Count(); x++)
                                {

                                    if (x == 0 || (x >= 2 && x % maxColumn == 1))
                                    {
                                        //strHtml4 = "<td style='border: 1px solid black; padding: 10px;' class='text-left'>" + dsDetail.Rows[i]["Pelapor"].ToString() + "</td>";
                                        strHtml4 = "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-left'>" + dsDetail.Rows[i]["Pelapor"].ToString() + "</td>";


                                    }
                                    else
                                    {
                                        //if (i == 0)
                                        //{
                                        //    var iDtl = vals[x].ToString().Split('|');
                                        //    var iDtl0 = iDtl[0].ToString().Split(';');

                                        //    if (iDtl0.Count() > 1)
                                        //    {
                                        //        for (var v = 0; v < iDtl.Count(); v++)
                                        //        {
                                        //            var iDtl2 = iDtl[v].ToString().Split(';');
                                        //            if (iDtl2[0] == "0")
                                        //            {
                                        //                strHtml4 = strHtml4 + "<td class='text-center'></td>";
                                        //            }
                                        //            else
                                        //            {
                                        //                strHtml4 = strHtml4 + "<td class='text-center' style='background-color:" + iDtl2[3] + "' align='center'>" + iDtl2[1] + "</td>";
                                        //                //strHtml4 = strHtml4 + "<td class='text-center'>" + iDtl0[7] + "</td>";
                                        //            }
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        if (iDtl.Count() > 1)
                                        //        {
                                        //            for (var c = 0; c < 2; c++)
                                        //            {
                                        //                strHtml4 = strHtml4 + "<td class='text-center'>" + iDtl[c] + "</td>";//add 20190909
                                        //            }
                                        //        }


                                        //    }
                                        //}
                                        //else {
                                        var iDtl = vals[x - 1].ToString().Split('|');
                                        var iDtl0 = iDtl[0].ToString().Split(';');

                                        if (iDtl0.Count() > 1)
                                        {
                                            for (var v = 0; v < iDtl.Count(); v++)
                                            {
                                                var iDtl2 = iDtl[v].ToString().Split(';');
                                                if (iDtl2[0] == "0")
                                                {
                                                    //strHtml4 = strHtml4 + "<td style='border: 1px solid black; padding: 10px;' class='text-center'></td>";
                                                    strHtml4 = strHtml4 + "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-center'></td>";
                                                }
                                                else
                                                {
                                                    //strHtml4 = strHtml4 + "<td class='text-center' style=' padding: 10px;border: 1px solid black;background-color:" + iDtl2[3] + "' align='center'>" + iDtl0[1] + "</td>";
                                                    //strHtml4 = strHtml4 + "<td class='text-center'>" + iDtl0[7] + "</td>";
                                                    strHtml4 = strHtml4 + "<td class='text-center' style='page-break-inside: avoid;border-right:1px solid #000000;border-bottom:1px solid #000000;background-color:" + iDtl2[3] + ";' align='center'>" + iDtl0[1] + "</td>";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (iDtl.Count() > 1)
                                            {
                                                for (var c = 0; c < 2; c++)
                                                {
                                                    //strHtml4 = strHtml4 + "<td style='border: 1px solid black; padding: 10px;' class='text-center'>" + iDtl[c] + "</td>";//add 20190909
                                                    strHtml4 = strHtml4 + "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-center'>" + iDtl[c] + "</td>";//add 20190909

                                                }
                                            }


                                        }

                                        //}

                                    }

                                    zzz = xcount % maxColumn;
                                    if ((x != 0) && x % maxColumn == 0)
                                    {

                                        var iDtl = vals[x].ToString().Split('|');
                                        var iDtl0 = iDtl[0].ToString().Split(';');

                                        if (iDtl0.Count() > 1)
                                        {
                                            for (var v = 0; v < iDtl.Count(); v++)
                                            {
                                                var iDtl2 = iDtl[v].ToString().Split(';');
                                                if (iDtl2[0] == "0")
                                                {
                                                    // strHtml4 = strHtml4 + "<td style='border: 1px solid black; padding: 10px;' class='text-center'></td>";
                                                    strHtml4 = strHtml4 + "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-center'></td>";
                                                }
                                                else
                                                {
                                                    //strHtml4 = strHtml4 + "<td class='text-center' style=' padding: 10px;border: 1px solid black;background-color:" + iDtl2[3] + "' align='center'>" + iDtl0[1] + "</td>";
                                                    //strHtml4 = strHtml4 + "<td class='text-center'>" + iDtl0[7] + "</td>";
                                                    strHtml4 = strHtml4 + "<td class='text-center' style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:" + iDtl2[3] + ";' align='center'>" + iDtl0[1] + "</td>";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (iDtl.Count() > 1)
                                            {
                                                for (var c = 0; c < 2; c++)
                                                {
                                                    //strHtml4 = strHtml4 + "<td style='border: 1px solid black; padding: 10px;' class='text-center'>" + iDtl[c] + "</td>";//add 20190909
                                                    strHtml4 = strHtml4 + "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-center'>" + iDtl[c] + "</td>";//add 20190909

                                                }
                                            }


                                        }
                                        var jcon = new JObject();
                                        jcon.Add("content", HttpUtility.HtmlEncode(strHtml4));
                                        jcon.Add("table", i + "-" + x);
                                        jae.Add(jcon);
                                        strHtml4 = "";
                                        countsisa += maxColumn;

                                    }
                                    xcount++;

                                }
                                for (var x = countsisa; x < vals.Length; x++)
                                {
                                    if (x == countsisa)
                                    {
                                        strHtml4 = "";
                                        // strHtml4 = strHtml4 + "<td style='border: 1px solid black; padding: 10px;' class='text-left'>" + dsDetail.Rows[i]["Pelapor"].ToString() + "</td>";//pelapor
                                        strHtml4 = strHtml4 + "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-left'>" + dsDetail.Rows[i]["Pelapor"].ToString() + "</td>";//pelapor

                                    }
                                    else
                                    {

                                        var iDtl = vals[x].ToString().Split('|');
                                        var iDtl0 = iDtl[0].ToString().Split(';');

                                        if (iDtl0.Count() > 1)
                                        {
                                            for (var v = 0; v < iDtl.Count(); v++)
                                            {
                                                var iDtl2 = iDtl[v].ToString().Split(';');
                                                if (iDtl2[0] == "0")
                                                {
                                                    //strHtml4 = strHtml4 + "<td style='border: 1px solid black; padding: 10px;' class='text-center'></td>";
                                                    strHtml4 = strHtml4 + "<td style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-center'></td>";
                                                }
                                                else
                                                {
                                                    //strHtml4 = strHtml4 + "<td class='text-center'><button onclick = 'iconeClick(this)' id='" + iDtl[v].ToString() + "' class='btn btn-sm' style='background-color:" + iDtl2[3] + "'><span class='" + iDtl2[2] + "'></span></button><p style='display: none'>" + iDtl2[1] + "</p></td>";
                                                    //strHtml4 = strHtml4 + "<td  class='text-center' style='border: 1px solid black; padding: 10px;background-color:" + iDtl2[3] + "' align='center'>" + iDtl2[1] + "</td>";
                                                    strHtml4 = strHtml4 + "<td  class='text-center' style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;background-color:" + iDtl2[3] + ";' align='center'>" + iDtl2[1] + "</td>";


                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (iDtl.Count() > 1)
                                            {
                                                for (var c = 0; c < 2; c++)
                                                {
                                                    //strHtml4 = strHtml4 + "<th style='border: 1px solid black; padding: 10px;' class='text-center'>" + iDtl[c] + "</th>";//add 20190909
                                                    strHtml4 = strHtml4 + "<th style='page-break-inside: avoid;border-bottom:1px solid #000000;border-right:1px solid #000000;' class='text-center'>" + iDtl[c] + "</th>";//add 20190909

                                                }
                                            }


                                        }

                                    }

                                    if (x == (vals.Length - 1))
                                    {
                                        var jcon = new JObject();
                                        jcon.Add("content", HttpUtility.HtmlEncode(strHtml4));
                                        jcon.Add("table", i + "-" + x);
                                        jae.Add(jcon);
                                        strHtml4 = "";
                                        //countsisa += 7;
                                    }
                                }


                                var jobject = new JObject();
                                jobject.Add("row", i);
                                jobject.Add("data", jae);
                                jacontent.Add(jobject); ;
                            }
                            var jac = new JObject();
                            jac.Add("kelinf", KelinfC);
                            jac.Add("data", jacontent);
                            //jacontent.Add(jac); ;

                            jarKelompok.Add(jac);
                        }


                        var jmain = new List<dynamic>();

                        foreach (JObject joitem in jarKelompok) //looping kelompok informasi r
                        {
                            JArray jokel = JArray.Parse(joitem.GetValue("data").ToString()); //row per kelompak infromasi
                            var tmp = new List<dynamic>();
                            foreach (JObject jatable in jokel)
                            { //Looping  row

                                JArray data = JArray.Parse(jatable.GetValue("data").ToString());

                                int xc = 0;

                                foreach (JObject tbl in data)
                                { //looping column

                                    try
                                    {
                                        string content = (string)tmp[xc];

                                        //tmp[xc] = content + "<tr style='page-break-inside: avoid;border: 1px solid black;'>" + tbl.GetValue("content") + "</tr>";
                                        tmp[xc] = content + "<tr>" + tbl.GetValue("content") + "</tr>";

                                    }
                                    catch (Exception)
                                    {
                                        //tmp.Add("<tr style='page-break-inside: avoid;border: 1px solid black;'>" + tbl.GetValue("content") + "</tr>");
                                        tmp.Add("<tr>" + tbl.GetValue("content") + "</tr>");
                                    }

                                    xc++;
                                }

                            }
                            jmain.Add(tmp);
                        }


                        //var ht = "<!DOCTYPE html><html>";
                        //ht += "<head>";
                        //ht += "<meta charset='utf-8' /><title>Laporan Absensi</title>";
                        //ht += "<style>table, th, td {border: 1px solid black}</style>";
                        //ht += "<link href=\"/Content/CSS/style.css\" rel=\"stylesheet\"/>";
                        //ht += "</head>";
                        //ht += "<body>";
                        //ht += "<h1 style='text-align: center;'>Laporan Absensi</h1>";
                        //ht += "<div style='margin-bottom:0px;'>";
                        //ht += "{body}";
                        //ht += "</div>";
                        //ht += "</body>";
                        //ht += "</html>";

                        var ht = "{body}";
                        var jh = jaheader.ToArray();
                        var jm = jmain.ToArray();
                        var hd = 0;
                        //var tblz = "<h1 style=' text-align: center;'>Laporan Absensi</h1>";
                        var tblz = "";
                        //var jtb = new JArray();
                        for (int o = 0; o < jm.Length; o++)
                        {
                            hd = 0;
                            var ln = jm[o]; ;
                            foreach (var x in ln)
                            {

                                tblz += "<table style='border:1px solid #000000;width:100%;border-spacing: 0; border-collapse: separate;margin-top:20px;' >";
                                //tblz += "<table style='page-break-inside: avoid; border:1px solid #000000; width:100%;border-spacing: 0; border-collapse: separate;' >";

                                tblz += "<thead style='display: table-header-group;'>";
                                tblz += "<tr>" + HttpUtility.HtmlDecode(jh[hd]["header1"].ToString()) + "</tr>";
                                tblz += "<tr>" + HttpUtility.HtmlDecode(jh[hd]["header2"].ToString()) + "</tr>";
                                tblz += "<tr>" + HttpUtility.HtmlDecode(jh[hd]["header3"].ToString()) + "</tr>";
                                tblz += "</thead>";
                                tblz += "<tbody>";
                                tblz += HttpUtility.HtmlDecode(x);
                                tblz += "</tbody>";
                                tblz += "</table>";

                                //jtb.Add(tblz);

                                hd++;

                            }
                        }


                        HtmlToPdf converter = new HtmlToPdf();

                        converter.Options.PdfPageSize = PdfPageSize.Legal;
                        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                        //converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                        converter.Options.MarginLeft = 20;
                        converter.Options.MarginRight = 20;
                        converter.Options.MarginTop = 30;
                        converter.Options.MarginBottom = 30;

                        SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(ht.Replace("{body}", tblz));
                        doc1.Append(doc1);
                        byte[] pdf = doc1.Save();

                        doc1.Close();


                        FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                        fileResult.FileDownloadName = "Laporan Absensi.pdf";
                        return fileResult;


                        //byte[] pdfBytes = new PdfConverter().GetPdfBytesFromHtmlString(tblz);


                        //FileResult fileResult = new FileContentResult(pdfBytes, "application/pdf");
                        //fileResult.FileDownloadName = "Laporan_absensi.pdf";
                        //return fileResult;

                        //job.Add("success", true);
                        //job.Add("content", jtb);
                        //return job;



                    }
                    else
                    {
                        //job.Add("success", false);
                        //job.Add("content", "");
                        //return job;
                        return new RedirectResult("/Logout/timout");
                    }
                }
                else
                {
                    //job.Add("success", false);
                    //job.Add("content", "");
                    //return job;
                    return new RedirectResult("/Logout/timout");
                }
            }
            catch (Exception ex)
            {
                //job.Add("success", false);
                //job.Add("content", ex.Message);
                //return job;
                return new RedirectResult("/Logout/timout");
            }
        }


        private JArray getDDWilayahKerja()
        {
            try
            {
                IGlobal ig = new IGlobal();
                var url = ig.getUrlInternalAPI("wilayah_kerja_bi").ToString();

                var restclient = obj.RestClient(url);
                //var restclient = new RestClient(url);
                //RestRequest request = new RestRequest();
                RestRequest request = obj.RestRequest();
                request.Method = Method.GET;
                var tResponse = restclient.Execute(request);
                var responseJson = tResponse.Content;
                var job = JObject.Parse(responseJson);

                var jab = JArray.Parse(job.GetValue("result").ToString());

                return jab;
            }
            catch (Exception)
            {
                return new JArray();
            }

        }

        private Boolean CallAPIBulkCopy(LapStatusPenyampaian dt)
        {
            try
            {
                IGlobal ig = new IGlobal();
                var url = ig.getUrlInternalAPI("laporan_absensi").ToString();
                string approotpath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                string usingcert = System.Configuration.ConfigurationManager.AppSettings["APIUsingCertificate"];
                string cert = System.Configuration.ConfigurationManager.AppSettings["APIClientCertificate"];
                string pwd = System.Configuration.ConfigurationManager.AppSettings["APIClientCertificatePassword"];

                var restclient = obj.RestClient(url);
                //var restclient = new RestClient(url);
                if (bool.Parse(usingcert) == true)
                {
                    string certFile = Path.Combine(approotpath, cert);
                    X509Certificate2 certificates = new X509Certificate2(certFile, pwd);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    restclient.ClientCertificates = new X509CertificateCollection() { certificates };
                }
                RestRequest request = obj.RestRequest();
                //RestRequest request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Content-Type", "application/json");
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(JsonConvert.SerializeObject(dt));
                var tResponse = restclient.Execute(request);
                var responseJson = tResponse.Content;

                var job = JObject.Parse(responseJson);
                var jor = false;
                if (job["status"].ToString().Equals("Success"))
                {
                    if (bool.Parse(job["result"].ToString()) == true)
                    {
                        jor = true;
                    }
                }
                return jor;

            }
            catch (Exception e)
            {
                return false;
            }

        }


        public bool bulkCopyList(string TblName, LapStatusPenyampaian Dt)
        {
            pv_loadUser();
            var ireturn = false;
            try
            {
                var iPeriode = "";
                var pd1 = "";
                var pd2 = "";
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
                    if (MMinggu == "4")
                    {
                        MBulan = (Convert.ToInt32(MBulan) + 1).ToString();
                        if (MBulan == "13") MBulan = "1";
                    }

                    //string bln = "00" + Dt.MBulan;
                    string bln = "00" + MBulan;

                    if (Dt.pdata_penyampaian == "periodedata")
                    {
                        //pd1 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + GetPeriodeMingguan(Dt.MBulan, "1");
                        pd2 = Dt.MTahun + "-" + bln.Substring(bln.Length - 2, 2) + "-" + GetPeriodeMingguan(Dt.Minggu, "1");
                        var dt1 = DateTime.Parse(pd2).AddDays(-1);
                        pd2 = dt1.ToString("yyyy-MM-dd");
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
                        pd2 = "concat(cast(year(" + Dt.pdata_penyampaian + ")as string),strright(concat('00',cast(month(" + Dt.pdata_penyampaian + ")as string)),2)) = '" + Dt.MTahun + bln.Substring(bln.Length - 2, 2) + "'";
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

                var iSQL =
                   " select b.IdPelapor, b.versioncode, b.Nama, b.KantorCabang, " +
                   " b.PeriodeLaporan, b.kelompokinformasi, b.IdInformasi, b.namainformasi, " +
                   " b.PeriodeData, b.laporanstatus, b.koreksistatus, b.laporantimestamp, b.koreksitimestamp " +
                   " from (" +
                   " select " +
                   " idpelapor as IdPelapor " +
                   " ,versioncode as versioncode " +
                   " ,namabank as Nama " +
                   " ,'kantorcabang' as KantorCabang " +
                   " ,periodelaporan as PeriodeLaporan " +
                   " ,kelompokinformasi as kelompokinformasi " +
                   " ,idinformasi as IdInformasi  " +
                   " ,informasi as namainformasi " +
                   " ,periodedata as PeriodeData " +
                   " ,laporanstatus as laporanstatus " +
                   " ,koreksistatus as koreksistatus " +
                   " ,laporantimestamp as laporantimestamp " +
                   " ,koreksitimestamp as koreksitimestamp " +
                   " from (" +
                   " select *,rank() over (partition by strleft(periodelaporan,1),IdPelapor,versioncode order by recordtimestamp_ desc) as rank " +
                   " from (select case when isnull(lower(recordtimestamp),'0')='null' then '0' else isnull(recordtimestamp,'0') end as recordtimestamp_,* " +
                   " from  ip_rpt.view_final_absensi ) as z " +
                   //" from  ip_rpt.view_absensi " +
                   //" where periodelaporan='{0}' " +
                   " where periodelaporan like '{0}%' " +
                   " and {1} " +
                   " and kelompokinformasi in ({2}) " +
                   //" and (lower(laporanstatus) = lower('{3}') or lower(koreksistatus)=lower('{4}') ) " +
                   " and idpelapor in ({5}) " +
                   //" and idinformasi in ({6}) " +
                   " and wilayahkerja in ({6}) " +
                   " and lower(cakupan) ='{7}' " +
                   " ) as a" +
                   " where a.rank=1) as b " +
                   " where (lower(b.laporanstatus) in ({3}) or lower(b.koreksistatus) in ({4}) ) ";
                iSQL = string.Format(iSQL, iPeriode, pd2, kelinf, iStatus, iStatus, idPlp, wilayahkerja, icakupan);
                //iSQL = string.Format(iSQL, iPeriode, pd2, kelinf, iStatus, iStatus, idPlp, idInf, Dt.wilayahkerja, Dt.pdata_penyampaian);

                var myconnImpala = obj.myConnImpala();
                OdbcCommand Odbccmd = new OdbcCommand(iSQL, myconnImpala);
                var dsTbl = obj.GetListImpala(Odbccmd);

                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("la_dropCreateTblTemp", myConn);
                cmd.Parameters.AddWithValue("@TblName", TblName);
                obj.Exec(myConn, cmd);

                //SqlCommand cmdDtl = new SqlCommand("lsp_getTEST", myConn);
                //DataTable dsTbl = obj.GetList(myConn, cmdDtl);
                //insert to SQL Server
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(myConn))
                {
                    //bulkCopy.DestinationTableName = "dbo."+ TblName;
                    bulkCopy.DestinationTableName = $"[{TblName}]";

                    try
                    {
                        bulkCopy.WriteToServer(dsTbl);
                        ireturn = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                myConn.Close();
                return ireturn;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetPeriodeMingguan(string minggu, string pd)
        {
            pv_loadUser();
            var ireturn = "";
            var myConn = obj.myConn();
            SqlCommand cmd = new SqlCommand("la_GetPeriodeMingguan", myConn);
            cmd.Parameters.AddWithValue("@minggu", minggu);
            //cmd.Parameters.AddWithValue("@param", minggu);
            DataTable ds = obj.GetList(myConn, cmd);
            myConn.Close();

            if (pd == "1")
            {
                ireturn = ds.Rows[0]["pd1"].ToString();
                ireturn = "00" + ireturn;
                ireturn = ireturn.Substring(ireturn.Length - 2, 2);
            }
            else
            {
                ireturn = ds.Rows[0]["pd2"].ToString();
                ireturn = "00" + ireturn;
                ireturn = ireturn.Substring(ireturn.Length - 2, 2);
            }

            return ireturn;
        }

        public string SetParam(SqlCommand cmd, LapStatusPenyampaian Dt, string TblName)
        {
            var iStatus = "";
            for (var st = 0; st < Dt.Status.Split(',').Count(); st++)
            {
                if (st == 0)
                {
                    iStatus += Dt.Status.Split(',')[st].Split(';')[1];
                }
                else
                {
                    iStatus += "," + Dt.Status.Split(',')[st].Split(';')[1];
                }
            }

            Dt.Status = iStatus;
            int startPage = Convert.ToInt32(Dt.page) - (perpage - 1);
            int EndPage = Convert.ToInt32(Dt.page);

            cmd.Parameters.AddWithValue("@TblName", TblName);
            cmd.Parameters.AddWithValue("@IdPelapor", Dt.IdPelapor);
            cmd.Parameters.AddWithValue("@Cakupan", Dt.Cakupan);
            cmd.Parameters.AddWithValue("@KelInformasi", Dt.KelInformasi);
            //cmd.Parameters.AddWithValue("@idinformation", Dt.idinformation);
            cmd.Parameters.AddWithValue("@Periode", Dt.Periode);
            cmd.Parameters.AddWithValue("@Harian", Dt.Harian);
            cmd.Parameters.AddWithValue("@Minggu", Dt.Minggu);
            cmd.Parameters.AddWithValue("@MBulan", Dt.MBulan);
            cmd.Parameters.AddWithValue("@MTahun", Dt.MTahun);
            cmd.Parameters.AddWithValue("@BBulan", Dt.BBulan);
            cmd.Parameters.AddWithValue("@BTahun", Dt.BTahun);
            cmd.Parameters.AddWithValue("@Triwulan", Dt.Triwulan);
            cmd.Parameters.AddWithValue("@TTahun", Dt.TTahun);
            cmd.Parameters.AddWithValue("@Semester", Dt.Semester);
            cmd.Parameters.AddWithValue("@STahun", Dt.STahun);
            cmd.Parameters.AddWithValue("@Status", Dt.Status);
            cmd.Parameters.AddWithValue("@wilayahkerja", Dt.wilayahkerja);
            cmd.Parameters.AddWithValue("@pdata_penyampaian", Dt.pdata_penyampaian);
            cmd.Parameters.AddWithValue("@startpage", startPage);
            cmd.Parameters.AddWithValue("@endpage", EndPage);
            cmd.Parameters.AddWithValue("@by", pUser);

            return "";
        }

        public bool cek_api(string pUser, string idpelapor, string idinformasi, string flag)
        {
            //pv_loadUser();
            //var acc = "Button Delete";
            //var iaccess = new MenuLeftClass().access(pUser, iMenuId, acc);
            //check session
            var jreturn = new JObject();
            try
            {
                //delete 
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("api_cekGroup", myConn);
                cmd.Parameters.AddWithValue("@username", pUser);
                cmd.Parameters.AddWithValue("@idpelapor", idpelapor);
                cmd.Parameters.AddWithValue("@idinformasi", idinformasi);
                cmd.Parameters.AddWithValue("@flag", flag);
                DataTable dt = new DataTable();
                dt = obj.GetList(myConn, cmd);
                myConn.Close();
                var a = dt.Rows[0][0].ToString();
                if (a == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public FileResult exportCSV(string KelinfDec)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbfasle = new StringBuilder();
            var cek = false;
            try
            {
                pv_loadUser();
                TblName = lhash.Base64Encode(pUser);
                TblName = "ZZLA_" + TblName;
                //TblName = "ZZLA_temp01";
                char delimiter = ';';
                var iStatus = "";

                //BEGIN fredy 20210818 add parameter download
                string[] KelinfDecArr = lhash.Base64Decode(KelinfDec).Split('|');
                string Kelinf = KelinfDecArr[0];
                string Status = KelinfDecArr[1];
                string Cakupan = KelinfDecArr[2];
                string JKegiatan = KelinfDecArr[3];
                string Periode = KelinfDecArr[4];
                string PD = KelinfDecArr[5];
                string PDDate = KelinfDecArr[6];
                string StatusClear = KelinfDecArr[7];

                string TanggalCetak = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                string UserCetak = pUser;

                sb.Append("Cakupan" + delimiter + Cakupan);
                sb.Append("\r\n");
                sb.Append("Jenis Kegiatan" + delimiter + JKegiatan);
                sb.Append("\r\n");
                sb.Append("Kelompok Informasi" + delimiter + Kelinf);
                sb.Append("\r\n");
                sb.Append("Periode" + delimiter + Periode);
                sb.Append("\r\n");
                sb.Append(PD + delimiter + PDDate);
                sb.Append("\r\n");
                sb.Append("Status" + delimiter + StatusClear);
                sb.Append("\r\n");
                sb.Append("Tanggal Cetak" + delimiter + TanggalCetak);
                sb.Append("\r\n");
                sb.Append("User Cetak" + delimiter + UserCetak);
                sb.Append("\r\n");
                sb.Append("\r\n");

                //END fredy

                for (var st = 0; st < Status.Split(',').Count(); st++)
                {
                    if (st == 0)
                    {
                        iStatus += Status.Split(',')[st].Split(';')[1];
                    }
                    else
                    {
                        iStatus += "," + Status.Split(',')[st].Split(';')[1];
                    }
                }
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("la_getDataHDR", myConn);
                cmd.Parameters.AddWithValue("@flag", "H2");
                cmd.Parameters.AddWithValue("@TblName", TblName);
                cmd.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                cmd.Parameters.AddWithValue("@Status", iStatus);
                DataTable dsHeader = obj.GetList(myConn, cmd);
                myConn.Close();

                if (dsHeader.Rows.Count > 0)
                {

                    //value
                    bool isCorrect = true;
                    DataTable dsDetail = new DataTable();
                    try
                    {
                        var myConn3 = obj.myConn();
                        SqlCommand cmd3 = new SqlCommand("la_exportdata", myConn3);
                        cmd3.Parameters.AddWithValue("@TblName", TblName);
                        cmd3.Parameters.AddWithValue("@Periode", Periode);
                        cmd3.Parameters.AddWithValue("@Status", iStatus);
                        dsDetail = obj.GetList(myConn3, cmd3);
                        myConn3.Close();
                    }
                    catch (Exception)
                    {
                        isCorrect = false;
                    }


                    //StringBuilder sb = new StringBuilder();



                    if (isCorrect)
                    {

                        sb.Append("Nama Bank" + delimiter);
                        if (dsHeader.Rows.Count > 0)
                        {
                            sb.Append(Kelinf + " - Status KI - Laporan" + delimiter);
                            sb.Append(Kelinf + " - Status KI - Koreksi" + delimiter);
                        }
                        var h = 0;
                        foreach (DataRow item in dsHeader.Rows)//dsHeader.Rows.Count
                        {
                            if (dsHeader.Rows.Count - 1 == h)
                            {
                                sb.Append(Kelinf + " - " + item[1].ToString() + " - Laporan" + delimiter);
                                sb.Append(Kelinf + " - " + item[1].ToString() + " - Koreksi");
                            }
                            else
                            {
                                sb.Append(Kelinf + " - " + item[1].ToString() + " - Laporan" + delimiter);
                                sb.Append(Kelinf + " - " + item[1].ToString() + " - Koreksi" + delimiter);
                            }
                            h++;
                        }

                        var ifirst = 0;
                        foreach (DataRow item in dsDetail.Rows)
                        {
                            //if (ifirst == 0)
                            //{
                            //    sb.Append(item[0].ToString());
                            //}
                            //else
                            //{
                            var iLength = item.ItemArray.Length;
                            for (var i = 0; i < iLength; i++)
                            {
                                if (i == 0)
                                {
                                    sb.Append("\r\n");
                                }
                                if (iLength - 1 == i)
                                {
                                    sb.Append(item[i].ToString());
                                }
                                else
                                {
                                    sb.Append(item[i].ToString() + delimiter);
                                }
                            }
                            ifirst = ifirst + 1;
                        }
                        cek = true;
                    }
                    else
                    {
                        sb.Append("Bank");
                        sb.Append("\r\n");
                        sb.Append("Data tidak ditemukan");
                        sb.Append("\r\n");
                    }
                }
                else
                {
                    cek = false;
                    sb.Append("Bank");
                    sb.Append("\r\n");
                    sb.Append("Data tidak ditemukan");
                    sb.Append("\r\n");
                }

            }
            catch (Exception ex)
            {
                cek = false;
                obj.CreateLog("#exportCSV#" + ex.Message);
                //throw;
            }
            //var isb = "";
            //if (cek)
            //{
            //    isb = sb.ToString();
            //}
            //else
            //{
            //    isb = sbfasle.ToString();
            //}
            byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
            string fileName = "Laporan Absensi.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public string ExportToExcel(string KelinfDec)
        {
            pv_loadUser();
            TblName = lhash.Base64Encode(pUser);
            TblName = "ZZLA_" + TblName;
            //TblName = "ZZLA_temp01";
            var isw = "";
            var iStatus = "";
            //BEGIN fredy 20210818 add parameter download
            string[] KelinfDecArr = lhash.Base64Decode(KelinfDec).Split('|');
            string Kelinf = KelinfDecArr[0];
            string Status = KelinfDecArr[1];
            string Cakupan = KelinfDecArr[2];
            string JKegiatan = KelinfDecArr[3];
            string Periode = KelinfDecArr[4];
            string PD = KelinfDecArr[5];
            string PDDate = KelinfDecArr[6];
            string StatusClear = KelinfDecArr[7];

            string TanggalCetak = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string UserCetak = pUser;
            //END fredy
            for (var st = 0; st < Status.Split(',').Count(); st++)
            {
                if (st == 0)
                {
                    iStatus += Status.Split(',')[st].Split(';')[1];
                }
                else
                {
                    iStatus += "," + Status.Split(',')[st].Split(';')[1];
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Laporan Absensi.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var jreturn = new JObject();

            DataTable dsDetail = new DataTable();
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                try
                {

                    //var iStatus = "Tidak Menyampaikan Laporan";
                    //var Kelinf = "Kelompok Informasi Keuangan";
                    var myConn = obj.myConn();
                    SqlCommand cmd = new SqlCommand("la_getDataHDR", myConn);
                    cmd.Parameters.AddWithValue("@flag", "H2");
                    cmd.Parameters.AddWithValue("@TblName", TblName);
                    cmd.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                    cmd.Parameters.AddWithValue("@Status", iStatus);
                    DataTable dsHeader = obj.GetList(myConn, cmd);
                    myConn.Close();

                    if (dsHeader.Rows.Count > 0)
                    {
                        //value

                        bool isCorrect = true;
                        try
                        {

                            var myConn3 = obj.myConn();
                            SqlCommand cmd3 = new SqlCommand("la_exportdata", myConn3);
                            cmd3.Parameters.AddWithValue("@TblName", TblName);
                            cmd3.Parameters.AddWithValue("@Periode", Periode);
                            cmd3.Parameters.AddWithValue("@Status", iStatus);
                            dsDetail = obj.GetList(myConn3, cmd3);
                            myConn3.Close();
                        }
                        catch (Exception)
                        {

                            isCorrect = false;
                        }

                        if (isCorrect)
                        {

                            var iHdr = dsHeader.AsEnumerable().ToList();

                            for (int i = 0; i < 12; i++)
                            {
                                if (i == 0)
                                {
                                    DataRow row1 = dsDetail.NewRow();

                                    row1[dsDetail.Columns[i].ColumnName] = "Bank";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = Kelinf;
                                    dsDetail.Rows.InsertAt(row1, i);


                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = "";
                                    dsDetail.Rows.InsertAt(row1, i);

                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "User Cetak";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = UserCetak;
                                    dsDetail.Rows.InsertAt(row1, i);

                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "Tanggal Cetak";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = TanggalCetak;
                                    dsDetail.Rows.InsertAt(row1, i);

                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "Status";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = StatusClear;
                                    dsDetail.Rows.InsertAt(row1, i);

                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = PD;
                                    row1[dsDetail.Columns[i + 1].ColumnName] = PDDate;
                                    dsDetail.Rows.InsertAt(row1, i);
                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "Periode";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = Periode;
                                    dsDetail.Rows.InsertAt(row1, i);
                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "Kelompok Informasi";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = Kelinf;
                                    dsDetail.Rows.InsertAt(row1, i);
                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "Jenis Kegiatan";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = JKegiatan;
                                    dsDetail.Rows.InsertAt(row1, i);
                                    row1 = dsDetail.NewRow();
                                    row1[dsDetail.Columns[i].ColumnName] = "Cakupan";
                                    row1[dsDetail.Columns[i + 1].ColumnName] = Cakupan;
                                    dsDetail.Rows.InsertAt(row1, i);
                                    row1 = dsDetail.NewRow();
                                }
                                if (i == 10)
                                {
                                    DataRow row1 = dsDetail.NewRow();
                                    string colname = "";
                                    int inf = 0;
                                    for (int k = 0; k < dsDetail.Columns.Count; k++)
                                    {
                                        colname += " ";
                                        dsDetail.Columns[k].ColumnName = colname;//set blank to header
                                        if (k == 1)
                                        {
                                            row1[dsDetail.Columns[k].ColumnName] = "Status KI";
                                        }
                                        else if (k > 1)
                                        {
                                            inf = inf + 1;
                                            if (k - 2 < dsHeader.Rows.Count)
                                            {
                                                row1[dsDetail.Columns[k + inf].ColumnName] = iHdr[k - 2].ItemArray[1].ToString();//Set nama informasi
                                            }
                                        }
                                    }
                                    dsDetail.Rows.InsertAt(row1, i);
                                }
                                if (i == 11)
                                {
                                    DataRow row1 = dsDetail.NewRow();
                                    for (int k = 0; k < dsDetail.Columns.Count; k++)
                                    {
                                        if (k > 0)
                                        {
                                            if (k % 2 != 0)
                                            {
                                                row1[dsDetail.Columns[k].ColumnName] = "Laporan";
                                            }
                                            else
                                            {
                                                row1[dsDetail.Columns[k].ColumnName] = "Koreksi";
                                            }
                                        }
                                    }
                                    dsDetail.Rows.InsertAt(row1, i);
                                }
                            }
                        }
                        else
                        {
                            string colname = " ";
                            dsDetail.Clear();
                            dsDetail.Columns.Add("");
                            dsDetail.Columns.Add("");
                            DataRow dataRow;
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Cakupan";
                            dataRow[1] = Cakupan;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Jenis Kegiatan";
                            dataRow[1] = JKegiatan;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Kelompok Informasi";
                            dataRow[1] = Kelinf;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Periode";
                            dataRow[1] = Periode;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = PD;
                            dataRow[1] = PDDate;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Status";
                            dataRow[1] = StatusClear;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Tanggal Cetak";
                            dataRow[1] = TanggalCetak;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "User Cetak";
                            dataRow[1] = UserCetak;
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "";
                            dataRow[1] = "";
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Bank";
                            dataRow[1] = "";
                            dsDetail.Rows.Add(dataRow);
                            dataRow = dsDetail.NewRow();
                            dataRow[0] = "Data tidak ditemukan";
                            dataRow[1] = "";
                            dsDetail.Rows.Add(dataRow);

                            dsDetail.Columns[0].ColumnName = colname;//set blank to header
                            dsDetail.Columns[1].ColumnName = colname + colname;//set blank to header
                        }

                    }
                    else
                    {
                        string colname = " ";
                        dsDetail.Clear();
                        dsDetail.Columns.Add("");
                        dsDetail.Columns.Add("");
                        DataRow dataRow;
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Cakupan";
                        dataRow[1] = Cakupan;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Jenis Kegiatan";
                        dataRow[1] = JKegiatan;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Kelompok Informasi";
                        dataRow[1] = Kelinf;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Periode";
                        dataRow[1] = Periode;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = PD;
                        dataRow[1] = PDDate;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Status";
                        dataRow[1] = StatusClear;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Tanggal Cetak";
                        dataRow[1] = TanggalCetak;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "User Cetak";
                        dataRow[1] = UserCetak;
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "";
                        dataRow[1] = "";
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Bank";
                        dataRow[1] = "";
                        dsDetail.Rows.Add(dataRow);
                        dataRow = dsDetail.NewRow();
                        dataRow[0] = "Data tidak ditemukan";
                        dataRow[1] = "";
                        dsDetail.Rows.Add(dataRow);

                        dsDetail.Columns[0].ColumnName = colname;//set blank to header
                        dsDetail.Columns[1].ColumnName = colname + colname;//set blank to header
                    }
                }
                catch (Exception ex)
                {
                    string colname = " ";
                    dsDetail.Clear();
                    dsDetail.Columns.Add("");
                    dsDetail.Columns.Add("");
                    DataRow dataRow;
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Cakupan";
                    dataRow[1] = Cakupan;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Jenis Kegiatan";
                    dataRow[1] = JKegiatan;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Kelompok Informasi";
                    dataRow[1] = Kelinf;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Periode";
                    dataRow[1] = Periode;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = PD;
                    dataRow[1] = PDDate;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Status";
                    dataRow[1] = StatusClear;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Tanggal Cetak";
                    dataRow[1] = TanggalCetak;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "User Cetak";
                    dataRow[1] = UserCetak;
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "";
                    dataRow[1] = "";
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Bank";
                    dataRow[1] = "";
                    dsDetail.Rows.Add(dataRow);
                    dataRow = dsDetail.NewRow();
                    dataRow[0] = "Data tidak ditemukan";
                    dataRow[1] = "";
                    dsDetail.Rows.Add(dataRow);

                    dsDetail.Columns[0].ColumnName = colname;//set blank to header
                    dsDetail.Columns[1].ColumnName = colname + colname;//set blank to header
                    obj.CreateLog("#ExportToExcel#" + ex.Message);
                    //throw;
                }



                var GridView1 = new GridView();
                GridView1.DataSource = dsDetail;
                GridView1.DataBind();
                GridView1.AllowPaging = false;

                int ir = 0;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    row.BackColor = Color.White;
                    int ir2 = 0;
                    foreach (TableCell cell in row.Cells)
                    {
                        //if (ir == 0 && ir2 == 0) { cell.RowSpan = 3; }//marge row Nama Bank
                        //if (ir == 0 && ir2 == 1) { cell.ColumnSpan = dsDetail.Columns.Count - 1; }//marge col kelompok informasi
                        //if (ir == 1 && ir2 < dsDetail.Columns.Count - 1) { cell.ColumnSpan = 2; }//marge col informasi
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView1.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                        ir2++;
                    }
                    ir++;
                }
                GridView1.RenderControl(hw);
                isw = sw.ToString();

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(isw);
                Response.Flush();
                Response.End();
            }
            return "";
        }
        public String ExportToPDF_old(string Kelinf, string Status)
        {
            //pv_loadUser();
            //TblName = lhash.Base64Encode(pUser);
            //TblName = "ZZLA_" + TblName;
            TblName = "ZZLA_aGVyeXVzbUBnbWFpbC5jb20=";
            var isw = "";
            var iStatus = "";
            for (var st = 0; st < Status.Split(',').Count(); st++)
            {
                if (st == 0)
                {
                    iStatus += Status.Split(',')[st].Split(';')[1];
                }
                else
                {
                    iStatus += "," + Status.Split(',')[st].Split(';')[1];
                }
            }

            var jreturn = new JObject();
            using (StringWriter sw = new StringWriter())
            {
                try
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    //var iStatus = "Tidak Menyampaikan Laporan";
                    //var Kelinf = "Kelompok Informasi Keuangan";
                    var myConn = obj.myConn();
                    SqlCommand cmd = new SqlCommand("la_getDataHDR", myConn);
                    cmd.Parameters.AddWithValue("@flag", "H2");
                    cmd.Parameters.AddWithValue("@TblName", TblName);
                    cmd.Parameters.AddWithValue("@kelompokinformasi", Kelinf);
                    cmd.Parameters.AddWithValue("@Status", iStatus);
                    DataTable dsHeader = obj.GetList(myConn, cmd);
                    myConn.Close();

                    if (dsHeader.Rows.Count > 0)
                    {
                        //value
                        var myConn3 = obj.myConn();
                        SqlCommand cmd3 = new SqlCommand("la_exportdata", myConn3);
                        cmd3.Parameters.AddWithValue("@TblName", TblName);
                        cmd3.Parameters.AddWithValue("@Status", iStatus);
                        DataTable dsDetail = obj.GetList(myConn3, cmd3);
                        myConn3.Close();

                        var iHdr = dsHeader.AsEnumerable().ToList();

                        for (int i = 0; i < 3; i++)
                        {
                            if (i == 0)
                            {
                                DataRow row1 = dsDetail.NewRow();
                                row1[dsDetail.Columns[i].ColumnName] = "Nama Bank";//set nama bank
                                row1[dsDetail.Columns[i + 1].ColumnName] = Kelinf;//set kelompok informasi
                                dsDetail.Rows.InsertAt(row1, i);
                            }
                            if (i == 1)
                            {
                                DataRow row1 = dsDetail.NewRow();
                                string colname = "";
                                for (int k = 0; k < dsDetail.Columns.Count; k++)
                                {
                                    colname += " ";
                                    dsDetail.Columns[k].ColumnName = colname;//set blank to header
                                    if (k == 0)
                                    {
                                        row1[dsDetail.Columns[k].ColumnName] = "Status KI";
                                    }
                                    else
                                    {
                                        if (k - 1 < dsHeader.Rows.Count)
                                        {
                                            row1[dsDetail.Columns[k].ColumnName] = iHdr[k - 1].ItemArray[1].ToString();//Set nama informasi
                                        }
                                    }
                                }
                                dsDetail.Rows.InsertAt(row1, i);
                            }
                            if (i == 2)
                            {
                                DataRow row1 = dsDetail.NewRow();
                                for (int k = 0; k < dsDetail.Columns.Count - 1; k++)
                                {
                                    if (k % 2 == 0)
                                    {
                                        row1[dsDetail.Columns[k].ColumnName] = "Laporan";
                                    }
                                    else
                                    {
                                        row1[dsDetail.Columns[k].ColumnName] = "Koreksi";
                                    }
                                }
                                dsDetail.Rows.InsertAt(row1, i);
                            }
                        }


                        //for (int k = dsDetail.Columns.Count; k < dsDetail.Columns.Count + 10000; k++)
                        //{
                        //    DataRow rowss = dsDetail.NewRow();
                        //    //row1[dsDetail.Columns[k].ColumnName] = "Bank"+k.ToString();
                        //    rowss[dsDetail.Columns[0].ColumnName] = "Bank" + k.ToString();
                        //    dsDetail.Rows.Add(rowss);
                        //}

                        //DataRow removeRow = dsDetail.Rows[0];
                        //removeRow.Delete();

                        var GridView1 = new GridView();
                        GridView1.DataSource = dsDetail;
                        GridView1.DataBind();
                        GridView1.AllowPaging = false;

                        int ir = 0;
                        foreach (GridViewRow row in GridView1.Rows)
                        {
                            if (ir <= 2)
                            {
                                row.BackColor = Color.DarkCyan; //"#2d4154";
                            }
                            else
                            {
                                row.BackColor = Color.White;
                            }
                            int ir2 = 0;
                            foreach (TableCell cell in row.Cells)
                            {
                                if (ir == 0 && ir2 == 0) { cell.RowSpan = 3; }//marge row Nama Bank
                                if (ir == 0 && ir2 == 1) { cell.ColumnSpan = dsDetail.Columns.Count - 1; }//marge col kelompok informasi
                                if (ir == 1 && ir2 < dsDetail.Columns.Count - 1) { cell.ColumnSpan = 2; }//marge col informasi

                                if (ir == 0 && ir2 > 1) { cell.Attributes.Add("style", "display: none"); }//hide col kelompok informasi
                                if (ir == 1 && ir2 >= dsHeader.Rows.Count + 1) { cell.Attributes.Add("style", "display: none"); }//hide col informasi
                                if (ir == 2 && ir2 >= dsHeader.Rows.Count * 2 + 2) { cell.Attributes.Add("style", "display: none"); }//hide col lapor/koreksi
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    cell.BackColor = GridView1.RowStyle.BackColor;
                                }
                                cell.CssClass = "textmode";
                                ir2++;
                            }
                            ir++;
                        }
                        GridView1.RenderControl(hw);
                        isw = sw.ToString();
                    }
                    else
                    {
                        isw = "";
                    }
                }
                catch (Exception)
                {
                    isw = "";
                    //throw;
                }

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";

                if (isw.Length > 1)
                {
                    int ilen = 95;
                    isw = isw.Substring(0, ilen) + " style='display: none'" + isw.Substring(ilen);
                }

                OpenHtmlToPdf.PaperSize size = new OpenHtmlToPdf.PaperSize(Length.Millimeters(210), Length.Millimeters(297));
                var pdf = Pdf.From(isw).OfSize(size);
                byte[] pdfBytes = pdf.Content();

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Laporan Absensi.pdf");
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.End();
            }
            return "";
        }

        public String ExportToPDF(string KelinfDec)
        {
            pv_loadUser();
            TblName = lhash.Base64Encode(pUser);
            TblName = "ZZLA_" + TblName;
            //TblName = "ZZLA_a3VtYWxhQGJpLmdvLmlk";
            string strHtml = "";

            //BEGIN fredy 20210818 add parameter download
            string[] KelinfDecArr = lhash.Base64Decode(KelinfDec).Split('|');
            string Kelinf = KelinfDecArr[0];
            string Status = KelinfDecArr[1];
            string Cakupan = KelinfDecArr[2];
            string JKegiatan = KelinfDecArr[3];
            string Periode = KelinfDecArr[4];
            string PD = KelinfDecArr[5];
            string PDDate = KelinfDecArr[6];
            string StatusClear = KelinfDecArr[7];

            //END fredy
            string bc = "#b1c0cf";
            var iStatus = "";
            for (var st = 0; st < Status.Split(',').Count(); st++)
            {
                if (st == 0)
                {
                    iStatus += Status.Split(',')[st].Split(';')[1];
                }
                else
                {
                    iStatus += "," + Status.Split(',')[st].Split(';')[1];
                }
            }
            strHtml += "<table>";
            strHtml += "<tbody>";
            strHtml += "<tr>";
            strHtml += "<td>Cakupan</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + Cakupan + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>Jenis Kegiatan</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + JKegiatan + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>Kelompok Informasi</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + Kelinf + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>Periode</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + Periode + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>" + PD + "</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + PDDate + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>Status</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + StatusClear + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>Tanggal Cetak</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "</td>";
            strHtml += "</tr>";
            strHtml += "<tr>";
            strHtml += "<td>User Cetak</td>";
            strHtml += "<td>:</td>";
            strHtml += "<td>" + pUser + "</td>";
            strHtml += "</tr>";
            strHtml += "</tbody>";
            strHtml += "</table><br>";
            try
            {
                var myConn3 = obj.myConn();
                SqlCommand cmd3 = new SqlCommand("la_ExportDataPdf", myConn3);
                cmd3.Parameters.AddWithValue("@TblName", TblName);
                cmd3.Parameters.AddWithValue("@Periode", Periode);
                cmd3.Parameters.AddWithValue("@Status", iStatus);
                cmd3.CommandTimeout = 600;
                DataTable dsDetail = obj.GetList(myConn3, cmd3);
                myConn3.Close();

                var iDt = dsDetail.AsEnumerable().ToList();
                if (iDt.Count > 0)
                {
                    strHtml += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>";
                    for (int i = 0; i < iDt.Count; i++)
                    {
                        var cek = iDt[i].ItemArray[0].ToString();
                        if (cek == "Bank")
                        {
                            #region kelompok informasi
                            strHtml += "<tr style='background-color:" + bc + ";'>";
                            strHtml += "<td class='textmode' rowspan='3'>" + iDt[i].ItemArray[0].ToString() + "</td>";
                            var ikel = iDt[i].ItemArray[1].ToString().Split('|');
                            strHtml += "<td class='textmode' colspan='6'>" + iDt[i].ItemArray[1].ToString().Split('|')[0] + "</td>";
                            strHtml += "</tr>";
                            #endregion kelompok informasi

                            #region informasi
                            strHtml += "<tr style='background-color:" + bc + ";'>";
                            if (iDt[i].ItemArray[1].ToString() == null || iDt[i].ItemArray[1].ToString() == "")
                            {
                                strHtml += "<td class='textmode' colspan='2'>&nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td class='textmode' colspan='2'>" + iDt[i].ItemArray[1].ToString().Split('|')[1] + "</td>";
                            }
                            if (iDt[i].ItemArray[3].ToString() == null || iDt[i].ItemArray[3].ToString() == "")
                            {
                                strHtml += "<td class='textmode' colspan='2'>&nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td class='textmode' colspan='2'>" + iDt[i].ItemArray[3].ToString().Split('|')[1] + "</td>";
                            }
                            if (iDt[i].ItemArray[5].ToString() == null || iDt[i].ItemArray[5].ToString() == "")
                            {
                                strHtml += "<td class='textmode' colspan='2'>&nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td class='textmode' colspan='2'>" + iDt[i].ItemArray[5].ToString().Split('|')[1] + "</td>";
                            }
                            strHtml += "</tr>";

                            #endregion informasi

                            #region lapor/koreksi
                            strHtml += "<tr style='background-color:" + bc + ";'>";
                            for (int lk = 0; lk < 3; lk++)
                            {
                                strHtml += "<td class='textmode'>Laporan</td>";
                                strHtml += "<td class='textmode'>Koreksi</td>";
                            }
                            strHtml += "</tr>";
                            #endregion lapor/koreksi
                        }
                        else
                        {
                            strHtml += "<tr>";
                            for (int k = 0; k < 7; k++)
                            {
                                if (iDt[i].ItemArray[k].ToString() == null || iDt[i].ItemArray[k].ToString() == "")
                                {
                                    strHtml += "<td class='textmode'>&nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td class='textmode'>" + iDt[i].ItemArray[k].ToString() + "</td>";
                                }
                            }
                            strHtml += "</tr>";
                        }
                    }
                    strHtml += "</table>";
                }
                else
                {
                    strHtml += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>";
                    strHtml += "<tbody>";
                    strHtml += "<tr>";
                    strHtml += "<td style='background-color:" + bc + ";'>Bank</td>";
                    strHtml += "</tr>";
                    strHtml += "<tr>";
                    strHtml += "<td>Data tidak ditemukan</td>";
                    strHtml += "</tr>";
                    strHtml += "</tbody>";
                    strHtml += "</table>";
                }


            }
            catch (Exception ex)
            {
                strHtml += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>";
                strHtml += "<tbody>";
                strHtml += "<tr>";
                strHtml += "<td style='background-color:" + bc + ";'>Bank</td>";
                strHtml += "</tr>";
                strHtml += "<tr>";
                strHtml += "<td>Data tidak ditemukan</td>";
                strHtml += "</tr>";
                strHtml += "</tbody>";
                strHtml += "</table>";
                obj.CreateLog("#ExportToPDF#" + ex.Message);
                //throw;
            }

            OpenHtmlToPdf.PaperSize size = new OpenHtmlToPdf.PaperSize(Length.Millimeters(210), Length.Millimeters(297));
            var pdf = Pdf.From(strHtml).OfSize(size);
            byte[] pdfBytes = pdf.Content();

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Laporan Absensi.pdf");
            Response.BinaryWrite(pdfBytes);
            Response.Flush();
            Response.End();

            return "";
        }


        //[HttpPost]
        //public ActionResult ConvertHtmlPageToPdf()
        //{
        //    // get the HTML code of this view
        //    string htmlToConvert = RenderViewAsString("Index", null);

        //    // the base URL to resolve relative images and css
        //    String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
        //    String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length -
        //        "Home/ConvertThisPageToPdf".Length);

        //    // instantiate the HiQPdf HTML to PDF converter
        //    HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

        //    // hide the button in the created PDF
        //    htmlToPdfConverter.HiddenHtmlElements = new string[]
        //               { "#convertThisPageButtonDiv" };

        //    // render the HTML code as PDF in memory
        //    byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

        //    // send the PDF file to browser
        //    FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
        //    fileResult.FileDownloadName = "ThisMvcViewToPdf.pdf";

        //    return fileResult;
        //}


        //public FileResult Export(string GridHtml)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(GridHtml);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "Grid.pdf");
        //    }
        //}


    }
}