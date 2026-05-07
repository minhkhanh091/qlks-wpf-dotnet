using System;
using System.Windows.Media;

namespace qlks_app.models
{
    public class Phong
    {
        public int MaPhong { get; set; }
        public string SoPhong { get; set; }
        public string LoaiPhong { get; set; }
        public string GiaMoiDem { get; set; }
        public string SucChua { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayCheckin { get; set; }
        public DateTime? NgayCheckout { get; set; }
        public Brush MauTrangThai { get; set; }
    }
}
