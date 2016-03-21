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
using Prism.Interactivity.InteractionRequest;
using AccountsWork.ExcelReports;
using Prism.Events;
using AccountsWork.Accounts.Events;
using AccountsWork.Accounts.Controllers;

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
        private string _accountPayNumber;
        private bool _isPayNumberVisible;
        private IExcelReportService _excelReportService;
        private IEventAggregator _eventAggregator;
        private string _filename;
        private AccountsController _accountsController;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        public InteractionRequest<IConfirmation>  ExportConfirmationRequest { get; set; }
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
            set
            {
                SetProperty(ref _selectedStatus, value);
                if (value == Statuses.InPayed)
                    IsPayNumberVisible = true;
                else
                {
                    AccountPayNumber = string.Empty;
                    IsPayNumberVisible = false;
                }
            }
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
        public string AccountPayNumber
        {
            get { return _accountPayNumber; }
            set { SetProperty(ref _accountPayNumber, value); }
        }
        public bool IsPayNumberVisible
        {
            get { return _isPayNumberVisible; }
            set { SetProperty(ref _isPayNumberVisible, value); }
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
        public ChangeStatusViewModel(IAccountsMainService accountsMainService, IAccountStatusService accountStatusService, IEventAggregator eventAggregator, AccountsController accountsController, IExcelReportService excelReportService)
        {
            #region infrastructure
            AccountsTabItemHeader = "Изменение статусов";
            ExportConfirmationRequest = new InteractionRequest<IConfirmation>();
            _accountsController = accountsController;
            #endregion infrastructure

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAllAccounts;
            _worker.DoWork += LoadAllAccounts_Completed;
            #endregion workers

            #region services
            _accountsMainService = accountsMainService;
            _accountStatusService = accountStatusService;
            _excelReportService = excelReportService;
            #endregion services

            #region events
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<SaveFileEvent>().Subscribe(GetFilename);
            #endregion events

            #region statuses
            SearchAccountNumberCommand = new DelegateCommand(SearchAccount);
            SelectAccountCommand = new DelegateCommand(SelectAccount);
            ChangeStatusCommand = new DelegateCommand(ChangeStatus, CanChange).ObservesProperty(() => SelectedStatus).ObservesProperty(() => AccountForChangeDate).ObservesProperty(() => AccountPayNumber);            
            AccountForChangeList = new ObservableCollection<AccountsMainSet>();
            
            #endregion statuses
        }        
        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            SearchAccountText = string.Empty;
            IsPayNumberVisible = false;
            SearchAccountList = new ObservableCollection<AccountsMainSet>();
            AccountForChangeList = new ObservableCollection<AccountsMainSet>();
            AccountForChangeDate = DateTime.Now;
            StatusesList = Statuses.GetStatusesList();
            SelectedStatus = string.Empty;
            ChangeStatusCommand.RaiseCanExecuteChanged();
            _worker.RunWorkerAsync();
        }

        private void GetFilename(string obj)
        {
            _filename = obj;
        }
        #endregion infrastructure

        #region statuses
        private void LoadAllAccounts(object sender, DoWorkEventArgs e)
        {
            IsChangeStatusBusy = true;
            AccountsList = new ObservableCollection<AccountsMainSet>(_accountsMainService.GetAllAccountsWithStores());
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
            int payNumber;
            if (SelectedStatus != Statuses.InPayed)
                return AccountForChangeList.Count != 0 && !string.IsNullOrWhiteSpace(SelectedStatus) && AccountForChangeDate != null;
            else
                if (int.TryParse(AccountPayNumber, out payNumber))
                    return AccountForChangeList.Count != 0 && !string.IsNullOrWhiteSpace(SelectedStatus) && AccountForChangeDate != null;
                else
                    return false;
        }
        private void ChangeStatus()
        {
            if (string.IsNullOrWhiteSpace(AccountPayNumber))
            {
                _accountStatusService.UpdateStatus(AccountForChangeList, SelectedStatus, AccountForChangeDate);
            }
            else
            {
                int payNumber;
                if (int.TryParse(AccountPayNumber, out payNumber))
                {
                    _accountStatusService.UpdateStatus(AccountForChangeList, SelectedStatus, AccountForChangeDate, payNumber);
                }
            }
            ExportConfirmationRequest.Raise(new Confirmation { Title = "Экспорт", Content = "Выгрузить в Excel?" },
                c =>
                {
                    if (c.Confirmed)
                    {
                        var report = _excelReportService.CreateNewStatusesReport(AccountForChangeList);
                        if (report != null)
                        {
                            _accountsController.SaveDialogWindow();
                            if (!string.IsNullOrWhiteSpace(_filename))
                                _excelReportService.SaveReport(_filename, report);
                        }
                    }
                });
            AccountForChangeList.Clear();
            SearchAccountText = string.Empty;
            AccountPayNumber = string.Empty;
            AccountForChangeDate = DateTime.Now;
            
        }
        #endregion statuses
        #endregion Methods
    }
}
