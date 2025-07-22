using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace Antasena.Models.Libs
{
    public class lTipePeriode
    {
        public DataTable GetListTipePeriode(string FgType)
        {

            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            SqlCommand cmdA = new SqlCommand("tp_getTipePeriode", myConn);
            cmdA.Parameters.Add("@FgType", FgType);
            cmdA.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmdA);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            myConn.Close();
            return ds.Tables[0];

        }
        public DataTable GetList(string id)
        {

            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            SqlCommand cmdA = new SqlCommand("tp_getAllTipePeriodeByID", myConn);
            cmdA.Parameters.Add("@PeriodId", SqlDbType.Int).Value = Int32.Parse(id);
            cmdA.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmdA);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            myConn.Close();
            return ds.Tables[0];

        }

        public DataTable Delete(TipePeriode Model, string pUser)
        {
            string sp = (Model.FgType == "E") ? "tp_deleteTipePeriode_EX" : "tp_deleteTipePeriode";
            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            myConn.Open();
            SqlCommand cmd = new SqlCommand(sp, myConn);
            cmd.Parameters.AddWithValue("@PeriodId", Model.PeriodId);
            cmd.Parameters.AddWithValue("@by", pUser);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            myConn.Close();
            return ds.Tables[0];
            //cmd.CommandType = CommandType.StoredProcedure;
            //int affectedrow = cmd.ExecuteNonQuery();
            //myConn.Close();
            //return affectedrow;
        }

        public bool Insert(TipePeriode Model, string pUser)
        {
            var ireturn = false;
            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            myConn.Open();
            SqlCommand cmd = new SqlCommand("tp_insertTipePeriode", myConn);
            setParam(cmd, Model, "Insert", pUser);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            myConn.Close();
            var ids = ds.Tables[0];

            if (Convert.ToInt32(ids.Rows[0]["isexists"]) > 0)
            {
                ireturn = true;
            }
            //int affectedrow = cmd.ExecuteNonQuery();
            //myConn.Close();

            return ireturn;
        }

        public int Update(TipePeriode Model, string pUser)
        {
            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            myConn.Open();
            SqlCommand cmd = new SqlCommand("tp_updateTipePeriode", myConn);
            setParam(cmd, Model, "Update", pUser);

            cmd.CommandType = CommandType.StoredProcedure;
            int affectedrow = cmd.ExecuteNonQuery();
            myConn.Close();
            return affectedrow;
        }

        public string setParam(SqlCommand cmd, TipePeriode Model, string Param, string pUser)
        {
            if (Param != "Insert")
            {
                cmd.Parameters.Add("@PeriodId", SqlDbType.Int).Value = Model.PeriodId;
            }
            cmd.Parameters.AddWithValue("@PeriodName", ((Model.PeriodName == null) ? "" : Model.PeriodName));
            cmd.Parameters.AddWithValue("@PeriodType", ((Model.PeriodType == null) ? "" : Model.PeriodType));
            cmd.Parameters.AddWithValue("@fgmove", ((Model.fgmove == null) ? "" : Model.fgmove));

            cmd.Parameters.AddWithValue("@tipeharian", ((Model.tipeharian == null) ? "" : Model.tipeharian));
            cmd.Parameters.AddWithValue("@JamBukaPelaporan", ((Model.JamBukaPelaporan == null) ? "" : Model.JamBukaPelaporan));
            cmd.Parameters.AddWithValue("@JamTutupAtasPelaporan", ((Model.JamTutupAtasPelaporan == null) ? "" : Model.JamTutupAtasPelaporan));
            cmd.Parameters.AddWithValue("@UseKoreksi", ((Model.UseKoreksi == null) ? "" : Model.UseKoreksi));
            cmd.Parameters.AddWithValue("@JumlahHariKoreksi", ((Model.JumlahHariKoreksi == null) ? "" : Model.JumlahHariKoreksi));
            cmd.Parameters.AddWithValue("@JamBukaKoreksi", ((Model.JamBukaKoreksi == null) ? "" : Model.JamBukaKoreksi));
            cmd.Parameters.AddWithValue("@JamTutupAtasKoreksi", ((Model.JamTutupAtasKoreksi == null) ? "" : Model.JamTutupAtasKoreksi));
            cmd.Parameters.AddWithValue("@JumlahHariPelaporan", ((String.IsNullOrEmpty(Model.JumlahHariPelaporan)) ? 0 : Int32.Parse(Model.JumlahHariPelaporan)));

            cmd.Parameters.AddWithValue("@Minggu1MulaiBatasPelaporan", ((Model.Minggu1MulaiBatasPelaporan == null) ? "" : Model.Minggu1MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu1AkhirBatasPelaporan", ((Model.Minggu1AkhirBatasPelaporan == null) ? "" : Model.Minggu1AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu2MulaiBatasPelaporan", ((Model.Minggu2MulaiBatasPelaporan == null) ? "" : Model.Minggu2MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu2AkhirBatasPelaporan", ((Model.Minggu2AkhirBatasPelaporan == null) ? "" : Model.Minggu2AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu3MulaiBatasPelaporan", ((Model.Minggu3MulaiBatasPelaporan == null) ? "" : Model.Minggu3MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu3AkhirBatasPelaporan", ((Model.Minggu3AkhirBatasPelaporan == null) ? "" : Model.Minggu3AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu4MulaiBatasPelaporan", ((Model.Minggu4MulaiBatasPelaporan == null) ? "" : Model.Minggu4MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Minggu4AkhirBatasPelaporan", ((Model.Minggu4AkhirBatasPelaporan == null) ? "" : Model.Minggu4AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@MPeriodeData2", ((Model.MPeriodeData2 == null) ? "" : Model.MPeriodeData2));
            cmd.Parameters.AddWithValue("@MPeriodeData3", ((Model.MPeriodeData3 == null) ? "" : Model.MPeriodeData3));
            cmd.Parameters.AddWithValue("@MPeriodeData4", ((Model.MPeriodeData4 == null) ? "" : Model.MPeriodeData4));
            cmd.Parameters.AddWithValue("@MBatasKeterlambatan", ((Model.MBatasKeterlambatan == null) ? "" : Model.MBatasKeterlambatan));

            cmd.Parameters.AddWithValue("@MulaiBatasPelaporan", ((Model.MulaiBatasPelaporan == null) ? "" : Model.MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@AkhirBatasPelaporan", ((Model.AkhirBatasPelaporan == null) ? "" : Model.AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@BBatasKeterlambatan", ((Model.BBatasKeterlambatan == null) ? "" : Model.BBatasKeterlambatan));

            cmd.Parameters.AddWithValue("@Triwulan1MulaiBatasPelaporan", ((Model.Triwulan1MulaiBatasPelaporan == null) ? "" : Model.Triwulan1MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan1AkhirBatasPelaporan", ((Model.Triwulan1AkhirBatasPelaporan == null) ? "" : Model.Triwulan1AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan2MulaiBatasPelaporan", ((Model.Triwulan2MulaiBatasPelaporan == null) ? "" : Model.Triwulan2MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan2AkhirBatasPelaporan", ((Model.Triwulan2AkhirBatasPelaporan == null) ? "" : Model.Triwulan2AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan3MulaiBatasPelaporan", ((Model.Triwulan3MulaiBatasPelaporan == null) ? "" : Model.Triwulan3MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan3AkhirBatasPelaporan", ((Model.Triwulan3AkhirBatasPelaporan == null) ? "" : Model.Triwulan3AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan4MulaiBatasPelaporan", ((Model.Triwulan4MulaiBatasPelaporan == null) ? "" : Model.Triwulan4MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Triwulan4AkhirBatasPelaporan", ((Model.Triwulan4AkhirBatasPelaporan == null) ? "" : Model.Triwulan4AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@TBatasKeterlambatan", ((Model.TBatasKeterlambatan == null) ? "" : Model.TBatasKeterlambatan));

            cmd.Parameters.AddWithValue("@Semester1MulaiBatasPelaporan", ((Model.Semester1MulaiBatasPelaporan == null) ? "" : Model.Semester1MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Semester1AkhirBatasPelaporan", ((Model.Semester1AkhirBatasPelaporan == null) ? "" : Model.Semester1AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@Semester2MulaiBatasPelaporan", ((Model.Semester2MulaiBatasPelaporan == null) ? "" : Model.Semester2MulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@Semester2AkhirBatasPelaporan", ((Model.Semester2AkhirBatasPelaporan == null) ? "" : Model.Semester2AkhirBatasPelaporan));
            cmd.Parameters.AddWithValue("@BatasKeterlambatan", ((Model.BatasKeterlambatan == null) ? "" : Model.BatasKeterlambatan));

            cmd.Parameters.AddWithValue("@TahunMulaiBatasPelaporan", ((Model.TahunMulaiBatasPelaporan == null) ? "" : Model.TahunMulaiBatasPelaporan));
            cmd.Parameters.AddWithValue("@TahunAkhirBatasPelaporan", ((Model.TahunAkhirBatasPelaporan == null) ? "" : Model.TahunAkhirBatasPelaporan));


            cmd.Parameters.AddWithValue("@by", pUser);
            return "";
        }


        public JObject Insert_EX(TipePeriode_EXC Model, string pUser)
        {
            var jreturn = new JObject();
            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            myConn.Open();
            SqlCommand cmd = new SqlCommand("tp_insertTipePeriode_EX", myConn);
            setParam_EX(cmd, Model, "Insert", pUser);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            myConn.Close();
            var ids = ds.Tables[0];

            if (Convert.ToInt32(ids.Rows[0]["isexists"]) > 0)
            {
                jreturn.Add("success", false);
                jreturn.Add("message", ids.Rows[0]["msg"].ToString());
            }
            else
            {
                jreturn.Add("success", true);
            }

            return jreturn;
        }
        public int Update_EX(TipePeriode_EXC Model, string pUser)
        {
            DbConn myDB = new DbConn();
            SqlConnection myConn;
            myConn = myDB.getConnString();
            myConn.Open();
            SqlCommand cmd = new SqlCommand("tp_updateTipePeriode_EX", myConn);
            setParam_EX(cmd, Model, "Update", pUser);

            cmd.CommandType = CommandType.StoredProcedure;
            int affectedrow = cmd.ExecuteNonQuery();
            myConn.Close();
            return affectedrow;
        }

        public string setParam_EX(SqlCommand cmd, TipePeriode_EXC Model, string Param, string pUser)
        {
            if (Param != "Insert")
            {
                cmd.Parameters.Add("@PeriodId", SqlDbType.Int).Value = Model.PeriodId;
            }
            cmd.Parameters.AddWithValue("@PeriodType", ((Model.PeriodType == null) ? "" : Model.PeriodType));
            cmd.Parameters.AddWithValue("@PeriodName", ((Model.PeriodName == null) ? "" : Model.PeriodName));
            cmd.Parameters.AddWithValue("@PeriodName_Ori", ((Model.PeriodName_Ori == null) ? "" : Model.PeriodName_Ori));
            cmd.Parameters.AddWithValue("@fgmove", ((Model.fgmove == null) ? "" : Model.fgmove));

            cmd.Parameters.AddWithValue("@PeriodData_H", ((Model.PeriodData_H == null) ? "" : Model.PeriodData_H));
            cmd.Parameters.AddWithValue("@TglBuka_H", ((Model.TglBuka_H == null) ? "" : Model.TglBuka_H));
            cmd.Parameters.AddWithValue("@JamBuka_H", ((Model.JamBuka_H == null) ? "" : Model.JamBuka_H));
            cmd.Parameters.AddWithValue("@TglTutup_H", ((Model.TglTutup_H == null) ? "" : Model.TglTutup_H));
            cmd.Parameters.AddWithValue("@JamTutup_H", ((Model.JamTutup_H == null) ? "" : Model.JamTutup_H));
            cmd.Parameters.AddWithValue("@UseKoreksi", ((Model.UseKoreksi == null) ? "" : Model.UseKoreksi));
            cmd.Parameters.AddWithValue("@JumlahHariKoreksi", ((Model.JumlahHariKoreksi == null) ? "" : Model.JumlahHariKoreksi));
            cmd.Parameters.AddWithValue("@JamBukaKoreksi", ((Model.JamBukaKoreksi == null) ? "" : Model.JamBukaKoreksi));
            cmd.Parameters.AddWithValue("@JamTutupKoreksi", ((Model.JamTutupKoreksi == null) ? "" : Model.JamTutupKoreksi));

            cmd.Parameters.AddWithValue("@PD_Minggu_M", ((Model.PD_Minggu_M == null) ? "" : Model.PD_Minggu_M));
            cmd.Parameters.AddWithValue("@PD_Bulan_M", ((Model.PD_Bulan_M == null) ? "" : Model.PD_Bulan_M));
            cmd.Parameters.AddWithValue("@PD_Tahun_M", ((Model.PD_Tahun_M == null) ? "" : Model.PD_Tahun_M));
            cmd.Parameters.AddWithValue("@TglMulai_M", ((Model.TglMulai_M == null) ? "" : Model.TglMulai_M));
            cmd.Parameters.AddWithValue("@TambahBulanMulai_M", ((Model.TambahBulanMulai_M == null) ? "" : Model.TambahBulanMulai_M));
            cmd.Parameters.AddWithValue("@TglAkhir_M", ((Model.TglAkhir_M == null) ? "" : Model.TglAkhir_M));
            cmd.Parameters.AddWithValue("@TambahBulanAkhir_M", ((Model.TambahBulanAkhir_M == null) ? "" : Model.TambahBulanAkhir_M));
            cmd.Parameters.AddWithValue("@BatasKeterlambatan_M", ((Model.BatasKeterlambatan_M == null) ? "" : Model.BatasKeterlambatan_M));

            cmd.Parameters.AddWithValue("@PD_Bulan_B", ((Model.PD_Bulan_B == null) ? "" : Model.PD_Bulan_B));
            cmd.Parameters.AddWithValue("@PD_Tahun_B", ((Model.PD_Tahun_B == null) ? "" : Model.PD_Tahun_B));
            cmd.Parameters.AddWithValue("@TglMulai_B", ((Model.TglMulai_B == null) ? "" : Model.TglMulai_B));
            cmd.Parameters.AddWithValue("@TglAkhir_B", ((Model.TglAkhir_B == null) ? "" : Model.TglAkhir_B));
            cmd.Parameters.AddWithValue("@TambahBulan_B", ((Model.TambahBulan_B == null) ? "" : Model.TambahBulan_B));
            cmd.Parameters.AddWithValue("@BatasKeterlambatan_B", ((Model.BatasKeterlambatan_B == null) ? "" : Model.BatasKeterlambatan_B));

            cmd.Parameters.AddWithValue("@PD_Bulan_T", ((Model.PD_Bulan_T == null) ? "" : Model.PD_Bulan_T));
            cmd.Parameters.AddWithValue("@PD_Tahun_T", ((Model.PD_Tahun_T == null) ? "" : Model.PD_Tahun_T));
            cmd.Parameters.AddWithValue("@TglMulai_T", ((Model.TglMulai_T == null) ? "" : Model.TglMulai_T));
            cmd.Parameters.AddWithValue("@TglAkhir_T", ((Model.TglAkhir_T == null) ? "" : Model.TglAkhir_T));
            cmd.Parameters.AddWithValue("@TambahBulan_T", ((Model.TambahBulan_T == null) ? "" : Model.TambahBulan_T));
            cmd.Parameters.AddWithValue("@BatasKeterlambatan_T", ((Model.BatasKeterlambatan_T == null) ? "" : Model.BatasKeterlambatan_T));

            cmd.Parameters.AddWithValue("@PD_Bulan_S", ((Model.PD_Bulan_S == null) ? "" : Model.PD_Bulan_S));
            cmd.Parameters.AddWithValue("@PD_Tahun_S", ((Model.PD_Tahun_S == null) ? "" : Model.PD_Tahun_S));
            cmd.Parameters.AddWithValue("@TglMulai_S", ((Model.TglMulai_S == null) ? "" : Model.TglMulai_S));
            cmd.Parameters.AddWithValue("@TglAkhir_S", ((Model.TglAkhir_S == null) ? "" : Model.TglAkhir_S));
            cmd.Parameters.AddWithValue("@TambahBulan_S", ((Model.TambahBulan_S == null) ? "" : Model.TambahBulan_S));
            cmd.Parameters.AddWithValue("@BatasKeterlambatan_S", ((Model.BatasKeterlambatan_S == null) ? "" : Model.BatasKeterlambatan_S));

            cmd.Parameters.AddWithValue("@PD_Tahun_TH", ((Model.PD_Tahun_TH == null) ? "" : Model.PD_Tahun_TH));
            cmd.Parameters.AddWithValue("@TglMulai_TH", ((Model.TglMulai_TH == null) ? "" : Model.TglMulai_TH));
            cmd.Parameters.AddWithValue("@TglAkhir_TH", ((Model.TglAkhir_TH == null) ? "" : Model.TglAkhir_TH));
            cmd.Parameters.AddWithValue("@TambahBulan_TH", ((Model.TambahBulan_TH == null) ? "" : Model.TambahBulan_TH));
            cmd.Parameters.AddWithValue("@BatasKeterlambatan_TH", ((Model.BatasKeterlambatan_TH == null) ? "" : Model.BatasKeterlambatan_TH));

            cmd.Parameters.AddWithValue("@by", pUser);
            return "";
        }


    }
}