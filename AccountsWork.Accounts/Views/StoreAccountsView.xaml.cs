using AccountsWork.Accounts.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Interaction logic for StoreAccountsView.xaml
    /// </summary>
    [Export("StoreAccountsView")]
    public partial class StoreAccountsView : UserControl
    {
        #region Public Properties
        [Import]
        public StoreAccountsViewModel ViewModel
        {
            set { DataContext = value; }
        }
        #endregion Public Properties

        #region Constructor
        public StoreAccountsView()
        {
            InitializeComponent();
        }
        #endregion Constructor
    }
}
