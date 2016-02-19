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
        #region Constructor
        public AddAccountView()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Public Properties
        [Import]
        public AddAccountViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties
    }
}
