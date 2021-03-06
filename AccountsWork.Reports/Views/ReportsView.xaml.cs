﻿using AccountsWork.Reports.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace AccountsWork.Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    [Export("ReportsView")]
    public partial class ReportsView : UserControl
    {
        #region Public Properties
        [Import]
        public ReportsViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
        #endregion Public Properties

        #region Constructor

        public ReportsView()
        {
            InitializeComponent();
        }

        #endregion Constructor
    }
}
