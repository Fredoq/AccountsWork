using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AccountsWork.Infrastructure;
using Prism.Modularity;
using Prism.Regions;
using Syncfusion.Windows.Shared;

namespace AccountsWork
{
    /// <summary>
    /// Логика взаимодействия для Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : ChromelessWindow, IPartImportsSatisfiedNotification
    {
        private const string AccountsModuleName = "AccountsModule";
        private static Uri AccountsViewUri = new Uri("/AccountsView", UriKind.Relative);

        public Shell()
        {
            InitializeComponent();
        }

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        [Import(AllowRecomposition = false)]
        public IRegionManager RegionManager;        

        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted +=
                (s, e) =>
                {  
                    if (e.ModuleInfo.ModuleName == AccountsModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                            RegionNames.MainContentRegion,
                            AccountsViewUri);
                    }
                };
        }
    }
}
