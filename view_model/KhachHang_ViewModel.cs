using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using qlks_app.commands;

namespace qlks_app.view_model
{
    public class KhachHang_ViewModel : BaseViewModel
    {
        private readonly QLKSEntities _db = new QLKSEntities();
        private int _tongSoPhong;
        private int _soPhongTrong;
        private double _giaToiThieu;
        private double _giaToiDa;
        private double _giaDaChonMin;
        private double _giaDaChonMax;
        private LoaiPhong _loaiPhongDaChon;
        private Phong _phongDangChon;
        private bool _hienThiForm;
        private DateTime? _ngayCheckin;
        private int _soDem;
        private string _hoTen;
        private string _dienThoai;
        private string _diaChi;
        private readonly List<Phong> _tatCaPhong = new List<Phong>();

        public ObservableCollection<Phong> DanhSachPhong { get; } = new ObservableCollection<Phong>();
        public ObservableCollection<LoaiPhong> DanhSachLoaiPhong { get; } = new ObservableCollection<LoaiPhong>();

        public int TongSoPhong
        {
            get { return _tongSoPhong; }
            set
            {
                _tongSoPhong = value;
                OnPropertyChanged(nameof(TongSoPhong));
            }
        }

        public Phong PhongDangChon
        {
            get { return _phongDangChon; }
            set
            {
                _phongDangChon = value;
                OnPropertyChanged(nameof(PhongDangChon));
            }
        }

        public bool HienThiForm
        {
            get { return _hienThiForm; }
            set
            {
                _hienThiForm = value;
                OnPropertyChanged(nameof(HienThiForm));
            }
        }

        public DateTime? NgayCheckin
        {
            get { return _ngayCheckin; }
            set
            {
                _ngayCheckin = value;
                OnPropertyChanged(nameof(NgayCheckin));
            }
        }

        public int SoDem
        {
            get { return _soDem; }
            set
            {
                _soDem = value;
                OnPropertyChanged(nameof(SoDem));
            }
        }

        public string HoTen
        {
            get { return _hoTen; }
            set
            {
                _hoTen = value;
                OnPropertyChanged(nameof(HoTen));
            }
        }

        public string DienThoai
        {
            get { return _dienThoai; }
            set
            {
                _dienThoai = value;
                OnPropertyChanged(nameof(DienThoai));
            }
        }

        public string DiaChi
        {
            get { return _diaChi; }
            set
            {
                _diaChi = value;
                OnPropertyChanged(nameof(DiaChi));
            }
        }

        public double GiaToiThieu
        {
            get { return _giaToiThieu; }
            set
            {
                _giaToiThieu = value;
                OnPropertyChanged(nameof(GiaToiThieu));
            }
        }

        public double GiaToiDa
        {
            get { return _giaToiDa; }
            set
            {
                _giaToiDa = value;
                OnPropertyChanged(nameof(GiaToiDa));
            }
        }

        public double GiaDaChonMin
        {
            get { return _giaDaChonMin; }
            set
            {
                _giaDaChonMin = value;
                OnPropertyChanged(nameof(GiaDaChonMin));
                ApDungBoLoc();
            }
        }

        public double GiaDaChonMax
        {
            get { return _giaDaChonMax; }
            set
            {
                _giaDaChonMax = value;
                OnPropertyChanged(nameof(GiaDaChonMax));
                ApDungBoLoc();
            }
        }

        public LoaiPhong LoaiPhongDaChon
        {
            get { return _loaiPhongDaChon; }
            set
            {
                _loaiPhongDaChon = value;
                OnPropertyChanged(nameof(LoaiPhongDaChon));
                ApDungBoLoc();
            }
        }

        public int SoPhongTrong
        {
            get { return _soPhongTrong; }
            set
            {
                _soPhongTrong = value;
                OnPropertyChanged(nameof(SoPhongTrong));
            }
        }

        public RelayCommand TaiLaiCommand { get; }
        public RelayCommand DongCommand { get; }
        public RelayCommand DangKyCommand { get; }
        public RelayCommand LuuDangKyCommand { get; }
        public RelayCommand HuyDangKyCommand { get; }

        public KhachHang_ViewModel()
        {
            TaiLaiCommand = new RelayCommand(o => TaiDuLieuPhong());
            DongCommand = new RelayCommand(o => (o as Window)?.Close());
            DangKyCommand = new RelayCommand(o => MoFormDangKy(), o => PhongDangChon != null);
            LuuDangKyCommand = new RelayCommand(o => LuuDangKy());
            HuyDangKyCommand = new RelayCommand(o => HuyDangKy());

            TaiDuLieuPhong();
        }

        private void TaiDuLieuPhong()
        {
            var phong = _db.Phongs
                .Include(p => p.LoaiPhong)
                .Include(p => p.DatPhongs)
                .Where(p => p.TrangThai == "Trống")
                .ToList();

            _tatCaPhong.Clear();
            _tatCaPhong.AddRange(phong);

            DanhSachLoaiPhong.Clear();
            DanhSachLoaiPhong.Add(new LoaiPhong { Id = 0, TenLoai = "Tất cả" });
            foreach (var item in _db.LoaiPhongs.ToList())
            {
                DanhSachLoaiPhong.Add(item);
            }

            LoaiPhongDaChon = DanhSachLoaiPhong.FirstOrDefault();

            var giaLonNhat = _tatCaPhong.Any() ? _tatCaPhong.Max(p => p.GiaMoiDem) : 0m;
            GiaToiThieu = 0;
            GiaToiDa = (double)giaLonNhat;
            GiaDaChonMin = GiaToiThieu;
            GiaDaChonMax = GiaToiDa;

            ApDungBoLoc();
        }

        private void ApDungBoLoc()
        {
            var giaMin = Math.Min(GiaDaChonMin, GiaDaChonMax);
            var giaMax = Math.Max(GiaDaChonMin, GiaDaChonMax);

            var phongLoc = _tatCaPhong.Where(p =>
                p.GiaMoiDem >= (decimal)giaMin &&
                p.GiaMoiDem <= (decimal)giaMax &&
                (LoaiPhongDaChon == null || LoaiPhongDaChon.Id == 0 || p.LoaiPhongId == LoaiPhongDaChon.Id));

            DanhSachPhong.Clear();
            foreach (var item in phongLoc)
            {
                DanhSachPhong.Add(item);
            }

            TongSoPhong = DanhSachPhong.Count;
            SoPhongTrong = _tatCaPhong.Count;
        }

        private void MoFormDangKy()
        {
            if (PhongDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để đăng ký");
                return;
            }

            NgayCheckin = DateTime.Today;
            SoDem = 1;
            HoTen = string.Empty;
            DienThoai = string.Empty;
            DiaChi = string.Empty;
            HienThiForm = true;
        }

        private void HuyDangKy()
        {
            HienThiForm = false;
        }

        private void LuuDangKy()
        {
            if (PhongDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để đăng ký");
                return;
            }

            if (!NgayCheckin.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày checkin");
                return;
            }

            if (SoDem <= 0)
            {
                MessageBox.Show("Số đêm phải lớn hơn 0");
                return;
            }

            if (string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(DienThoai))
            {
                MessageBox.Show("Vui lòng nhập tên và số điện thoại");
                return;
            }

            var khachHang = _db.KhachHangs.FirstOrDefault(kh => kh.DienThoai == DienThoai);
            if (khachHang == null)
            {
                khachHang = new KhachHang
                {
                    HoTen = HoTen,
                    DienThoai = DienThoai,
                    DiaChi = DiaChi
                };

                _db.KhachHangs.Add(khachHang);
                _db.SaveChanges();
            }

            var datPhong = new DatPhong
            {
                KhachHangId = khachHang.Id,
                PhongId = PhongDangChon.Id,
                NgayCheckin = NgayCheckin.Value,
                NgayCheckout = NgayCheckin.Value.AddDays(SoDem),
                TrangThai = "Chờ xác nhận",
                TongTien = PhongDangChon.GiaMoiDem * SoDem
            };

            _db.DatPhongs.Add(datPhong);
            PhongDangChon.TrangThai = "Đã đặt";
            _db.SaveChanges();

            HienThiForm = false;
            TaiDuLieuPhong();
        }
    }
}
