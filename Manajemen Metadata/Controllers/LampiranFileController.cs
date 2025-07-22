using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Antasena.Models;
using Antasena.Models.Libs;
using Newtonsoft.Json.Linq;

namespace Antasena.Controllers
{
    public class LampiranFileController : Controller
    {
        private lMetadataManagement objMm = new lMetadataManagement();
        lConvert objCon = new lConvert();
        private IGlobal obj = new IGlobal();
        public string pUser = "";
        public string iMenuId = "30601000"; //tambahan
        public bool iaccess = false;
        // GET: LampiranFile
        public ActionResult Index()
        {
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
                    return Redirect("Logout/timeout");
                }

            }
            catch (Exception ex)
            {
                return Redirect("Logout/timeout");

            }
        }

        public string getlist()
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
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    lLampiran lmp = new lLampiran();
                    DataTable datasource = lmp.GetListLampiranAll();
                    jReturn.Add("data", objCon.DatatabletoJarray(datasource));

                }
                return jReturn.ToString();
            }
            catch (Exception e)
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                else
                {
                    return e.Message;
                }
            }
        }

        public string getlistactive()
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
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    lLampiran lmp = new lLampiran();
                    DataTable datasource = lmp.GetListLampiranAllActiveOnly();
                    jReturn.Add("data", objCon.DatatabletoJarray(datasource));

                }
                return jReturn.ToString();
            }
            catch (Exception e)
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                else
                {
                    return e.Message;
                }
            }
        }

        [HttpPost]
        public string getlistid()
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
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    lLampiran lmp = new lLampiran();
                    var lamid = Request.Form["id"].ToString();
                    DataTable datasource = lmp.GetListLampiranDet(lamid);
                    datasource.Columns.Add("file_token", typeof(System.String));
                    lhash lh = new lhash();
                    var xtoken = "";
                    foreach (DataRow r in datasource.Rows)
                    {
                        List<string> listtoken = new List<string>();
                        var yx = r["LampiranFileName"].ToString().Split(';');
                        foreach (string item in yx)
                        {
                            if (!String.IsNullOrEmpty(item))
                            {


                                listtoken.Add(item + "~" + lh.EncodeAES(item + "|" + pUser + "|" + lh.GenerateRandomPassword(8)));
                            }

                        }


                        r["file_token"] = String.Join(";", listtoken);
                    }
                    //jReturn.Add("data", objCon.DatatabletoJarray(datasource));


                    jReturn.Add("data", objCon.DatatabletoJarray(datasource));

                }
                return jReturn.ToString();
            }
            catch (Exception e)
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                else
                {
                    return e.Message;
                }
            }
        }

        [HttpPost]
        public string checkName()
        {
            var name = Request.Form["name"].ToString();
            var varsi = Request.Form["varsi"].ToString();
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
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    lLampiran lmp = new lLampiran();
                    DataTable datasource = lmp.GetListLampiranByNama(name, varsi);
                    jReturn.Add("data", objCon.DatatabletoJarray(datasource));

                }
                return jReturn.ToString();
            }
            catch (Exception e)
            {
                if (!iaccess || pUser == "")
                {
                    jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jReturn.Add("message", "Anda tidak memiliki akses");
                    return jReturn.ToString();
                }
                else
                {
                    return e.Message;
                }
            }
        }


        //[HttpPost]
        //public string checkNamelama()
        //{
        //    var namelama = Request.Form["namelama"].ToString();
        //    var namebaru = Request.Form["namebaru"].ToString();
        //    var versi = Request.Form["versi"].ToString();
        //    var jReturn = new JObject();
        //    pv_loadUser();
        //    var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
        //    try
        //    {
        //        if (!iaccess || pUser == "")
        //        {
        //            jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
        //            jReturn.Add("message", "Anda tidak memiliki akses");
        //            return jReturn.ToString();

        //        }
        //        pUser = Session["username"].ToString();
        //        if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
        //        {
        //            lLampiran lmp = new lLampiran();
        //            DataTable datasource = lmp.GetListLampiranByNamaLama(namebaru, namelama, versi);
        //            jReturn.Add("data", objCon.DatatabletoJarray(datasource));

        //        }
        //        return jReturn.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        if (!iaccess || pUser == "")
        //        {
        //            jReturn.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
        //            jReturn.Add("message", "Anda tidak memiliki akses");
        //            return jReturn.ToString();
        //        }
        //        else
        //        {
        //            return e.Message;
        //        }
        //    }
        //}


        private void pv_loadUser()
        {
            if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
            {
                pUser = Session["username"].ToString();
            }
        }


        [HttpPost]
        public JObject senddt(Lampiran lmpdata)
        {
            lLampiran lmp = new lLampiran();
            JObject jdata = new JObject();
            List<int> listIdAtt = new List<int>(); //yaq
            List<string> allfile = new List<string>();
            string fname = "";
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {

                if (!iaccess || pUser == "")
                {
                    jdata.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jdata.Add("message", "Anda tidak memiliki akses");
                    return jdata;

                }
                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    var isedit = Request.Form["isedit"].ToString();

                    if (!String.IsNullOrEmpty(isedit))
                    {
                        if (isedit.Equals("0"))
                        {
                            //baru
                            DataTable dtnama = lmp.GetListLampiranByNama(lmpdata.LampiranName, lmpdata.Version);
                            if (dtnama.Rows.Count == 0)
                            {
                                HttpFileCollectionBase files = Request.Files;
                                if (files.Count > 0)
                                {
                                    for (int i = 0; i < files.Count; i++)
                                    {
                                        HttpPostedFileBase file = files[i];
                                        IGlobal lg = new IGlobal();
                                        JObject jfallow = lg.getPengumumanAllowFileJson();
                                        var allowmime = jfallow.GetValue("mime_type_allowed").ToString().Split(',');
                                        if (allowmime.Contains(file.ContentType))
                                        {
                                            var extfile = JObject.Parse(jfallow.GetValue("ext_allowed").ToString());
                                            var extfl = extfile.GetValue(file.ContentType).ToString().Split(',');
                                            var fnm = file.FileName.ToString().Split('.');

                                            if (extfl.Contains(fnm[fnm.Length - 1].ToString()))
                                            {
                                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                                {
                                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                                    fname = testfiles[testfiles.Length - 1];
                                                }
                                                else
                                                {
                                                    fname = file.FileName;
                                                }

                                                fname = DateTime.Now.ToString("yyyyMMddTHHmmssfff") + "_lampiran_" + fname.Replace(' ', '_');

                                                //#region old
                                                //file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/"), fname));
                                                //#endregion

                                                #region new upload
                                                Stream fileContent = file.InputStream;
                                                lAttachment libAttachment = new lAttachment();
                                                AttachmentFile attachment = new AttachmentFile();
                                                BinaryReader br = new BinaryReader(fileContent);
                                                byte[] bytes = br.ReadBytes((Int32)fileContent.Length);
                                                attachment.att_filename = fname;
                                                attachment.att_content = bytes;
                                                attachment.att_source = this.ControllerContext.RouteData.Values["controller"].ToString();
                                                attachment.att_createdby = Session["username"].ToString();

                                                int idAtt = libAttachment.InsertAttachment(attachment);

                                                listIdAtt.Add(idAtt);

                                                #endregion

                                                allfile.Add(fname.ToString());

                                            }
                                        }
                                    }

                                    if (allfile.Count > 0)
                                    {

                                        lmpdata.allfiles = allfile;
                                        lmpdata.LampiranFgActive = (String.IsNullOrEmpty(lmpdata.LampiranFgActive)) ? "N" : "Y";
                                        lmpdata.user = Session["username"].ToString();
                                        int iid = lmp.InsertLampiran(lmpdata);

                                        if (iid > 0)
                                        {
                                            #region Update Source ID
                                            foreach (int item in listIdAtt)
                                            {
                                                lAttachment libAttachment = new lAttachment();
                                                AttachmentFile attachmentFile = new AttachmentFile();
                                                attachmentFile.att_sourceid = iid;
                                                attachmentFile.att_id = item;
                                                libAttachment.UpdateSourceIDAttachment(attachmentFile);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region roolback attachemnt
                                            foreach (int item in listIdAtt)
                                            {
                                                lAttachment libAttachment = new lAttachment();
                                                libAttachment.RollbackAttachment(item);
                                            }
                                            #endregion
                                            jdata.Add("success", false);
                                            jdata.Add("message", "Gagal Simpan");
                                            return jdata;
                                        }
                                    }

                                    jdata.Add("success", true);
                                    jdata.Add("message", "Berhasil Simpan");
                                }
                                else
                                {
                                    jdata.Add("success", false);
                                    jdata.Add("message", "tidak ada file");

                                }


                            }
                            else
                            {
                                jdata.Add("success", false);
                                jdata.Add("message", "Judul sudah ada");
                            }


                        }
                        else if (isedit.Equals("1"))
                        {
                            DataTable dtnama = lmp.GetListLampiranDet(lmpdata.LampiranId);
                            var tf = (lmpdata.LampiranName != lmpdata.LampiranNameLama || lmpdata.Version != lmpdata.VersionLama) ? (dtnama.Rows.Count > 0) ? true : false : true;
                            if (tf)
                            {
                                DataTable dtdet = lmp.GetListLampiranDet(lmpdata.LampiranId);
                                if (dtdet.Rows.Count > 0)
                                {
                                    var filelamadb = dtdet.Rows[0]["LampiranFileName"].ToString();
                                    List<string> arrfile = filelamadb.Split(';').ToList<string>();
                                    var filelama = Request.Form["filelama"].ToString();
                                    List<string> arrfilelama = filelama.Split(';').ToList<string>();

                                    #region old delete
                                    //foreach (string item in arrfile)
                                    //{
                                    //    if (!arrfilelama.Contains(item))
                                    //    {
                                    //        var pathx = Path.Combine(Server.MapPath("~/App_Data/"), item);
                                    //        if (System.IO.File.Exists(pathx))
                                    //        {
                                    //            System.IO.File.Delete(pathx);
                                    //        }

                                    //    }


                                    //}
                                    #endregion old delete

                                    HttpFileCollectionBase files = Request.Files;
                                    if (files.Count > 0)
                                    {
                                        for (int i = 0; i < files.Count; i++)
                                        {
                                            HttpPostedFileBase file = files[i];
                                            IGlobal lg = new IGlobal();
                                            JObject jfallow = lg.getPengumumanAllowFileJson();
                                            var allowmime = jfallow.GetValue("mime_type_allowed").ToString().Split(',');
                                            if (allowmime.Contains(file.ContentType))
                                            {
                                                var extfile = JObject.Parse(jfallow.GetValue("ext_allowed").ToString());
                                                var extfl = extfile.GetValue(file.ContentType).ToString().Split(',');
                                                var fnm = file.FileName.ToString().Split('.');

                                                if (extfl.Contains(fnm[fnm.Length - 1].ToString()))
                                                {
                                                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                                    {
                                                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                                        fname = testfiles[testfiles.Length - 1];
                                                    }
                                                    else
                                                    {
                                                        fname = file.FileName;
                                                    }

                                                    fname = DateTime.Now.ToString("yyyyMMddTHHmmssfff") + "_lampiran_" + fname.Replace(' ', '_');

                                                    #region old
                                                    //file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/"), fname));
                                                    //allfile.Add(fname.ToString());
                                                    #endregion

                                                    #region new upload
                                                    Stream fileContent = file.InputStream;
                                                    lAttachment libAttachment = new lAttachment();
                                                    AttachmentFile attachment = new AttachmentFile();
                                                    BinaryReader br = new BinaryReader(fileContent);
                                                    byte[] bytes = br.ReadBytes((Int32)fileContent.Length);
                                                    attachment.att_filename = fname;
                                                    attachment.att_content = bytes;
                                                    attachment.att_source = this.ControllerContext.RouteData.Values["controller"].ToString();
                                                    attachment.att_createdby = Session["username"].ToString();

                                                    int idAtt = libAttachment.InsertAttachment(attachment);

                                                    listIdAtt.Add(idAtt);

                                                    allfile.Add(fname.ToString());

                                                    #endregion

                                                }
                                            }
                                        }

                                    }
                                    allfile.AddRange(arrfilelama);

                                    if (allfile.Count > 0)
                                    {
                                        lmpdata.allfiles = allfile;
                                        lmpdata.LampiranFgActive = (String.IsNullOrEmpty(lmpdata.LampiranFgActive)) ? "N" : "Y";
                                        lmpdata.user = Session["username"].ToString();
                                        int iid = lmp.UpdateLampiran(lmpdata);

                                        if (iid > 0)
                                        {
                                            #region Update Source ID
                                            string createdBy = Session["username"].ToString();
                                            DateTime createdOn = DateTime.Now;
                                            DataTable dtAttachment = new DataTable();

                                            for (int ig = 0; ig < listIdAtt.Count; ig++)
                                            {
                                                lAttachment libAttachment = new lAttachment();
                                                AttachmentFile attachmentFile = new AttachmentFile();
                                                attachmentFile.att_id = listIdAtt[ig];
                                                attachmentFile.att_sourceid = Convert.ToInt32(iid);
                                                attachmentFile.att_updatedby = Session["username"].ToString();
                                                attachmentFile.att_updatedon = createdOn;

                                                libAttachment.UpdateSourceIDAttachment(attachmentFile);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region roolback attachemnt
                                            foreach (int item in listIdAtt)
                                            {
                                                lAttachment libAttachment = new lAttachment();
                                                libAttachment.RollbackAttachment(item);
                                            }
                                            #endregion
                                            jdata.Add("success", false);
                                            jdata.Add("message", "Gagal Simpan");
                                            return jdata;
                                        }
                                    }
                                    jdata.Add("success", true);
                                    jdata.Add("message", "Berhasil Simpan");


                                }
                                else
                                {
                                    jdata.Add("success", false);
                                    jdata.Add("message", "Data Tidak Ditemukan");
                                }

                            }
                            else
                            {
                                jdata.Add("success", false);
                                jdata.Add("message", "Judul dan Version sudah ada");

                            }

                        }
                        else
                        {
                            jdata.Add("success", false);
                            jdata.Add("message", "isedit kosong");
                        }

                    }
                    else
                    {
                        jdata.Add("success", false);
                        jdata.Add("message", "isedit kosong");
                    }
                }
                else
                {
                    jdata.Add("success", false);
                    jdata.Add("message", "session timeout");

                }
                return jdata;
            }
            catch (Exception e)
            {
                //Object jdata = new JObject();
                if (!iaccess || pUser == "")
                {
                    jdata.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jdata.Add("message", "Anda tidak memiliki akses");
                    return jdata;
                }
                else
                {
                    jdata.Add("success", false);
                    jdata.Add("message", e.Message);
                    return jdata;
                }
            }
        }


        [HttpPost]
        public ActionResult Download_Old()
        {
            pv_loadUser();
            byte[] fileBytes = null; string fileName = "failed";
            try
            {

                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    String Token = Request.Form["tf"].ToString();

                    if (!String.IsNullOrEmpty(Token))
                    {
                        lhash lh = new lhash();
                        var dec = lh.DecodeAES(Token).Split('|');
                        var username = dec[1].ToString();
                        var filename = dec[0].ToString();
                        var sess = Session["username"].ToString();
                        if (username.Equals(sess))
                        {
                            var fpath = Server.MapPath("~/App_Data/") + filename;
                            if (System.IO.File.Exists(fpath))
                            {
                                fileBytes = System.IO.File.ReadAllBytes(fpath);
                                fileName = filename;
                                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                            }
                            else
                            {

                                return new HttpNotFoundResult();
                            }

                        }
                        else
                        {

                            return new HttpNotFoundResult();

                        }

                    }
                    else
                    {

                        return new HttpNotFoundResult();
                    }


                }
                else
                {
                    return new HttpNotFoundResult();
                }

            }
            catch (Exception ex)
            {

                return new HttpNotFoundResult();

            }
        }

        #region Metode download baru
        [HttpPost]
        public ActionResult Download()
        {
            pv_loadUser();
            byte[] fileBytes = null; string fileName = "failed";
            try
            {

                if (Session[Session.Keys[0].ToString()] != null && Session.Keys[0].ToString().Contains("SessionID"))
                {
                    var sess = Session["username"].ToString();
                    var att_id = Request.Form["att_id"].ToString();

                    if (!String.IsNullOrEmpty(att_id))
                    {
                        lAttachment libAttachment = new lAttachment();
                        AttachmentFile attachment = new AttachmentFile();
                        DataTable dtAttachment = new DataTable();

                        dtAttachment = libAttachment.GetAttachmentByID(Convert.ToInt32(att_id));

                        if (dtAttachment.Rows.Count > 0)
                        {

                            MemoryStream ms = new MemoryStream((byte[])dtAttachment.Rows[0]["att_content"], 0, 0, true, true);
                            Response.ContentType = "application/octet-stream";

                            Response.AddHeader("content-disposition", "attachment;filename=" + dtAttachment.Rows[0]["att_filename"].ToString());
                            Response.Buffer = true;
                            Response.Clear();
                            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                            Response.OutputStream.Flush();

                            Response.End();
                            return new FileStreamResult(Response.OutputStream, "application/octet-stream");
                        }
                        else
                        {

                            return new HttpNotFoundResult();
                        }


                    }
                    else
                    {

                        return new HttpNotFoundResult();
                    }
                    //}
                    //else
                    //{

                    //    return new HttpNotFoundResult();
                    //}
                }
                else
                {
                    return new HttpNotFoundResult();
                }

            }
            catch (Exception ex)
            {

                return new HttpNotFoundResult();

            }
        }
        #endregion

        [HttpPost]
        public JObject deldt()
        {
            lLampiran lmp = new lLampiran();
            JObject jdata = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {


                if (!iaccess || pUser == "")
                {
                    jdata.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jdata.Add("message", "Anda tidak memiliki akses");
                    return jdata;

                }
                var id = Request.Form["id"].ToString();
                DataTable dtx = lmp.GetListLampiranDet(id);
                if (dtx.Rows.Count > 0)
                {
                    Lampiran lmpdt = new Lampiran();
                    lmpdt.LampiranId = id;
                    lmpdt.user = Session["username"].ToString();
                    int exec = lmp.DeleteLampiran(lmpdt);

                    if (exec > 0)
                    {
                        //var lampfilename = dtx.Rows[0]["LampiranFileName"].ToString();
                        //var lmpspl = lampfilename.Split(';');
                        //foreach (string item in lmpspl)
                        //{
                        //    var fpath = Server.MapPath("~/App_Data/") + item;
                        //    if (System.IO.File.Exists(fpath))
                        //    {
                        //        System.IO.File.Delete(fpath);
                        //    }

                        //}

                        jdata.Add("success", true);
                        jdata.Add("message", "Berhasil Hapus");

                    }
                    else
                    {
                        jdata.Add("success", false);
                        jdata.Add("message", "Gagal hapus");
                    }

                }
                else
                {
                    jdata.Add("success", false);
                    jdata.Add("message", "Gagal hapus");
                }
                return jdata;
            }
            catch (Exception e)
            {
                if (!iaccess || pUser == "")
                {
                    jdata.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jdata.Add("message", "Anda tidak memiliki akses");
                    return jdata;
                }
                else
                {
                    jdata.Add("success", false);
                    jdata.Add("message", e.Message);
                    return jdata;
                }
            }
        }

        [HttpPost]
        public JObject deleteAtt()
        {
            lLampiran lmp = new lLampiran();
            JObject jdata = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                if (!iaccess || pUser == "")
                {
                    jdata.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jdata.Add("message", "Anda tidak memiliki akses");
                    return jdata;

                }
                var id = Request.Form["id"].ToString();
                var idel = lmp.deleteAtt(id, Session["username"].ToString());
                if (idel > 0)
                {
                    jdata.Add("success", true);
                    jdata.Add("message", "Berhasil Hapus");
                }
                else
                {
                    jdata.Add("success", false);
                    jdata.Add("message", "Gagal hapus");
                }

                return jdata;
            }
            catch (Exception e)
            {
                if (!iaccess || pUser == "")
                {
                    jdata.Add("success", Url.Action("timeout", "Logout", null, Request.Url.Scheme));
                    jdata.Add("message", "Anda tidak memiliki akses");
                    return jdata;
                }
                else
                {
                    jdata.Add("success", false);
                    jdata.Add("message", e.Message);
                    return jdata;
                }
            }
        }

        //-- Metadata Start --

        public FileResult testallcsv(string versioncode, string idinformation)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                if (!iaccess || pUser == "")
                {
                    Session["username"].ToString();

                    if (!iaccess && pUser != "")
                    {
                        Session["username_"].ToString();
                    }
                }
                var jReturn = new JObject();
                DataTable datasource = objMm.GetAllCsvMM(versioncode, idinformation);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                StringBuilder sb = new StringBuilder();
                char delimiter = '|';


                //ds.Tables[0].TableName = "Metadata_Management";
                sb.Append("idinformation" + delimiter);
                sb.Append("idelement" + delimiter);
                sb.Append("nillable_yn" + delimiter);
                sb.Append("idtoindex" + delimiter);
                sb.Append("compositekey_yn" + delimiter);
                sb.Append("idtodata" + delimiter);
                sb.Append("length" + delimiter);
                sb.Append("fractiondigit");
                sb.Append("\r\n");

                foreach (DataRow metadata in datasource.Rows)
                {
                    sb.Append(metadata["idinformation"].ToString() + delimiter);
                    sb.Append(metadata["idelement"].ToString() + delimiter);
                    sb.Append(metadata["nillable_yn"].ToString() + delimiter);
                    sb.Append(metadata["idtoindex"].ToString() + delimiter);
                    sb.Append(metadata["compositekey_yn"].ToString() + delimiter);
                    sb.Append(metadata["idtodata"].ToString() + delimiter);
                    sb.Append(metadata["length"].ToString() + delimiter);
                    sb.Append(metadata["fractiondigit"].ToString());

                    sb.Append("\r\n");
                }


                byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
                string fileName = "AllActive.csv";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FileResult testallxml(string versioncode, string idinformation)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                if (!iaccess || pUser == "")
                {
                    Session["username"].ToString();

                    if (!iaccess && pUser != "")
                    {
                        Session["username_"].ToString();
                    }
                }
                var jReturn = new JObject();
                DataTable datasource = objMm.GetAllXmlMM(versioncode, idinformation);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                StringBuilder sb = new StringBuilder();
                char delimiter = ',';


                //ds.Tables[0].TableName = "Metadata_Management";
                sb.AppendLine("<?xml version='1.0' encoding='UTF - 8'?>");
                sb.AppendLine("\r\n");

                foreach (DataRow metadata in datasource.Rows)
                {

                    sb.AppendLine("<row>");

                    sb.AppendLine("<idinformation>" + metadata["idinformation"].ToString() + "</idinformation>");
                    sb.AppendLine("<idelement>" + metadata["idelement"].ToString() + "</idelement>");
                    sb.AppendLine("<nillable_yn>" + metadata["nillable_yn"].ToString() + "</nillable_yn>");
                    sb.AppendLine("<idtoindex>" + metadata["idtoindex"].ToString() + "</idtoindex>");
                    sb.AppendLine("<compositekey_yn>" + metadata["compositekey_yn"].ToString() + "</compositekey_yn>");
                    sb.AppendLine("<idtodata>" + metadata["idtodata"].ToString() + "</idtodata>");
                    sb.AppendLine("<length>" + metadata["length"].ToString() + "</length>");
                    sb.AppendLine("<fractiondigit>" + metadata["fractiondigit"].ToString() + "</fractiondigit>");
                    sb.AppendLine("</row>");
                }


                byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
                string fileName = "AllActive.xml";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FileResult testallsql(string versioncode, string idinformation)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var jReturn = new JObject();
            try
            {
                if (!iaccess || pUser == "")
                {
                    Session["username"].ToString();

                    if (!iaccess && pUser != "")
                    {
                        Session["username_"].ToString();
                    }
                }

                DataTable datasource = objMm.GetAllSqlMM(versioncode, idinformation);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                StringBuilder sb = new StringBuilder();
                char delimiter = ',';



                foreach (DataRow metadata in datasource.Rows)
                {
                    sb.Append(metadata["dtsql"].ToString());
                }


                byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
                string fileName = "AllActive.sql";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public String GetListInfoMM()
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            var jReturn = new JObject();
            try
            {
                pUser = Session["username"].ToString();
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("mm_getInformasiMetadataManagement", myConn);
                DataTable datasource = obj.GetList(myConn, cmd);
                myConn.Close();
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
                    return ex.Message;
                }
            }
        }

        public String VersionCodeList(string idinformation)
        {
            var jReturn = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                pUser = Session["username"].ToString();
                var myConn = obj.myConn();
                SqlCommand cmd = new SqlCommand("mm_getVersionCodeMetadataManagement", myConn);
                cmd.Parameters.Add("@idinformation", SqlDbType.VarChar, 5000).Value = idinformation;
                DataTable datasource = obj.GetList(myConn, cmd);
                myConn.Close();
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
                    return ex.Message;
                }

            }
        }


        public String GetListMetadata(string idinformation, string versioncode, string dateStart, string dateEnd)
        {
            var jReturn = new JObject();
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                pUser = Session["username"].ToString();
                DataTable datasource = objMm.GetList(idinformation, versioncode, dateStart, dateEnd);
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
                    return ex.Message;
                }

            }
        }

        public FileResult testcsv(string versioncode, string idinformation, string startDate = null, string endDate = null)
        {
            pv_loadUser();
            iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);

            try
            {
                if (!iaccess || pUser == "")
                {
                    Session["username"].ToString();

                    if (!iaccess && pUser != "")
                    {
                        Session["username_"].ToString();
                    }
                }

                var jReturn = new JObject();
                DataTable datasource = objMm.GetCsvMM(versioncode, idinformation, startDate, endDate);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                StringBuilder sb = new StringBuilder();
                char delimiter = '|';


                //ds.Tables[0].TableName = "Metadata_Management";
                sb.Append("idelement" + delimiter);
                sb.Append("nillable_yn" + delimiter);
                sb.Append("idtoindex" + delimiter);
                sb.Append("compositekey_yn" + delimiter);
                sb.Append("idtodata" + delimiter);
                sb.Append("length" + delimiter);
                sb.Append("fractiondigit");
                sb.Append("\r\n");

                foreach (DataRow metadata in datasource.Rows)
                {

                    sb.Append(metadata["idelement"].ToString() + delimiter);
                    sb.Append(metadata["nillable_yn"].ToString() + delimiter);
                    sb.Append(metadata["idtoindex"].ToString() + delimiter);
                    sb.Append(metadata["compositekey_yn"].ToString() + delimiter);
                    sb.Append(metadata["idtodata"].ToString() + delimiter);
                    sb.Append(metadata["length"].ToString() + delimiter);
                    sb.Append(metadata["fractiondigit"].ToString());
                    sb.Append("\r\n");
                }


                byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
                string fileName = idinformation + ".csv";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult testxml(string versioncode, string idinformation, string startDate, string endDate)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                if (!iaccess || pUser == "")
                {
                    Session["username"].ToString();

                    if (!iaccess && pUser != "")
                    {
                        Session["username_"].ToString();
                    }
                }
                var jReturn = new JObject();
                DataTable datasource = objMm.GetXmlMM(versioncode, idinformation, startDate, endDate);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                StringBuilder sb = new StringBuilder();
                char delimiter = ',';


                //ds.Tables[0].TableName = "Metadata_Management";
                //ds.Tables[0].TableName = "Metadata_Management";
                sb.AppendLine("<?xml version='1.0' encoding='UTF - 8'?>");
                sb.AppendLine("\r\n");

                foreach (DataRow metadata in datasource.Rows)

                {

                    sb.AppendLine("<row>");

                    sb.AppendLine("<idelement>" + metadata["idelement"].ToString() + "</idelement>");
                    sb.AppendLine("<nillable_yn>" + metadata["nillable_yn"].ToString() + "</nillable_yn>");
                    sb.AppendLine("<idtoindex>" + metadata["idtoindex"].ToString() + "</idtoindex>");
                    sb.AppendLine("<compositekey_yn>" + metadata["compositekey_yn"].ToString() + "</compositekey_yn>");
                    sb.AppendLine("<idtodata>" + metadata["idtodata"].ToString() + "</idtodata>");
                    sb.AppendLine("<length>" + metadata["length"].ToString() + "</length>");
                    sb.AppendLine("<fractiondigit>" + metadata["fractiondigit"].ToString() + "</fractiondigit>");

                    sb.AppendLine("</row>");

                }


                byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
                string fileName = idinformation + ".xml";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FileResult testsql(string versioncode, string idinformation, string startDate, string endDate)
        {
            pv_loadUser();
            var iaccess = new MenuLeftClass().access(pUser, iMenuId, "", true);
            try
            {
                if (!iaccess || pUser == "")
                {
                    Session["username"].ToString();

                    if (!iaccess && pUser != "")
                    {
                        Session["username_"].ToString();
                    }
                }
                var jReturn = new JObject();
                DataTable datasource = objMm.GetSqlMM(versioncode, idinformation, startDate, endDate);
                jReturn.Add("data", objCon.DatatabletoJarray(datasource));
                StringBuilder sb = new StringBuilder();
                char delimiter = ',';



                foreach (DataRow metadata in datasource.Rows)
                {
                    sb.Append(metadata["dtsql"].ToString());
                }


                byte[] fileBytes = Encoding.ASCII.GetBytes(sb.ToString());
                string fileName = idinformation + ".sql";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //-- Metadata End --


    }
}