using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Prism.Regions;
using Syncfusion.UI.Xaml.Grid;
using System.ComponentModel;
using Prism.Interactivity.InteractionRequest;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountsViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private IAccountsMainService _accountsMainService;
        private IRegionManager _regionManager;
        private string _searchAccountNumber;
        private bool _isSearchAccountOpen;
        private ObservableCollection<AccountsMainSet> _searchResultList;
        private bool _isSearchAccBusy;
        private BackgroundWorker _searchWorker;
        private AccountsMainSet _resultAccount;
        private const string AdditionalInfoViewKey = "AdditionalInfoView";
        private const string AddAccountViewKey = "AddAccountView";
        private const string StoreAccountsViewKey = "StoreAccountsView";
        private string _searchStore;
        private ObservableCollection<StoresSet> _searchStoreResultList;
        private ObservableCollection<StoresSet> _storeList;
        private IStoresService _storeService;
        private StoresSet _resultStore;
        #endregion Private Fields

        #region Public Properties

        #region search acc
        public string SearchAccountNumber
        {
            get { return _searchAccountNumber; }
            set { SetProperty(ref _searchAccountNumber, value); }
        }        
        public bool IsSearchAccountOpen
        {
            get { return _isSearchAccountOpen; }
            set { SetProperty(ref _isSearchAccountOpen, value); }
        }
        public InteractionRequest<IConfirmation> DeleteConfirmationRequest { get; set; }
        public ObservableCollection<AccountsMainSet> SearchResultList
        {
            get { return _searchResultList; }
            set { SetProperty(ref _searchResultList, value); }
        }
        public AccountsMainSet ResultAccount
        {
            get { return _resultAccount; }
            set { SetProperty(ref _resultAccount, value); }
        }
        public bool IsSearchAccBusy
        {
            get { return _isSearchAccBusy; }
            set { SetProperty(ref _isSearchAccBusy, value); }
        }
        #endregion search acc

        #region search store
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
        #endregion search store
        #endregion Public Properties

        #region Commands

        #region infrastructure
        public DelegateCommand<string> NavigateCommand { get; set; }
        
        #endregion infrastructure

        #region search acc
        public DelegateCommand SearchAccountCommand { get; set; }
        public DelegateCommand DeleteAccountCommand { get; set; }
        public DelegateCommand CloseSearchCommand { get; set; }
        #endregion search acc

        #region search store
        public DelegateCommand SearchStoreCommand { get; set; }
        #endregion search store
        #endregion Commands



        #region Constructor
        [ImportingConstructor]
        public AccountsViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager, IStoresService storeService)
        {
            #region services
            _accountsMainService = accountsMainService;
            _storeService = storeService;
            #endregion services

            #region infrastructure
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            #endregion infrastructure

            #region search acc
            SearchResultList = new ObservableCollection<AccountsMainSet>();
            IsSearchAccBusy = false;
            SearchAccountCommand = new DelegateCommand(StartSearch);
            DeleteAccountCommand = new DelegateCommand(DeleteAccount);
            CloseSearchCommand = new DelegateCommand(CloseSearch);
            DeleteConfirmationRequest = new InteractionRequest<IConfirmation>();
            #endregion search acc

            #region search store
            SearchStoreResultList = new ObservableCollection<StoresSet>();
            StoreList = new ObservableCollection<StoresSet>(storeService.GetStores());

            SearchStoreCommand = new DelegateCommand(SearchStoreMethod);
            #endregion search store

            #region workers
            _searchWorker = new BackgroundWorker();
            _searchWorker.DoWork += SearchAccountWork;
            _searchWorker.RunWorkerCompleted += SeachAccountWork_Completed;
            #endregion workers

        }

        
        #endregion Constructor

        #region Methods

        #region infrastructure
        void Navigate(string navigationProperty)
        {
            if (ResultAccount != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("Account", ResultAccount);
                _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, navigationProperty, navigationParameters);
                ResultAccount = null;
                IsSearchAccountOpen = false;
            }
            else
            {
                if (navigationProperty == StoreAccountsViewKey)
                {
                    if (ResultStore != null)
                    {
                        var navigationParameters = new NavigationParameters();
                        navigationParameters.Add("Store", ResultStore);
                        _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, navigationProperty, navigationParameters);
                    }
                }
                else
                {
                    _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, navigationProperty);
                }
            }
            SearchResultList.Clear();
            
        }
        #endregion infrastrcture

        #region search acc

        private void SearchAccountWork(object sender, DoWorkEventArgs e)
        {
            IsSearchAccBusy = true;
            SearchAccount();
        }
        private void SeachAccountWork_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsSearchAccBusy = false;
            SearchAccountNumber = string.Empty;
        }
        private void StartSearch()
        {
            _searchWorker.RunWorkerAsync();
        }
        private void SearchAccount()
        {
            if(!string.IsNullOrWhiteSpace(SearchAccountNumber))
            {
                IsSearchAccountOpen = true;
                SearchResultList = new ObservableCollection<AccountsMainSet>(_accountsMainService.GetAccountsByNumber(SearchAccountNumber));
            }
        }
        private void DeleteAccount()
        {
            DeleteConfirmationRequest.Raise(
                    new Confirmation { Content = "Удалить счет?", Title = "Удаление счета" },
                    c => {
                        if (c.Confirmed)
                        {
                            if (ResultAccount != null)
                            {
                                _accountsMainService.RemoveAccount(ResultAccount);
                                SearchResultList.Remove(ResultAccount);
                            }
                        }
                    });
        }
        private void CloseSearch()
        {
            IsSearchAccountOpen = false;
            SearchResultList.Clear();            
        }
        #endregion search acc

        #region search store
        private void SearchStoreMethod()
        {
            if (!string.IsNullOrWhiteSpace(SearchStore))
            {
                SearchStoreResultList = new ObservableCollection<StoresSet>(StoreList.Where(s => s.StoreName.ToLower().Contains(SearchStore.ToLower()) || s.StoreNumber.ToString().Contains(SearchStore)));
            }
            else
                SearchStoreResultList.Clear();
        }
        #endregion search store
        #endregion Methods   
    }
}