using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for LoadServiceInvoView.xaml
    /// </summary>
    [Export("LoadServiceInvoView")]
    public partial class LoadServiceInvoView : UserControl
    {

        #region Public Properties
        [Import]
        public LoadServiceInvoViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        #endregion Public Properties

        public LoadServiceInvoView()
        {
            InitializeComponent();
        }
    }
}
