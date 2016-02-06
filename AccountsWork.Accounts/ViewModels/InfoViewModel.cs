using System;
using System.ComponentModel.Composition;
using AccountsWork.Infrastructure;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

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
        //public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }

        public InfoViewModel()
        {
            AccountsTabItemHeader = "Общая информация";
            //ConfirmationRequest = new InteractionRequest<IConfirmation>();
        }

        //public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        //{
        //    ConfirmationRequest.Raise(
        //        new Confirmation { Content = "Закрыть без сохранения?", Title = "Закрытие вкладки" },
        //          c => { continuationCallback(c.Confirmed); });
        //    //continuationCallback(true);
        //}

    }
}