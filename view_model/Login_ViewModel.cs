using qlks_app.commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace qlks_app.view_model
{
    public class Login_ViewModel : BaseViewModel
    {
        private string _tenTaiKhoan;
        public string tenTaiKhoan
        {
            get { return _tenTaiKhoan; }
            set
            {
                _tenTaiKhoan = value;
                OnPropertyChanged(nameof(tenTaiKhoan));
            }
        }

        private string _vaiTro;
        public string vaiTro
        {
            get { return _vaiTro; }
            set 
            { 
                _vaiTro = value;
                OnPropertyChanged(nameof(vaiTro));
            }
        }

        private string _matKhau;
        public string matKhau
        {
            get { return _matKhau; }
            set
            {
                _matKhau = value;
                OnPropertyChanged(nameof(matKhau));
            }
        }

        public RelayCommand DangNhapCommand { get; set; }
        public RelayCommand HuyCommand { get; set; }

        public Login_ViewModel()
        {
            DangNhapCommand = new RelayCommand(o =>
            {
                try
                {
                    var db = new QLKSEntities();
                    var account = db.TaiKhoans.FirstOrDefault(a =>
                        a.TenDangNhap == tenTaiKhoan &&
                        a.MatKhau == matKhau);

                    if (account == null)
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                        return;
                    }

                    var selectedRole = NormalizeRole(vaiTro);
                    var accountRole = NormalizeRole(account.VaiTro);

                    if (!string.Equals(selectedRole, accountRole, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Vai trò đăng nhập không đúng");
                        return;
                    }

                    if (string.Equals(accountRole, "ADMIN", StringComparison.OrdinalIgnoreCase))
                    {
                        new views.QuanTri_View().Show();
                    }
                    else if (string.Equals(accountRole, "NHANVIEN", StringComparison.OrdinalIgnoreCase))
                    {
                        new views.NhanVien_View().Show();
                    }
                    else if (string.Equals(accountRole, "KHACHHANG", StringComparison.OrdinalIgnoreCase))
                    {
                        new views.KhachHang_View().Show();
                    }
                    else
                    {
                        MessageBox.Show("Vai trò tài khoản không hợp lệ");
                        return;
                    }

                    (o as Window)?.Close();
                }
                catch
                {
                    MessageBox.Show("Không thể kết nối dữ liệu đăng nhập");
                }
            }, o =>
                !string.IsNullOrWhiteSpace(tenTaiKhoan) &&
                !string.IsNullOrWhiteSpace(matKhau) &&
                !string.IsNullOrWhiteSpace(vaiTro));

            HuyCommand = new RelayCommand(o => {
                Application.Current.Shutdown();
            });
        }

        private string NormalizeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return string.Empty;

            var normalized = role.Trim().ToLowerInvariant();
            if (normalized == "admin" || normalized == "quản trị" || normalized == "quan tri")
                return "ADMIN";
            if (normalized == "nhân viên" || normalized == "nhan vien" || normalized == "le tan")
                return "NHANVIEN"; // Consistent with stored role names
            if (normalized == "khách hàng" || normalized == "khach hang")
                return "KHACHHANG";

            return role.Trim().ToUpperInvariant();
        }
    }
}
