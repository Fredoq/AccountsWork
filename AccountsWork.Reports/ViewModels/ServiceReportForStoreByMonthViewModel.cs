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
        public ObservableCollection<MonthExp> MonthExpList
        {
            get { return _monthExpList; }
            set { SetProperty(ref _monthExpList, value); }
        }
        #endregion report

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public ServiceReportForStoreByMonthViewModel(IServiceZipsService serviceZipService, IStoresService storeService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Отчет по затратам на ресторан";
            MonthesList = Monthes.GetMonthesList();
            #endregion infrastructure

            #region report
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>();
            MonthExpList = new ObservableCollection<MonthExp>();
            StoresList = new List<StoresSet>();
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
            StoresList.Clear();
            MonthExpList.Clear();
            IsServiceBusy = true;
            _worker.RunWorkerAsync();       
        }
        private void LoadServiceZip_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsServiceBusy = false;
            foreach (var exp in GetMonthExpList())
                MonthExpList.Add(exp);
        }

        private void LoadServiceZip(object sender, DoWorkEventArgs e)
        {
            ServiceZipList = new ObservableCollection<ServiceZipDetailsSet>(_serviceZipService.GetAll());
            StoresList = _storeService.GetStores().Where(s => s.StoreNumber > 10000).ToList();           
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
        #endregion report

        #endregion Methods

    }
}
