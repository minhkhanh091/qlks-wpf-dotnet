using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using qlks_app.commands;
using qlks_app.models;

namespace qlks_app.view_model
{
    public class NhanVienManagement_ViewModel : BaseViewModel
    {
        private bool _hienThiForm;
        private bool _dangSua;
        private NhanVien _nhanVienDangChon;
        private string _tenNhanVien;
        private string _soDienThoai;
        private string _hinhAnh;

        public ObservableCollection<NhanVien> DanhSachNhanVien { get; } = new ObservableCollection<NhanVien>();

        public NhanVien NhanVienDangChon
        {
            get { return _nhanVienDangChon; }
            set
            {
                _nhanVienDangChon = value;
                OnPropertyChanged(nameof(NhanVienDangChon));
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

        public string TenNhanVien
        {
            get { return _tenNhanVien; }
            set
            {
                _tenNhanVien = value;
                OnPropertyChanged(nameof(TenNhanVien));
            }
        }

        public string SoDienThoai
        {
            get { return _soDienThoai; }
            set
            {
                _soDienThoai = value;
                OnPropertyChanged(nameof(SoDienThoai));
            }
        }

        public string HinhAnh
        {
            get { return _hinhAnh; }
            set
            {
                _hinhAnh = value;
                OnPropertyChanged(nameof(HinhAnh));
            }
        }

        public RelayCommand ThemNhanVienCommand { get; }
        public RelayCommand SuaNhanVienCommand { get; }
        public RelayCommand XoaNhanVienCommand { get; }
        public RelayCommand LuuNhanVienCommand { get; }
        public RelayCommand HuyNhanVienCommand { get; }

        public NhanVienManagement_ViewModel()
        {
            ThemNhanVienCommand = new RelayCommand(o =>
            {
                _dangSua = false;
                XoaForm();
                HienThiForm = true;
            });

            SuaNhanVienCommand = new RelayCommand(o =>
            {
                if (NhanVienDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên để sửa");
                    return;
                }

                _dangSua = true;
                TenNhanVien = NhanVienDangChon.TenNhanVien;
                SoDienThoai = NhanVienDangChon.SoDienThoai;
                HinhAnh = NhanVienDangChon.HinhAnh;
                HienThiForm = true;
            });

            XoaNhanVienCommand = new RelayCommand(o =>
            {
                if (NhanVienDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên để xóa");
                    return;
                }

                DanhSachNhanVien.Remove(NhanVienDangChon);
            });

            LuuNhanVienCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrWhiteSpace(TenNhanVien))
                {
                    MessageBox.Show("Vui lòng nhập tên nhân viên");
                    return;
                }

                if (_dangSua && NhanVienDangChon != null)
                {
                    NhanVienDangChon.TenNhanVien = TenNhanVien;
                    NhanVienDangChon.SoDienThoai = SoDienThoai;
                    NhanVienDangChon.HinhAnh = HinhAnh;
                }
                else
                {
                    var nextId = DanhSachNhanVien.Count == 0 ? 1 : DanhSachNhanVien.Max(p => p.MaNhanVien) + 1;
                    DanhSachNhanVien.Add(new NhanVien
                    {
                        MaNhanVien = nextId,
                        TenNhanVien = TenNhanVien,
                        SoDienThoai = SoDienThoai,
                        HinhAnh = HinhAnh
                    });
                }

                HienThiForm = false;
            });

            HuyNhanVienCommand = new RelayCommand(o =>
            {
                HienThiForm = false;
            });

            DanhSachNhanVien.Add(new NhanVien
            {
                MaNhanVien = 1,
                TenNhanVien = "Nguyễn Văn A",
                SoDienThoai = "0901234567",
                HinhAnh = string.Empty
            });
        }

        private void XoaForm()
        {
            TenNhanVien = string.Empty;
            SoDienThoai = string.Empty;
            HinhAnh = string.Empty;
        }
    }
}
