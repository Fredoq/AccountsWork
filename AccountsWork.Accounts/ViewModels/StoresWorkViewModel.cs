using AccountsWork.Infrastructure;
using System.ComponentModel.Composition;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class StoresWorkViewModel : ValidatableBindableBase
    {
        private string _accountsTabItemHeader;

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        #endregion infrastructure

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public StoresWorkViewModel()
        {
            #region infrastructure
            AccountsTabItemHeader = "Работы в ресторанах";
            #endregion infrastructure
        }
        #endregion Constructor


    }
}
