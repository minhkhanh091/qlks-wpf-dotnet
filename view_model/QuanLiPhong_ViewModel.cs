using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using qlks_app.commands;

namespace qlks_app.view_model
{
    public class QuanLiPhong_ViewModel : BaseViewModel
    {
        private readonly QLKSEntities _db = new QLKSEntities();
        private bool _hienThiForm;
        private bool _dangSua;
        private Phong _phongDangChon;
        private string _soPhong;
        private string _loaiPhong;
        private LoaiPhong _loaiPhongDuocChon;
        private string _giaMoiDem;
        private string _sucChua;
        private string _moTa;
        private string _trangThai;
        private DateTime? _ngayCheckin;
        private DateTime? _ngayCheckout;
        private int _tongSoPhong;
        private int _soPhongTrong;
        private int _soPhongDaDat;
        private bool _choPhepChinhSuaNgay;

        public ObservableCollection<Phong> DanhSachPhong { get; } = new ObservableCollection<Phong>();

        public ObservableCollection<LoaiPhong> DanhSachLoaiPhong { get; } = new ObservableCollection<LoaiPhong>();

        public ObservableCollection<string> DanhSachTrangThai { get; } = new ObservableCollection<string>
        {
            "Trống",
            "Đã đặt",
            "Đang ở",
            "Đang sửa chữa"
        };

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

        public string SoPhong
        {
            get { return _soPhong; }
            set
            {
                _soPhong = value;
                OnPropertyChanged(nameof(SoPhong));
            }
        }

        public string LoaiPhong
        {
            get { return _loaiPhong; }
            set
            {
                _loaiPhong = value;
                OnPropertyChanged(nameof(LoaiPhong));
            }
        }

        public LoaiPhong LoaiPhongDuocChon
        {
            get { return _loaiPhongDuocChon; }
            set
            {
                _loaiPhongDuocChon = value;
                OnPropertyChanged(nameof(LoaiPhongDuocChon));

                if (value != null)
                {
                    LoaiPhong = value.TenLoai;
                    GiaMoiDem = value.Gia.ToString("N0", CultureInfo.CurrentCulture);
                    SucChua = value.SucChua.ToString();
                }
            }
        }

        public string GiaMoiDem
        {
            get { return _giaMoiDem; }
            set
            {
                _giaMoiDem = value;
                OnPropertyChanged(nameof(GiaMoiDem));
            }
        }

        public string SucChua
        {
            get { return _sucChua; }
            set
            {
                _sucChua = value;
                OnPropertyChanged(nameof(SucChua));
            }
        }

        public string MoTa
        {
            get { return _moTa; }
            set
            {
                _moTa = value;
                OnPropertyChanged(nameof(MoTa));
            }
        }

        public string TrangThai
        {
            get { return _trangThai; }
            set
            {
                _trangThai = value;
                OnPropertyChanged(nameof(TrangThai));
                CapNhatChoPhepChinhSuaNgay();
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

        public DateTime? NgayCheckout
        {
            get { return _ngayCheckout; }
            set
            {
                _ngayCheckout = value;
                OnPropertyChanged(nameof(NgayCheckout));
            }
        }

        public bool ChoPhepChinhSuaNgay
        {
            get { return _choPhepChinhSuaNgay; }
            set
            {
                _choPhepChinhSuaNgay = value;
                OnPropertyChanged(nameof(ChoPhepChinhSuaNgay));
            }
        }

        public int TongSoPhong
        {
            get { return _tongSoPhong; }
            set
            {
                _tongSoPhong = value;
                OnPropertyChanged(nameof(TongSoPhong));
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

        public int SoPhongDaDat
        {
            get { return _soPhongDaDat; }
            set
            {
                _soPhongDaDat = value;
                OnPropertyChanged(nameof(SoPhongDaDat));
            }
        }

        public RelayCommand ThemPhongCommand { get; }
        public RelayCommand SuaPhongCommand { get; }
        public RelayCommand XoaPhongCommand { get; }
        public RelayCommand LuuPhongCommand { get; }
        public RelayCommand HuyPhongCommand { get; }

        public QuanLiPhong_ViewModel()
        {
            TaiDuLieuLoaiPhong();
            TaiDuLieuPhong();

            ThemPhongCommand = new RelayCommand(o =>
            {
                _dangSua = false;
                XoaForm();
                HienThiForm = true;
                CapNhatChoPhepChinhSuaNgay();
            });

            SuaPhongCommand = new RelayCommand(o =>
            {
                if (PhongDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng để sửa");
                    return;
                }

                _dangSua = true;
                SoPhong = PhongDangChon.SoPhong;
                LoaiPhongDuocChon = DanhSachLoaiPhong.FirstOrDefault(p => p.Id == PhongDangChon.LoaiPhongId);
                GiaMoiDem = PhongDangChon.GiaMoiDem.ToString("N0", CultureInfo.CurrentCulture);
                SucChua = PhongDangChon.SucChua.ToString();
                MoTa = PhongDangChon.MoTa;
                TrangThai = PhongDangChon.TrangThai;
                NgayCheckin = PhongDangChon.NgayCheckin;
                NgayCheckout = PhongDangChon.NgayCheckout;
                HienThiForm = true;
                CapNhatChoPhepChinhSuaNgay();
            });

            XoaPhongCommand = new RelayCommand(o =>
            {
                if (PhongDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng để xóa");
                    return;
                }

                if (!string.Equals(PhongDangChon.TrangThai, "Trống", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Chỉ được xóa phòng đang trống");
                    return;
                }

                if (PhongDangChon.DatPhongs != null && PhongDangChon.DatPhongs.Any())
                {
                    MessageBox.Show("Không thể xóa phòng đã có lịch đặt");
                    return;
                }

                _db.Phongs.Remove(PhongDangChon);
                _db.SaveChanges();
                TaiDuLieuPhong();
            });

            LuuPhongCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrWhiteSpace(SoPhong))
                {
                    MessageBox.Show("Vui lòng nhập số phòng");
                    return;
                }

                if ((string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase))
                    && !NgayCheckin.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn ngày khách đến checkin");
                    return;
                }

                if (NgayCheckin.HasValue && NgayCheckout.HasValue && NgayCheckout.Value.Date < NgayCheckin.Value.Date)
                {
                    MessageBox.Show("Ngày checkout phải sau hoặc bằng ngày checkin");
                    return;
                }

                if (LoaiPhongDuocChon == null)
                {
                    MessageBox.Show("Vui lòng chọn loại phòng");
                    return;
                }

                if (!decimal.TryParse(GiaMoiDem, NumberStyles.Any, CultureInfo.CurrentCulture, out var giaMoiDem))
                {
                    MessageBox.Show("Giá mỗi đêm không hợp lệ");
                    return;
                }

                if (!int.TryParse(SucChua, out var sucChua))
                {
                    MessageBox.Show("Sức chứa không hợp lệ");
                    return;
                }

                if (_dangSua && PhongDangChon != null)
                {
                    PhongDangChon.SoPhong = SoPhong;
                    PhongDangChon.LoaiPhongId = LoaiPhongDuocChon.Id;
                    PhongDangChon.GiaMoiDem = giaMoiDem;
                    PhongDangChon.SucChua = sucChua;
                    PhongDangChon.MoTa = MoTa;
                    PhongDangChon.TrangThai = TrangThai;
                    PhongDangChon.NgayCheckin = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase)
                        ? NgayCheckin
                        : null;
                    PhongDangChon.NgayCheckout = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase)
                        ? NgayCheckout
                        : null;
                    _db.SaveChanges();
                }
                else
                {
                    var phongMoi = new Phong
                    {
                        SoPhong = SoPhong,
                        LoaiPhongId = LoaiPhongDuocChon.Id,
                        GiaMoiDem = giaMoiDem,
                        SucChua = sucChua,
                        MoTa = MoTa,
                        TrangThai = TrangThai,
                        NgayCheckin = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase)
                            ? NgayCheckin
                            : null,
                        NgayCheckout = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase)
                            ? NgayCheckout
                            : null
                    };

                    _db.Phongs.Add(phongMoi);
                    _db.SaveChanges();
                }

                HienThiForm = false;
                TaiDuLieuPhong();
            });

            HuyPhongCommand = new RelayCommand(o =>
            {
                HienThiForm = false;
            });

            CapNhatThongKe();
        }

        private void XoaForm()
        {
            SoPhong = string.Empty;
            LoaiPhongDuocChon = DanhSachLoaiPhong.FirstOrDefault();
            TrangThai = DanhSachTrangThai.FirstOrDefault();
            NgayCheckin = null;
            NgayCheckout = null;
            CapNhatChoPhepChinhSuaNgay();
        }

        private void TaiDuLieuLoaiPhong()
        {
            var loaiPhong = _db.LoaiPhongs.ToList();
            DanhSachLoaiPhong.Clear();
            foreach (var item in loaiPhong)
            {
                DanhSachLoaiPhong.Add(item);
            }

            LoaiPhongDuocChon = DanhSachLoaiPhong.FirstOrDefault();
        }

        private void TaiDuLieuPhong()
        {
            var phong = _db.Phongs
                .Include(p => p.LoaiPhong)
                .Include(p => p.DatPhongs)
                .ToList();

            DanhSachPhong.Clear();
            foreach (var item in phong)
            {
                if (string.Equals(item.TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(item.TrangThai, "Đang ở", StringComparison.OrdinalIgnoreCase))
                {
                    var datPhong = item.DatPhongs
                        .OrderByDescending(dp => dp.NgayCheckin)
                        .FirstOrDefault();

                    if (datPhong != null)
                    {
                        item.NgayCheckin = datPhong.NgayCheckin;
                        item.NgayCheckout = datPhong.NgayCheckout;
                    }
                }

                DanhSachPhong.Add(item);
            }

            CapNhatThongKe();
        }

        private void CapNhatThongKe()
        {
            TongSoPhong = DanhSachPhong.Count;
            SoPhongTrong = DanhSachPhong.Count(p => string.Equals(p.TrangThai, "Trống", StringComparison.OrdinalIgnoreCase));
            SoPhongDaDat = DanhSachPhong.Count(p => string.Equals(p.TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase));
        }

        private Brush MauTheoTrangThai(string trangThai)
        {
            if (string.Equals(trangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase))
                return Brushes.Orange;
            if (string.Equals(trangThai, "Đang ở", StringComparison.OrdinalIgnoreCase))
                return Brushes.DodgerBlue;
            if (string.Equals(trangThai, "Đang sửa chữa", StringComparison.OrdinalIgnoreCase))
                return Brushes.Gray;

            return Brushes.Green;
        }

        private void CapNhatChoPhepChinhSuaNgay()
        {
            ChoPhepChinhSuaNgay = _dangSua;
        }
    }

}
