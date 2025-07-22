namespace API02.Models
{
    public class LapStatusPenyampaian
    {
        public string Cakupan { get; set; }
        public string IdPelapor { get; set; }
        public string KelInformasi { get; set; }
        public string idinformation { get; set; }
        public string Periode { get; set; }
        public string Harian { get; set; }
        public string Minggu { get; set; }
        public string MBulan { get; set; }
        public string MTahun { get; set; }
        public string BBulan { get; set; }
        public string BTahun { get; set; }
        public string Triwulan { get; set; }
        public string TTahun { get; set; }
        public string Semester { get; set; }
        public string STahun { get; set; }
        public string TnTahun { get; set; }
        public string Tahunan { get; set; }
        public string Status { get; set; }
        public string wilayahkerja { get; set; }
        public string pdata_penyampaian { get; set; }
        public string TblName { get; set; }
        public string TblNameDtl { get; set; }
        public string username { get; set; }
        public string menuid { get; set; }
        public string fg { get; set; }

        //Add new atribute denda
        public string dateStart { get; set; }
        public string dateEnd { get; set; }
    }

    public class RptCols
    {
        public string Cols { get; set; }
        public string header { get; set; }
        public string header1 { get; set; }
    }
}
