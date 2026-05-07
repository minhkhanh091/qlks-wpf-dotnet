using System.Windows;
using System.Windows.Controls;

namespace qlks_app.views
{
    /// <summary>
    /// Interaction logic for QuanLiPhong_View.xaml
    /// </summary>
    public partial class QuanLiPhong_View : UserControl
    {
        public QuanLiPhong_View()
        {
            InitializeComponent();
            DataContext = new view_model.QuanLiPhong_ViewModel();
        }
    }
}
