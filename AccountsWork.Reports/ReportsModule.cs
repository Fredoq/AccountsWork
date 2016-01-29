using System.ComponentModel.Composition;
using Prism.Modularity;
using Prism.Mef.Modularity;
using Prism.Regions;
using AccountsWork.Infrastructure;
using AccountsWork.Reports.Views;

namespace AccountsWork.Reports
{
    [ModuleExport(typeof(ReportsModule))]
    public class ReportsModule : IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ReportsNavigationView));
        }
    }
}
