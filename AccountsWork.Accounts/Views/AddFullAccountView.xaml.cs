using AccountsWork.Accounts.ViewModels;
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

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Interaction logic for AddFullAccountView.xaml
    /// </summary>
    [Export("AddFullAccountView")]
    public partial class AddFullAccountView : UserControl
    {
        public AddFullAccountView()
        {
            InitializeComponent();
        }

        #region Public Properties
        [Import]
        public AddAccountViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties
    }
}
