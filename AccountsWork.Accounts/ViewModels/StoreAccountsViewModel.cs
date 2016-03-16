using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class StoreAccountsViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private const string StoresKey = "Store";
        private StoresSet _currentStore;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        #endregion infrastructure

        #region store
        public StoresSet CurrentStore
        {
            get { return _currentStore; }
            set { SetProperty(ref _currentStore, value); }
        }
        #endregion store

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public StoreAccountsViewModel()
        {
            AccountsTabItemHeader = "Информация";
        }
        #endregion Constructor

        #region Methods

        #region infrastructure
        private StoresSet GetStore(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters[StoresKey];
            var store = (StoresSet)parameter;
            

            return (StoresSet)store;
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentStore = GetStore(navigationContext);
            if (CurrentStore != null)
            {
                AccountsTabItemHeader = string.Format("{0} {1}", CurrentStore.StoreNumber, CurrentStore.StoreName);
            }
        }
        #endregion infrastructure

        #endregion Methods
    }
    public class StoreAccounts
    {
        public string AccountNumber { get; set; }
        public string AccountCompany { get; set; }
        public DateTime AccountDate { get; set; }
        public decimal AccountAmount { get; set; }
        public string AccountCapex { get; set; }
        public string AccountDescription { get; set; }
        public string AccountStatus { get; set; }
        public DateTime AccountStatusDate { get; set; }
    }
}
