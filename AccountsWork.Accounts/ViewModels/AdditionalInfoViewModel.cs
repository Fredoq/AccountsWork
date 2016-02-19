using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
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
        #endregion Private Fields

        #region Public Properties
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
        public AccountsStatusDetailsSet AccountStatus
        {
            get { return _accountStatus; }
            set { SetProperty(ref _accountStatus, value); }
        }
        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public AdditionalInfoViewModel(IAccountStatusService accountStatusService)
        {
            ConfirmationRequest = new InteractionRequest<IConfirmation>();

            _accountStatusService = accountStatusService;

            _worker = new BackgroundWorker();
            _worker.DoWork += LoadAccountAdditionalInfo;
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
        }
        private void LoadAccountLastStatus()
        {
            AccountStatus = _accountStatusService.GetAccountStatusById(CurrentAccount.Id);
        }
        #endregion Methods
    }
}
