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
        #region Public Fields
        [Import]
        public IRegionManager regionManager;
        #endregion Public Fields

        #region Methods
        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(AccountsNavigationView));
        }
        #endregion Methods
    }
}
