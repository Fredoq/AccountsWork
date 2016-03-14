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
        public ObservableCollection<AccountsMainSet> SearchResultList
        {
            get { return _searchResultList; }
            set { SetProperty(ref _searchResultList, value); }
        }
        public bool IsSearchAccBusy
        {
            get { return _isSearchAccBusy; }
            set { SetProperty(ref _isSearchAccBusy, value); }
        }
        #endregion search acc

        #endregion Public Properties

        #region Commands

        #region infrastructure
        public DelegateCommand<string> NavigateCommand { get; set; }
        #endregion infrastructure

        #region search acc
        public DelegateCommand SearchAccountCommand { get; set; }
        #endregion search acc

        #endregion Commands

     

        #region Constructor
        [ImportingConstructor]
        public AccountsViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager)
        //public AccountsViewModel()
        {
            #region services
            _accountsMainService = accountsMainService;
            #endregion services

            #region infrastructure
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
            #endregion infrastructure

            #region search acc
            SearchResultList = new ObservableCollection<AccountsMainSet>();
            IsSearchAccBusy = false;
            SearchAccountCommand = new DelegateCommand(StartSearch);
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
            _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, navigationProperty);
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
        #endregion search acc

        #endregion Methods   
    }
}