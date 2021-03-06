﻿using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using AccountsWork.Infrastructure;
using Prism.Regions;

namespace AccountsWork.Accounts.Views
{
    /// <summary>
    /// Логика взаимодействия для AccountsNavigationView.xaml
    /// </summary>
    [Export]
    [ViewSortHint("01")]
    public partial class AccountsNavigationView : UserControl, IPartImportsSatisfiedNotification
    {
        #region Private Fields
        private static Uri accountsViewUri = new Uri("/AccountsView", UriKind.Relative);
        #endregion Private Fields

        #region Public Fields
        [Import]
        public IRegionManager regionManager;
        #endregion Public Fields

        #region Constructor
        public AccountsNavigationView()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Methods
        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this.regionManager.Regions[RegionNames.MainContentRegion];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }
        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {
            this.UpdateNavigationButtonState(e.Uri);
        }
        private void UpdateNavigationButtonState(Uri uri)
        {
            this.NavigateToAccountsRadioButton.IsChecked = (uri == accountsViewUri);
        }
        private void NavigateToAccountsRadioButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, accountsViewUri);
        }
        #endregion Methods
    }
}
