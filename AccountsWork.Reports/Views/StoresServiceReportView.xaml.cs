using AccountsWork.Reports.Model;
using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for StoresServiceReportView.xaml
    /// </summary>
    [Export("StoresServiceReportView")]
    public partial class StoresServiceReportView : UserControl
    {
        #region Public Properties
        [Import]
        public StoresServiceReportViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        #endregion Public Properties


        public StoresServiceReportView()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var records = dgStores.View.Records;
            var viewModel = dgStores.DataContext as StoresServiceReportViewModel;
            var che = sender as CheckBox;
            foreach(var record in dgStores.SelectedItems)
            {
                foreach(var store in viewModel.StoresWithCheckList)
                {
                    if (store.Store.StoreNumber == ((StoresWithCheck)record).Store.StoreNumber)
                        store.Check = che.IsChecked.Value;
                }
            }
            dgStores.View.Refresh();
        }
    }
}
