using System.Windows.Controls;

namespace qlks_app.views
{
    /// <summary>
    /// Interaction logic for NhanVienManagement_View.xaml
    /// </summary>
    public partial class NhanVienManagement_View : UserControl
    {
        public NhanVienManagement_View()
        {
            InitializeComponent();
            DataContext = new view_model.NhanVienManagement_ViewModel();
        }
    }
}
