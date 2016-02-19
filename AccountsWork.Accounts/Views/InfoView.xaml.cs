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
        #region Constructor
        public InfoView()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Public Properties
        [Import]
        public InfoViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties
    }
}
