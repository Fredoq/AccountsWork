using System.ComponentModel.Composition;
using System.Windows.Controls;
using AccountsWork.Accounts.ViewModels;

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Логика взаимодействия для AddAccountView.xaml
    /// </summary>
    [Export("AddAccountView")]
    public partial class AddAccountView : UserControl
    {
        public AddAccountView()
        {
            InitializeComponent();
        }
        [Import]
        public AddAccountViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
    }
}
