using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Prism.Regions;
using Syncfusion.UI.Xaml.Grid;


namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountsViewModel : ValidatableBindableBase
    {
        private int _id;
        private string _accountNumber;
        private string _accountCompany;
        private DateTime _accountDate;
        private decimal _accountAmount;
        private string _accountDescription;
        private string _accountMcdType;
        private string _accountYear;
        private string _accountType;
        private IAccountsMainService _accountsMainService;
        private IRegionManager _regionManager;

        #region Public Properties
        [Required]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [Required]
        public string AccountNumber
        {
            get { return _accountNumber; }
            set { SetProperty(ref _accountNumber, value); }
        }
        [Required]
        public string AccountCompany
        {
            get { return _accountCompany; }
            set { SetProperty(ref _accountCompany, value); }
        }
        [CustomValidation(typeof(AccountsViewModel), "CheckDateRange")]
        public DateTime AccountDate
        {
            get { return _accountDate; }
            set { SetProperty(ref _accountDate, value); }
        }
        [Required]
        public decimal AccountAmount
        {
            get { return _accountAmount; }
            set { SetProperty(ref _accountAmount, value); }
        }
        public string AccountDescription
        {
            get { return _accountDescription; }
            set { SetProperty(ref _accountDescription, value); }
        }
        [Required]
        public string AccountMcdType
        {
            get { return _accountMcdType; }
            set { SetProperty(ref _accountMcdType, value); }
        }
        [Required]
        public string AccountYear
        {
            get { return _accountYear; }
            set { SetProperty(ref _accountYear, value); }
        }
        [Required]
        public string AccountType
        {
            get { return _accountType; }
            set { SetProperty(ref _accountType, value); }
        }
        #endregion Public Properties

        #region Commands
        public DelegateCommand<string> NavigateCommand { get; set; } 
        #endregion Commands

        #region Methods

        void Navigate(string navigationProperty)
        {
            _regionManager.RequestNavigate("AccountsTabRegion", navigationProperty);
        }
        #endregion Methods

        #region Validation Methods
        public static ValidationResult CheckDateRange(DateTime date, ValidationContext context)
        {
            if (date < new DateTime(2013, 1, 1) || date > DateTime.Now)
                return new ValidationResult("Указана неверная дата счета");
            return ValidationResult.Success;
        }
        #endregion Validation Methods

        #region Constructor
        [ImportingConstructor]
        public AccountsViewModel(IAccountsMainService accountsMainService, IRegionManager regionManager)
        //public AccountsViewModel()
        {
            _accountsMainService = accountsMainService;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }
        #endregion Constructor
    }
}