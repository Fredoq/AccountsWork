using AccountsWork.Infrastructure;
using System.ComponentModel.Composition;

namespace AccountsWork.Reports.ViewModels
{
    [Export]
    public class ServiceReportForStoreViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _reportsTabItemHeader;
        #endregion Private Fields

        #region Public Properties

        #region infrastructure

        public string ReportsTabItemHeader
        {
            get { return _reportsTabItemHeader; }
            set { SetProperty(ref _reportsTabItemHeader, value); }
        }

        #endregion infrastructure

        #endregion Public Properties

        #region Constructor
        [ImportingConstructor]
        public ServiceReportForStoreViewModel()
        {
            #region infrastructure
            ReportsTabItemHeader = "Детальный отчет по ресторанам";
            #endregion infrastructure
        }
        #endregion Constructor
    }
}
