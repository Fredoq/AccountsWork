using System;

namespace AccountsWork.Accounts.Model
{
    public class StoreAccount
    {
        public string AccountNumber { get; set; }
        public string AccountCompany { get; set; }
        public DateTime AccountDate { get; set; }
        public decimal AccountAmount { get; set; }
        public string AccountCapex { get; set; }
        public string AccountDescription { get; set; }
        public string AccountStatus { get; set; }
        public DateTime? AccountStatusDate { get; set; }
    }
}
