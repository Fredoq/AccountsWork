using AccountsWork.Accounts.ViewModels;
using Syncfusion.UI.Xaml.Grid;
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
    /// Interaction logic for AddAdditionalInfoView.xaml
    /// </summary>
    [Export("AdditionalInfoView")]
    public partial class AddAdditionalInfoView : UserControl
    {
        #region Private Fields
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        double Height = double.NaN;
        #endregion Private Fields

        #region Constructor
        public AddAdditionalInfoView()
        {
            InitializeComponent();
            statusHistoryGrid.QueryRowHeight += statusHistoryGrid_QueryRowHeight;
        }        
        #endregion Constructor

        #region Public Properties
        [Import]
        public AdditionalInfoViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties

        #region Methods
        private void statusHistoryGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (statusHistoryGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out Height))
            {
                e.Height = Height;
                e.Handled = true;
            }
        }
        #endregion Methods
    }
}
