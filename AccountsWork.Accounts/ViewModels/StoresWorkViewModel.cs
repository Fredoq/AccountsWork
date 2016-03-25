using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Prism.Regions;
using System;
using AccountsWork.BusinessLayer;
using System.Linq;
using Prism.Commands;

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
        private ObservableCollection<StoreProvenWorkSet> _changedWorkList;
        private StoreProvenWorkSet _newWork;
        private IStoresService _storeService;
        private ObservableCollection<StoresSet> _storesList;
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
            set
            {
                if (_selectedWork != null)
                    SelectedWork.PropertyChanged -= SelectedWorkChanged;
                SetProperty(ref _selectedWork, value);
                if (_selectedWork != null)
                    SelectedWork.PropertyChanged += SelectedWorkChanged;
            }
        }     

        public ObservableCollection<StoreProvenWorkSet> ChangedWorkList
        {
            get { return _changedWorkList; }
            set { SetProperty(ref _changedWorkList, value); }
        }
        public StoreProvenWorkSet NewWork
        {
            get { return _newWork; }
            set
            {
                if (_newWork != null)
                    NewWork.PropertyChanged -= NewWorkChanged;
                SetProperty(ref _newWork, value);
                if (_newWork != null)
                    NewWork.PropertyChanged += NewWorkChanged;
            }
        }       

        public ObservableCollection<StoresSet> StoresList
        {
            get { return _storesList; }
            set { SetProperty(ref _storesList, value); }
        }
        #endregion work

        #endregion Public Properties

        #region Commands

        #region work
        public DelegateCommand SaveChangesCommand { get; set; }
        public DelegateCommand AddNewWorkCommand { get; set; }
        #endregion work

        #endregion Commands

        #region Constructor
        [ImportingConstructor]
        public StoresWorkViewModel(IStoresWorkService storesWorkService, IStoresService storeService)
        {
            #region infrastructure
            AccountsTabItemHeader = "Работы в ресторанах";
            #endregion infrastructure

            #region services
            _storesWorkService = storesWorkService;
            _storeService = storeService;
            #endregion services

            #region work
            StoresWorkList = new ObservableCollection<StoreProvenWorkSet>();
            ChangedWorkList = new ObservableCollection<StoreProvenWorkSet>();
            SaveChangesCommand = new DelegateCommand(SaveChanges, CanSave);
            AddNewWorkCommand = new DelegateCommand(AddNewWork, CanAdd);
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
            NewWork = new StoreProvenWorkSet();
            SaveChangesCommand.RaiseCanExecuteChanged();
            _worker.RunWorkerAsync();
        }

        #endregion infrastructure

        #region work

        private void LoadStoresWork(object sender, DoWorkEventArgs e)
        {
            IsStoreWorkBusy = true;
            if (StoresList == null)
            {
                StoresList = new ObservableCollection<StoresSet>(_storeService.GetStores());
            }
            StoresWorkList = new ObservableCollection<StoreProvenWorkSet>(_storesWorkService.GetWorksList());
        }
        private void LoadStoresWork_Comleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsStoreWorkBusy = false;
        }
        private void SelectedWorkChanged(object sender, PropertyChangedEventArgs e)
        {
            var work = sender as StoreProvenWorkSet;
            if (work != null)
            {
                if (!ChangedWorkList.Contains(work))
                    ChangedWorkList.Add(work);
                else
                    ChangedWorkList.Remove(work);
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        private void NewWorkChanged(object sender, PropertyChangedEventArgs e)
        {
            AddNewWorkCommand.RaiseCanExecuteChanged();
        }
        private void SaveChanges()
        {
            if (ChangedWorkList.Count > 0)
                _storesWorkService.UpdateWorks(ChangedWorkList);
            ChangedWorkList.Clear();
        }
        private bool CanSave()
        {
            return ChangedWorkList.Count > 0;
        }
        private void AddNewWork()
        {
            if (NewWork != null)
            {
                _storesWorkService.AddNewWork(NewWork);
                _worker.RunWorkerAsync();
            }
        }
        private bool CanAdd()
        {
            if (NewWork != null)
            {
                NewWork.ValidateProperties();
                return !NewWork.HasErrors;
            }
            return false;
        }
        #endregion work

        #endregion Methods


    }
}
