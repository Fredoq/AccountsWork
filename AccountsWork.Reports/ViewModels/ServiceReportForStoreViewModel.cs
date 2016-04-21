using AccountsWork.Infrastructure;
using System.ComponentModel.Composition;
using Prism.Regions;
using System.Collections.ObjectModel;
using AccountsWork.DomainModel;
using AccountsWork.BusinessLayer;
using System.ComponentModel;
using System;
using AccountsWork.Reports.Model;
using Prism.Commands;
using System.Linq;
using System.Collections.Generic;
using AccountsVork.Infrastructure;

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
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _minimumDate;
        private DateTime _maximumDate;
        private ObservableCollection<StackedStoreInfo> _stackedStoreList;
        private IList<string> _monthes;
        private ObservableCollection<StoreZip> _storeZipList;
        private StackedStoreInfo _selectedStackedStore;
        private bool _isSelectAll;
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
        public IList<string> MonthesList
        {
            get { return _monthes; }
            set { SetProperty(ref _monthes, value); }
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
            set { SetProperty(ref _selectedStore, value);}
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { SetProperty(ref _minimumDate, value); }
        }
        public DateTime MaximumDate
        {
            get { return _maximumDate; }
            set { SetProperty(ref _maximumDate, value); }
        }
        public ObservableCollection<StackedStoreInfo> StackedStoreList
        {
            get { return _stackedStoreList; }
            set { SetProperty(ref _stackedStoreList, value); }
        }
        public ObservableCollection<StoreZip> StoreZipList
        {
            get { return _storeZipList; }
            set { SetProperty(ref _storeZipList, value); }
        }
        public StackedStoreInfo SelectedStackedStore
        {
            get { return _selectedStackedStore; }
            set { SetProperty(ref _selectedStackedStore, value); LoadSelectedStorezip(); }
        }
        public bool IsSelectAll
        {
            get { return _isSelectAll; }
            set { SetProperty(ref _isSelectAll, value); }
        }       
        #endregion report

        #endregion Public Properties

        #region Commands

        #region report
        public DelegateCommand LoadReportCommand { get; set; }
        public DelegateCommand SelectAllCommand { get; set; }
        #endregion report

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public ServiceReportForStoreViewModel(IServiceZipsService serviceZipService, IStoresService storeService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Детальный отчет по ресторанам";
            MonthesList = Monthes.GetMonthesList();
            #endregion infrastructure

            #region report
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>();
            StoresWithCheckList = new ObservableCollection<StoresWithCheck>();
            StartDate = new DateTime(2015, 1, 1);
            MinimumDate = new DateTime(2015, 1, 1);
            MaximumDate = new DateTime(DateTime.Now.Year, 12, 31);
            EndDate = DateTime.Now;
            LoadReportCommand = new DelegateCommand(LoadReport);
            StackedStoreList = new ObservableCollection<StackedStoreInfo>();
            StoreZipList = new ObservableCollection<StoreZip>();
            IsSelectAll = false;
            SelectAllCommand = new DelegateCommand(SelectAll);
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
        private int ReturnNumberMonth(string month)
        {
            var num = 0;
            for (var i = 0; i < 12; i++)
                if (MonthesList[i] == month)
                {
                    num = i + 1;
                    break;
                }
            return num;
        }
      
        #endregion infrastructure

        #region report
        private void LoadReport()
        {
            StackedStoreList.Clear();
            StoreZipList.Clear();
            foreach(var store in StoresWithCheckList.Where(s => s.Check))
            {
                var stackedStore = new StackedStoreInfo();
                stackedStore.Store = store.Store;
                stackedStore.EquipmentSum = ServiceZipList.Where(se => se.StoreNumber == store.Store.StoreNumber && (new DateTime(se.ServiceYear.Value, ReturnNumberMonth(se.ServiceMonth), 1) >= new DateTime(StartDate.Year, StartDate.Month, 1)) && (new DateTime(se.ServiceYear.Value, ReturnNumberMonth(se.ServiceMonth), 1) < new DateTime(EndDate.Year, EndDate.Month, 1)) && se.ZipName != "Ремонт").Sum(se => se.ZipQuantity==0 ? se.ZipPrice : se.ZipPrice * se.ZipQuantity.Value);
                stackedStore.RepairSum = ServiceZipList.Where(se => se.StoreNumber == store.Store.StoreNumber && (new DateTime(se.ServiceYear.Value, ReturnNumberMonth(se.ServiceMonth), 1) >= new DateTime(StartDate.Year, StartDate.Month, 1)) && (new DateTime(se.ServiceYear.Value, ReturnNumberMonth(se.ServiceMonth), 1) < new DateTime(EndDate.Year, EndDate.Month, 1)) && se.ZipName == "Ремонт").Sum(se => se.ZipPrice);
                stackedStore.ServiceZipList = ServiceZipList.Where(se => se.StoreNumber == store.Store.StoreNumber && (new DateTime(se.ServiceYear.Value, ReturnNumberMonth(se.ServiceMonth), 1) >= new DateTime(StartDate.Year, StartDate.Month, 1)) && (new DateTime(se.ServiceYear.Value, ReturnNumberMonth(se.ServiceMonth), 1) < new DateTime(EndDate.Year, EndDate.Month, 1)));
                StackedStoreList.Add(stackedStore);
            }
        }
        private void LoadSelectedStorezip()
        {
            StoreZipList.Clear();
            var query = from s in SelectedStackedStore.ServiceZipList
                        group s by s.ZipSet.MainZipName into zip
                        select new StoreZip { ZipName = zip.Key, Summ = zip.Sum(z => z.ZipQuantity == 0 ? z.ZipPrice : z.ZipPrice * z.ZipQuantity.Value), ZipList = new ObservableCollection<ZipPrice> (zip.Select(z => new ZipPrice { Price = z.ZipPrice, Quantity =z.ZipQuantity.Value, Zip = z.ZipName }).ToList()) };
            foreach(var item in query)
            {
                StoreZipList.Add(item);
            }
        }
        private void SelectAll()
        {
            if (IsSelectAll)
            {
                foreach (var store in StoresWithCheckList)
                    store.Check = true;
                LoadReport();
            }
            else
            {
                foreach (var store in StoresWithCheckList)
                    store.Check = false;
                LoadReport();
            }
        }
        #endregion report

        #endregion Methods
    }
}
