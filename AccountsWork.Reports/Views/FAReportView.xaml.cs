using AccountsWork.Reports.Model;
using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for FAReportView.xaml
    /// </summary>
    [Export("FAReportView")]
    public partial class FAReportView : UserControl
    {
        [Import]
        public FAReportViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        public FAReportView()
        {
            InitializeComponent();
        }

        private void SfChart_SelectionChanged(object sender, Syncfusion.UI.Xaml.Charts.ChartSelectionChangedEventArgs e)
        {
            if (e.SelectedSegment != null)
            {
                var viewModel = DataContext as FAReportViewModel;
                var stackedStore = (AccountFA)e.SelectedSegment.Item;
                if (stackedStore != null)
                    viewModel.SelectedAccountFA = stackedStore;
            }
        }
        
    }
}
