using System.ComponentModel.Composition.Hosting;
using System.Windows;
using AccountsWork.BusinessLayer;
using AccountsWork.DataAccessLayer;
using Prism.Mef;
using Prism.Modularity;
using Prism.Mvvm;
using System.Reflection;
using System;
using System.Globalization;
using AccountsWork.ExcelReports;

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
        //protected override void ConfigureViewModelLocator()
        //{
        //    base.ConfigureViewModelLocator();

        //    ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
        //    {
        //        var viewName = viewType.FullName;
        //        viewName = viewName.Replace(".Views.", ".ViewModels.");
        //        var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
        //        var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
        //        var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}", viewName, suffix);

        //        var assembly = viewType.GetTypeInfo().Assembly;
        //        var type = assembly.GetType(viewModelName, true);

        //        return type;
        //    });
        //}
    }
}