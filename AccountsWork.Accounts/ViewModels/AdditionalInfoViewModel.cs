﻿using AccountsVork.Infrastructure;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class AdditionalInfoViewModel : ValidatableBindableBase, IConfirmNavigationRequest
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private string AccountKey = "Account";
        private AccountsMainSet _currentAccount;
        private AccountsStatusDetailsSet _accountStatus;
        private readonly BackgroundWorker _worker;
        private readonly IAccountStatusService _accountStatusService;
        private bool _isChangeStatusOpen;
        private List<string> _statusesList;
        private AccountsStatusDetailsSet _newAccountStatus;
        private bool _isStatusHistoryOpen;
        private ObservableCollection<AccountsStatusDetailsSet> _statusHistoryList;
        private bool _isEditAccountStoresOpen;
        private ObservableCollection<StoresSet> _accountStoresList;
        private readonly IAccountStoresService _accountStoresService;
        private string _storesForLoad;
        private string _storesError;
        private List<int> _storesNum;
        #endregion Private Fields

        #region Public Properties
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        public bool IsChangeStatusOpen
        {
            get { return _isChangeStatusOpen; }
            set { SetProperty(ref _isChangeStatusOpen, value); }
        }
        public bool IsStatusHistoryOpen
        {
            get { return _isStatusHistoryOpen; }
            set { SetProperty(ref _isStatusHistoryOpen, value); }
        }
        public bool IsEditAccountStoresOpen
        {
            get { return _isEditAccountStoresOpen; }
            set { SetProperty(ref _isEditAccountStoresOpen, value); }
        }
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }
        public AccountsMainSet CurrentAccount
        {
            get { return _currentAccount; }
            set { SetProperty(ref _currentAccount, value); }
        }
        public AccountsStatusDetailsSet CurrentAccountStatus
        {
            get { return _accountStatus; }
            set { SetProperty(ref _accountStatus, value); }
        }
        public AccountsStatusDetailsSet NewAccountStatus
        {
            get { return _newAccountStatus; }
            set
            {
                if (_newAccountStatus != null)
                    NewAccountStatus.PropertyChanged -= NewAccountPropertyChanged;
                SetProperty(ref _newAccountStatus, value);
                if (_newAccountStatus != null)
                    NewAccountStatus.PropertyChanged += NewAccountPropertyChanged;
            }
        }        
        public List<string> StatusesList
        {
            get { return _statusesList; }
            set { SetProperty(ref _statusesList, value); }
        }
        public ObservableCollection<AccountsStatusDetailsSet> StatusHistoryList
        {
            get { return _statusHistoryList; }
            set { SetProperty(ref _statusHistoryList, value); }         
        }
        public ObservableCollection<StoresSet> AccountStoresList
        {
            get { return _accountStoresList; }
            set { SetProperty(ref _accountStoresList, value); }
        }
        public string StoresForLoad
        {
            get { return _storesForLoad; }
            set { SetProperty(ref _storesForLoad, value); }
        }
        public string StoresError
        {
            get { return _storesError; }
            set { SetProperty(ref _storesError, value); }
        }
        #endregion Public Properties

        #region Commands
        public DelegateCommand ChangeStatusCommand { get; set; }
        public DelegateCommand SaveNewStatusCommand { get; set; }
        public DelegateCommand CancelNewStatusCommand { get; set; }
        public DelegateCommand OpenStatusHistoryCommand { get; set; }
        public DelegateCommand EditAccountStoresListCommand { get; set; }
        public DelegateCommand AddStoresToAccountCommand { get; set; }
        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public AdditionalInfoViewModel(IAccountStatusService accountStatusService, IAccountStoresService accountStoresService)
        {
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            IsChangeStatusOpen = false;
            IsStatusHistoryOpen = false;
            IsEditAccountStoresOpen = false;
            StatusesList = Statuses.GetStatusesList();
            StatusHistoryList = new ObservableCollection<AccountsStatusDetailsSet>();
            AccountStoresList = new ObservableCollection<StoresSet>();
            _accountStatusService = accountStatusService;
            _accountStoresService = accountStoresService;

            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccountAdditionalInfo;

            ChangeStatusCommand = new DelegateCommand(ChangeStatus);
            SaveNewStatusCommand = new DelegateCommand(SaveNew, CanSaveNew);
            CancelNewStatusCommand = new DelegateCommand(CancelNew);
            OpenStatusHistoryCommand = new DelegateCommand(OpenHistory);
            EditAccountStoresListCommand = new DelegateCommand(EditAccountStoresList);
            AddStoresToAccountCommand = new DelegateCommand(AddStoresToAccount);
        }        
        #endregion Constructor

        #region Methods
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (navigationContext.Uri == null)
            {
                ConfirmationRequest.Raise(
                    new Confirmation { Content = "Закрыть без сохранения?", Title = "Закрытие вкладки" },
                    c => {                        
                            continuationCallback(c.Confirmed);
                         });
            }
            else
            {
                continuationCallback(true);
            }
        }
        private AccountsMainSet GetAccount(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters[AccountKey];
            var account = (AccountsMainSet)parameter;
            AccountsTabItemHeader = "Информация по сч. " + account.AccountNumber;
            
            return (AccountsMainSet)account;
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentAccount = GetAccount(navigationContext);
            if (CurrentAccount != null)
            {
                _worker.RunWorkerAsync();
            }
        }
        private void LoadAccountAdditionalInfo(object sender, DoWorkEventArgs e)
        {
            LoadAccountLastStatus();
            _worker.DoWork -= LoadAccountAdditionalInfo;
        }
        private void LoadAccountLastStatus()
        {
            CurrentAccountStatus = _accountStatusService.GetAccountStatusById(CurrentAccount.Id);
        }
        private void ChangeStatus()
        {
            NewAccountStatus = new AccountsStatusDetailsSet();
            IsChangeStatusOpen = true;
            IsStatusHistoryOpen = false;
        }
        private void OpenHistory()
        {
            if (!IsStatusHistoryOpen)
            {
                IsStatusHistoryOpen = true;
                IsChangeStatusOpen = false;
                if (!_worker.IsBusy)
                {
                    _worker.DoWork += LoadHistoryList;
                    _worker.RunWorkerAsync();
                }
            }
            else
            {
                IsStatusHistoryOpen = false;
            }
        }
        private void LoadHistoryList(object sender, DoWorkEventArgs e)
        {
            StatusHistoryList.Clear();
            StatusHistoryList = new ObservableCollection<AccountsStatusDetailsSet>(_accountStatusService.GetStatusesById(CurrentAccount.Id));
            _worker.DoWork -= LoadHistoryList;
        }
        private void SaveNew()
        {
            if(NewAccountStatus != null)
            {
                NewAccountStatus.AccountMainId = CurrentAccount.Id;
                _accountStatusService.AddNewStatus(NewAccountStatus);
                CurrentAccountStatus = NewAccountStatus;
                IsChangeStatusOpen = false;
            }
        }
        private void CancelNew()
        {
            IsChangeStatusOpen = false;
        }
        private bool CanSaveNew()
        {
            if (NewAccountStatus == null) return false;
            NewAccountStatus.ValidateProperties();
            return !NewAccountStatus.HasErrors;
        }

        private void EditAccountStoresList()
        {
            if (!IsEditAccountStoresOpen)
            {
                IsEditAccountStoresOpen = true;
                AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(CurrentAccount.Id));
            }
            else
            {
                IsEditAccountStoresOpen = false;
            }
        }
        private void AddStoresToAccount()
        {
            

        }
        private bool CheckStoreErrors()
        {
            var strStores = StoresForLoad.Split('\n');
            _storesNum = new List<int>();
            var stores = new ObservableCollection<StoresSet>();
            foreach (var item in strStores)
            {
                int storeNumber;
                StoresError = string.Empty;
                if (!int.TryParse(item, out storeNumber))
                {
                    StoresError += item + "\n";
                }               
            }
            return !string.IsNullOrWhiteSpace(StoresError);
        }
        private void NewAccountPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveNewStatusCommand.RaiseCanExecuteChanged();
        }
        #endregion Methods
    }
}
