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
        private bool _isLeftOpen;
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
        public bool IsLeftOpen
        {
            get { return _isLeftOpen; }
            set { SetProperty(ref _isLeftOpen, value); }
        }
        #endregion search acc

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

        #endregion Commands


        #region Constructor
        [ImportingConstructor]
        public AccountsViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager)
        {
            #region services
            _accountsMainService = accountsMainService;
            #endregion services

            #region infrastructure
            _regionManager = regionManager;
            IsLeftOpen = false;
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
                _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, navigationProperty);
            }
            SearchResultList.Clear();
            IsLeftOpen = false;          
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
            if (!IsSearchAccBusy)
            {
                _searchWorker.RunWorkerAsync();
            }
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

        #endregion Methods   
    }
}