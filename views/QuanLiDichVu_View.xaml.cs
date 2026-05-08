using System.Windows.Controls;

namespace qlks_app.views
{
    public partial class QuanLiDichVu_View : UserControl
    {
        public QuanLiDichVu_View()
        {
            InitializeComponent();
            DataContext = new view_model.QuanLiDichVu_ViewModel();
        }
    }
}
