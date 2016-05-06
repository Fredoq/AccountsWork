using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Commands;
using Prism.Regions;

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
        #endregion capexes

        #endregion Public Properties

        #region Commands
        public DelegateCommand SaveAccountCommand { get; set; }
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

        #endregion Methods




    }
}