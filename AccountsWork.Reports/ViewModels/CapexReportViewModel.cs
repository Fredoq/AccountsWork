using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using AccountsWork.Reports.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Prism.Regions;
using AccountsVork.Infrastructure;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class CapexReportViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private List<int> _yearList;
        private int _selectedYear;
        private ObservableCollection<CapexWithRest> _capexList;
        private ICapexesService _capexService;
        private BackgroundWorker _worker;
        private IList<CapexSet> _fullCapexList;
        private bool _isCapexBusy;
        private ObservableCollection<AccountsWithStatus> _capexAccountsList;
        private IAccountStatusService _statusService;
        private CapexWithRest _selectedCapex;
        private IAccountsMainService _accountsMainService;
        private IList<AccountsMainSet> _accountsList;
        private ObservableCollection<StatusSum> _statusSumList;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string ReportsTabItemHeader
        {
            get { return _reportsTabItemHeader; }
            set { SetProperty(ref _reportsTabItemHeader, value); }
        }
        public IList<CapexSet> FullCapexList
        {
            get { return _fullCapexList; }
            set { SetProperty(ref _fullCapexList, value); }
        }  
        public IList<AccountsMainSet> AccountsList
        {
            get { return _accountsList; }
            set { SetProperty(ref _accountsList, value); }
        }     
        public bool IsCapexBusy
        {
            get { return _isCapexBusy; }
            set { SetProperty(ref _isCapexBusy, value); }
        }
        #endregion infrastructure

        #region capex

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
        public ObservableCollection<CapexWithRest> CapexList
        {
            get { return _capexList; }
            set { SetProperty(ref _capexList, value); }
        }
        public CapexWithRest SelectedCapex
        {
            get { return _selectedCapex; }
            set { SetProperty(ref _selectedCapex, value); }
        }
        public ObservableCollection<AccountsWithStatus> CapexAccountsList
        {
            get { return _capexAccountsList; }
            set { SetProperty(ref _capexAccountsList, value); }
        }
        public ObservableCollection<StatusSum> StatusSumList
        {
            get { return _statusSumList; }
            set { SetProperty(ref _statusSumList, value); }
        }
        #endregion capex

        #endregion Public Properties

        #region Commands

        #region capex
        public DelegateCommand LoadCapexCommand { get; set; }
        public DelegateCommand LoadSelectedCapexCommand { get; set; }
        #endregion capex

        #endregion Commands

        [ImportingConstructor]
        public CapexReportViewModel(ICapexesService capexService, IAccountStatusService statusService, IAccountsMainService accountsMainService)
        {
            #region infrastructure
            ReportsTabItemHeader = "Отчет по CAPEX";
            #endregion infrastructure

            #region capex
            YearList = new List<int>();
            for (var i = 2015; i <= DateTime.Now.Year; i++)
                YearList.Add(i);
            CapexList = new ObservableCollection<CapexWithRest>();
            CapexAccountsList = new ObservableCollection<AccountsWithStatus>();
            LoadCapexCommand = new DelegateCommand(LoadCapex);
            LoadSelectedCapexCommand = new DelegateCommand(LoadSelectedCapex);
            StatusSumList = new ObservableCollection<StatusSum>();
            #endregion capex

            #region services
            _capexService = capexService;
            _statusService = statusService;
            _accountsMainService = accountsMainService;
            #endregion services

            #region workers
            _worker = new BackgroundWorker();
            _worker.DoWork += LoadFullCapexInfo;
            _worker.RunWorkerCompleted += LoadFullCapexInfo_Completed;
            #endregion workers
        }

        





        #region Methods

        #region infrastructure
        private void LoadFullCapexInfo(object sender, DoWorkEventArgs e)
        {
            IsCapexBusy = true;
            FullCapexList = _capexService.GetCapexes();
            AccountsList = _accountsMainService.GetAccountsWithCapexesAndStatus();       
        }
        private void LoadFullCapexInfo_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            IsCapexBusy = false;
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _worker.RunWorkerAsync();
        }
        #endregion infrastructure

        #region capex
        private void LoadCapex()
        {
            if (SelectedYear != 0)
            {
                CapexList.Clear();
                CapexAccountsList.Clear();
                StatusSumList.Clear();
                foreach (var capex in FullCapexList.Where(f => f.CapexYear == SelectedYear))
                {
                    var capexWithRest = new CapexWithRest();
                    capexWithRest.Capex = capex;
                    var sum = AccountsList.Where(a => a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus != Statuses.InCancel).Sum(a => a.AccountsCapexInfoSets.Where(c => c.CapexId == capex.Id).Sum(c => c.AccountCapexAmount));                    
                    if (capex.CapexAmount == 0)
                    {                        
                        capexWithRest.Rest = sum;
                    }
                    else
                    {
                        capexWithRest.Rest = capex.CapexAmount - sum;
                    }
                    CapexList.Add(capexWithRest);             
                }
            }
        }
        private void LoadSelectedCapex()
        {
            if (SelectedCapex != null)
            {
                CapexAccountsList.Clear();
                StatusSumList.Clear();
                var query = from a in AccountsList
                            where a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus != Statuses.InCancel &&
                                  a.AccountsCapexInfoSets.Any(c => c.CapexId == SelectedCapex.Capex.Id)
                            select new AccountsWithStatus { Account = a, Status = a.AccountsStatusDetailsSets.LastOrDefault() };
                var groups = from a in AccountsList
                             where a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus != Statuses.InCancel &&
                                   a.AccountsCapexInfoSets.Any(c => c.CapexId == SelectedCapex.Capex.Id)
                             group a by a.AccountsStatusDetailsSets.LastOrDefault().AccountStatus into status
                             select new StatusSum { Status = status.Key, Sum = status.Sum(s => s.AccountsCapexInfoSets.Where(c => c.CapexId == SelectedCapex.Capex.Id).Sum(c => c.AccountCapexAmount)) };
                foreach (var item in query)
                    CapexAccountsList.Add(item);
                foreach (var item in groups)
                    StatusSumList.Add(item);
                
            }
        }
        #endregion capex

        #endregion Methods
    }
}
