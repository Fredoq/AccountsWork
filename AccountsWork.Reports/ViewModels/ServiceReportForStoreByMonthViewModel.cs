using AccountsWork.Infrastructure;
using System.ComponentModel.Composition;
using Prism.Regions;
using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using AccountsWork.DomainModel;
using AccountsWork.BusinessLayer;
using System.Collections.Generic;
using System.Linq;
using AccountsWork.Reports.Model;
using AccountsVork.Infrastructure;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class ServiceReportForStoreByMonthViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private BackgroundWorker _worker;
        private bool _isServiceBusy;
        private ObservableCollection<ServiceZipDetailsSet> _serviceZipList;
        private IServiceZipsService _serviceZipService;
        private IStoresService _storeService;
        private IList<StoresSet> _storesList;
        private ObservableCollection<MonthExp> _monthExpList;
        private IList<string> _monthes;
        private IList<AccountsMainSet> _serviceAccounts;
        private IAccountsMainService _accountsMainService;
        private ObservableCollection<MonthExp> _monthServExpList;
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
        public IList<StoresSet> StoresList
        {
            get { return _storesList; }
            set { SetProperty(ref _storesList, value); }
        }
        public IList<AccountsMainSet> ServiceAccountsList
        {
            get { return _serviceAccounts; }
            set { SetProperty(ref _serviceAccounts, value); }
        }
        public ObservableCollection<MonthExp> MonthExpList
        {
            get { return _monthExpList; }
            set { SetProperty(ref _monthExpList, value); }
        }
        public ObservableCollection<MonthExp> MonthServExpList
        {
            get { return _monthServExpList; }
            set { SetProperty(ref _monthServExpList, value); }
        }
        #endregion report

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public ServiceReportForStoreByMonthViewModel(IServiceZipsService serviceZipService, IStoresService storeService, IAccountsMainService accountsMainService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Отчет по затратам на ресторан";
            MonthesList = Monthes.GetMonthesList();
            #endregion infrastructure

            #region report
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>();
            MonthExpList = new ObservableCollection<MonthExp>();
            StoresList = new List<StoresSet>();
            ServiceAccountsList = new List<AccountsMainSet>();
            MonthServExpList = new ObservableCollection<MonthExp>();
            #endregion report

            #region services
            _serviceZipService = serviceZipService;
            _storeService = storeService;
            _accountsMainService = accountsMainService;
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
            StoresList.Clear();
            MonthExpList.Clear();
            ServiceAccountsList.Clear();
            IsServiceBusy = true;
            _worker.RunWorkerAsync();       
        }
        private void LoadServiceZip_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsServiceBusy = false;
            foreach (var exp in GetMonthExpList())
                MonthExpList.Add(exp);
            var list = GetServiceExp();
            foreach (var ser in list)
            {
                if (ser.MonthYear >= new DateTime(2015,1,1))
                    MonthServExpList.Add(ser);
            }
        }

        private void LoadServiceZip(object sender, DoWorkEventArgs e)
        {
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>(_serviceZipService.GetAll());
            StoresList = _storeService.GetStores().Where(s => s.StoreNumber > 10000).ToList();
            ServiceAccountsList = _accountsMainService.GetServiceAccounts();  
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
        private int StoreCount(DateTime date)
        {
            return StoresList.Where(s => s.StoreOpenDate.HasValue).Where(s => s.StoreOpenDate <= date && (!s.StoreCloseDate.HasValue || s.StoreCloseDate > date)).Count();
        }
        #endregion infrastructure

        #region report
        private IList<MonthExp> GetMonthExpList()
        {
            return (from s in ServiceZipList
                    group s by new { s.ServiceMonth, s.ServiceYear }
                    into ym
                    select new MonthExp { MonthYear = new DateTime(ym.Key.ServiceYear.Value, ReturnNumberMonth(ym.Key.ServiceMonth), 1), Expense = (ym.Where(s => s.ZipQuantity.Value == 0).Sum(s => s.ZipPrice) + ym.Where(s => s.ZipQuantity.Value != 0).Sum(s => s.ZipPrice * s.ZipQuantity.Value)) / StoreCount(new DateTime(ym.Key.ServiceYear.Value, ReturnNumberMonth(ym.Key.ServiceMonth), 1)), StoreCount = StoreCount(new DateTime(ym.Key.ServiceYear.Value, ReturnNumberMonth(ym.Key.ServiceMonth), 1)) }).ToList();
        }
        private IList<MonthExp> GetServiceExp()
        {
            return (from a in ServiceAccountsList
                    where a.AccountCompany == "ККС Интер Фуд" || a.AccountCompany == "АйСиЭл"
                    group a by new { a.AccountDate.Month, a.AccountDate.Year }
                    into ym
                    select new MonthExp { MonthYear = new DateTime(ym.Key.Year, ym.Key.Month, 1), Expense = ym.Sum(ac => ac.AccountAmount) / StoreCount(new DateTime(ym.Key.Year, ym.Key.Month, 1)), StoreCount = StoreCount(new DateTime(ym.Key.Year, ym.Key.Month, 1)) }).ToList();

        }
        #endregion report

        #endregion Methods

    }
}
