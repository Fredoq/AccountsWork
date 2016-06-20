using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Prism.Regions;
using AccountsWork.BusinessLayer;
using System.ComponentModel;
using System.Linq;
using Prism.Commands;
using AccountsWork.Reports.Model;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class FAReportViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private ObservableCollection<FASet> _faList;
        private IFAService _faService;
        private FASet _selectedFA;
        private BackgroundWorker _worker;
        private bool _isLoading;
        private ObservableCollection<AccountFA> _accountFAList;
        private IList<AccountsBudgetDetailsSet> _fullFAList;
        private IAccountFAService _accountFAService;
        private IList<YearCheck> _yearList;
        private int _sumFAQuantity;
        private AccountFA _selectedAccountFA;
        private ObservableCollection<AccountsMainSet> _selectedAccountFAList;
        private ObservableCollection<FAInfo> _selectedFAInfo;
        private IStoresService _storeService;
        private IList<StoresSet> _storeList;
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
        #endregion infrastructure

        #region report
        public ObservableCollection<FASet> FAList
        {
            get { return _faList; }
            set { SetProperty(ref _faList, value); }
        }
        public FASet SelectedFA
        {
            get { return _selectedFA; }
            set { SetProperty(ref _selectedFA, value); }
        }
        public ObservableCollection<AccountFA> AccountFAList
        {
            get { return _accountFAList; }
            set { SetProperty(ref _accountFAList, value); }
        }
        public IList<AccountsBudgetDetailsSet> FullFAList
        {
            get { return _fullFAList; }
            set { SetProperty(ref _fullFAList, value); }
        }
        public IList<YearCheck> YearList
        {
            get { return _yearList; }
            set { SetProperty(ref _yearList, value); }
        }
        public int SumFAQuantity
        {
            get { return _sumFAQuantity; }
            set { SetProperty(ref _sumFAQuantity, value); }
        }
        public AccountFA SelectedAccountFA
        {
            get { return _selectedAccountFA; }
            set { SetProperty(ref _selectedAccountFA, value); LoadSelectedAccountFA(); }
        } 
        public ObservableCollection<AccountsMainSet> SelectedAccountFAList
        {
            get { return _selectedAccountFAList; }
            set { SetProperty(ref _selectedAccountFAList, value); }
        }   
        public ObservableCollection<FAInfo> SelectedFAInfoList
        {
            get { return _selectedFAInfo; }
            set { SetProperty(ref _selectedFAInfo, value); }
        }
        public IList<StoresSet> StoreList
        {
            get { return _storeList; }
            set { SetProperty(ref _storeList, value); }
        }
        #endregion report

        #endregion Public Properties

        #region Commands

        #region report
        public DelegateCommand LoadSelectedFACommand { get; set; }
        #endregion report

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public FAReportViewModel(IFAService faService, IAccountFAService accountFAService, IStoresService storeService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Отчет по основным средствам";            
            #endregion infrastructure

            #region report
            FAList = new ObservableCollection<FASet>();
            AccountFAList = new ObservableCollection<AccountFA>();
            FullFAList = new List<AccountsBudgetDetailsSet>();
            LoadSelectedFACommand = new DelegateCommand(LoadSelectedFA);
            SelectedAccountFAList = new ObservableCollection<AccountsMainSet>();
            int i = 2016;
            YearList = new List<YearCheck>();
            while(i < DateTime.Now.Year + 5)
            {
                YearList.Add(new YearCheck { IsSelected = i == DateTime.Now.Year ? true : false, Year = i });
                i++;
            }
            #endregion report

            #region services
            _faService = faService;
            _accountFAService = accountFAService;
            _storeService = storeService;
            #endregion services

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadReportData;
            _worker.RunWorkerCompleted += LoadReportData_Completed;
            #endregion workers
        }        
        #endregion Constructor

        #region Methods

        #region infrastructure
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {            
            IsLoading = true;
            _worker.RunWorkerAsync();
        }
        private void LoadReportData(object sender, DoWorkEventArgs e)
        {
            FAList = new ObservableCollection<FASet>(_faService.GetFAListFull());
            FullFAList = _accountFAService.GetFAFullList();
            StoreList = _storeService.GetStores();

        }
        private void LoadReportData_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsLoading = false;
        }
        #endregion infrastructure

        #region report
        private void LoadSelectedFA()
        {
            if (SelectedFA != null)
            {
                SelectedAccountFA = null;
                SelectedFAInfoList = new ObservableCollection<FAInfo>();
                AccountFAList = new ObservableCollection<AccountFA>();
                SumFAQuantity = FullFAList.Where(f => f.AccountEquipmentName == SelectedFA.FAName).Sum(f => f.AccountEquipmentQuantity);
                var query = from fa in FullFAList
                            where fa.AccountEquipmentName == SelectedFA.FAName
                            group fa by fa.CapexSet.CapexName into ca
                            select new AccountFA { Capex = ca.Key, Sum = ca.Sum(c => c.AccountEquipmentQuantity), SumMoney = ca.Sum(c => c.AccountEquipmentQuantity * c.AccountEquipmentPrice) };
                foreach(var item in query)
                {                 
                    AccountFAList.Add(item);
                }
            }
        }
        private void LoadSelectedAccountFA()
        {
            if (SelectedAccountFA == null) return;
            SelectedFAInfoList = new ObservableCollection<FAInfo>();
            foreach(var item in FullFAList)
            {
                if (item.AccountEquipmentName == SelectedFA.FAName && item.CapexSet.CapexName == SelectedAccountFA.Capex)
                {
                    //SelectedAccountFAList.Add(item.AccountsMainSet);
                    SelectedFAInfoList.Add(new FAInfo { Company = item.AccountsMainSet.AccountCompany, DateAccount = item.AccountsMainSet.AccountDate, FAPrice = item.AccountEquipmentPrice, Store = item.AccountStoreNumber.ToString() + " " + StoreList.FirstOrDefault(s => s.StoreNumber == item.AccountStoreNumber).StoreName, Accounts = new ObservableCollection<AccountsMainSet> { item.AccountsMainSet }, Quantity = item.AccountEquipmentQuantity, FAName = item.AccountEquipmentName });
                }
            }
        }
        #endregion report

        #endregion Methods
    }
}
