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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
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
        private bool _isEditAccountStoresOpen;
        private ObservableCollection<StoresSet> _accountStoresList;
        private IAccountStoresService _accountStoresService;
        private string _storesForLoad;
        private string _storesError;
        private List<int> storesNum;
        private ObservableCollection<StoresSet> _addingStoresList;
        private IStoresService _storesService;
        private StoresSet _currentAccountStore;
        private int _storesCount;
        private string _serchResultStore;
        private string _serchStoreName;
        private ObservableCollection<StoresSet> _storesList;
        private BackgroundWorker _addStoresWorker;
        private bool _isAddStoreBusy;
        private decimal _availableSum;
        private ObservableCollection<AccountsCapexInfoSet> _accountCapexList;
        private IAccountCapexesService _accountCapexService;
        private AccountsCapexInfoSet _newCapexForAccount;
        private ObservableCollection<AccountsExpenseSet> _expensesList;
        private ObservableCollection<CapexSet> _capexesList;
        private bool _isAddCapexOpen;
        private IExpensesService _expenseService;
        private ICapexesService _capexService;
        private decimal _capexAmount;
        private AccountsCapexInfoSet _currentCapex;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }
        public AccountsMainSet CurrentAccount
        {
            get { return _currentAccount; }
            set { SetProperty(ref _currentAccount, value); }
        }
        #endregion infrastructure

        #region statuses
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
        #endregion statuses

        #region stores
        public bool IsEditAccountStoresOpen
        {
            get { return _isEditAccountStoresOpen; }
            set { SetProperty(ref _isEditAccountStoresOpen, value); }
        } 
        public int StoresCount
        {
            get { return _storesCount; }
            set { SetProperty(ref _storesCount, value); }
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
                StoresCount = value?.Count ?? 0;
            }
        }
        public ObservableCollection<StoresSet> AddingStoresList
        {
            get { return _addingStoresList; }
            set { SetProperty(ref _addingStoresList, value); }
        }
        public ObservableCollection<StoresSet> StoresList
        {
            get { return _storesList; }
            set { SetProperty(ref _storesList, value); }
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
        public bool IsAddStoreBusy
        {
            get { return _isAddStoreBusy; }
            set { SetProperty(ref _isAddStoreBusy, value); }
        }
        #endregion stores

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
            set { SetProperty(ref _newCapexForAccount, value); }
        }
        public AccountsCapexInfoSet CurrentCapex
        {
            get { return _currentCapex; }
            set { SetProperty(ref _currentCapex, value); }
        }
        public decimal CapexAmount
        {
            get { return _capexAmount; }
            set { SetProperty(ref _capexAmount, value); }
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

        #endregion Public Properties

        #region Commands

        #region statuses
        public DelegateCommand ChangeStatusCommand { get; set; }
        public DelegateCommand SaveNewStatusCommand { get; set; }
        public DelegateCommand CancelNewStatusCommand { get; set; }
        public DelegateCommand OpenStatusHistoryCommand { get; set; }
        #endregion statuses

        #region stores
        public DelegateCommand EditAccountStoresListCommand { get; set; }
        public DelegateCommand AddStoresToAccountCommand { get; set; }
        public DelegateCommand DeleteAccountStoreCommand { get; set; }
        public DelegateCommand SearchStoreNumberByNameCommand { get; set; }
        #endregion stores

        #region capexes
        public DelegateCommand OpenAddCapexToAccountCommand { get; set; }
        public DelegateCommand AddCapexToAccountCommand { get; set; }
        public DelegateCommand DeleteCapexAccountCommand { get; set; }
        public DelegateCommand CopyAvailableSumCommand { get; set; }
        #endregion capexes

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public AdditionalInfoViewModel(IAccountStatusService accountStatusService, IAccountStoresService accountStoresService, IStoresService storesService, IAccountCapexesService accountCapexService, IExpensesService expenseService, ICapexesService capexService)
        {
            #region infrastrcture
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            #endregion infrastructure

            #region statuses
            IsChangeStatusOpen = false;
            IsStatusHistoryOpen = false;

            StatusesList = Statuses.GetStatusesList();
            StatusHistoryList = new ObservableCollection<AccountsStatusDetailsSet>();

            ChangeStatusCommand = new DelegateCommand(ChangeStatus);
            SaveNewStatusCommand = new DelegateCommand(SaveNew, CanSaveNew);
            CancelNewStatusCommand = new DelegateCommand(CancelNew);
            OpenStatusHistoryCommand = new DelegateCommand(OpenHistory);
            #endregion statuses

            #region stores
            IsEditAccountStoresOpen = false;

            AccountStoresList = new ObservableCollection<StoresSet>();
            StoresList = new ObservableCollection<StoresSet>();

            EditAccountStoresListCommand = new DelegateCommand(EditAccountStoresList, CanEdit);
            AddStoresToAccountCommand = new DelegateCommand(() => _addStoresWorker.RunWorkerAsync(), CheckStoreErrors).ObservesProperty(() => StoresForLoad);
            DeleteAccountStoreCommand = new DelegateCommand(DeleteAccountStore);
            SearchStoreNumberByNameCommand = new DelegateCommand(SearchStoreNumberByName);
            #endregion stores

            #region  services
            _accountStatusService = accountStatusService;
            _accountStoresService = accountStoresService;
            _storesService = storesService;
            _accountCapexService = accountCapexService;
            _expenseService = expenseService;
            _capexService = capexService;
            #endregion services

            #region capexes
            AccountCapexList = new ObservableCollection<AccountsCapexInfoSet>();
            ExpensesList = new ObservableCollection<AccountsExpenseSet>(expenseService.GetExpensesList());
            CapexesList = new ObservableCollection<CapexSet>();          
            IsAddCapexOpen = false;

            OpenAddCapexToAccountCommand = new DelegateCommand(OpenAddCapexToAccount);
            CopyAvailableSumCommand = new DelegateCommand(CopyAvailableSum);
            AddCapexToAccountCommand = new DelegateCommand(AddCapexToAccount);
            DeleteCapexAccountCommand = new DelegateCommand(DeleteCapexAccount);
            #endregion capexes

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccountAdditionalInfo;

            _addStoresWorker = new BackgroundWorker();
            _addStoresWorker.DoWork += LoadAddStoresToAccount;
            _addStoresWorker.RunWorkerCompleted += LoadAddStoresToAccount_Completed;            
            #endregion workers                        
        }        
        #endregion Constructor

        #region Methods

        #region infrastructure
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
        private void LoadAccountAdditionalInfo(object sender, DoWorkEventArgs e)
        {
            LoadAccountLastStatus();
            LoadCapexInfo();
            _worker.DoWork -= LoadAccountAdditionalInfo;
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
        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        #endregion infrastructure

        #region statuses
        private void LoadAccountLastStatus()
        {
            CurrentAccountStatus = _accountStatusService.GetAccountStatusById(CurrentAccount.Id);
        }
        private void ChangeStatus()
        {
            NewAccountStatus = new AccountsStatusDetailsSet();
            IsChangeStatusOpen = !IsChangeStatusOpen;
            IsStatusHistoryOpen = false;
        }
        private void OpenHistory()
        {
            if (!IsStatusHistoryOpen)
            {
                IsStatusHistoryOpen = true;
                IsChangeStatusOpen = false;
                if (_worker.IsBusy) return;
                _worker.DoWork += LoadHistoryList;
                _worker.RunWorkerAsync();
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
            if (NewAccountStatus == null) return;
            NewAccountStatus.AccountMainId = CurrentAccount.Id;
            _accountStatusService.AddNewStatus(NewAccountStatus);
            CurrentAccountStatus = NewAccountStatus;
            IsChangeStatusOpen = false;
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

        private void NewAccountPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveNewStatusCommand.RaiseCanExecuteChanged();
        }
        #endregion statuses

        #region stores
        private bool CanEdit()
        {
            return !IsAddStoreBusy;
        }
        private void EditAccountStoresList()
        {
            if (!IsEditAccountStoresOpen)
            {
                IsEditAccountStoresOpen = true;
                StoresForLoad = string.Empty;
                StoresList = new ObservableCollection<StoresSet>(_storesService.GetStores());
                Task.Run(() => AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(CurrentAccount.Id)));
            }
            else
            {
                IsEditAccountStoresOpen = false;
            }
        }
        private void AddStoresToAccount(List<int> storesNumbers)
        {
            if (storesNumbers.Count <= 0) return;
            var storesForAddList = new ObservableCollection<AccountsStoreDetailsSet>();
            foreach (var storeNumber in storesNumbers.Where(storeNumber => AccountStoresList.All(s => s.StoreNumber != storeNumber) && storesForAddList.All(s => s.AccountStore != storeNumber)))
            {
                storesForAddList.Add(new AccountsStoreDetailsSet { AccountsMainId = CurrentAccount.Id, AccountStore = storeNumber });
            }
            if (storesForAddList.Count == 0) return;
            _accountStoresService.AddStoresToAccount(storesForAddList);
            AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(CurrentAccount.Id));
            Application.Current.Dispatcher.BeginInvoke(new Action(() => StoresForLoad = string.Empty));
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
                return string.IsNullOrWhiteSpace(StoresError)&&!IsAddStoreBusy;
            }
            StoresError = string.Empty;
            return false;
        }
        private void DeleteAccountStore()
        {
            if (CurrentAccountStore == null) return;
            _accountStoresService.DeleteStoreFromAccount(CurrentAccountStore.StoreNumber, CurrentAccount.Id);
            AccountStoresList = new ObservableCollection<StoresSet>(_accountStoresService.GetAccountStoresById(CurrentAccount.Id));
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
        #endregion stores

        #region capexes
        private void LoadCapexInfo()
        {
            AccountCapexList = new ObservableCollection<AccountsCapexInfoSet>(_accountCapexService.GetCapexesById(CurrentAccount.Id));
            AvailableSum = CurrentAccount.AccountAmount - AccountCapexList.Sum(c => c.AccountCapexAmount);
        }
        private void OpenAddCapexToAccount()
        {
            NewCapexForAccount = new AccountsCapexInfoSet();
            CapexAmount = 0M;
            if (!IsAddCapexOpen)
            {
                CapexesList = new ObservableCollection<CapexSet>(_capexService.GetCapexesForYearList(CurrentAccount.AccountYear));
                NewCapexForAccount.AccountsMainId = CurrentAccount.Id;
            }
            IsAddCapexOpen = !IsAddCapexOpen;
        }
        private void CopyAvailableSum()
        {
            CapexAmount = AvailableSum;
        }
        private void AddCapexToAccount()
        {
            if (NewCapexForAccount != null)
            {
                NewCapexForAccount.AccountCapexAmount = CapexAmount;
                NewCapexForAccount.CapexId = _capexService.GetCapexIdByName(NewCapexForAccount.AccountCapexName, CurrentAccount.AccountYear);
                _accountCapexService.AddCapexToAccount(NewCapexForAccount);
                LoadCapexInfo();
                IsAddCapexOpen = false;
            }
        }
        private void DeleteCapexAccount()
        {
            if (CurrentCapex != null)
            {
                _accountCapexService.DeleteCapexFromAccount(CurrentCapex);
                LoadCapexInfo();
            }
        }
        #endregion capexes

        #endregion Methods
    }
}
