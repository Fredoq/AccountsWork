using AccountsWork.Accounts.Model;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Commands;
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
        private ObservableCollection<StoreAccount> _storeAccountsList;
        private BackgroundWorker _worker;
        private bool _isStoreAccountsBusy;
        private IAccountsMainService _accountsMainService;
        private string _searchStore;
        private ObservableCollection<StoresSet> _searchStoreResultList;
        private ObservableCollection<StoresSet> _storeList;
        private IStoresService _storeService;
        private StoresSet _resultStore;
        private ObservableCollection<AccountsMainSet> _accountsList;
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

        public string SearchStore
        {
            get { return _searchStore; }
            set { SetProperty(ref _searchStore, value); }
        }
        public ObservableCollection<StoresSet> SearchStoreResultList
        {
            get { return _searchStoreResultList; }
            set { SetProperty(ref _searchStoreResultList, value); }
        }
        public ObservableCollection<StoresSet> StoreList
        {
            get { return _storeList; }
            set { SetProperty(ref _storeList, value); }
        }
        public StoresSet ResultStore
        {
            get { return _resultStore; }
            set { SetProperty(ref _resultStore, value); }
        }
        public ObservableCollection<AccountsMainSet> AccountsList
        {
            get { return _accountsList; }
            set { SetProperty(ref _accountsList, value); }
        }
        #endregion store

        #endregion Public Properties

        #region Commands

        #region store
        public DelegateCommand SearchStoreCommand { get; set; }
        public DelegateCommand LoadResultStoreCommand { get; set; }
        #endregion store

    #endregion Commands

    #region Constructor
    [ImportingConstructor]
        public StoreAccountsViewModel(IAccountsMainService accountsMainService, IStoresService storeService)
        {
            #region services
            _accountsMainService = accountsMainService;
            _storeService = storeService;
            #endregion services

            #region infrastructure
            AccountsTabItemHeader = "Поиск по ресторанам";
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadStoresAndAccounts;
            _worker.RunWorkerCompleted += LoadStoresAndAccounts_Completed;
            #endregion infrastructure

            #region store
            SearchStoreResultList = new ObservableCollection<StoresSet>();
            StoreAccountsList = new ObservableCollection<StoreAccount>();
            IsStoreAccountsBusy = false;
            SearchStoreCommand = new DelegateCommand(SearchStoreMethod);
            LoadResultStoreCommand = new DelegateCommand(LoadAccountsForStore);
            #endregion store
        }        


        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            StoreAccountsList.Clear();
            _worker.RunWorkerAsync();
        }
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        private void LoadStoresAndAccounts(object sender, DoWorkEventArgs e)
        {
            IsStoreAccountsBusy = true;
            StoreList = new ObservableCollection<StoresSet>(_storeService.GetStores());
            AccountsList = new ObservableCollection<AccountsMainSet>(_accountsMainService.GetAllAccountsForStore());

        }
        #endregion infrastructure

        #region store
        private void LoadAccountsForStore()
        {
            if (ResultStore == null || AccountsList == null) return;
            StoreAccountsList.Clear();                  
            foreach (var account in AccountsList.Where(a => a.AccountsStoreDetailsSets.Any(s => s.AccountStore == ResultStore.StoreNumber)))
                {
                    var storeAccount = new StoreAccount();
                    var status = account.AccountsStatusDetailsSets.LastOrDefault();
                    var capexes = account.AccountsCapexInfoSets;

                    storeAccount.AccountAmount = account.AccountAmount.Value;
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
        private void LoadStoresAndAccounts_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsStoreAccountsBusy = false;
        }
        private void SearchStoreMethod()
        {
            if (!string.IsNullOrWhiteSpace(SearchStore))
            {
                SearchStoreResultList = new ObservableCollection<StoresSet>(StoreList.Where(s => s.StoreName.ToLower().Contains(SearchStore.ToLower()) || s.StoreNumber.ToString().Contains(SearchStore)));
            }
            else
                SearchStoreResultList.Clear();
        }
        #endregion store

        #endregion Methods
    }
}
