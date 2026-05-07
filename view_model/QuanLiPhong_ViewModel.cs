using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using qlks_app.commands;
using qlks_app.models;

namespace qlks_app.view_model
{
    public class QuanLiPhong_ViewModel : BaseViewModel
    {
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

        public ObservableCollection<LoaiPhong> DanhSachLoaiPhong { get; } = new ObservableCollection<LoaiPhong>
        {
            new LoaiPhong { TenLoai = "Standard", GiaMoiDem = 500000, SucChua = 2, MoTa = "Phòng tiêu chuẩn" },
            new LoaiPhong { TenLoai = "Deluxe", GiaMoiDem = 800000, SucChua = 3, MoTa = "Phòng cao cấp" },
            new LoaiPhong { TenLoai = "VIP", GiaMoiDem = 1200000, SucChua = 4, MoTa = "Phòng VIP" },
            new LoaiPhong { TenLoai = "Suite", GiaMoiDem = 2000000, SucChua = 4, MoTa = "Phòng Suite" }
        };

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
                    GiaMoiDem = value.GiaMoiDem.ToString("N0");
                    SucChua = value.SucChua.ToString();
                    MoTa = value.MoTa;
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
                LoaiPhongDuocChon = DanhSachLoaiPhong.FirstOrDefault(p =>
                    string.Equals(p.TenLoai, PhongDangChon.LoaiPhong, StringComparison.OrdinalIgnoreCase));
                GiaMoiDem = PhongDangChon.GiaMoiDem;
                SucChua = PhongDangChon.SucChua;
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

                DanhSachPhong.Remove(PhongDangChon);
                CapNhatThongKe();
            });

            LuuPhongCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrWhiteSpace(SoPhong))
                {
                    MessageBox.Show("Vui lòng nhập số phòng");
                    return;
                }

                if (string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase) && !NgayCheckin.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn ngày khách đến checkin");
                    return;
                }

                if (NgayCheckin.HasValue && NgayCheckout.HasValue && NgayCheckout.Value.Date < NgayCheckin.Value.Date)
                {
                    MessageBox.Show("Ngày checkout phải sau hoặc bằng ngày checkin");
                    return;
                }

                if (_dangSua && PhongDangChon != null)
                {
                    PhongDangChon.SoPhong = SoPhong;
                    PhongDangChon.LoaiPhong = LoaiPhong;
                    PhongDangChon.GiaMoiDem = GiaMoiDem;
                    PhongDangChon.SucChua = SucChua;
                    PhongDangChon.MoTa = MoTa;
                    PhongDangChon.TrangThai = TrangThai;
                    PhongDangChon.NgayCheckin = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                        ? NgayCheckin
                        : null;
                    PhongDangChon.NgayCheckout = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                        ? NgayCheckout
                        : null;
                    PhongDangChon.MauTrangThai = MauTheoTrangThai(TrangThai);
                }
                else
                {
                    var nextId = DanhSachPhong.Count == 0 ? 1 : DanhSachPhong.Max(p => p.MaPhong) + 1;
                    DanhSachPhong.Add(new Phong
                    {
                        MaPhong = nextId,
                        SoPhong = SoPhong,
                        LoaiPhong = LoaiPhong,
                        GiaMoiDem = GiaMoiDem,
                        SucChua = SucChua,
                        MoTa = MoTa,
                        TrangThai = TrangThai,
                        NgayCheckin = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                            ? NgayCheckin
                            : null,
                        NgayCheckout = string.Equals(TrangThai, "Đã đặt", StringComparison.OrdinalIgnoreCase)
                            ? NgayCheckout
                            : null,
                        MauTrangThai = MauTheoTrangThai(TrangThai)
                    });
                }

                HienThiForm = false;
                CapNhatThongKe();
            });

            HuyPhongCommand = new RelayCommand(o =>
            {
                HienThiForm = false;
            });

            DanhSachPhong.Add(new Phong
            {
                MaPhong = 101,
                SoPhong = "101",
                LoaiPhong = "Phòng Standard",
                GiaMoiDem = "500.000",
                SucChua = "2",
                TrangThai = "Trống",
                MoTa = "Phòng tiêu chuẩn",
                NgayCheckin = null,
                NgayCheckout = null,
                MauTrangThai = MauTheoTrangThai("Trống")
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
