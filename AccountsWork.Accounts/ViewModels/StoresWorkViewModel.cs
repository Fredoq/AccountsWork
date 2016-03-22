using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Prism.Regions;
using System;
using AccountsWork.BusinessLayer;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class StoresWorkViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private bool _isStoreWorkBusy;
        private ObservableCollection<StoreProvenWorkSet> _storesWorkList;
        private BackgroundWorker _worker;
        private IStoresWorkService _storesWorkService;
        private StoreProvenWorkSet _selectedWork;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
        #endregion infrastructure

        #region work
        public bool IsStoreWorkBusy
        {
            get { return _isStoreWorkBusy; }
            set { SetProperty(ref _isStoreWorkBusy, value); }
        }
        public ObservableCollection<StoreProvenWorkSet> StoresWorkList
        {
            get { return _storesWorkList; }
            set { SetProperty(ref _storesWorkList, value); }
        }
        public StoreProvenWorkSet SelectedWork
        {
            get { return _selectedWork; }
            set { SetProperty(ref _selectedWork, value); }
        }

        #endregion work

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public StoresWorkViewModel(IStoresWorkService storesWorkService)
        {
            #region infrastructure
            AccountsTabItemHeader = "Работы в ресторанах";
            #endregion infrastructure

            #region services
            _storesWorkService = storesWorkService;
            #endregion services

            #region work
            StoresWorkList = new ObservableCollection<StoreProvenWorkSet>();
            #endregion work

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadStoresWork;
            _worker.RunWorkerCompleted += LoadStoresWork_Comleted;
            #endregion workers
        }        

        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _worker.RunWorkerAsync();
        }

        #endregion infrastructure

        #region work

        private void LoadStoresWork(object sender, DoWorkEventArgs e)
        {
            IsStoreWorkBusy = true;
            StoresWorkList = new ObservableCollection<StoreProvenWorkSet>(_storesWorkService.GetWorksList());
        }
        private void LoadStoresWork_Comleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsStoreWorkBusy = false;
        }

        #endregion work

        #endregion Methods


    }
}
