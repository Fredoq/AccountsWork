using System.ComponentModel.Composition;
using System.Windows.Controls;
using AccountsWork.Accounts.ViewModels;

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Логика взаимодействия для InfoView.xaml
    /// </summary>
    [Export("InfoView")]
    public partial class InfoView : UserControl
    {
        public InfoView()
        {
            InitializeComponent();
        }
        [Import]
        public InfoViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
    }
}
