using System.ComponentModel.Composition;
using AccountsWork.Infrastructure;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class InfoViewModel : ValidatableBindableBase
    {
        private string _accountsTabItemHeader;

        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }

        public InfoViewModel()
        {
            AccountsTabItemHeader = "Общая информация";
        }
    }
}