using AccountsVork.Infrastructure;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.ExcelReports;
using AccountsWork.Infrastructure;
using AccountsWork.Reports.Controllers;
using AccountsWork.Reports.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Prism.Regions;
using System.Linq;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class LoadServiceInvoViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private List<string> _monthes;
        private List<string> _companyList;
        private string _selectedMonth;
        private string _selectedCompany;
        private List<int> _yearList;
        private int _selectedYear;
        private string _specFileName;
        private IEventAggregator _eventAggregator;
        private ReportsController _reportsController;
        private IExcelSpecificationLoader _excelSpecificationLoader;
        private ObservableCollection<ServiceZipDetailsSet> _serviceZipList;
        private ObservableCollection<ZipSet> _emptyZipsList;
        private ZipSet _selectedEmptyZip;
        private IZipService _zipService;
        private IServiceZipsService _serviceZipService;
        private BackgroundWorker _worker;
        private bool _isServiceBusy;
        private IList<ZipSet> _zipList;
        private IList<ZipSet> _newZipList;
        private IList<string> _mainZipList;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure

        public string ReportsTabItemHeader
        {
            get { return _reportsTabItemHeader; }
            set { SetProperty(ref _reportsTabItemHeader, value); }
        }
        public List<string> MonthesList
        {
            get { return _monthes; }
            set { SetProperty(ref _monthes, value); }
        }
        public List<string> CompanyList
        {
            get { return _companyList; }
            set { SetProperty(ref _companyList, value); }
        }
        public List<int> YearList
        {
            get { return _yearList; }
            set { SetProperty(ref _yearList, value); }
        }
        public int SelectedYear
        {
            get { return _selectedYear; }
            set { SetProperty(ref _selectedYear, value); }
        }
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set { SetProperty(ref _selectedMonth, value); }
        }
        public string SelectedCompany
        {
            get { return _selectedCompany; }
            set { SetProperty(ref _selectedCompany, value); }
        }

        #endregion infrastructure

        #region zips
        public bool IsServiceBusy
        {
            get { return _isServiceBusy; }
            set { SetProperty(ref _isServiceBusy, value); }
        }
        public ObservableCollection<ServiceZipDetailsSet> ServiceZipList
        {
            get { return _serviceZipList; }
            set { SetProperty(ref _serviceZipList, value); }
        }
        public ObservableCollection<ZipSet> EmptyZipsList
        {
            get { return _emptyZipsList; }
            set { SetProperty(ref _emptyZipsList, value); }
        }
        public IList<ZipSet> ZipList
        {
            get { return _zipList; }
            set { SetProperty(ref _zipList, value); }

        }
        public IList<ZipSet> NewZipList
        {
            get { return _newZipList; }
            set { SetProperty(ref _newZipList, value); }
        }
        public IList<string> MainZipList
        {
            get { return _mainZipList; }
            set { SetProperty(ref _mainZipList, value); }
        }
        public ZipSet SelectedEmptyZip
        {
            get { return _selectedEmptyZip; }
            set { SetProperty(ref _selectedEmptyZip, value); }
        }
        #endregion zips

        #endregion Public Properties

        #region Commands

        #region infrastrcture
        public DelegateCommand LoadSpecificationCommand { get; set; }
        #endregion infrastructure

        #region zips
        public DelegateCommand RefreshEmptyListCommand { get; set; }
        public DelegateCommand AddServiceZipsCommand { get; set; }
        public DelegateCommand SaveEmptyZipsCommand { get; set; }
        #endregion zips

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public LoadServiceInvoViewModel(IEventAggregator eventAggregator, ReportsController reportsController, IExcelSpecificationLoader excelSpecificationLoader, IZipService zipService, IServiceZipsService serviceZipService)
        {
            #region infrastructure

            ReportsTabItemHeader = "Загрузка инфо для сервиса";
            MonthesList = Monthes.GetMonthesList();
            CompanyList = ServiceCompanies.GetServiceCompaniesList();
            YearList = new List<int>();
            for (var i = 2015; i <= DateTime.Now.Year; i++)
                YearList.Add(i);
            LoadSpecificationCommand = new DelegateCommand(LoadSpecification);
            _reportsController = reportsController;
            #endregion infrastructure

            #region events
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<OpenFileEvent>().Subscribe(GetFilename);
            #endregion events

            #region zips
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>();
            EmptyZipsList = new ObservableCollection<ZipSet>();
            ZipList = new List<ZipSet>();
            NewZipList = new List<ZipSet>();
            MainZipList = new List<string>();
            IsServiceBusy = false;
            RefreshEmptyListCommand = new DelegateCommand(RefreshEmptyList);
            AddServiceZipsCommand = new DelegateCommand(AddServiceZips);
            SaveEmptyZipsCommand = new DelegateCommand(SaveEmptyZips);
            #endregion zips

            #region services
            _excelSpecificationLoader = excelSpecificationLoader;
            _zipService = zipService;
            _serviceZipService = serviceZipService;
            #endregion services

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += AddServiceZipsWork;
            _worker.RunWorkerCompleted += AddServiceZipsWork_Completed;
            #endregion workers
        }        
        #endregion Constructor

        #region Methods

        #region infrastructure
        private void LoadSpecification()
        {
            ServiceZipList.Clear();
            EmptyZipsList.Clear();
            NewZipList.Clear();
            _reportsController.ShowDialogWindow();
            foreach(var serviceZip in _excelSpecificationLoader.GetServiceZips(SelectedCompany, _specFileName, SelectedMonth, SelectedYear).Result)
            {
                ServiceZipList.Add(serviceZip);
            }
            foreach(var serviceZip in ServiceZipList)
            {
                if (!ZipList.Any(z => z.ZipName.ToLower() == serviceZip.ZipName.ToLower()))
                    if (!NewZipList.Any(e => e.ZipName.ToLower() == serviceZip.ZipName.ToLower()))
                        NewZipList.Add(new ZipSet { ZipName = serviceZip.ZipName });
            }
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            EmptyZipsList.Clear();
            ZipList = _zipService.GetZips();
            MainZipList = _zipService.GetMainZips();

        }
        private void GetFilename(string obj)
        {
            _specFileName = obj;
        }
        #endregion infrastructure

        #region zips
        private void RefreshEmptyList()
        {
            EmptyZipsList.Clear();
            EmptyZipsList = new ObservableCollection<ZipSet>(_zipService.GetEmptyZips());
        }
        private void AddServiceZips()
        {
            if (ServiceZipList.Count > 0)
            {
                IsServiceBusy = true;
                _worker.RunWorkerAsync();                            
            }
        }
        private void AddServiceZipsWork(object sender, DoWorkEventArgs e)
        {
            _zipService.AddNewZips(NewZipList);
            _serviceZipService.AddServiceZips(ServiceZipList);
        }
        private void AddServiceZipsWork_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ZipList = _zipService.GetZips();
            RefreshEmptyList();
            ServiceZipList.Clear();
            IsServiceBusy = false;
        }
        private void SaveEmptyZips()
        {
            _zipService.UpdateMainZip(EmptyZipsList.Where(z => !string.IsNullOrWhiteSpace(z.MainZipName)));
            RefreshEmptyList();
        }
        #endregion zips

        #endregion Methods
    }
}
