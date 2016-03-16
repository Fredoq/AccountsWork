using System.ComponentModel.Composition;
using AccountsWork.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using AccountsWork.DomainModel;
using AccountsWork.BusinessLayer;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Prism.Commands;
using System.Linq;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class AddAccountViewModel : ValidatableBindableBase, IConfirmNavigationRequest
    {
        #region Private Fields
        private string _accountsTabItemHeader;   
        private IList<AccountsCompaniesSet> _companies;
        private readonly ICompaniesService _companiesService;
        private readonly ITypesService _typesService;
        private IList<TypeSet> _types;
        private readonly BackgroundWorker _worker;
        private AccountsMainSet _account;
        private readonly IAccountsMainService _accountsService;
        private IRegionManager _regionManager;
        private const string AdditionalInfoViewKey = "AdditionalInfoView";
        private const string AccountKey = "Account";

        #endregion Private Fields

        #region Public Properties
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
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
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }
        public InteractionRequest<IConfirmation> AdditionalInfoConfirmationRequest { get; set; }
        #endregion Public Properties

        #region Commands
        public DelegateCommand LoadAllCommand { get; set; }
        public DelegateCommand SaveAccountCommand { get; set; }
        #endregion Commands

        #region Constructors
        [ImportingConstructor]
        public AddAccountViewModel(ICompaniesService companiesService, ITypesService typesService, IAccountsMainService accountsService, IRegionManager regionManager)
        {           
            _regionManager = regionManager;
            _companiesService = companiesService;
            _typesService = typesService;
            _accountsService = accountsService;

            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            AdditionalInfoConfirmationRequest = new InteractionRequest<IConfirmation>();
            _worker = new BackgroundWorker();
            _worker.DoWork += DoWork;
            LoadAllCommand = new DelegateCommand(() =>
            {
                if (!_worker.IsBusy)
                    _worker.RunWorkerAsync();
            });
            SaveAccountCommand = new DelegateCommand(SaveCommand, CanSave).ObservesProperty(() => Account);

            //Account = new AccountsMainSet();
            //Account.AccountYear = DateTime.Now.Year;
            //Account.AccountDate = DateTime.Now;

        }
        #endregion Constructors
             
        #region Methods

        void LoadCompaniesAndTypes()
        {
            if (Companies == null || Types == null)
            {
                Companies = _companiesService.GetCompanies();
                Types = _typesService.GetTypes();
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            LoadCompaniesAndTypes();
        }

        void SaveCommand()
        {
            var id = _accountsService.SaveAccount(Account);            
            AdditionalInfoConfirmationRequest.Raise(new Confirmation { Content = "Счет сохранен. Перейти к редактированию доп. информации?", Title = "Редактирование счета" },
                                             c =>
                                             {
                                                 if (c.Confirmed)
                                                 {
                                                     var singleView = _regionManager.Regions[RegionNames.AccountsTabRegion].ActiveViews.FirstOrDefault();
                                                     _regionManager.Regions[RegionNames.AccountsTabRegion].Remove(singleView);
                                                     var parameters = new NavigationParameters();
                                                     parameters.Add(AccountKey, Account);
                                                     _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, new Uri(AdditionalInfoViewKey, UriKind.Relative), parameters);
                                                 }
                                             });
            Account = new AccountsMainSet
            {
                AccountYear = DateTime.Now.Year,
                AccountDate = DateTime.Now
            };
        }
        bool CanSave()
        {
            if (Account != null)
            {
                Account.ValidateProperties();
                return !Account.HasErrors;
            }
            return false;
        }
        private void AccountPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveAccountCommand.RaiseCanExecuteChanged();
        }
        private AccountsMainSet GetAccount(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters[AccountKey];
            var account = (AccountsMainSet)parameter;
            

            return (AccountsMainSet)account;
        }
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (navigationContext.Uri == null)
            {
                ConfirmationRequest.Raise(
                    new Confirmation { Content = "Закрыть без сохранения?", Title = "Закрытие вкладки" },
                    c => {
                        Account = new AccountsMainSet
                        {
                            AccountYear = DateTime.Now.Year,
                            AccountDate = DateTime.Now
                        };
                        continuationCallback(c.Confirmed);
                    });
            }
            else
            {
                continuationCallback(true);
            }
        }
        
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Account = GetAccount(navigationContext);
            if(Account != null)
            {
                AccountsTabItemHeader = "Ред. информации по сч. " + Account.AccountNumber;     
            }
            else
            {
                AccountsTabItemHeader = "Новый счет";
                Account = new AccountsMainSet();
                Account.AccountYear = DateTime.Now.Year;
                Account.AccountDate = DateTime.Now;
            }
        }
        #endregion Methods 
    }
}