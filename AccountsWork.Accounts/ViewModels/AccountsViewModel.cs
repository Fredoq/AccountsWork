using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Prism.Regions;
using Syncfusion.UI.Xaml.Grid;


namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountsViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private IAccountsMainService _accountsMainService;
        private IRegionManager _regionManager;
        #endregion Private Fields

        #region Commands
        public DelegateCommand<string> NavigateCommand { get; set; } 
        #endregion Commands

        #region Methods

        void Navigate(string navigationProperty)
        {
            _regionManager.RequestNavigate(RegionNames.AccountsTabRegion, navigationProperty);
        }
        #endregion Methods        

        #region Constructor
        [ImportingConstructor]
        public AccountsViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager)
        //public AccountsViewModel()
        {
            _accountsMainService = accountsMainService;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }
        #endregion Constructor
    }
}