using System.ComponentModel.Composition.Hosting;
using System.Windows;
using AccountsWork.BusinessLayer;
using AccountsWork.DataAccessLayer;
using Prism.Mef;
using Prism.Modularity;
using AccountsWork.ExcelReports;
using AccountsWork.Accounts;
using AccountsWork.Reports;

namespace AccountsWork
{
    public class Bootstrapper : MefBootstrapper
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(GenericDataRepository<>).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountsMainService).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ExcelReportService).Assembly));
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
        protected override DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<Shell>();
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }
    }
}