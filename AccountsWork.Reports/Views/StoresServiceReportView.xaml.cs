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

        private void dgStores_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            var dataContext = dgStores.DataContext as StoresServiceReportViewModel;
            dataContext.SelectedStores = new System.Collections.ObjectModel.ObservableCollection<StoresWithCheck>();
            foreach(var item in dgStores.SelectedItems)
            {
                dataContext.SelectedStores.Add((StoresWithCheck)item);
            }
            
        }
    }
}
