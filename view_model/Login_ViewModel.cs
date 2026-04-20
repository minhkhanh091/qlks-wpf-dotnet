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

        public RelayCommand DangNhapCommand { get; set; }

        public Login_ViewModel()
        {
            DangNhapCommand = new RelayCommand(o =>
            {
                bool isValid = true;

                if (isValid)
                {
                    if (vaiTro == "Quản trị")
                    {
                        new views.QuanTri_View().Show();
                    }
                    else if (vaiTro == "Khách hàng")
                    {
                        new views.KhachHang_View().Show();
                    }

                    Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                }
            });
        }
    }
}
