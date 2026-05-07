using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using qlks_app.commands;

namespace qlks_app.view_model
{
    public class QuanTri_ViewModel : BaseViewModel
    {
        private object _current;
        public object Current
        {
            get { return _current; }
            set
            {
                _current = value;
                OnPropertyChanged(nameof(Current));
            }
        }

        public RelayCommand ShowRoomManagementCommand { get; set; }
        public RelayCommand ShowServiceManagementCommand { get; set; }
        public RelayCommand ShowEmployeeManagementCommand { get; set; }
        public RelayCommand ShowInventoryManagementCommand { get; set; }
        public RelayCommand ShowReportCommand { get; set; }
        public RelayCommand DangXuatCommand { get; set; }

        public QuanTri_ViewModel()
        {
            ShowRoomManagementCommand = new RelayCommand(o =>
            {
                Current = new views.QuanLiPhong_View();
            });

            ShowServiceManagementCommand = new RelayCommand(o =>
            {
                Current = null;
                MessageBox.Show("Chức năng quản lý dịch vụ đang phát triển");
            });

            ShowEmployeeManagementCommand = new RelayCommand(o =>
            {
                Current = new views.NhanVienManagement_View();
            });

            ShowInventoryManagementCommand = new RelayCommand(o =>
            {
                Current = null;
                MessageBox.Show("Chức năng quản lý kho đang phát triển");
            });

            ShowReportCommand = new RelayCommand(o =>
            {
                Current = null;
                MessageBox.Show("Chức năng báo cáo đang phát triển");
            });

            DangXuatCommand = new RelayCommand(o =>
            {
                new views.Login_View().Show();
                (o as Window)?.Close();
            });
        }
    }
}
