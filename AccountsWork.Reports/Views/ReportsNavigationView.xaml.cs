using AccountsWork.Infrastructure;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsNavigationView.xaml
    /// </summary>
    [Export]
    [ViewSortHint("02")]
    public partial class ReportsNavigationView : UserControl, IPartImportsSatisfiedNotification
    {
        private static Uri reportsViewUri = new Uri("/ReportsView", UriKind.Relative);

        [Import]
        public IRegionManager regionManager;

        public ReportsNavigationView()
        {
            InitializeComponent();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this.regionManager.Regions[RegionNames.MainContentRegion];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }
        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {
            this.UpdateNavigationButtonState(e.Uri);
        }
        private void UpdateNavigationButtonState(Uri uri)
        {
            this.NavigateToAccountsRadioButton.IsChecked = (uri == reportsViewUri);
        }
        private void NavigateToReportsRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, reportsViewUri);
        }
    }
}
