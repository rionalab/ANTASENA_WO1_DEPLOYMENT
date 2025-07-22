namespace Antasena.Models
{
    public class TipePeriode
    {
        public int? PeriodId { get; set; }
        public string PeriodName { get; set; }
        public string PeriodType { get; set; }
        //public string TipeHari { get; set; }
        public string fgmove { get; set; }

        public string tipeharian { get; set; }
        public string JamBukaPelaporan { get; set; }
        public string JamTutupAtasPelaporan { get; set; }
        public string UseKoreksi { get; set; }
        public string JumlahHariKoreksi { get; set; }
        public string JamBukaKoreksi { get; set; }
        public string JamTutupAtasKoreksi { get; set; }
        public string JumlahHariPelaporan { get; set; }

        public string Minggu1MulaiBatasPelaporan { get; set; }
        public string Minggu1AkhirBatasPelaporan { get; set; }
        public string Minggu2MulaiBatasPelaporan { get; set; }
        public string Minggu2AkhirBatasPelaporan { get; set; }
        public string Minggu3MulaiBatasPelaporan { get; set; }
        public string Minggu3AkhirBatasPelaporan { get; set; }
        public string Minggu4MulaiBatasPelaporan { get; set; }
        public string Minggu4AkhirBatasPelaporan { get; set; }
        public string MPeriodeData2 { get; set; }
        public string MPeriodeData3 { get; set; }
        public string MPeriodeData4 { get; set; }
        public string MBatasKeterlambatan { get; set; }

        public string MulaiBatasPelaporan { get; set; }
        public string AkhirBatasPelaporan { get; set; }
        public string BBatasKeterlambatan { get; set; }

        public string Triwulan1MulaiBatasPelaporan { get; set; }
        public string Triwulan1AkhirBatasPelaporan { get; set; }
        public string Triwulan2MulaiBatasPelaporan { get; set; }
        public string Triwulan2AkhirBatasPelaporan { get; set; }
        public string Triwulan3MulaiBatasPelaporan { get; set; }
        public string Triwulan3AkhirBatasPelaporan { get; set; }
        public string Triwulan4MulaiBatasPelaporan { get; set; }
        public string Triwulan4AkhirBatasPelaporan { get; set; }
        public string TBatasKeterlambatan { get; set; }

        // Semesteran
        public string Semester1MulaiBatasPelaporan { get; set; }
        public string Semester1AkhirBatasPelaporan { get; set; }
        public string Semester2MulaiBatasPelaporan { get; set; }
        public string Semester2AkhirBatasPelaporan { get; set; }

        //Tahunan
        public string TahunMulaiBatasPelaporan { get; set; }
        public string TahunAkhirBatasPelaporan { get; set; }

        public string BatasKeterlambatan { get; set; }
        public string FgType { get; set; }
    }

    public class TipePeriode_EXC
    {
        public int? PeriodId { get; set; }
        public string PeriodType { get; set; }
        public string PeriodName { get; set; }
        public string fgmove { get; set; }
        public string PeriodName_Ori { get; set; }

        //Harian
        public string PeriodData_H { get; set; }
        public string TglBuka_H { get; set; }
        public string JamBuka_H { get; set; }
        public string TglTutup_H { get; set; }
        public string JamTutup_H { get; set; }
        public string UseKoreksi { get; set; }
        public string JumlahHariKoreksi { get; set; }
        public string JamBukaKoreksi { get; set; }
        public string JamTutupKoreksi { get; set; }

        //Mingguan
        public string PD_Minggu_M { get; set; }
        public string PD_Bulan_M { get; set; }
        public string PD_Tahun_M { get; set; }
        public string TglMulai_M { get; set; }
        public string TambahBulanMulai_M { get; set; }
        public string TglAkhir_M { get; set; }
        public string TambahBulanAkhir_M { get; set; }
        public string BatasKeterlambatan_M { get; set; }

        //Bulan
        public string PD_Bulan_B { get; set; }
        public string PD_Tahun_B { get; set; }
        public string TglMulai_B { get; set; }
        public string TglAkhir_B { get; set; }
        public string TambahBulan_B { get; set; }
        public string BatasKeterlambatan_B { get; set; }

        //Triwulan
        public string PD_Bulan_T { get; set; }
        public string PD_Tahun_T { get; set; }
        public string TglMulai_T { get; set; }
        public string TglAkhir_T { get; set; }
        public string TambahBulan_T { get; set; }
        public string BatasKeterlambatan_T { get; set; }

        // Semesteran
        public string PD_Bulan_S { get; set; }
        public string PD_Tahun_S { get; set; }
        public string TglMulai_S { get; set; }
        public string TglAkhir_S { get; set; }
        public string TambahBulan_S { get; set; }
        public string BatasKeterlambatan_S { get; set; }

        // Tahunan
        public string PD_Tahun_TH { get; set; }
        public string TglMulai_TH { get; set; }
        public string TglAkhir_TH { get; set; }
        public string TambahBulan_TH { get; set; }
        public string BatasKeterlambatan_TH { get; set; }

    }

}