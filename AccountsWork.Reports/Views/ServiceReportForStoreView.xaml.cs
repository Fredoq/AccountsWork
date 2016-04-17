using AccountsWork.Reports.Model;
using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for ServiceReportForStoreView.xaml
    /// </summary>
    [Export("ServiceReportForStoreView")]
    public partial class ServiceReportForStoreView : UserControl
    {
        #region Public Properties
        [Import]
        public ServiceReportForStoreViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        #endregion Public Properties


        public ServiceReportForStoreView()
        {
            InitializeComponent();
        }

        private void SfChart_SelectionChanged(object sender, Syncfusion.UI.Xaml.Charts.ChartSelectionChangedEventArgs e)
        {
            if (e.SelectedSegment != null)
            {
                var viewModel = DataContext as ServiceReportForStoreViewModel;
                var stackedStore = (StackedStoreInfo)e.SelectedSegment.Item;
                if (stackedStore != null)
                    viewModel.SelectedStackedStore = stackedStore;
            }
        }
    }
}
