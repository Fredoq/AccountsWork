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
using AccountsVork.Infrastructure;
using AccountsWork.ExcelReports;
using Prism.Events;
using AccountsWork.Accounts.Controllers;
using AccountsWork.Accounts.Events;
using Syncfusion.Data;

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
        private int _currentYear;
        private int _startYearStatus;
        private int _endYearStatus;
        private int _payedCount;
        private int _accCount;
        private int _acc10Count;
        private bool _isAth;
        private ObservableCollection<AccountsExt> _accountSelectedList;
        private int _workCount;
        private int _work10Count;
        private int _poCount;
        private int _po10Count;
        private int _returnCount;
        private int _cancelCount;
        private IList<StatusChoice> _statusChoicesList;
        private IExcelReportService _excelReportService;
        private string _filename;
        private IEventAggregator _eventAggregator;
        private AccountsController _accountsController;
        private AccountsExt _selectedAccountExt;
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
        public int CurrentYear
        {
            get { return _currentYear; }
            set { SetProperty(ref _currentYear, value); }
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

        #region statuses
        public int StartYearStatus
        {
            get { return _startYearStatus; }
            set { SetProperty(ref _startYearStatus, value); }
        }
        public int EndYearStatus
        {
            get { return _endYearStatus; }
            set { SetProperty(ref _endYearStatus, value); }
        }
        public bool IsATH
        {
            get { return _isAth; }
            set { SetProperty(ref _isAth, value); }
        }
        public int PayedCount
        {
            get { return _payedCount; }
            set { SetProperty(ref _payedCount, value); }
        }
        public ObservableCollection<AccountsExt> AccountSelectedList
        {
            get { return _accountSelectedList; }
            set { SetProperty(ref _accountSelectedList, value); }
        }
        public AccountsExt SelectedAccountExt
        {
            get { return _selectedAccountExt; }
            set { SetProperty(ref _selectedAccountExt, value); }
        }
        public int AccCount
        {
            get { return _accCount; }
            set { SetProperty(ref _accCount, value); }
        }
        public int Acc10Count
        {
            get { return _acc10Count; }
            set { SetProperty(ref _acc10Count, value); }
        }
        public int WorkCount
        {
            get { return _workCount; }
            set { SetProperty(ref _workCount, value); }
        }
        public int Work10Count
        {
            get { return _work10Count; }
            set { SetProperty(ref _work10Count, value); }
        }
        public int POCount
        {
            get { return _poCount; }
            set { SetProperty(ref _poCount, value); }
        }
        public int PO10Count
        {
            get { return _po10Count; }
            set { SetProperty(ref _po10Count, value); }
        }
        public int ReturnCount
        {
            get { return _returnCount; }
            set { SetProperty(ref _returnCount, value); }
        }
        public int CancelCount
        {
            get { return _cancelCount; }
            set { SetProperty(ref _cancelCount, value); }
        }
        public IList<StatusChoice> StatusChoicesList
        {
            get { return _statusChoicesList; }
            set { SetProperty(ref _statusChoicesList, value); }
        }
        #endregion statuses

        #endregion Public Properties

        #region Commands

        #region infrastructure
        public DelegateCommand<object> NavigateCommand { get; set; }
        public DelegateCommand ExportToExcelCommand { get; set; }
        #endregion infrastructure

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public InfoViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager, IExcelReportService excelReportService, IEventAggregator eventAggregator, AccountsController accountsController)
        {
            #region infrastructure
            AccountsTabItemHeader = "Общая информация";
            _regionManager = regionManager;
            AccountList = new List<AccountsMainSet>();
            NavigateCommand = new DelegateCommand<object>(Navigate);
            CurrentYear = DateTime.Now.Year;
            _accountsController = accountsController;
            #endregion infrastructure

            #region services
            _accountsMainService = accountsMainService;
            _excelReportService = excelReportService;
            #endregion services

            #region events
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<SaveFileEvent>().Subscribe(GetFilename);
            #endregion events

            #region errors
            StoreErrorList = new ObservableCollection<AccountsMainSet>();
            CapexErrorList = new ObservableCollection<AccountsMainSet>();

            #endregion errors

            #region statuses
            StartYearStatus = 2010;
            EndYearStatus = CurrentYear;
            IsATH = false;
            StatusChoicesList = new List<StatusChoice> { new StatusChoice { Status = Statuses.InAcc},
                                                         new StatusChoice { Status = Statuses.InCancel },
                                                         new StatusChoice { Status = Statuses.InPayed },
                                                         new StatusChoice { Status = Statuses.InPO },
                                                         new StatusChoice { Status = Statuses.InReturn },
                                                         new StatusChoice { Status = Statuses.InWork } };
            AccountSelectedList = new ObservableCollection<AccountsExt>();
            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            #endregion statuses

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
            LoadStatuses();
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
        private void GetFilename(string obj)
        {
            _filename = obj;
        }
        #endregion infrastructure

        #region statuses
        public void LoadStatuses()
        {
            PayedCount = ChooseAccount(Statuses.InPayed, IsATH, AccountList, 0).Count;
            AccCount = ChooseAccount(Statuses.InAcc, IsATH, AccountList, 0).Count;
            Acc10Count = ChooseAccount(Statuses.InAcc, IsATH, AccountList, 10).Count;
            WorkCount = ChooseAccount(Statuses.InWork, IsATH, AccountList, 0).Count;
            Work10Count = ChooseAccount(Statuses.InWork, IsATH, AccountList, 10).Count;
            POCount = ChooseAccount(Statuses.InPO, IsATH, AccountList, 0).Count;
            PO10Count = ChooseAccount(Statuses.InPO, IsATH, AccountList, 10).Count;
            ReturnCount = ChooseAccount(Statuses.InReturn, IsATH, AccountList, 0).Count;
            CancelCount = ChooseAccount(Statuses.InCancel, IsATH, AccountList, 0).Count;
        }
        private IList<AccountsMainSet> ChooseAccount(string status, bool isAth, IList<AccountsMainSet> accounts, int days)
        {
            if (isAth)
            {
                return accounts.Where(a => a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus == status && a.AccountYear >= StartYearStatus && a.AccountYear <= EndYearStatus && DateTime.Now - a.AccountsStatusDetailsSets.LastOrDefault().AccountStatusDate >= new TimeSpan(days, 0, 0, 0)).ToList();
            }
            else
            {
                return accounts.Where(a => a.AccountCompany != "ЭйТиЭйч" && a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus == status && a.AccountYear >= StartYearStatus && a.AccountYear <= EndYearStatus && DateTime.Now - a.AccountsStatusDetailsSets.LastOrDefault().AccountStatusDate >= new TimeSpan(days, 0, 0, 0)).ToList();
            }
        }
        private IList<AccountsExt> ChooseAccountExt(string status, bool isAth, IList<AccountsMainSet> accounts)
        {
            if (isAth)
            {
                return (from a in accounts
                        where a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus == status && a.AccountYear >= StartYearStatus && a.AccountYear <= EndYearStatus
                        select new AccountsExt { Account = a, LastStatus = a.AccountsStatusDetailsSets.LastOrDefault() }).ToList();
            }
            else
            {
                return (from a in accounts
                        where a.AccountCompany != "ЭйТиЭйч" && a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus == status && a.AccountYear >= StartYearStatus && a.AccountYear <= EndYearStatus 
                        select new AccountsExt { Account = a, LastStatus = a.AccountsStatusDetailsSets.LastOrDefault() }).ToList();
            }
        }              
        public void LoadStatusesForChoices()
        {
            AccountSelectedList.Clear();
            foreach (var status in StatusChoicesList.Where(s => s.IsChecked))
            {
                foreach (var acc in ChooseAccountExt(status.Status, IsATH, AccountList))
                    AccountSelectedList.Add(acc);
            }
        }
        private void ExportToExcel()
        {
            var report = _excelReportService.CreateAccountWithStatusesReport(new ObservableCollection<AccountsMainSet>(AccountSelectedList.Select(a => a.Account)));
            if (report != null)
            {
                _accountsController.SaveDialogWindow();
                if (!string.IsNullOrWhiteSpace(_filename))
                    _excelReportService.SaveReport(_filename, report);
            }
        }
        #endregion statuses

        #endregion Methods
    }
    public class StatusChoice
    {
        public string Status { get; set; }
        public bool IsChecked { get; set; } = false;
    }

    public class AccountsExt
    {
        public AccountsMainSet Account { get; set; }
        public AccountsStatusDetailsSet LastStatus { get; set; }
    }
    public class CustomAggregate : ISummaryAggregate
    {
        public CustomAggregate()
        {
        }
        public decimal PositiveSummation { get; set; }

        public Action<System.Collections.IEnumerable, string, System.ComponentModel.PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<AccountsExt>;
                foreach (var item in enumerableItems)
                {
                    this.PositiveSummation += item.Account.AccountAmount.Value;
                }
            };
        }
    }
}