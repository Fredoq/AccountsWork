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

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class AddFullAccountViewModel : ValidatableBindableBase
    {
        private readonly ICompaniesService _companiesService;
        private readonly ITypesService _typesService;
        private readonly IAccountsMainService _accountsService;

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
        private decimal _capexAmount;
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
        public AddFullAccountViewModel(ICompaniesService companiesService, ITypesService typesService, IAccountsMainService accountsService)
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
            #endregion capexes

            #region services
            _companiesService = companiesService;
            _typesService = typesService;
            _accountsService = accountsService;

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
            if (Companies != null && Types != null) return;
            Companies = _companiesService.GetCompanies();
            Types = _typesService.GetTypes();
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
        }
        private void CloseAddCapexToAccount()
        {
            IsAddCapexOpen = false;
        }
        #endregion capexes

        #endregion Methods




    }
}