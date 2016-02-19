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
using AccountsWork.Accounts.ViewModels;

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Логика взаимодействия для AccountsView.xaml
    /// </summary>
    [Export("AccountsView")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AccountsView : UserControl
    {
        #region Constructor
        public AccountsView()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Public Properties
        [Import]
        public AccountsViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties
    }
}
