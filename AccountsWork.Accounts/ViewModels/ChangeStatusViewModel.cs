using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using AccountsWork.BusinessLayer;
using Prism.Regions;
using Prism.Commands;
using System;
using System.Linq;
using System.Collections.Generic;
using AccountsVork.Infrastructure;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class ChangeStatusViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private ObservableCollection<AccountsMainSet> _accountsList;
        private BackgroundWorker _worker;
        private IAccountsMainService _accountsMainService;
        private bool _isChangeStatusBusy;
        private string _searchAccountText;
        private ObservableCollection<AccountsMainSet> _searchAccountList;
        private AccountsMainSet _selectedSearchAccount;
        private ObservableCollection<AccountsMainSet> _accountForChangeList;
        private List<string> _statusesList;
        private string _selectedStatus;
        private DateTime _accountForChangeDate;
        private IAccountStatusService _accountStatusService;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        #endregion infrastrcture

        #region statuses
        public ObservableCollection<AccountsMainSet> AccountsList
        {
            get { return _accountsList; }
            set { SetProperty(ref _accountsList, value); }
        }
        public ObservableCollection<AccountsMainSet> SearchAccountList
        {
            get { return _searchAccountList; }
            set { SetProperty(ref _searchAccountList, value); }
        }
        public ObservableCollection<AccountsMainSet> AccountForChangeList
        {
            get { return _accountForChangeList; }
            set
            {
                SetProperty(ref _accountForChangeList, value);                
            }
        }
        public List<string> StatusesList
        {
            get { return _statusesList; }
            set { SetProperty(ref _statusesList, value); }
        }
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { SetProperty(ref _selectedStatus, value); }
        }
        public DateTime AccountForChangeDate
        {
            get { return _accountForChangeDate; }
            set { SetProperty(ref _accountForChangeDate, value); }
        }
        public bool IsChangeStatusBusy
        {
            get { return _isChangeStatusBusy; }
            set { SetProperty(ref _isChangeStatusBusy, value); }
        }
        public string SearchAccountText
        {
            get { return _searchAccountText; }
            set { SetProperty(ref _searchAccountText, value); }
        }
        public AccountsMainSet SelectedSearchAccount
        {
            get { return _selectedSearchAccount; }
            set { SetProperty(ref _selectedSearchAccount, value); }
        }
        #endregion statuses

        #endregion Public Properties

        #region Commands

        #region statuses
        public DelegateCommand AddAccountForChangeCommand { get; set; }
        public DelegateCommand SearchAccountNumberCommand { get; set; }
        public DelegateCommand SelectAccountCommand { get; set; }
        public DelegateCommand ChangeStatusCommand { get; set; }
        #endregion statuses

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public ChangeStatusViewModel(IAccountsMainService accountsMainService, IAccountStatusService accountStatusService)
        {
            #region infrastructure
            AccountsTabItemHeader = "Изменение статусов";
            #endregion infrastructure

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAllAccounts;
            _worker.DoWork += LoadAllAccounts_Completed;
            #endregion workers

            #region services
            _accountsMainService = accountsMainService;
            _accountStatusService = accountStatusService;
            #endregion services

            #region statuses
            SearchAccountNumberCommand = new DelegateCommand(SearchAccount);
            SelectAccountCommand = new DelegateCommand(SelectAccount);
            ChangeStatusCommand = new DelegateCommand(ChangeStatus, CanChange).ObservesProperty(() => SelectedStatus).ObservesProperty(() => AccountForChangeDate);            
            AccountForChangeList = new ObservableCollection<AccountsMainSet>();
            
            #endregion statuses
        }        
        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SearchAccountText = string.Empty;
            SearchAccountList = new ObservableCollection<AccountsMainSet>();
            AccountForChangeList = new ObservableCollection<AccountsMainSet>();
            AccountForChangeDate = DateTime.Now;
            StatusesList = Statuses.GetStatusesList();
            SelectedStatus = string.Empty;
            ChangeStatusCommand.RaiseCanExecuteChanged();
            _worker.RunWorkerAsync();
        }
        #endregion infrastructure

        #region statuses
        private void LoadAllAccounts(object sender, DoWorkEventArgs e)
        {
            IsChangeStatusBusy = true;
            AccountsList = new ObservableCollection<AccountsMainSet>(_accountsMainService.GetAllAccounts());
        }
        private void LoadAllAccounts_Completed(object sender, DoWorkEventArgs e)
        {
            IsChangeStatusBusy = false;
        }
        private void SearchAccount()
        {
            if (!string.IsNullOrWhiteSpace(SearchAccountText))
            {
                SearchAccountList = new ObservableCollection<AccountsMainSet>(AccountsList.Where(a => a.AccountNumber.Contains(SearchAccountText)));
            }
            else
                SearchAccountList.Clear();
        }
        private void SelectAccount()
        {
            if (SelectedSearchAccount != null)
            {
                AccountForChangeList.Add(SelectedSearchAccount);
                SearchAccountText = string.Empty;
                ChangeStatusCommand.RaiseCanExecuteChanged();
            }
        }
        private bool CanChange()
        {
            return AccountForChangeList.Count != 0 && !string.IsNullOrWhiteSpace(SelectedStatus) && AccountForChangeDate != null;
        }
        private void ChangeStatus()
        {
                _accountStatusService.UpdateStatus(AccountForChangeList, SelectedStatus, AccountForChangeDate);
                AccountForChangeList.Clear();
                SearchAccountText = string.Empty;
                AccountForChangeDate = DateTime.Now;
        }
        #endregion statuses
        #endregion Methods
    }
}
