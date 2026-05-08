using System;
using System.Windows.Media;

namespace qlks_app
{
    public partial class Phong
    {
        public Brush MauTrangThai
        {
            get
            {
                if (string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase))
                    return Brushes.Orange;
                if (string.Equals(TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase))
                    return Brushes.DodgerBlue;
                if (string.Equals(TrangThai, "Đang sửa chữa", StringComparison.OrdinalIgnoreCase))
                    return Brushes.Gray;

                return Brushes.Green;
            }
        }
    }
}
