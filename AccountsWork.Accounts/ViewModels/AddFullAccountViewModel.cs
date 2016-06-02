using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using AccountsVork.Infrastructure;
using System.Threading.Tasks;
using System.Windows;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AddFullAccountViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private AccountsMainSet _account;
        private IList<AccountsCompaniesSet> _companies;
        private IList<TypeSet> _types;
        private readonly BackgroundWorker _worker;
        private bool _isInEditMode;
        private bool _isAdditinalInfoEnabled;
        private decimal _availableSum;
        private const string AccountKey = "Account";
        private bool _isAddCapexOpen;
        private AccountsCapexInfoSet _newCapexForAccount;
        private AccountsCapexInfoSet _currentCapex;
        private ObservableCollection<AccountsCapexInfoSet> _accountCapexList;
        private ObservableCollection<AccountsExpenseSet> _expensesList;
        private ObservableCollection<CapexSet> _capexesList;
        private readonly ICompaniesService _companiesService;
        private readonly ITypesService _typesService;
        private readonly IAccountsMainService _accountsService;
        private readonly IAccountStatusService _accountStatusService;
        private readonly IAccountStoresService _accountStoresService;
        private readonly IStoresService _storesService;
        private readonly IAccountCapexesService _accountCapexService;
        private readonly IExpensesService _expenseService;
        private readonly ICapexesService _capexService;
        private readonly IStoresWorkService _storesWorkService;
        private ObservableCollection<AccountsStatusDetailsSet> _statusHistoryList;
        private List<string> _statusesList;
        private bool _isChangeStatusOpen;
        private AccountsStatusDetailsSet _newAccountStatus;
        private StoresSet _currentAccountStore;
        private ObservableCollection<StoresSet> _accountStoresList;
        private bool _isEditAccountStoresOpen;
        private string _storesForLoad;
        private ObservableCollection<StoresSet> _storesList;
        private string _storesError;
        private List<int> storesNum;
        private BackgroundWorker _addStoresWorker;
        private bool _isAddStoreBusy;
        private ObservableCollection<StoreProvenWorkSet> _storesWorkList;
        private StoreProvenWorkSet _selectedWork;
        private string _serchStoreName;
        private string _serchResultStore;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }

        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set { SetProperty(ref _isInEditMode, value); }
        }

        public bool IsAdditinalInfoEnabled
        {
            get { return _isAdditinalInfoEnabled; }
            set { SetProperty(ref _isAdditinalInfoEnabled, value); }
        }
        #endregion infrastructure

        #region account
        public AccountsMainSet Account
        {
            get { return _account; }
            set
            {
                if (_account != null)
                    Account.PropertyChanged -= AccountPropertyChanged;
                SetProperty(ref _account, value);
                if (_account != null)
                    Account.PropertyChanged += AccountPropertyChanged;
            }
        }
        public IList<AccountsCompaniesSet> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }
        public IList<TypeSet> Types
        {
            get { return _types; }
            set { SetProperty(ref _types, value); }
        }
        #endregion account

        #region capexes
        public decimal AvailableSum
        {
            get { return _availableSum; }
            set { SetProperty(ref _availableSum, value); }
        }
        public bool IsAddCapexOpen
        {
            get { return _isAddCapexOpen; }
            set { SetProperty(ref _isAddCapexOpen, value); }
        }
        public AccountsCapexInfoSet NewCapexForAccount
        {
            get { return _newCapexForAccount; }
            set
            {
                if (_newCapexForAccount != null)
                    NewCapexForAccount.PropertyChanged -= NewCapexPropertyChanged;
                SetProperty(ref _newCapexForAccount, value);
                if (_newCapexForAccount != null)
                    NewCapexForAccount.PropertyChanged += NewCapexPropertyChanged;
            }
        }

        public AccountsCapexInfoSet CurrentCapex
        {
            get { return _currentCapex; }
            set { SetProperty(ref _currentCapex, value); }
        }

        public ObservableCollection<AccountsCapexInfoSet> AccountCapexList
        {
            get { return _accountCapexList; }
            set { SetProperty(ref _accountCapexList, value); }
        }
        public ObservableCollection<AccountsExpenseSet> ExpensesList
        {
            get { return _expensesList; }
            set { SetProperty(ref _expensesList, value); }
        }
        public ObservableCollection<CapexSet> CapexesList
        {
            get { return _capexesList; }
            set { SetProperty(ref _capexesList, value); }
        }
        #endregion capexes

        #region statuses
        public bool IsChangeStatusOpen
        {
            get { return _isChangeStatusOpen; }
            set { SetProperty(ref _isChangeStatusOpen, value); }
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
        public ObservableCollection<AccountsStatusDetailsSet> StatusHistoryList
        {
            get { return _statusHistoryList; }
            set { SetProperty(ref _statusHistoryList, value); }
        }
        public List<string> StatusesList
        {
            get { return _statusesList; }
            set { SetProperty(ref _statusesList, value); }
        }
        #endregion statuses

        #region stores
        public bool IsEditAccountStoresOpen
        {
            get { return _isEditAccountStoresOpen; }
            set { SetProperty(ref _isEditAccountStoresOpen, value); }
        }
        public StoresSet CurrentAccountStore
        {
            get { return _currentAccountStore; }
            set { SetProperty(ref _currentAccountStore, value); }
        }
        public ObservableCollection<StoresSet> AccountStoresList
        {
            get { return _accountStoresList; }
            set
            {
                SetProperty(ref _accountStoresList, value);
            }
        }
        public string StoresForLoad
        {
            get { return _storesForLoad; }
            set { SetProperty(ref _storesForLoad, value); }
        }
        public string SearchStoreName
        {
            get { return _serchStoreName; }
            set { SetProperty(ref _serchStoreName, value); }
        }
        public string SerchResultStores
        {
            get { return _serchResultStore; }
            set { SetProperty(ref _serchResultStore, value); }
        }
        public ObservableCollection<StoresSet> StoresList
        {
            get { return _storesList; }
            set { SetProperty(ref _storesList, value); }
        }
        public string StoresError
        {
            get { return _storesError; }
            set { SetProperty(ref _storesError, value); }
        }
        public bool IsAddStoreBusy
        {
            get { return _isAddStoreBusy; }
            set { SetProperty(ref _isAddStoreBusy, value); }
        }
        #endregion stores

        #region work
        public ObservableCollection<StoreProvenWorkSet> StoresWorkList
        {
            get { return _storesWorkList; }
            set { SetProperty(ref _storesWorkList, value); }
        }
        public StoreProvenWorkSet SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                if (_selectedWork != null)
                    SelectedWork.PropertyChanged -= SelectedWorkChanged;
                SetProperty(ref _selectedWork, value);
                if (_selectedWork != null)
                    SelectedWork.PropertyChanged += SelectedWorkChanged;
            }
        }
        #endregion work

        #endregion Public Properties

        #region Commands

        #region account
        public DelegateCommand SaveAccountCommand { get; set; }
        #endregion account

        #region capexes
        public DelegateCommand OpenAddCapexToAccountCommand { get; set; }
        public DelegateCommand CloseAddCapexToAccountCommand { get; set; }
        public DelegateCommand AddCapexToAccountCommand { get; set; }
        public DelegateCommand DeleteCapexAccountCommand { get; set; }
        public DelegateCommand CopyAvailableSumCommand { get; set; }
        #endregion capexes

        #region statuses
        public DelegateCommand SaveNewStatusCommand { get; set; }
        public DelegateCommand ChangeStatusCommand { get; set; }
        public DelegateCommand CancelNewStatusCommand { get; set; }
        #endregion statuses

        #region stores
        public DelegateCommand EditAccountStoresListCommand { get; set; }
        public DelegateCommand DeleteAccountStoreCommand { get; set; }
        public DelegateCommand AddStoresToAccountCommand { get; set; }
        #endregion stores

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public AddFullAccountViewModel(ICompaniesService companiesService, ITypesService typesService, IAccountsMainService accountsService, IAccountStatusService accountStatusService, IAccountStoresService accountStoresService, IStoresService storesService, IAccountCapexesService accountCapexService, IExpensesService expenseService, ICapexesService capexService, IStoresWorkService storesWorkService)
        {


            #region account
            SaveAccountCommand = new DelegateCommand(SaveAccount, CanSave).ObservesProperty(() => Account);
            #endregion account

            #region workers

            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccount;

            _addStoresWorker = new BackgroundWorker();
            _addStoresWorker.DoWork += LoadAddStoresToAccount;
            _addStoresWorker.RunWorkerCompleted += LoadAddStoresToAccount_Completed;
            #endregion workers 

            #region  capexes
            OpenAddCapexToAccountCommand = new DelegateCommand(OpenAddCapexToAccount);
            CloseAddCapexToAccountCommand = new DelegateCommand(CloseAddCapexToAccount);
            CopyAvailableSumCommand = new DelegateCommand(CopyAvailableSum);
            AddCapexToAccountCommand = new DelegateCommand(AddCapexToAccount, CanAddCapex).ObservesProperty(() => NewCapexForAccount);
            #endregion capexes

            #region services
            _companiesService = companiesService;
            _typesService = typesService;
            _accountsService = accountsService;
            _accountStatusService = accountStatusService;
            _accountStoresService = accountStoresService;
            _storesService = storesService;
            _accountCapexService = accountCapexService;
            _expenseService = expenseService;
            _capexService = capexService;
            _storesWorkService = storesWorkService;

            #endregion services

            #region statuses
            IsChangeStatusOpen = false;
            StatusesList = Statuses.GetStatusesList();

            CancelNewStatusCommand = new DelegateCommand(CancelNew);
            ChangeStatusCommand = new DelegateCommand(ChangeStatus);
            SaveNewStatusCommand = new DelegateCommand(SaveNew, CanSaveNew);
            #endregion statuses

            #region stores
            EditAccountStoresListCommand = new DelegateCommand(EditAccountStoresList, CanEdit);
            DeleteAccountStoreCommand = new DelegateCommand(DeleteAccountStore);
            AddStoresToAccountCommand = new DelegateCommand(() => _addStoresWorker.RunWorkerAsync(), CheckStoreErrors).ObservesProperty(() => StoresForLoad);
            IsEditAccountStoresOpen = false;
            #endregion stores

        }
        #endregion Constructor

        #region Methods

        #region infrastructure

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Companies = _companiesService.GetCompanies();
            Types = _typesService.GetTypes();
            Account = GetAccount(navigationContext);
            if (Account != null)
            {
                AccountsTabItemHeader = "Ред. информации по сч. " + Account.AccountNumber;
                IsAdditinalInfoEnabled = true;
            }
            else
            {
                AccountsTabItemHeader = "Новый счет";
                Account = new AccountsMainSet
                {
                    AccountYear = DateTime.Now.Year,
                    AccountDate = DateTime.Now
                };
                IsAdditinalInfoEnabled = false;
                IsInEditMode = true;
                AvailableSum = 0M;               
            }
            _worker.RunWorkerAsync();
        }
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        #endregion infrastructure

        #region account
        private void LoadAccount(object sender, DoWorkEventArgs e)
        {            
            ExpensesList = new ObservableCollection<AccountsExpenseSet>(_expenseService.GetExpensesList());
            if (Account.Id != 0)
            {
                LoadCapexInfo();
                LoadHistoryStatus(Account.Id);
                AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(Account.Id));
                StoresWorkList = new ObservableCollection<StoreProvenWorkSet>(_storesWorkService.GetWorksList(AccountStoresList, false));
            }
            else
            {
                AccountCapexList = new ObservableCollection<AccountsCapexInfoSet>();
                AccountStoresList = new ObservableCollection<StoresSet>();
                StoresWorkList = new ObservableCollection<StoreProvenWorkSet>();
            }
        }
        private AccountsMainSet GetAccount(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters[AccountKey];
            var account = (AccountsMainSet)parameter;
            return account;
        }
        private void AccountPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveAccountCommand.RaiseCanExecuteChanged();
        }
        private void SaveAccount()
        {
            Account.Id = _accountsService.SaveAccount(Account);
            IsAdditinalInfoEnabled = true;
            CapexesList = new ObservableCollection<CapexSet>(_capexService.GetCapexesForYearList(Account.AccountYear));
            IsInEditMode = false;
            LoadCapexInfo();
        }
        private bool CanSave()
        {
            if (Account == null)
            {
                IsInEditMode = false;
                return false;
            }           
            Account.ValidateProperties();
            IsInEditMode = !Account.HasErrors;
            return !Account.HasErrors;
        }
        #endregion account

        #region capexes
        private void OpenAddCapexToAccount()
        {
            IsAddCapexOpen = true;
            NewCapexForAccount = new AccountsCapexInfoSet { AccountsMainId = Account.Id};
        }
        private void CloseAddCapexToAccount()
        {
            IsAddCapexOpen = false;
        }
        private void CopyAvailableSum()
        {
            NewCapexForAccount.AccountCapexAmount = AvailableSum;
        }
        private void AddCapexToAccount()
        {
            if (NewCapexForAccount != null)
            {
                NewCapexForAccount.CapexId = _capexService.GetCapexIdByName(NewCapexForAccount.AccountCapexName, Account.AccountYear);
                _accountCapexService.AddCapexToAccount(NewCapexForAccount);
                LoadCapexInfo();
                IsAddCapexOpen = false;
            }
        }
        private bool CanAddCapex()
        {
            if (NewCapexForAccount == null) return false;
            NewCapexForAccount.ValidateProperties();
            if (NewCapexForAccount.AccountCapexAmount > AvailableSum)
                return false;
            return !NewCapexForAccount.HasErrors;
        }
        private void LoadCapexInfo()
        {
            AccountCapexList = new ObservableCollection<AccountsCapexInfoSet>(_accountCapexService.GetCapexesById(Account.Id));
            AvailableSum = Account.AccountAmount.Value - AccountCapexList.Sum(c => c.AccountCapexAmount);
        }
        private void NewCapexPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AddCapexToAccountCommand.RaiseCanExecuteChanged();
        }
        #endregion capexes

        #region statuses
        private void LoadHistoryStatus(int id)
        {
            StatusHistoryList = new ObservableCollection<AccountsStatusDetailsSet>(_accountStatusService.GetStatusesById(id));
        }
        private void NewAccountPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveNewStatusCommand.RaiseCanExecuteChanged();
        }
        private void SaveNew()
        {
            if (NewAccountStatus == null) return;
            NewAccountStatus.AccountMainId = Account.Id;
            _accountStatusService.AddNewStatus(NewAccountStatus);
            IsChangeStatusOpen = false;
        }
        private bool CanSaveNew()
        {
            if (NewAccountStatus == null) return false;
            NewAccountStatus.ValidateProperties();
            return !NewAccountStatus.HasErrors;
        }
        private void ChangeStatus()
        {
            NewAccountStatus = new AccountsStatusDetailsSet();
            IsChangeStatusOpen = true;
        }
        private void CancelNew()
        {
            IsChangeStatusOpen = false;
        }
        #endregion statuses

        #region stores
        private void EditAccountStoresList()
        {
                IsEditAccountStoresOpen = true;
                StoresForLoad = string.Empty;
                StoresList = new ObservableCollection<StoresSet>(_storesService.GetStores());
                Task.Run(() => AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(Account.Id)));
        }
        private void DeleteAccountStore()
        {
            if (CurrentAccountStore == null) return;
            _accountStoresService.DeleteStoreFromAccount(CurrentAccountStore.StoreNumber, Account.Id);
            AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(Account.Id));
        }
        private bool CanEdit()
        {
            return !IsAddStoreBusy;
        }
        private bool CheckStoreErrors()
        {
            if (!string.IsNullOrWhiteSpace(StoresForLoad))
            {
                var strStores = StoresForLoad.Split('\n');
                storesNum = new List<int>();
                StoresError = string.Empty;
                foreach (var item in strStores)
                {
                    int storeNumber;
                    if (string.IsNullOrWhiteSpace(item))
                        continue;
                    if (!int.TryParse(item, out storeNumber))
                    {
                        StoresError += item.Trim() + " ошибка\n";
                    }
                    else
                    {
                        if (StoresList.Any(s => s.StoreNumber == storeNumber))
                        {
                            storesNum.Add(storeNumber);
                        }
                        else
                        {
                            StoresError += storeNumber + " не заведен в базе\n";
                        }
                    }
                }
                return string.IsNullOrWhiteSpace(StoresError);
            }
            StoresError = string.Empty;
            return false;
        }
        private void SearchStoreNumberByName()
        {
            if (!string.IsNullOrWhiteSpace(SearchStoreName))
            {
                SerchResultStores = string.Empty;
                foreach (var item in StoresList.Where(s => s.StoreName.ToLower().Contains(SearchStoreName.ToLower())))
                {
                    SerchResultStores += $"{item.StoreNumber} {item.StoreName}\n";
                }
            }
            else
                SerchResultStores = string.Empty;
        }
        private void LoadAddStoresToAccount(object sender, DoWorkEventArgs e)
        {
            IsAddStoreBusy = true;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => EditAccountStoresListCommand.RaiseCanExecuteChanged()));
            AddStoresToAccount(storesNum);
        }
        private void LoadAddStoresToAccount_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsAddStoreBusy = false;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => EditAccountStoresListCommand.RaiseCanExecuteChanged()));
        }
        private void AddStoresToAccount(List<int> storesNumbers)
        {
            if (storesNumbers.Count <= 0) return;
            var storesForAddList = new ObservableCollection<AccountsStoreDetailsSet>();
            foreach (var storeNumber in storesNumbers.Where(storeNumber => AccountStoresList.All(s => s.StoreNumber != storeNumber) && storesForAddList.All(s => s.AccountStore != storeNumber)))
            {
                storesForAddList.Add(new AccountsStoreDetailsSet { AccountsMainId = Account.Id, AccountStore = storeNumber });
            }
            if (storesForAddList.Count == 0) return;
            _accountStoresService.AddStoresToAccount(storesForAddList);
            AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(Account.Id));
            StoresWorkList = new ObservableCollection<StoreProvenWorkSet>(_storesWorkService.GetWorksList(AccountStoresList, false));
            Application.Current.Dispatcher.BeginInvoke(new Action(() => StoresForLoad = string.Empty));
        }
        #endregion stores

        #region work
        private void SelectedWorkChanged(object sender, PropertyChangedEventArgs e)
        {
            var work = sender as StoreProvenWorkSet;
            if (work != null)
            {
                _storesWorkService.UpdateWork(work, Account.Id);
                StoresWorkList = new ObservableCollection<StoreProvenWorkSet>(_storesWorkService.GetWorksList(AccountStoresList, false));
            }
        }
        #endregion work

        #endregion Methods




    }
}