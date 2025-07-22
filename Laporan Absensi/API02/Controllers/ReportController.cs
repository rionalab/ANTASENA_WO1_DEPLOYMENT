using System.Data;
using System.Dynamic;
using API02.Libs;
using API02.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API02.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly lData ld;
        private lUser lusr = new lUser();
        private lMessage mc = new lMessage();
        private lGlobal obj = new lGlobal();
        private lConvert lc = new lConvert();

        public ReportController(lData ld)
        {
            this.ld = ld;
        }

        [HttpPost("GetAbsensi")]
        public IActionResult Absensi([FromBody] LapStatusPenyampaian Dt)
        {
            int code = 200;
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;
            //JObject jOut = new JObject();

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkAbsensi(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    obj.CreateLog("#GetAbsensi#failed#" + Dt.username + "#" + Dt.username + "#" + ex.Message + ex.ToString());

                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));
                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("GetRincianAbsensi")]
        public IActionResult RincianAbsensi([FromBody] LapStatusPenyampaian Dt)
        {
            int code = 200;
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkRincianAbsensi(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetRincianAbsensi#failed#" + Dt.username + "#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("GetRincianAbsensiDtl")]
        public IActionResult RincianAbsensiDtl([FromBody] LapStatusPenyampaian Dt)
        {
            int code = 200;
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblNameDtl;
                            res = ld.BulkRincianAbsensiDtl(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    obj.CreateLog("#GetRincianAbsensiDtl#failed#" + Dt.username + "#" + Dt.username + "#" + ex.Message + ex.ToString());

                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("GetStatusPenyampaian")]
        public IActionResult StatusPenyampaian([FromBody] LapStatusPenyampaian Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;


            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkStatusPenyampaian(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetStatusPenyampaian#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("GetRincianGagalValidasi")]
        public IActionResult RincianGagalValidasi([FromBody] LapRincianGagalValidasi Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkRincianGagalValidasi(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetRincianGagalValidasi#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("RekapBelumMatching")]
        public IActionResult RekapBelumMatch([FromBody] RekapBelumMatching Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkRekapBelumMatching(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#RekapBelumMatching#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("RincianBelumMatching")]
        public IActionResult RincianBelumMatch([FromBody] RincianBelumMatching Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        string lIdPelapor = "";
                        string lkodePelapor = "";
                        string lKodePelaporlawan = "";
                        if (Dt.IdPelapor.ToLower().Equals("all"))
                        {
                            lIdPelapor = lusr.GetIDPelaporCommaByUsername(Dt.username, "0", "", "", "", "");
                            lkodePelapor = lusr.GetKodeBankIDPelaporCommaByUsername(Dt.username, "2", "", "", "", "");
                        }
                        else
                        {
                            lkodePelapor = lusr.GetKodeBankIDPelaporCommaByUsername(Dt.username, "2", "", "", Dt.IdPelapor, "");
                        }
                        if (Dt.IdLawan.ToLower().Equals("all"))
                        {
                            lKodePelaporlawan = lusr.GetKodeBankIDPelaporCommaByUsername(Dt.username, "2", "", "", "", "");
                        }
                        else
                        {
                            lKodePelaporlawan = lusr.GetKodeBankIDPelaporCommaByUsername(Dt.username, "2", "", "", Dt.IdLawan, "");
                        }
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkRincianBelumMatching(tblname, Dt, lIdPelapor, lkodePelapor, lKodePelaporlawan);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#RincianBelumMatching#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        #region Denda
        [HttpPost("GetDenda")]
        public IActionResult Get_Denda([FromBody] LapStatusPenyampaian Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkDenda(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetDenda#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        #endregion Denda

        #region Laporan Dinamis
        [HttpPost("GetDinamis")]
        public IActionResult Get_Dinamis([FromBody] ReportGenerator Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.IdPelapor.Replace(",", "','") + "'";
                        //dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        //if (dtbl.Rows.Count > 0)
                        //{
                        //jOut = new JObject();
                        if (Dt.fg == "1")
                        {
                            Dt.ReportQuery = "show tables in ip";
                            var res = ld.SeacrhTbl(Dt);
                            jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                            jOut.Add("data", "");
                        }
                        else if (Dt.fg == "2")
                        {
                            var res = ld.GetListDinamis(Dt);
                            jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                            jOut.Add("data", res["data"].ToString());
                        }
                        else if (Dt.fg == "3")
                        {
                            var res = ld.ValidasiDinamis(Dt);
                            jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                            jOut.Add("data", "");
                        }
                        else if (Dt.fg == "4")
                        {
                            Dt.ReportQuery = String.Format("describe {0}", Dt.searchTerm);
                            var res = ld.GetListDinamis(Dt);
                            jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                            jOut.Add("data", res["data"].ToString());
                        }

                        jOut.Add("status", mc.GetMessage("api_output_ok"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", mc.GetMessage("process_success"));
                        //}
                        //else
                        //{
                        //    jOut = new JObject();
                        //    code = 401;
                        //    jOut.Add("status", mc.GetMessage("unauthorize"));
                        //    jOut.Add("status_code", code);
                        //    jOut.Add("message", "Can't access ID Pelapor");
                        //}
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetDinamis#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }
        #endregion Laporan Dinamis

        #region Data Balance
        [HttpPost("GetDataBalance")]
        public IActionResult GetDataBalance([FromBody] DataBalance Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.userName);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.userName, Dt.menuId);
                    if (auth == true)
                    {
                        var res = ld.ValidasiDataBalance(Dt);
                        jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                        jOut.Add("data", "");

                        jOut.Add("status", mc.GetMessage("api_output_ok"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", mc.GetMessage("process_success"));
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetDataBalance#failed#" + Dt.userName + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.userName);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpPost("PreviewDataBalance")]
        public IActionResult PreviewDataBalance([FromBody] DataBalance Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.userName);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.userName, Dt.menuId);
                    if (auth == true)
                    {
                        var res = ld.GetListDataBalance(Dt);
                        jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                        jOut.Add("data", res["data"].ToString());

                        jOut.Add("status", mc.GetMessage("api_output_ok"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", mc.GetMessage("process_success"));
                    }
                    else
                    {
                        //jOut = new JObject();
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetDataBalance#failed#" + Dt.userName + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.userName);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }

        [HttpGet("GetIdPelaporAbsensiKelengkapan")]
        public IActionResult IdPelaporAbsensiKelengkapan(string periodeData)
        {
            int code = 200;
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            try
            {
                var res = ld.GetIdPelaporAbsensiKelengkapan(periodeData);
                jOut.Add("status_bulk", bool.Parse(res["status_bulk"].ToString()));
                jOut.Add("data", res["data"].ToString());

                jOut.Add("status", mc.GetMessage("api_output_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("process_success"));
            }
            catch (Exception ex)
            {
                obj.CreateLog("#GetIdPelaporAbsensiKelengkapan#failed#" + ex.Message);
                code = 500;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", ex.Message);
            }

            lOut.Add((ExpandoObject)jOut);

            return StatusCode(code, lOut[0]);
        }

        #endregion Data Balance

        #region Kelengkaapn Informasi
        [HttpPost("GetKelengkapanInformasi")]
        public IActionResult Get_Kelengkapan_Informasi([FromBody] KelengkapanInformasi Dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;


            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(Dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", Dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                //jOut = new JObject();
                code = 400;
                jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                jOut.Add("status_code", code);
                jOut.Add("message", mc.GetMessage("bad_parameter"));
            }
            else
            {
                try
                {
                    var auth = lusr.IsAuth(Dt.username, Dt.menuid);
                    if (auth == true)
                    {
                        //check ID Pelapor
                        DataTable dtbl = new DataTable();
                        var idPlp = "'" + Dt.idpelapor.Replace(",", "','") + "'";
                        dtbl = lusr.GetDataIDPelaporByUser(Dt.username, idPlp);
                        if (dtbl.Rows.Count > 0)
                        {
                            bool res = new bool();
                            string tblname = Dt.TblName;
                            res = ld.BulkKelengkapanInformasi(tblname, Dt);

                            //jOut = new JObject();
                            jOut.Add("status", mc.GetMessage("api_output_ok"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", mc.GetMessage("process_success"));
                            jOut.Add("result", res);
                        }
                        else
                        {
                            //jOut = new JObject();
                            code = 401;
                            jOut.Add("status", mc.GetMessage("unauthorize"));
                            jOut.Add("status_code", code);
                            jOut.Add("message", "Can't access ID Pelapor");
                        }
                    }
                    else
                    {
                        //jOut = new JObject(); 
                        code = 401;
                        jOut.Add("status", mc.GetMessage("unauthorize"));
                        jOut.Add("status_code", code);
                        jOut.Add("message", "Access denied");
                    }
                }
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#Get_Kelengkapan_Informasi#failed#" + Dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", Dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);

        }
        #endregion Kelengkaapn Informasi

    }
}
