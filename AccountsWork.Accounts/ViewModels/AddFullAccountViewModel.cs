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

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class AddFullAccountViewModel : ValidatableBindableBase
    {
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
        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public AddFullAccountViewModel(ICompaniesService companiesService, ITypesService typesService, IAccountsMainService accountsService, IAccountStatusService accountStatusService, IAccountStoresService accountStoresService, IStoresService storesService, IAccountCapexesService accountCapexService, IExpensesService expenseService, ICapexesService capexService, IStoresWorkService storesWorkService)
        {


            #region infrastructure
            SaveAccountCommand = new DelegateCommand(SaveCommand, CanSave).ObservesProperty(() => Account);
            #endregion infrastructure  

            #region workers

            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccount;
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

        }
       
        #endregion Constructor

        #region Methods

        #region infrastructure

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Account = GetAccount(navigationContext);
            if (Account != null)
            {
                AccountsTabItemHeader = "Ред. информации по сч. " + Account.AccountNumber;
            }
            else
            {
                IsInEditMode = true;
                AccountsTabItemHeader = "Новый счет";
                Account = new AccountsMainSet
                {
                    AccountYear = DateTime.Now.Year,
                    AccountDate = DateTime.Now
                };
                IsAdditinalInfoEnabled = false;
                AvailableSum = 0M;               
            }
            _worker.RunWorkerAsync();
        }

        #endregion infrastructure

        #region account
        private void LoadAccount(object sender, DoWorkEventArgs e)
        {
            Companies = _companiesService.GetCompanies();
            Types = _typesService.GetTypes();
            ExpensesList = new ObservableCollection<AccountsExpenseSet>(_expenseService.GetExpensesList());
            if (Account.Id != 0)
            {
                LoadCapexInfo();
            }
            else
            {
                AccountCapexList = new ObservableCollection<AccountsCapexInfoSet>();
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
        private void SaveCommand()
        {
            Account.Id = _accountsService.SaveAccount(Account);
            IsAdditinalInfoEnabled = true;
            CapexesList = new ObservableCollection<CapexSet>(_capexService.GetCapexesForYearList(Account.AccountYear));
            LoadCapexInfo();
            IsInEditMode = false;
        }
        private bool CanSave()
        {
            if (Account == null) return false;
            Account.ValidateProperties();
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
            AvailableSum = Account.AccountAmount - AccountCapexList.Sum(c => c.AccountCapexAmount);
        }
        private void NewCapexPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AddCapexToAccountCommand.RaiseCanExecuteChanged();
        }
        #endregion capexes

        #endregion Methods




    }
}