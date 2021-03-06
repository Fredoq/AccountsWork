//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountsWork.DomainModel
{
    using Infrastructure;
    using System.ComponentModel.DataAnnotations;

    public partial class AccountsBudgetDetailsSet : ValidatableBindableBase
    {
        private int _id;
        private int _accountsMainId;
        private string _accountEquipmentName;
        private int _accountEquipmentQuantity;
        private decimal _accountEquipmentPrice;
        private int _accountStoreNumber;
        private int _equipmentCapexId;

        [Required]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        [Required]
        public int AccountsMainId
        {
            get { return _accountsMainId; }
            set { SetProperty(ref _accountsMainId, value); }
        }
        [Required]
        public string AccountEquipmentName
        {
            get { return _accountEquipmentName; }
            set { SetProperty(ref _accountEquipmentName, value); }
        }
        [Required]
        public int AccountEquipmentQuantity
        {
            get { return _accountEquipmentQuantity; }
            set { SetProperty(ref _accountEquipmentQuantity, value); }
        }
        [Required]
        public decimal AccountEquipmentPrice
        {
            get { return _accountEquipmentPrice; }
            set { SetProperty(ref _accountEquipmentPrice, value); }
        }
        [Required]
        public int AccountStoreNumber
        {
            get { return _accountStoreNumber; }
            set { SetProperty(ref _accountStoreNumber, value); }
        }
        [Required]
        public int EquipmentCapexId
        {
            get { return _equipmentCapexId; }
            set { SetProperty(ref _equipmentCapexId, value); }
        }
    
        public virtual AccountsMainSet AccountsMainSet { get; set; }
        public virtual CapexSet CapexSet { get; set; }
        public virtual FASet FASet { get; set; }
    }
}
