using AccountsVork.Infrastructure;
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
        private BackgroundWorker _worker;
        private IAccountStatusService _accountStatusService;
        private bool _isChangeStatusOpen;
        private List<string> _statusesList;
        private AccountsStatusDetailsSet _newAccountStatus;
        private bool _isStatusHistoryOpen;
        private ObservableCollection<AccountsStatusDetailsSet> _statusHistoryList;
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
        #endregion Public Properties

        #region Commands
        public DelegateCommand ChangeStatusCommand { get; set; }
        public DelegateCommand SaveNewStatusCommand { get; set; }
        public DelegateCommand CancelNewStatusCommand { get; set; }
        public DelegateCommand OpenStatusHistoryCommand { get; set; }
        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public AdditionalInfoViewModel(IAccountStatusService accountStatusService)
        {
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            IsChangeStatusOpen = false;
            IsStatusHistoryOpen = false;
            StatusesList = Statuses.GetStatusesList();
            StatusHistoryList = new ObservableCollection<AccountsStatusDetailsSet>();
            _accountStatusService = accountStatusService;

            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccountAdditionalInfo;

            ChangeStatusCommand = new DelegateCommand(ChangeStatus);
            SaveNewStatusCommand = new DelegateCommand(SaveNew, CanSaveNew);
            CancelNewStatusCommand = new DelegateCommand(CancelNew);
            OpenStatusHistoryCommand = new DelegateCommand(OpenHistory);
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
            IsStatusHistoryOpen = true;
            IsChangeStatusOpen = false;
            if (!_worker.IsBusy)
            {
                _worker.DoWork += LoadHistoryList;
                _worker.RunWorkerAsync();
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
            if (NewAccountStatus != null)
            {
                NewAccountStatus.ValidateProperties();
                return !NewAccountStatus.HasErrors;
            }
            else
                return false;
        }
        private void NewAccountPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveNewStatusCommand.RaiseCanExecuteChanged();
        }
        #endregion Methods
    }
}
