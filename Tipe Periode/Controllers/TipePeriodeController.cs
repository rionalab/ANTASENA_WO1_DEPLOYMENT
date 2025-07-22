using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Antasena.Models;
using Antasena.Models.Libs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Antasena.Controllers
{
    public class TipePeriodeController : Controller
    {
        // GET: TipePeriode
        private lTipePeriode objTp = new lTipePeriode();
        private lConvert objCon = new lConvert();
        private IGlobal obj = new IGlobal();
        public string pUser = "";
        public string iMenuId = "20201000";

        public ActionResult Index()
        {
            try
            {

                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    pUser = Session?["username"]?.ToString();
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

        public String GetList(string FgType = "")
        {
            var jReturn = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }

                pUser = Session["username"].ToString();
                DataTable datasource = objTp.GetListTipePeriode(FgType);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));

                return jReturn.ToString();
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
                    obj.CreateLog($"#TipePeriode#GetList({FgType})#{ex.Message}");
                    return ex.Message;
                }
            }
        }
        public String GetListId(string id)
        {
            var jReturn = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                pUser = Session["username"].ToString();
                if (id != null)
                {
                    DataTable datasource = objTp.GetList(id);
                    jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                }
                return jReturn.ToString();
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
                    obj.CreateLog($"#TipePeriode#GetListId({id})#{ex.Message}");
                    return ex.Message;
                }
            }
        }

        [HttpPost]
        public JObject deldt(TipePeriode Dt)
        {
            pv_loadUser();
            var acc = "Button Delete";
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, acc);
            var jreturn = new JObject();
            try
            {
                if (!iaccess || pUser == "")
                {
                    jreturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jreturn.Add("message", "Anda tidak memiliki akses");
                    return jreturn;
                }

                if (Dt.PeriodId != null)
                {
                    var dt = objTp.Delete(Dt, pUser);

                    jreturn.Add("success", dt.Rows[0]["isexists"].ToString());
                    jreturn.Add("message", dt.Rows[0]["message"].ToString());
                }

            }
            catch (Exception ex)
            {
                jreturn.Add("success", "0");
                jreturn.Add("message", ex.Message);
                obj.CreateLog($"#TipePeriode#deldt({Dt.PeriodId}, {(Dt?.FgType ?? "D")})#{ex.Message}");
            }

            return jreturn;
        }

        [HttpPost]
        public JObject senddt(TipePeriode Dt)
        {
            pv_loadUser();
            var acc = "";
            if (Dt.PeriodId == 0)
            {
                acc = "Button New";
            }
            else
            {
                acc = "Button Update";
            }
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, acc);
            var jreturn = new JObject();
            var Berhasil = "Berhasil Simpan";
            try
            {
                if (!iaccess || pUser == "")
                {
                    jreturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jreturn.Add("message", "Anda tidak memiliki akses");
                    return jreturn;
                }

                var isuccess = false;
                if (Dt.PeriodId != null && Dt.PeriodId != 0)
                {
                    //edit
                    objTp.Update(Dt, pUser);
                    Berhasil = "Berhasil Update";
                    isuccess = true;
                }
                else
                {
                    //create new
                    if (objTp.Insert(Dt, pUser))
                    {
                        Berhasil = "Nama periode telah terdaftar!";
                    }
                    else
                    {
                        isuccess = true;
                    }
                }
                jreturn.Add("success", isuccess);
                jreturn.Add("message", Berhasil);
            }
            catch (Exception ex)
            {
                jreturn.Add("success", false);
                jreturn.Add("message", ex.Message);
                obj.CreateLog($"#TipePeriode#senddt#{acc.Split(' ')[1]}#{JsonConvert.SerializeObject(Dt)}#{ex.Message}");
            }

            return jreturn;
        }



        public JObject dd_PeriodeException(string PeriodType = "")
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
                    SqlCommand cmd = new SqlCommand("tp_getDD_PeriodeException", myConn);
                    cmd.Parameters.AddWithValue("@PeriodType", PeriodType);
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
                    obj.CreateLog($"#TipePeriode#dd_PeriodeException#{ex.Message}");
                }
            }
            return jret;
        }


        [HttpPost]
        public JObject senddt_EX(TipePeriode_EXC Dt)
        {
            pv_loadUser();
            var acc = "";
            if (Dt.PeriodId == 0)
            {
                acc = "Button New";
            }
            else
            {
                acc = "Button Update";
            }
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, acc);
            var jreturn = new JObject();
            var Berhasil = "Berhasil Simpan";
            //check session
            try
            {
                if (!iaccess || pUser == "")
                {
                    jreturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jreturn.Add("message", "Anda tidak memiliki akses");
                    return jreturn;
                }
                //var zz = tmData["TemplateContain"].ToString();
                var isuccess = false;
                if (Dt.PeriodId != null && Dt.PeriodId != 0)
                {
                    //edit
                    objTp.Update_EX(Dt, pUser);
                    Berhasil = "Berhasil Update";
                    isuccess = true;
                }
                else
                {
                    //create new
                    //if (objTp.Insert_EX(Dt, pUser))
                    var response = objTp.Insert_EX(Dt, pUser);
                    if (Convert.ToBoolean(response.GetValue("success")))
                    {
                        isuccess = true;
                    }
                    else
                    {
                        isuccess = false;
                        Berhasil = response.GetValue("message").ToString();
                    }
                }
                jreturn.Add("success", isuccess);
                jreturn.Add("message", Berhasil);
            }
            catch (Exception ex)
            {
                jreturn.Add("success", false);
                jreturn.Add("message", ex.Message);
                obj.CreateLog($"#TipePeriode#senddt_EX#{acc.Split(' ')[1]}#{JsonConvert.SerializeObject(Dt)}#{ex.Message}");
            }

            return jreturn;
        }
    }
}