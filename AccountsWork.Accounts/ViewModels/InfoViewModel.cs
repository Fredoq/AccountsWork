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
        #region Private Fields
        private string _accountsTabItemHeader;
        #endregion Private Fields 

        #region Public Properties
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        #endregion Public Properties

        #region Constructor
        public InfoViewModel()
        {
            AccountsTabItemHeader = "Общая информация";
        }
        #endregion Constructor
    }
}