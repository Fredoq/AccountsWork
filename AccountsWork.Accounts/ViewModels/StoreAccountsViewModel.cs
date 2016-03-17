using AccountsWork.Accounts.Model;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class StoreAccountsViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private const string StoresKey = "Store";
        private StoresSet _currentStore;
        private ObservableCollection<StoreAccount> _storeAccountsList;
        private BackgroundWorker _worker;
        private bool _isStoreAccountsBusy;
        private IAccountStoresService _accountStoreService;
        private IAccountsMainService _accountsMainService;
        private IAccountStatusService _accountStatusService;
        private IAccountCapexesService _accountCapexesService;
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
        public ObservableCollection<StoreAccount> StoreAccountsList
        {
            get { return _storeAccountsList; }
            set { SetProperty(ref _storeAccountsList, value); }
        }
        public bool IsStoreAccountsBusy
        {
            get { return _isStoreAccountsBusy; }
            set { SetProperty(ref _isStoreAccountsBusy, value); }
        }
        #endregion store

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public StoreAccountsViewModel(IAccountStoresService accountStoreService, IAccountsMainService accountsMainService, IAccountStatusService accountStatusService, IAccountCapexesService accountCapexesService)
        {
            #region services
            _accountStoreService = accountStoreService;
            _accountsMainService = accountsMainService;
            _accountStatusService = accountStatusService;
            _accountCapexesService = accountCapexesService;
            #endregion services

            #region infrastructure
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccountsForStore;
            _worker.RunWorkerCompleted += LoadAccountsForStore_Completed;
            #endregion infrastructure

            #region store
            StoreAccountsList = new ObservableCollection<StoreAccount>();
            IsStoreAccountsBusy = false;
            #endregion store
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
                _worker.RunWorkerAsync();
            }
        }
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        #endregion infrastructure

        #region store
        private void LoadAccountsForStore(object sender, DoWorkEventArgs e)
        {
            IsStoreAccountsBusy = true;
            StoreAccountsList.Clear();
            var accounts = _accountsMainService.GetAllAccountsWithStatusAndCapex();
            foreach (var account in accounts.Where(a => a.AccountsStoreDetailsSets.Any(s => s.AccountStore == CurrentStore.StoreNumber)))
            {
                    var storeAccount = new StoreAccount();
                    var status = account.AccountsStatusDetailsSets.LastOrDefault();
                    var capexes = account.AccountsCapexInfoSets;

                    storeAccount.AccountAmount = account.AccountAmount;
                    if (capexes != null)
                    {
                        var i = 1;
                        foreach(var capex in capexes)
                        {
                            if (i == capexes.Count)
                                storeAccount.AccountCapex += capex.AccountCapexName;
                            else
                                storeAccount.AccountCapex += capex.AccountCapexName + ";";
                            i++;
                        }                    
                    }
                    storeAccount.AccountCompany = account.AccountCompany;
                    storeAccount.AccountDate = account.AccountDate;
                    storeAccount.AccountDescription = account.AccountDescription;
                    storeAccount.AccountNumber = account.AccountNumber;
                    storeAccount.AccountStatus = status.AccountStatus;
                    storeAccount.AccountStatusDate = status.AccountStatusDate;
                    StoreAccountsList.Add(storeAccount);                
            }

        }
        private void LoadAccountsForStore_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsStoreAccountsBusy = false;
        }
        #endregion store

        #endregion Methods
    }
}
