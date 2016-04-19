using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for ServiceReportForStoreByMonthView.xaml
    /// </summary>
    [Export("ServiceReportForStoreByMonthView")]
    public partial class ServiceReportForStoreByMonthView : UserControl
    {
        #region Public Properties
        [Import]
        public ServiceReportForStoreByMonthViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        #endregion Public Properties

        public ServiceReportForStoreByMonthView()
        {
            InitializeComponent();
        }
    }
}
