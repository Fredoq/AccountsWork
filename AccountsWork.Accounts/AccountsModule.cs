using System.ComponentModel.Composition;
using AccountsWork.Accounts.Views;
using AccountsWork.Infrastructure;
using Prism.Modularity;
using Prism.Mef.Modularity;
using Prism.Regions;

namespace AccountsWork.Accounts
{
    [ModuleExport(typeof(AccountsModule))]
    public class AccountsModule : IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(AccountsNavigationView));
        }
    }
}
