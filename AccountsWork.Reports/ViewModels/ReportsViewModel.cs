using AccountsWork.Infrastructure;
using Prism.Commands;
using Prism.Regions;
using System.ComponentModel.Composition;
using System;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class ReportsViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private IRegionManager _regionManager;
        #endregion PrivateFields

        #region Public Properties
        #endregion Public Properties

        #region Commands

        #region infrastructure
        public DelegateCommand<string> NavigateCommand { get; set; }
        #endregion infrastructure

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public ReportsViewModel(IRegionManager regionManager)
        {
            #region infrastructure
            _regionManager = regionManager;           
            NavigateCommand = new DelegateCommand<string>(Navigate);
            #endregion infrastructure
        }

        #region Methods

        #region infrastructure
        private void Navigate(string navigationProperty)
        {
            _regionManager.RequestNavigate(RegionNames.ReportsTabRegion, navigationProperty);
        }
        #endregion infrastructure

        #endregion Methods

        #endregion Constructor
    }
}
