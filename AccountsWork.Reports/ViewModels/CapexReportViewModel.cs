using AccountsWork.Infrastructure;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class CapexReportViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        private List<int> _yearList;
        private int _selectedYear;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure
        public string ReportsTabItemHeader
        {
            get { return _reportsTabItemHeader; }
            set { SetProperty(ref _reportsTabItemHeader, value); }
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
        #endregion capex

        #endregion Public Properties

        #region Commands

        #region capex
        public DelegateCommand LoadCapexCommand { get; set; }
        #endregion capex

        #endregion Commands

        [ImportingConstructor]
        public CapexReportViewModel()
        {
            #region infrastructure
            ReportsTabItemHeader = "Отчет по CAPEX";
            #endregion infrastructure

            #region capex
            YearList = new List<int>();
            for (var i = 2015; i <= DateTime.Now.Year; i++)
                YearList.Add(i);

            LoadCapexCommand = new DelegateCommand(LoadCapex);
            #endregion capex
        }

        #region Methods

        #region capex
        private void LoadCapex()
        {
            
        }
        #endregion capex

        #endregion Methods
    }
}
