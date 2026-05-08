using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using qlks_app.commands;

namespace qlks_app.view_model
{
    public class QuanLiDichVu_ViewModel : BaseViewModel
    {
        private readonly QLKSEntities _db = new QLKSEntities();
        private bool _hienThiForm;
        private bool _dangSua;
        private DichVu _dichVuDangChon;
        private string _tenDichVu;
        private string _donGia;
        private string _donViTinh;

        public ObservableCollection<DichVu> DanhSachDichVu { get; } = new ObservableCollection<DichVu>();

        public DichVu DichVuDangChon
        {
            get { return _dichVuDangChon; }
            set
            {
                _dichVuDangChon = value;
                OnPropertyChanged(nameof(DichVuDangChon));
                CommandManager.InvalidateRequerySuggested();
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

        public string TenDichVu
        {
            get { return _tenDichVu; }
            set
            {
                _tenDichVu = value;
                OnPropertyChanged(nameof(TenDichVu));
            }
        }

        public string DonGia
        {
            get { return _donGia; }
            set
            {
                _donGia = value;
                OnPropertyChanged(nameof(DonGia));
            }
        }

        public string DonViTinh
        {
            get { return _donViTinh; }
            set
            {
                _donViTinh = value;
                OnPropertyChanged(nameof(DonViTinh));
            }
        }

        public RelayCommand ThemDichVuCommand { get; }
        public RelayCommand SuaDichVuCommand { get; }
        public RelayCommand LuuDichVuCommand { get; }
        public RelayCommand HuyDichVuCommand { get; }
        public RelayCommand TaiLaiCommand { get; }

        public QuanLiDichVu_ViewModel()
        {
            ThemDichVuCommand = new RelayCommand(o =>
            {
                _dangSua = false;
                XoaForm();
                HienThiForm = true;
            });

            SuaDichVuCommand = new RelayCommand(o =>
            {
                if (DichVuDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn dịch vụ để sửa");
                    return;
                }

                _dangSua = true;
                TenDichVu = DichVuDangChon.TenDichVu;
                DonGia = DichVuDangChon.DonGia.ToString("N0", CultureInfo.CurrentCulture);
                DonViTinh = DichVuDangChon.DonViTinh;
                HienThiForm = true;
            });

            LuuDichVuCommand = new RelayCommand(o => LuuDichVu());
            HuyDichVuCommand = new RelayCommand(o => HienThiForm = false);
            TaiLaiCommand = new RelayCommand(o => TaiDuLieu());

            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            var danhSach = _db.DichVus
                .OrderBy(dv => dv.TenDichVu)
                .ToList();

            DanhSachDichVu.Clear();
            foreach (var item in danhSach)
            {
                DanhSachDichVu.Add(item);
            }

            DichVuDangChon = null;
        }

        private void LuuDichVu()
        {
            if (string.IsNullOrWhiteSpace(TenDichVu))
            {
                MessageBox.Show("Vui lòng nhập tên dịch vụ");
                return;
            }

            if (!decimal.TryParse(DonGia, NumberStyles.Number, CultureInfo.CurrentCulture, out var donGia))
            {
                MessageBox.Show("Đơn giá không hợp lệ");
                return;
            }

            if (string.IsNullOrWhiteSpace(DonViTinh))
            {
                MessageBox.Show("Vui lòng nhập đơn vị tính");
                return;
            }

            if (_dangSua && DichVuDangChon != null)
            {
                DichVuDangChon.TenDichVu = TenDichVu;
                DichVuDangChon.DonGia = donGia;
                DichVuDangChon.DonViTinh = DonViTinh;
            }
            else
            {
                var dichVu = new DichVu
                {
                    TenDichVu = TenDichVu,
                    DonGia = donGia,
                    DonViTinh = DonViTinh
                };

                _db.DichVus.Add(dichVu);
            }

            _db.SaveChanges();
            HienThiForm = false;
            TaiDuLieu();
        }

        private void XoaForm()
        {
            TenDichVu = string.Empty;
            DonGia = string.Empty;
            DonViTinh = string.Empty;
        }
    }
}
