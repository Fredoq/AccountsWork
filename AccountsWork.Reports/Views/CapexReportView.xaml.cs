using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for CapexReportView.xaml
    /// </summary>
    [Export("CapexReportView")]
    public partial class CapexReportView : UserControl
    {
        #region Public Properties
        [Import]
        public CapexReportViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        #endregion Public Properties

        #region Constructor

        public CapexReportView()
        {
            InitializeComponent();
        }

        #endregion Constructor
    }
}
