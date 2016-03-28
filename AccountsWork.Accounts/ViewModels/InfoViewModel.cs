using System;
using System.ComponentModel.Composition;
using AccountsWork.Infrastructure;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System.Collections.ObjectModel;
using AccountsWork.DomainModel;
using System.ComponentModel;
using AccountsWork.BusinessLayer;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class InfoViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private int _storeError;
        private int _sumError;
        private IList<AccountsMainSet> _accountList;
        private BackgroundWorker _worker;
        private IAccountsMainService _accountsMainService;
        private bool _isInfoBusy;
        private ObservableCollection<AccountsMainSet> _storeErrorList;
        private ObservableCollection<AccountsMainSet> _capexErrorList;
        private AccountsMainSet _selectedCapexError;
        private AccountsMainSet _selectedStoreError;
        private IRegionManager _regionManager;
        #endregion Private Fields 

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        public IList<AccountsMainSet> AccountList
        {
            get { return _accountList; }
            set { SetProperty(ref _accountList, value); }
        }
        public bool IsInfoBusy
        {
            get { return _isInfoBusy; }
            set { SetProperty(ref _isInfoBusy, value); }
        }
        #endregion infrastructure


        #region errors
        public int StoreError
        {
            get { return _storeError; }
            set { SetProperty(ref _storeError, value); }
        }
        public int SumError
        {
            get { return _sumError; }
            set { SetProperty(ref _sumError, value); }
        }
        public ObservableCollection<AccountsMainSet> StoreErrorList
        {
            get { return _storeErrorList; }
            set { SetProperty(ref _storeErrorList, value); }
        }
        public ObservableCollection<AccountsMainSet> CapexErrorList
        {
            get { return _capexErrorList; }
            set { SetProperty(ref _capexErrorList, value); }
        }
        public AccountsMainSet SelectedCapexError
        {
            get { return _selectedCapexError; }
            set { SetProperty(ref _selectedCapexError, value); }
        }
        public AccountsMainSet SelectedStoreError
        {
            get { return _selectedStoreError; }
            set { SetProperty(ref _selectedStoreError, value); }
        }
        #endregion errors

        #endregion Public Properties

        #region Commands

        #region infrastructure
        public DelegateCommand<object> NavigateCommand { get; set; }
        #endregion infrastructure

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public InfoViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager)
        {
            #region infrastructure
            AccountsTabItemHeader = "Общая информация";
            _regionManager = regionManager;
            AccountList = new List<AccountsMainSet>();
            NavigateCommand = new DelegateCommand<object>(Navigate);
            #endregion infrastructure

            #region services
            _accountsMainService = accountsMainService;
            #endregion services

            #region errors
            StoreErrorList = new ObservableCollection<AccountsMainSet>();
            CapexErrorList = new ObservableCollection<AccountsMainSet>();

            #endregion errors

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccountList;
            _worker.RunWorkerCompleted += LoadAccountList_Completed;
            #endregion workers
        }

        
        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _worker.RunWorkerAsync();
        }
        private void LoadAccountList(object sender, DoWorkEventArgs e)
        {
            IsInfoBusy = true;
            AccountList = _accountsMainService.GetAllAccountsWithStoresAndCapex();
            StoreErrorList = new ObservableCollection<AccountsMainSet>(AccountList.Where(a => a.AccountsStoreDetailsSets.Count == 0));
            CapexErrorList = new ObservableCollection<AccountsMainSet>(AccountList.Where(a => a.AccountsCapexInfoSets.Count == 0 || a.AccountsCapexInfoSets.Sum(c => c.AccountCapexAmount) != a.AccountAmount));
            StoreError = StoreErrorList.Count;
            SumError = CapexErrorList.Count;
        }
        private void LoadAccountList_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsInfoBusy = false;
        }
        void Navigate(object account)
        {
            var ResultAccount = account as AccountsMainSet;
            if (ResultAccount != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("Account", ResultAccount);
                _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, "AdditionalInfoView", navigationParameters);
            }
        }
        #endregion infrastructure

        #endregion Methods
    }
}