using AccountsWork.Infrastructure;
using System.ComponentModel.Composition;
using Prism.Regions;
using System.Collections.ObjectModel;
using AccountsWork.DomainModel;
using AccountsWork.BusinessLayer;
using System.ComponentModel;
using System;
using AccountsWork.Reports.Model;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class ServiceReportForStoreViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private ObservableCollection<ServiceZipDetailsSet> _serviceZipList;
        private IServiceZipsService _serviceZipService;
        private BackgroundWorker _worker;
        private bool _isServiceBusy;
        private ObservableCollection<StoresWithCheck> _storesWithCheckList;
        private StoresWithCheck _selectedStore;
        private IStoresService _storeService;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure

        public string ReportsTabItemHeader
        {
            get { return _reportsTabItemHeader; }
            set { SetProperty(ref _reportsTabItemHeader, value); }
        }
        public bool IsServiceBusy
        {
            get { return _isServiceBusy; }
            set { SetProperty(ref _isServiceBusy, value); }
        }
        #endregion infrastructure

        #region report
        public ObservableCollection<ServiceZipDetailsSet> ServiceZipList
        {
            get { return _serviceZipList; }
            set { SetProperty(ref _serviceZipList, value); }
        }
        public ObservableCollection<StoresWithCheck> StoresWithCheckList
        {
            get { return _storesWithCheckList; }
            set { SetProperty(ref _storesWithCheckList, value); }
        }
        public StoresWithCheck SelectedStore
        {
            get { return _selectedStore; }
            set { SetProperty(ref _selectedStore, value); }
        }
        #endregion report

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public ServiceReportForStoreViewModel(IServiceZipsService serviceZipService, IStoresService storeService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Детальный отчет по ресторанам";
            #endregion infrastructure

            #region report
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>();
            StoresWithCheckList = new ObservableCollection<StoresWithCheck>();
            #endregion report

            #region services
            _serviceZipService = serviceZipService;
            _storeService = storeService;
            #endregion services

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadServiceZip;
            _worker.RunWorkerCompleted += LoadServiceZip_Completed;
            #endregion workers
        }      
        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ServiceZipList.Clear();
            StoresWithCheckList.Clear();
            IsServiceBusy = true;
            _worker.RunWorkerAsync();
        }
        private void LoadServiceZip(object sender, DoWorkEventArgs e)
        {
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>(_serviceZipService.GetAllWithZips());
            foreach(var store in _storeService.GetStores())
            {
                if (store.StoreNumber > 10000)
                    StoresWithCheckList.Add(new StoresWithCheck { Store = store });
            }
        }
        private void LoadServiceZip_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsServiceBusy = false;
        }
        #endregion infrastructure

        #endregion Methods
    }
}
