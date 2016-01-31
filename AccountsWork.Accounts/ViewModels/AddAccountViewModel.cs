using System.ComponentModel.Composition;
using AccountsWork.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AccountsWork.DomainModel;
using AccountsWork.BusinessLayer;

namespace AccountsWork.Accounts.ViewModels
{
    [Export]
    public class AddAccountViewModel : ValidatableBindableBase
    {
        #region Private Fields
        private string _accountsTabItemHeader;
        private int _id;
        private string _accountNumber;
        private string _accountCompany;
        private DateTime? _accountDate;
        private decimal _accountAmount;
        private string _accountDescription;
        private string _accountMcdType;
        private string _accountYear;
        private string _accountType;
        private IList<AccountsCompaniesSet> _companies;
        private ICompaniesService _companiesService;
        private ITypesService _typesService;
        private IList<TypeSet> _types;
        #endregion Private Fields

        #region Public Properties
        public string AccountsTabItemHeader
        {
            get { return _accountsTabItemHeader; }
            set { SetProperty(ref _accountsTabItemHeader, value); }
        }
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
        [CustomValidation(typeof(AddAccountViewModel), "CheckDateRange")]
        public DateTime? AccountDate
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
        
        public IList<AccountsCompaniesSet> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }
        public IList<TypeSet> Types
        {
            get { return _types; }
            set { SetProperty(ref _types, value); }
        }

        #endregion Public Properties

        #region Constructors
        [ImportingConstructor]
        public AddAccountViewModel(ICompaniesService companiesService, ITypesService typesService)
        {
            AccountsTabItemHeader = "Новый счет";
            _companiesService = companiesService;
            _typesService = typesService;
            Companies = companiesService.GetCompanies();
            Types = typesService.GetTypes();

        }
        #endregion Constructors

        #region Validation Methods
        public static ValidationResult CheckDateRange(DateTime? date, ValidationContext context)
        {
            if (date.Value < new DateTime(2013, 1, 1) || date.Value > DateTime.Now)
                return new ValidationResult("Указана неверная дата счета");
            return ValidationResult.Success;
        }
        #endregion Validation Methods        
    }
}