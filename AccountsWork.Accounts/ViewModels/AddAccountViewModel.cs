using System.ComponentModel.Composition;
using AccountsWork.Infrastructure;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class AddAccountViewModel : ValidatableBindableBase
    {
        private string _accountsTabItemHeader;

        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }

        public AddAccountViewModel()
        {
            AccountsTabItemHeader = "Добавить счет";
        }
    }
}