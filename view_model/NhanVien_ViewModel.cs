using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using qlks_app.commands;

namespace qlks_app.view_model
{
    public class NhanVien_ViewModel : BaseViewModel
    {
        private readonly QLKSEntities _db = new QLKSEntities();
        private Phong _phongDangChon;
        private DatPhong _datPhongDangChon;
        private DatPhong _datPhongTraDangChon;
        private bool _hienThiFormDatTrucTiep;
        private bool _hienThiChiTiet;
        private DateTime? _ngayCheckin;
        private int _soDem;
        private string _hoTen;
        private string _dienThoai;
        private string _diaChi;
        private DatPhong _datPhongChiTiet;
        private KhachHang _khachHangChiTiet;
        private object _current;

        public ObservableCollection<Phong> DanhSachPhongTrong { get; } = new ObservableCollection<Phong>();
        public ObservableCollection<DatPhong> DanhSachDatPhongChoXacNhan { get; } = new ObservableCollection<DatPhong>();
        public ObservableCollection<DatPhong> DanhSachDatPhongChoTra { get; } = new ObservableCollection<DatPhong>();

        public Phong PhongDangChon
        {
            get { return _phongDangChon; }
            set
            {
                _phongDangChon = value;
                OnPropertyChanged(nameof(PhongDangChon));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public DatPhong DatPhongDangChon
        {
            get { return _datPhongDangChon; }
            set
            {
                _datPhongDangChon = value;
                OnPropertyChanged(nameof(DatPhongDangChon));
                if (_datPhongDangChon == null)
                {
                    AnChiTiet();
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public DatPhong DatPhongTraDangChon
        {
            get { return _datPhongTraDangChon; }
            set
            {
                _datPhongTraDangChon = value;
                OnPropertyChanged(nameof(DatPhongTraDangChon));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool HienThiFormDatTrucTiep
        {
            get { return _hienThiFormDatTrucTiep; }
            set
            {
                _hienThiFormDatTrucTiep = value;
                OnPropertyChanged(nameof(HienThiFormDatTrucTiep));
            }
        }

        public bool HienThiChiTiet
        {
            get { return _hienThiChiTiet; }
            set
            {
                _hienThiChiTiet = value;
                OnPropertyChanged(nameof(HienThiChiTiet));
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

        public DatPhong DatPhongChiTiet
        {
            get { return _datPhongChiTiet; }
            set
            {
                _datPhongChiTiet = value;
                OnPropertyChanged(nameof(DatPhongChiTiet));
            }
        }

        public KhachHang KhachHangChiTiet
        {
            get { return _khachHangChiTiet; }
            set
            {
                _khachHangChiTiet = value;
                OnPropertyChanged(nameof(KhachHangChiTiet));
            }
        }

        public object Current
        {
            get { return _current; }
            set
            {
                _current = value;
                OnPropertyChanged(nameof(Current));
            }
        }

        public RelayCommand TaiLaiCommand { get; }
        public RelayCommand DongCommand { get; }
        public RelayCommand DangXuatCommand { get; }
        public RelayCommand MoDatTrucTiepCommand { get; }
        public RelayCommand LuuDatTrucTiepCommand { get; }
        public RelayCommand HuyDatTrucTiepCommand { get; }
        public RelayCommand ChiTietDatPhongCommand { get; }
        public RelayCommand XacNhanDatPhongCommand { get; }
        public RelayCommand DongChiTietCommand { get; }
        public RelayCommand TraPhongCommand { get; }
        public RelayCommand ShowDatPhongTrucTiepCommand { get; }
        public RelayCommand ShowXacNhanDatPhongCommand { get; }
        public RelayCommand ShowTraPhongCommand { get; }

        public NhanVien_ViewModel()
        {
            TaiLaiCommand = new RelayCommand(o => TaiDuLieu());
            DongCommand = new RelayCommand(o => (o as Window)?.Close());
            DangXuatCommand = new RelayCommand(o =>
            {
                new views.Login_View().Show();
                (o as Window)?.Close();
            });
            MoDatTrucTiepCommand = new RelayCommand(o => MoFormDatTrucTiep(), o => PhongDangChon != null);
            LuuDatTrucTiepCommand = new RelayCommand(o => LuuDatTrucTiep());
            HuyDatTrucTiepCommand = new RelayCommand(o => HuyDatTrucTiep());
            ChiTietDatPhongCommand = new RelayCommand(o => HienThiChiTietDatPhong(), o => DatPhongDangChon != null);
            XacNhanDatPhongCommand = new RelayCommand(o => XacNhanDatPhong(), o => DatPhongDangChon != null);
            DongChiTietCommand = new RelayCommand(o => AnChiTiet());
            TraPhongCommand = new RelayCommand(o => TraPhong(), o => DatPhongTraDangChon != null);

            ShowDatPhongTrucTiepCommand = new RelayCommand(o =>
            {
                Current = new views.NhanVienDatPhongTrucTiep_View();
            });

            ShowXacNhanDatPhongCommand = new RelayCommand(o =>
            {
                Current = new views.NhanVienXacNhanDatPhong_View();
            });

            ShowTraPhongCommand = new RelayCommand(o =>
            {
                Current = new views.NhanVienTraPhong_View();
            });

            TaiDuLieu();
            Current = new views.NhanVienDatPhongTrucTiep_View();
        }

        private void TaiDuLieu()
        {
            CapNhatTrangThaiCheckin();
            TaiPhongTrong();
            TaiDatPhongChoXacNhan();
            TaiDatPhongChoTra();
            AnChiTiet();
        }

        private void TaiPhongTrong()
        {
            var phongTrong = _db.Phongs
                .Include(p => p.LoaiPhong)
                .Where(p => p.TrangThai == "Trống")
                .ToList();

            DanhSachPhongTrong.Clear();
            foreach (var item in phongTrong)
            {
                DanhSachPhongTrong.Add(item);
            }
        }

        private void TaiDatPhongChoXacNhan()
        {
            var danhSach = _db.DatPhongs
                .Include(dp => dp.Phong)
                .Include("Phong.LoaiPhong")
                .Include(dp => dp.KhachHang)
                .Where(dp => dp.TrangThai == "Chờ xác nhận")
                .OrderByDescending(dp => dp.NgayCheckin)
                .ToList();

            DanhSachDatPhongChoXacNhan.Clear();
            foreach (var item in danhSach)
            {
                DanhSachDatPhongChoXacNhan.Add(item);
            }
        }

        private void TaiDatPhongChoTra()
        {
            var homNay = DateTime.Today;
            var danhSach = _db.DatPhongs
                .Include(dp => dp.Phong)
                .Include("Phong.LoaiPhong")
                .Include(dp => dp.KhachHang)
                .Where(dp => dp.TrangThai == "Đã nhận phòng" && DbFunctions.TruncateTime(dp.NgayCheckout) <= homNay)
                .OrderBy(dp => dp.NgayCheckout)
                .ToList();

            DanhSachDatPhongChoTra.Clear();
            foreach (var item in danhSach)
            {
                DanhSachDatPhongChoTra.Add(item);
            }

            DatPhongTraDangChon = null;
        }

        private void CapNhatTrangThaiCheckin()
        {
            var homNay = DateTime.Today;
            var datPhongCanCapNhat = _db.DatPhongs
                .Include(dp => dp.Phong)
                .Where(dp => dp.TrangThai == "Đã xác nhận" || dp.TrangThai == "Chờ xác nhận")
                .ToList();

            var coCapNhat = false;
            foreach (var datPhong in datPhongCanCapNhat)
            {
                if (string.Equals(datPhong.TrangThai, "Chờ xác nhận", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.Equals(datPhong.Phong.TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase))
                    {
                        datPhong.Phong.TrangThai = "Đã đặt";
                        coCapNhat = true;
                    }
                }

                if (string.Equals(datPhong.TrangThai, "Đã xác nhận", StringComparison.OrdinalIgnoreCase)
                    && datPhong.NgayCheckin.Date <= homNay
                    && !string.Equals(datPhong.Phong.TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase))
                {
                    datPhong.Phong.TrangThai = "Đang ở";
                    datPhong.Phong.NgayCheckin = datPhong.NgayCheckin;
                    datPhong.Phong.NgayCheckout = datPhong.NgayCheckout;
                    datPhong.TrangThai = "Đã nhận phòng";
                    coCapNhat = true;
                }
            }

            if (coCapNhat)
            {
                _db.SaveChanges();
            }
        }

        private void MoFormDatTrucTiep()
        {
            if (PhongDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để đặt");
                return;
            }

            NgayCheckin = DateTime.Today;
            SoDem = 1;
            HoTen = string.Empty;
            DienThoai = string.Empty;
            DiaChi = string.Empty;
            HienThiFormDatTrucTiep = true;
        }

        private void HuyDatTrucTiep()
        {
            HienThiFormDatTrucTiep = false;
        }

        private void LuuDatTrucTiep()
        {
            if (PhongDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để đặt");
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
                TrangThai = "Đã xác nhận",
                TongTien = PhongDangChon.GiaMoiDem * SoDem
            };

            _db.DatPhongs.Add(datPhong);
            PhongDangChon.TrangThai = "Đã đặt";
            _db.SaveChanges();

            HienThiFormDatTrucTiep = false;
            TaiDuLieu();
        }

        private void HienThiChiTietDatPhong()
        {
            if (DatPhongDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn đặt phòng để xem chi tiết");
                return;
            }

            DatPhongChiTiet = DatPhongDangChon;
            KhachHangChiTiet = DatPhongDangChon.KhachHang;
            HienThiChiTiet = true;
        }

        private void AnChiTiet()
        {
            HienThiChiTiet = false;
            DatPhongChiTiet = null;
            KhachHangChiTiet = null;
        }

        private void XacNhanDatPhong()
        {
            if (DatPhongDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn đặt phòng để xác nhận");
                return;
            }

            DatPhongDangChon.TrangThai = "Đã xác nhận";
            if (DatPhongDangChon.Phong != null)
            {
                DatPhongDangChon.Phong.TrangThai = "Đã đặt";
            }

            _db.SaveChanges();
            TaiDuLieu();
        }

        private void TraPhong()
        {
            if (DatPhongTraDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn phòng để trả");
                return;
            }

            DatPhongTraDangChon.TrangThai = "Đã trả phòng";
            if (DatPhongTraDangChon.Phong != null)
            {
                DatPhongTraDangChon.Phong.TrangThai = "Trống";
                DatPhongTraDangChon.Phong.NgayCheckin = null;
                DatPhongTraDangChon.Phong.NgayCheckout = null;
            }

            _db.SaveChanges();
            TaiDuLieu();
        }
    }
}
