using System.Data;
using System.Dynamic;
using API02.Libs;
using API02.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API02.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly lData id;
        private lConvert ic = new lConvert();
        private lMessage mc = new lMessage();
        private lUser iusr = new lUser();
        private lGlobal obj = new lGlobal();

        public HomeController(lData id)
        {
            this.id = id;
        }

        [HttpPost("GetDataMatching")]
        public IActionResult GetDataMatching([FromBody] RekapBelumMatching dt)
        {
            int code = 200;
            //JObject jOut = new JObject();
            var lOut = new List<dynamic>();
            var jOut = new ExpandoObject() as IDictionary<string, object>;

            #region Log
            var ipid = Guid.NewGuid().ToString();
            var ipath = HttpContext.Request.Path.Value.ToString();
            var irequest = JsonConvert.SerializeObject(dt);
            //var irequest2 = HttpContext.Request.QueryString.ToString();
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "Start", dt.username);
            #endregion Log

            if (!ModelState.IsValid)
            {
                // var x = JsonConvert.SerializeObject
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
                    //check ID Pelapor
                    DataTable dtbl = new DataTable();
                    var idPlp = "'" + dt.IdPelapor.Replace(",", "','") + "'";
                    dtbl = iusr.GetDataIDPelaporByUser(dt.username, idPlp);
                    if (dtbl.Rows.Count > 0)
                    {
                        bool res;
                        res = id.BulkDataMatching(dt.TblName, dt);

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
                catch (Exception ex)
                {
                    //jOut = new JObject();
                    code = 500;
                    jOut.Add("status", mc.GetMessage("api_output_not_ok"));
                    jOut.Add("status_code", code);
                    jOut.Add("message", mc.GetMessage("process_not_success"));

                    obj.CreateLog("#GetDataMatching#failed#" + dt.username + "#" + ex.Message + ex.ToString());

                }
            }

            lOut.Add((ExpandoObject)jOut);

            #region Log
            obj.InsertLogs(ipid, ipath, irequest, code, JsonConvert.SerializeObject(lOut, Newtonsoft.Json.Formatting.Indented).ToString(), "End", dt.username);
            #endregion Log

            return StatusCode(code, lOut[0]);
        }
    }
}
