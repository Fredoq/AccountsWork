using AccountsWork.Accounts.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Interaction logic for ChangeStatusView.xaml
    /// </summary>
    [Export("ChangeStatusView")]
    public partial class ChangeStatusView : UserControl
    {
        #region Public Properties
        [Import]
        public ChangeStatusViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties

        public ChangeStatusView()
        {
            InitializeComponent();
        }
    }
}
