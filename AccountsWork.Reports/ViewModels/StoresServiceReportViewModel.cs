using AccountsWork.Infrastructure;
using System.ComponentModel.Composition;
using Prism.Regions;
using System.Collections.ObjectModel;
using AccountsWork.Reports.Model;
using System.ComponentModel;
using System;
using AccountsWork.BusinessLayer;
using System.Collections.Generic;
using AccountsVork.Infrastructure;
using AccountsWork.DomainModel;
using Prism.Commands;
using System.Linq;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class StoresServiceReportViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private bool _isLoading;
        private ObservableCollection<StoresWithCheck> _storesWithCheckList;
        private StoresWithCheck _selectedStore;
        private BackgroundWorker _worker;
        private IStoresService _storeService;
        private List<string> _monthes;
        private List<int> _yearList;
        private string _startMonth;
        private int _startYear;
        private string _endMonth;
        private int _endYear;
        private ObservableCollection<ServiceZipDetailsSet> _serviceZipList;
        private IServiceZipsService _serviceZipService;
        private decimal _totalICL;
        private decimal _workICL;
        private decimal _equipmentICL;
        private decimal _trICL;
        private decimal _avICL;
        private decimal _totalKKS;
        private decimal _workKKS;
        private decimal _equipmentKKS;
        private decimal _trKKS;
        private decimal _avKKS;
        #endregion Private Fields


        #region Public Properties

        #region infrastructure

        public string ReportsTabItemHeader
        {
            get { return _reportsTabItemHeader; }
            set { SetProperty(ref _reportsTabItemHeader, value); }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        public List<string> MonthesList
        {
            get { return _monthes; }
            set { SetProperty(ref _monthes, value); }
        }
        public List<int> YearList
        {
            get { return _yearList; }
            set { SetProperty(ref _yearList, value); }
        }
        public string StartMonth
        {
            get { return _startMonth; }
            set { SetProperty(ref _startMonth, value); }
        }
        public int StartYear
        {
            get { return _startYear; }
            set { SetProperty(ref _startYear, value); }
        }
        public string EndMonth
        {
            get { return _endMonth; }
            set { SetProperty(ref _endMonth, value); }
        }
        public int EndYear
        {
            get { return _endYear; }
            set { SetProperty(ref _endYear, value); }
        }
        #region report
        public ObservableCollection<ServiceZipDetailsSet> ServiceZipList
        {
            get { return _serviceZipList; }
            set { SetProperty(ref _serviceZipList, value); }
        }
        #endregion report
        #endregion infrastructure

        #region stores
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
        #endregion stores

        #region report
        public decimal TotalICL
        {
            get { return _totalICL; }
            set { SetProperty(ref _totalICL, value); }
        }
        public decimal WorkICL
        {
            get { return _workICL; }
            set { SetProperty(ref _workICL, value); }
        }
        public decimal EquipmentICL
        {
            get { return _equipmentICL; }
            set { SetProperty(ref _equipmentICL, value); }
        }
        public decimal TrICL
        {
            get { return _trICL; }
            set { SetProperty(ref _trICL, value); }
        }
        public decimal AvICL
        {
            get { return _avICL; }
            set { SetProperty(ref _avICL, value); }
        }
        public decimal TotalKKS
        {
            get { return _totalKKS; }
            set { SetProperty(ref _totalKKS, value); }
        }
        public decimal WorkKKS
        {
            get { return _workKKS; }
            set { SetProperty(ref _workKKS, value); }
        }
        public decimal EquipmentKKS
        {
            get { return _equipmentKKS; }
            set { SetProperty(ref _equipmentKKS, value); }
        }
        public decimal TrKKS
        {
            get { return _trKKS; }
            set { SetProperty(ref _trKKS, value); }
        }
        public decimal AvKKS
        {
            get { return _avKKS; }
            set { SetProperty(ref _avKKS, value); }
        }
        #endregion report
        #endregion Public Properties

        #region Commands
        #region report
        public DelegateCommand LoadReportCommand { get; set; }
        #endregion report
        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public StoresServiceReportViewModel(IStoresService storeService,
                                            IServiceZipsService serviceZipService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Отчет по ресторанскому сервису";
            MonthesList = Monthes.GetMonthesList();
            YearList = new List<int>();
            for (var i = 2015; i <= DateTime.Now.Year; i++)
                YearList.Add(i);
            #endregion infrastructure

            #region stores
            StoresWithCheckList = new ObservableCollection<StoresWithCheck>();
            #endregion stores

            #region report
            LoadReportCommand = new DelegateCommand(LoadReport);
            #endregion report

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadReportData;
            _worker.RunWorkerCompleted += LoadReportData_Completed;
            #endregion workers

            #region services
            _storeService = storeService;
            _serviceZipService = serviceZipService;
            #endregion services
        }       
        #endregion Constructor

        #region Methods
        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            StoresWithCheckList.Clear();
            IsLoading = true;
            _worker.RunWorkerAsync();
        }
        private void LoadReportData(object sender, DoWorkEventArgs e)
        {
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>(_serviceZipService.GetAllWithZips());
            foreach (var store in _storeService.GetStores())
            {
                if (store.StoreNumber > 10000)
                    StoresWithCheckList.Add(new StoresWithCheck { Store = store });
            }
        }
        private void LoadReportData_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsLoading = false;
        }
        #endregion infrastructure
        #region report
        private void LoadReport()
        {
            var query = from s in ServiceZipList
                        where StoresWithCheckList.Where(st => st.Check).Any(st => st.Store.StoreNumber == s.StoreNumber) &&
                              ReturnNumberMonth(s.ServiceMonth) >= ReturnNumberMonth(StartMonth) &&
                              ReturnNumberMonth(s.ServiceMonth) <= ReturnNumberMonth(EndMonth) &&
                              s.ServiceYear >= StartYear &&
                              s.ServiceYear <= EndYear
                        select s;
            WorkICL = query.Where(q => q.Company == "АйСиЭл").Where(q => q.ZipName == "Ремонт").Sum(q => q.ZipQuantity.Value == 0 ? q.ZipPrice : q.ZipQuantity.Value * q.ZipPrice);
            EquipmentICL = query.Where(q => q.Company == "АйСиЭл").Where(q => q.ZipName != "Транспортные услуги" && q.ZipName != "Ремонт").Sum(q => q.ZipQuantity.Value == 0 ? q.ZipPrice : q.ZipQuantity.Value * q.ZipPrice);
            TrICL = query.Where(q => q.Company == "АйСиЭл").Where(q => q.ZipName == "Транспортные услуги").Sum(q => q.ZipQuantity.Value == 0 ? q.ZipPrice : q.ZipQuantity.Value * q.ZipPrice);
            TotalICL = WorkICL + EquipmentICL + TrICL;
            AvICL = TotalICL / StoresWithCheckList.Where(st => st.Check).Count();
            WorkKKS = query.Where(q => q.Company == "ККС").Where(q => q.ZipName == "Ремонт").Sum(q => q.ZipQuantity.Value == 0 ? q.ZipPrice : q.ZipQuantity.Value * q.ZipPrice);
            EquipmentKKS = query.Where(q => q.Company == "ККС").Where(q => q.ZipName != "Транспортные услуги" && q.ZipName != "Ремонт").Sum(q => q.ZipQuantity.Value == 0 ? q.ZipPrice : q.ZipQuantity.Value * q.ZipPrice);
            TrKKS = query.Where(q => q.Company == "ККС").Where(q => q.ZipName == "Транспортные услуги").Sum(q => q.ZipQuantity.Value == 0 ? q.ZipPrice : q.ZipQuantity.Value * q.ZipPrice);
            TotalKKS = WorkKKS + EquipmentKKS + TrKKS;
            AvKKS = TotalKKS / StoresWithCheckList.Where(st => st.Check).Count();
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
        #endregion report
        #endregion Methods
    }
}
